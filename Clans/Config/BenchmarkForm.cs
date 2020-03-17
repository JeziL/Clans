using System;
using System.IO;
using System.Net.Http;
using System.Windows.Forms;
using YamlDotNet.Serialization;
using System.Collections.Generic;

namespace Clans {
    public partial class BenchmarkForm : Form {
        private ClanApplicationContext _appctxt;
        private string _currentConfig;
        private string _benchmarkURL;
        private ClashAPI _clashAPI;
        private Dictionary<string, Proxy> _proxies;
        private List<string> _groups;
        private List<ProxyBenchmarkResult> _results;

        public BenchmarkForm(ClanApplicationContext appctxt, ClashAPI api, string config) {
            Icon = Properties.Resources.Clash;
            InitializeComponent();
            _appctxt = appctxt;
            _currentConfig = config;
            _clashAPI = api;
            _groups = new List<string>();
            _results = new List<ProxyBenchmarkResult>();
        }

        private void BenchmarkForm_Load(object sender, EventArgs e) {
            string cfg = File.ReadAllText(_currentConfig);
            Dictionary<string, object> dict = new Deserializer().Deserialize<Dictionary<string, object>>(cfg);
            _benchmarkURL = dict.ContainsKey("cfw-latency-url") ? dict["cfw-latency-url"].ToString() : "http://www.gstatic.com/generate_204";

            _proxies = _clashAPI.GetProxies();
            if (_clashAPI.config.mode == "Global") {
                _groups.Add(_proxies["GLOBAL"].name);
            } else { // Rules
                foreach (Proxy proxy in _proxies.Values) {
                    if (proxy.proxyType == "Selector" && proxy.name != "GLOBAL") _groups.Add(proxy.name);
                }
            }

            groupListBox.DataSource = _groups;
            groupListBox.SelectedIndexChanged += new EventHandler(groupSelected);

            proxyGridView.AutoGenerateColumns = false;

            if (_groups.Count > 0) {
                groupListBox.SelectedIndex = 0;
                groupSelected(groupListBox, null);
            }
        }

        private void groupSelected(object sender, EventArgs e) {
            _results.Clear();
            string currentGroup = groupListBox.SelectedItem.ToString();
            foreach (string proxyName in _proxies[currentGroup].selections) {
                if (proxyName == _proxies[currentGroup].now) {
                    _results.Insert(0, new ProxyBenchmarkResult(proxyName));
                } else {
                    _results.Add(new ProxyBenchmarkResult(proxyName));
                }
            }
            refreshList();
        }

        private void refreshList() {
            proxyGridView.DataSource = null;
            proxyGridView.Update();
            proxyGridView.Refresh();
            proxyGridView.DataSource =_results;
        }

        private void proxyGridView_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e) {
            if (e.Button != MouseButtons.Right) return;
            proxyGridView.Rows[e.RowIndex].Selected = true;

            MenuItem startItem = new MenuItem("开始测试");
            startItem.Click += ((sender_, e_) => startBenchmark(sender_, e_, e.RowIndex));
            MenuItem setItem = new MenuItem("设为该组当前代理");
            setItem.Click += ((sender_, e_) => setProxy(sender_, e_, e.RowIndex));

            ContextMenu menu = new ContextMenu();
            menu.MenuItems.Add(startItem);
            menu.MenuItems.Add(setItem);

            menu.Show(proxyGridView, proxyGridView.PointToClient(Cursor.Position));
        }

        private void startBenchmark(object sender, EventArgs e, int index) {
            ProxyBenchmarkResult r = _results[index];
            r.delay = _clashAPI.GetDelay(r.name, 5000, _benchmarkURL);
            refreshList();
        }

        private void setProxy(object sender, EventArgs e, int index) {
            string groupName = groupListBox.SelectedItem.ToString();
            string proxyName = _results[index].name;
            _appctxt.ChangeProxy(groupName, proxyName);
        }
    }

    class ProxyBenchmarkResult {
        public string name { get; set; }
        public int delay { get; set; }

        public ProxyBenchmarkResult(string name) {
            this.name = name;
            delay = -1;
        }

        public string delayString {
            get {
                switch (delay) {
                    case -1:
                        return "";
                    case -2:
                        return "超时";
                    case -3:
                        return "错误";
                    default:
                        return $"{delay}ms";
                }
            }
            set { }
        }
    }
}
