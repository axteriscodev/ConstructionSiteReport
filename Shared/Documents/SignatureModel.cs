using System.Text.Json;
using System.Text.Json.Serialization;

namespace Shared.Documents;

public class SignatureModel
{
    [JsonPropertyName("CompanyId")]
    public int CompanyId { get; set; }
    [JsonPropertyName("reportedQuestionsId")]
    public List<int> ReporteQuestionsIds { get; set; } = [];
    [JsonPropertyName("name")]
    public string? Name { get; set; }
    [JsonPropertyName("sign")]
    public string? Sign { get; set; }
    [JsonPropertyName("width")]
    public int Width { get; set; }
    [JsonPropertyName("height")]
    public int Height { get; set; }

    public SignatureModel Clone()
    {
        string serializedObject = JsonSerializer.Serialize(this);
        var newObject = JsonSerializer.Deserialize<SignatureModel>(serializedObject) ?? new();
        return newObject;
    }
}
