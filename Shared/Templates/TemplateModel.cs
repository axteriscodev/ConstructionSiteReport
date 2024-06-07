using System.Data;
using System.Text.Json.Serialization;
using Shared.Defaults;

namespace Shared.Templates;

public class TemplateModel
{
    [JsonPropertyName("idTemplate")]
    public int IdTemplate { get; set; }

    [JsonPropertyName("titleTemplate")]
    public string TitleTemplate { get; set; } = "";

    [JsonPropertyName("note")]
    public string Note { get; set; } = "";

    [JsonPropertyName("creationDateTemplate")]
    public DateTime CreationDate { get; set; } = DateTime.Now;

    [JsonPropertyName("categories")]
    public List<TemplateCategoryModel> Categories { get; set; } = [];

    [JsonPropertyName("description")]
    public TemplateDescriptionModel Description { get; set; } = new();
}
