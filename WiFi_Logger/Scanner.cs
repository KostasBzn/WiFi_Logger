using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.Win32;

namespace WiFi_Logger
{
    internal class Scanner
    {  
        public void Start()
        {
            try
            {
                string keyPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\CapabilityAccessManager\ConsentStore\location";
                string valueName = "Value";
                string lmVal = Registry.LocalMachine.OpenSubKey(keyPath)?.GetValue(valueName)?.ToString();
                string cuVal = Registry.CurrentUser.OpenSubKey(keyPath)?.GetValue(valueName)?.ToString();
                bool locServOn = "Allow".Equals(lmVal, StringComparison.OrdinalIgnoreCase) &&
                          "Allow".Equals(cuVal, StringComparison.OrdinalIgnoreCase);

                if (!locServOn)
                {
                    if (IsAdmin())
                    {
                        if (!"Allow".Equals(lmVal, StringComparison.OrdinalIgnoreCase))
                            Registry.LocalMachine.CreateSubKey(keyPath).SetValue(valueName, "Allow", RegistryValueKind.String);

                        if (!"Allow".Equals(cuVal, StringComparison.OrdinalIgnoreCase))
                            Registry.CurrentUser.CreateSubKey(keyPath).SetValue(valueName, "Allow", RegistryValueKind.String);
                    }
                    else
                    {
                        SaveToFile("Location services are disabled, run as admin to enable and scan.");
                        return;
                    }
                }
                Debug.WriteLine("Running NetScan", "Status");
                Net_Scan();
            }

            catch (Exception ex) 
            {
                string er = "--- ERROR  ---\n" + ex.Message;
                SaveToFile(er);
            }
            
        } 

        private void Net_Scan()
        {
            try
            {
                Process proc = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "netsh",
                        Arguments = "wlan show networks mode=bssid",
                        WindowStyle = ProcessWindowStyle.Hidden,
                        RedirectStandardOutput = true,
                        UseShellExecute = false,
                        CreateNoWindow = true,
                        StandardOutputEncoding = Encoding.UTF8,
                    }
                };
                proc.Start();
                string output = proc.StandardOutput.ReadToEnd();
                proc.WaitForExit();
                Debug.WriteLine("scan result \n" + output); 
                SaveToFile(output);

            }
            catch (Exception ex) 
            {
                Debug.WriteLine("Net_Scan failed: " + ex.Message);
                string er = "--- ERROR Net_Scan  ---\n" + ex.Message;
                SaveToFile(er);
            }

        }

        private void SaveToFile(string output)
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wifi_scan.txt");

            using (StreamWriter sw = new StreamWriter(filePath, false, Encoding.UTF8))
            {
                sw.WriteLine(output);
            }
        }

        public static bool IsAdmin()
        {
            return new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);
        }
    }
}
