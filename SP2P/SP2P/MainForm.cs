using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SP2P
{
    public partial class MainForm : Form
    {
        bool connected = false;
        List<string> file_paths = new List<string>();

        public MainForm()
        {
            InitializeComponent();
            IPChangeChecker.IPChanged += IPChangeEventMethod;
            IPChangeChecker.ForceCheck();
        }

        //Ha esetleg valamikor később szükség lenne a label-ek frissítésére a main thread-en.
        //Ha nem, akkor törölve lesz, majd ha működik a program.
        //private void Update_WAN_LAN()
        //{
        //    lb_wan.Invalidate();
        //    lb_wan.Update();
        //    lb_wan.Refresh();
        //    lb_lan.Invalidate();
        //    lb_lan.Update();
        //    lb_lan.Refresh();
        //    Application.DoEvents();
        //}

        private void IPChangeEventMethod(object sender, IPChangedEventArgs e)
        {
            lb_wan_inf.BackColor = lb_wan.BackColor = IPChangeChecker.PublicIpIsNone ? Color.Red : Color.LimeGreen;
            lb_lan_inf.BackColor = lb_lan.BackColor = IPChangeChecker.PrivateIpIsLoopback ? Color.Red : Color.LimeGreen;
            lb_wan.Text = IPChangeChecker.PublicIP.ToString();
            lb_lan.Text = IPChangeChecker.PrivateIP.ToString();
            //e.PublicIP.CheckAndLogIP();
        }

        private void bt_connect_Click(object sender, EventArgs e)
        {
            string s_ip = $"{tb_ip_1.Text}.{tb_ip_2.Text}.{tb_ip_3.Text}.{tb_ip_4.Text}";
            if (IPAddress.TryParse(s_ip, out IPAddress ip) && ushort.TryParse(tb_port.Text, out ushort port))
            {
                connected = !connected;
                bt_connect.Text = connected ? "Szétválasztás" : "Csatlakozás";
                panel.Enabled = connected;
            }
        }

        private void bt_send_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void bt_send_info_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Lista:\n\tBal egérgomb: Kijelölés." +
                                  "\n\tBal egérgomb + Ctrl: Több kijelölés." +
                                  "\n\tBal egérgomb + Shift: Kijelölttől kurzorig kijelölés." +
                                  "\n\tDupla Bal egérgomb: Fájl hozzáadása a listához." +
                                  "\n\tJobb egérgomb: Kijelöltek törlése." +
                                  "\n\nA \"Több fájl küldése\" gomb: Összes listában szereplő fájl küldése.", "Információk");
        }

        private void lb_files_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                int c = lb_files.SelectedItems.Count;
                for (int i = 0; i < c; i++)
                {
                    int index = lb_files.SelectedIndex;
                    lb_files.Items.RemoveAt(index);
                    file_paths.RemoveAt(index);
                }
            }
        }

        private void lb_files_DoubleClick(object sender, EventArgs e)
        {
            ofd.Multiselect = true;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                foreach (var item in ofd.FileNames)
                {
                    file_paths.Add(item);
                }
                foreach (var item in ofd.SafeFileNames)
                {
                    lb_files.Items.Add(item);
                }
            }
        }

        private void tb_ip_1_TextChanged(object sender, EventArgs e)
        {
            //if (tb_ip_1.Text.Length == 3) tb_ip_2.Focus();
        }

        private void tb_ip_2_TextChanged(object sender, EventArgs e)
        {
            //if (tb_ip_2.Text.Length == 3) tb_ip_3.Focus();
        }

        private void tb_ip_3_TextChanged(object sender, EventArgs e)
        {
            //if (tb_ip_3.Text.Length == 3) tb_ip_4.Focus();
        }

        private void tb_ip_4_TextChanged(object sender, EventArgs e)
        {
            //if (tb_ip_4.Text.Length == 3) tb_port.Focus();
        }

        private void tb_port_TextChanged(object sender, EventArgs e)
        {
            //if (tb_port.Text.Length == 5) bt_connect.Focus();
        }

        private void cms_item_ip_Click(object sender, EventArgs e)
        {
            IPChangeChecker.ForceCheck();
        }

        private void cms_item_settings_Click(object sender, EventArgs e)
        {
            SettingsForm f = new SettingsForm();
            f.ShowDialog();
            f.Dispose();
        }

        private void cms_Opening(object sender, CancelEventArgs e)
        {
            if (lb_files.ClientRectangle.Contains(lb_files.PointToClient(MousePosition)))
            {
                e.Cancel = true;
            }
        }
    }
}
