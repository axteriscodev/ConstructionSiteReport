using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using Shared;
using TDatabase.Database;
using DB = TDatabase.Database.DbCsclAxteriscoContext;

namespace TDatabase.Queries
{
    public class QuestionDbHelper
    {
        public static List<QuestionModel> Select(DB db, int idCategory = 0, int idSubject = 0)
        {
            var questions = db.Questions.AsQueryable();

            if (idSubject > 0)
            {
                questions = from q in questions
                            where q.IdSubject == idSubject
                            select q;
            }
            if (idCategory > 0)
            {
                questions = from q in questions
                            where q.IdCategory == idCategory
                            select q;
            }

            var list = questions.Where(x=>x.Active == true).Select(x => new QuestionModel()
            {
                Id = x.Id,
                Text = x.Text,
                IdCategory = x.IdCategory,
                Choices = (from qc in db.QuestionChoices
                           from c in db.Choices
                           where qc.IdQuestion == x.Id
                           && c.Id == qc.IdChoice
                           select new ChoiceModel()
                           {
                               Id = c.Id,
                               Tag = c.Tag,
                               Value = c.Value,
                           }).ToList()
            }).ToList();
            return list;
        } 

        public static async Task<int> Insert(DB db, QuestionModel question)
        {
            var questionId = 0;
            try
            {
                var nextId = (db.Questions.Any() ? db.Questions.Max(x => x.Id) : 0) + 1;
                Question newQuestion = new()
                {
                    Id = nextId,
                    Text = question.Text,
                    IdSubject = question.IdSubject,
                    IdCategory = question.IdCategory,
                    Active = true
                };
                db.Questions.Add(newQuestion);

                foreach(var c in question.Choices)
                {
                    QuestionChoice qc = new()
                    {
                        IdChoice = c.Id,
                        IdQuestion = nextId
                    };
                    db.QuestionChoices.Add(qc);
                }
                await db.SaveChangesAsync();
                questionId = nextId;
            }
            catch (Exception) { }

            return questionId;
        }

        public static async Task<List<int>> Update(DB db, List<QuestionModel> questions)
        {
            List<int> modified = [];
            try
            {
                foreach (var elem in questions)
                {
                    var q = db.Questions.Where(x=>x.Id == elem.Id).FirstOrDefault();
                    if (q is not null)
                    {
                        q.IdCategory = elem.IdCategory;
                        q.IdSubject = elem.IdSubject;
                        q.Text = elem.Text;

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

        public static async Task<List<int>> Hide(DB db, List<QuestionModel> questions)
        {
            List<int> hiddenItems = [];
            try
            {
                foreach (var elem in questions)
                {
                    var c = db.Questions.Where(x => x.Id == elem.Id).SingleOrDefault();
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
