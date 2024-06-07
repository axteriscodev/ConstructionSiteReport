using System.Text.Json.Serialization;

namespace Shared.Templates;

public class TemplateDescriptionModel
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; } = "";

    [JsonPropertyName("description")]
    public string Description { get; set; } = "";

}
