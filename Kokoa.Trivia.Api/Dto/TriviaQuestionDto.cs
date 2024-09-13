using System.Text.Json.Serialization;

namespace Kokoa.Trivia.Api.Dto;

public class TriviaQuestionDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("correct_option")]
    public TriviaOptionDto CorrectOption { get; set; } = null!;

    [JsonPropertyName("options")]
    public IEnumerable<TriviaOptionDto> Options { get; set; } = null!;
}