using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConstructionSiteLibrary
{
    public class ImporterPages
    {
        public static List<System.Reflection.Assembly> LoadAssemblyPages()
        {
            List<System.Reflection.Assembly> pages = [];
            pages.Add(typeof(Pages.Home).Assembly);
            pages.Add(typeof(Pages.QuestionPage).Assembly);
            pages.Add(typeof(Pages.FormCreationPage).Assembly);
            pages.Add(typeof(Pages.FormCompilationPage).Assembly);
            pages.Add(typeof(Pages.ChoicesPage).Assembly);
            pages.Add(typeof(Pages.CategoriesPage).Assembly);

            return pages;
        }
    }
}
