using Shared.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConstructionSiteLibrary.Model.DocumentCompilation
{
    class VisualCategory
    {
        public bool ShowQuestion = true;
        public DocumentCategoryModel Category { get; set; } = new();
    }
}
