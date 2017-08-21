using System.Net;
using System.IO;

namespace SP2P
{
    /// <summary>
    /// 
    /// Az IPLogger célja, hogy elmentse az IP címet akkor, ha az megváltozott.
    /// Az elmentett IP a fájl elejére kerül így a fájlban mindig a legfrissebbtől
    /// a legrégebbiig van tárolva az IP. Helysprórolásért az elmentett IP binárisan
    /// van elmentve, azaz kódolástól függően a külső szemlélőnek egy cím 2 vagy 4
    /// karakternek felel meg.
    /// 
    /// Általános használata:
    /// 
    /// IPAddress x = bármilyen IP cím;
    /// x.CheckAndLogIP();
    /// x.ResetLogToThisIP();
    /// 
    /// </summary>
    static class IPLogger
    {
        /// <summary>
        /// 
        /// Statikus konstruktor, program indításakor fut le és beállítja az alapértelmezett
        /// helyét (és nevét) a log fájlnak. Az alapértelmezett hely azt a mappát jelenti
        /// ahol a futtatható állomány is megtalálható.
        /// 
        /// </summary>
        static IPLogger()
        {
            Fpath = "IPLog.ipd";
        }

        public static string Fpath { get; set; }

        /// <summary>
        /// 
        /// Függvény neve magáért beszél, elkészíti a fájlt és
        /// az első bejegyzés a megadott ip cím lesz.
        /// 
        /// </summary>
        /// <param name="ip"> Megadott IP cím </param>
        public static void ResetLogToThisIP(this IPAddress ip)
        {
            using (FileStream f = new FileStream(Fpath, FileMode.Create))
            {
                f.Write(ip.GetAddressBytes(), 0, 4);
            }
        }

        /// <summary>
        /// 
        /// Függvény neve magáért beszél, ha létezik a fájl akkor végrahajtja az
        /// ellenőrzést, és ezen ellenőrzéstől függ, hogy elmenti-e a megadott ip-t.
        /// Amennyiben a fájl nem létezik, akkor elkészül a ResetLogToThisIP függvényen keresztül.
        /// 
        /// </summary>
        /// <param name="ip"> Megadott IP cím </param>
        public static void CheckAndLogIP(this IPAddress ip)
        {
            if (File.Exists(Fpath))
            {
                if (CheckIp(ip)) LogIp(ip);
            }
            else ResetLogToThisIP(ip);
        }

        /// <summary>
        /// 
        /// Beolvassa az első 4 karaktert, ami a legutoljára elmentett ip cím és
        /// összehasonlítja a megadott ip címmel, és ez alapján vissza ad egy értéket.
        /// 
        /// </summary>
        /// <param name="ip"> Megadott IP cím </param>
        /// <returns> Logikai érték, igaz, ha nem egyezik az ip, azaz el kell menteni </returns>
        private static bool CheckIp(IPAddress ip)
        {
            FileStream f = new FileStream(Fpath, FileMode.Open);
            byte[] arr = new byte[4];
            f.Read(arr, 0, 4);
            f.Close();
            IPAddress last_ip = new IPAddress(arr);
            return !(ip.Equals(last_ip));
        }

        /// <summary>
        /// 
        /// Függvény neve magáért beszél, beolvassa a teljes fájl tartalmát,
        /// újra elkészíti a fájlt (így üres), és a megadott ip-t a fájl elejére írja,
        /// az elmentett előző adatot pedig utána írja.
        /// 
        /// </summary>
        /// <param name="ip"> Megadott IP cím </param>
        private static void LogIp(IPAddress ip)
        {
            byte[] arr = File.ReadAllBytes(Fpath);
            using (FileStream f = new FileStream(Fpath, FileMode.Create))
            {
                f.Write(ip.GetAddressBytes(), 0, 4);
                f.Write(arr, 0, arr.Length);
            }
        }
    }
}
