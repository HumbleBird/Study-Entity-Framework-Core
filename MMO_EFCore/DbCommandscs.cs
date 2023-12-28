using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.VisualBasic;
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

        public static void Update_1vM()
        {
            ShowGuild();

            Console.WriteLine("Input ItemSwitch Playerid");
            Console.WriteLine(" > ");
            int id = int.Parse(Console.ReadLine());

            using (AppDbContext  db = new AppDbContext())
            {
                Guild guild = db.Guilds
                    .Include(g => g.Members)
                    .Single(g => g.GuildId == id);


                guild.Members.Add(new Player()
                {
                    Name = "Dopa"
                });

                db.SaveChanges();
            }

            Console.WriteLine("Test Complete");
            ShowGuild();
        }

        public static void Update_1v1()
        {
            ShowItems();

            Console.WriteLine("Input ItemSwitch Playerid");
            Console.WriteLine(" > ");
            int id = int.Parse(Console.ReadLine());

            using (AppDbContext  db = new AppDbContext())
            {
                Player player =  db.Players
                    .Include(p => p.Item)
                    .Single(p => p.PlayerId == id);

                if( player.Item != null)
                {
                    player.Item.TemplateId = 888;
                    player.Item.CreatedDate = DateTime.Now;
                }

                player.Item = new Item()
                {
                    TemplateId = 777,
                    CreatedDate = DateTime.Now
                };

                db.SaveChanges();
            }

            Console.WriteLine("Test Complete");
            ShowItems();
        }

        public static void ShowItems()
        {
            using (AppDbContext db = new AppDbContext())
            {
                foreach (var item in db.Items.Include(i => i.Owner).ToList())
                {
                    if (item.Owner == null)
                        Console.WriteLine($"ItemId({item.ItemId}) TemplateId({item.TemplateId}) Owner(0)");
                    else
                        Console.WriteLine($"ItemId({item.ItemId}) TemplateId({item.TemplateId}) Owner({item.Owner.PlayerId}) Owner({item.Owner})");
                }
            }
        }

        public static void ShowGuild()
        {
            using (AppDbContext db = new AppDbContext())
            {
                foreach (var guild in db.Guilds.Include(i => i.Members).ToList())
                {
                    Console.WriteLine($"guildId({guild.GuildId}) guildName({guild.GuildName}) MemberCount({guild.MapGuildToDto()})");
                }
            }
        }
    }
}
