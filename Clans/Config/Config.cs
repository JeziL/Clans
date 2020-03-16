using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Clans {
    public class ConfigFile {
        public string timestamp { get; set; }
        public string name { get; set; }
        public string url { get; set; }
        public Dictionary<string, string> selections { get; set; }

        [JsonIgnore]
        public string timeString {
            get {
                DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeMilliseconds(long.Parse(timestamp));
                DateTime dateTime = dateTimeOffset.UtcDateTime.ToLocalTime();
                return dateTime.ToString("yyyy-MM-dd HH:mm:ss");
            }
        }

        public ConfigFile(string timestamp, string name, string url) {
            this.timestamp = timestamp;
            this.name = name;
            this.url = url;
            selections = new Dictionary<string, string>();
        }
    }

    public class ConfigList {
        public int index { get; set; }
        public List<ConfigFile> files { get; set; }

        public ConfigList() {
            index = -1;
            files = new List<ConfigFile>();
        }
    }
}
