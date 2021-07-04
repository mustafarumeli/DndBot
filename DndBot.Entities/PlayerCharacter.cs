using MongoORM4NetCore.Interfaces;

namespace DndBot.Entities
{
    public class PlayerCharacter : DbObject
    {
        public string DiscordUserName { get; set; }
        public string PcName { get; set; }
    }
}
