using DndBot.Bot;
using DndBot.Commands;
using DndBot.DAL;
using DndBot.Entities;
using MongoORM4NetCore;
using System;
using System.Threading.Tasks;

namespace DndBot
{
    class Program
    {
        static async Task Main(string[] args)
        {
            CrudFactory.OpenConnection(databaseName: "botDb");
            var customBot = new CustomBot();
            await customBot.RunBotAsync("dndBot", new GeneralCommandGroup(), new CombatCommandGroup(), new NoteCommandGroup());
            Console.WriteLine("Hello World!");
        }
    }
}
