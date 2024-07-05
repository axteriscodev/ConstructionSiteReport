using Shared.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConstructionSiteLibrary.Model.DocumentCompilation
{
    public class ChoiceReportedCompanies
    {
        public List<int> ReportedCompanyIds { get; set; } = new();
        public int QuestionId { get; set; } = new();
        public int ChoiceNumber { get; set; } = new();

    }
}
