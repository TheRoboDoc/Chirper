using DSharpPlus.Commands;
using DSharpPlus.Commands.ContextChecks;
using DSharpPlus.Entities;
using System.ComponentModel;

namespace Chirper.Commands
{
    public class Settings
    {
        [Command("toggle_here")]
        [Description("Toggle the bot in this chat")]
        [RequirePermissions(botPermissions: DiscordPermissions.None, userPermissions: DiscordPermissions.ManageChannels)]
        public static async Task ToggleHere(CommandContext context,

            [Parameter("visible")]
            [Description("Is the command response visible to others")]
            bool visible = false)
        {
            bool? enabled = null;

            // Toggle logic

            DiscordInteractionResponseBuilder interactionResponseBuilder = new()
            {
                IsEphemeral = !visible,
                Content = $"Fixing Twitter/X embeds in {context.Channel.Mention} set to {enabled}\n" +
                          $"-# This feature currently **is not implemented**"
            };

            await context.RespondAsync(interactionResponseBuilder);
        }
    }
}
