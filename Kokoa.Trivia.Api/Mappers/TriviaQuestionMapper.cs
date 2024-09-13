using Kokoa.Trivia.Api.Commands;
using Kokoa.Trivia.Api.Dto;
using Riok.Mapperly.Abstractions;

namespace Kokoa.Trivia.Api.Mappers;

[Mapper]
public partial class TriviaQuestionMapper
{
    public partial CreateTriviaQuestionCommand NewTriviaQuestionDtoToCreateTriviaQuestionCommand(
        NewTriviaQuestionDto dto);
}