using System;
using System.Text.Json.Serialization;

namespace Shared.Documents;

public class PatInailModel
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    [JsonPropertyName("name")]
    public string Name { get; set; } = "";
}
