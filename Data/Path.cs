using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chirper.Data
{
    public static class Path
    {
        private static readonly string basePath = AppDomain.CurrentDomain.BaseDirectory;
        private static readonly string settingsPath = $@"{basePath}/Settings";

        public static readonly string togglePath = $@"{settingsPath}/Toggle";
    }
}
