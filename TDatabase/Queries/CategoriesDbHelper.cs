using Shared;
using TDatabase.Database;
using DB = TDatabase.Database.DbCsclAxteriscoContext;

namespace TDatabase.Queries
{
    public class CategoriesDbHelper
    {
        public static List<CategoryModel> Select(DB db)
        {
            return db.Categories.Where(x => x.Active == true).Select(x => new CategoryModel()
            {
                Id = x.Id,
                Text = x.Text,
                Order = x.Order,
                Questions = db.Questions.Where(q => q.IdCategory == x.Id).Select(q => new QuestionModel()
                {
                    Id = q.Id,
                    Text = q.Text,
                    IdCategory = q.IdCategory,
                    IdSubject = q.IdSubject,
                    Choices = (from qc in db.QuestionChoices
                               join c in db.Choices on qc.IdChoice equals c.Id
                               where qc.IdQuestion == q.Id
                               select new ChoiceModel()
                               {
                                   Id = c.Id,
                                   Tag = c.Tag,
                                   Value = c.Value,
                               }).ToList(),
                }).ToList(),
            }).OrderBy(x => x.Order).ToList();
        }

        public static async Task<int> Insert(DB db, CategoryModel category)
        {
            var macroId = 0;
            try
            {
                var nextId = (db.Categories.Any() ? db.Categories.Max(x => x.Id) : 0) + 1;
                Category newMacro = new()
                {
                    Id = nextId,
                    Text = category.Text,
                    Order = category.Order,
                    Active = true
                };
                db.Categories.Add(newMacro);
                await db.SaveChangesAsync();
                macroId = nextId;
            }
            catch (Exception) { }

            return macroId;
        }

        public static async Task<List<int>> Update(DB db, List<CategoryModel> categories)
        {
            List<int> modified = [];
            try
            {
                foreach (var elem in categories)
                {
                    var m = db.Categories.Where(x => x.Id == elem.Id).SingleOrDefault();
                    if (m is not null)
                    {
                        m.Text = elem.Text;
                        m.Order = elem.Order;
                        if (await db.SaveChangesAsync() > 0)
                        {
                            modified.Add(elem.Id);
                        }
                    }
                }
            }
            catch (Exception) { }

            return modified;
        }

        public static async Task<List<int>> Hide(DB db, List<CategoryModel> categories)
        {
            List<int> hiddenItems = [];
            try
            {
                foreach (var elem in categories)
                {
                    var mc = db.Categories.Where(x => x.Id == elem.Id).SingleOrDefault();
                    if (mc is not null)
                    {
                        mc.Active = false;
                        if (await db.SaveChangesAsync() > 0)
                        {
                            hiddenItems.Add(elem.Id);
                        }
                    }
                }
            }
            catch (Exception) { }

            return hiddenItems;
        }
    }
}
