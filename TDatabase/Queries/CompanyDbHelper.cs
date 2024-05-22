using Shared;
using TDatabase.Database;
using DB = TDatabase.Database.DbCsclAxteriscoContext;

namespace TDatabase.Queries;

public class CompanyDbHelper
{
    public static List<CompanyModel> Select(DB db)
    {
        return db.Companies.Where(x => x.Active == true).Select(c => new CompanyModel()
        {
            Id = c.Id,
            Name = c.Name,
            Address = c.Address ?? "",
            VatCode = c.Vatcode,
        }).ToList();
    }

    public static async Task<int> Insert(DB db, CompanyModel company)
    {
        var companyId = 0;
        try
        {
            var nextId = (db.Companies.Any() ? db.Companies.Max(x => x.Id) : 0) + 1;
            Company newCompany = new()
            {
                Id = companyId,
                Name = company.Name,
                Address = company.Address,
                Vatcode = company.VatCode,
            };
            db.Companies.Add(newCompany);
            await db.SaveChangesAsync();
            companyId = nextId;
        }
        catch (Exception) { }

        return companyId;
    }

    public static async Task<List<int>> Update(DB dB, List<CompanyModel> companies)
    {
        List<int> modified = [];
        try
        {
            foreach (var elem in companies)
            {
                var c = dB.Companies.Where(x => x.Id == elem.Id).SingleOrDefault();
                if (c is not null)
                {
                    c.Name = elem.Name;
                    c.Address = elem.Address;
                    c.Vatcode = elem.VatCode;
                    if (await dB.SaveChangesAsync() > 0)
                    {
                        modified.Add(elem.Id);
                    }
                }
            }
        }
        catch (Exception) { }

        return modified;
    }

    public static async Task<List<int>> Hide(DB db, List<CompanyModel> companies)
    {
        List<int> hiddenItems = [];
            try
            {
                foreach (var elem in companies)
                {
                    var mc = db.Companies.Where(x => x.Id == elem.Id).SingleOrDefault();
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
