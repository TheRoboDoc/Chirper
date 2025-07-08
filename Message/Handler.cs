using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Exceptions;

namespace Chirper.Message
{
    public static partial class Handler
    {
        public static async Task Run(MessageCreatedEventArgs messageArgs)
        {
            if (!await Analyzer.IsTwitterLink(messageArgs.Message.Content))
            {
                return;
            }

            string response = await Replace(messageArgs.Message.Content);

            try
            {
                await messageArgs.Message.ModifyEmbedSuppressionAsync(true);
            }
            catch (UnauthorizedException)
            {
                response += $"\n-# {Program.BotClient?.CurrentUser.Mention} doesn't have **Manage Messages** permission to manage duplicate embeds";
            }

            await messageArgs.Message.RespondAsync(response);
        }

        public static async Task MessageDelete(DiscordMessage deletedMessage)
        {
            DiscordChannel? channel = deletedMessage.Channel;

            DiscordMessage? message = channel?.GetMessagesAfterAsync(deletedMessage.Id).ToBlockingEnumerable().ToList().First();

            if (Program.BotClient?.CurrentUser is null || message is null)
            {
                return;
            }

            if (message.Author == Program.BotClient.CurrentUser)
            {
                await message.DeleteAsync();
            }
        }

        private static async Task<string> Replace(string content)
        {
            return await Task.Run(() =>
            {
                string[] words = content.Split(' ', '\n');

                foreach (string word in words)
                {
                    if (!Uri.TryCreate(word, UriKind.Absolute, out Uri? uriResult))
                    {
                        continue;
                    }

                    if (!(uriResult.Host == "twitter.com" || uriResult.Host == "x.com") || !uriResult.AbsolutePath.Contains("/status/"))
                    {
                        continue;
                    }

                    string newHost = uriResult.Host switch
                    {
                        "twitter.com" => "fxtwitter.com",
                        "x.com" => "fixupx.com",
                        _ => uriResult.Host
                    };

                    return new UriBuilder(uriResult) { Host = newHost }.Uri.ToString();
                }

                return string.Empty;
            });
        }

        private static partial class Analyzer
        {
            public static async Task<bool> IsTwitterLink(string content)
            {
                return await Task.Run(() =>
                {
                    if (string.IsNullOrEmpty(content))
                    {
                        return false;
                    }

                    return (content.Contains("https://x.com/") && content.Contains("/status/")) ||
                           (content.Contains("https://twitter.com/") && content.Contains("/status/"));
                });
            }
        }
    }
}
