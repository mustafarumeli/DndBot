using MongoORM4NetCore.Interfaces;
using System;

namespace DndBot.Entities
{
    public class CombatActor : DbObject
    {
        public CombatActor(string discordUserName, string name, int hp, string status, bool ısVisibleToPc)
        {
            DiscordUserName = discordUserName;
            Name = name;
            Hp = hp;
            Status = status;
            IsVisibleToPc = ısVisibleToPc;
        }

        public string DiscordUserName { get; set; }
        public string Name { get; set; }
        public int Hp { get; set; }
        public string Status { get; set; }
        public bool IsVisibleToPc { get; set; }
    }
}
