using DSharpPlus;
using DSharpPlus.SlashCommands;
using DSharpPlus.SlashCommands.Attributes;

namespace Chirper.SlashCommands
{
    public class Settings : ApplicationCommandModule
    {
        [SlashCommand("Toggle_here", "Toggle the bot in this chat")]
        [SlashRequireUserPermissions(Permissions.ManageChannels)]
        public static async Task ToggleHere(InteractionContext context,

            [Option("Visible", "Is the command response visible to others")]
            bool visible = false)
        {
            bool? enabled = null;

            // Toggle logic

            await context.CreateResponseAsync($"Fixing Twitter/X embeds in {context.Channel.Mention} set to {enabled}", !visible);
        }
    }
}
