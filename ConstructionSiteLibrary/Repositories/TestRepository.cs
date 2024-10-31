using ConstructionSiteLibrary.Managers;
using Shared.ApiRouting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConstructionSiteLibrary.Repositories
{

    /// <summary>
    /// Repository di Test, utilizzato per debug o modifiche in fase di sviluppo
    /// </summary>
    public class TestRepository(HttpManager httpManager)
    {

        private readonly HttpManager _httpManager = httpManager;

        public async Task ApiTest()
        {
            try
            {
                var response = await _httpManager.SendHttpRequest(ApiRouting.Test, "");
                if (response.Code.Equals("0"))
                {

                }
            }
            catch (Exception) { }
        }
    }
}
