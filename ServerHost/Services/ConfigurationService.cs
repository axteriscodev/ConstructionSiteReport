using System.Runtime.InteropServices;
using System.Text.Json;

namespace ServerHost.Services
{
    public class ConfigurationService
    {


        private static readonly string DEFAULT_CONFIG = @"appsettings.json";
        private static readonly string WIN_CONFIG_FILE = @"config\appsettings.json";
        private static readonly string LINUX_CONFIG_FILE = "config/appsettings.json";

        /// <summary>
        /// Metodo statico che legge la stringa di connesione al database dall' appsettings.json
        /// </summary>
        /// <returns>la stringa di connesione al database</returns>
        public static string? GetConnection()
        {
            string conn = "";
            try
            {
                var config = GetConfigFile();
                conn = config.GetValue<string>("connectionStrings:dbConnection") ?? "";
            }
            catch (Exception) { }
            return conn;
        }

        public static string? GetConfiguration(string key)
        {
            var value = "";
            try
            {
                var config = GetConfigFile();
                value = config.GetValue<string>(key);
            }
            catch (Exception) { }

            return value;
        }

        public static int GetConfigurationInt(string key)
        {
            int value = 0;
            try
            {
                var config = GetConfigFile();
                value = config.GetValue<int>(key);
            }
            catch (Exception) { }

            return value;
        }

        /// <summary>
        /// Metodo statico che restituisce il nome della cartella dove vengono scritti i file di log
        /// </summary>
        /// <returns>il percorso della cartella</returns>
        public static string GetLogsFolder()
        {
            var config = GetConfigFile();
            var folderName = config.GetValue<string>("logs");
            var directory = AppDomain.CurrentDomain.BaseDirectory;
            return directory + folderName;
        }

        #region Metodi per modificare/creare/leggere il file appsettings.json

        /// <summary>
        /// Metodo che restituisce true se siamo su sistema operativo Windows
        /// </summary>
        /// <returns></returns>
        public static bool IsWindows()
        {
            return RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        }

        /// <summary>
        /// Metodo che restituisce true se siamo su sistema operativo linux
        /// </summary>
        /// <returns></returns>
        public static bool IsLinux()
        {
            return RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
        }

        /// <summary>
        /// Metodo che restituisce il percorso giusto del file di configurazione in base all OS
        /// </summary>
        /// <returns></returns>
        public static string ConfigFilePath()
        {
            return IsWindows() ? WIN_CONFIG_FILE : LINUX_CONFIG_FILE;
        }

        #endregion

        #region Metodi Privati

        /// <summary>
        /// Metodo utilizzato per caricare il file di configurazione appsettings.json
        /// </summary>
        /// <returns>restiuisce la root del file di configurazione su cui richiedere i vari tag</returns>
        private static IConfigurationRoot GetConfigFile()
        {
            var path = AppDomain.CurrentDomain.BaseDirectory + ConfigFilePath();
            var file = File.Exists(path) ? ConfigFilePath() : DEFAULT_CONFIG;
            var root = new ConfigurationBuilder().AddJsonFile(file).Build();
            return root;
        }

        #endregion
    }
}
