using System.Text.Json.Serialization;

namespace Shared;

public class Company
{
    [JsonPropertyName("Id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = "";

    [JsonPropertyName("address")]
    public string Address { get; set; } = "";

    [JsonPropertyName("vatCode")]
    public string VatCode { get; set; } = "";
}
