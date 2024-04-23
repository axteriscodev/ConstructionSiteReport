using System.Text.Json.Serialization;

namespace Shared;

public class Document
{
    [JsonPropertyName("id")]
    public int Id { get; set;}

    [JsonPropertyName("title")]
    public string Title { get; set;} = "";

    [JsonPropertyName("date")]
    public DateTime Date { get; set;}

    [JsonPropertyName("client")]
    public Client client { get; set;} = new Client();

    [JsonPropertyName("constructorSite")]
    public ConstructorSite ConstructorSite { get; set;} = new ConstructorSite();

    [JsonPropertyName("version")]
    public int Version { get; set;}

    [JsonPropertyName("revision")]
    public int Revision { get; set;}

    [JsonPropertyName("macroCategories")]
    public List<MacroCategoryModel> MacroCategories { get; set;} = [];

    [JsonPropertyName("companies")]
    public List<Company> Companiesù { get; set;} = [];

    [JsonPropertyName("attachments")]
    public List<Attachment> Attachments { get; set;} = [];
}
