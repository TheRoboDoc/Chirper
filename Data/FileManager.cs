using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chirper.Data
{
    public static class FileManager
    {
        public static bool FileExists(string path)
        {
            return File.Exists(path);
        }

        public static async Task CreateFile(string path)
        {
            FileInfo fileInfo = new(path);

            await fileInfo.Create().DisposeAsync();
        }

        public static void DeleteFile(string path)
        {
            File.Delete(path);
        }

        public static async Task WriteToFile(string path, object data, bool createFile = true)
        {
            if (!FileExists(path) && !createFile)
            {
                Program.BotClient?.Logger.LogError("Tried writing to {path}, which doesn't exist", path);

                return;
            }

            await CreateFile(path);

            string? json = Converter.DataToJson(data);

            using StreamWriter writer = new(path);

            await writer.WriteAsync(json);
        }

        public static async Task<string?> ReadFromFile(string path)
        {
            if (!FileExists(path))
            {
                Program.BotClient?.Logger.LogError("Tried reading {path}, which doesn't exist", path);

                return null;
            }

            using StreamReader reader = new(path);

            return await reader.ReadToEndAsync();
        }
    }
}
