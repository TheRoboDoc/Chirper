using DSharpPlus.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chirper.Data
{
    public class SettingsManager
    {
        public SettingsManager()
        {
            if (!Directory.Exists(Path.togglePath))
            {
                Directory.CreateDirectory(Path.togglePath);
            }

            CreateToggleSettingsForGuilds();
        }

        private static void CreateToggleSettingsForGuilds()
        {
            List<ulong>? guildIDs = Program.BotClient?.Guilds.Keys.ToList();

            if (guildIDs == null || guildIDs.Count == 0)
            {
                Program.BotClient?.Logger.LogError("Failed to fetch guilds");

                return;
            }

            foreach (ulong guildID in guildIDs)
            {
                if (!Directory.Exists($"{Path.togglePath}/{guildID}"))
                {
                    Directory.CreateDirectory($"{Path.togglePath}/{guildID}");
                }
            }
        }

        public static async Task<Types.ChatToggle?> ReadToggleSetting(ulong guildID, ulong channelID)
        {
            if (Program.BotClient == null)
            {
                throw new Exception("OwO");
            } 

            DiscordGuild guild = await Program.BotClient.GetGuildAsync(guildID);

            string path = $"{Path.togglePath}/{guild.Id}/{channelID}.json";

            string? read = await FileManager.ReadFromFile(path);

            if (read == null)
            {
                return null;
            }

            Types.ChatToggle chatSetting = Converter.JsonToData<Types.ChatToggle>(read);

            return chatSetting;
        }
    }
}
