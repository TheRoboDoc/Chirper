using Chirper.SlashCommands;
using DSharpPlus;
using DSharpPlus.SlashCommands;

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

                Intents = DiscordIntents.AllUnprivileged | DiscordIntents.MessageContents,

                LogUnknownEvents = false
            });

            BotClient.MessageCreated += async (client, args) =>
            {
                if (args.Message.Author.IsBot)
                {
                    return;
                }

                await Message.Handler.Run(args);
            };

            SlashCommandsExtension slashCommands = BotClient.UseSlashCommands();

            slashCommands.RegisterCommands<Settings>();

            await BotClient.ConnectAsync();
            await Task.Delay(-1);
        }
    }
}
