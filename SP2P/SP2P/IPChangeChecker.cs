// Készítette és a kódot fenttartja: Tóth Ákos
// Potenciális ötletadók és módosítók: Bense Viktor, Csaholczi Atilla

using System;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace SP2P
{
    /// <summary>
    /// 
    /// IPChangeChecker célja, hogy érzékelje az IP cím változásait, és ez eventként kezelődik,
    /// azaz köthető hozzá kód, ami csak akkor fut le, ha éppen változott az IP (és nem kell
    /// feleslegesen megadott időintervallumban ellenőrizni azt). Ez az osztály visszaadja a
    /// Privát (LAN) és publikus (WAN) IP címet is.
    /// 
    /// Általános használata:
    /// 
    /// IPChangeChecker.IPChanged += EventMetodus, ahol
    /// private void Eventmetodus(object sender, IPChangedEventArgs e) { ... }
    /// 
    /// vagy 
    /// 
    /// IPChangeChecker.IPChanged += (object sender, IPChangedEventArgs e) => { ... }
    /// 
    /// és végül kell:
    /// 
    /// IPChangeChecker.ForceCheck();
    /// 
    /// </summary>
    class IPChangeChecker
    {
        public static event IPChangedEventHandler IPChanged;

        protected static void RaiseNetChanged(IPChangedEventArgs e)
        {
            IPChanged?.Invoke(typeof(IPChangeChecker), e);
        }

        /// <summary>
        /// 
        /// Statikus konstruktor, Beállítja a NetworkChange két eventjére
        /// ugyanazt a metódust.
        /// 
        /// </summary>
        static IPChangeChecker()
        {
            NetworkChange.NetworkAddressChanged += AddressChanged;
            NetworkChange.NetworkAvailabilityChanged += AddressChanged;
        }

        public static IPAddress PublicIP { get; private set; }

        public static IPAddress PrivateIP { get; private set; }

        /// <summary>
        /// 
        /// Loopback = 127.0.0.1 (= localhost)
        /// 
        /// </summary>
        public static bool PrivateIpIsLoopback
        {
            get { return IPAddress.IsLoopback(PrivateIP); }
        }

        /// <summary>
        /// 
        /// None = 255.255.255.255
        /// 
        /// </summary>
        public static bool PublicIpIsNone
        {
            get { return PublicIP.Equals(IPAddress.None); }
        }

        /// <summary>
        /// 
        /// Kényszerített ellenőrzés, meghívja a NetworkChange eventjeihez
        /// kapcsolt metódust paraméterek nélkül.
        /// 
        /// </summary>
        public static void ForceCheck()
        {
            AddressChanged(null, null);
        }

        /// <summary>
        /// 
        /// Függvény neve magáért beszél, a két foreach ciklus a nyelv által kezelt lehetséges interfészeken,
        /// illetve a hardvereken megy keresztül. Benne az első "if" ellenőrzi, hogy az a típusú interfész
        /// működik-e, azaz tud adatokat küldeni. Ha igen, akkor a második "if" ellenőrzi, hogy
        /// ahhoz tartozik-e egy gateway cím. Ha igen, akkor az ahhoz tartozó összes IP közül ki kell
        /// választani az IPv4-eset.
        /// 
        /// </summary>
        /// <returns> LAN IP cím, sikertelen lekéréskor a Loopback / localhost cím </returns>
        private static IPAddress GetLocalIPv4()
        {
            foreach (NetworkInterfaceType nit_item in Enum.GetValues(typeof(NetworkInterfaceType)))
            {
                foreach (NetworkInterface ni_item in NetworkInterface.GetAllNetworkInterfaces())
                {
                    if (ni_item.NetworkInterfaceType == nit_item && ni_item.OperationalStatus == OperationalStatus.Up)
                    {
                        IPInterfaceProperties adapterProperties = ni_item.GetIPProperties();
                        if (adapterProperties.GatewayAddresses.FirstOrDefault() != null)
                        {
                            foreach (UnicastIPAddressInformation ip in adapterProperties.UnicastAddresses)
                            {
                                if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                                {
                                    return ip.Address;
                                }
                            }
                        }
                    }
                }
            }
            return IPAddress.Loopback;
        }

        /// <summary>
        /// 
        /// Függvény neve magáért beszél, időtúllépes képességgel módosított WebClient-tel
        /// letöltődik az oldal tartalma ami az IP cím, viszont tartalmaz egy sortörést,
        /// a végén ezért azt el kell távolítani, és végül át kell alakítani IPAddress-re.
        /// Exception-nel elszállna, ha nincs internet vagy a megadott 5 másodperc alatt
        /// nem töltődik le az adat (ami gyakorlatilag azt jelenti hogy nincs internet).
        /// 
        /// </summary>
        /// <returns> WAN IP cím, sikertelen lekéréskor a None cím </returns>
        private static IPAddress GetPublicIPv4()
        {
            try
            {
                string s = new WebDownload(5000).DownloadString("http://icanhazip.com");
                return IPAddress.Parse(s.Remove(s.Length - 1));
            }
            catch
            {
                return IPAddress.None;
            }
        }

        /// <summary>
        /// 
        /// NetworkChange eventjeihez kötött metódus, célja megszerezni az IP címeket
        /// és lőni az IPChanged eventet.
        /// 
        /// </summary>
        /// <param name="sender"> NetworkChange eventjeihez tartozó nemhasznált paraméter </param>
        /// <param name="e"> NetworkChange eventjeihez tartozó nemhasznált paraméter </param>
        private static void AddressChanged(object sender, EventArgs e)
        {
            PublicIP = GetPublicIPv4();
            PrivateIP = GetLocalIPv4();
            RaiseNetChanged(new IPChangedEventArgs(PublicIP, PrivateIP, PublicIpIsNone, PrivateIpIsLoopback));
        }
    }

    delegate void IPChangedEventHandler(object sender, IPChangedEventArgs e);

    /// <summary>
    /// 
    /// IPChanged event argumentumai (metódusban általában e változó), az
    /// IP címek és lehetséges állapotaik így metódouson belül lokálisan elérhető.
    /// Nem biztos, hogy szükséges ez, mert statikusan is elérhetőek ezek az adatok a
    /// fő osztályból. Esetleg változás ellenőrzésre lehet használni.
    /// 
    /// </summary>
    class IPChangedEventArgs : EventArgs
    {
        public IPAddress PublicIP { get; }
        public IPAddress PrivateIP { get; }
        public bool PublicIpIsNone { get; }
        public bool PrivateIpIsLoopback { get; }


        public IPChangedEventArgs(IPAddress pub_ip, IPAddress priv_ip, bool pub_ip_n, bool priv_ip_l)
        {
            PublicIP = pub_ip;
            PrivateIP = priv_ip;
            PublicIpIsNone = pub_ip_n;
            PrivateIpIsLoopback = priv_ip_l;
        }
    }

    /// <summary>
    /// 
    /// Letöltött osztály, lényege, hogy úgy módosítja a WebClient osztályt, hogy
    /// egyszerűen használható az időtúllépés funkciója.
    /// 
    /// </summary>
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