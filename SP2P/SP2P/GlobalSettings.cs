using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SP2P
{
    public static class Settings
    {
        static Settings()
        {
            RelativePath = Directory.GetCurrentDirectory() + "\\";
            ConfigPath = RelativePath + "config.ini";
            SetDefaultSettingValues();

            if (File.Exists(ConfigPath))
            {
                foreach (var item in File.ReadLines(ConfigPath, Encoding.Default))
                {
                    string[] sv = item.Split('=');
                    if (sv.Length == 2)
                    {
                        switch (sv[0])
                        {
                            case "Port": Port = Port.ParseIfPossible(sv[1]); break;
                            default: break;
                        }
                    }
                }
            }
            else
            {
                CreateConfigFile();
            }
        }

        public static string RelativePath { get; private set; }
        public static string ConfigPath { get; private set; }
        public static ushort Port { get; private set; }

        public static void SetDefaultSettingValues()
        {
            Port = 55585;
        }

        public static void AcceptSettings(ushort? port)
        {
            if (port != null) Port = port.Value;
            CreateConfigFile();
        }

        public static void CreateConfigFile()
        {
            List<string> lines = new List<string>();
            lines.Add("[Net]");
            lines.Add("");
            lines.Add($"Port={Port}");
            File.WriteAllLines(ConfigPath, lines);
        }
    }
}
