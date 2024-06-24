using System.Text.Json.Serialization;
using Shared.Defaults;

namespace Shared.Documents;

public class DocumentQuestionModel : IQuestion
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("text")]
    public string Text { get; set; } = "";

    [JsonPropertyName("order")]
    public int Order { get; set; }  

    [JsonPropertyName("choices")]
    public List<DocumentChoiceModel> Choices { get; set; } = [];

    [JsonPropertyName("currentChoice")]
    public List<DocumentChoiceModel> CurrentChoices { get; set; } = [];

    [JsonPropertyName("attachments")]
    public List<AttachmentModel> Attachments { get; set; } = [];

    [JsonPropertyName("note")]
    public string Note { get; set; } = "";

    [JsonPropertyName("printable")]
    public bool Printable { get; set; }

    [JsonPropertyName("hidden")]
    public bool Hidden { get; set; }
}
