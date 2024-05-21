using Shared;
using TDatabase.Database;
using DB = TDatabase.Database.DbCsclAxteriscoContext;

namespace TDatabase.Queries
{
    public class ChoiceDbHelper
    {

        public static List<ChoiceModel> Select(DB db)
        {
            return [.. db.Choices.Where(x=>x.Active == true).Select(c => new ChoiceModel
            {
                Id = c.Id,
                Value = c.Value,
                Tag = c.Tag,
            })];
        }

        public static async Task<int> Insert(DB db, ChoiceModel choice)
        {
            var choiseId = 0;
            try
            {
                var nextId = (db.Choices.Any() ? db.Choices.Max(x=>x.Id) : 0) + 1;
                Choice newChoice = new()
                {
                    Id = nextId,
                    Value = choice.Value,
                    Tag = choice.Tag,
                    Active = true
                };
                db.Choices.Add(newChoice);
                await db.SaveChangesAsync();
                choiseId = nextId;
            }catch (Exception) { }

            return choiseId;
        }

        public static async Task<List<int>> Update(DB db, List<ChoiceModel> choices)
        {
            List<int> modified = [];
            try
            {
                foreach(var elem in choices)
                {
                    var c = db.Choices.Where(x => x.Id == elem.Id).SingleOrDefault();
                    if (c is not null)
                    {
                        c.Value = elem.Value;
                        c.Tag = elem.Tag;
                        if( await db.SaveChangesAsync() > 0)
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
                    var c = db.Choices.Where(x => x.Id == elem.Id).SingleOrDefault();
                    if (c is not null)
                    {
                        c.Active = false;
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
