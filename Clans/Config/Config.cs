using System.Collections.Generic;

namespace Clans {
    class ConfigFile {
        public string timestamp { get; set; }
        public string name { get; set; }
        public string url { get; set; }
        public Dictionary<string, string> selections { get; set; }
    }

    class ConfigList {
        public int index { get; set; }
        public List<ConfigFile> files { get; set; }

        public ConfigList() {
            index = -1;
            files = new List<ConfigFile>();
        }
    }
}
