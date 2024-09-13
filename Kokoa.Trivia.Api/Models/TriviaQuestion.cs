using System.ComponentModel.DataAnnotations.Schema;

namespace Kokoa.Trivia.Api.Models;

[Table("T_TRIVIA_QUESTION")]
public class TriviaQuestion
{
    [Column("TRIVIA_QUESTION_ID")]
    public int Id { get; set; }

    [Column("title")]
    public string Title { get; set; } = null!;

    public ICollection<TriviaOption> TriviaOptions { get; set; } = new List<TriviaOption>();
}