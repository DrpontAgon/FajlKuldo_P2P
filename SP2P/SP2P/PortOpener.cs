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
using System.Collections.Generic;
using System.Linq;
using Open.Nat;

namespace SP2P
{
    class PortOpener
    {
        static PortOpener()
        {
            NatDiscoverer discoverer = new NatDiscoverer();
            CancellationTokenSource cts = new CancellationTokenSource(20000);
            GetDevice(discoverer, cts);
        }

        private static async void GetDevice(NatDiscoverer discoverer, CancellationTokenSource cts)
        {
            Device = await discoverer.DiscoverDeviceAsync(PortMapper.Upnp, cts);
        }

        public static NatDevice Device { get; private set; } = null;

        public static async Task<bool> OpenPort(int? nullable_port = null, bool silent = true)
        {
            try
            {
                int port = nullable_port.HasValue ? nullable_port.Value : Settings.Port;
                await Device.CreatePortMapAsync(new Mapping(Protocol.Tcp, IPChangeChecker.PrivateIP, port, port, 0, "SP2P"));
                if (!silent)
                {
                    MessageBox.Show("A port megnyitva!", "Portnyitás");
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static async Task<bool> ClosePort(int? nullable_port = null, bool silent = true)
        {
            try
            {
                int port = nullable_port.HasValue ? nullable_port.Value : Settings.Port;
                await Device.DeletePortMapAsync(new Mapping(Protocol.Tcp, IPChangeChecker.PrivateIP, port, port, 0, "SP2P"));
                if (!silent)
                {
                    MessageBox.Show("A port bezárva!", "Portzárás"); 
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static async Task<bool> ClosePort(Mapping mapping, bool silent = true)
        {
            try
            {
                await Device.DeletePortMapAsync(mapping);
                if (!silent)
                {
                    MessageBox.Show("A port bezárva!", "Portzárás");
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static async Task<bool> IsPortOpen(int? nullable_port = null, bool silent = true)
        {
            try
            {
                int port = nullable_port.HasValue ? nullable_port.Value : Settings.Port;
                foreach (var item in await Device.GetAllMappingsAsync())
                {
                    if (item.Description == "SP2P" && (item.PrivatePort == port || item.PublicPort == port))
                    {
                        if (!silent)
                        {
                            MessageBox.Show($"Meg van nyitva ez a port: {port}!");
                        }
                        return true;
                    }
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public static async Task<bool> CloseAllPortsExcept(params int[] ports)
        {
            try
            {
                bool ret = true;
                foreach (var item in await Device.GetAllMappingsAsync())
                {
                    if (item.Description == "SP2P")
                    {
                        bool is_exception = false;
                        for (int i = 0; i < ports.Length; i++)
                        {
                            if (item.PrivatePort == ports[i] || item.PublicPort == ports[i])
                            {
                                is_exception = true;
                                break;
                            }
                        }
                        if (!is_exception)
                        {
                            ret &= await ClosePort(item);
                        }
                    }
                }
                return ret;
            }
            catch
            {
                return false;
            }
        }
    }
}
