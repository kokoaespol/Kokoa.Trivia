using ErrorOr;
using Kokoa.Trivia.Api.Database;
using Kokoa.Trivia.Api.Models;
using MediatR;

namespace Kokoa.Trivia.Api.Commands;

public class CreateTriviaTopicCommand(string name) : IRequest<ErrorOr<TriviaTopic>>
{
    public string Name { get; set; } = name;

    public class Handler(TriviaDbContext db) : IRequestHandler<CreateTriviaTopicCommand, ErrorOr<TriviaTopic>>
    {
        public async Task<ErrorOr<TriviaTopic>> Handle(
            CreateTriviaTopicCommand command,
            CancellationToken cancellationToken)
        {
            var entity = new TriviaTopic { Name = command.Name };
            var result = db.TriviaTopics.Add(entity);
            await db.SaveChangesAsync(cancellationToken);
            return result.Entity;
        }
    }
}