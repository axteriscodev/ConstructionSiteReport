using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Organizations
{
    public class RoleModel
    {
        public int Id { get; set; }
        public int IdOrganization { get; set; }
        public string Name { get; set; } = "";
        public bool DocumentsManagement { get;set; }
        public bool UsersManagement { get;set; }
    }
}
