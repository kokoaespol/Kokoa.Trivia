using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kokoa.Trivia.Api.Models;

public class TriviaQuestion
{
    public int Id { get; set; }
    [MaxLength(255)]
    public string Title { get; set; } = null!;
    public TriviaAnswer TriviaAnswer { get; set; } = null!;
    public ICollection<TriviaOption> TriviaOptions { get; set; } = new List<TriviaOption>();
}
