using System.ComponentModel.DataAnnotations.Schema;

namespace Kokoa.Trivia.Api.Models;

[Table("T_TRIVIA_ANSWER")]
public class TriviaAnswer
{
    [Column("TRIVIA_QUESTION_ID")]
    public int TriviaQuestionId { get; set; }

    [Column("TRIVIA_OPTION_ID")]
    public int TriviaOptionId { get; set; }

    public TriviaQuestion TriviaQuestion { get; set; } = null!;
    public TriviaOption TriviaOption { get; set; } = null!;
}