using Microsoft.EntityFrameworkCore;
using PostgreDB_Connection.Entities;

namespace PostgreDB_Connection
{
    /// <summary>
    /// Contexto de conexión de base de datos para PostgreSQL
    /// </summary>
    public class PostgreSQL_Context : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connection = Properties.Resources.ResourceManager.GetString("Postgre_Connection");
            optionsBuilder.UseNpgsql(connection);
        }

        public DbSet<users> users { get; set; }

        public DbSet<cars> cars { get; set; }
    }
}