using System.Text.Json.Serialization;

namespace Kokoa.Trivia.Api.Dto;

public class GetQuestionsDto
{
    [JsonPropertyName("topic_id")] public int TopicId { get; set; }
}