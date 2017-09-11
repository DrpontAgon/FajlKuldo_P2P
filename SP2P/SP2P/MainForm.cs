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
        bool listening = false;
        bool connected = false;
        bool port_open = false;
        List<string> file_paths = new List<string>();
        SimpleConnection sc;

        public MainForm()
        {
            InitializeComponent();
        }

        private async void MainForm_Load(object sender, EventArgs e)
        {
            IPChangeChecker.IPChanged += IPChangeEventMethod;
            IPChangeChecker.ForceCheck();
            if (await PortOpener.GetDevice())
            {
                if (!await PortOpener.CloseAllPortsExcept(Settings.Port))
                {
                    MessageBox.Show("Nem sikerült minden, nem alapértelmezett portot bezárni!");
                }
                port_open = await PortOpener.IsPortOpen();
                bt_port_openclose.Text = port_open ? "Port zárása" : "Port nyitása";
                bt_port_openclose.Enabled = true; 
            }
            else
            {
                MessageBox.Show("Port műveletek nem lehetségesek a programon belül!");
            }
        }

        private void IPChangeEventMethod(object sender, IPChangedEventArgs e)
        {
            lb_wan_inf.BackColor = lb_wan.BackColor = IPChangeChecker.PublicIpIsNone ? Color.Red : Color.LimeGreen;
            lb_lan_inf.BackColor = lb_lan.BackColor = IPChangeChecker.PrivateIpIsLoopback ? Color.Red : Color.LimeGreen;
            lb_wan.Text = IPChangeChecker.PublicIP.ToString();
            lb_lan.Text = IPChangeChecker.PrivateIP.ToString();
            //e.PublicIP.CheckAndLogIP();
        }

        private async void bt_port_openclose_Click(object sender, EventArgs e)
        {
            bt_port_openclose.Text = "folyamatban...";
            bt_port_openclose.Enabled = false;
            if (port_open)
            {
                if (await PortOpener.ClosePort())
                {
                    bt_port_openclose.Text = "Port nyitása";
                    port_open = false;
                }
                else
                {
                    bt_port_openclose.Text = "Port zárása";
                }
                bt_port_openclose.Enabled = true;
            }
            else
            {
                if (await PortOpener.OpenPort())
                {
                    bt_port_openclose.Text = "Port zárása";
                    port_open = true;
                }
                else
                {
                    bt_port_openclose.Text = "Port nyitása";
                }
                bt_port_openclose.Enabled = true;
            }
        }

        private async void bt_listen_Click(object sender, EventArgs e)
        {
            if (listening)
            {
                bt_listen.Text = "Kapcsolatra várás";
                bt_connect.Enabled = true;
                panel.Enabled = false;
                listening = false;
                sc.Close();
            }
            else
            {
                bt_listen.Text = "Várás befejezése";
                bt_connect.Enabled = false;
                while (listening)
                {
                    listening = true;
                    sc = new SimpleConnection(true);
                    if (await sc.AcceptAsync())
                    {
                        byte[] receive_bytes = new byte[1024];
                        byte[] send_bytes = new byte[1];
                        int received;
                        bool valid_response = false;

                        //received = await sc.ReceiveBytesAsync(receive_bytes);
                        //if (received == 1 && receive_bytes[0] == (byte)Message.CONNECT_REQUEST)
                        //{
                        //    send_bytes[0] = (byte)Message.OK;
                        //    await sc.SendBytesAsync(send_bytes);
                        //}
                        //else
                        //{
                        //    invalid_response = true;
                        //}

                        valid_response = await sc.MessageCommunicationAsync(Message.OK, Message.CONNECT_REQUEST, false);

                        int n = 0;

                        if (valid_response)
                        {
                            received = await sc.ReceiveBytesAsync(receive_bytes);
                            if (received == 1)
                            {
                                n = receive_bytes[0] * 4;
                                valid_response = await sc.MessageCommunicationAsync(Message.OK, true);
                            }
                            else
                            {
                                valid_response = false;
                            }
                        }

                        if (n > 1024)
                        {
                            receive_bytes = new byte[n];
                        }

                        bool connection_accepted = true;

                        if (valid_response)
                        {
                            received = await sc.ReceiveBytesAsync(receive_bytes);
                            if (received == n)
                            {
                                //IPShowForm f = new IPShowForm(...);
                                //f.ShowDialog();
                                //connection_accepted = f.Accepted;

                                if (connection_accepted /*f.Accepted*/)
                                {
                                    valid_response = await sc.MessageCommunicationAsync(Message.ANSWER_YES, Message.OK, true);
                                }
                                else
                                {
                                    valid_response = await sc.MessageCommunicationAsync(Message.ANSWER_NO, Message.OK, true);
                                }
                            }
                            else
                            {
                                valid_response = false;
                            }
                        }

                        if (connection_accepted)
                        {
                            panel.Enabled = true;
                        }

                        while (valid_response && connection_accepted)
                        {
                            received = await sc.ReceiveBytesAsync(receive_bytes);
                            if (received == 1)
                            {
                                switch (receive_bytes[0])
                                {
                                    case (byte)Message.FILE_TRANSFER_REQUEST:
                                        /*FileTransferOperation();*/
                                        break;
                                    case (byte)Message.DISCONNECT_REQUEST:
                                        MessageBox.Show("A másik fél lecsatlakozott!");
                                        connection_accepted = false;
                                        valid_response = await sc.MessageCommunicationAsync(Message.OK, true);
                                        break;
                                    default: valid_response = false; break;
                                }
                            }
                            else
                            {
                                valid_response = false;
                            }
                        }

                        if (!valid_response)
                        {
                            while (await sc.MessageCommunicationAsync(Message.INVALID, Message.OK, true)) { /*nothing*/ }
                        }

                        panel.Enabled = false;
                        sc.Close();
                    }
                    else
                    {
                        bt_listen.Text = "Kapcsolatra várás";
                        bt_connect.Enabled = true;
                        listening = false;
                        sc.Close();
                    }
                }
            }
        }

        private async void bt_connect_Click(object sender, EventArgs e)
        {
            if (connected)
            {
                bt_connect.Text = "Csatlakozás";
                bt_listen.Enabled = true;
                panel.Enabled = false;
                connected = false;
                sc.Close();
            }
            else
            {
                string s_ip = $"{tb_ip_1.Text}.{tb_ip_2.Text}.{tb_ip_3.Text}.{tb_ip_4.Text}";
                if (IPAddress.TryParse(s_ip, out IPAddress ip) && ushort.TryParse(tb_port.Text, out ushort port))
                {
                    if (port.PortInLimits(50000, 65535))
                    {
                        bt_connect.Text = "Lecsatlakozás";
                        bt_listen.Enabled = false;
                        connected = true;
                        sc = new SimpleConnection(false);
                        if (await sc.ConnectAsync(ip, port))
                        {
                            panel.Enabled = true;
                        }
                        else
                        {
                            bt_connect.Text = "Csatlakozás";
                            bt_listen.Enabled = true;
                            connected = false;
                            sc.Close();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Portszám csak 50000 és 65535 között lehet!");
                    }
                }
                else
                {
                    MessageBox.Show("Hibás IP cím vagy portszám!");
                }
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
