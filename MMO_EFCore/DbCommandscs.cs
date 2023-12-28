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



        public static void ShowItems()
        {
            using (AppDbContext db = new AppDbContext())
            {
                foreach (var item in db.Items.Include(i => i.Owner).IgnoreQueryFilters().ToList())
                {
                    if(item.SoftDeleted)
                    {
                        Console.WriteLine($"Deleted ItemId({item.ItemId}) TemplateId({item.TemplateId}) Owner({item.Owner.PlayerId}) Owner({item.Owner})");

                    }
                    else
                    {
                        if (item.Owner == null)
                            Console.WriteLine($"ItemId({item.ItemId}) TemplateId({item.TemplateId}) Owner(0)");
                        else
                            Console.WriteLine($"ItemId({item.ItemId}) TemplateId({item.TemplateId}) Owner({item.Owner.PlayerId}) Owner({item.Owner})");
                    }

                    
                }
            }
        }

        public static void ShowGuild()
        {
            using (AppDbContext db = new AppDbContext())
            {
                foreach (var guild in db.Guilds.Include(i => i.Members).ToList())
                {
                    Console.WriteLine($"guildId({guild.GuildId}) guildName({guild.GuildName}) MemberCount()");
                }
            }
        }

        public static void TestDelete()
        {
            ShowItems();

            Console.WriteLine("Select Delete ItemId");
            Console.WriteLine(" > ");
            int id = int.Parse( Console.ReadLine() );

            using (AppDbContext db = new AppDbContext())
            {
                Item item = db.Items.Find(id);
                //db.Items.Remove(item);
                item.SoftDeleted = true;
                db.SaveChanges();
            }

            Console.WriteLine("Test Complete");
            ShowItems();
        }
    }
}
