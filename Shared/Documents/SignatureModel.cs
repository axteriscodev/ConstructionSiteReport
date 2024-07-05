using System.Text.Json.Serialization;

namespace Shared.Documents;

public class SignatureModel
{
    [JsonPropertyName("idAzienda")]
    public int IdAzienda { get; set; }
    [JsonPropertyName("reportedQuestionsId")]
    public List<int> ReporteQuestionsIds { get; set; } = [];
    [JsonPropertyName("sign")]
    public string? Sign { get; set; }
    [JsonPropertyName("width")]
    public int Width { get; set; }
    [JsonPropertyName("height")]
    public int Height { get; set; }
}
