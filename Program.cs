using DSharpPlus;
using Microsoft.Extensions.Logging;

namespace Chirper
{
    internal class Program
    {
        public static DiscordClient? BotClient { get; private set; }

        static async Task Main()
        {
            BotClient = new DiscordClient(new DiscordConfiguration()
            {
                Token = tokens.discordToken,
                TokenType = TokenType.Bot,
                Intents =
                    DiscordIntents.MessageContents |
                    DiscordIntents.GuildMessages |
                    DiscordIntents.DirectMessages,
            });

            BotClient.MessageCreated += async (client, args) =>
            {
                if (args.Message.Author.IsBot)
                {
                    return;
                }

                BotClient.Logger.LogInformation("Message triggered");

                await Message.Handler.Run(args);
            };

            await BotClient.ConnectAsync();
            await Task.Delay(-1);
        }
    }
}
