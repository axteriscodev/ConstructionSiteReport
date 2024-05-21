using System.Diagnostics;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using AXT_WebComunication.WebResponse;

namespace ConstructionSiteLibrary.Managers;

public class HttpManager(HttpClient client)
{

    /// <summary>
    /// Oggetto HttpClient che esegue la richiesta HTTPS
    /// </summary>
    private readonly HttpClient httpClient = client;

    #region Funzioni Http

    /// <summary>
    /// non usato al momento
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public async Task SendHttpGet(string path)
    {
        var url = GetServerBaseUrl() + path;
        var resp = await httpClient.GetAsync(url);
    }

    /// <summary>
    /// Metodo che effettua la chiamata HTTP/HTTPS (POST) verso il server
    /// per eseguire gli upload di file (l'invio è diverso rispetto al metodo SendHttpRequest)
    /// </summary>
    /// <param name="path">metodo da richiamare sul server</param>
    /// <param name="content">il contenuto da inviare nella richiesta</param>
    /// <returns>WebResponse</returns>
    public async Task<AXT_WebResponse> SendHttpUploadRequest(string path, HttpContent? content)
    {
        AXT_WebResponse webResponse;
        var url = GetServerBaseUrl() + path;
        var response = await httpClient.PostAsync(url, content);
        if (response is not null)
        {
            webResponse = await ProcessServerResponse(response);
        }
        else
        {
            webResponse = new(StatusResponse.GetStatus(Status.NO_RESPONSE), "");
        }
        return webResponse;
    }

    /// <summary>
    /// Metodo che effettua la chiamata HTTP/HTTPS (POST) verso il server
    /// </summary>
    /// <param name="path">metodo da richiamare sul server</param>
    /// <param name="content">il contenuto da inviare nella richiesta</param>
    /// <returns>WebResponse</returns>
    public async Task<AXT_WebResponse> SendHttpRequest(string path, object content)
    {
        AXT_WebResponse webResponse;
        var url = GetServerBaseUrl() + path;
        var watch = Stopwatch.StartNew();
        var response = await httpClient.PostAsJsonAsync(url, content);
        Console.WriteLine("tempo chiamata: " + watch.ElapsedMilliseconds + " ms");
        if (response is not null)
        {
            webResponse = await ProcessServerResponse(response);
        }
        else
        {
            webResponse = new(StatusResponse.GetStatus(Status.NO_RESPONSE), "");
        }
        return webResponse;
    }

    /// <summary>
    /// Metodo privato per analizzare la risposta del server, se HTTP STATUS = 200
    /// allora crea la WebResponse dal json ricevuto dal server, in caso di errore HTTP
    /// crea una WebResponse ad-hoc
    /// </summary>
    /// <param name="response">la risposta ricevuta dal server</param>
    /// <returns>WebResponse</returns>
    protected static async Task<AXT_WebResponse> ProcessServerResponse(HttpResponseMessage response)
    {
        AXT_WebResponse webResponse;
        //Se status Code 200 deserializzo la risposta del server
        if (response.StatusCode == HttpStatusCode.OK)
        {
            string json = await response.Content.ReadAsStringAsync();
            //Console.WriteLine(json);
            webResponse = JsonSerializer.Deserialize<AXT_WebResponse>(json) ?? new();
        }
        //altrimenti se HTTP error creo una risposta per HTTP ERROR
        else
        {
            webResponse = new(StatusResponse.GetStatus(Status.HTTP_ERROR), "")
            {
                Message = "HTTP STATUS: " + ((int)response.StatusCode).ToString(),
                Content = response.StatusCode.ToString()
            };
        }
        return webResponse;
    }

    protected string GetServerBaseUrl()
    {
        var baseUrl = "";
        if (httpClient.BaseAddress is not null)
        {
            var array = httpClient.BaseAddress.ToString().Split("/");
            baseUrl = array[0] + "//" + array[2] + "/";
        }
        return baseUrl;
    }

    #endregion
}

    
