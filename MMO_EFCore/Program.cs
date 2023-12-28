﻿using Microsoft.EntityFrameworkCore.Infrastructure;
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
            Console.WriteLine("[1] Test Delete");

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
                       DbCommandscs.TestDelete();
                        break;
                    case "2":
                        break;
                    case "3":
                        break;
                }
            }
        }
    }
}