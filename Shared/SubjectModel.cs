using System.Text.Json.Serialization;

namespace Shared;

public class SubjectModel
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("text")]
    public string Text { get; set; } = "";
    
    [JsonPropertyName("questions")]
    public List<QuestionModel> Questions { get; set; } = [];
}
