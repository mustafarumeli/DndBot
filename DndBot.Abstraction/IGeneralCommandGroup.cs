using DSharpPlus.CommandsNext;
using System.Threading.Tasks;

namespace DndBot.Abstraction
{
    public interface IGeneralCommandGroup : ICommandGroup
    {
        Task Ping(CommandContext ctx);
        /// <summary>
        /// Deletes All Messages Of A Group
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        Task Cls(CommandContext ctx);

    }
}
