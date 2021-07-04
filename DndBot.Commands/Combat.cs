using DndBot.Abstraction;
using DndBot.Entities;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using MongoORM4NetCore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DndBot.Commands
{
    public class Combat : ICombat
    {
        public static string CombatName = ";e";
        public static Crud<CombatActor> CombatActors = new Crud<CombatActor>();
        private CommandContext _ctx = null;

        public Combat(CommandContext ctx)
        {
            _ctx = ctx;
        }
        public async Task InitiateCombat(string combatName)
        {
            CombatName = combatName;
            await _ctx.RespondAsync("Combat Started".ToDiscordBold());
        }

        public async Task AddPc(string name, int hp)
        {

            var username = _ctx.Member.Username;

            var combatActor = CombatActors.GetAll().FirstOrDefault(x => x.DiscordUserName == username);
            if (combatActor != null)
            {
                CombatActors.Delete(combatActor.Id);
            }
            CombatActors.Insert(new CombatActor(username, name, hp, "", true));
            await _ctx.RespondAsync($"{name.ToDiscordBold()} has entered to combat");
        }

        public async Task AddChar(string name, int hp, bool isVisible)
        {
            if (!_ctx.Member.Roles.Any(x => x.Name == "DM"))
            {
                await _ctx.RespondAsync($"Only DM can add a Char".ToDiscordBold());
            }
            else
            {
                var username = _ctx.Member.Username;

                var combatActor = CombatActors.GetAll().FirstOrDefault(x => x.DiscordUserName == username);
                if (combatActor != null)
                {
                    CombatActors.Delete(combatActor.Id);
                }
                CombatActors.Insert(new CombatActor("DM", name, hp, "", isVisible));
                var message = $"{name.ToDiscordBold()} has entered to combat";
                if (isVisible)
                {
                    await _ctx.RespondAsync(message);
                }
                else
                {
                    var discordDmChannel = await _ctx.Member.CreateDmChannelAsync();
                    await discordDmChannel.SendMessageAsync(message);
                }
            }

        }
        public async Task DealDamage(string to, int hp)
        {
            var combatActor = CombatActors.FindCombatActor(to);
            var character = CombatActors.FindCombatActorByDiscordUserName(_ctx.Member.Username);
            if (hp > 0)
            {
                combatActor.Hp -= hp;
                StringBuilder response = new StringBuilder();
                response.AppendLine($"{DiscordEmoji.FromName(_ctx.Client, ":crossed_swords:")} {_ctx.Member.Mention}");
                response.AppendLine($"{character.Name} dealt {hp} damage to {combatActor.Name}");
                if (combatActor.Hp <= 0)
                {
                    response.AppendLine($"{combatActor.Name} has died by hands of {character.Name}");
                    CombatActors.Delete(combatActor.Id);
                }
                else
                {
                    response.AppendLine($"{combatActor.Name} now has {combatActor.Hp} hp");
                }
                await _ctx.RespondAsync(response.ToString());
            }

        }

        public async Task DealDamage(string to, string dice)
        {
            int hp = await new DiceRoller().RollDice(_ctx, dice);
            var character = CombatActors.FindCombatActorByDiscordUserName(_ctx.Member.Username);
            if (DiceRoller.WasFailed)
            {
                await _ctx.RespondAsync($"{character.Name} was failed while attacking");
            }
            else
            {
                var combatActor = CombatActors.FindCombatActor(to);
                if (hp > 0)
                {
                    combatActor.Hp -= hp;
                    StringBuilder response = new StringBuilder();
                    response.AppendLine($"{DiscordEmoji.FromName(_ctx.Client, ":crossed_swords:")} {_ctx.Member.Mention}");
                    response.AppendLine($"{character.Name} dealt {hp} damage to {combatActor.Name}");
                    if (combatActor.Hp <= 0)
                    {
                        response.AppendLine($"{combatActor.Name} has died by hands of {character.Name}");
                        CombatActors.Delete(combatActor.Id);
                    }
                    else
                    {
                        response.AppendLine($"{combatActor.Name} now has {combatActor.Hp} hp");
                    }
                    await _ctx.RespondAsync(response.ToString());
                }
            }
        }

        public async Task Heal(string to, int hp)
        {
            var character = CombatActors.FindCombatActorByDiscordUserName(_ctx.Member.Username);
            if (DiceRoller.WasFailed)
            {
                await _ctx.RespondAsync($"{character.Name} was failed to heal");
            }
            else
            {
                var combatActor = CombatActors.FindCombatActor(to);
                if (hp > 0)
                {
                    combatActor.Hp += hp;
                    StringBuilder response = new StringBuilder();
                    response.AppendLine($"{DiscordEmoji.FromName(_ctx.Client, ":hospital:")} {_ctx.Member.Mention}");
                    response.AppendLine($"{character.Name} healed {hp} HP to {combatActor.Name}");
                    response.AppendLine($"{combatActor.Name} now has {combatActor.Hp} hp");
                    await _ctx.RespondAsync(response.ToString());
                }
            }
        }
        public async Task Heal(string to, string dice)
        {
            int hp = await new DiceRoller().RollDice(_ctx, dice);
            var character = CombatActors.FindCombatActorByDiscordUserName(_ctx.Member.Username);
            if (DiceRoller.WasFailed)
            {
                await _ctx.RespondAsync($"{character.Name} was failed to heal");
            }
            else
            {
                var combatActor = CombatActors.FindCombatActor(to);
                if (hp > 0)
                {
                    combatActor.Hp += hp;
                    StringBuilder response = new StringBuilder();
                    response.AppendLine($"{DiscordEmoji.FromName(_ctx.Client, ":hospital:")} {_ctx.Member.Mention}");
                    response.AppendLine($"{character.Name} healed {hp} HP to {combatActor.Name}");
                    response.AppendLine($"{combatActor.Name} now has {combatActor.Hp} hp");
                    await _ctx.RespondAsync(response.ToString());
                }
            }
        }

        public async Task GetCombatStatus()
        {
            var visibleCombatActors = CombatActors.GetAll().Where(x => x.IsVisibleToPc);
            //int longestNameLength = visibleCombatActors.Max(x => x.Name.Length);
            //int longestHpLength = visibleCombatActors.Max(x => x.Hp.ToString().Length);
            //int longestStatusLength = visibleCombatActors.Max(x => x.Status.Length);
            //if (longestStatusLength == 4)
            //{
            //    longestStatusLength = longestNameLength + longestHpLength - 8;
            //}
            StringBuilder sb = new StringBuilder();
            //sb.AppendLine(GenerateRow(longestNameLength, longestHpLength, longestStatusLength, RowType.AllLine));
            foreach (var visibleCombatActor in visibleCombatActors)
            {
                sb.AppendLine($" {visibleCombatActor.Name.ToDiscordBold()} has {visibleCombatActor.Hp} HP");
                if (!string.IsNullOrWhiteSpace(visibleCombatActor.Status))
                {
                    sb.Append($" With Status {visibleCombatActor.Status}");
                }
                //sb.AppendLine(GenerateRow(longestNameLength, longestHpLength, longestStatusLength, RowType.EmptyLine));
                //sb.AppendLine(GenerateRow(longestNameLength, longestHpLength, longestStatusLength, RowType.NamedLine, visibleCombatActor.Name, visibleCombatActor.Hp, visibleCombatActor.Status));
                //sb.AppendLine(GenerateRow(longestNameLength, longestHpLength, longestStatusLength, RowType.EmptyLine));
            }
            //sb.AppendLine(GenerateRow(longestNameLength, longestHpLength, longestStatusLength, RowType.AllLine));
            await _ctx.RespondAsync(sb.ToString());
        }
        public enum RowType
        {
            AllLine,
            EmptyLine,
            NamedLine,
        }
        public string GenerateRow(int nameLenth, int hpLength, int statusLength, RowType rowType, string name = "", int hp = -1, string status = "")
        {

            int rowLength = nameLenth + hpLength + statusLength + 4;
            string row = "";
            if (rowType == RowType.AllLine)
            {
                for (int i = 0; i <= rowLength; i++)
                {
                    row += "-";
                }
            }
            else if (rowType == RowType.EmptyLine)
            {
                for (int i = 0; i <= rowLength; i++)
                {

                    if (i == 0 || i == rowLength)
                    {
                        row += "|";
                    }
                    else
                    {
                        row += " ";
                    }
                }
            }
            else
            {
                row += "|";
                row += name.CenterText(nameLenth) + "|";
                row += hp.ToString().CenterText(hpLength) + "|";
                row += status.CenterText(statusLength);
                row += "|";
            }
            return row;
        }
        public async Task SetStatus(string to, string status)
        {
            var combatActor = CombatActors.FindCombatActor(to);
            var character = CombatActors.FindCombatActorByDiscordUserName(_ctx.Member.Username);
            combatActor.Status = status;
            StringBuilder response = new StringBuilder();
            response.AppendLine($"{DiscordEmoji.FromName(_ctx.Client, ":information_source:")} {_ctx.Member.Mention}");
            response.AppendLine($"{character.Name} has now has {status}");
            await _ctx.RespondAsync(response.ToString());
        }

        public async Task SetVisibility(string name)
        {
            if (_ctx.Member.Roles.Any(x => x.Name != "DM"))
            {
                await _ctx.RespondAsync($"Only DM can add a Char".ToDiscordBold());
            }
            else
            {
                var combatActor = CombatActors.FindCombatActor(name);
                combatActor.IsVisibleToPc = true;
                await _ctx.RespondAsync($"{name.ToDiscordBold()} has entered to combat");

            }
        }
    }
}
