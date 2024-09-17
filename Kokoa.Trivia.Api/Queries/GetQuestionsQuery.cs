using Kokoa.Trivia.Api.Database;
using Kokoa.Trivia.Api.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Kokoa.Trivia.Api.Queries;

public class GetQuestionsQuery(int topicId) : IRequest<IEnumerable<TriviaQuestionAnswer>>
{
    public int TopicId { get; set; } = topicId;

    public class Handler(TriviaDbContext db) : IRequestHandler<GetQuestionsQuery, IEnumerable<TriviaQuestionAnswer>>
    {
        private readonly TriviaDbContext _db = db;

        public async Task<IEnumerable<TriviaQuestionAnswer>> Handle(
            GetQuestionsQuery query,
            CancellationToken cancellationToken)
        {
            return await _db.TriviaQuestions
                .Include(x => x.TriviaOptions)
                .Include(x => x.TriviaAnswer)
                .ThenInclude(x => x.TriviaOption)
                .Where(x => x.TriviaTopicId == query.TopicId)
                .Select(x => new TriviaQuestionAnswer
                {
                    Id = x.Id,
                    Title = x.Title,
                    Options = x.TriviaOptions,
                    CorrectOption = x.TriviaAnswer.TriviaOption
                })
                .ToListAsync(cancellationToken);
        }
    }
}