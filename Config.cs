using IniParser;
using IniParser.Model;
using System.IO;
using System.Diagnostics;
using Microsoft.Win32;
using System.Security.Principal;


namespace ProtocolHandler
{
    class Config
    {
        public enum HIVE_KEYS { HKCU, HKLM, HKCR }

        public static string REG_BASE = "Software\\fweber-de\\ProtocolHandler";

        public string Protocol { get; set; }

        public string HandlerPath { get; set; }

        public string AdditionalArguments { get; set; }

        public static Config LoadFromFile(string protocol)
        {
            string filename = $"config.{protocol}.ini";

            if(!File.Exists(filename))
            {
                return null;
            }

            Config config = new Config();
            var parser = new FileIniDataParser();
            IniData data = parser.ReadFile(filename);

            config.Protocol = protocol;
            config.HandlerPath = data["Handler"]["Path"];
            config.AdditionalArguments = data["Handler"]["AdditionalArguments"];

            return config;
        }

        public static Config LoadFromRegistry(string protocol)
        {
            Config config = new Config();

            config.Protocol = protocol;
            config.HandlerPath = (string)GetSettingFromRegistry(protocol, "HandlerPath");
            config.AdditionalArguments = (string)GetSettingFromRegistry(protocol, "AdditionalArguments");

            if(config.HandlerPath == "" || config.HandlerPath == null)
            {
                return null;
            }

            return config;
        }

        public static object GetSettingFromRegistry(string path, string key, string def = null)
        {
            string _base = REG_BASE;

            if(path != "")
            {
                _base = _base + @"\" + path;
            }

            object value = Config.GetRegistryValue(HIVE_KEYS.HKCU, _base, key, def);

            if (value != null)
            {
                return value;
            }

            value = Config.GetRegistryValue(HIVE_KEYS.HKLM, _base, key, def);

            if (value != null)
            {
                return value;
            }

            return def;
        }

        public static void SetRegistryValue(HIVE_KEYS hiveKey, string keyPath, string key, object value, RegistryValueKind registryValueKind)
        {
            string hk = "";

            if (hiveKey == HIVE_KEYS.HKCU)
            {
                hk = "HKEY_CURRENT_USER";
            }

            if (hiveKey == HIVE_KEYS.HKLM)
            {
                hk = "HKEY_LOCAL_MACHINE";
            }

            if (hiveKey == HIVE_KEYS.HKCR)
            {
                hk = "HKEY_CLASSES_ROOT";
            }

            Registry.SetValue(hk + "\\" + keyPath, key, value, registryValueKind);
        }

        public static object GetRegistryValue(HIVE_KEYS hiveKey, string keyPath, string key, string defaultValue = null)
        {
            string hk = "";

            if (hiveKey == HIVE_KEYS.HKCU)
            {
                hk = "HKEY_CURRENT_USER";
            }

            if (hiveKey == HIVE_KEYS.HKLM)
            {
                hk = "HKEY_LOCAL_MACHINE";
            }

            if (hiveKey == HIVE_KEYS.HKCR)
            {
                hk = "HKEY_CLASSES_ROOT";
            }

            return Registry.GetValue(hk + "\\" + keyPath, key, defaultValue);
        }

        public static bool IsAdministrator()
        {
            using (WindowsIdentity identity = WindowsIdentity.GetCurrent())
            {
                WindowsPrincipal principal = new WindowsPrincipal(identity);
                return principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
        }
    }
}
