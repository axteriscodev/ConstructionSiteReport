using System.Text.Json;
using System.Text.Json.Serialization;
using Shared.Defaults;

namespace Shared.Documents
{
    public class DocumentCategoryModel : CategoryModel
    {
        [JsonPropertyName("printable")]
        public bool Printable { get; set; } = true;
        [JsonPropertyName("questions")]
        public List<DocumentQuestionModel> Questions { get; set; } = [];

        public DocumentCategoryModel Clone()
        {
            string serializedObject = JsonSerializer.Serialize(this);
            var newObject = JsonSerializer.Deserialize<DocumentCategoryModel>(serializedObject) ?? new();
            return newObject;
        }
    }
}
