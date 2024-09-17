using FastEndpoints;
using Kokoa.Trivia.Api.Dto;
using Kokoa.Trivia.Api.Mappers;
using Kokoa.Trivia.Api.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace Kokoa.Trivia.Api.Endpoints;

[AllowAnonymous]
[HttpGet("/api/topics/{TopicId:int}/questions")]
public class GetQuestionsEndpoint : Endpoint<GetQuestionsDto, IEnumerable<TriviaQuestionDto>>
{
    public IMediator Mediator { get; set; } = null!;
    public TriviaQuestionMapper Mapper { get; set; } = null!;

    public override async Task<IEnumerable<TriviaQuestionDto>> ExecuteAsync(
        GetQuestionsDto req, 
        CancellationToken ct)
    {
        var query = new GetQuestionsQuery(req.TopicId);
        var questions = await Mediator.Send(query, ct);
        return questions.Select(x => Mapper.TriviaQuestionAnswerToTriviaQuestionDto(x));
    }
}
