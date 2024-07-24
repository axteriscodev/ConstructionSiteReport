using System.Data.Common;
using Shared.Organizations;
using TDatabase.Database;
using DB = TDatabase.Database.DbCsclDamicoV2Context;

namespace TDatabase.Queries;

public class OrganizationDbHelper
{
    public static OrganizationModel Select(DB db, int id)
    {
        return db.Organizations.Where(x => x.Id == id && x.Active == true).Select(o => new OrganizationModel()
        {
        
            Id = o.Id,
            Name = o.Name,
            Address = o.Address ?? "",
            Phone = o.Phone ?? "",
            Active = o.Active,
            Hidden = o.Hidden  
        }).SingleOrDefault();
    }

    public static async Task<int> Update(DB db, OrganizationModel organization)
    {
        try
        {
            var o = db.Organizations.Where(x => x.Id == organization.Id).SingleOrDefault();
            if(o is not null)
            {
                o.Name = organization.Name;
                o.Address = organization.Address;
                o.Phone = organization.Phone;
                o.Active = organization.Active;
                o.Hidden = organization.Hidden;
            }

            if (await db.SaveChangesAsync() > 0)
            {
                return organization.Id;
            }
        }
        catch(Exception)
        {}

        return 0;
    }
}
