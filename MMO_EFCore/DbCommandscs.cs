using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMO_EFCore
{
    public class DbCommandscs
    {
        public static void InitializeDB(bool foreceReset = false)
        {
            using (AppDbContext db = new AppDbContext())
            {
                if (!foreceReset && (db.GetService<IDatabaseCreator>() as RelationalDatabaseCreator).Exists())
                    return;

                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();

                CreateTestData(db);
                Console.WriteLine("DB Initialized");
            }
        }

        public static void CreateTestData(AppDbContext db)
        {
            var Rookiss = new Player(){ Name = "Rookiss"};
            var Faker = new Player(){ Name = "Faker" };
            var deft = new Player(){ Name = "Deft" };

            List<Item> items = new List<Item>()
            {
                new Item()
                {
                    TemplateId = 101,
                    CreatedDate = DateTime.Now,
                    Owner = Rookiss
                },
                new Item()
                {
                    TemplateId = 102,
                    CreatedDate = DateTime.Now,
                    Owner = Faker
                },
                new Item()
                {
                    TemplateId = 103,
                    CreatedDate = DateTime.Now,
                    Owner = deft
                },
            };

            Guild guild = new Guild()
            {
                GuildName = "T1",
                Members = new List<Player>() { Rookiss, Faker, deft }
            };

            db.Items.AddRange(items);
            db.Guilds.Add(guild);


            db.SaveChanges();

        }

        public static void UpdateTest()
        {
            using (AppDbContext db = new AppDbContext())
            {
                var guild = db.Guilds.Single(g => g.GuildName == "T1");

                guild.GuildName = "DWM";

                db.SaveChanges();
            }
        }
    }
}
