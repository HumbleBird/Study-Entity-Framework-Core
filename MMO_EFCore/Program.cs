using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using System;

namespace MMO_EFCore
{
    class Program
    {
        static void Main(string[] arge)
        {

            Console.WriteLine("명령어를 입력하세요");

            Console.WriteLine("[0] ForceRest");
            Console.WriteLine("[1] ResetALL");
            Console.WriteLine("[2] UpdateDate");
            Console.WriteLine("[3] Delete");

            while (true)
            {
                Console.WriteLine("> ");
                string command = Console.ReadLine();
                switch (command)
                {
                    case "0":
                       DbCommandscs.InitializeDB(true);
                        break;
                    case "1":
                       DbCommandscs.ReadAll();
                        break;
                    case "2":
                       DbCommandscs.UpdateDate();
                        break;
                    case "3":
                       DbCommandscs.DeleteItem();
                        break;
                }
            }
        }
    }
}