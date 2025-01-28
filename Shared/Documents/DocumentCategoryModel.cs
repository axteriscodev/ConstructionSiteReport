using System.Text.Json.Serialization;
using Shared.Defaults;

namespace Shared.Documents
{
    public class DocumentCategoryModel : CategoryModel
    {
        [JsonPropertyName("printable")]
        public bool Printable { get; set; }
        [JsonPropertyName("questions")]
        public List<DocumentQuestionModel> Questions { get; set; } = [];
    }
}
