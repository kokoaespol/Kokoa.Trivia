using FastEndpoints;
using Kokoa.Trivia.Api.Dto;
using Kokoa.Trivia.Api.Mappers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using ErrorOr;
using Kokoa.Trivia.Api.Commands;
using Throw;
using Created = Microsoft.AspNetCore.Http.HttpResults.Created;

namespace Kokoa.Trivia.Api.Endpoints;

[AllowAnonymous]
[HttpPost("/api/topics/{TopicId:int}/questions")]
public class CreateQuestionEndpoint
    : Endpoint<NewTriviaQuestionDto, Results<Created<TriviaQuestionDto>, BadRequest<string>>>
{
    private readonly IMediator _mediator;
    private readonly TriviaQuestionMapper _mapper;

    public CreateQuestionEndpoint(IMediator mediator, TriviaQuestionMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    public override async Task<Results<Created<TriviaQuestionDto>, BadRequest<string>>> ExecuteAsync(
        NewTriviaQuestionDto req,
        CancellationToken ct)
    {
        var command = new CreateTriviaQuestionCommand(
            req.TopicId,
            req.Title,
            req.Options.ToList(),
            req.CorrectOption);

        var result = await _mediator.Send(command, ct);

        if (result is { IsError: true, FirstError.Type: ErrorType.Validation })
            return TypedResults.BadRequest(result.FirstError.Description);

        result.IsError.Throw().IfTrue();

        var response = new TriviaQuestionDto
        {
            Id = result.Value.Id,
            Title = result.Value.Title,
            Options = result.Value.TriviaOptions.Select(x => _mapper.TriviaOptionToTriviaOptionDto(x)),
            CorrectOption = _mapper.TriviaOptionToTriviaOptionDto(result.Value.TriviaAnswer.TriviaOption)
        };

        return TypedResults.Created("/api/v1/questions/" + result.Value.Id, response);
    }
}