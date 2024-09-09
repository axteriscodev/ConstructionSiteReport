using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Documents;

namespace Shared.Organizations
{
    public class UserModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Surname { get; set; } = "";
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
        public string Phone { get; set; } = "";
        public OrganizationModel Organization { get; set; } = new();
        public RoleModel Role { get; set; } = new();    
        public List<AttachmentModel> attachments { get; set; } = [];
        public bool Active { get; set; }
        public bool Hidden { get; set; }
    }
}
