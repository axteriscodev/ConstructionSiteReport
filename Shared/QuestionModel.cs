using System.Text.Json.Serialization;

namespace Shared;

public class QuestionModel
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("text")]
    public string Text { get; set; } = "";

    [JsonPropertyName("choices")]
    public List<ChoiceModel> Choices { get; set; } = [];

    [JsonPropertyName("currentChoice")]
    public ChoiceModel CurrentChoice { get; set; } = new ChoiceModel();

    [JsonPropertyName("note")]
    public string Note { get; set; } = "";

    [JsonPropertyName("printable")]
    public bool Printable { get; set; } = true;
    
    [JsonPropertyName("hidden")]
    public bool Hidden { get; set; } = false;

}
