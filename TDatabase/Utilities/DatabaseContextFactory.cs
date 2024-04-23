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
        public static DbCsclDamicoContext Create(string connectionString)
        {
            var OptionsBuilder = new DbContextOptionsBuilder<DbCsclDamicoContext>();
            OptionsBuilder.UseSqlServer(connectionString);
            return new DbCsclDamicoContext(OptionsBuilder.Options);
        }
    }
}
