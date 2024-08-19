using Chirper.Commands;
using DSharpPlus;
using DSharpPlus.Commands;
using DSharpPlus.Commands.Processors.SlashCommands;
using Microsoft.Extensions.Logging;

namespace Chirper
{
    internal class Program
    {
        public static DiscordClient? BotClient { get; private set; }

        static async Task Main()
        {
            DiscordClientBuilder builder = DiscordClientBuilder.CreateDefault
            (
                token: tokens.discordToken,
                intents: DiscordIntents.AllUnprivileged | DiscordIntents.MessageContents | DiscordIntents.GuildMessages
            );

            builder.ConfigureEventHandlers
            (
                x =>
                x.HandleMessageCreated(async (client, args) =>
                {
                    if (args.Message.Author.IsBot)
                    {
                        return;
                    }

                    await Message.Handler.Run(args);
                }).

                HandleMessageDeleted(async (client, args) =>
                {
                    if (args.Message.Author.IsBot)
                    {
                        return;
                    }

                    try
                    {
                        await Message.Handler.MessageDelete(args.Message);
                    }
                    catch
                    {
                        BotClient?.Logger.LogWarning("Failed to delete a message");
                    }
                })
            );

            builder.UseCommands
            (
                extension =>
                {
                    extension.AddCommands([typeof(Settings)]);

                    SlashCommandProcessor slashCommandProcessor = new(new()
                    {
                        RegisterCommands = true
                    });

                    extension.AddProcessor(slashCommandProcessor);
                },

                new CommandsConfiguration()
                {
                    DebugGuildId = 766478619513585675
                }
            );

            BotClient = builder.Build();

            await BotClient.ConnectAsync();
            await Task.Delay(-1);
        }
    }
}
