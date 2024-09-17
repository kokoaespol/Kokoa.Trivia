namespace Kokoa.Trivia.Api.Models;

public class TriviaTopic
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;

    public ICollection<TriviaQuestion> TriviaQuestions { get; set; } = [];
}
