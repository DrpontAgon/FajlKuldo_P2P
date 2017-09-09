// Készítette és a kódot fenttartja: Tóth Ákos
// Potenciális ötletadók és módosítók: Bense Viktor, Csaholczi Atilla

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
//using System.Text;
using System.Threading.Tasks;

namespace SP2P
{
    public static class GenericExtensions
    {
        public delegate bool TryParse<T>(string str, out T value);

        public static object GenericTryParse<T>(this string str, ref bool success, TryParse<T> parseFunc)
        {
            success = parseFunc(str, out T val);
            return val;
        }

        public static T ParseIfPossible<T>(this T input_t, string str) where T : IConvertible
        {
            T success_t = default(T);
            bool success = false;
            switch (input_t.GetTypeCode())
            {
                case TypeCode.Boolean: success_t = (T)str.GenericTryParse<bool>(ref success, bool.TryParse); break;
                case TypeCode.Byte: success_t = (T)str.GenericTryParse<byte>(ref success, byte.TryParse); break;
                case TypeCode.SByte: success_t = (T)str.GenericTryParse<sbyte>(ref success, sbyte.TryParse); break;
                case TypeCode.UInt16: success_t = (T)str.GenericTryParse<ushort>(ref success, ushort.TryParse); break;
                case TypeCode.Int16: success_t = (T)str.GenericTryParse<short>(ref success, short.TryParse); break;
                case TypeCode.UInt32: success_t = (T)str.GenericTryParse<uint>(ref success, uint.TryParse); break;
                case TypeCode.Int32: success_t = (T)str.GenericTryParse<int>(ref success, int.TryParse); break;
                case TypeCode.UInt64: success_t = (T)str.GenericTryParse<ulong>(ref success, ulong.TryParse); break;
                case TypeCode.Int64: success_t = (T)str.GenericTryParse<long>(ref success, long.TryParse); break;
                case TypeCode.Single: success_t = (T)str.GenericTryParse<float>(ref success, float.TryParse); break;
                case TypeCode.Double: success_t = (T)str.GenericTryParse<double>(ref success, double.TryParse); break;
                case TypeCode.Decimal: success_t = (T)str.GenericTryParse<decimal>(ref success, decimal.TryParse); break;
                case TypeCode.Char: success_t = (T)str.GenericTryParse<char>(ref success, char.TryParse); break;
                case TypeCode.DateTime: success_t = (T)str.GenericTryParse<DateTime>(ref success, DateTime.TryParse); break;
                default: break;
            }
            return success ? success_t : input_t;
        }
    }

    public static class UInt16Extensions
    {
        public static bool PortInLimits(this ushort input, ushort min, ushort max)
        {
            return (input >= min && input <= max);
        }

        public static ushort ChangePortIfInLimits(this ushort input, ushort min, ushort max, ushort previous)
        {
            return PortInLimits(input, min, max) ? input : previous;
        }
    }

    /// <summary>
    /// 
    /// Egyéb IPAddress-t kiegészítő osztály, célja tárolni az összes többi szükséges függvényt
    /// vagy metódust, ami alapértelmezetten nem szerepel az IPAddress osztályban.
    /// 
    /// Általános használata:
    /// 
    /// IPAddress x = bármilyen IP cím;
    /// 
    /// x.KiegeszitoMetodus();
    /// var0 = x.KiegeszitoMetodus(param1, param2, ..., paramn);
    /// 
    /// </summary>
    public static class IPAddressExtensions
    {
        /// <summary>
        /// 
        /// A függvény feladata, hogy visszadja a korrekt pozitív egész számot, ami reprezentálja az IP címet.
        /// Memóriakezelési és átváltási okokból a szám típusok a memóriában bájtonként
        /// fordítva vannak tárolva, és ezt a beépített típusfordítók nem veszik figyelembe,
        /// ezzel azt elérve, hogy az IP címet reprezentáló szám nincs helyes sorrendben.
        /// 
        /// Példa:
        /// 
        /// helyes sorrend:    Beépített fordítás:    Ez a függvény:
        ///     0.0.0.1             16777216                1
        ///     0.0.0.2             33554432                2
        ///     0.0.1.0              65536                 256
        ///     1.0.0.0                1                 16777216
        /// 
        /// </summary>
        /// <param name="ip"> A megadott IP cím </param>
        /// <returns> Az IP címet reprezentáló </returns>
        public static uint ToUInt32(this IPAddress ip)
        {
            byte[] x = ip.GetAddressBytes();
            x = x.Reverse().ToArray();
            return BitConverter.ToUInt32(x, 0);
        }
    }

    /// <summary>
    /// 
    /// IPAddress-t összehasonlító osztály, az általános összehasonlító osztályból származtatva,
    /// egyetlen kötelezően implementált függvénnyel. Célja az olyan függvények kielégítése,
    /// ahol szükséges egy összehasonlító és az összehasonlítandó adatok, amik IPAddress típusúak.
    /// 
    /// </summary>
    public class IPAddressComparer : Comparer<IPAddress>
    {
        /// <summary>
        /// 
        /// A kötelezően implementált függvény, célja összehasonlítási eredményt
        /// adni az első IP cím kapcsolatáról a másodikhoz képest.
        /// Az összehasonlításhoz azokat a pozitív egész számokat hasonlítjuk össze 
        /// a számokhoz tartozó beépített összehasonlítóval, amik helyesen
        /// reprezentálják az IP címet.
        /// 
        /// </summary>
        /// <param name="ip1"> Az első megadott IP cím </param>
        /// <param name="ip2"> A második megadott IP cím </param>
        /// <returns>
        /// 
        /// Visszaadja az IP címek kapcsolatát:
        /// -1 ha az első cím megelőzi a másodikat  (kisebb)
        ///  0 ha a címek azonosak                  (egyenlő)
        ///  1 ha az első cím követi a másodikat    (nagyobb)
        ///  
        /// </returns>
        public override int Compare(IPAddress ip1, IPAddress ip2)
        {
            return ip1.ToUInt32().CompareTo(ip2.ToUInt32());
        }
    }

    static class SocketExtensions
    {
        public static IAsyncResult BeginSend(this Socket socket, byte[] buffer, AsyncCallback callback, object state)
        {
            return socket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, callback, state);
        }
        public static IAsyncResult BeginReceive(this Socket socket, byte[] buffer, AsyncCallback callback, object state)
        {
            return socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, callback, state);
        }

        public static Task<Socket> AcceptAsyncTAP(this Socket socket)
        {
            return Task<Socket>.Factory.FromAsync(socket.BeginAccept, socket.EndAccept, null);
        }

        public static Task ConnectAsyncTAP(this Socket socket, EndPoint endpoint)
        {
            return Task.Factory.FromAsync(socket.BeginConnect, socket.EndConnect, endpoint, null);
        }

        public static Task ConnectAsyncTAP(this Socket socket, IPAddress address, int port)
        {
            return socket.ConnectAsyncTAP(new IPEndPoint(address, port));
        }

        //public static Task DisconnectAsyncTAP(this Socket socket, bool reuseSocket)
        //{
        //    return Task.Factory.FromAsync(socket.BeginDisconnect, socket.EndDisconnect, reuseSocket, null);
        //}

        public static Task<int> SendAsyncTAP(this Socket socket, byte[] buffer)
        {
            //return Task<int>.Factory.FromAsync(socket.BeginSend, socket.EndSend, buffer, 0, buffer.Length, SocketFlags.None, null);

            //return Task<int>.Factory.FromAsync(
            //    (callback, state) => socket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, callback, state), socket.EndSend, null);

            return Task<int>.Factory.FromAsync(socket.BeginSend, socket.EndSend, buffer, null);
        }
        public static Task<int> ReceiveAsyncTAP(this Socket socket, byte[] buffer)
        {
            //return Task<int>.Factory.FromAsync(socket.BeginReceive, socket.EndReceive, buffer, 0, buffer.Length, SocketFlags.None, null);

            //return Task<int>.Factory.FromAsync(
            //     (callback, state) => socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, callback, state), socket.EndReceive, null);

            return Task<int>.Factory.FromAsync(socket.BeginReceive, socket.EndReceive, buffer, null);
        }
        public static Task SendFileAsyncTAP(this Socket socket, string fileName)
        {
            return Task.Factory.FromAsync(socket.BeginSendFile, socket.EndSendFile, fileName, null);
        }
    }
}
