using AXT_WebComunication.WebRequest;
using AXT_WebComunication.WebResponse;
using ConstructionSiteLibrary.Managers;
using ConstructionSiteLibrary.Services;
using Shared.ApiRouting;
using Shared.Documents;
using Shared.Login;
using Shared.Organizations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;


namespace ConstructionSiteLibrary.Repositories
{
    public class UserRepository(HttpManager httpManager, AppAuthenticationStateProvider appAuth)
    {

        private readonly HttpManager _httpManager = httpManager;

        private readonly AppAuthenticationStateProvider _appAuth = appAuth;


        public async Task<AXT_WebResponse> Login(UserLoginRequest rq)
        {
            var response = await _httpManager.SendHttpRequest(ApiRouting.Login, rq);
            return response;
        }

        public async Task<AXT_WebResponse> UpdateUsers(List<UserModel> rq)
        {
            var response = await _httpManager.SendHttpRequest(ApiRouting.UpdateUsers, rq);

            if(response.Code.Equals("0"))
            {
                var jwt = response.Content.ToString();
                if(!string.IsNullOrEmpty(jwt))
                {
                     await _appAuth.SaveAuthenticationAsync(jwt, true);
                }
               
            }
           
            return response;
        }
    }
}
