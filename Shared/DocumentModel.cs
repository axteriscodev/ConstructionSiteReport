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
    [JsonPropertyName("lastModified")]
    public DateTime? LastModified { get; set;}
    /// <summary>
    /// campo utilizzato per mappare le modifiche offline
    /// </summary>
    /// <remarks>
    /// bisogna usare il campo ad int per poterlo usare come indice di recarca su indexedDB
    /// 0 = false;
    /// 1 = true;
    /// </remarks>
    [JsonPropertyName("offlineChange")]
    public int OfflineChange { get; set; } = 0;
    [JsonPropertyName("active")]
    public bool Active { get; set; }
}
