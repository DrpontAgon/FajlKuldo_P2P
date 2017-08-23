﻿// Készítette és a kódot fenttartja: Tóth Ákos
// Potenciális ötletadók és módosítók: Bense Viktor, Csaholczi Atilla

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
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
                case TypeCode.Int32: success_t = (T)str.GenericTryParse<int>(ref success, int.TryParse); break;
                case TypeCode.UInt16: success_t = (T)str.GenericTryParse<ushort>(ref success, ushort.TryParse); break;
                default: break;
            }
            return success ? success_t : input_t;
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
    public static class IPAddressExtender
    {
        /// <summary>
        /// 
        /// A függvény feladata hogy visszadja a korrekt pozitív egész számot ami, reprezentálja az IP címet.
        /// Memóriakezelési és átváltási okokból a szám típúsok a memóriában bájtonként
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
    /// ahol szükséges egy összehasonlító, és az összehasonlítandó adatok, amik IPAddress típusúak.
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
}
