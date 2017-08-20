using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace SP2P
{
    public static class IPNameSolver
    {
        /// <summary>
        /// Statikus konstruktor, program indításakor fut le és beállítja az alapértelmezett
        /// helyét (és nevét) a fájlnak. Az alapértelmezett hely azt a mappát jelenti
        /// ahol a futtatható állomány is megtalálható.
        /// </summary>
        static IPNameSolver()
        {
            Fpath = "IPNames.ipn";
        }

        public static string Fpath { get; set; }

        private static bool GetIPOrNamesIfExist(this IPAddress ip, out string name, out int index)
        {
            name = string.Empty;
            index = 0;

            IPAddress line_ip;
            string line_name;

            foreach (var item in File.ReadLines(Fpath, Encoding.Default))
            {
                line_ip = new IPAddress(Encoding.Default.GetBytes(item.Substring(0, 4)));
                line_name = item.Substring(4);
                if (ip.Equals(line_ip))
                {
                    name = line_name;
                    break;
                }
                index++;
            }
            return name != string.Empty;
        }

        private static bool GetIPOrNamesIfExist(this string name, out IPAddress[] ips, out int[] indices)
        {
            List<IPAddress> l_ips = new List<IPAddress>();
            List<int> l_indices = new List<int>();
            int index = 0;

            IPAddress line_ip;
            string line_name;

            foreach (var item in File.ReadLines(Fpath, Encoding.Default))
            {
                line_ip = new IPAddress(Encoding.Default.GetBytes(item.Substring(0, 4)));
                line_name = item.Substring(4);
                if (name == line_name)
                {
                    l_ips.Add(line_ip);
                    l_indices.Add(index);
                }
                index++;
            }
            ips = l_ips.ToArray();
            indices = l_indices.ToArray();
            return name != string.Empty;
        }

        public static bool GetIPOrNamesIfExist(this IPAddress ip, out string name)
        {
            return GetIPOrNamesIfExist(ip, out name, out int unused);
        }

        public static bool GetIPOrNamesIfExist(this string name, out IPAddress[] ips)
        {
            return GetIPOrNamesIfExist(name, out ips, out int[] unused);
        }

        private static string IPAndNameLine(IPAddress ip, string name)
        {
            byte[] b_arr = ip.GetAddressBytes();
            char[] c_arr = new char[4];
            for (int i = 0; i < 4; i++)
            {
                c_arr[i] = (char)b_arr[i];
            }
            return new string(c_arr) + name;
        }

        public static void SetIPOrName(this IPAddress ip, string name)
        {
            if (GetIPOrNamesIfExist(ip, out string unused, out int index))
            {
                string[] lines = File.ReadAllLines(Fpath, Encoding.Default);
                lines[index] = IPAndNameLine(ip, name);
                File.WriteAllLines(Fpath, lines, Encoding.Default);
            }
            else
            {
                using (StreamWriter f = new StreamWriter(Fpath, true, Encoding.Default))
                    f.WriteLine(IPAndNameLine(ip, name));
            }
        }

        //public static void SetIPOrName(this string name, IPAddress ip)
        //{
        //    if (GetIPOrNamesIfExist(name, out IPAddress unused, out int index))
        //    {
        //        string[] lines = File.ReadAllLines(Fpath, Encoding.Default);
        //        lines[index] = IPAndNameLine(ip, name);
        //        File.WriteAllLines(Fpath, lines, Encoding.Default);
        //    }
        //    else
        //    {
        //        using (StreamWriter f = new StreamWriter(Fpath, true, Encoding.Default))
        //            f.WriteLine(IPAndNameLine(ip, name));
        //    }
        //}

        public static void RemoveIPOrName(this IPAddress ip)
        {
            if (GetIPOrNamesIfExist(ip, out string unused, out int index))
            {
                string[] lines = File.ReadAllLines(Fpath, Encoding.Default);
                lines = lines.Except(new string[] { lines[index] }).ToArray();
                File.WriteAllLines(Fpath, lines, Encoding.Default);
            }
        }

        public static void RemoveIPOrName(this string name)
        {
            if (GetIPOrNamesIfExist(name, out IPAddress[] unused))
            {
                List<string> lines = File.ReadAllLines(Fpath, Encoding.Default).ToList();
                lines.RemoveAll((s) => s.Substring(4) == name);
                File.WriteAllLines(Fpath, lines, Encoding.Default);
            }
        }

        public static Dictionary<IPAddress, string> GetAllIPNames()
        {
            Dictionary<IPAddress, string> dictionary = new Dictionary<IPAddress, string>();
            IPAddress line_ip;
            string line_name;

            foreach (var item in File.ReadLines(Fpath, Encoding.Default))
            {
                line_ip = new IPAddress(Encoding.Default.GetBytes(item.Substring(0, 4)));
                line_name = item.Substring(4);
                dictionary.Add(line_ip, line_name);
            }
            return dictionary;
        }

        public static SortedDictionary<IPAddress, string> GetAllIPNamesSorted()
        {
            //IComparer<IPAddress> comparer = new IPAddressComparer();
            Comparer<IPAddress> comparer = new IPAddressComparer();
            return new SortedDictionary<IPAddress, string>(GetAllIPNames(), comparer);
        }
    }

    public static class IPAddressExtender
    {
        public static uint ToUInt32(this IPAddress ip)
        {
            byte[] x = ip.GetAddressBytes();
            x = x.Reverse().ToArray();
            return BitConverter.ToUInt32(x, 0);
        }
    }

    //public class IPAddressComparer : IComparer<IPAddress>
    //{
    //    int IComparer<IPAddress>.Compare(IPAddress ip1, IPAddress ip2)
    //    {
    //        return ip1.ToUInt32().CompareTo(ip2.ToUInt32());
    //    }
    //}

    public class IPAddressComparer : Comparer<IPAddress>
    {
        public override int Compare(IPAddress ip1, IPAddress ip2)
        {
            return ip1.ToUInt32().CompareTo(ip2.ToUInt32());
        }
    }
}