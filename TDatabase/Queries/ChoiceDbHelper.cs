using Shared.Defaults;
using Shared.Templates;
using TDatabase.Database;
using DB = TDatabase.Database.DbCsclDamicoV2Context;

namespace TDatabase.Queries
{
    public class ChoiceDbHelper
    {

        public static List<TemplateChoiceModel> Select(DB db, int organizationId)
        {
            return [.. db.Choices.Where(x=>x.IdOrganization == organizationId && x.Active == true).Select(c => new TemplateChoiceModel
            {
                Id = c.Id,
                Value = c.Value,
                Tag = c.Tag,
                Reportable = c.Reportable,
                Color = c.Color,
            })];
        }

        public static async Task<int> Insert(DB db, TemplateChoiceModel choice, int organizationId)
        {
            var choiceId = 0;
            try
            {
                var nextId = (db.Choices.Any() ? db.Choices.Max(x => x.Id) : 0) + 1;
                Choice newChoice = new()
                {
                    Id = nextId,
                    Value = choice.Value,
                    Tag = choice.Tag, //Max 10 char
                    Reportable = choice.Reportable,
                    Color = "black", //TODO impostare colore quando verrà usato, max 10 char
                    Active = true,
                    IdOrganization = organizationId
                };
                db.Choices.Add(newChoice);
                await db.SaveChangesAsync();
                choiceId = nextId;
            }
            catch (Exception) { }

            return choiceId;
        }

        public static async Task<List<int>> Update(DB db, List<TemplateChoiceModel> choices)
        {
            List<int> modified = [];
            try
            {
                foreach (var elem in choices)
                {
                    var c = db.Choices.Where(x => x.Id == elem.Id).SingleOrDefault();
                    if (c is not null)
                    {
                        c.Value = elem.Value;
                        c.Tag = elem.Tag;
                        c.Reportable = elem.Reportable;
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

        public static async Task<List<int>> Hide(DB db, List<TemplateChoiceModel> choices)
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
