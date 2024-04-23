using System.Text.Json.Serialization;

namespace Shared;

public class SubCategoryModel
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("text")]
    public string Text { get; set; } = "";
    
    [JsonPropertyName("questions")]
    public List<QuestionModel> questions { get; set; } = [];
}
