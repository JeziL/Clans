﻿using Clans;
using System;
using System.IO;
using Newtonsoft.Json;
using System.Windows.Forms;
using YamlDotNet.Serialization;
using System.Collections.Generic;

namespace Clans {
    static class Program {
        [STAThread]
        static void Main() {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ClanApplicationContext());
        }
    }
}

public class ClanApplicationContext : ApplicationContext {
    private NotifyIcon _trayIcon;
    private ContextMenuStrip _clansMenuStrip;
    private ClashAPI _clashAPI;
    private Sysproxy _sysproxy;

    private static string _configDir = Environment.ExpandEnvironmentVariables("%USERPROFILE%\\.config\\clash");
    private static string _configFile = Path.Combine(_configDir, "config.yaml");
    private static string _profileDir = Path.Combine(_configDir, "profiles");
    private static string _profileListFile = Path.Combine(_profileDir, "list.json");

    private string _currentConfig;
    private ConfigList _configList;

    private Clash _clash;

    public ClanApplicationContext() {
        _clansMenuStrip = new ContextMenuStrip();
        _clansMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(menuOpening);

        // 读取配置并开启 clash 进程
        readConfig();
        _clash = new Clash(_currentConfig);
        _clash.Start();

        // 读取 API 端口，并连接 API
        string cfg = File.ReadAllText(_currentConfig);
        Dictionary<string, object> dict = new Deserializer().Deserialize<Dictionary<string, object>>(cfg);
        string extController = dict["external-controller"].ToString();
        _clashAPI = new ClashAPI(extController); //TODO: external controller protentially not started yet.
        
        // 打开系统代理
        _sysproxy = new Sysproxy(_clashAPI.config.port);
        _sysproxy.Enabled = true;

        // 若当前托管配置记录了 selector 选择，则恢复选择状态
        if (_configList.index >= 0 && _configList.files[_configList.index].selections.Count > 0) {
            restoreSelections(_configList.files[_configList.index].selections);
        }

        _trayIcon = new NotifyIcon() {
            Icon = Clans.Properties.Resources.Clash,
            ContextMenuStrip = _clansMenuStrip,
            Visible = true
        };
    }

    void onExit(object sender, EventArgs e) {
        _sysproxy.Enabled = false;
        _clash.Stop();
        _trayIcon.Dispose();
        Application.Exit();
    }

    void menuOpening(object sender, System.ComponentModel.CancelEventArgs e) {
        _clansMenuStrip.Items.Clear();

        // 出站模式
        ToolStripItem mode_tsi = _clansMenuStrip.Items.Add("出站模式");
        ToolStripMenuItem direct_tsi = new ToolStripMenuItem("直接连接", null, null, "Direct");
        ToolStripMenuItem global_tsi = new ToolStripMenuItem("全局代理", null, null, "Global");
        ToolStripMenuItem rule_tsi = new ToolStripMenuItem("规则模式", null, null, "Rule");

        switch (_clashAPI.config.mode) {
            case "Global":
                global_tsi.Checked = true;
                break;
            case "Rule":
                rule_tsi.Checked = true;
                break;
            case "Direct":
                direct_tsi.Checked = true;
                break;
        }

        ((ToolStripDropDownItem)mode_tsi).DropDownItems.Add(direct_tsi);
        ((ToolStripDropDownItem)mode_tsi).DropDownItems.Add(rule_tsi);
        ((ToolStripDropDownItem)mode_tsi).DropDownItems.Add(global_tsi);

        foreach (ToolStripMenuItem item in ((ToolStripDropDownItem)mode_tsi).DropDownItems) {
            item.Click += new EventHandler(modeSelected);
        }
        _clansMenuStrip.Items.Add("-");

        // 节点列表
        updateProxyList();

        // 系统代理
        ToolStripMenuItem sys_tsi = new ToolStripMenuItem("设置为系统代理");
        sys_tsi.Checked = _sysproxy.Enabled;
        sys_tsi.Click += new EventHandler(sysProxyChanged);
        _clansMenuStrip.Items.Add(sys_tsi);

        // 复制代理命令
        ToolStripMenuItem cmd_tsi = new ToolStripMenuItem("复制终端代理命令");
        cmd_tsi.Click += new EventHandler(copyCmdClicked);
        _clansMenuStrip.Items.Add(cmd_tsi);
        _clansMenuStrip.Items.Add("-");

        // 配置管理
        ToolStripItem cfg_tsi = _clansMenuStrip.Items.Add("配置");
        ToolStripMenuItem switch_tsi = new ToolStripMenuItem("切换托管配置");
        if (_configList.index >= 0 && _configList.files.Count > 0) {
            // 存在有效的配置目录
            for (int i = 0; i < _configList.files.Count; i++) {
                string name = _configList.files[i].name;
                ToolStripMenuItem tsi = new ToolStripMenuItem(name);
                tsi.Tag = i;
                if (i == _configList.index) tsi.Checked = true;
                tsi.Click += new EventHandler(configChanged);
                switch_tsi.DropDownItems.Add(tsi);
            }
        } else {
            // 配置目录无效，禁用
            switch_tsi.Enabled = false;
        }
        ((ToolStripDropDownItem)cfg_tsi).DropDownItems.Add(switch_tsi);

        ToolStripMenuItem manage_tsi = new ToolStripMenuItem("管理...");
        manage_tsi.Click += new EventHandler(showConfigForm);
        ((ToolStripDropDownItem)cfg_tsi).DropDownItems.Add(manage_tsi);
        _clansMenuStrip.Items.Add("-");


        // 退出
        ToolStripItem exit_tsi = _clansMenuStrip.Items.Add("退出");
        exit_tsi.Click += new EventHandler(onExit);

        e.Cancel = false;
    }

    void modeSelected(object sender, EventArgs e) {
        string mode = ((ToolStripMenuItem)sender).Name;
        _clashAPI.ChangeMode(mode);
    }

    void updateProxyList() {
        if (_clashAPI.config.mode == "Direct") return;
        Dictionary<string, Proxy> proxies = _clashAPI.GetProxies();

        if (_clashAPI.config.mode == "Global") {
            Proxy global = proxies["GLOBAL"];

            _clansMenuStrip.Items.Add(global.name);
            foreach (string proxyName in global.selections) {
                ToolStripMenuItem tsi = new ToolStripMenuItem(proxyName, null, null, proxyName);
                tsi.Tag = global.name;
                if (proxyName == global.now) tsi.Checked = true;
                ((ToolStripDropDownItem)(_clansMenuStrip.Items[2])).DropDownItems.Add(tsi);
                tsi.Click += new EventHandler(proxySelected);
            }
        }
        else if (_clashAPI.config.mode == "Rule") {
            int i = 0;
            foreach (Proxy proxy in proxies.Values) {
                if (proxy.proxyType != "Selector" || proxy.name == "GLOBAL") continue;

                _clansMenuStrip.Items.Add(proxy.name);
                foreach (string proxyName in proxy.selections) {
                    ToolStripMenuItem tsi = new ToolStripMenuItem(proxyName, null, null, proxyName);
                    tsi.Tag = proxy.name;
                    if (proxyName == proxy.now) tsi.Checked = true;
                    ((ToolStripDropDownItem)(_clansMenuStrip.Items[i + 2])).DropDownItems.Add(tsi);
                    tsi.Click += new EventHandler(proxySelected);
                }
                i++;
            }
        }
        _clansMenuStrip.Items.Add("-");
    }

    void proxySelected(object sender, EventArgs e) {
        string proxyName = ((ToolStripMenuItem)sender).Name;
        string groupName = ((ToolStripMenuItem)sender).Tag.ToString();

        _clashAPI.ChangeProxy(groupName, proxyName);

        // 将代理选择更新至托管配置目录文件
        if (_configList.index >= 0) {
            _configList.files[_configList.index].selections[groupName] = proxyName;
            File.WriteAllText(_profileListFile, JsonConvert.SerializeObject(_configList));
        }
    }

    void sysProxyChanged(object sender, EventArgs e) {
        ToolStripMenuItem tsi = (ToolStripMenuItem)sender;
        tsi.Checked = !tsi.Checked;
        _sysproxy.Enabled = tsi.Checked;
    }

    void copyCmdClicked(object sender, EventArgs e) {
        string cmd = $"set HTTP_PROXY=http://127.0.0.1:{_clashAPI.config.port}\nset HTTPS_PROXY=http://127.0.0.1:{_clashAPI.config.port}\n";
        Clipboard.SetText(cmd);
    }

    void readConfig() {
        // 检查是否存在配置文件夹，若无，则创建并导入默认初始配置及数据库
        if (!Directory.Exists(_configDir)) {
            Directory.CreateDirectory(_configDir);
            File.Copy(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "Resources\\config.yaml"), _configFile, true);
            File.Copy(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "Resources\\Country.mmdb"), Path.Combine(_configDir, "Country.mmdb"), true);
        }
        // 检查是否存在托管配置文件夹，若无，创建并建立初始托管配置目录
        if (!Directory.Exists(_profileDir)) {
            Directory.CreateDirectory(_profileDir);
            File.WriteAllText(_profileListFile, JsonConvert.SerializeObject(new ConfigList()));
        }

        // 读取托管配置目录
        string cfgStr = File.ReadAllText(_profileListFile);
        _configList = JsonConvert.DeserializeObject<ConfigList>(cfgStr);
        if (_configList.index < 0 || _configList.files.Count == 0) {
            // 目录为空，将当前配置指向默认初始配置
            _currentConfig = _configFile;
        }
        else {
            // 目录不为空，设置当前配置
            string timestamp = _configList.files[_configList.index].timestamp;
            _currentConfig = Path.Combine(_profileDir, $"{timestamp}.yaml");
        }
    }

    void restoreSelections(Dictionary<string, string> selections) {
        foreach (KeyValuePair<string, string> selection in selections) {
            _clashAPI.ChangeProxy(selection.Key, selection.Value);
        }
    }

    void configChanged(object sender, EventArgs e) {
        ToolStripMenuItem tsi = (ToolStripMenuItem)sender;

        _configList.index = (int)(tsi.Tag);
        string timestamp = _configList.files[_configList.index].timestamp;
        File.WriteAllText(_profileListFile, JsonConvert.SerializeObject(_configList));

        _currentConfig = Path.Combine(_profileDir, $"{timestamp}.yaml");
        _clash.ReloadConfig(_currentConfig);

        // 读取 API 端口，并连接 API
        string cfg = File.ReadAllText(_currentConfig);
        Dictionary<string, object> dict = new Deserializer().Deserialize<Dictionary<string, object>>(cfg);
        string extController = dict["external-controller"].ToString();
        _clashAPI.UpdateURL(extController); //TODO: external controller protentially not started yet.

        // 更新系统代理
        _sysproxy = new Sysproxy(_clashAPI.config.port);
        _sysproxy.Enabled = true;

        // 若当前托管配置记录了 selector 选择，则恢复选择状态
        if (_configList.index >= 0 && _configList.files[_configList.index].selections.Count > 0) {
            restoreSelections(_configList.files[_configList.index].selections);
        }
    }

    void showConfigForm(object sender, EventArgs e) {
        ConfigForm form = new ConfigForm(_configList);
        form.Show();
    }
}