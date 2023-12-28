using DSharpPlus;
using DSharpPlus.SlashCommands;
using DSharpPlus.SlashCommands.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chirper.SlashCommands
{
    public class Settings : ApplicationCommandModule
    {
        [SlashCommand("Toggle here", "Toggle the bot in this chat")]
        [SlashRequireUserPermissions(Permissions.ManageChannels)]
        public static async Task ToggleHere(InteractionContext context,

            [Option("Visible", "Is the command response visible to others")]
            bool visible = false)
        {
            bool? enabled = null;

            // Toggle logic

            await context.CreateResponseAsync($"Fixing Twitter/X embeds in {context.Channel.Mention} set to {enabled}", !visible);
        }

        [SlashCommand("Toggle global", "Toggle the bot in this server")]
        [SlashRequireUserPermissions(Permissions.Administrator)]
        public static async Task ToggleGlobal(InteractionContext context,

            [Option("Visible", "Is the command response visible to others")]
            bool visible = false)
        {
            bool? enabled = null;

            // Toggle logic

            await context.CreateResponseAsync($"Fixing Twitter/X embeds set to {enabled}", !visible);
        }
    }
}
