using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Exceptions;
using System.Text.RegularExpressions;

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
            DiscordChannel channel = deletedMessage.Channel;

            DiscordMessage message = channel.GetMessagesAfterAsync(deletedMessage.Id).ToBlockingEnumerable().ToList().First();

            if (Program.BotClient?.CurrentUser is null)
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
                Match? matches = TwitterOrXPattern().Match(content);

                string domain = matches.Groups["domain"].Value.Trim();
                string rest = matches.Groups["rest"].Value.Trim();

                string replacement = "";

                if (domain == "twitter.com")
                {
                    replacement = "https://fxtwitter.com" + rest;
                }
                else if (domain == "x.com")
                {
                    replacement = "https://fixupx.com" + rest;
                }

                return replacement;
            });
        }

        private static partial class Analyzer
        {
            public static async Task<bool> IsTwitterLink(string content)
            {
                if (content == null)
                {
                    return false;
                }

                return await Task.Run(() =>
                {
                    Regex regex = TwitterOrXPattern();
                    return regex.IsMatch(content);
                });
            }
        }

        [GeneratedRegex(@"((?:https?://)?(?:www\.)?(?:(?<domain>(?<!\w)x\.com|(?<!\w)twitter\.com)))(?<rest>\S+)")]
        private static partial Regex TwitterOrXPattern();
    }
}
