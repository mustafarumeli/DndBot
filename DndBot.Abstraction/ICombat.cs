using System;
using System.Threading.Tasks;

namespace DndBot.Abstraction
{
    public interface ICombat
    {
        Task InitiateCombat(string combatName);
        Task AddPc(string name, int hp);
        Task AddChar(string name, int hp, bool isVisible);
        Task DealDamage(string to, int hp);
        Task DealDamage(string to, string dice);
        Task Heal(string to, int hp);
        Task Heal(string to, string dice);
        Task GetCombatStatus();
        Task SetStatus(string to, string status);
        Task SetVisibility(string name);
    }
}
