using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MMO_EFCore
{
    // 오늘의 주제 : State (상태)
    // 0) Detached (No Tracking ! 추적되지 않는 상태. SaveChanges를 해도 존재도 모름)
    // 1) Unchanged (DB에 있고, 딱히 수정사항도 없었음. SaveChanges를 해도 아무 것도 X)
    // 2) Deleted (DB에는 아직 있지만, 삭제되어야 함. SaveChanges로 DB에 적용)
    // 3) Modified (DB에 있고, 클라에서 수정된 상태. SaveChanges로 DB에 적용)
    // 4) Added (DB에는 아직 없음. SaveChanges로 DB에 적용)

    public class DbCommands
    {
        // 초기화 시간이 좀 걸림
        public static void InitializeDB(bool forceReset = false)
        {
            using (AppDbContext db = new AppDbContext())
            {
                if (!forceReset && (db.GetService<IDatabaseCreator>() as RelationalDatabaseCreator).Exists())
                    return;

                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();

                string command =
                   @" CREATE FUNCTION GetAverageReviewScore (@itemId INT) RETURNS FLOAT
                      AS
                      BEGIN

                      DECLARE @result AS FLOAT

                      SELECT @result = AVG(CAST([Score] AS FLOAT))
                      FROM ItemReview AS r
                      WHERE @itemId = r.ItemId

                      RETURN @result                        

                      END";

                db.Database.ExecuteSqlRaw(command);

                CreateTestData(db);
                Console.WriteLine("DB Initialized");
            }
        }

        public static void CreateTestData(AppDbContext db)
        {
            var rookiss = new Player() { Name = "rr" };
            var faker = new Player() { Name = "Faker" };
            var deft = new Player() { Name = "Deft" };

            List<Item> items = new List<Item>()
            {
                new Item()
                {
                    TemplateId = 101,
                    Owner = rookiss
                }
            };

            Guild guild = new Guild()
            {
                GuildName = "T1",
                Members = new List<Player>() { rookiss, faker, deft }
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
                    if (item.SoftDeleted)
                    {
                        Console.WriteLine($"DELETED - ItemId({item.ItemId}) TemplateId({item.TemplateId})");
                    }
                    else
                    {
                        if (item.Owner == null)
                            Console.WriteLine($"ItemId({item.ItemId}) TemplateId({item.TemplateId}) Owner(0)");
                        else
                            Console.WriteLine($"ItemId({item.ItemId}) TemplateId({item.TemplateId}) OwnerId({item.Owner.PlayerId}) Owner({item.Owner.Name})");
                    }
                }
            }
        }

        public static void Test()
        {
            using (AppDbContext db = new AppDbContext())
            {
                {
                    string name = "rr";

                    var list = db.Players
                        .FromSqlRaw("select * from dbo.Player Where Name = {0}", name)
                        .ToList();

                    foreach (var p in list)
                    {
                        Console.WriteLine($"{p.Name} {p.PlayerId}");
                    }

                    var list2 = db.Players
                        .FromSqlInterpolated($"select * from dbo.Player Where Name = {name}")
                        .ToList();

                    foreach (var p in list2)
                    {
                        Console.WriteLine($"{p.Name} {p.PlayerId}");
                    }
                }

                // ExecuteSqlCommand (Non-Queary SQL) + Reload
                {
                    Player p = db.Players.Single(p => p.Name == "Faker");

                    string prevName = "Faker";
                    string afterName = "Faker_New";
                    db.Database.ExecuteSqlInterpolated($"Update dbo.Player SET Name={afterName} Where Name={prevName}");

                    db.Entry(p).Reload();
                }


            }
        }

    }
}
