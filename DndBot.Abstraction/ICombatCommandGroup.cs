using DSharpPlus.CommandsNext;
using System.Threading.Tasks;

namespace DndBot.Abstraction
{

    public interface ICombatCommandGroup : ICommandGroup
    {
        Task SetHp(CommandContext ctx, int hit);
        Task GetHp(CommandContext ctx);
        Task HitToPc(CommandContext ctx, string pc, int hit);
        Task Roll(CommandContext ctx, string command);
        Task AddPc(CommandContext ctx, string name, int hp);
        Task AddChar(CommandContext ctx, string name, int hp, bool isVisible);
        Task DealDamage(CommandContext ctx, string name, int hp);
        Task DealDamage(CommandContext ctx, string name, string dice);
        Task Heal(CommandContext ctx, string name, string dice);
        Task Heal(CommandContext ctx, string name, int hp);
        Task CombatStatus(CommandContext ctx);
        Task SetStatus(CommandContext ctx, string name, string status);
        Task SetVisibility(CommandContext ctx, string name);

    }
}
