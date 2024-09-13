using System.ComponentModel.DataAnnotations.Schema;

namespace Kokoa.Trivia.Api.Models;

[Table("T_TRIVIA_OPTION")]
public class TriviaOption
{
    [Column("TRIVIA_OPTION_ID")]
    public int Id { get; set; }

    [Column("content")]
    public string Content { get; set; } = null!;

    [Column("TRIVIA_QUESTION_ID")]
    public int TriviaQuestionId { get; set; }

    public TriviaQuestion TriviaQuestion { get; set; } = null!;
}