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

    static class SimpleConnection
    {
        #region variables
        public static Socket ServerSocket { get; private set; } = null;
        public static Socket ClientSocket { get; private set; } = null;
        public static bool IsServer { get; private set; } = false;
        public static bool IsConnected
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
        public static bool IsSilent { get; set; } = true;
        public static bool IsClosed { get; private set; } = true;
        #endregion

        #region basic
        public static async Task<bool> Accept(int port = 55585, int timeout_ms = 1000)
        {
            try
            {
                if (!IsClosed)
                {
                    Close();
                }
                ServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                ServerSocket.Bind(new IPEndPoint(IPAddress.Any, port));
                ServerSocket.Listen(2);
                IsServer = true;
                ClientSocket = await ServerSocket.AcceptAsyncTAP();
                ClientSocket.ReceiveTimeout = timeout_ms;
                ClientSocket.SendTimeout = timeout_ms;
                IsClosed = false;
                if (IsConnected)
                {
                    if (!IsSilent)
                    {
                        MessageBox.Show("Sikeres fogadás.");
                    }
                }
                else
                {
                    if (!IsSilent)
                    {
                        MessageBox.Show("Sikertelen fogadás.");
                    }
                    Close();
                }
                return IsConnected;
            }
            catch (SocketException e)
            {
                if (!IsSilent)
                {
                    MessageBox.Show($"{e.ErrorCode}: {e.Message}");
                }
                return false;
            }
            catch (ObjectDisposedException) { return false; /*nothing*/ }
        }
        public static async Task<bool> Connect(IPAddress ip, int port = 55585, int timeout_ms = 1000)
        {
            try
            {
                if (!IsClosed)
                {
                    Close();
                }
                ClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                await ClientSocket.ConnectAsyncTAP(ip, port);
                ClientSocket.ReceiveTimeout = timeout_ms;
                ClientSocket.SendTimeout = timeout_ms;
                IsClosed = false;
                if (IsConnected)
                {
                    if (!IsSilent)
                    {
                        MessageBox.Show("Sikeres csatlakozás.");
                    }
                }
                else
                {
                    if (!IsSilent)
                    {
                        MessageBox.Show("Sikertelen csatlakozás.");
                    }
                    Close();
                }
                return IsConnected;
            }
            catch (SocketException e)
            {
                if (!IsSilent)
                {
                    MessageBox.Show($"{e.ErrorCode}: {e.Message}");
                }
                return false;
            }
            catch (ObjectDisposedException) { return false; /*nothing*/ }
        }
        public static async Task<int> SendBytes(byte[] bytes)
        {
            try
            {
                if (!IsConnected)
                {
                    return -60;
                }
                int ret = await ClientSocket.SendAsyncTAP(bytes);
                if (!IsSilent)
                {
                    MessageBox.Show("Sikeres adatküldés.");
                }
                return ret;
            }
            catch (SocketException e)
            {
                if (!IsSilent)
                {
                    MessageBox.Show($"{e.ErrorCode}: {e.Message}");
                }
                return -1 * e.ErrorCode;
            }
            catch (ObjectDisposedException) { return -1; /*nothing*/ }
        }
        public static async Task<int> ReceiveBytes(byte[] bytes)
        {
            try
            {
                if (!IsConnected)
                {
                    return -60;
                }
                int ret = await ClientSocket.ReceiveAsyncTAP(bytes);
                if (!IsSilent)
                {
                    MessageBox.Show("Sikeres adatfogadás.");
                }
                return ret;
            }
            catch (SocketException e)
            {
                if (!IsSilent)
                {
                    MessageBox.Show($"{e.ErrorCode}: {e.Message}");
                }
                return -1 * e.ErrorCode;
            }
            catch (ObjectDisposedException) { return -1; /*nothing*/ }
        }
        public static async Task<bool> SendFile(string path)
        {
            try
            {
                await ClientSocket.SendFileAsyncTAP(path);
                if (!IsSilent)
                {
                    MessageBox.Show("Sikeres fájlküldés.");
                }
                return true;
            }
            catch (SocketException e)
            {
                if (!IsSilent)
                {
                    MessageBox.Show($"{e.ErrorCode}: {e.Message}");
                }
                return false;
            }
            catch (ObjectDisposedException) { return false; /*nothing*/ }
        }
        public static void Close()
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
                IsServer = false;
                IsClosed = true;
                if (!IsSilent)
                {
                    MessageBox.Show("Sikeres szétkapcsolás.");
                }
            }
            catch (SocketException e)
            {
                if (!IsSilent)
                {
                    MessageBox.Show($"{e.ErrorCode}: {e.Message}");
                }
            }
            catch (ObjectDisposedException) { /*nothing*/ }
        }
        #endregion

        #region advanced
        private static async Task<bool> SendMessage(Message message)
        {
            int sent = await SendBytes(new byte[] { (byte)message });
            return sent == 1;
        }
        private static async Task<bool> ReceiveMessage(Message message)
        {
            byte[] message_byte = new byte[2];
            int arrived = await ReceiveBytes(message_byte);
            return arrived == 1 && message_byte[0] == (byte)message;
        }
        public static async Task<bool> MessageCommunication(Message send_msg, Message receive_msg, bool send_first)
        {
            bool valid_response;
            if (send_first)
            {
                valid_response = await SendMessage(send_msg);
                if (valid_response)
                {
                    valid_response = await ReceiveMessage(receive_msg);
                }
            }
            else
            {
                valid_response = await ReceiveMessage(receive_msg);
                if (valid_response)
                {
                    valid_response = await SendMessage(send_msg);
                }
            }
            return valid_response;
        }
        public static async Task<bool> MessageCommunication(Message msg, bool send)
        {
            bool valid_response;
            if (send)
            {
                valid_response = await SendMessage(msg);
            }
            else
            {
                valid_response = await ReceiveMessage(msg);
            }
            return valid_response;
        }
        public static async Task<bool> ValidateCommunication(bool send_first)
        {
            return await MessageCommunication(Message.OK, Message.OK, send_first);
        }
        public static string FormatCommunication(bool valid_response, Message send_msg, Message receive_msg, bool send_first)
        {
            string validity_str = $"Validity: {valid_response}";
            string send_str = "Send: ";
            switch (send_msg)
            {
                case Message.OK: send_str += "OK"; break;
                case Message.INVALID: send_str += "INVALID"; break;
                case Message.CONNECT_REQUEST: send_str += "CONNECT_REQUEST"; break;
                case Message.DISCONNECT_REQUEST: send_str += "DISCONNECT_REQUEST"; break;
                case Message.FILE_TRANSFER_REQUEST: send_str += "FILE_TRANSFER_REQUEST"; break;
                case Message.ANSWER_YES: send_str += "ANSWER_YES"; break;
                case Message.ANSWER_NO: send_str += "ANSWER_NO"; break;
            }
            string get_str = "Get: ";
            switch (receive_msg)
            {
                case Message.OK: get_str += "OK"; break;
                case Message.INVALID: get_str += "INVALID"; break;
                case Message.CONNECT_REQUEST: get_str += "CONNECT_REQUEST"; break;
                case Message.DISCONNECT_REQUEST: get_str += "DISCONNECT_REQUEST"; break;
                case Message.FILE_TRANSFER_REQUEST: get_str += "FILE_TRANSFER_REQUEST"; break;
                case Message.ANSWER_YES: get_str += "ANSWER_YES"; break;
                case Message.ANSWER_NO: get_str += "ANSWER_NO"; break;
            }
            if (send_first) return $"{validity_str} {send_str} {get_str}.";
            else return $"{validity_str} {get_str} {send_str}.";
        }
        public static string FormatCommunication(bool valid_response, Message msg, bool send)
        {
            string validity_str = $"Validity: {valid_response}";
            string msg_str = send ? "Send: " : "Get: ";
            switch (msg)
            {
                case Message.OK: msg_str += "OK"; break;
                case Message.INVALID: msg_str += "INVALID"; break;
                case Message.CONNECT_REQUEST: msg_str += "CONNECT_REQUEST"; break;
                case Message.DISCONNECT_REQUEST: msg_str += "DISCONNECT_REQUEST"; break;
                case Message.FILE_TRANSFER_REQUEST: msg_str += "FILE_TRANSFER_REQUEST"; break;
                case Message.ANSWER_YES: msg_str += "ANSWER_YES"; break;
                case Message.ANSWER_NO: msg_str += "ANSWER_NO"; break;
            }
            return $"{validity_str} {msg_str}.";
        }
        #endregion
    }
}