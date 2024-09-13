using Shared.Defaults;
using Shared.Organizations;
using Shared.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDatabase.Database;
using DB = TDatabase.Database.DbCsclDamicoV2Context;

namespace TDatabase.Queries
{
    public class UserDbHelper
    {

        public static User? SelectUserFromEmail(DB db, string email)
        {
            return db.Users.Where(usr => usr.Email.Equals(email)).SingleOrDefault();
        }

        public static List<UserModel> Select(DB db, int organizationId)
        {
            return db.Users.Where(usr => usr.IdOrganization == organizationId)
                           .Select(usr => new UserModel()
                           {
                               Id = usr.Id,
                               Name = usr.Name ?? "",
                               Surname = usr.Surname ?? "",
                               Email = usr.Email ?? "",
                               Phone = usr.Phone ?? "",
                               Role = db.Roles.Where(role => role.Id == usr.IdRole).Select(role => new RoleModel()
                               {
                                   Id = role.Id,
                                   Name = role.Name,
                                   DocumentsManagement = role.DocumentsManagement,
                                   UsersManagement = role.UsersManagement,
                               }).SingleOrDefault() ?? new(),
                           }).ToList();
        }

        public static async Task<int> Insert(DB db, UserModel usr, PasswordModel pwd)
        {
            var id = 0;
            try
            {
                var nextId = (db.Users.Any() ? db.Users.Max(x => x.Id) : 0) + 1;

                User newUser = new()
                {
                    Id = nextId,
                    Name = usr.Name,
                    Surname = usr.Surname,
                    Email = usr.Email,
                    Phone = usr.Phone,
                    Password = pwd.CryptedPassword,
                    Salt = pwd.Salt,
                    IdOrganization = usr.Organization.Id,
                    IdRole = usr.Role.Id,
                    Active = true,

                };
                db.Users.Add(newUser);
                await db.SaveChangesAsync();
                id = nextId;
            }
            catch (Exception) { }

            return id;
        }

        public static async Task<List<int>> Update(DB db, List<UserModel> usrs)
        {
            List<int> modified = [];
            try
            {
                foreach (var usr in usrs)
                {
                    var user = db.Users.Where(x => x.Id == usr.Id).SingleOrDefault();
                    if (user is not null)
                    {
                        user.Name = usr.Name;
                        user.Surname = usr.Surname;
                        user.Email = usr.Email;
                        user.Phone = usr.Phone;
                        user.IdRole = usr.Role.Id;
                        if (await db.SaveChangesAsync() > 0)
                        {
                            modified.Add(usr.Id);
                        }
                    }
                }
            }
            catch (Exception e) {
                e.ToString();
             }
            
            return modified;
        }

        public static async Task<List<int>> Hide(DB db, List<UserModel> usrs)
        {
            List<int> hiddenItems = [];
            try
            {
                foreach (var usr in usrs)
                {
                    var u = db.Users.Where(x => x.Id == usr.Id).SingleOrDefault();
                    if (u is not null)
                    {
                        u.Active = false;
                        if (await db.SaveChangesAsync() > 0)
                        {
                            hiddenItems.Add(u.Id);
                        }
                    }
                }
            }
            catch (Exception) { }

            return hiddenItems;
        }

        public static UserModel CreateUserModel(DB db, User user)
        {
            return new()
            {
                Id = user.Id,
                Name = user.Name ?? "",
                Surname = user.Surname ?? "",
                Email = user.Email,
                Phone = user.Phone ?? "",
                Organization = db.Organizations.Where(org => org.Id == user.IdOrganization).Select(org => new OrganizationModel()
                {
                    Id = org.Id,
                    Name = org.Name,
                    Address = org.Address ?? "",
                }).SingleOrDefault() ?? new() { Id = user.Id },
                Role = db.Roles.Where(r => r.Id == user.IdRole).Select(r => new RoleModel()
                {
                    Id = r.Id,
                    Name = r.Name,
                    DocumentsManagement = r.DocumentsManagement,
                    UsersManagement = r.UsersManagement
                }).SingleOrDefault() ?? new() { Id = user.IdRole },
            };
        }
    }
}
