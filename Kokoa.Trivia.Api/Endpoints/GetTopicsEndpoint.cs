using FastEndpoints;
using Kokoa.Trivia.Api.Dto;
using Kokoa.Trivia.Api.Mappers;
using Kokoa.Trivia.Api.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace Kokoa.Trivia.Api.Endpoints;

[AllowAnonymous]
[HttpGet("/api/topics")]
public class GetTopicsEndpoint : EndpointWithoutRequest<IEnumerable<TriviaTopicDto>>
{
    public IMediator Mediator { get; set; } = null!;
    public TriviaQuestionMapper Mapper { get; set; } = null!;

    public override async Task<IEnumerable<TriviaTopicDto>> ExecuteAsync(CancellationToken ct)
    {
        var result = await Mediator.Send(new GetTopicsQuery(), ct);
        return result.Select(x => Mapper.TriviaTopicToTriviaTopicDto(x));
    }
}