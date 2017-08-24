using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using Open.Nat;

namespace SP2P
{
    class PortOpener
    {
        /*[DllImport("OpenNat.dll",CallingConvention = CallingConvention.Winapi)]
        private static extern */
        public static async Task<NatDevice> GetConnectedDevice()
        {
            var discoverer = new NatDiscoverer();
            var cts = new CancellationTokenSource(20000);
            return await discoverer.DiscoverDeviceAsync(PortMapper.Upnp, cts);
        }
        public static async Task OpenPort(NatDevice device, int port = 55585)
        {
            await device.CreatePortMapAsync(new Mapping(Protocol.Tcp, IPChangeChecker.PrivateIP, port, port, 3600000, "SP2P"));
            MessageBox.Show("A port megnyitva!", "Portnyitás");
        }
        public static async Task ClosePort(NatDevice device, int port = 55585)
        {
            await device.CreatePortMapAsync(new Mapping(Protocol.Tcp, IPChangeChecker.PrivateIP, port, port, 3600000, "SP2P"));
            MessageBox.Show("A port bezárva!", "Portzárás");
        }
    }
}
