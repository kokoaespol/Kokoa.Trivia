using System.Text.Json.Serialization;

namespace Kokoa.Trivia.Api.Dto;

public class NewTriviaTopicDto
{
    [JsonPropertyName("name")]
    public required string Name { get; set; }
}
