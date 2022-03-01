using System;
using CLAP;
using System.Text.RegularExpressions;
using System.Diagnostics;
using Microsoft.Win32;

namespace ProtocolHandler
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Parser.Run<App>(args);
            }
            catch(Exception e)
            {
#if DEBUG
                throw e;
#else
                Console.WriteLine(e.Message);
#endif
            }

#if DEBUG
            Console.ReadLine();
#endif
        }
    }

    class App
    {
        [Verb(IsDefault = true)]
        public static void Open(string path)
        {
            path = path.UrlDecode();

            Console.WriteLine($"trying to resolve {path}");

            ProtocolResult proto = ResolveProtocol(path);

            if(proto == null)
            {
                throw new Exception("the protocol could not be resolved");
            }

            Console.WriteLine($"path resolved to protocol {proto.Protocol} and file {proto.Path}");

            Config config = Config.LoadFromRegistry(proto.Protocol);

            if(config == null)
            {
                throw new Exception($"no config found for protocol {proto.Protocol}");
            }

            Console.WriteLine($"config loaded for protocol {proto.Protocol}");

            Console.WriteLine($"opening file {proto.Path} with handler {config.HandlerPath}");

            //open file
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.CreateNoWindow = false;
            startInfo.UseShellExecute = false;
            startInfo.FileName = config.HandlerPath;
            startInfo.Arguments = $"\"{proto.Path}\" {config.AdditionalArguments}";
            Process exeProcess = Process.Start(startInfo);
        }

        [Verb]
        public static void InstallUrlSchemes(string[] schemes, bool forMachine = false)
        {
            Config.HIVE_KEYS key = Config.HIVE_KEYS.HKCU;

            if (Config.IsAdministrator() && forMachine)
            {
                key = Config.HIVE_KEYS.HKLM;

                Console.WriteLine("installing schemes for local machine");
            }
            else
            {
                Console.WriteLine("installing schemes for current user");
            }

            string launchPath = "\"" + System.Reflection.Assembly.GetExecutingAssembly().Location + "\" Open -path=\"%1\"";

            foreach (string scheme in schemes)
            {
                string _base = @"Software\Classes\" + scheme;

                Config.SetRegistryValue(key, _base, "URL protocol", "", RegistryValueKind.String);
                Config.SetRegistryValue(key, _base, "UseOriginalUrlEncoding", 1, RegistryValueKind.DWord);
                Config.SetRegistryValue(key, _base + @"\shell\open\command", "", launchPath, RegistryValueKind.String);
            }
        }

        [Verb]
        public static void RegisterScheme(string protocol, string handlerPath, string arguments = "", bool forMachine = false)
        {
            Config.HIVE_KEYS key = Config.HIVE_KEYS.HKCU;

            if (Config.IsAdministrator() && forMachine)
            {
                key = Config.HIVE_KEYS.HKLM;

                Console.WriteLine($"registering scheme {protocol} for local machine");
            }
            else
            {
                Console.WriteLine($"registering scheme {protocol} for current user");
            }

            Config.SetRegistryValue(key, Config.REG_BASE + @"\" + protocol, "HandlerPath", handlerPath, RegistryValueKind.String);

            if(arguments != "" && arguments != null)
                Config.SetRegistryValue(key, Config.REG_BASE + @"\" + protocol, "AdditionalArguments", arguments, RegistryValueKind.String);
        }

        private static ProtocolResult ResolveProtocol(string path)
        {
            string[] patterns = new string[]
            {
                @"(ms-excel):ofe\|u\|(https?:\/\/.*\.xlsx?)",
                @"(ms-word):ofe\|u\|(https?:\/\/.*\.docx?)",
                @"(ms-powerpoint):ofe\|u\|(https?:\/\/.*\.pptx?)"
            };

            ProtocolResult result = null;

            foreach(string pattern in patterns)
            {
                Match match = Regex.Match(path, pattern);

                if(match.Success)
                {
                    result = new ProtocolResult(match.Groups[1].Value, match.Groups[2].Value);

                    break;
                }
            }

            return result;
        }
    }
}
