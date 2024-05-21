using ConstructionSiteLibrary.Components.Utilities;
using Microsoft.JSInterop;



namespace ConstructionSiteLibrary.Services
{
    public class IndexedDBService
    {
        private const string JS_FILE = "./_content/ConstructionSiteLibrary/indexedDB.js";
        private IJSObjectReference? jsModule;

        public bool DbSupport { get; set; } = false;


        public async Task<bool> Init(IJSRuntime js)
        {
            bool dbSupport;
            try
            {
                jsModule = await js.InvokeAsync<IJSObjectReference>("import", JS_FILE);
                //setto la dimensione corrente dello schermo
                dbSupport = await jsModule.InvokeAsync<bool>("checkDBSupport");
                DbSupport = dbSupport;
            }
            catch (Exception e)
            {
                var ciao = e.Message;
                dbSupport = false;
            }
            return dbSupport;
        }

        public async Task<bool> OpenDB()
        {
            bool result = false;
            if (DbSupport)
            {
                result = await jsModule.InvokeAsync<bool>("openDB");
                Console.WriteLine("CS: " + result);
            }
            return result;
        }


        public async Task<int> Insert(object[] list)
        {
            int result = 0;
            if (DbSupport)
            {
                result = await jsModule.InvokeAsync<int>("inserts", ["choices", list]);
            }
            return result;
        }

        public async Task<List<object>> ReadObjectStore()
        {
            List<object> result = [];
            if (DbSupport)
            {
                result = await jsModule.InvokeAsync<List<object>>("selectMulti", ["choices"]);
            }
            return result;
        }

        public async Task<object?> Read()
        {
            object? result = null ;
            
            if (DbSupport)
            {
               result = await jsModule.InvokeAsync<object>("selectByKey", ["choices", 1]);
            }
            return result;
        }

        public async Task<bool> Delete()
        {
            var result = false;
            if (DbSupport)
            {
                result = await jsModule.InvokeAsync<bool>("deleteRecord", ["choices", 2]);
            }
            return result;
        }
    }
}
