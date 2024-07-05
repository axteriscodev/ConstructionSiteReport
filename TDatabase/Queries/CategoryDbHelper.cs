using Shared.Defaults;
using Shared.Templates;
using TDatabase.Database;
using DB = TDatabase.Database.DbCsclDamicoV2Context;

namespace TDatabase.Queries
{
    public class CategoryDbHelper
    {
        public static List<TemplateCategoryModel> Select(DB db)
        {
            return db.Categories.Where(x => x.Active == true).Select(x => new TemplateCategoryModel()
            {
                Id = x.Id,
                Text = x.Text,
                Order = x.Order,
                Questions = (from q in db.Questions
                             where q.IdCategory == x.Id
                             select new TemplateQuestionModel()
                             {
                                 Id = q.Id,
                                 IdCategory = q.IdCategory,
                                 Text = q.Text,                                  
                             }).ToList()
            }).OrderBy(x => x.Order).ToList();
        }

        public static async Task<int> Insert(DB db, CategoryModel category)
        {
            var id = 0;
            try
            {
                var nextId = (db.Categories.Any() ? db.Categories.Max(x => x.Id) : 0) + 1;
                var nextOrder = (db.Categories.Any() ? db.Categories.Max(x => x.Order) : 0) + 1;
                Category newCat = new()
                {
                    Id = nextId,
                    Text = category.Text,
                    Order = nextOrder,
                    Active = true
                };
                db.Categories.Add(newCat);
                await db.SaveChangesAsync();
                id = nextId;
            }
            catch (Exception) { }

            return id;
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
