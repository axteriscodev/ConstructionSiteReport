using Shared.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConstructionSiteLibrary.Model.DocumentCompilation
{
    class DocumentUtils
    {


        #region Titoli macro categorie documento

        public static string ConstructionSiteData = "Dati, cantiere, figure responsabili, progettisti, riunioni";
        public static string Companies = "Imprese presenti: figure responsabili, personale, formale, attrezzatura";
        public static string QuestionNotes = "Ulteriori precisazioni ed eventuali prescrizioni";
        public static string Attachment = "Allegati";
        public static string DocumentNotes = "Note aggiuntive";
        public static string DocumentSignatures = "Firme";

        #endregion

        #region Ancore per navigazione documento

        public static int TypeConstructionSite = 1;
        public static int TypeCompanies = 2;
        public static int TypeQuestions = 3;
        public static int TypeQuestionNotes = 4;
        public static int TypeAttachment = 5;
        public static int TypeDocumentNotes = 6;
        public static int TypeSign = 7;

        public static DocumentAnchor ADatiVerbale = new("Dati Verbale", "DatiVerbale");
        public static DocumentAnchor ADatiCantiere = new("Dati Cantiere", "DatiCantiere");
        public static DocumentAnchor ADatiAnziende = new("", "");

        #endregion


        #region Metodi di visualizzazione per Firme

        public static string PrintCompanyFromSignature(SignatureModel signature, DocumentModel document)
        {
            var compName = "";
            var company = document.Companies.Where(x => x.Id == signature.CompanyId).FirstOrDefault();
            if (company is not null)
            {
                compName = string.IsNullOrEmpty(company.CompanyName) ? company.SelfEmployedName : company.CompanyName;
            }
            return compName;
        }

        public static string PrintQuestionForReported(int idQuestion, DocumentModel document)
        {
            var questionNumber = "";
            foreach (var category in document.Categories)
            {
                var q = category.Questions.Where(x => x.Id == idQuestion).SingleOrDefault();
                if (q is not null)
                {
                    questionNumber = $"{category.Order}.{q.Order} ";
                    break;
                }
            }
            return questionNumber;
        }

        #endregion

        #region Metodi di visualizzazione per Question

        public static string ReportedCompany(List<int> reportedCompanyIds, DocumentModel document)
        {
            var message = reportedCompanyIds.Count > 0 ? "(" : "";
            foreach (var companyId in reportedCompanyIds)
            {
                var companyName = document.Companies.Where(x => x.Id == companyId).Select(x => x.CompanyName).FirstOrDefault() ?? "";
                message += $"{companyName}, ";
            }

            if (message.EndsWith(", "))
            {
                //rimuovo l'ultima virgola
                message = message.Remove(message.Length - 2);
                message += ")";
            }

            return message;
        }

        public static string PrintDocumentField(string? field)
        {
            return string.IsNullOrEmpty(field) ? "" : field;
        }

        public static string CategoryNumber(DocumentCategoryModel cat)
        {
            return cat.Order + ".";
        }

        public static string CategoryText(DocumentCategoryModel cat)
        {
            return cat.Order +". " + cat.Text;
        }

        public static string QuestionTextNumber(DocumentCategoryModel cat, string questionText, int order)
        {
            return cat.Order + "." + order + " " + questionText;
        }

        public static string QuestionText(DocumentCategoryModel cat, string questionText)
        {
            return questionText;
        }

        public static string QuestionNumber(DocumentCategoryModel cat, int order)
        {
            return cat.Order + "." + order;
        }

        public static void ShowQuestions(VisualCategory cat)
        {
            cat.ShowQuestion = !cat.ShowQuestion;
        }

        public static string AccordionIcon(VisualCategory cat)
        {
            return cat.ShowQuestion ? "remove" : "add";
        }

        #endregion


        #region Metodi di visualizzazione per allegati domande

        public static List<VisualCategory> CreateVisualCategories(List<DocumentCategoryModel> categories)
        {
            List<VisualCategory> visualCategories = [];

            if (categories is not null)
            {
                foreach (var cat in categories)
                {
                    var hasAttachments = false;
                    foreach (var question in cat.Questions)
                    {
                        if (question.Attachments.Any())
                        {
                            hasAttachments = true;
                        }
                    }

                    if (hasAttachments)
                    {
                        visualCategories.Add(new VisualCategory() { Category = cat });
                    }
                }
            }
            return visualCategories;
        }

        #endregion
    }
}
