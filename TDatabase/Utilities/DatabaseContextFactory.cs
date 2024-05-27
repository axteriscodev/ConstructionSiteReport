using Microsoft.EntityFrameworkCore;
using TDatabase.Database;

namespace TDatabase.Utilities
{

    /// <summary>
    /// Classe utilizzata per creare il contesto del db
    /// </summary>
    public class DatabaseContextFactory
    {
        /// <summary>
        /// Metodo per creare un contesto del database
        /// </summary>
        /// <returns></returns>
        public static DbCsclDamicoV2Context Create(string connectionString)
        {
            var OptionsBuilder = new DbContextOptionsBuilder<DbCsclDamicoV2Context>();
            OptionsBuilder.UseSqlServer(connectionString);
            return new DbCsclDamicoV2Context(OptionsBuilder.Options);
        }
    }
}
