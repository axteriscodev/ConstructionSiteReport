using System.Data.Common;
using Shared;
using Shared.Templates;
using TDatabase.Database;
using DB = TDatabase.Database.DbCsclDamicoV2Context;

namespace TDatabase.Queries;

public class TemplateDbHelper
{
    public static List<TemplateModel> Select(DB db, int idTemplate = 0)
    {
        var templateSelect = db.Templates.AsQueryable();

        if (idTemplate > 0)
        {
            templateSelect = templateSelect.Where(x => x.Id == idTemplate);
        }

        var docs = (from t in templateSelect
                    join desc in db.TemplateDescriptions on t.IdDescription equals desc.Id
                    where t.Active == true
                    select new TemplateModel()
                    {
                        IdTemplate = t.Id,
                        TitleTemplate = t.Title,
                        Description = new() { Id = desc.Id, Title = desc.Title, Description = desc.Description ?? "" },
                        Note = t.Note ?? "",
                        CreationDate = t.Date,
                        Categories = (from r in (from qc in db.QuestionChosens
                                                 from q in db.Questions
                                                 where qc.IdTemplate == t.Id
                                                 && q.Id == qc.IdQuestion
                                                 group q by q.IdCategory into q2
                                                 select new { q2.First().IdCategory })

                                      from c in db.Categories
                                      where c.Id == r.IdCategory
                                      orderby c.Order
                                      select new TemplateCategoryModel()
                                      {
                                          Id = c.Id,
                                          Text = c.Text,
                                          Order = c.Order,
                                          Questions = (from qc in db.QuestionChosens
                                                       from q in db.Questions
                                                       where qc.IdTemplate == t.Id
                                                       && q.Id == qc.IdQuestion
                                                       && q.IdCategory == c.Id
                                                       select new TemplateQuestionModel()
                                                       {
                                                           Id = qc.IdQuestion,
                                                           Text = q.Text,
                                                           Order = qc.Order,
                                                           Choices = (
                                                             from qch in db.QuestionChoices
                                                             from ch in db.Choices
                                                             where qch.IdQuestion == q.Id
                                                             && ch.Id == qch.IdChoice
                                                             && ch.Active == true
                                                             select new TemplateChoiceModel()
                                                             {
                                                                 Id = ch.Id,
                                                                 Value = ch.Value,
                                                                 Reportable = ch.Reportable,
                                                             }).ToList()
                                                       }).ToList()
                                      }).ToList(),

                    }).ToList();

        return docs;
    }

    public static async Task<int> Insert(DB db, TemplateModel template)
    {
        var templateId = 0;

        try
        {
            var nextId = (db.Templates.Any() ? db.Templates.Max(x => x.Id) : 0) + 1;
            Template newTemplate = new()
            {
                Id = nextId,
                Title = template.TitleTemplate,
                Note = template.Note,
                Date = template.CreationDate,
                Active = true,
            };

            // se ho associato una descrizione allora lo salvo nel db
            if(template.Description.Id > 0)
            {
                newTemplate.IdDescription = template.Description.Id;
            }

            db.Templates.Add(newTemplate);

            var nextQuestionChosenId = (db.QuestionChosens.Any() ? db.QuestionChosens.Max(x => x.Id) : 0) + 1;

            foreach (var c in template.Categories)
            {
                foreach (var tq in c.Questions.Cast<TemplateQuestionModel>())
                {
                    QuestionChosen qc = new()
                    {
                        Id = nextQuestionChosenId,
                        IdTemplate = nextId,
                        IdQuestion = tq.Id,
                        Order = tq.Order,
                    };
                    db.QuestionChosens.Add(qc);
                    nextQuestionChosenId++;
                }
            }

            await db.SaveChangesAsync();
            templateId = nextId;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        return templateId;
    }

    //NOTA non esiste un metodo di update, poiché ogni template eventualmente modificato viene creato come nuovo

    public static async Task<List<int>> Hide(DB db, List<TemplateModel> templates)
    {
        List<int> hiddenItems = [];
        try
        {
            foreach (var elem in templates)
            {
                var c = db.Templates.Where(x => x.Id == elem.IdTemplate).SingleOrDefault();
                if (c is not null)
                {
                    c.Active = false;
                    if (await db.SaveChangesAsync() > 0)
                    {
                        hiddenItems.Add(elem.IdTemplate);
                    }
                }
            }
        }
        catch (Exception) { }

        return hiddenItems;
    }
}
