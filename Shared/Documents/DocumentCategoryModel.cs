using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
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
