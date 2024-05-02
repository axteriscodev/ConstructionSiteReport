using System.Data.Common;
using Shared;
using TDatabase.Database;
using DB = TDatabase.Database.DbCsclAxteriscoContext;
using DocumentModel = Shared.DocumentModel;

namespace TDatabase.Queries;

public class DocumentDbHelper
{
    public static List<DocumentModel> Select(DB db, int idDocument = 0)
    {
        var documents = db.Documents.AsQueryable();

        if (idDocument > 0)
        {
            documents = documents.Where(d => d.Id == idDocument);
        }

        var docs = (from d in documents
                    where d.Id == idDocument
                    select new DocumentModel()
                    {
                        Categories = (from r in (from qc in db.QuestionChosens
                                                 from q in db.Questions
                                                 where qc.IdDocument == d.Id
                                                 && qc.Id == q.Id
                                                 group q by q.IdCategory into q2
                                                 select new { Id = q2.First().IdCategory })

                                      from c in db.Categories
                                      where c.Id == r.Id
                                      select new CategoryModel()
                                      {
                                          Id = c.Id,
                                          Text = c.Text,
                                          Questions = (from qc in db.QuestionChosens
                                                       from q in db.Questions
                                                       where qc.IdDocument == d.Id
                                                       && qc.Id == q.Id
                                                       select new QuestionModel()
                                                       {
                                                           Id = qc.Id,
                                                           Text = q.Text,
                                                            Choices = (
                                                             from qch in db.QuestionChoices
                                                             from ch in db.Choices
                                                             where qch.IdQuestion == q.Id
                                                             && ch.Id == qch.IdQuestion
                                                             && ch.Active == true
                                                             select new ChoiceModel()
                                                             {
                                                                Id = ch.Id,
                                                                Value = ch.Value,
                                                                Tag = ch.Tag,
                                                             }).ToList()
                                                       }).ToList()
                                      }).ToList(),

                    }).ToList();

        return docs;
    }

    public static async Task<int> Insert(DB db, DocumentModel document)
    {
        var documentId = 0;
        try
        {
            var nextId = (db.Documents.Any() ? db.Documents.Max(x => x.Id) : 0) + 1;
            Document newDocument = new()
            {
                Id = nextId,
                IdConstructorSite = document.ConstructorSite?.Id,
                IdClient = document.Client?.Id,
                Title = document.Title,
                Date = document.Date,
            };

            db.Documents.Add(newDocument);

            var nextQuestionChosenId = (db.QuestionChosens.Any() ? db.QuestionChosens.Max(x => x.Id) : 0) + 1;

            foreach (var c in document.Categories)
            {
                foreach (var q in c.Questions)
                {
                    QuestionChosen qc = new()
                    {
                        Id = nextQuestionChosenId,
                        IdDocument = nextId,
                        IdQuestion = q.Id,
                        Printable = true,
                        Hidden = false
                    };
                    db.QuestionChosens.Add(qc);
                    nextQuestionChosenId++;
                }

            }
            await db.SaveChangesAsync();
            documentId = nextId;
        }
        catch (Exception e) {
            var ex = e.Message;
         }

        return documentId;
    }

    public static async Task<List<int>> Update(DB db, List<DocumentModel> documents)
    {
        List<int> modified = [];
        try
        {
            foreach (var elem in documents)
            {
                var d = db.Documents.Where(x => x.Id == elem.Id).FirstOrDefault();
                if (d is not null)
                {
                    // d.IdCategory = elem.IdCategory;
                    // d.IdSubject = elem.IdSubject;
                    // d.Text = elem.Text;

                    // db.QuestionChoices.RemoveRange(db.QuestionChoices.Where(x => x.IdQuestion == elem.Id));
                    // foreach(var choice in elem.Choices)
                    // {
                    //     QuestionChoice newqc = new()
                    //     {
                    //         IdChoice = choice.Id,
                    //         IdQuestion = elem.Id
                    //     };
                    //     db.Add(newqc);
                    // }

                    // if (await db.SaveChangesAsync() > 0)
                    // {
                    //     modified.Add(elem.Id);
                    // }
                }
            }
        }
        catch (Exception) { }

        return modified;
    }

    public static async Task<List<int>> Hide(DB db, List<DocumentModel> documents)
    {
        List<int> hiddenItems = [];
        try
        {
            foreach (var elem in documents)
            {
                var c = db.Documents.Where(x => x.Id == elem.Id).SingleOrDefault();
                if (c is not null)
                {
                    //c.Active = false;
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
