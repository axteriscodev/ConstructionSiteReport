using ConstructionSiteLibrary.Components.Utilities;
using ConstructionSiteLibrary.Model;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Text.Json;



namespace ConstructionSiteLibrary.Services
{

    public class IndexedDBService
    {
        private const string JS_FILE = "./_content/ConstructionSiteLibrary/IndexedDB.js";
        private IJSObjectReference? jsModule;

        public delegate void DbSupportValidated(bool supported);
        public event DbSupportValidated OnDbSupportValidated;

        private bool dbSupport = false;

        public async Task Init(IJSRuntime js)
        {
            if(jsModule is null)
            {
                try
                {
                    jsModule = await js.InvokeAsync<IJSObjectReference>("import", JS_FILE);
                    //setto la dimensione corrente dello schermo
                    dbSupport = await jsModule.InvokeAsync<bool>("checkDBSupport");
                    if (dbSupport)
                    {
                        _ = await OpenDB();
                    }
                }
                catch (Exception)
                {
                    dbSupport = false;
                }
            }
            OnDbSupportValidated.Invoke(dbSupport);
        }

        public async Task<bool> OpenDB()
        {
            bool result = false;
            if (dbSupport)
            {
                result = await jsModule!.InvokeAsync<bool>("openDB");
                Console.WriteLine("CS: " + result);
            }
            return result;
        }

        public async Task<int> Insert(IndexedDBTables table,object[] list)
        {
            var tableName = table.ToString();
            int result = 0;
            if (dbSupport)
            {
                result = await jsModule!.InvokeAsync<int>("inserts", [tableName, list]);
            }
            return result;
        }

        public async Task<string?> ReadObjectStore(IndexedDBTables table)
        {
            string? jsonResponse = null;
            if (dbSupport)
            {
                var results = await jsModule!.InvokeAsync<List<object>>("selectMulti", [table.ToString()]);
                jsonResponse = JsonSerializer.Serialize(results);
            }
            return jsonResponse;
        }

        public async Task<string?> Read(IndexedDBTables table, int id)
        {
            string? jsonResponse = null;
            if (dbSupport)
            {
              var result = await jsModule!.InvokeAsync<object>("selectByKey", [table.ToString(), id]);
                jsonResponse = JsonSerializer.Serialize(result);
            }
            return jsonResponse;
        }

        public async Task<string?> SelectByIndex(IndexedDBTables table, string idx, object value)
        {
            string? jsonResponse = null;
            if (dbSupport)
            {
                var result = await jsModule!.InvokeAsync<object>("selectByIndex", [table.ToString(), idx, value]);
                jsonResponse = JsonSerializer.Serialize(result);
            }
            return jsonResponse;
        }

        public async Task<bool> Delete()
        {
            var result = false;
            if (dbSupport)
            {
                result = await jsModule!.InvokeAsync<bool>("deleteRecord", ["choices", 2]);
            }
            return result;
        }
    }
}
