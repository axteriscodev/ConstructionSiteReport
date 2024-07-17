using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConstructionSiteLibrary.Interfaces
{
    public interface ICameraService
    {
        public Task<string> OpenCamera();

        public Task OpenDocuments();
    }
}
