using System.Text.Json.Serialization;

namespace Shared;

public class ConstructorSiteModel
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("client")]
    public ClientModel Client { get; set; } = new ClientModel();

    [JsonPropertyName("jobDescription")]
    public string JobDescription { get; set;} = "";

    [JsonPropertyName("address")]
    public string Address { get; set; } = "";

    [JsonPropertyName("startDate")]
    public DateTime StartDate { get; set; } = DateTime.Now;
}
