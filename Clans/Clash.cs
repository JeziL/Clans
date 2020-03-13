using System.Net.Http;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Clans {
    class ClashConfig {
        public int port { get; set; }
        [JsonProperty("socks-port")]
        public int socksPort { get; set; }
        public string mode { get; set; }
    }

    class Proxy {
        public string name { get; set; }
        [JsonProperty("type")]
        public string proxyType { get; set; }
        [JsonProperty("all")]
        public List<string> selections { get; set; }
        public string now { get; set; }

    }

    class Proxies {
        public Dictionary<string, Proxy> proxies { get; set; }
    }

    class Clash {
        private string _exUrl;
        private static readonly HttpClient _client = new HttpClient();
        public ClashConfig config { get; set; }

        private string request(string endpoint, string method = "GET", HttpContent content = null) {
            string url = $"{_exUrl}{endpoint}";
            HttpRequestMessage req = new HttpRequestMessage(new HttpMethod(method), url);
            req.Content = content;
            return _client.SendAsync(req).Result.Content.ReadAsStringAsync().Result;
        }

        public Clash(string url, int port) {
            _exUrl = $"http://{url}:{port}";

            string resp = request("/configs");
            config = JsonConvert.DeserializeObject<ClashConfig>(resp);
        }

        public void ChangeMode(string mode) {
            HttpContent content = new StringContent($"{{\"mode\":\"{mode}\"}}");
            _ = request("/configs", "PATCH", content);
        }

        public Dictionary<string, Proxy> GetProxies() {
            string resp = request("/proxies");
            return JsonConvert.DeserializeObject<Proxies>(resp).proxies;
        }

        public void ChangeProxy(string group, string proxy) {
            HttpContent content = new StringContent($"{{\"name\":\"{proxy}\"}}");
            string uri = string.Format("/proxies/{0}", group);
            _ = request(uri, "PUT", content);
        }
    }
}
