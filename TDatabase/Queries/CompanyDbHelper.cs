using Shared.Documents;
using TDatabase.Database;
using DB = TDatabase.Database.DbCsclDamicoV2Context;

namespace TDatabase.Queries;

public class CompanyDbHelper
{
    public static List<CompanyModel> Select(DB db,int organizationId, int companyId = 0)
    {
        var companySelect = db.Companies.AsQueryable();

        if(companyId > 0)
        {
            companySelect = companySelect.Where(x => x.IdOrganization == organizationId && x.Id == companyId && x.Active == true);
        }

        return companySelect.Where(x => x.IdOrganization == organizationId && x.Active == true).Select(c => new CompanyModel()
        {
            Id = c.Id,
            CompanyName = c.CompanyName ?? "",
            SelfEmployedName = c.SelfEmployedName ?? "",
            Address = c.Address ?? "",
            TaxId = c.TaxId ?? "",
            VatCode = c.Vatcode ?? "",
            Phone = c.Phone ?? "",
            Email = c.Email ?? "",
            Pec = c.Pec ?? "",
            ReaNumber = c.ReaNumber ?? "",
            WorkerWelfareFunds = c.WorkerWelfareFunds ?? "",
            Ccnl = c.Ccnl ?? "",
            InpsId = c.InpsId ?? "",
            InailId = c.InailId ?? "",
            InailPat = c.InailPat ?? "",
            JobsDescriptions = c.JobsDescriptions ?? "",
        }).ToList();
    }

    public static async Task<int> Insert(DB db, CompanyModel company, int organizationId)
    {
        var companyId = 0;
        try
        {
            var nextId = (db.Companies.Any() ? db.Companies.Max(x => x.Id) : 0) + 1;
            Company newCompany = new()
            {
                Id = nextId,
                CompanyName = company.CompanyName ?? "",
                SelfEmployedName = company.SelfEmployedName ?? "",
                Address = company.Address ?? "",
                TaxId = company.TaxId ?? "",
                Vatcode = company.VatCode ?? "",
                Phone = company.Phone ?? "",
                Email = company.Email ?? "",
                Pec = company.Pec ?? "",
                ReaNumber = company.ReaNumber ?? "",
                WorkerWelfareFunds = company.WorkerWelfareFunds ?? "",
                Ccnl = company.Ccnl ?? "",
                InpsId = company.InpsId ?? "",
                InailId = company.InailId ?? "",
                InailPat = company.InailPat ?? "",
                JobsDescriptions = company.JobsDescriptions ?? "",
                Active = true,
                IdOrganization = organizationId
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
                    c.CompanyName = elem.CompanyName;
                    c.SelfEmployedName = elem.SelfEmployedName;
                    c.Address = elem.Address;
                    c.TaxId = elem.TaxId;
                    c.Vatcode = elem.VatCode;
                    c.Phone = elem.Phone;
                    c.Email = elem.Email;
                    c.Pec = elem.Pec;
                    c.ReaNumber = elem.ReaNumber;
                    c.WorkerWelfareFunds = elem.WorkerWelfareFunds;
                    c.Ccnl = elem.Ccnl;
                    c.InpsId = elem.InpsId;
                    c.InailId = elem.InailId;
                    c.InailPat = elem.InailPat;
                    c.JobsDescriptions = elem.JobsDescriptions;
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
