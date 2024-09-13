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
                if (command.Options.Count <= 0)
                    return Error.Validation(description: "Question must have at least one option");

                var correctOptionIndex = command.CorrectOption;
                if (correctOptionIndex < 0 || correctOptionIndex >= command.Options.Count)
                    return Error.Validation(description: "Correct option index is not valid");

                var correctOptionText = command.Options[correctOptionIndex];

                var question = new TriviaQuestion { Title = command.Title };
                var questionResult = await db.TriviaQuestions.AddAsync(question, cancellationToken);
                await db.SaveChangesAsync(cancellationToken);

                var options = command.Options.Select(x => new TriviaOption
                {
                    TriviaQuestionId = questionResult.Entity.Id,
                    Content = x
                });
                await db.TriviaOptions.AddRangeAsync(options, cancellationToken);
                await db.SaveChangesAsync(cancellationToken);

                var correctOption = db.TriviaOptions.FirstAsync(
                    x => x.Content == correctOptionText,
                    cancellationToken);

                var answer = new TriviaAnswer
                {
                    TriviaQuestionId = questionResult.Entity.Id,
                    TriviaOptionId = correctOption.Id
                };
                await db.TriviaAnswers.AddAsync(answer, cancellationToken);
                await db.SaveChangesAsync(cancellationToken);

                await trx.CommitAsync(cancellationToken);

                return questionResult.Entity;
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