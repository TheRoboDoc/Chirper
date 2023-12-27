using DSharpPlus.EventArgs;
using System.Text.RegularExpressions;

namespace Chirper.Message
{
    public static partial class Handler
    {
        public static async Task Run(MessageCreateEventArgs messageArgs)
        {
            if (!await Analyzier.IsTwitterLink(messageArgs.Message.Content))
            {
                return;
            }

            await messageArgs.Message.RespondAsync(await Replace(messageArgs.Message.Content));
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

        private static partial class Analyzier
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

        [GeneratedRegex(@"((?:https?://)?(?:www\.)?(?:(?<domain>twitter\.com|x\.com))(?<rest>\S+))")]
        private static partial Regex TwitterOrXPattern();
    }
}
