using System.Text.Json.Serialization;

namespace Kokoa.Trivia.Api.Dto;

public class TriviaOptionDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("content")]
    public required string Content { get; set; }
}