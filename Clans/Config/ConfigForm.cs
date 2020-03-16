using System;
using System.Windows.Forms;

namespace Clans {
    public partial class ConfigForm : Form {
        private ConfigList _configList;
        private ClanApplicationContext _appctxt;

        public ConfigForm(ClanApplicationContext appctxt, ConfigList configList) {
            InitializeComponent();
            _appctxt = appctxt;
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
        private void refreshList() {
            configGridView.DataSource = null;
            configGridView.Update();
            configGridView.Refresh();
            configGridView.DataSource = _configList.files;
        }

        private void deleteBtn_Click(object sender, EventArgs e) {
            int index = configGridView.CurrentRow.Index;
            _appctxt.DeleteConfigList(index);
            refreshList();
        }

        private void addBtn_Click(object sender, EventArgs e) {
            AddConfigForm form = new AddConfigForm();
            if (form.ShowDialog(this) == DialogResult.OK) {
                string name = form.nameLabel.Text;
                string url = form.urlLabel.Text;
                _appctxt.AddConfig(name, url);
                refreshList();
            }
            form.Dispose();
        }

        private void updateBtn_Click(object sender, EventArgs e) {
            int index = configGridView.CurrentRow.Index;
            _appctxt.UpdateConfig(index);
            refreshList();
        }
    }
}
