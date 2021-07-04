using MongoORM4NetCore.Interfaces;

namespace DndBot.Entities
{
    public class Notes : DbObject
    {
        public PlayerCharacter Pc { get; set; }
        public string Title { get; set; }
        public string Note { get; set; }

    }
}
