using EntityFramework.Exceptions.Common;
using ErrorOr;
using Kokoa.Trivia.Api.Database;
using Kokoa.Trivia.Api.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Kokoa.Trivia.Api.Commands;

public class CreateTriviaQuestionCommand(string title, List<string> options, int correctOption)
    : IRequest<ErrorOr<TriviaQuestion>>
{
    public string Title { get; set; } = title;
    public List<string> Options { get; set; } = options;
    public int CorrectOption { get; set; } = correctOption;

    public class Handler(TriviaDbContext db, ILogger<Handler> logger)
        : IRequestHandler<CreateTriviaQuestionCommand, ErrorOr<TriviaQuestion>>
    {
        public async Task<ErrorOr<TriviaQuestion>> Handle(
            CreateTriviaQuestionCommand command,
            CancellationToken cancellationToken)
        {
            await using var trx = await db.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                if (command.Title.Length == 0)
                    return Error.Validation(description: "The question title must not be empty");

                if (command.Options.Any(x => x.Length == 0))
                    return Error.Validation(description: "The question options must not be empty");

                if (command.Options.Count <= 0)
                    return Error.Validation(description: "Question must have at least one option");

                var correctOptionIndex = command.CorrectOption;
                if (correctOptionIndex < 0 || correctOptionIndex >= command.Options.Count)
                    return Error.Validation(description: "Correct option index is not valid");

                var question = new TriviaQuestion { Title = command.Title };
                var questionResult = db.TriviaQuestions.Add(question);
                await db.SaveChangesAsync(cancellationToken);

                var options = command.Options.Select(x => new TriviaOption
                {
                    TriviaQuestionId = questionResult.Entity.Id,
                    Content = x
                })
                .ToList();

                db.TriviaOptions.AddRange(options);
                await db.SaveChangesAsync(cancellationToken);

                var answer = new TriviaAnswer
                {
                    TriviaQuestionId = questionResult.Entity.Id,
                    TriviaOptionId = options[command.CorrectOption].Id
                };
                await db.TriviaAnswers.AddAsync(answer, cancellationToken);
                await db.SaveChangesAsync(cancellationToken);

                await trx.CommitAsync(cancellationToken);

                return questionResult.Entity;
            }
            catch (UniqueConstraintException ex)
            {
                logger.LogError("Unique constraint violation: {Exception}", ex);
                await trx.RollbackAsync(cancellationToken);
                return Error.Validation(description: "A question with that title already exists");
            }
            catch (Exception ex)
            {
                logger.LogError("Failed to create trivia question: {Exception}", ex);
                await trx.RollbackAsync(cancellationToken);
                return Error.Unexpected(description: "Failed to create the trivia question");
            }
        }
    }
}
