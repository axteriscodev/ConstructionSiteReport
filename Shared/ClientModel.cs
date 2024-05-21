using System.Text.Json.Serialization;

namespace Shared;

public class ClientModel
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = "";
}
