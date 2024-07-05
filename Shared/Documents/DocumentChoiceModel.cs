using System.Text.Json.Serialization;
using Shared.Defaults;
using Shared.Templates;

namespace Shared.Documents;

public class DocumentChoiceModel : TemplateChoiceModel
{

    [JsonPropertyName("reportedCompanyIds")]
    public List<int> ReportedCompanyIds { get; set; } = [];
    /// <summary>
    /// Campo utilizzato per numerare le varie risposte durante la compilazione del documento
    /// </summary>
    [JsonPropertyName("choiceIndex")]
    public int ChoiceIndex {  get; set; }


    public DocumentChoiceModel Clone()
    {
        DocumentChoiceModel clone = new()
        {
            Id = Id,
            Tag = Tag,
            Value = Value,
            Color = Color,
            Reportable = Reportable,
            ReportedCompanyIds = [],
            ChoiceIndex = ChoiceIndex
        };
        ReportedCompanyIds.AddRange(this.ReportedCompanyIds);
        return clone;
    }

}
