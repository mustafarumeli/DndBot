using DndBot.Abstraction;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;

namespace DndBot.Commands
{

    public class CombatCommandGroup : BaseCommandModule, ICombatCommandGroup
    {
        [Command("AddChar"), Description("Dm Adds new Character.")]
        public async Task AddChar(CommandContext ctx, string name, int hp, bool isVisible)
        {
            await new Combat(ctx).AddChar(name, hp, isVisible);
        }


        [Command("AddPc"), Description("User Adds New Pc.")]
        public async Task AddPc(CommandContext ctx, string name, int hp)
        {
            await new Combat(ctx).AddPc(name, hp);
        }

        [Command("cs")]
        public async Task CombatStatus(CommandContext ctx)
        {
            await new Combat(ctx).GetCombatStatus();
        }


        [Command("DealDamage")]
        public async Task DealDamage(CommandContext ctx, string name, int hp)
        {
            await new Combat(ctx).DealDamage(name, hp);
        }
        [Command("DealDamage")]
        public async Task DealDamage(CommandContext ctx, string name, string dice)
        {
            await new Combat(ctx).DealDamage(name, dice);
        }
        public static ConcurrentDictionary<string, int> _hpTracker = new ConcurrentDictionary<string, int>();

        [Command("getHp"), Description("Get Hp.")]
        public async Task GetHp(CommandContext ctx) // this command takes a member as an argument; you can pass one by username, nickname, id, or mention
        {
            int hp = 0;
            var isTrue = _hpTracker.TryGetValue(ctx.Member.Username, out hp);
            if (isTrue)
            {
                await ctx.RespondAsync($"Your Hp is {hp} {ctx.Member.Username}");

            }
            else
            {
                await ctx.RespondAsync($"You don't have a Hp Pal {ctx.Member.Username}");
            }

            await ctx.Message.DeleteAsync();

        }


        [Command("Heal")]
        public async Task Heal(CommandContext ctx, string name, string dice) // this command takes a member as an argument; you can pass one by username, nickname, id, or mention
        {
            await new Combat(ctx).Heal(name, dice);
        }
        [Command("Heal")]
        public async Task Heal(CommandContext ctx, string name, int hp) // this command takes a member as an argument; you can pass one by username, nickname, id, or mention
        {
            await new Combat(ctx).Heal(name, hp);
        }


        [Command("hittopc"), Description("Hit.")]
        public async Task HitToPc(CommandContext ctx, string pc, int hit) // this command takes a member as an argument; you can pass one by username, nickname, id, or mention
        {
            var discordRoles = ctx.Member.Roles.Select(x => x.Name);
            if (discordRoles.Contains("DM"))
            {
                int hp = 0;
                var isTrue = _hpTracker.TryGetValue(pc, out hp);
                if (isTrue)
                {
                    _hpTracker.TryUpdate(pc, hp - hit, hp);
                    await ctx.RespondAsync($"DM Hit {pc}");
                    await ctx.RespondAsync($"{pc}'s new hp is {hp - hit}");
                    await ctx.Message.DeleteAsync();
                }

            }
            else
            {
                await ctx.RespondAsync($"YOU ARE A PEASANT PC");
            }
        }


        [Command("roll"), Description("Hit.")]
        public async Task Roll(CommandContext ctx, string command) // this command takes a member as an argument; you can pass one by username, nickname, id, or mention
        {
            await new DiceRoller().RollDice(ctx, command);
        }
        [Command("setHp"), Description("Hit.")]
        public async Task SetHp(CommandContext ctx, int hp) // this command takes a member as an argument; you can pass one by username, nickname, id, or mention
        {
            _hpTracker.AddOrUpdate(ctx.Member.Username, hp, (key, value) => value = hp);
            await ctx.RespondAsync($"Set Your Hp to  {hp} Pal {ctx.Member.Username}");
            await ctx.Message.DeleteAsync();
        }


        [Command("SetStatus")]
        public async Task SetStatus(CommandContext ctx, string name, string status) // this command takes a member as an argument; you can pass one by username, nickname, id, or mention
        {
            await new Combat(ctx).SetStatus(name, status);
        }

        [Command("SetVisibility")]
        public async Task SetVisibility(CommandContext ctx, string name) // this command takes a member as an argument; you can pass one by username, nickname, id, or mention
        {
            await new Combat(ctx).SetVisibility(name);
        }

    }
}
