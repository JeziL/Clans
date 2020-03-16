﻿using Newtonsoft.Json;
using System.Net.Http;
using System.Collections.Generic;

namespace Clans {
    class ClashConfig {
        public int port { get; set; }
        [JsonProperty("socks-port")]
        public int socksPort { get; set; }
        public string mode { get; set; }
        [JsonProperty("allow-lan")]
        public bool allowLAN { get; set; }
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

    class ClashAPI {
        private string _exUrl;
        private static readonly HttpClient _client = new HttpClient();
        public ClashConfig config {
            get {
                string resp = request("/configs");
                return JsonConvert.DeserializeObject<ClashConfig>(resp);
            }
            set { }
        }

        private string request(string endpoint, string method = "GET", HttpContent content = null) {
            string url = $"{_exUrl}{endpoint}";
            HttpRequestMessage req = new HttpRequestMessage(new HttpMethod(method), url);
            req.Content = content;
            return _client.SendAsync(req).Result.Content.ReadAsStringAsync().Result;
        }

        public ClashAPI(string url) {
            _exUrl = url.StartsWith("http") ? url : $"http://{url}";
        }

        public void ChangeMode(string mode) {
            HttpContent content = new StringContent($"{{\"mode\":\"{mode}\"}}");
            _ = request("/configs", "PATCH", content);
        }

        public void ChangeAllowLAN(bool allow) {
            HttpContent content = new StringContent($"{{\"allow-lan\":{allow}}}".ToLower());
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

        public void UpdateURL(string url) {
            _exUrl = url.StartsWith("http") ? url : $"http://{url}";

            string resp = request("/configs");
            config = JsonConvert.DeserializeObject<ClashConfig>(resp);
        }
    }
}
