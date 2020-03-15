using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Clans {
    public partial class ConfigForm : Form {
        private ConfigList _configList;

        public ConfigForm(ConfigList configList) {
            InitializeComponent();
            _configList = configList;
        }

        private void ConfigForm_Load(object sender, EventArgs e) {
            configGridView.AutoGenerateColumns = false;
            configGridView.SelectionChanged += new EventHandler(configSelected);
            configGridView.DataSource = _configList.files;
        }

        private void configSelected(object sender, EventArgs e) {
            if (configGridView.SelectedRows.Count > 0) {
                deleteBtn.Enabled = true;
                updateBtn.Enabled = true;
            } else {
                deleteBtn.Enabled = false;
                updateBtn.Enabled = false;
            }
        }
    }
}
