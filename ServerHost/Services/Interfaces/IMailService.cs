using ConstructionSiteLibrary.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerHost.Services.Interfaces
{
    public interface IMailService
    {
        public Task<bool> SendMail(Mail mail);
    }
}
