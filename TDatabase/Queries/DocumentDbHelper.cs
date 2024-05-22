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
                    select new DocumentModel()
                    {
                        Id = d.Id,
                        Date = d.Date,
                        Title = d.Title,
                        LastModified = d.LastModified,
                        Categories = (from r in (from qc in db.QuestionChosens
                                                 from q in db.Questions
                                                 where qc.IdDocument == d.Id
                                                 && q.Id == qc.IdQuestion
                                                 group q by q.IdCategory into q2
                                                 select new { IdCategory = q2.First().IdCategory })

                                      from c in db.Categories
                                      where c.Id == r.IdCategory
                                      orderby c.Order
                                      select new CategoryModel()
                                      {
                                          Id = c.Id,
                                          Text = c.Text,
                                          Order = c.Order,
                                          Questions = (from qc in db.QuestionChosens
                                                       from q in db.Questions
                                                       where qc.IdDocument == d.Id
                                                       && q.Id == qc.IdQuestion
                                                       && q.IdCategory == c.Id
                                                       select new QuestionModel()
                                                       {
                                                           Id = qc.IdQuestion,
                                                           Text = q.Text,
                                                           Order = qc.Order,
                                                           Note = qc.Note ?? "",
                                                           CurrentChoice = (from cc in db.Choices
                                                                            where cc.Id == qc.IdCurrentChoice
                                                                            select new ChoiceModel() 
                                                                            {
                                                                                Id = cc.Id,
                                                                                Value = cc.Value,
                                                                                Tag = cc.Tag,
                                                                            }).FirstOrDefault(),
                                                            Choices = (
                                                             from qch in db.QuestionChoices
                                                             from ch in db.Choices
                                                             where qch.IdQuestion == q.Id
                                                             && ch.Id == qch.IdChoice
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
                Active = true,
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
                        Order = q.Order,
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
            foreach (var document in documents)
            {
                var d = db.Documents.Where(x => x.Id == document.Id).FirstOrDefault();
                if (d is not null && CheckLastEdit(d.LastModified, document.LastModified!.Value))
                {
                    d.LastModified = document.LastModified;
                    foreach (var c in document.Categories)
                    {
                        foreach(var q in c.Questions)
                        {
                            var qc = db.QuestionChosens.Where(x => x.IdDocument == document.Id && x.IdQuestion == q.Id).FirstOrDefault();
                            if(qc is not null)
                            {
                                qc.IdCurrentChoice = q.CurrentChoice?.Id;
                                qc.Order = q.Order;
                                qc.Printable = q.Printable;
                                qc.Hidden = q.Hidden;
                                await db.SaveChangesAsync();
                                modified.Add(qc.Id);
                            }
                        }
                    }

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

    private static bool CheckLastEdit(DateTime? oldEdit, DateTime newEdit)
    {
        return oldEdit is null || oldEdit < newEdit;
    }
}
