using Microsoft.EntityFrameworkCore.Query.Internal;
using Shared.Documents;
using System.Reflection.Emit;
using TDatabase.Database;
using DB = TDatabase.Database.DbCsclDamicoV2Context;

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
                        CSE = d.Cse,
                        DraftedIn = d.DraftedIn,
                        CompletedIn = d.CompletedIn,
                        MeteoCondition = (from m in db.MeteoConditions
                                          where d.IdMeteo == m.Id
                                          select new MeteoConditionModel()
                                          {
                                              Id = m.Id,
                                              Description = m.Description,
                                          }).SingleOrDefault(),
                        Categories = (from r in (from qc in db.QuestionChosens
                                                 where qc.IdTemplate == d.IdTemplate
                                                 join q in db.Questions on qc.IdQuestion equals q.Id
                                                 select new { q.IdCategory }).Distinct()

                                      from c in db.Categories
                                      where c.Id == r.IdCategory
                                      orderby c.Order
                                      select new DocumentCategoryModel()
                                      {
                                          Id = r.IdCategory,
                                          Text = c.Text,
                                          Order = c.Order,
                                          Questions = (from tc in db.Templates
                                                       from qc in db.QuestionChosens
                                                       from q in db.Questions
                                                       where tc.Id == d.IdTemplate
                                                       && qc.IdTemplate == tc.Id
                                                       && q.Id == qc.IdQuestion
                                                       && q.IdCategory == c.Id
                                                       orderby qc.Order
                                                       select new DocumentQuestionModel()
                                                       {
                                                           Id = qc.IdQuestion,
                                                           Text = q.Text,
                                                           Order = qc.Order,
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
                                                                              Path = att.FilePath ?? "",
                                                                          }).ToList(),
                                                       }).ToList()
                                      }).ToList(),
                        ConstructorSite = (from cs in db.ConstructorSites
                                           where cs.Id == d.IdConstructorSite
                                           select new ConstructorSiteModel()
                                           {
                                               Id = cs.Id,
                                               JobDescription = cs.JobDescription ?? "",
                                               StartDate = cs.StartDate ?? DateTime.Now,
                                               Address = cs.Address ?? "",
                                               Client = (from cl in db.Clients
                                                         where cl.Id == cs.IdClient
                                                         select new ClientModel()
                                                         {
                                                             Id = cl.Id,
                                                             Name = cl.Name
                                                         }).SingleOrDefault() ?? new(),
                                           }).SingleOrDefault() ?? new(),
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
                                         CompanyName = comp.CompanyName ?? "",
                                         Address = comp.Address ?? "",
                                         VatCode = comp.Vatcode ?? "",
                                         Present = compDoc.Present
                                     }).ToList(),
                        Notes = (from n in db.Notes
                                 where n.IdDocument == d.Id
                                 && n.Active == true
                                 orderby n.Id
                                 select new NoteModel()
                                 {
                                     Id = n.Id,
                                     Text = n.Text ?? "",
                                     CompanyListIds = (from cn in db.CompanyNotes
                                                       where cn.IdNote == n.Id
                                                       select cn.IdCompany).ToList(),
                                     Attachments = (from an in db.AttachmentNotes
                                                    from att in db.Attachments
                                                    where an.IdNote == n.Id
                                                    && att.Id == an.IdAttachment
                                                    select new AttachmentModel()
                                                    {
                                                        Id = att.Id,
                                                        Name = att.Name,
                                                        Date = att.DateTime,
                                                        Path = att.FilePath ?? "",
                                                    }).ToList(),
                                 }).ToList(),
                    }).ToList();

        return docs;
    }


    public static List<DocumentModel> SelectFromSite(DB db, int siteId)
    {
        var docs = (from d in db.Documents
                    where d.IdConstructorSite == siteId
                    join cs in db.ConstructorSites on d.IdConstructorSite equals cs.Id
                    select new DocumentModel()
                    {
                        Id = d.Id,
                        Title = d.Title,
                        CreationDate = d.CreationDate ?? DateTime.Now,
                        CompilationDate = d.CompilationDate,
                        LastEditDate = d.LastEditDate,
                        ConstructorSite = new()
                        {
                            Id = cs.Id,
                            JobDescription = cs.JobDescription,
                            StartDate = cs.StartDate ?? DateTime.Now,
                            Address = cs.Address,
                            Client = (from cl in db.Clients
                                      where cl.Id == cs.IdClient
                                      select new ClientModel()
                                      {
                                          Id = cl.Id,
                                          Name = cl.Name
                                      }).SingleOrDefault() ?? new(),
                        },
                        Client = (from cl in db.Clients
                                  where cl.Id == d.IdClient
                                  select new ClientModel()
                                  {
                                      Id = cl.Id,
                                      Name = cl.Name
                                  }).SingleOrDefault(),
                    }).ToList();

        return docs;
    }

    public static async Task<int> Insert(DB db, DocumentModel document)
    {
        //valore restituito dalla funzione
        var documentId = 0;
        try
        {
            var nextId = (db.Documents.Any() ? db.Documents.Max(x => x.Id) : 0) + 1;
            // inserisco i dati principali del documento
            Document newDocument = new()
            {
                Id = nextId,
                IdConstructorSite = document.ConstructorSite.Id,
                IdClient = document.Client?.Id > 0 ? document.Client?.Id : null,
                IdTemplate = document.IdTemplate,
                CreationDate = document.CreationDate,
                LastEditDate = document.LastEditDate,
                CompilationDate = document.CompilationDate,
                Title = document.Title,
                ReadOnly = document.ReadOnly,
                Cse = document.CSE,
                DraftedIn = document.DraftedIn,
                CompletedIn = document.CompletedIn,
                IdMeteo = document.MeteoCondition?.Id,
            };

            db.Documents.Add(newDocument);

            //aggiungo le compagnia associate al documento
            foreach (var companyDoc in document.Companies)
            {
                CompanyDocument cd = new()
                {
                    IdCompany = companyDoc.Id,
                    IdDocument = nextId,
                    Present = companyDoc.Present ?? false,
                };

                db.CompanyDocuments.Add(cd);
            }

            var nextAttachId = (db.Attachments.Any() ? db.Attachments.Max(x => x.Id) : 0) + 1;

            //aggiungo le risposte compilate del documento
            foreach (var cat in document.Categories)
            {
                foreach (var q in cat.Questions)
                {
                    foreach (var cc in q.CurrentChoices)
                    {
                        QuestionAnswered qc = new()
                        {
                            IdDocument = nextId,
                            IdCurrentChoice = cc.Id,
                            IdQuestionChosen = q.Id
                        };
                        db.QuestionAnswereds.Add(qc);
                        //aggiungo le compagnie riportate delle varie risposte se ci sono
                        foreach (var rci in cc.ReportedCompanyIds)
                        {
                            ReportedCompany rc = new()
                            {
                                IdCompany = rci,
                                IdDocument = nextId,
                                IdCurrentChoice = cc.Id,
                                IdQuestionChosen = q.Id
                            };
                            db.ReportedCompanies.Add(rc);
                        }
                    }
                    foreach (var attach in q.Attachments)
                    {

                        Attachment attachment = new()
                        {
                            Id = nextAttachId,
                            IdDocument = nextId,
                            DateTime = attach.Date,
                        };
                        db.Attachments.Add(attachment);

                        AttachmentQuestion attachmentQuestion = new()
                        {
                            IdAttachment = nextAttachId,
                            IdQuestion = q.Id,
                        };
                        db.AttachmentQuestions.Add(attachmentQuestion);
                        nextAttachId++;
                    }
                }
            }
            //carico le note
            foreach (var n in document.Notes)
            {
                var nextNoteId = (db.Notes.Any() ? db.Attachments.Max(x => x.Id) : 0) + 1;
                var note = new Note()
                {
                    Id = nextNoteId,
                    IdDocument = nextId,
                    Text = n.Text,
                };
                //inserisco gli allegati della nota se presenti
                foreach (var a in n.Attachments)
                {
                    Attachment attachment = new()
                    {
                        Id = nextAttachId,
                        IdDocument = nextId,
                        DateTime = a.Date,
                    };
                    db.Attachments.Add(attachment);

                    AttachmentNote attachmentNote = new()
                    {
                        IdAttachment = nextAttachId,
                        IdNote = nextNoteId,
                    };
                    db.AttachmentNotes.Add(attachmentNote);
                    nextAttachId++;
                }
                //inserisco le compagnie associate alle note se presenti
                foreach (var c in n.CompanyListIds)
                {
                    var companyNote = new CompanyNote()
                    {
                        IdCompany = c,
                        IdNote = nextNoteId
                    };
                }
                nextNoteId++;
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
                var doc = db.Documents.Where(x => x.Id == document.Id).SingleOrDefault();
                if (doc is not null)
                {
                    db.Documents.Remove(doc);
                    await Insert(db, document);
                }
            }

        }
        catch (Exception e)
        {
            var ex = e.Message;
        }

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

    public static List<MeteoConditionModel> SelectMeteo(DB db, int idMeteo = 0)
    {
        var meteoConditions = db.MeteoConditions.AsQueryable();

        var meteoConditionsList = (from m in meteoConditions
                                   orderby m.Id
                                   select new MeteoConditionModel()
                                   {
                                       Id = m.Id,
                                       Description = m.Description,
                                   }).ToList();

        return meteoConditionsList;
    }

    private static bool CheckLastEdit(DateTime? oldEdit, DateTime newEdit)
    {
        return oldEdit is null || oldEdit < newEdit;
    }
}



/*
 

        var nextAttachId = (db.Attachments.Any() ? db.Attachments.Max(x => x.Id) : 0) + 1;
 
            foreach (var document in documents)
            {
                var doc = db.Documents.Where(x => x.Id == document.Id).SingleOrDefault();
                //aggiorno i dati base del documento
                if (doc is not null)
                {
                    doc.LastEditDate = document.LastEditDate;
                    doc.CompilationDate = document.CompilationDate;
                    doc.Title = document.Title;
                    doc.ReadOnly = document.ReadOnly;

                    //aggiungo le compagnia associate al documento
                    foreach (var companyDoc in document.Companies)
                    {
                        var comp = db.CompanyDocuments.Where(x => x.IdDocument == document.Id && x.IdCompany == companyDoc.Id).SingleOrDefault();
                        if (comp is not null)
                        {
                            comp.Present = companyDoc.Present ?? false;
                        }
                        else
                        {
                            CompanyDocument cd = new()
                            {
                                IdCompany = companyDoc.Id,
                                IdDocument = document.Id,
                                Present = companyDoc.Present ?? false,
                            };

                            db.CompanyDocuments.Add(cd);
                        }
                    }

                    foreach (var cat in document.Categories)
                    {
                        foreach (var q in cat.Questions)
                        {
                            var responses = db.QuestionAnswereds.Where(x => x.IdDocument == document.Id
                                                                       && x.IdQuestionChosen == q.Id).ToList();
                            foreach (var cc in q.CurrentChoices)
                            {
                                var current = responses.Where(x => x.IdDocument == document.Id
                                                                         && x.IdQuestionChosen == q.Id
                                                                         && x.IdCurrentChoice == cc.Id).SingleOrDefault();
                                //se è null creo la nuova risposta
                                if (current is null)
                                {
                                    current = new()
                                    {
                                        IdDocument = document.Id,
                                        IdCurrentChoice = cc.Id,
                                        IdQuestionChosen = q.Id
                                    };
                                    db.QuestionAnswereds.Add(current);
                                    //aggiungo le compagnie riportate delle varie risposte se ci sono
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
                                else
                                {
                                    var oldReported = db.ReportedCompanies.Where(x => x.IdDocument == document.Id
                                                                                 && x.IdQuestionChosen == q.Id
                                                                                 && x.IdCurrentChoice == cc.Id).ToList();
                                    foreach(var idcomp in cc.ReportedCompanyIds)
                                    {
                                        var repComp = oldReported.Where(x => x.IdCompany == idcomp).SingleOrDefault();
                                        if(repComp is null)
                                        {
                                            ReportedCompany rc = new()
                                            {
                                                IdCompany = idcomp,
                                                IdDocument = document.Id,
                                                IdCurrentChoice = cc.Id,
                                                IdQuestionChosen = q.Id
                                            };
                                            db.ReportedCompanies.Add(rc);
                                        } else
                                        {
                                            oldReported.Remove(repComp);
                                        }
                                    }
                                    if(oldReported.Count > 0)
                                    {
                                        db.ReportedCompanies.RemoveRange(oldReported);
                                    }

                                    responses.Remove(current);
                                }
                            }

                            if (responses.Count > 0)
                            {
                                db.QuestionAnswereds.RemoveRange(responses);
                            }

                            var oldAttach = (from a in db.Attachments
                                             join aq in db.AttachmentQuestions on a.Id equals aq.IdAttachment
                                             where a.IdDocument == document.Id
                                             && aq.IdQuestion == q.Id
                                             select aq).ToList();

                            foreach (var attach in q.Attachments)
                            {

                                var att = oldAttach.Where(x => x.IdAttachment == attach.Id).SingleOrDefault();
                                if(att is null)
                                {
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
                                    nextAttachId++;
                                } else
                                {
                                    oldAttach.Remove(att);
                                }
                            }
                            if(oldAttach.Count > 0)
                            {
                                db.AttachmentQuestions.Where
                            }
                        }
                    }

                }
            }
 
 */