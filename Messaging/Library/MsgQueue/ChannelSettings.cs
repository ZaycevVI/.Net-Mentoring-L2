using System.Collections.Generic;

namespace Library.MsgQueue
{
    public class ChannelSettings
    {
        private readonly Dictionary<ChannelType, string> _types = 
            new Dictionary<ChannelType, string>
        {
            [ChannelType.Direct] = "direct",
            [ChannelType.Fanout] = "fanout"
        };

        public ChannelSettings(string exchange, ChannelType type)
        {
            Exchange = exchange;
            Type = _types[type];
        }

        public string Exchange { get; set; }
        public string Type { get; set; }
    }

    public enum ChannelType
    {
        Direct,
        Fanout
    }
}