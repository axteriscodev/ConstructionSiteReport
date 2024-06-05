using System.Text.Json.Serialization;

namespace Shared.Documents;

public class ConstructorSiteModel
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = "";

    [JsonPropertyName("client")]
    public ClientModel? Client { get; set; }

    [JsonPropertyName("jobDescription")]
    public string JobDescription { get; set; } = "";

    [JsonPropertyName("address")]
    public string Address { get; set; } = "";

    [JsonPropertyName("startDate")]
    public DateTime? StartDate { get; set; } = DateTime.Now;

    [JsonPropertyName("endDate")]
    public DateTime? EndDate { get; set; }
}
