using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SP2P
{
    enum Message : byte
    {
        OK = 1,
        INVALID = 2,
        CONNECT_REQUEST = 11,
        DISCONNECT_REQUEST = 12,
        FILE_TRANSFER_REQUEST = 13,
        ANSWER_YES = 21,
        ANSWER_NO = 22
    };

    class SimpleConnection
    {
        public Socket ServerSocket { get; private set; } = null;
        public Socket ClientSocket { get; private set; } = null;
        public bool IsServer { get; private set; } = false;
        public bool IsConnected
        {
            get
            {
                if (ClientSocket != null)
                {
                    return ClientSocket.Connected;
                }
                return false;
            }
        }

        public bool Silent { get; set; } = true;

        public SimpleConnection(bool server, bool silent = true, bool throw_anyway = false)
        {
            Silent = silent;
            try
            {
                if (server)
                {
                    ServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    ServerSocket.Bind(new IPEndPoint(IPAddress.Any, 55585));
                    ServerSocket.Listen(2);
                    IsServer = true;
                }
                else
                {
                    ClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                }
            }
            catch (SocketException e)
            {
                ServerSocket = null;
                ClientSocket = null;
                IsServer = false;
                if (throw_anyway)
                {
                    throw;
                }
                if (!Silent)
                {
                    MessageBox.Show($"{e.ErrorCode}: {e.Message}");
                }
            }
        }

        #region Non-async

        public bool Accept(int timeout_ms = 1000)
        {
            try
            {
                if (ServerSocket != null)
                {
                    ClientSocket = ServerSocket.Accept();
                    ClientSocket.ReceiveTimeout = timeout_ms;
                    ClientSocket.SendTimeout = timeout_ms;
                    if (!Silent)
                    {
                        MessageBox.Show("Sikeres fogadás.");
                    }
                    return true;
                }
                else
                {
                    if (!Silent)
                    {
                        MessageBox.Show("Csak szerver használhatja az Accept függvényt.");
                    }
                    return false;
                }
            }
            catch (SocketException e)
            {
                if (!Silent)
                {
                    MessageBox.Show($"{e.ErrorCode}: {e.Message}");
                }
                return false;
            }
            catch (ObjectDisposedException) { return false; /*nothing*/ }
        }

        public bool Connect(IPAddress ip, int port = 55585, int timeout_ms = 1000)
        {
            try
            {
                if (ClientSocket != null)
                {
                    ClientSocket.Connect(ip, port);
                    ClientSocket.ReceiveTimeout = timeout_ms;
                    ClientSocket.SendTimeout = timeout_ms;
                    if (!Silent)
                    {
                        MessageBox.Show("Sikeres csatlakozás.");
                    }
                    return true;
                }
                else
                {
                    if (!Silent)
                    {
                        MessageBox.Show("Csak kliens használhatja a Connect függvényt.");
                    }
                    return false;
                }
            }
            catch (SocketException e)
            {
                if (!Silent)
                {
                    MessageBox.Show($"{e.ErrorCode}: {e.Message}");
                }
                return false;
            }
            catch (ObjectDisposedException) { return false; /*nothing*/ }
        }

        public int SendBytes(byte[] bytes)
        {
            try
            {
                int ret = ClientSocket.Send(bytes);
                if (!Silent)
                {
                    MessageBox.Show("Sikeres fájlküldés.");
                }
                return ret;
            }
            catch (SocketException e)
            {
                if (!Silent)
                {
                    MessageBox.Show($"{e.ErrorCode}: {e.Message}");
                }
                return -1 * e.ErrorCode;
            }
            catch (ObjectDisposedException) { return -1; /*nothing*/ }
        }

        public int ReceiveBytes(byte[] bytes)
        {
            try
            {
                int ret = ClientSocket.Receive(bytes);
                if (!Silent)
                {
                    MessageBox.Show("Sikeres adatfogadás.");
                }
                return ret;
            }
            catch (SocketException e)
            {
                if (!Silent)
                {
                    MessageBox.Show($"{e.ErrorCode}: {e.Message}");
                }
                return -1 * e.ErrorCode;
            }
            catch (ObjectDisposedException) { return -1; /*nothing*/ }
        }

        public bool SendFile(string path)
        {
            try
            {
                ClientSocket.SendFile(path);
                if (!Silent)
                {
                    MessageBox.Show("Sikeres fájlküldés.");
                }
                return true;
            }
            catch (SocketException e)
            {
                if (!Silent)
                {
                    MessageBox.Show($"{e.ErrorCode}: {e.Message}");
                }
                return false;
            }
            catch (ObjectDisposedException) { return false; /*nothing*/ }
        }
        #endregion

        #region Async

        public async Task<bool> AcceptAsync(int timeout_ms = 1000)
        {
            try
            {
                if (ServerSocket != null)
                {
                    ClientSocket = await ServerSocket.AcceptAsyncTAP();
                    ClientSocket.ReceiveTimeout = timeout_ms;
                    ClientSocket.SendTimeout = timeout_ms;
                    if (!Silent)
                    {
                        MessageBox.Show("Sikeres fogadás.");
                    }
                    return true;
                }
                else
                {
                    if (!Silent)
                    {
                        MessageBox.Show("Csak szerver használhatja az Accept függvényt.");
                    }
                    return false;
                }
            }
            catch (SocketException e)
            {
                if (!Silent)
                {
                    MessageBox.Show($"{e.ErrorCode}: {e.Message}");
                }
                return false;
            }
            catch (ObjectDisposedException) { return false; /*nothing*/ }
        }

        public async Task<bool> ConnectAsync(IPAddress ip, int port = 55585, int timeout_ms = 1000)
        {
            try
            {
                if (ClientSocket != null)
                {
                    await ClientSocket.ConnectAsyncTAP(ip, port);
                    ClientSocket.ReceiveTimeout = timeout_ms;
                    ClientSocket.SendTimeout = timeout_ms;
                    if (!Silent)
                    {
                        MessageBox.Show("Sikeres csatlakozás.");
                    }
                    return true;
                }
                else
                {
                    if (!Silent)
                    {
                        MessageBox.Show("Csak kliens használhatja a Connect függvényt.");
                    }
                    return false;
                }
            }
            catch (SocketException e)
            {
                if (!Silent)
                {
                    MessageBox.Show($"{e.ErrorCode}: {e.Message}");
                }
                return false;
            }
            catch (ObjectDisposedException) { return false; /*nothing*/ }
        }

        public async Task<int> SendBytesAsync(byte[] bytes)
        {
            try
            {
                int ret = await ClientSocket.SendAsyncTAP(bytes);
                if (!Silent)
                {
                    MessageBox.Show("Sikeres adatküldés.");
                }
                return ret;
            }
            catch (SocketException e)
            {
                if (!Silent)
                {
                    MessageBox.Show($"{e.ErrorCode}: {e.Message}");
                }
                return -1 * e.ErrorCode;
            }
            catch (ObjectDisposedException) { return -1; /*nothing*/ }
        }

        public async Task<int> ReceiveBytesAsync(byte[] bytes)
        {
            try
            {
                int ret = await ClientSocket.ReceiveAsyncTAP(bytes);
                if (!Silent)
                {
                    MessageBox.Show("Sikeres adatfogadás.");
                }
                return ret;
            }
            catch (SocketException e)
            {
                if (!Silent)
                {
                    MessageBox.Show($"{e.ErrorCode}: {e.Message}");
                }
                return -1 * e.ErrorCode;
            }
            catch (ObjectDisposedException) { return -1; /*nothing*/ }
        }

        public async Task<bool> SendFileAsync(string path)
        {
            try
            {
                await ClientSocket.SendFileAsyncTAP(path);
                if (!Silent)
                {
                    MessageBox.Show("Sikeres fájlküldés.");
                }
                return true;
            }
            catch (SocketException e)
            {
                if (!Silent)
                {
                    MessageBox.Show($"{e.ErrorCode}: {e.Message}");
                }
                return false;
            }
            catch (ObjectDisposedException) { return false; /*nothing*/ }
        }
        #endregion

        public void Close()
        {
            try
            {
                if (ClientSocket != null)
                {
                    ClientSocket.Shutdown(SocketShutdown.Both);
                    ClientSocket.Close();
                }
                if (ServerSocket != null)
                {
                    ServerSocket.Close();
                }
                if (!Silent)
                {
                    MessageBox.Show("Sikeres szétkapcsolás.");
                }
            }
            catch (SocketException e)
            {
                if (!Silent)
                {
                    MessageBox.Show($"{e.ErrorCode}: {e.Message}");
                }
            }
            catch (ObjectDisposedException) { /*nothing*/ }
        }
    }
    static class SimpleConnectionExtensions
    {
        private static async Task<bool> SendMessageAsync(this SimpleConnection sc, Message message)
        {
            int sent = await sc.SendBytesAsync(new byte[] { (byte)message });
            return sent == 1;
        }

        private static async Task<bool> ReceiveMessageAsync(this SimpleConnection sc, Message message)
        {
            byte[] message_byte = new byte[2];
            int arrived = await sc.ReceiveBytesAsync(message_byte);
            return arrived == 1 && message_byte[0] == (byte)message;
        }

        public static async Task<bool> MessageCommunicationAsync(this SimpleConnection sc, Message send_msg, Message receive_msg, bool send_first)
        {
            bool valid_response;
            if (send_first)
            {
                valid_response = await sc.SendMessageAsync(send_msg);
                if (valid_response)
                {
                    valid_response = await sc.ReceiveMessageAsync(receive_msg);
                }
            }
            else
            {
                valid_response = await sc.ReceiveMessageAsync(receive_msg);
                if (valid_response)
                {
                    valid_response = await sc.SendMessageAsync(send_msg);
                }
            }
            return valid_response;
        }

        public static async Task<bool> MessageCommunicationAsync(this SimpleConnection sc, Message msg, bool send)
        {
            bool valid_response;
            if (send)
            {
                valid_response = await sc.SendMessageAsync(msg);
            }
            else
            {
                valid_response = await sc.ReceiveMessageAsync(msg);
            }
            return valid_response;
        }

        public static async Task<bool> ValidateCommunicationAsync(this SimpleConnection sc, bool send_first)
        {
            return await sc.MessageCommunicationAsync(Message.OK, Message.OK, send_first);
        }
    }
}