using System.Text.Json.Serialization;

namespace Shared.Documents;

public class MeteoConditionModel
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; } = "";
}
