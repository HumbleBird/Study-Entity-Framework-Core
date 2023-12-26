using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using System;

namespace MMO_EFCore
{
    class Program
    {
        static void InitializeDB(bool foreceReset = false)
        {
            using (AppDbContext db = new AppDbContext())
            {
                if (!foreceReset && (db.GetService<IDatabaseCreator>() as RelationalDatabaseCreator).Exists())
                    return;

                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();

                Console.WriteLine("DB Initialized");
            }

        }

        static void Main(string[] arge)
        {
            InitializeDB(true);
        }
    }
}