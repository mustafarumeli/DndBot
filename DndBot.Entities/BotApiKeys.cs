using MongoORM4NetCore.Interfaces;

namespace DndBot.Entities
{
    public class BotApiKeys : DbObject
    {
        public string BotName { get; set; }
        public string ApiKey { get; set; }
        public string CommandPrefix { get; set; }
    }
}
