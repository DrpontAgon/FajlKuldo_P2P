//
// Authors:
//   Alan McGovern alan.mcgovern@gmail.com
//   Ben Motmans <ben.motmans@gmail.com>
//   Lucas Ontivero lucasontivero@gmail.com
//
// Copyright (C) 2006 Alan McGovern
// Copyright (C) 2007 Ben Motmans
// Copyright (C) 2014 Lucas Ontivero
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//__________________________________________________________________
//
// Köszönet a fent említett készítőknek az Open.Nat könyvtár létezéséért.

using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using Open.Nat;

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

        public static async Task OpenPort(NatDevice device)
        {
            int port = Settings.Port;
            await device.CreatePortMapAsync(new Mapping(Protocol.Tcp, IPChangeChecker.PrivateIP, port, port, 3600000, "SP2P"));
            MessageBox.Show("A port megnyitva!", "Portnyitás");
        }

        public static async Task ClosePort(NatDevice device)
        {
            int port = Settings.Port;
            await device.CreatePortMapAsync(new Mapping(Protocol.Tcp, IPChangeChecker.PrivateIP, port, port, 3600000, "SP2P"));
            MessageBox.Show("A port bezárva!", "Portzárás");
        }
    }
}
