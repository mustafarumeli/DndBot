using DndBot.Entities;
using MongoORM4NetCore;
using System.Linq;

namespace DndBot.Commands
{
    public static class NomadPattern
    {
        public static string ToDiscordBold(this string input) => $"**{input}**";
        public static string CenterText(this string input, int maxLength)
        {
            var length = maxLength - input.Length;
            int ceil = length / 2;
            var spaces = string.Join(string.Empty, Enumerable.Range(0, ceil).Select(x => " "));
            string extraspace = "";
            if (ceil % 2 == 1)
            {
                extraspace = " ";
            }
            return extraspace + spaces + input + spaces + extraspace;
        }
        public static void AddOrUpdate(this Crud<CombatActor> list, CombatActor ca)
        {
            var prevCombatActor = list.GetAll().FirstOrDefault(x => x.DiscordUserName != "DM" && x.DiscordUserName == ca.DiscordUserName);
            if (prevCombatActor != null)
            {
                list.Delete(prevCombatActor.Id);
            }
            list.Insert(ca);
        }

        public static CombatActor FindCombatActor(this Crud<CombatActor> list, string name) =>
            list.GetAll().FirstOrDefault(x => x.Name.ToLowerInvariant().Equals(name.ToLowerInvariant()));
        public static CombatActor FindCombatActorByDiscordUserName(this Crud<CombatActor> list, string name) =>
            list.GetAll().FirstOrDefault(x => x.DiscordUserName.Equals(name));

    }
}
