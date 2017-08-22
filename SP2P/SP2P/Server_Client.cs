using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.IO.Ports;
using System.Text;
using System.Threading.Tasks;

namespace SP2P
{
    public class Server
    {
        public TcpListener Tcp = null;
        public Server(IPAddress localIP, int port = 55585)
        {
            Tcp = new TcpListener(localIP, port);
        }
    }
    public class Client
    {
        private TcpClient client = null;
        public NetworkStream ns = null;
        public Client(TcpClient socket)
        {
            client = socket;
            ns = socket.GetStream();
        }
        public void Send(string filepath)
        {
            
        }
    }
}
