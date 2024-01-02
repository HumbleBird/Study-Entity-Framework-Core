using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMO_EFCore
{
    public class AppDbContext : DbContext
    {
        // DbSet<Item> -> EF Core한테 알려준다.
        public DbSet<Item> Items { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Guild> Guilds { get; set; }

        //public const string ConnectionString = @"Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=EfCoreDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";
        public const string ConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=EfCoreDb;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ConnectionString);

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Item>().HasQueryFilter(i => i.SoftDeleted == false);

            modelBuilder.Entity<Player>()
                .HasIndex(p => p.Name)
                .HasName("Index_Person_Name")
                .IsUnique();

            modelBuilder.Entity<Player>()
                .HasMany(p => p.CreateItems)
                .WithOne(i => i.Creator)
                .HasForeignKey(i => i.CreateId);

            // Shadow Property
            modelBuilder.Entity<Item>().Property<DateTime>("RecoveredDate");

            // Backing Field
            modelBuilder.Entity<Item>()
                .Property(i => i.JsonData)
                .HasField("_jsonData");
        }
    }
}
