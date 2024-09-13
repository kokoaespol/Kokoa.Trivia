using FastEndpoints;
using Kokoa.Trivia.Api.Dto;
using Kokoa.Trivia.Api.Mappers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using ErrorOr;
using Throw;
using Created = Microsoft.AspNetCore.Http.HttpResults.Created;

namespace Kokoa.Trivia.Api.Endpoints;

[AllowAnonymous]
[HttpPost("/api/questions")]
public class CreateQuestionEndpoint: Endpoint<NewTriviaQuestionDto, Results<Created, BadRequest<string>>>
{
    private readonly IMediator _mediator;
    private readonly TriviaQuestionMapper _mapper;

    public CreateQuestionEndpoint(IMediator mediator, TriviaQuestionMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    public override async Task<Results<Created, BadRequest<string>>> ExecuteAsync(
        NewTriviaQuestionDto req,
        CancellationToken ct)
    {
        var command = _mapper.NewTriviaQuestionDtoToCreateTriviaQuestionCommand(req);
        var result = await _mediator.Send(command, ct);

        if (result is { IsError: true, FirstError.Type: ErrorType.Validation })
            return TypedResults.BadRequest(result.FirstError.Description);

        result.IsError.Throw().IfTrue();

        return TypedResults.Created("/api/v1/questions/" + result.Value.Id);
    }
}