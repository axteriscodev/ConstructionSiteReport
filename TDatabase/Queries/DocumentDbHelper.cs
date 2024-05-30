using System.Data.Common;
using Shared;
using TDatabase.Database;
using DB = TDatabase.Database.DbCsclDamicoV2Context;
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
                        Title = d.Title,
                        CreationDate = d.CreationDate ?? DateTime.Now,
                        CompilationDate = d.CompilationDate,
                        LastEditDate = d.LastEditDate ?? DateTime.Now,
                        Categories = (from r in (from qc in db.QuestionChosens
                                                 from q in db.Questions
                                                 where qc.Id == d.Id
                                                 && q.Id == qc.IdQuestion
                                                 group q by q.IdCategory into q2
                                                 select new { q2.First().IdCategory })

                                      from c in db.Categories
                                      where c.Id == r.IdCategory
                                      orderby c.Order
                                      select new CategoryModel()
                                      {
                                          Id = c.Id,
                                          Text = c.Text,
                                          Order = c.Order,
                                          Questions = (from tc in db.Templates
                                                       from qc in db.QuestionChosens
                                                       from q in db.Questions
                                                       where tc.Id == d.IdTemplate
                                                       && qc.IdTemplate == tc.Id
                                                       && q.Id == qc.IdQuestion
                                                       && q.IdCategory == c.Id
                                                       select new DocumentQuestionModel()
                                                       {
                                                           Id = qc.IdQuestion,
                                                           Text = q.Text,
                                                           //Order = qc.Order,
                                                           Note = qc.Note ?? "",
                                                           CurrentChoices = (
                                                                            from qa in db.QuestionAnswereds
                                                                            from cc in db.Choices
                                                                            where qa.IdDocument == d.Id
                                                                            && qa.IdQuestionChosen == qc.Id
                                                                            && cc.Id == qa.IdCurrentChoice
                                                                            select new DocumentChoiceModel()
                                                                            {
                                                                                Id = cc.Id,
                                                                                Value = cc.Value,
                                                                                Reportable = cc.Reportable,
                                                                                Color = cc.Color,
                                                                                ReportedCompanyIds = (from rc in db.ReportedCompanies
                                                                                                      where rc.IdCurrentChoice == qa.IdCurrentChoice
                                                                                                      && rc.IdQuestionChosen == qa.IdQuestionChosen
                                                                                                      && rc.IdDocument == d.Id
                                                                                                      select rc.IdCompany).ToList(),
                                                                            }).ToList(),
                                                           Choices = (
                                                             from qch in db.QuestionChoices
                                                             from ch in db.Choices
                                                             where qch.IdQuestion == q.Id
                                                             && ch.Id == qch.IdChoice
                                                             && ch.Active == true
                                                             select new DocumentChoiceModel()
                                                             {
                                                                 Id = ch.Id,
                                                                 Value = ch.Value,
                                                                 Reportable = ch.Reportable,
                                                                 Color = ch.Color,

                                                             }).ToList(),

                                                           Attachments = (from aq in db.AttachmentQuestions
                                                                          from att in db.Attachments
                                                                          where aq.IdQuestion == qc.Id
                                                                          && att.IdDocument == d.Id
                                                                          && att.Id == aq.IdAttachment
                                                                          select new AttachmentModel()
                                                                          {
                                                                              Id = att.Id,
                                                                              Name = att.Name,
                                                                              Date = att.DateTime,
                                                                              //TODO manca il binding alla path
                                                                          }).ToList(),
                                                       }).Cast<IQuestion>().ToList()
                                      }).ToList(),
                        ConstructorSite = (from cs in db.ConstructorSites
                                           where cs.Id == d.IdConstructorSite
                                           select new ConstructorSiteModel()
                                           {
                                               Id = cs.Id,
                                               JobDescription = cs.JobDescription,
                                               StartDate = cs.StartDate,
                                               Address = cs.Address,
                                               Client = (from cl in db.Clients
                                                         where cl.Id == cs.IdClient
                                                         select new ClientModel()
                                                         {
                                                             Id = cl.Id,
                                                             Name = cl.Name
                                                         }).SingleOrDefault() ?? new(),
                                           }).SingleOrDefault(),
                        Client = (from cl in db.Clients
                                  where cl.Id == d.IdClient
                                  select new ClientModel()
                                  {
                                      Id = cl.Id,
                                      Name = cl.Name
                                  }).SingleOrDefault(),

                        Companies = (from compDoc in db.CompanyDocuments
                                     from comp in db.Companies
                                     where compDoc.IdDocument == d.Id
                                     && comp.Id == compDoc.IdCompany
                                     select new CompanyModel()
                                     {
                                         Id = comp.Id,
                                         Name = comp.Name,
                                         Address = comp.Address ?? "",
                                         VatCode = comp.Vatcode,
                                         Present = compDoc.Present
                                     }).ToList()
                    }).ToList();

        return docs;
    }

    public static async Task<int> Insert(DB db, DocumentModel document)
    {

        //TODO associare template

        var documentId = 0;
        try
        {
            var nextId = (db.Documents.Any() ? db.Documents.Max(x => x.Id) : 0) + 1;
            Document newDocument = new()
            {
                Id = nextId,
                IdConstructorSite = document.ConstructorSite?.Id,
                IdClient = document.Client?.Id,
                CreationDate = document.CreationDate,
                LastEditDate = document.LastEditDate,
                CompilationDate = document.CompilationDate,
                Title = document.Title,
                ReadOnly = document.ReadOnly,
            };

            db.Documents.Add(newDocument);

            foreach (var companyDoc in document.Companies)
            {
                CompanyDocument cd = new()
                {
                    IdCompany = companyDoc.Id,
                    IdDocument = document.Id,
                    Present = companyDoc.Present ?? false,
                };

                db.CompanyDocuments.Add(cd);
            }

            foreach (var cat in document.Categories)
            {
                foreach (var q in cat.Questions.Cast<DocumentQuestionModel>())
                {
                    foreach (var cc in q.CurrentChoices)
                    {
                        QuestionAnswered qc = new()
                        {
                            IdDocument = document.Id,
                            IdCurrentChoice = cc.Id,
                            IdQuestionChosen = q.Id
                        };
                        db.QuestionAnswereds.Add(qc);

                        foreach (var rci in cc.ReportedCompanyIds)
                        {
                            ReportedCompany rc = new()
                            {
                                IdCompany = rci,
                                IdDocument = document.Id,
                                IdCurrentChoice = cc.Id,
                                IdQuestionChosen = q.Id
                            };
                            db.ReportedCompanies.Add(rc);
                        }
                    }

                    foreach(var attach in q.Attachments)
                    {
                        var nextAttachId =(db.Attachments.Any() ? db.Attachments.Max(x => x.Id) : 0) + 1; 

                        Attachment attachment = new()
                        {
                            Id = nextAttachId,
                            IdDocument = document.Id,
                            DateTime = attach.Date,
                        };
                        db.Attachments.Add(attachment);

                        AttachmentQuestion attachmentQuestion = new()
                        {
                            IdAttachment = nextAttachId,
                            IdQuestion = q.Id,
                        };
                        db.AttachmentQuestions.Add(attachmentQuestion);
                    }
                }
            }
            await db.SaveChangesAsync();
            documentId = nextId;
        }
        catch (Exception e)
        {
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
                     if (d is not null && CheckLastEdit(d.LastEditDate, document.LastEditDate))
                     {
                       //  d.LastModified = document.LastModified;
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

                         d.IdCategory = elem.IdCategory;
                         d.IdSubject = elem.IdSubject;
                         d.Text = elem.Text;

                         db.QuestionChoices.RemoveRange(db.QuestionChoices.Where(x => x.IdQuestion == elem.Id));
                         foreach(var choice in elem.Choices)
                         {
                             QuestionChoice newqc = new()
                             {
                                 IdChoice = choice.Id,
                                 IdQuestion = elem.Id
                             };
                             db.Add(newqc);
                         }

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
                    // c.Active = false;
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
