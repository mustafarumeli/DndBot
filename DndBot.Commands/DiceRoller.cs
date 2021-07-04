using DndBot.Abstraction;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DndBot.Commands
{
    public class DiceRoller : IDiceRoller
    {
        static Random rnd = new Random();
        static List<string> rolledDices = new List<string>();
        public static bool WasFailed = false;
        public async Task<int> RollDice(CommandContext ctx, string text)
        {
            WasFailed = false;
            rolledDices = new List<string>();
            var damage = CalculateDice(text);
            if (WasFailed)
            {
                await ctx.RespondAsync($"**Result** {text} (" + string.Join("+", rolledDices) + ")");
                await ctx.RespondAsync($"**CRITICAL FAIL** {ctx.Member.Mention}");
            }
            else
            {
                StringBuilder response = new StringBuilder();
                response.AppendLine($"{DiscordEmoji.FromName(ctx.Client, ":game_die:")} {ctx.Member.Mention}");
                response.AppendLine($"**Result** {text} ({string.Join("+", rolledDices)})");
                response.AppendLine($"**Total** {damage} ");
                await ctx.RespondAsync(response.ToString());
            }
            await ctx.Message.DeleteAsync();
            return damage;
        }
        public int GetDiceCount(string text)
        {
            var dIndex = text.IndexOf("d");
            var diceCount = text.Substring(0, dIndex);
            return int.Parse(diceCount);
        }
        public int GetDiceType(string text)
        {
            var dIndex = text.IndexOf("d");
            string retVal = "";
            for (int charIndex = dIndex + 1; charIndex < text.Length; charIndex++)
            {
                var c = text[charIndex];
                if (char.IsDigit(c))
                {
                    retVal += c;
                }
                else
                {
                    break;
                }
            }

            return int.Parse(retVal);
        }
        public int CalculateDice(string text)
        {
            int diceCount = GetDiceCount(text);
            int diceType = GetDiceType(text);
            int calcResult = 0;
            for (int i = 0; i < diceCount; i++)
            {
                var res = rnd.Next(1, diceType + 1);
                if (res == 1)
                {
                    WasFailed = true;
                    rolledDices.Add($"**{res}**");
                    return 0;
                }
                if (res == diceType)
                {
                    rolledDices.Add($"**{res}**");
                    var innerRoll = rnd.Next(1, diceType + 1);
                    res += innerRoll;
                    rolledDices.Add($"{innerRoll}");

                }
                else
                {
                    rolledDices.Add(res.ToString());
                }
                calcResult += res;
            }
            return calcResult + GetModifierPart(text);
        }
        public int GetModifierPart(string text)
        {
            var plusIndex = text.IndexOf("+");
            if (plusIndex > -1)
            {
                var dAfterPlus = text.IndexOf("d", plusIndex);
                if (dAfterPlus < 0)
                {
                    return int.Parse(text.Substring(plusIndex + 1, text.Length - plusIndex - 1));
                }
                else
                {
                    var textLength = text.Length;
                    text = text.Substring(plusIndex + 1, textLength - plusIndex - 1);
                    return CalculateDice(text);
                }
            }

            return 0;
        }
    }
}
