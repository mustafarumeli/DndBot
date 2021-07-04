using DndBot.Abstraction;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DndBot.Commands
{
    public class GeneralCommandGroup : BaseCommandModule, IGeneralCommandGroup
    {
        [Command("cls"), Description("Clears All Messages of Current Channel.")]
        public async Task Cls(CommandContext ctx)
        {
            var discordRoles = ctx.Member.Roles.Select(x => x.Name);
            if (discordRoles.Contains("Bots"))
            {
                await ctx.Channel.DeleteMessagesAsync(await ctx.Channel.GetMessagesAsync(1000));
            }
            else
            {
                await ctx.RespondAsync($"Go Away Peasant");
            }
        }

        [Command("hello")]
        [Description("Example ping command")]
        [Aliases("hello2")]
        public async Task Ping(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();
            await ctx.RespondAsync($"Hello {ctx.Member.Username}");
        }
    }
}
