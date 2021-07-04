using DSharpPlus.CommandsNext;
using System.Threading.Tasks;

namespace DndBot.Abstraction
{
    public interface IDiceRoller
    {
        int GetModifierPart(string text);
        int CalculateDice(string text);
        int GetDiceType(string text);
        int GetDiceCount(string text);
        Task<int> RollDice(CommandContext ctx, string text);

    }
}
