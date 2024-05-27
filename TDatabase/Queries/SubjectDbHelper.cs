using Shared;
using TDatabase.Database;
using DB = TDatabase.Database.DbCsclDamicoV2Context;

namespace TDatabase.Queries
{
    public class SubjectDbHelper
    {
        public static List<SubjectModel> Select(DB db)
        {
            return [];
        }

        public static async Task<int> Insert(DB db, SubjectModel sub)
        {
            var subId = 0;
            try
            {
                await db.SaveChangesAsync();

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
                }
            }
            catch (Exception) { }

            return hiddenItems;
        }
    }
}
