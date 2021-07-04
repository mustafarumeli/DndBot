using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;
using System.Threading.Tasks;

namespace DndBot.Abstraction
{
    public interface IBot
    {
        Task RunBotAsync(string botName, params BaseCommandModule[] commandGroups);
        Task Client_Ready(DiscordClient sender, ReadyEventArgs e);
        Task Client_GuildAvailable(DiscordClient sender, GuildCreateEventArgs e);
        Task Client_ClientError(DiscordClient sender, ClientErrorEventArgs e);
        Task Commands_CommandExecuted(CommandsNextExtension sender, CommandExecutionEventArgs e);
        Task Commands_CommandErrored(CommandsNextExtension sender, CommandErrorEventArgs e);
    }
}
