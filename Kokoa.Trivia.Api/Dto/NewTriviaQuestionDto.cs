using System.Text.Json.Serialization;

namespace Kokoa.Trivia.Api.Dto;

public class NewTriviaQuestionDto
{
    public int TopicId { get; set; } 
    
    [JsonPropertyName("title")]
    public required string Title { get; set; }

    [JsonPropertyName("correct_option")]
    public required int CorrectOption { get; set; }

    [JsonPropertyName("options")]
    public required IEnumerable<string> Options { get; set; }
}