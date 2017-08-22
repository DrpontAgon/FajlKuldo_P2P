﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Open.Nat;
using System.Threading;
using System.Windows.Forms;

namespace SP2P
{
    class PortOpener
    {
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
