using DndBot.Abstraction;
using DndBot.Entities;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using MongoORM4NetCore;
using System.Linq;
using System.Threading.Tasks;

namespace DndBot.Commands
{
    public class NoteCommandGroup : BaseCommandModule, INoteCommandGroup
    {
        [Command("spn"), Description("Sets Pc's Name.")]
        public async Task SetPcName(CommandContext ctx, string alias)
        {
            Crud<PlayerCharacter> pcCrud = new Crud<PlayerCharacter>();
            string username = ctx.User.Username;
            var pc = pcCrud.GetAll().FirstOrDefault(x => x.DiscordUserName == username);
            if (pc == null)
            {
                pc = new PlayerCharacter
                {
                    DiscordUserName = username,
                    PcName = alias
                };
                pcCrud.Insert(pc);
            }
            else
            {
                pc.PcName = alias;
                pcCrud.Update(pc.Id, pc);
            }
            await ctx.RespondAsync($"{ctx.Member.Mention} Ok from now on your name shall be {alias}");
            await ctx.Message.DeleteAsync();

        }
        [Command("n"), Description("Takes Notes.")]
        public async Task TakeNote(CommandContext ctx, string notes)
        {

            Crud<Notes> crud = new Crud<Notes>();
            Crud<PlayerCharacter> pcCrud = new Crud<PlayerCharacter>();

            string username = ctx.User.Username;

            var pc = pcCrud.GetAll().FirstOrDefault(x => x.DiscordUserName == username);
            if (pc == null)
            {
                pc = new PlayerCharacter
                {
                    DiscordUserName = username,
                    PcName = ""
                };
                pcCrud.Insert(pc);

            }
            crud.Insert(new Notes
            {
                Pc = pc,
                Note = notes
            });

            var emoji = DiscordEmoji.FromName(ctx.Client, ":penguin:");
            await ctx.RespondAsync($"{emoji} Noted, { ctx.Member.Mention}!");
            await ctx.Message.DeleteAsync();
        }
        [Command("r"), Description("Sends Notes To User As DM.")]
        public async Task ReadNote(CommandContext ctx)
        {

            Crud<Notes> crud = new Crud<Notes>();
            Crud<PlayerCharacter> pcCrud = new Crud<PlayerCharacter>();

            string username = ctx.User.Username;

            var pc = pcCrud.GetAll().FirstOrDefault(x => x.DiscordUserName == username);
            if (pc != null)
            {

                var allMessages = crud.GetAll().Where(x => x.Pc.Id == pc.Id);
                var emoji = DiscordEmoji.FromName(ctx.Client, ":book:");

                var discordDmChannel = await ctx.Member.CreateDmChannelAsync();
                var messages = string.Join("\n", allMessages.Select(x => x.CreationDate.ToString("dd-MM-yyyy HH:mm") + ":" + x.Note));
                await discordDmChannel.SendMessageAsync(messages);
                // and finally, let's respond and greet the user.
                await ctx.RespondAsync($"{emoji} sent to you {ctx.Member.Mention}");
            }
            else
            {
                await ctx.RespondAsync($"No notes for you");
                await ctx.RespondAsync($"Cause you don't have any");
            }
            await ctx.Message.DeleteAsync();

        }
    }
}
