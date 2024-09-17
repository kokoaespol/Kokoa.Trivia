using System.Text.Json.Serialization;

namespace Kokoa.Trivia.Api.Dto;


public class TriviaTopicDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;
}
