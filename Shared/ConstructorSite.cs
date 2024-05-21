using System.Text.Json.Serialization;

namespace Shared;

public class ConstructorSite
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("client")]
    public Client Client { get; set; } = new Client();

    [JsonPropertyName("jobDescription")]
    public string JobDescription { get; set;} = "";

    [JsonPropertyName("address")]
    public string Address { get; set; } = "";

    [JsonPropertyName("startDate")]
    public DateTime StartDate { get; set; } = DateTime.Now;
}
