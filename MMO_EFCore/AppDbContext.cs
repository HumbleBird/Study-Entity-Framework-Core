using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMO_EFCore
{
    internal class AppDbContext : DbContext
    {
        // DbSet<Item> -> EF Core한테 알려준다.
        public DbSet<Item> Items { get; set; }

        //public const string ConnectionString = @"Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=EfCoreDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";
        public const string ConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=EfCoreDb;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ConnectionString);

        }
    }
}
