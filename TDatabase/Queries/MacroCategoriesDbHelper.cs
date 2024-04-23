using Shared;
using TDatabase.Database;
using DB = TDatabase.Database.DbCsclDamicoContext;

namespace TDatabase.Queries
{
    public class MacroCategoriesDbHelper
    {
        public static List<MacroCategoryModel> Select(DB db)
        {
            return db.MacroCategories.Select(x => new MacroCategoryModel()
            {
                Id = x.Id,
                Text = x.Text,
                Questions = (db.Questions.Where(q => q.IdMacroCategory == x.Id).Select(q => new QuestionModel()
                {
                    Id = q.Id,
                    Text = q.Text,
                    Choices = (from qc in db.QuestionChoices
                               join c in db.Choices on qc.IdChoice equals c.Id
                               where qc.IdQuestion == q.Id
                               select new ChoiceModel()
                               {
                                   Id = c.Id,
                                   Tag = c.Tag,
                                   Value = c.Value,
                               }).ToList(),
                })).ToList(),
            }).ToList();
        }

        public static async Task<int> Insert(DB db, MacroCategoryModel macro)
        {
            var macroId = 0;
            try
            {
                var nextId = (db.MacroCategories.Any() ? db.MacroCategories.Max(x => x.Id) : 0) + 1;
                MacroCategory newMacro = new()
                {
                    Id = nextId,
                    Text = macro.Text,
                    Active = true
                };
                db.MacroCategories.Add(newMacro);
                await db.SaveChangesAsync();
                macroId = nextId;
            }
            catch (Exception) { }

            return macroId;
        }

        public static async Task<List<int>> Update(DB db, List<MacroCategoryModel> macros)
        {
            List<int> modified = [];
            try
            {
                foreach (var elem in macros)
                {
                    var m = db.MacroCategories.Where(x => x.Id == elem.Id).SingleOrDefault();
                    if (m is not null)
                    {
                        m.Text = elem.Text;
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

        public static async Task<List<int>> Hide(DB db, List<ChoiceModel> choices)
        {
            List<int> hiddenItems = [];
            try
            {
                foreach (var elem in choices)
                {
                    var mc = db.MacroCategories.Where(x => x.Id == elem.Id).SingleOrDefault();
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
