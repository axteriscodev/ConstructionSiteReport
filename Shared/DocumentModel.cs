using System.Text.Json.Serialization;

namespace Shared;

public class DocumentModel
{
    [JsonPropertyName("id")]
    public int Id { get; set;}

    [JsonPropertyName("title")]
    public string Title { get; set;} = "";

    [JsonPropertyName("date")]
    public DateTime Date { get; set;} = DateTime.Now;

    [JsonPropertyName("client")]
    public ClientModel? Client { get; set;}

    [JsonPropertyName("constructorSite")]
    public ConstructorSiteModel? ConstructorSite { get; set;}

    [JsonPropertyName("version")]
    public int Version { get; set;}

    [JsonPropertyName("revision")]
    public int Revision { get; set;}

    [JsonPropertyName("macroCategories")]
    public List<CategoryModel> Categories { get; set;} = [];

    [JsonPropertyName("companies")]
    public List<Company> Companies { get; set;} = [];

    [JsonPropertyName("attachments")]
    public List<Attachment> Attachments { get; set;} = [];
}
