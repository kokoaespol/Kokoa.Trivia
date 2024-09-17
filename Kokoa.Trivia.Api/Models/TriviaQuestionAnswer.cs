namespace Kokoa.Trivia.Api.Models;

public class TriviaQuestionAnswer
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public TriviaOption CorrectOption { get; set; } = null!;
    public IEnumerable<TriviaOption> Options { get; set; } = new List<TriviaOption>();
}
