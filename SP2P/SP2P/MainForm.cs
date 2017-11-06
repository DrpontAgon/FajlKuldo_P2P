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

        public MainForm()
        {
            InitializeComponent();
            RuntimeLogger.Open("log.txt");
            RuntimeLogger.WriteLine("Program Start.");
            IPChangeChecker.IPChanged += IPChangeEventMethod;
            IPChangeChecker.ForceCheck();
            SimpleConnection.IsSilent = false;
        }

        private async void MainForm_Load(object sender, EventArgs e)
        {
            RuntimeLogger.WriteLine("Trying to get NAT device...");
            if (await PortOpener.GetDevice())
            {
                RuntimeLogger.WriteLine("Successfully found NAT device.");
                if (!await PortOpener.CloseAllPortsExcept(Settings.Port))
                {
                    MessageBox.Show("Nem sikerült minden, nem alapértelmezett portot bezárni!");
                }
                port_open = await PortOpener.IsPortOpen();
                RuntimeLogger.WriteLine($"{Settings.Port} is open: {port_open}");
                bt_port_openclose.Text = port_open ? "Port zárása" : "Port nyitása";
                bt_port_openclose.Enabled = true; 
            }
            else
            {
                RuntimeLogger.WriteLine("NAT device not found.");
                MessageBox.Show("Port műveletek nem lehetségesek a programon belül!");
            }
        }

        private void IPChangeEventMethod(object sender, IPChangedEventArgs e)
        {
            lb_wan_inf.BackColor = lb_wan.BackColor = IPChangeChecker.PublicIpIsNone ? Color.Red : Color.LimeGreen;
            lb_lan_inf.BackColor = lb_lan.BackColor = IPChangeChecker.PrivateIpIsLoopback ? Color.Red : Color.LimeGreen;
            lb_wan.Text = IPChangeChecker.PublicIP.ToString();
            lb_lan.Text = IPChangeChecker.PrivateIP.ToString();
            IPChangeChecker.PublicIP.CheckAndStoreIP();
            //IPChangeChecker.PrivateIP.CheckAndStoreIP();
            RuntimeLogger.WriteLine($"IP changed.");
        }

        private async void bt_port_openclose_Click(object sender, EventArgs e)
        {
            bt_port_openclose.Text = "folyamatban...";
            bt_port_openclose.Enabled = false;
            if (port_open)
            {
                RuntimeLogger.WriteLine($"Closing port...");
                if (await PortOpener.ClosePort())
                {
                    RuntimeLogger.WriteLine($"Port successfully closed.");
                    bt_port_openclose.Text = "Port nyitása";
                    port_open = false;
                }
                else
                {
                    RuntimeLogger.WriteLine($"Port could not be closed.");
                    bt_port_openclose.Text = "Port zárása";
                }
                bt_port_openclose.Enabled = true;
            }
            else
            {
                RuntimeLogger.WriteLine($"Opening port...");
                if (await PortOpener.OpenPort())
                {
                    RuntimeLogger.WriteLine($"Port successfully opened.");
                    bt_port_openclose.Text = "Port zárása";
                    port_open = true;
                }
                else
                {
                    RuntimeLogger.WriteLine($"Port successfully opened.");
                    bt_port_openclose.Text = "Port nyitása";
                }
                bt_port_openclose.Enabled = true;
            }
        }

        private async void bt_listen_Click(object sender, EventArgs e)
        {
            if (listening)
            {
                RuntimeLogger.WriteLine("Sending DISCONNECT_REQUEST...");
                bool valid_response = await SimpleConnection.MessageCommunication(Message.DISCONNECT_REQUEST, Message.OK, true);
                if (SimpleConnection.IsConnected)
                {
                    if (!valid_response)
                    {
                        MessageBox.Show("Hiba történt a lecsatlakozáskor!");
                    }
                }
                bt_listen.Text = "Kapcsolatra várás";
                bt_connect.Enabled = true;
                panel.Enabled = false;
                listening = false;
                SimpleConnection.Close();
                RuntimeLogger.WriteLine($"Stopped waiting for connection.");
            }
            else
            {
                RuntimeLogger.WriteLine($"Started waiting for connection...");
                bt_listen.Text = "Várás befejezése";
                bt_connect.Enabled = false;
                listening = true;
                bool connection_accepted = true;
                while (listening)
                {
                    if (connection_accepted)
                    {
                        await SimpleConnection.Accept();
                        RuntimeLogger.WriteLine($"Connection started.");
                        byte[] receive_bytes = new byte[1024];
                        byte[] send_bytes = new byte[1];
                        int received = 0;
                        bool valid_response;

                        RuntimeLogger.WriteLine("Waiting for Connection request...");
                        valid_response = await SimpleConnection.MessageCommunication(Message.OK, Message.CONNECT_REQUEST, false);
                        RuntimeLogger.WriteLine(SimpleConnection.FormatCommunication(valid_response, Message.OK, Message.CONNECT_REQUEST, false));

                        int n = 0;

                        if (valid_response)
                        {
                            received = await SimpleConnection.ReceiveBytes(receive_bytes);
                            RuntimeLogger.WriteLine($"Expected to receive 4 bytes, received {received} byte(s).");
                            if (received == 4)
                            {
                                n = BitConverter.ToInt32(receive_bytes, 0) * 4;
                                RuntimeLogger.WriteLine($"Expecting to receive {n} byte(s) later.");
                                valid_response = await SimpleConnection.MessageCommunication(Message.OK, true);
                                RuntimeLogger.WriteLine($"Validity: {valid_response}.");
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

                        if (valid_response)
                        {
                            received = await SimpleConnection.ReceiveBytes(receive_bytes);
                            RuntimeLogger.WriteLine($"Expected to receive {n} byte(s), received {received} byte(s).");
                            if (received == n)
                            {
                                //string ip_strings = "Elfogadja-e a kapcsolatot ettől a féltől?\n";
                                byte[] ip_bytes;

                                //for (int i = 0; i < n / 4; i++)
                                //{
                                //    ip_bytes = new byte[] { receive_bytes[i * 4 + 0], receive_bytes[i * 4 + 1], receive_bytes[i * 4 + 2], receive_bytes[i * 4 + 3] };
                                //    ip_strings += $"\t{(new IPAddress(ip_bytes))}\n";
                                //}
                                //connection_accepted = MessageBox.Show(ip_strings, "Csatlakozás", MessageBoxButtons.YesNo) == DialogResult.Yes;

                                //RuntimeLogger.WriteLine($"Connection Accepted: {connection_accepted}.");


                                IPAddress[] ips = new IPAddress[n / 4];
                                for (int i = 0; i < ips.Length; i++)
                                {
                                    ip_bytes = new byte[] { receive_bytes[i * 4 + 0], receive_bytes[i * 4 + 1], receive_bytes[i * 4 + 2], receive_bytes[i * 4 + 3] };
                                    ips[i] = new IPAddress(ip_bytes);
                                }
                                using (IPShowForm f = new IPShowForm(ips))
                                {
                                    f.ShowDialog();
                                    connection_accepted = f.DialogResult == DialogResult.Yes;
                                }

                                RuntimeLogger.WriteLine($"Sending the answer back...");
                                if (connection_accepted)
                                {
                                    valid_response = await SimpleConnection.MessageCommunication(Message.ANSWER_YES, Message.OK, true);
                                    RuntimeLogger.WriteLine(SimpleConnection.FormatCommunication(valid_response, Message.ANSWER_YES, Message.OK, true));
                                    //valid_response = await sc.MessageCommunicationAsync(Message.ANSWER_YES, true);
                                    //if (valid_response)
                                    //{
                                    //    valid_response = await sc.ValidateCommunicationAsync(false);
                                    //}
                                }
                                else
                                {
                                    valid_response = await SimpleConnection.MessageCommunication(Message.ANSWER_NO, Message.OK, true);
                                    RuntimeLogger.WriteLine(SimpleConnection.FormatCommunication(valid_response, Message.ANSWER_NO, Message.OK, true));
                                    //valid_response = await sc.MessageCommunicationAsync(Message.ANSWER_NO, true);
                                    //if (valid_response)
                                    //{
                                    //    valid_response = await sc.ValidateCommunicationAsync(false);
                                    //}
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
                            RuntimeLogger.WriteLine("Waiting for other client to respond...");
                            received = await SimpleConnection.ReceiveBytes(receive_bytes);
                            RuntimeLogger.WriteLine($"Expected to receive 1 byte, received {received} byte(s).");
                            if (received == 1)
                            {
                                switch (receive_bytes[0])
                                {
                                    case (byte)Message.FILE_TRANSFER_REQUEST:
                                        valid_response = await SimpleConnection.MessageCommunication(Message.OK, true);
                                        if (valid_response)
                                        {
                                            /*await FileTransferOperation();*/
                                        }
                                        break;
                                    case (byte)Message.DISCONNECT_REQUEST:
                                        RuntimeLogger.WriteLine($"Other Client requested to disconnect.");
                                        MessageBox.Show("A másik fél lecsatlakozott!");
                                        connection_accepted = false;
                                        valid_response = await SimpleConnection.MessageCommunication(Message.OK, true);
                                        break;
                                    default:
                                        RuntimeLogger.WriteLine($"Invalid answer.");
                                        valid_response = false;
                                        break;
                                }
                            }
                            else
                            {
                                valid_response = false;
                            }
                        }

                        if (!valid_response)
                        {
                            RuntimeLogger.WriteLine($"Attempting to send back INVALID.");
                            while (await SimpleConnection.MessageCommunication(Message.INVALID, Message.OK, true)) { /*nothing*/ }
                        }

                        panel.Enabled = false;
                        SimpleConnection.Close();
                    } // await sc.AcceptAsync()
                    else
                    {
                        RuntimeLogger.WriteLine($"Connection closed.");
                        bt_listen.Text = "Kapcsolatra várás";
                        bt_connect.Enabled = true;
                        listening = false;
                        SimpleConnection.Close();
                    }
                } // listening
            }
        }

        private async void bt_connect_Click(object sender, EventArgs e)
        {
            if (connected)
            {
                RuntimeLogger.WriteLine("Sending DISCONNECT_REQUEST...");
                bool valid_response = await SimpleConnection.MessageCommunication(Message.DISCONNECT_REQUEST, Message.OK, true);
                if (!valid_response)
                {
                    MessageBox.Show("Hiba történt a lecsatlakozáskor!");
                }
                bt_connect.Text = "Csatlakozás";
                bt_listen.Enabled = true;
                panel.Enabled = false;
                connected = false;
                SimpleConnection.Close();
                RuntimeLogger.WriteLine($"Stopped accepting the connection.");
            }
            else
            {
                string s_ip = $"{tb_ip_1.Text}.{tb_ip_2.Text}.{tb_ip_3.Text}.{tb_ip_4.Text}";
                if (IPAddress.TryParse(s_ip, out IPAddress ip) && ushort.TryParse(tb_port.Text, out ushort port))
                {
                    if (port.PortInLimits(50000, 65535))
                    {
                        RuntimeLogger.WriteLine($"Started accepting the connection...");
                        bt_connect.Text = "Lecsatlakozás";
                        bt_listen.Enabled = false;
                        connected = true;
                        bool connection_accepted = true;
                        while (connected)
                        {
                            if (connection_accepted)
                            {
                                await SimpleConnection.Connect(ip, port);
                                RuntimeLogger.WriteLine($"Connection started.");
                                byte[] receive_bytes = new byte[1024];
                                byte[] send_bytes = new byte[1];
                                int sent = 0;
                                int received = 0;
                                bool valid_response;

                                RuntimeLogger.WriteLine("Sending Connection request...");
                                valid_response = await SimpleConnection.MessageCommunication(Message.CONNECT_REQUEST, Message.OK, true);
                                RuntimeLogger.WriteLine(SimpleConnection.FormatCommunication(valid_response, Message.CONNECT_REQUEST, Message.OK, true));

                                int n = IPStorer.GetStoredAmount();

                                if (valid_response)
                                {
                                    sent = await SimpleConnection.SendBytes(BitConverter.GetBytes(n));
                                    RuntimeLogger.WriteLine($"Expected to send 4 bytes, sent {sent} byte(s).");
                                    if (sent == 4)
                                    {
                                        valid_response = await SimpleConnection.MessageCommunication(Message.OK, false);
                                        RuntimeLogger.WriteLine($"Validity: {valid_response}.");
                                    }
                                    else
                                    {
                                        valid_response = false;
                                    }
                                }

                                if (valid_response)
                                {
                                    valid_response = await SimpleConnection.SendFile(IPStorer.Fpath);
                                    RuntimeLogger.WriteLine($"Expected to send 4 bytes, sent {sent} byte(s).");
                                    if (valid_response)
                                    {
                                        received = await SimpleConnection.ReceiveBytes(receive_bytes);
                                        if (received == 1)
                                        {
                                            switch (receive_bytes[0])
                                            {
                                                case (byte)Message.ANSWER_YES:
                                                    connection_accepted = true;
                                                    valid_response = await SimpleConnection.MessageCommunication(Message.OK, true);
                                                    break;
                                                case (byte)Message.ANSWER_NO:
                                                    connection_accepted = false;
                                                    valid_response = await SimpleConnection.MessageCommunication(Message.OK, true);
                                                    break;
                                                default: valid_response = false; break;
                                            }
                                        }
                                        else
                                        {
                                            valid_response = false;
                                        }
                                    }
                                }

                                if (connection_accepted)
                                {
                                    panel.Enabled = true;
                                }

                                while (valid_response && connection_accepted)
                                {
                                    RuntimeLogger.WriteLine("Waiting for other client to respond...");
                                    received = await SimpleConnection.ReceiveBytes(receive_bytes);
                                    RuntimeLogger.WriteLine($"Expected to receive 1 byte, received {received} byte(s).");
                                    if (received == 1)
                                    {
                                        switch (receive_bytes[0])
                                        {
                                            case (byte)Message.FILE_TRANSFER_REQUEST:
                                                valid_response = await SimpleConnection.MessageCommunication(Message.OK, true);
                                                if (valid_response)
                                                {
                                                    /*await FileTransferOperation();*/
                                                }
                                                break;
                                            case (byte)Message.DISCONNECT_REQUEST:
                                                RuntimeLogger.WriteLine($"Other Client requested to disconnect.");
                                                MessageBox.Show("A másik fél lecsatlakozott!");
                                                connection_accepted = false;
                                                valid_response = await SimpleConnection.MessageCommunication(Message.OK, true);
                                                break;
                                            default:
                                                RuntimeLogger.WriteLine($"Invalid answer.");
                                                valid_response = false;
                                                break;
                                        }
                                    }
                                    else
                                    {
                                        valid_response = false;
                                    }
                                }

                                if (!valid_response)
                                {
                                    while (await SimpleConnection.MessageCommunication(Message.INVALID, Message.OK, true)) { /*nothing*/ }
                                }

                                panel.Enabled = false;
                                SimpleConnection.Close();
                            } // await sc.ConnectAsync(ip, port)
                            else
                            {
                                bt_connect.Text = "Csatlakozás";
                                bt_listen.Enabled = true;
                                connected = false;
                                SimpleConnection.Close();
                            }
                        } // connected
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

        private async void bt_send_Click(object sender, EventArgs e)
        {
            if (await SimpleConnection.MessageCommunication(Message.FILE_TRANSFER_REQUEST, Message.OK, true))
            {
                var indices = lb_files.SelectedIndices;
                List<byte> send_bytes = new List<byte>();

                send_bytes.AddRange(BitConverter.GetBytes(indices.Count));
                foreach (int item in indices)
                {
                    send_bytes.AddRange(Encoding.Default.GetBytes(lb_files.Items[item].ToString()));
                    send_bytes.Add(0);
                }

                int received = await SimpleConnection.SendBytes(send_bytes.ToArray());
                bool valid_response = await SimpleConnection.MessageCommunication(Message.OK, false);

                for (int i = 0; i < indices.Count; i++)
                {
                    if (!valid_response) break;
                    valid_response = await SimpleConnection.SendFile(file_paths[indices[i]]);
                }

                if (!valid_response)
                {
                    MessageBox.Show("Hiba történt a fájlküldés folyamán!");
                }

            }
            else
            {
                MessageBox.Show("Sikertelen fájlküldési kezdeményezés!");
            }
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

        private void cms_item_ip_Click(object sender, EventArgs e)
        {
            IPChangeChecker.ForceCheck();
        }

        private void cms_item_settings_Click(object sender, EventArgs e)
        {
            using (SettingsForm f = new SettingsForm())
            {
                f.ShowDialog(); 
            }
        }

        private void cms_Opening(object sender, CancelEventArgs e)
        {
            if (lb_files.ClientRectangle.Contains(lb_files.PointToClient(MousePosition)))
            {
                e.Cancel = true;
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            RuntimeLogger.WriteLine("Program terminating.");
            RuntimeLogger.Write("\n" + new string('-', 50));
            RuntimeLogger.Close();
        }
    }
}
