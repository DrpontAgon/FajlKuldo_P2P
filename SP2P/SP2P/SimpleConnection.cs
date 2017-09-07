using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SP2P
{
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

        //public SimpleConnection()
        //{
        //    try
        //    {
        //        ServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        //        ServerSocket.Bind(new IPEndPoint(IPAddress.Any, 55585));
        //        ServerSocket.Listen(2);
        //        ClientSocket = ServerSocket.Accept();
        //        IsServer = true;
        //    }
        //    catch (SocketException e)
        //    {
        //        ServerSocket = null;
        //        ClientSocket = null;
        //        IsServer = false;
        //        MessageBox.Show(e.Message);
        //    }
        //}

        //public SimpleConnection(IPAddress ip)
        //{
        //    try
        //    {
        //        ClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        //        ClientSocket.Connect(ip, 55585);
        //    }
        //    catch (SocketException e)
        //    {
        //        ClientSocket = null;
        //        MessageBox.Show(e.Message);
        //    }
        //}

        public SimpleConnection(bool server)
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
                MessageBox.Show($"{e.ErrorCode}: {e.Message}");
            }
        }

        public void Accept()
        {
            try
            {
                if (ServerSocket != null)
                {
                    ClientSocket = ServerSocket.Accept();
                    MessageBox.Show("Sikeres fogadás.");
                }
                else
                {
                    MessageBox.Show("Csak szerver használhatja az Accept függvényt.");
                }
            }
            catch (SocketException e)
            {
                MessageBox.Show($"{e.ErrorCode}: {e.Message}");
            }
        }

        public async Task AcceptAsync()
        {
            try
            {
                if (ServerSocket != null)
                {
                    ClientSocket = await ServerSocket.AcceptAsyncTAP();
                    MessageBox.Show("Sikeres fogadás.");
                }
                else
                {
                    MessageBox.Show("Csak szerver használhatja az Accept függvényt.");
                }
            }
            catch (SocketException e)
            {
                MessageBox.Show($"{e.ErrorCode}: {e.Message}");
            }
        }

        public void Connect(IPAddress ip)
        {
            try
            {
                if (ClientSocket != null)
                {
                    ClientSocket.Connect(ip, 55585);
                    MessageBox.Show("Sikeres csatlakozás.");
                }
                else
                {
                    MessageBox.Show("Csak kliens használhatja a Connect függvényt.");
                }
            }
            catch (SocketException e)
            {
                MessageBox.Show($"{e.ErrorCode}: {e.Message}");
            }
        }

        public async Task ConnectAsync(IPAddress ip)
        {
            try
            {
                if (ClientSocket != null)
                {
                    await ClientSocket.ConnectAsyncTAP(ip, 55585);
                    MessageBox.Show("Sikeres csatlakozás.");
                }
                else
                {
                    MessageBox.Show("Csak kliens használhatja a Connect függvényt.");
                }
            }
            catch (SocketException e)
            {
                MessageBox.Show($"{e.ErrorCode}: {e.Message}");
            }
        }

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
                MessageBox.Show("Sikeres szétkapcsolás.");
            }
            catch (SocketException e)
            {
                MessageBox.Show($"{e.ErrorCode}: {e.Message}");
            }
        }
        //public void Disconnect(bool reuse)
        //{
        //    try
        //    {
        //        if (ClientSocket != null)
        //        {
        //            //ClientSocket.Disconnect(reuse);
        //            ClientSocket.Close();
        //        }
        //        if (ServerSocket != null)
        //        {
        //            //ServerSocket.Shutdown(SocketShutdown.Both);
        //            //ServerSocket.Disconnect(reuse);
        //            ServerSocket.Close();
        //        }
        //    }
        //    catch (SocketException e)
        //    {
        //        MessageBox.Show($"{e.ErrorCode}: {e.Message}");
        //    }
        //}

        //public async Task DisconnectAsync(bool reuse)
        //{
        //    try
        //    {
        //        if (ClientSocket != null)
        //        {
        //            await ClientSocket.DisconnectAsyncTAP(reuse);
        //        }
        //        if (ServerSocket != null)
        //        {
        //            await ServerSocket.DisconnectAsyncTAP(reuse);
        //        }
        //    }
        //    catch (SocketException e)
        //    {
        //        MessageBox.Show($"{e.ErrorCode}: {e.Message}");
        //    }
        //}

        public int SendBytes(byte[] bytes)
        {
            try
            {
                int ret = ClientSocket.Send(bytes);
                MessageBox.Show("Sikeres adatküldés.");
                return ret;
            }
            catch (SocketException e)
            {
                MessageBox.Show($"{e.ErrorCode}: {e.Message}");
                return 0;
            }
        }

        public async Task<int> SendBytesAsync(byte[] bytes)
        {
            try
            {
                int ret = await ClientSocket.SendAsyncTAP(bytes);
                MessageBox.Show("Sikeres adatküldés.");
                return ret;
            }
            catch (SocketException e)
            {
                MessageBox.Show($"{e.ErrorCode}: {e.Message}");
                return 0;
            }
        }

        public int ReceiveBytes(byte[] bytes)
        {
            try
            {
                int ret = ClientSocket.Receive(bytes);
                MessageBox.Show("Sikeres adatfogadás.");
                return ret;
            }
            catch (SocketException e)
            {
                MessageBox.Show($"{e.ErrorCode}: {e.Message}");
                return 0;
            }
        }

        public async Task<int> ReceiveBytesAsync(byte[] bytes)
        {
            try
            {
                int ret = await ClientSocket.ReceiveAsyncTAP(bytes);
                MessageBox.Show("Sikeres adatfogadás.");
                return ret;
            }
            catch (SocketException e)
            {
                MessageBox.Show($"{e.ErrorCode}: {e.Message}");
                return 0;
            }
        }

        public void SendFile(string path)
        {
            try
            {
                ClientSocket.SendFile(path);
                MessageBox.Show("Sikeres fájlküldés.");
            }
            catch (SocketException e)
            {
                MessageBox.Show($"{e.ErrorCode}: {e.Message}");
            }
        }

        public async Task SendFileAsync(string path)
        {
            try
            {
                await ClientSocket.SendFileAsyncTAP(path);
                MessageBox.Show("Sikeres fájlküldés.");
            }
            catch (SocketException e)
            {
                MessageBox.Show($"{e.ErrorCode}: {e.Message}");
            }
        }
    }
}