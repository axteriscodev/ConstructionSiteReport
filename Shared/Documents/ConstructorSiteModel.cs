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

    [JsonPropertyName("companies")]
    public List<CompanyModel> Companies { get; set; } = [];

    [JsonPropertyName("jobDescription")]
    public string JobDescription { get; set; } = "";

    [JsonPropertyName("address")]
    public string Address { get; set; } = "";

    [JsonPropertyName("startDate")]
    public DateTime? StartDate { get; set; } = DateTime.Now;

    [JsonPropertyName("endDate")]
    public DateTime? EndDate { get; set; }

    [JsonPropertyName("idSico")]
    public string? IdSico { get; set; }

    [JsonPropertyName("idSicoInProgress")]
    public string? IdSicoInProgress { get; set; }

    [JsonPropertyName("preliminaryNotificationStartDate")]
    public string? PreliminaryNotificationStartDate { get; set; }

    [JsonPropertyName("preliminaryNotificationInProgress")]
    public string? PreliminaryNotificationInProgress { get; set; }

    [JsonPropertyName("note")]
    public string? Note { get; set; }

    [JsonPropertyName("rl")]
    public string? RL { get; set; }

     [JsonPropertyName("dl")]
    public string? DL { get; set; }

     [JsonPropertyName("cse")]
    public string? CSE { get; set; }


}
