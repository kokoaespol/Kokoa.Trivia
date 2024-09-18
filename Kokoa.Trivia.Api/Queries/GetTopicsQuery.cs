using Kokoa.Trivia.Api.Database;
using Kokoa.Trivia.Api.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Kokoa.Trivia.Api.Queries;

public class GetTopicsQuery: IRequest<IEnumerable<TriviaTopic>>
{
   public class Handler(TriviaDbContext db) : IRequestHandler<GetTopicsQuery, IEnumerable<TriviaTopic>>
   {
      public async Task<IEnumerable<TriviaTopic>> Handle(
         GetTopicsQuery query, 
         CancellationToken cancellationToken)
      {
         return await db.TriviaTopics.ToListAsync(cancellationToken);
      }
   }
}