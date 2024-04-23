using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using Shared;
using TDatabase.Database;
using DB = TDatabase.Database.DbCsclDamicoContext;

namespace TDatabase.Queries
{
    public class QuestionDbHelper
    {
        public static List<QuestionModel> Select(DB db, int idSubCategory, int idMacroCategory)
        {
            var questions = db.Questions.AsQueryable();

            if (idMacroCategory > 0)
            {
                questions = from q in questions
                            where q.IdMacroCategory == idMacroCategory
                            select q;
            }
            if (idSubCategory > 0)
            {
                questions = from q in questions
                            where q.IdSubCategory == idSubCategory
                            select q;
            }

            var list = questions.Where(x=>x.Active == true).Select(x => new QuestionModel()
            {
                Id = x.Id,
                Text = x.Text,
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

        public static async Task<int> Insert(DB db, QuestionModel question, SubCategory sub)
        {
            var questionId = 0;
            try
            {
                var nextId = (db.Questions.Any() ? db.Questions.Max(x => x.Id) : 0) + 1;
                Question newQuestion = new()
                {
                    Id = nextId,
                    Text = question.Text,
                    IdSubCategory = sub.Id,
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
                        //q.IdSubCategory = elem.IdSubCategory;
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
