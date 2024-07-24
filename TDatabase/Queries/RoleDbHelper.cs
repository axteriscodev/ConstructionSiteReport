using Shared.Organizations;
using TDatabase.Database;
using DB = TDatabase.Database.DbCsclDamicoV2Context;

namespace TDatabase.Queries;

public class RoleDbHelper
{
    public static List<RoleModel> Select(DB db)
    {
        return db.Roles.Where(x => x.Active == true).Select(r => new RoleModel()
        {
            Id = r.Id,
            Name = r.Name,
            IdOrganization = r.IdOrganization,
            DocumentsManagement = r.DocumentsManagement,
            UsersManagement = r.UsersManagement,
        }).ToList();
    }

    public static async Task<int> Insert(DB db, RoleModel role)
    {
        var roleId = 0;
        try
        {
            var nextId = (db.Roles.Any() ? db.Roles.Max(x => x.Id) : 0) + 1;
            Role newRole = new()
            {
                Id = nextId,
                Name = role.Name,
                IdOrganization = role.IdOrganization,
                Active = true,
                Hidden = false,
            };
            db.Roles.Add(newRole);
            await db.SaveChangesAsync();
            roleId = nextId;
        } 
        catch (Exception e) 
        {
            Console.WriteLine(e.Message);
        }

        return roleId;
    }

    public static async Task<List<int>> Update(DB db, List<RoleModel> roles)
    {
        List<int> modified = [];
        try
        {
            foreach (var elem in roles)
            {
                var c = db.Roles.Where(x => x.Id == elem.Id).SingleOrDefault();
                if (c is not null)
                {
                    c.Name = elem.Name;
                    c.DocumentsManagement = elem.DocumentsManagement;
                    c.UsersManagement = elem.UsersManagement;
                }
                if (await db.SaveChangesAsync() > 0)
                {
                    modified.Add(elem.Id);
                }

            }
        }
        catch (Exception) {}

        return modified;
    }

    public static async Task<List<int>> Hide(DB db, List<RoleModel> roles)
    {
        List<int> hiddenItems = [];
        try
        {
            foreach(var elem in roles)
            {
                var mc = db.Roles.Where(x => x.Id == elem.Id).SingleOrDefault();
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
        catch (Exception) {}

        return hiddenItems;
    }
}
