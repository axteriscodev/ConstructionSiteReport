using Shared;
using TDatabase.Database;
using DB = TDatabase.Database.DbCsclAxteriscoContext;

namespace TDatabase.Queries
{
    public class SubjectDbHelper
    {
        public static List<SubjectModel> Select(DB db)
        {
            return db.Subjects.Select(s => new SubjectModel()
            {
                Id = s.Id,
                Text = s.Text,
                Questions = db.Questions.Where(q => q.IdSubject == s.Id).Select(q => new QuestionModel()
                {
                    Id = q.Id,
                    Text = q.Text,
                    Choices = (from qc in db.QuestionChoices
                               join c in db.Choices on qc.IdChoice equals c.Id
                               where qc.IdQuestion == q.Id
                               select new ChoiceModel()
                               {
                                   Id = c.Id,
                                   Tag = c.Tag,
                                   Value = c.Value,
                               }).ToList(),
                }).ToList(),
            }).ToList();
        }

        public static async Task<int> Insert(DB db, SubjectModel sub)
        {
            var subId = 0;
            try
            {
                var nextId = (db.Subjects.Any() ? db.Subjects.Max(x => x.Id) : 0) + 1;
                Subject newSub = new()
                {
                    Id = nextId,
                    Text = sub.Text,
                    Active = true,
                };
                db.Subjects.Add(newSub);
                await db.SaveChangesAsync();
                subId = nextId;
            }
            catch (Exception) { }

            return subId;
        }

        public static async Task<List<int>> Update(DB db, List<SubjectModel> subs)
        {
            List<int> modified = [];
            try
            {
                foreach (var elem in subs)
                {
                    var s = db.Subjects.Where(x => x.Id == elem.Id).SingleOrDefault();
                    if (s is not null)
                    {
                        s.Text = elem.Text;
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

        public static async Task<List<int>> Hide(DB db, List<SubjectModel> subjects)
        {
            List<int> hiddenItems = [];
            try
            {
                foreach (var elem in subjects)
                {
                    var mc = db.Subjects.Where(x => x.Id == elem.Id).SingleOrDefault();
                    if (mc is not null)
                    {
                        mc.Active = false;
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
