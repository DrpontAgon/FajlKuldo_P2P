using System;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace SP2P_old
{
    class IPChangeChecker
    {
        public static event IPChangeCheckerEventHandler NetChanged;

        protected static void RaiseNetChanged(IPChangeCheckerEventArgs e)
        {
            NetChanged?.Invoke(typeof(IPChangeChecker), e);
        }

        static IPChangeChecker()
        {
            NetworkChange.NetworkAddressChanged += AddressChanged;
            NetworkChange.NetworkAvailabilityChanged += AddressChanged;
        }

        public static IPAddress PublicIP { get; private set; }

        //public static IPAddress[] PrivateIPs { get; private set; }

        public static IPAddress PrivateIP { get; private set; }

        public static bool LoopbackIpPresent
        {
            get
            {
                //bool lp = false;
                //foreach (var item in PrivateIPs)
                //{
                //    lp |= IPAddress.IsLoopback(item);
                //}
                //return lp;
                //return true;

                return IPAddress.IsLoopback(PrivateIP);
            }
        }

        public static void ForceCheck()
        {
            AddressChanged(null, null);
        }

        private static string GetLocalIPv4(NetworkInterfaceType _type)
        {
            string output = "";
            foreach (NetworkInterface item in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (item.NetworkInterfaceType == _type && item.OperationalStatus == OperationalStatus.Up)
                {
                    IPInterfaceProperties adapterProperties = item.GetIPProperties();

                    if (adapterProperties.GatewayAddresses.FirstOrDefault() != null)
                    {
                        foreach (UnicastIPAddressInformation ip in adapterProperties.UnicastAddresses)
                        {
                            if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                            {
                                output = ip.Address.ToString();
                            }
                        }
                    }
                }
            }
            return output;
        }

        private static void AddressChanged(object sender, EventArgs e)
        {
            PublicIP = IPAddress.None;
            PrivateIP = IPAddress.Loopback;
            try
            {
                string s = new WebDownload(5000).DownloadString("http://icanhazip.com");
                s = s.Remove(s.Length - 1);
                PublicIP = IPAddress.Parse(s);
            }
            catch /*(Exception ex)*/
            {
                //throw ex;
            }

            //PrivateIPs = Dns.GetHostAddresses(Dns.GetHostName());
            //for (int i = 0; i < PrivateIPs.Length; i++)
            //{
            //    if (PrivateIPs[i].AddressFamily != AddressFamily.InterNetwork)
            //    {
            //        PrivateIPs[i] = IPAddress.None;
            //    }
            //}

            //var x = Dns.GetHostAddresses(Dns.GetHostName());
            //PrivateIPs = IPAddress.Loopback;
            //foreach (var item in x)
            //{
            //    string[] sv = item.ToString().Split('.');
            //    if (sv.Length == 4 && item.AddressFamily == AddressFamily.InterNetwork)
            //    {
            //        PrivateIPs = item;
            //    }
            //}

            foreach (NetworkInterfaceType item in Enum.GetValues(typeof(NetworkInterfaceType)))
            {
                string s = GetLocalIPv4(item);
                if (s != "")
                {
                    PrivateIP = IPAddress.Parse(s);
                    break;
                }
            }
            RaiseNetChanged(new IPChangeCheckerEventArgs(PublicIP, PrivateIP));
        }
    }

    delegate void IPChangeCheckerEventHandler(object sender, IPChangeCheckerEventArgs e);

    class IPChangeCheckerEventArgs : EventArgs
    {
        public IPAddress PublicIP { get; }        // nem kell private set?
        //public IPAddress[] PrivateIPs { get; }  // nem static miatt?
        public IPAddress PrivateIPs { get; }

        public IPChangeCheckerEventArgs(IPAddress pub_ip, IPAddress priv_ip)
        {
            PublicIP = pub_ip;
            PrivateIPs = priv_ip;
        }
    }

    public class WebDownload : WebClient
    {
        /// <summary>
        /// Time in milliseconds
        /// </summary>
        public int Timeout { get; set; }

        public WebDownload() : this(60000) { }

        public WebDownload(int timeout)
        {
            Timeout = timeout;
        }

        protected override WebRequest GetWebRequest(Uri address)
        {
            var request = base.GetWebRequest(address);
            if (request != null)
            {
                request.Timeout = Timeout;
            }
            return request;
        }
    }
}
