using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WiFi_Logger
{
    internal class Scanner
    {
        public void Start()
        {
            Net_Scan();
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
                string er = "--- ERROR Net_Scan  ---\n" + ex.Message + "\n";
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
    }
}
