using Microsoft.AspNetCore.Components.Authorization;
using Shared.Login;
using Shared.Organizations;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using WebStorageManagement.Models;

namespace ConstructionSiteLibrary.Services
{
    /// <summary>
    /// Classe che estende AuthenticationStateProvider per controllare l'autenticazione e l'autorizzazione
    /// di un utente a navigare sul lato cliente del GestionaleClienti
    /// </summary>
    /// <remarks>
    /// L'autenticazione viene gestita dal server che restituisce indietro un token che viene salvato 
    /// nel local storage del browser per essere successivamente recuperato per riconoscere l'utente.
    /// </remarks>
    public class AppAuthenticationStateProvider(IWebStorageService storageService, HttpClient httpClient) : AuthenticationStateProvider
    {
        #region Campi

        /// <summary>
        /// Tag con il quale viene salvata la chiave sullo storage del browser che contiene il JWT 
        /// per l'autenticazione ed autorizzazione
        /// </summary>
        private const string TAG_ACCESS_INFO = "accessInfo";
        /// <summary>
        /// il servizio per scrivere/leggere nello storage del browser
        /// </summary>
        private readonly IWebStorageService storageService = storageService;
        /// <summary>
        /// Servizio http (singleton) utilizzato per aggiungere l'header di autenticazione alle richieste 
        /// in caso di autenticazione riuscita
        /// </summary>
        private readonly HttpClient httpClient = httpClient;
        /// <summary>
        /// Identità dell'utente tramite IdentityManager
        /// </summary>
        public ClaimsPrincipal? User { get; set; }
        /// <summary>
        /// Oggetto che permette la creazione e validazione di un JWT (IdentityManager)
        /// </summary>
        private readonly JwtSecurityTokenHandler jwtSecurityTokenHandler = new();
        /// <summary>
        /// Il token JWT
        /// </summary>
        public string Token { get; set; } = "";

        public bool updated = false;

        #endregion

        #region Autenticazione

        /// <summary>
        /// Metodo che viene richiamato all'apertura dell'app ed ogni volta che viene notificato un cambiamento di stato,
        /// controlla se sono disponibili i dati per l'autenticazione dell'utente cercandoli nell'oggetto utente o nel caso
        /// nello storage del browser. 
        /// </summary>
        /// <returns>
        /// restituisce un oggetto AuthenticationState che nel caso di autenticazione riuscita contiene un ClaimPrincipal
        /// contenente i dati dell'utente loggato, altrimenti un ClaimPrincipal vuoto
        /// </returns>
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            AuthenticationState AuthState = User is null ? new(new ClaimsPrincipal(new ClaimsIdentity())) : new(User);
            try
            {
                //se utente è null controllo nello storage se ho dei dati di accesso per l'autenticazione
                if (User is null || updated)
                {
                    updated = false;
                    //inizializzo il servizio di storage
                    await storageService.LocalStorage.Initialize();
                    //cerco nello storage se ci sono le info di accesso
                    var savedToken = await storageService.LocalStorage.Leggi(TAG_ACCESS_INFO);
                    Console.WriteLine(savedToken);
                    if (!string.IsNullOrEmpty(savedToken))
                    {
                        //leggo il token
                        JwtSecurityToken jwtSecurityToken = jwtSecurityTokenHandler.ReadJwtToken(savedToken);
                        DateTime expires = jwtSecurityToken.ValidTo;
                        if (expires < DateTime.UtcNow)
                        {
                            await storageService.LocalStorage.Cancella(TAG_ACCESS_INFO);
                        }
                        else
                        {
                            //prelevo la lista di claims dal jwt
                            var claims = jwtSecurityToken.Claims.ToList();
                            //creo l'identita dell utente con la lista di claim
                            var userIdentity = new ClaimsIdentity(claims, UserClaims.User);
                            //creo il ClaimsPrincipal inserendogli l'identità
                            var userPrincipal = new ClaimsPrincipal(userIdentity);
                            User = userPrincipal;
                            AuthState = new(User);
                            Token = savedToken;
                            SetAuthenticationHeader(Token);
                        }
                    }
                }
            }
            catch (Exception e) { Console.WriteLine(e.Message); }
            return AuthState;
        }

        /// <summary>
        /// Metodo che notifica il cambio di stato e quindi la riferifica dell'autenticazione da parte dell'utente
        /// </summary>
        public void StateChanged()
        {
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }


        /// <summary>
        /// Metodo utilizzato se l'autenticazione con il server è andata a buon fine
        /// per salvare il token nel LocalStorage e comunicare che l' AuthenticateState
        /// e stato modificato
        /// </summary>
        /// <param name="token">stringa che contiene i dati dell'utente autenticato</param>
        /// <returns></returns>
        public async Task SaveAuthenticationAsync(string token, bool isUpdated = false)
        {
            //salvo i dati nel localStorage
            await storageService.LocalStorage.Salva(TAG_ACCESS_INFO, token);
            updated = isUpdated;
            StateChanged();
        }

        /// <summary>
        /// Metodo che restituisce i dati dell'utente loggato (Shared_Operatore)
        /// </summary>
        /// <returns></returns>
        public UserModel GetUserInfo()
        {
            UserModel user = new();
            var temp = User!.Claims.FirstOrDefault(x => x.Type.Equals(UserClaims.User))!.Value;
            if (temp is not null)
            {
                user = JsonSerializer.Deserialize<UserModel>(temp) ?? new();
            }
            return user;
        }

        /// <summary>
        /// Metodo che restituisce il ruolo dell'utente loggato
        /// </summary>
        /// <returns></returns>
        public RoleModel GetUserRole()
        {
            var user = GetUserInfo();
            return user.Role;
        }

        /// <summary>
        /// Metodo che restituisce la sede dell'utente loggato
        /// </summary>
        /// <returns></returns>
        public OrganizationModel GetUserOrganization()
        {
            var user = GetUserInfo();
            return user.Organization;
        }

        public async Task Logout()
        {
            await storageService.LocalStorage.Cancella(TAG_ACCESS_INFO);
            User = null;
            StateChanged();
        }

        #endregion

        #region Metodi Privati

        private void SetAuthenticationHeader(string token)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
        }

        #endregion
    }
}
