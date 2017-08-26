using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SP2P
{
    public partial class SettingsForm : Form
    {
        string RelativePath;
        string ConfigPath;
        ushort Port;

        public SettingsForm()
        {
            InitializeComponent();

            RelativePath = Settings.RelativePath;
            ConfigPath = Settings.ConfigPath;
            Port = Settings.Port; tb_port.Text = Port.ToString();
        }

        private void bt_elfogad_Click(object sender, EventArgs e)
        {
            Port = Port.ParseIfPossible(tb_port.Text).ChangePortIfInLimits(50000, 65535, Port);

            Settings.AcceptSettings(Port);
            Close();
        }

        private void bt_megse_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void bt_alap_beall_Click(object sender, EventArgs e)
        {
            Port = 55585; tb_port.Text = "55585";
        }
    }
}
