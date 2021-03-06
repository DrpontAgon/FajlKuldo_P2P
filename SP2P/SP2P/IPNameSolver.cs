﻿// Készítette és a kódot fenttartja: Tóth Ákos
// Potenciális ötletadók és módosítók: Bense Viktor, Csaholczi Atilla

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace SP2P
{
    /// <summary>
    /// 
    /// Az IPNameSolver célja, hogy elmentsen a bejövő IP címekhez a felhasználó által
    /// megadott neveket, és hogy ezekből névfeloldást csináljon a következő alkalommal
    /// az adott IP esetén. Egy IP cím csak egy nevet tárolhat, azonban ugyan ahhoz a névhez
    /// több IP cím is tartozhat. Tárolása helytakarékosság céljából a számítógépen
    /// alapértelmezett ANSI kódolást használ, ami fix 1 bájt hosszú. Ezenfelül az IP binárisan van elmentve,
    /// ezért külső szemlélőnek egy cím 4 karakternek felel meg.
    /// 
    /// Általános használata:
    /// 
    /// IPAddress x = bármilyen IP cím;
    /// string y = bármilyen név;
    /// var z;
    /// var zs;
    /// 
    /// x.GetIPOrNamesIfExist(out string y_out);
    /// y.GetIPOrNamesIfExist(out IPAddress x_out);
    /// 
    /// x.SetIPOrNames(out string y_out);
    /// 
    /// x.RemoveIPOrName();
    /// y.RemoveIPOrName();
    /// 
    /// z = GetAllIPAndNames();
    /// zs = GetAllIPAndNamesSorted();
    /// 
    /// </summary>
    public static class IPNameSolver
    {
        /// <summary>
        /// 
        /// Statikus konstruktor, program indításakor fut le és beállítja az alapértelmezett
        /// helyét (és nevét) a fájlnak. Az alapértelmezett hely azt a mappát jelenti,
        /// ahol a futtatható állomány is megtalálható.
        /// 
        /// </summary>
        static IPNameSolver()
        {
            string dir_path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "SP2P");
            if (!Directory.Exists(dir_path)) Directory.CreateDirectory(dir_path);
            Fpath = Path.Combine(dir_path, "IPNames.ipn");
        }

        public static string Fpath { get; set; }

        /// <summary>
        /// 
        /// A függvény neve magáért beszél, az a változat, ahol a bemenet egy IP cím és a kimenet a
        /// hozzá tartozó név, ha létezik, és ennek a sorszáma a fájlon belül. A sikerességről is kapunk
        /// egy értéket.
        /// 
        /// Mivel csak belső műveleteknél szükséges a sorszám, és mivel minden függvény ami
        /// a fájllal dolgozik valamilyen formában, itt kell, hogy megvalósítva legyen,
        /// ezért a program egyéb részeihez nem szükséges a sorszám, így ez a verziója függvénynek privát.
        /// 
        /// </summary>
        /// <param name="ip"> A megadott IP cím </param>
        /// <param name="name"> A hozzá tartozó név </param>
        /// <param name="index"> A hozzá tartozó sorszám </param>
        /// <returns> Logikai érték igaz, ha szerepel az IP cím a fájlban </returns>
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

        /// <summary>
        /// 
        /// A függvény neve magáért beszél, az a változat ahol a bemenet egy név és a kimenet a
        /// hozzá tartozó IP címek, ha létezik, és ennek a sorszámai a fájlon belül. A sikerességről is kapunk
        /// egy értéket.
        /// 
        /// Mivel csak belső műveleteknél szükséges a sorszám, és mivel minden függvény ami
        /// a fájllal dolgozik valamilyen formában, itt kell, hogy megvalósítva legyen,
        /// ezért a program egyéb részeihez nem szükséges a sorszám, így ez a verziója függvénynek privát.
        /// 
        /// </summary>
        /// <param name="name"> A megadott név </param>
        /// <param name="ips"> A hozzá tartozó IP címek </param>
        /// <param name="indices"> A hozzá tartozó sorszámok </param>
        /// <returns> Logikai érték igaz, ha szerepel a név a fájlban </returns>
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
            return ips.Length != 0;
        }

        /// <summary>
        /// 
        /// A két paraméteres változat, különbség a három paraméteres megfelelőjéhez képest,
        /// hogy nem kapjuk vissza a sorszámokat és ezért ez publikus is.
        /// 
        /// </summary>
        /// <param name="ip"> A megadott IP cím </param>
        /// <param name="name"> A hozzá tartozó név </param>
        /// <returns> Logikai érték, igaz, ha szerepel az IP cím a fájlban </returns>
        public static bool GetIPOrNamesIfExist(this IPAddress ip, out string name)
        {
            return GetIPOrNamesIfExist(ip, out name, out int unused);
        }

        /// <summary>
        /// 
        /// A két paraméteres változat, különbség a három paraméteres megfelelőjéhez képest,
        /// hogy nem kapjuk vissza a sorszámokat és ezért ez publikus is.
        /// 
        /// </summary>
        /// <param name="name"> A megadott név </param>
        /// <param name="ips"> A hozzá tartozó IP címek </param>
        /// <returns> Logikai érték, igaz, ha szerepel a név a fájlban </returns>
        public static bool GetIPOrNamesIfExist(this string name, out IPAddress[] ips)
        {
            return GetIPOrNamesIfExist(name, out ips, out int[] unused);
        }

        /// <summary>
        /// 
        /// Segéd függvény, feladata létrehozni a fájlban szereplő sor formátumát
        /// a megadott IP címből és névből.
        /// 
        /// </summary>
        /// <param name="ip"> A megadott IP cím </param>
        /// <param name="name"> A megadott név </param>
        /// <returns> A fájl formátumának megfelelő egy sor </returns>
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

        /// <summary>
        /// 
        /// A függvény feladata hogy a megadott IP címhez hozzárendelje a megadott nevet.
        /// Ha az IP cím már létezik, akkor az felülíródik; ha nem, akkor hozzáadódik a fájlhoz
        /// az új bejegyzés.
        /// 
        /// Ebből csak ez az egy változat van, ha szükségessé válik, akkor implementálva lesz
        /// a másik változat is.
        /// 
        /// </summary>
        /// <param name="ip"> A megadott IP cím </param>
        /// <param name="name"> A megadott név </param>
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

        /// <summary>
        /// 
        /// A függvény neve magáért beszél, eltávolítja az IP címet és a hozzá
        /// tartozó nevet a fájlból.
        /// 
        /// </summary>
        /// <param name="ip"> A megadott IP cím </param>
        public static void RemoveIPOrName(this IPAddress ip)
        {
            if (GetIPOrNamesIfExist(ip, out string unused, out int index))
            {
                string[] lines = File.ReadAllLines(Fpath, Encoding.Default);
                lines = lines.Except(new string[] { lines[index] }).ToArray();
                File.WriteAllLines(Fpath, lines, Encoding.Default);
            }
        }

        /// <summary>
        /// 
        /// A függvény neve magáért beszél, eltávolítja a nevet és a hozzá
        /// tartozó összes IP címet a fájlból.
        /// 
        /// </summary>
        /// <param name="name"> A megadott név </param>
        public static void RemoveIPOrName(this string name)
        {
            if (GetIPOrNamesIfExist(name, out IPAddress[] unused))
            {
                List<string> lines = File.ReadAllLines(Fpath, Encoding.Default).ToList();
                lines.RemoveAll((s) => s.Substring(4) == name);
                File.WriteAllLines(Fpath, lines, Encoding.Default);
            }
        }

        /// <summary>
        /// 
        /// A függvény neve magáért beszél, visszaadja az összes fájlban szereplő bejegyzést
        /// kulcs-érték párokban, abban a sorrendben ahogy az a fájlban szerepel.
        /// 
        /// </summary>
        /// <returns> Az összes bejegyzés kulcs-érték párokban </returns>
        public static Dictionary<IPAddress, string> GetAllIPAndNames()
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

        /// <summary>
        /// 
        /// A függvény neve magáért beszél, visszaadja az összes fájlban szereplő bejegyzést
        /// kulcs-érték párokban, IP cím szerint növekvő sorrendben.
        /// 
        /// </summary>
        /// <returns> Az összes bejegyzés kulcs-érték párokban, sorrendben </returns>
        public static SortedDictionary<IPAddress, string> GetAllIPAndNamesSorted()
        {
            return new SortedDictionary<IPAddress, string>(GetAllIPAndNames(), new IPAddressComparer());
        }
    }
}