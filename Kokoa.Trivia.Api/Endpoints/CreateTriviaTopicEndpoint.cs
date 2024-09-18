using FastEndpoints;
using Kokoa.Trivia.Api.Commands;
using Kokoa.Trivia.Api.Dto;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Throw;

namespace Kokoa.Trivia.Api.Endpoints;

[AllowAnonymous]
[HttpPost("/api/topics")]
public class CreateTriviaTopicEndpoint : Endpoint<NewTriviaTopicDto, Results<Created<TriviaTopicDto>, BadRequest>>
{
    public IMediator Mediator { get; set; } = null!;

    public override async Task<Results<Created<TriviaTopicDto>, BadRequest>> ExecuteAsync(
        NewTriviaTopicDto req,
        CancellationToken ct)
    {
        var command = new CreateTriviaTopicCommand(req.Name);
        var result = await Mediator.Send(command, ct);
        result.IsError.Throw().IfTrue();
        return TypedResults.Created(
            $"/api/topics/{result.Value.Id}",
            new TriviaTopicDto
            {
                Id = result.Value.Id,
                Name = result.Value.Name,
            }
        );
    }
}