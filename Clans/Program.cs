using Clans;
using System;
using System.Windows.Forms;
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
    private Clash _clash;
    private Sysproxy _sysproxy;

    public ClanApplicationContext() {
        _clansMenuStrip = new ContextMenuStrip();
        _clansMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(menuOpening);

        _trayIcon = new NotifyIcon() {
            Icon = Clans.Properties.Resources.Clash,
            ContextMenuStrip = _clansMenuStrip,
            Visible = true
        };
    }

    void onExit(object sender, EventArgs e) {
        _trayIcon.Dispose();
        Application.Exit();
    }

    void menuOpening(object sender, System.ComponentModel.CancelEventArgs e) {
        _clansMenuStrip.Items.Clear();
        _clash = new Clash("127.0.0.1", 9090);
        _sysproxy = new Sysproxy(_clash.config.port);

        // 出站模式
        _clansMenuStrip.Items.Add("出站模式");
        ToolStripMenuItem direct_tsi = new ToolStripMenuItem("直接连接", null, null, "Direct");
        ToolStripMenuItem global_tsi = new ToolStripMenuItem("全局代理", null, null, "Global");
        ToolStripMenuItem rule_tsi = new ToolStripMenuItem("规则模式", null, null, "Rule");

        switch (_clash.config.mode) {
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

        ((ToolStripDropDownItem)(_clansMenuStrip.Items[0])).DropDownItems.Add(direct_tsi);
        ((ToolStripDropDownItem)(_clansMenuStrip.Items[0])).DropDownItems.Add(rule_tsi);
        ((ToolStripDropDownItem)(_clansMenuStrip.Items[0])).DropDownItems.Add(global_tsi);

        foreach (ToolStripMenuItem item in ((ToolStripDropDownItem)(_clansMenuStrip.Items[0])).DropDownItems) {
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

        // 退出
        _clansMenuStrip.Items.Add("退出");
        _clansMenuStrip.Items[_clansMenuStrip.Items.Count - 1].Click += new EventHandler(onExit);

        e.Cancel = false;
    }

    void modeSelected(object sender, EventArgs e) {
        string mode = ((ToolStripMenuItem)sender).Name;
        _clash.ChangeMode(mode);
    }

    void updateProxyList() {
        if (_clash.config.mode == "Direct") return;
        Dictionary<string, Proxy> proxies = _clash.GetProxies();

        if (_clash.config.mode == "Global") {
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
        else if (_clash.config.mode == "Rule") {
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

        _clash.ChangeProxy(groupName, proxyName);
    }

    void sysProxyChanged(object sender, EventArgs e) {
        ToolStripMenuItem tsi = (ToolStripMenuItem)sender;
        tsi.Checked = !tsi.Checked;
        _sysproxy.Enabled = tsi.Checked;
    }

    void copyCmdClicked(object sender, EventArgs e) {
        string cmd = $"set HTTP_PROXY=http://127.0.0.1:{_clash.config.port}\nset HTTPS_PROXY=http://127.0.0.1:{_clash.config.port}\n";
        Clipboard.SetText(cmd);
    }
}