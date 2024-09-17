using System.ComponentModel.DataAnnotations.Schema;

namespace Kokoa.Trivia.Api.Models;

public class TriviaAnswer
{
    public int TriviaQuestionId { get; set; }
    public int TriviaOptionId { get; set; }
    public TriviaQuestion TriviaQuestion { get; set; } = null!;
    public TriviaOption TriviaOption { get; set; } = null!;
}
