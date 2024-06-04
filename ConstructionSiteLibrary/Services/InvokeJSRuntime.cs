using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConstructionSiteLibrary.Services
{
    public class InvokeJSRuntime
    {
        private readonly IJSRuntime jsRuntime;

        public InvokeJSRuntime(IJSRuntime js)
        {
            jsRuntime = js;
        }

        public async Task OpenNewTab(string page)
        {
            await jsRuntime.InvokeVoidAsync("open", page, "_blank");
        }
    }
}
