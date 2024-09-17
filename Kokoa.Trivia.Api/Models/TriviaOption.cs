using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kokoa.Trivia.Api.Models;

public class TriviaOption
{
    public int Id { get; set; }
    [MaxLength(255)]
    public string Content { get; set; } = null!;
    public int TriviaQuestionId { get; set; }
    public TriviaQuestion TriviaQuestion { get; set; } = null!;
}
