using DSharpPlus.CommandsNext;
using System.Threading.Tasks;

namespace DndBot.Abstraction
{
    public interface INoteCommandGroup : ICommandGroup
    {
        Task SetPcName(CommandContext ctx, string alias);
        Task TakeNote(CommandContext ctx, string notes);
        Task ReadNote(CommandContext ctx);
    }
}
