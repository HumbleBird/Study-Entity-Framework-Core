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
            var player = new Player()
            {
                Name = "Rookiss"
            };

            List<Item> items = new List<Item>()
            {
                new Item()
                {
                    TemplateId = 101,
                    CreatedDate = DateTime.Now,
                    Owner = player
                },
                new Item()
                {
                    TemplateId = 102,
                    CreatedDate = DateTime.Now,
                    Owner = player
                },
                new Item()
                {
                    TemplateId = 103,
                    CreatedDate = DateTime.Now,
                    Owner = new Player() {Name = "Fake"}
                },
            };

            db.Items.AddRange(items);
            db.SaveChanges();
        }

        public static void ReadAll()
        {
            using (var db = new AppDbContext())
            {
                // AsNoTracking : ReadOnly 
                // Include : Eager Loading (즉시 로딩)
                foreach(Item item in db.Items.AsNoTracking().Include(i=> i.Owner))
                {
                    Console.WriteLine($"TemplatedId({item.TemplateId}) Owner({item.Owner.Name}) Created({item.CreatedDate})");
                }
            }
        }

        public static void ShowItems()
        {
            Console.WriteLine("플레이어 이름을 입력하세요");
            Console.Write(" > ");
            string name = Console.ReadLine();

            //using (var db = new AppDbContext())
            //{
            //    foreach(Player player in  db.Players.AsNoTracking().Where(p => p.Name == name).Include(p=>p.Items))
            //    {
            //        foreach(Item item in player.Items)
            //        {
            //            Console.WriteLine($"{item.TemplateId}");
            //        }
            //    }
            //}
        }
    }
}
