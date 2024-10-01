namespace TDatabase.Queries;

using System.Diagnostics;
using Shared.Documents;
using TDatabase.Database;
using DB = TDatabase.Database.DbCsclDamicoV2Context;


public class ConstructorSiteDbHelper
{

    #region Query principali cantiere

    public static List<ConstructorSiteModel> Select(DB db, int organizationId, int idConstructorSite = 0)
    {
        var constructorSites = db.ConstructorSites.AsQueryable();

        if (idConstructorSite > 0)
        {
            constructorSites = constructorSites.Where(x =>x.IdOrganization == organizationId && x.Id == idConstructorSite);
        }

        return constructorSites.Where(x => x.IdOrganization == organizationId).Select(x => new ConstructorSiteModel()
        {
            Id = x.Id,
            Name = x.Name,
            JobDescription = x.JobDescription ?? "",
            Address = x.Address ?? "",
            StartDate = x.StartDate ?? DateTime.Now,
            EndDate = x.EndDate,
            Client = db.Clients.Where(c => c.Id == x.IdClient).Select(nc => new ClientModel()
            {
                Id = nc.Id,
                Name = nc.Name,
            }).FirstOrDefault() ?? new()
        }).ToList();
    }

    public static async Task<int> Insert(DB db, ConstructorSiteModel constructorSite, int organizationId)
    {
        var siteId = 0;
        try
        {
            var nextId = (db.ConstructorSites.Any() ? db.ConstructorSites.Max(x => x.Id) : 0) + 1;
            ConstructorSite newConstructorSite = new()
            {
                Id = nextId,
                Name = constructorSite.Name,
                JobDescription = constructorSite.JobDescription,
                Address = constructorSite.Address,
                StartDate = constructorSite.StartDate,
                EndDate = constructorSite.EndDate,
                IdClient = constructorSite.Client.Id > 0 ? constructorSite.Client.Id : null,
                IdOrganization = organizationId
            };
            //associo le aziende al cantiere
            AddCompaniesToConstructionSite(db, constructorSite.Companies, constructorSite.Id);

            db.ConstructorSites.Add(newConstructorSite);
            await db.SaveChangesAsync();
            siteId = nextId;
        }
        catch (Exception) { }

        return siteId;
    }

    public static async Task<List<int>> Update(DB db, List<ConstructorSiteModel> constructorSites)
    {
        List<int> modified = [];
        try
        {
            foreach (var elem in constructorSites)
            {
                var m = db.ConstructorSites.Where(x => x.Id == elem.Id).SingleOrDefault();
                if (m is not null)
                {
                    m.Name = elem.Name;
                    m.Address = elem.Address;
                    m.StartDate = elem.StartDate;
                    m.EndDate = elem.EndDate;
                    m.IdSico = elem.IdSico;
                    m.IdSicoInProgress = elem.IdSicoInProgress;
                    m.PreliminaryNotificationStart = elem.PreliminaryNotificationStartDate;
                    m.PreliminaryNotificationInProgress = elem.PreliminaryNotificationInProgress;
                    if (elem.Client is not null && elem.Client.Id > 0)
                    {
                        m.IdClient = elem.Client.Id;
                    }
                    m.JobDescription = elem.JobDescription;
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

    public static async Task<List<int>> Hide(DB db, List<ConstructorSiteModel> constructorSites)
    {
        List<int> hiddenItems = [];
            try
            {
                foreach (var elem in constructorSites)
                {
                    var mc = db.ConstructorSites.Where(x => x.Id == elem.Id).SingleOrDefault();
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

    #endregion

    #region Associazione Cantiere ed Aziende 


    /// <summary>
    /// Query parziale (non viene eseguito il salvataggio) per associare le anziede al cantiere
    /// </summary>
    /// <param name="db"></param>
    /// <param name="companies"></param>
    /// <param name="idContructorSite"></param>
    public static void AddCompaniesToConstructionSite(DB db, List<CompanyModel> companies, int idContructorSite)
    {
        foreach (var comp in companies)
        {
            var select = db.CompanyConstructorSites.Where(x => x.IdCompany == comp.Id && x.IdConstructorSite == idContructorSite).SingleOrDefault();
            if (select is null)
            {
                var CompConstruct = new CompanyConstructorSite()
                {
                    IdCompany = comp.Id,
                    IdConstructorSite = idContructorSite,
                    JobsDescription = comp.JobsDescriptions,
                    SubcontractedBy = comp.SubcontractedBy,
                };
                db.CompanyConstructorSites.Add(CompConstruct);
            }
        }
    }

    /// <summary>
    /// Query parziale (non viene eseguito il salvataggio) per associare una singola azienda al cantiere
    /// </summary>
    /// <param name="db"></param>
    /// <param name="companies"></param>
    /// <param name="idContructorSite"></param>
    public static void AddCompanyToConstructionSite(DB db, CompanyModel company, int idContructorSite)
    {
        var select = db.CompanyConstructorSites.Where(x => x.IdCompany == company.Id && x.IdConstructorSite == idContructorSite).SingleOrDefault();
        if (select is null)
        {
            var CompConstruct = new CompanyConstructorSite()
            {
                IdCompany = company.Id,
                IdConstructorSite = idContructorSite,
                JobsDescription = company.JobsDescriptions,
                SubcontractedBy = company.SubcontractedBy,
            };
            db.CompanyConstructorSites.Add(CompConstruct);
        }
    }

    #endregion
}
