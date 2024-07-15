using Shared.Documents;
using TDatabase.Database;
using DB = TDatabase.Database.DbCsclDamicoV2Context;

namespace TDatabase.Queries;

public class ClientDbHelper
{
    public static List<ClientModel> Select(DB db)
    {
        return db.Clients.Where(x => x.Active == true).Select(c => new ClientModel()
        {
            Id = c.Id,
            Name = c.Name,
        }).ToList();
    }

    public static async Task<int> Insert(DB db, ClientModel client)
    {
        var clientId = 0;
        try
        {
            var nextId = (db.Clients.Any() ? db.Clients.Max(x => x.Id) : 0) + 1;
            Client newClient = new()
            {
                Id = nextId,
                Name = client.Name,
                Active = true,
            };
            db.Clients.Add(newClient);
            await db.SaveChangesAsync();
            clientId = nextId;
        } 
        catch (Exception e) 
        {
            Console.WriteLine(e.Message);
        }

        return clientId;
    }

    public static async Task<List<int>> Update(DB db, List<ClientModel> clients)
    {
        List<int> modified = [];
        try
        {
            foreach (var elem in clients)
            {
                var c = db.Clients.Where(x => x.Id == elem.Id).SingleOrDefault();
                if (c is not null)
                {
                    c.Name = elem.Name;
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

    public static async Task<List<int>> Hide(DB db, List<ClientModel> clients)
    {
        List<int> hiddenItems = [];
        try
        {
            foreach(var elem in clients)
            {
                var mc = db.Clients.Where(x => x.Id == elem.Id).SingleOrDefault();
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
