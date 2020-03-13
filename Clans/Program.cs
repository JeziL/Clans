using Clans;
using System;
using System.Windows.Forms;
using System.Collections.Generic;

namespace Clans {
    static class Program {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ClanApplicationContext());
        }
    }
}

public class ClanApplicationContext : ApplicationContext {
    private NotifyIcon trayIcon;
    private ContextMenuStrip clansMenuStrip;
    private Clash clash;
    private Sysproxy sysproxy;

    public ClanApplicationContext() {
        clansMenuStrip = new ContextMenuStrip();
        clansMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(menuOpening);

        trayIcon = new NotifyIcon() {
            Icon = Clans.Properties.Resources.Clash,
            ContextMenuStrip = clansMenuStrip,
            Visible = true
        };
    }

    void onExit(object sender, EventArgs e) {
        trayIcon.Dispose();
        Application.Exit();
    }

    void menuOpening(object sender, System.ComponentModel.CancelEventArgs e) {
        clansMenuStrip.Items.Clear();
        clash = new Clash("127.0.0.1", 9090);
        sysproxy = new Sysproxy(clash.config.port);

        // 出站模式
        clansMenuStrip.Items.Add("出站模式");
        ToolStripMenuItem direct_tsi = new ToolStripMenuItem("直接连接", null, null, "Direct");
        ToolStripMenuItem global_tsi = new ToolStripMenuItem("全局代理", null, null, "Global");
        ToolStripMenuItem rule_tsi = new ToolStripMenuItem("规则模式", null, null, "Rule");

        switch (clash.config.mode) {
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

        ((ToolStripDropDownItem)(clansMenuStrip.Items[0])).DropDownItems.Add(direct_tsi);
        ((ToolStripDropDownItem)(clansMenuStrip.Items[0])).DropDownItems.Add(rule_tsi);
        ((ToolStripDropDownItem)(clansMenuStrip.Items[0])).DropDownItems.Add(global_tsi);

        foreach (ToolStripMenuItem item in ((ToolStripDropDownItem)(clansMenuStrip.Items[0])).DropDownItems) {
            item.Click += new EventHandler(modeSelected);
        }
        clansMenuStrip.Items.Add("-");

        // 节点列表
        updateProxyList();

        // 系统代理
        ToolStripMenuItem sys_tsi = new ToolStripMenuItem("设置为系统代理");
        sys_tsi.Checked = sysproxy.Enabled;
        sys_tsi.Click += new EventHandler(sysProxyChanged);
        clansMenuStrip.Items.Add(sys_tsi);

        // 复制代理命令
        ToolStripMenuItem cmd_tsi = new ToolStripMenuItem("复制终端代理命令");
        cmd_tsi.Click += new EventHandler(copyCmdClicked);
        clansMenuStrip.Items.Add(cmd_tsi);
        clansMenuStrip.Items.Add("-");

        // 退出
        clansMenuStrip.Items.Add("退出");
        clansMenuStrip.Items[clansMenuStrip.Items.Count - 1].Click += new EventHandler(onExit);

        e.Cancel = false;
    }

    void modeSelected(object sender, EventArgs e) {
        string mode = ((ToolStripMenuItem)sender).Name;
        clash.ChangeMode(mode);
    }

    void updateProxyList() {
        if (clash.config.mode == "Direct") return;
        Dictionary<string, Proxy> proxies = clash.GetProxies();

        if (clash.config.mode == "Global") {
            Proxy global = proxies["GLOBAL"];

            clansMenuStrip.Items.Add(global.name);
            foreach (string proxyName in global.selections) {
                ToolStripMenuItem tsi = new ToolStripMenuItem(proxyName, null, null, proxyName);
                tsi.Tag = global.name;
                if (proxyName == global.now) tsi.Checked = true;
                ((ToolStripDropDownItem)(clansMenuStrip.Items[2])).DropDownItems.Add(tsi);
                tsi.Click += new EventHandler(proxySelected);
            }
        }
        else if (clash.config.mode == "Rule") {
            int i = 0;
            foreach (Proxy proxy in proxies.Values) {
                if (proxy.proxyType != "Selector" || proxy.name == "GLOBAL") continue;

                clansMenuStrip.Items.Add(proxy.name);
                foreach (string proxyName in proxy.selections) {
                    ToolStripMenuItem tsi = new ToolStripMenuItem(proxyName, null, null, proxyName);
                    tsi.Tag = proxy.name;
                    if (proxyName == proxy.now) tsi.Checked = true;
                    ((ToolStripDropDownItem)(clansMenuStrip.Items[i + 2])).DropDownItems.Add(tsi);
                    tsi.Click += new EventHandler(proxySelected);
                }
                i++;
            }
        }
        clansMenuStrip.Items.Add("-");
    }

    void proxySelected(object sender, EventArgs e) {
        string proxyName = ((ToolStripMenuItem)sender).Name;
        string groupName = ((ToolStripMenuItem)sender).Tag.ToString();

        clash.ChangeProxy(groupName, proxyName);
    }

    void sysProxyChanged(object sender, EventArgs e) {
        ToolStripMenuItem tsi = (ToolStripMenuItem)sender;
        tsi.Checked = !tsi.Checked;
        sysproxy.Enabled = tsi.Checked;
    }

    void copyCmdClicked(object sender, EventArgs e) {
        string cmd = $"set HTTP_PROXY=http://127.0.0.1:{clash.config.port}\nset HTTPS_PROXY=http://127.0.0.1:{clash.config.port}\n";
        Clipboard.SetText(cmd);
    }
}