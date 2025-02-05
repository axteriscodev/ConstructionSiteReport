using System.Text.Json.Serialization;

namespace Shared.Documents;

public class CompanyModel
{
    [JsonPropertyName("Id")]
    public int Id { get; set; }
    [JsonPropertyName("subcontractedBy")]
    public int? SubcontractedBy { get; set; }

    [JsonPropertyName("companyName")]
    public string CompanyName { get; set; } = "";

    [JsonPropertyName("selfEmployedName")]
    public string SelfEmployedName { get; set; } = "";

    [JsonPropertyName("address")]
    public string Address { get; set; } = "";

    [JsonPropertyName("taxId")]
    public string TaxId { get; set; } = "";

    [JsonPropertyName("vatCode")]
    public string VatCode { get; set; } = "";

    [JsonPropertyName("phone")]
    public string Phone { get; set; } = "";

    [JsonPropertyName("email")]
    public string Email { get; set; } = "";

    [JsonPropertyName("pec")]
    public string Pec { get; set; } = "";

    [JsonPropertyName("reaNumber")]
    public string ReaNumber { get; set; } = "";

    [JsonPropertyName("workerWelfareFunds")]
    public string WorkerWelfareFunds { get; set; } = "";

    [JsonPropertyName("ccnl")]
    public string Ccnl { get; set; } = "";

    [JsonPropertyName("inpsId")]
    public string InpsId { get; set; } = "";

    [JsonPropertyName("inailId")]
    public string InailId { get; set; } = "";

    [JsonPropertyName("inailPat")]
    public List<PatInailModel> InailPat { get; set; } = [];

    [JsonPropertyName("jobsDescriptions")]
    public string JobsDescriptions { get; set; } = "";

    [JsonPropertyName("present")]
    public bool? Present { get; set; }

    [JsonPropertyName("inChargeWorker")]
    public string? InChargeWorker { get; set; } = "";

    [JsonPropertyName("workers")]
    public string? Workers { get; set; } = "";


    public override bool Equals(object? obj)
    {
        var result = false;
        var comp = obj as CompanyModel;
        if(comp is not null)
        {
            result = Id == comp.Id;
        }

        return result;
    }
}
