using Shared.Templates;
using TDatabase.Database;
using DB = TDatabase.Database.DbCsclDamicoV2Context;

namespace TDatabase.Queries
{
    public class TemplateDescriptionDbHelper
    {


        public static List<TemplateDescriptionModel> Select(DB db, int idDescription = 0)
        {
            var templateSelect = idDescription > 0 ? db.TemplateDescriptions.Where(x => x.Id == idDescription)
                                                   : db.TemplateDescriptions;

            return templateSelect.Select(x => new TemplateDescriptionModel()
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description ?? "",
            }).ToList();
        }

        public static async Task<int> Insert(DB db, TemplateDescriptionModel description)
        {
            var descriptionId = 0;

            try
            {
                var nextId = (db.TemplateDescriptions.Any() ? db.TemplateDescriptions.Max(x => x.Id) : 0) + 1;
                TemplateDescription newDesc = new()
                {
                    Id = nextId,
                    Title = description.Title,
                    Description = description.Description,
                    Active = true
                };
                db.TemplateDescriptions.Add(newDesc);

                await db.SaveChangesAsync();
                descriptionId = nextId;
            }catch(Exception) 
            { 
                
            }
            return descriptionId;
        }

        public static async Task<List<int>> Update(DB db, List<TemplateDescriptionModel> descriptions)
        {
            List<int> ids = [];
            try
            {
                foreach (var desc in descriptions)
                {
                    var d = db.TemplateDescriptions.Where(x => x.Id == desc.Id).FirstOrDefault();
                    if (d is not null)
                    {
                        d.Title = desc.Title;
                        d.Description = desc.Description;

                        if (await db.SaveChangesAsync() > 0)
                        {
                            ids.Add(d.Id);
                        }
                    }
                }
            }
            catch (Exception) { }

            return ids;
        }

        public static async Task<List<int>> hide(DB db, List<TemplateDescriptionModel> descriptions)
        {
            List<int> ids = [];
            try
            {
                foreach (var desc in descriptions)
                {
                    var d = db.TemplateDescriptions.Where(x => x.Id == desc.Id).FirstOrDefault();
                    if (d is not null)
                    {
                        d.Active = false;

                        if (await db.SaveChangesAsync() > 0)
                        {
                            ids.Add(d.Id);
                        }
                    }
                }
            }
            catch (Exception) { }

            return ids;
        }

    }
}
