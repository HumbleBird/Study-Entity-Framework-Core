using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
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

        public static void UpdateReload()
        {
            ShowGuilds();

            Console.WriteLine("Input GuildId");
            Console.WriteLine(" > ");
            int id = int.Parse(Console.ReadLine());

            Console.WriteLine("Input GuildName");
            Console.WriteLine(" > ");
            string name = Console.ReadLine();

            using (AppDbContext db = new AppDbContext())
            {
                Guild guild = db.Find<Guild>(id);
                guild.GuildName = name;

                db.SaveChanges();
            }
            Console.WriteLine("Update cop");

            ShowGuilds();
        }

        public static string MakeUpdateJsonStr()
        {
            var jsonStr = "{\"GuildId\":1, \"GuildName\":\"Hellow\", \"Members\":null}";
            return jsonStr;
        }

        public static void UpdateFull()
        {
            ShowGuilds();

            string jsonStr = MakeUpdateJsonStr();
            Guild guild = JsonConvert.DeserializeObject<Guild>(jsonStr);
            using (AppDbContext db = new AppDbContext())

            {
                db.Guilds.Update(guild);
                db.SaveChanges();
            }
            Console.WriteLine("Update cop");

            ShowGuilds();
        }

        public static void ShowGuilds()
        {
            using (AppDbContext db = new AppDbContext())
            {
                foreach (var guild in db.Guilds.MapGuildToDto().ToList())
                {
                    Console.WriteLine($"GuildId({guild.GuildId}) GuildName({guild.Name}) MemberCount({guild.MemberCount})");
                }
            }
        }
    }
}
