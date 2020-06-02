using Microsoft.EntityFrameworkCore;
using SqlServerDB_Connection.Entities;
using System;

namespace SqlServerDB_Connection
{
    public class SqlServer_Context : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connection = Properties.Resources.ResourceManager.GetString("SqlServer_Connection");
            optionsBuilder.UseSqlServer(connection);

            base.OnConfiguring(optionsBuilder);
        }

        public DbSet<Users> Users { get; set; }
    }
}