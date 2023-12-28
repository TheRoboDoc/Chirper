using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chirper.Data
{
    public static class Types
    {
        public struct ChatToggle
        {
            public ulong ChannelID { get; set; }

            public bool Enabled { get; set; }
        }
    }
}
