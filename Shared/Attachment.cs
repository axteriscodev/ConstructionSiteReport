using System.Text.Json.Serialization;

namespace Shared;

public class Attachment
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = "";

    [JsonPropertyName("date")]
    public DateTime Date { get; set; }

    [JsonPropertyName("question")]
    public QuestionModel question{ get; set; }= new QuestionModel();

    [JsonPropertyName("path")]
    public string Path { get; set; } = "";
}
