using System.Net;
using System.IO;

namespace SP2P
{
    static class IPLogger
    {
        private static string fpath = "IPLog.ipd";

        public static void ResetLogToThisIP(this IPAddress ip)
        {
            using (FileStream f = new FileStream(fpath, FileMode.Create))
            {
                f.Write(ip.GetAddressBytes(), 0, 4);
            }
        }

        public static void CheckAndLogIP(this IPAddress ip)
        {
            if (File.Exists(fpath))
            {
                if (CheckIp(ip)) LogIp(ip);
            }
            else ResetLogToThisIP(ip);
        }

        private static bool CheckIp(IPAddress ip)
        {
            FileStream f = new FileStream(fpath, FileMode.Open);
            byte[] arr = new byte[4];
            f.Read(arr, 0, 4);
            f.Close();
            IPAddress last_ip = new IPAddress(arr);
            return !(ip.Equals(last_ip));
        }

        private static void LogIp(IPAddress ip)
        {
            byte[] arr = File.ReadAllBytes(fpath);
            using (FileStream f = new FileStream(fpath, FileMode.Create))
            {
                f.Write(ip.GetAddressBytes(), 0, 4);
                f.Write(arr, 0, arr.Length);
            }
        }
    }
}
