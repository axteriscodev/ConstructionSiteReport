using System.Text.Json.Serialization;
using Shared.Defaults;

namespace Shared.Documents;

public class DocumentModel
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    [JsonPropertyName("idTemplate")]
    public int IdTemplate { get; set; }

    [JsonPropertyName("meteoCondition")]
    public MeteoConditionModel? MeteoCondition { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; } = "";

    [JsonPropertyName("description")]
    public string Description { get; set; } = "";

    [JsonPropertyName("creationDate")]
    public DateTime? CreationDate { get; set; }

    [JsonPropertyName("compilationDate")]
    public DateTime? CompilationDate { get; set; }

    [JsonPropertyName("lastEditDate")]
    public DateTime? LastEditDate { get; set; }

    [JsonPropertyName("readonly")]
    public bool ReadOnly { get; set; }

    [JsonPropertyName("ChangedOffline")]
    public int ChangedOffline { get; set; }

    [JsonPropertyName("client")]
    public ClientModel? Client { get; set; }

    [JsonPropertyName("constructorSite")]
    public ConstructorSiteModel ConstructorSite { get; set; } = new();

    [JsonPropertyName("categories")]
    public List<DocumentCategoryModel> Categories { get; set; } = [];

    [JsonPropertyName("companies")]
    public List<CompanyModel> Companies { get; set; } = [];

    [JsonPropertyName("signatures")]
    public List<SignatureModel> Signature { get; set; } = [];

    [JsonPropertyName("attachments")]
    public List<AttachmentModel> Attachments { get; set; } = [];

    [JsonPropertyName("notes")]
    public List<NoteModel> Notes { get; set; } = [];

    [JsonPropertyName("cse")]
    public string? CSE { get; set; }

    [JsonPropertyName("draftedIn")]
    public string? DraftedIn { get; set; }

     [JsonPropertyName("completedIn")]
    public string? CompletedIn { get; set; }


}
