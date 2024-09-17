using Kokoa.Trivia.Api.Commands;
using Kokoa.Trivia.Api.Dto;
using Kokoa.Trivia.Api.Models;
using Riok.Mapperly.Abstractions;

namespace Kokoa.Trivia.Api.Mappers;

[Mapper]
public partial class TriviaQuestionMapper
{
    public partial CreateTriviaQuestionCommand NewTriviaQuestionDtoToCreateTriviaQuestionCommand(
        NewTriviaQuestionDto dto);

    public partial TriviaOptionDto TriviaOptionToTriviaOptionDto(TriviaOption option);
}
