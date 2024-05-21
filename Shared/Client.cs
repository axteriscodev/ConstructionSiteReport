using System.Text.Json.Serialization;

namespace Shared;

public class Client
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = "";
}
