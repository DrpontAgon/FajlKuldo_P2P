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

        public SimpleConnection(bool server, bool throw_anyway = false, bool silent = true)
        {
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
                if (!silent)
                {
                    MessageBox.Show($"{e.ErrorCode}: {e.Message}");
                }
            }
        }

        #region Non-async

        public bool Accept(int timeout_ms = 1000, bool silent = true)
        {
            try
            {
                if (ServerSocket != null)
                {
                    ClientSocket = ServerSocket.Accept();
                    ClientSocket.ReceiveTimeout = timeout_ms;
                    ClientSocket.SendTimeout = timeout_ms;
                    if (!silent)
                    {
                        MessageBox.Show("Sikeres fogadás.");
                    }
                    return true;
                }
                else
                {
                    if (!silent)
                    {
                        MessageBox.Show("Csak szerver használhatja az Accept függvényt.");
                    }
                    return false;
                }
            }
            catch (SocketException e)
            {
                if (!silent)
                {
                    MessageBox.Show($"{e.ErrorCode}: {e.Message}");
                }
                return false;
            }
            catch (ObjectDisposedException) { return false; /*nothing*/ }
        }

        public bool Connect(IPAddress ip, int port = 55585, int timeout_ms = 1000, bool silent = true)
        {
            try
            {
                if (ClientSocket != null)
                {
                    ClientSocket.Connect(ip, port);
                    ClientSocket.ReceiveTimeout = timeout_ms;
                    ClientSocket.SendTimeout = timeout_ms;
                    if (!silent)
                    {
                        MessageBox.Show("Sikeres csatlakozás.");
                    }
                    return true;
                }
                else
                {
                    if (!silent)
                    {
                        MessageBox.Show("Csak kliens használhatja a Connect függvényt.");
                    }
                    return false;
                }
            }
            catch (SocketException e)
            {
                if (!silent)
                {
                    MessageBox.Show($"{e.ErrorCode}: {e.Message}");
                }
                return false;
            }
            catch (ObjectDisposedException) { return false; /*nothing*/ }
        }

        public int SendBytes(byte[] bytes, bool silent = true)
        {
            try
            {
                int ret = ClientSocket.Send(bytes);
                if (!silent)
                {
                    MessageBox.Show("Sikeres fájlküldés.");
                }
                return ret;
            }
            catch (SocketException e)
            {
                if (!silent)
                {
                    MessageBox.Show($"{e.ErrorCode}: {e.Message}");
                }
                return -1 * e.ErrorCode;
            }
            catch (ObjectDisposedException) { return -1; /*nothing*/ }
        }

        public int ReceiveBytes(byte[] bytes, bool silent = true)
        {
            try
            {
                int ret = ClientSocket.Receive(bytes);
                if (!silent)
                {
                    MessageBox.Show("Sikeres adatfogadás.");
                }
                return ret;
            }
            catch (SocketException e)
            {
                if (!silent)
                {
                    MessageBox.Show($"{e.ErrorCode}: {e.Message}");
                }
                return -1 * e.ErrorCode;
            }
            catch (ObjectDisposedException) { return -1; /*nothing*/ }
        }

        public bool SendFile(string path, bool silent = true)
        {
            try
            {
                ClientSocket.SendFile(path);
                if (!silent)
                {
                    MessageBox.Show("Sikeres fájlküldés.");
                }
                return true;
            }
            catch (SocketException e)
            {
                if (!silent)
                {
                    MessageBox.Show($"{e.ErrorCode}: {e.Message}");
                }
                return false;
            }
            catch (ObjectDisposedException) { return false; /*nothing*/ }
        }
        #endregion

        #region Async

        public async Task<bool> AcceptAsync(int timeout_ms = 1000, bool silent = true)
        {
            try
            {
                if (ServerSocket != null)
                {
                    ClientSocket = await ServerSocket.AcceptAsyncTAP();
                    ClientSocket.ReceiveTimeout = timeout_ms;
                    ClientSocket.SendTimeout = timeout_ms;
                    if (!silent)
                    {
                        MessageBox.Show("Sikeres fogadás.");
                    }
                    return true;
                }
                else
                {
                    if (!silent)
                    {
                        MessageBox.Show("Csak szerver használhatja az Accept függvényt.");
                    }
                    return false;
                }
            }
            catch (SocketException e)
            {
                if (!silent)
                {
                    MessageBox.Show($"{e.ErrorCode}: {e.Message}");
                }
                return false;
            }
            catch (ObjectDisposedException) { return false; /*nothing*/ }
        }

        public async Task<bool> ConnectAsync(IPAddress ip, int port = 55585, int timeout_ms = 1000, bool silent = true)
        {
            try
            {
                if (ClientSocket != null)
                {
                    await ClientSocket.ConnectAsyncTAP(ip, port);
                    ClientSocket.ReceiveTimeout = timeout_ms;
                    ClientSocket.SendTimeout = timeout_ms;
                    if (!silent)
                    {
                        MessageBox.Show("Sikeres csatlakozás.");
                    }
                    return true;
                }
                else
                {
                    if (!silent)
                    {
                        MessageBox.Show("Csak kliens használhatja a Connect függvényt.");
                    }
                    return false;
                }
            }
            catch (SocketException e)
            {
                if (!silent)
                {
                    MessageBox.Show($"{e.ErrorCode}: {e.Message}");
                }
                return false;
            }
            catch (ObjectDisposedException) { return false; /*nothing*/ }
        }

        public async Task<int> SendBytesAsync(byte[] bytes, bool silent = true)
        {
            try
            {
                int ret = await ClientSocket.SendAsyncTAP(bytes);
                if (!silent)
                {
                    MessageBox.Show("Sikeres adatküldés.");
                }
                return ret;
            }
            catch (SocketException e)
            {
                if (!silent)
                {
                    MessageBox.Show($"{e.ErrorCode}: {e.Message}");
                }
                return -1 * e.ErrorCode;
            }
            catch (ObjectDisposedException) { return -1; /*nothing*/ }
        }

        public async Task<int> ReceiveBytesAsync(byte[] bytes, bool silent = true)
        {
            try
            {
                int ret = await ClientSocket.ReceiveAsyncTAP(bytes);
                if (!silent)
                {
                    MessageBox.Show("Sikeres adatfogadás.");
                }
                return ret;
            }
            catch (SocketException e)
            {
                if (!silent)
                {
                    MessageBox.Show($"{e.ErrorCode}: {e.Message}");
                }
                return -1 * e.ErrorCode;
            }
            catch (ObjectDisposedException) { return -1; /*nothing*/ }
        }

        public async Task<bool> SendFileAsync(string path, bool silent = true)
        {
            try
            {
                await ClientSocket.SendFileAsyncTAP(path);
                if (!silent)
                {
                    MessageBox.Show("Sikeres fájlküldés.");
                }
                return true;
            }
            catch (SocketException e)
            {
                if (!silent)
                {
                    MessageBox.Show($"{e.ErrorCode}: {e.Message}");
                }
                return false;
            }
            catch (ObjectDisposedException) { return false; /*nothing*/ }
        }
        #endregion

        public void Close(bool silent = true)
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
                if (!silent)
                {
                    MessageBox.Show("Sikeres szétkapcsolás.");
                }
            }
            catch (SocketException e)
            {
                if (!silent)
                {
                    MessageBox.Show($"{e.ErrorCode}: {e.Message}");
                }
            }
            catch (ObjectDisposedException) { /*nothing*/ }
        }
    }
}