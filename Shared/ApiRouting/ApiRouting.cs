using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.ApiRouting
{
    public static class ApiRouting
    {


        #region Funzionality Check

        public const string CheckOnline = "CheckOnline";

        #endregion

        #region Document routing

        public const string SiteDocumentsList = "SiteDocumentsList";
        public const string DocumentsList = "DocumentsList";
        public const string SaveDocument = "SaveDocument";
        public const string UpdateDocument = "UpdateDocument";
        public const string HideDocuments = "HideDocuments";

        #endregion

        #region Template routing

        public const string TemplatesList = "TemplatesList";
        public const string SaveTemplate = "SaveTemplate";
        public const string HideTemplates = "HideTemplates";

        #endregion

        #region Category routing

        public const string CategoriesList = "CategoriesList";
        public const string SaveCategory = "SaveCategory";
        public const string UpdateCategories = "UpdateCategories";
        public const string HideCategories = "HideCategories";

        #endregion

        #region Questions routing

        public const string QuestionsList = "QuestionsList";
        public const string SaveQuestion = "SaveQuestion";
        public const string UpdateQuestions = "UpdateQuestions";
        public const string HideQuestions = "HideQuestions";

        #endregion

        #region Choices routing

        public const string ChoicesList = "ChoicesList";
        public const string SaveChoice = "SaveChoice";
        public const string UpdateChoices = "UpdateChoices";
        public const string HideChoices = "HideChoices";

        #endregion

        #region Client routing

        public const string ClientsList = "ClientsList";
        public const string SaveClient = "SaveClient";
        public const string UpdateClients = "UpdateClients";
        public const string HideClients = "HideClients";

        #endregion

        #region Company routing

        public const string CompaniesList = "CompaniesList";
        public const string SaveCompany = "SaveCompany";
        public const string UpdateCompanies = "UpdateCompanies";
        public const string HideCompanies = "HideCompanies";

        #endregion

        #region ConstructionSite routing

        public const string ConstructorSitesList = "ConstructorSitesList";
        public const string ConstructorSiteInfo = "ConstructorSiteInfo";
        public const string SaveConstructorSite = "SaveConstructorSite";
        public const string UpdateConstructorSites = "UpdateConstructorSites";
        public const string HideConstructorSites = "HideConstructorSites";

        #endregion
    }
}
