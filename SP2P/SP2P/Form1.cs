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
    public partial class Form1 : Form
    {
        bool connected = false;
        List<string> file_paths = new List<string>();

        public Form1()
        {
            InitializeComponent();
            IPChangeChecker.IPChanged += IPChangeEventMethod;
            IPChangeChecker.ForceCheck();

            file_paths.Add("asd.txt");
            file_paths.Add("secret.part1");
            file_paths.Add("secret.part2");
            file_paths.Add("secret.crc");
        }

        private void IPChangeEventMethod(object sender, IPChangedEventArgs e)
        {
            lb_wan_inf.BackColor = lb_wan.BackColor = e.PublicIpIsNone ? Color.Red : Color.LimeGreen;
            lb_lan_inf.BackColor = lb_lan.BackColor = e.PrivateIpIsLoopback ? Color.Red : Color.LimeGreen;
            lb_wan.Text = e.PublicIP.ToString();
            lb_lan.Text = e.PrivateIP.ToString();
        }

        private void bt_connect_Click(object sender, EventArgs e)
        {
            connected = !connected;
            bt_connect.Text = connected ? "Lecsatlakozás" : "Csatlakozás";
            panel.Enabled = connected;
        }

        private void bt_one_send_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void bt_more_send_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void bt_one_info_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Az \"Egy fájl küldése\" gomb: Fájl kiválasztása és annak küldése.", "Információk");
        }

        private void bt_more_info_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Lista:\n\tBal egérgomb: Kijelölés.\n\tBal egérgomb + Ctrl: Több kijelölés.\n\tBal egérgomb + Shift: Kijelölttől kurzorig kijelölés.\n\tDupla Bal egérgomb: Fájl hozzáadása a listához.\n\tJobb egérgomb: Kijelöltek törlése.\n\nA \"Több fájl küldése\" gomb: Összes listában szereplő fájl küldése.", "Információk");
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
            throw new NotImplementedException();
        }
    }
}
