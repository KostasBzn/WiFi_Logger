using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace WiFi_Logger
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var sc = new Scanner();
                sc.Start();
            }
            catch (Exception ex) 
            {
                Debug.WriteLine("Error starting the app: " + ex);
            }
            
        }
    }
}
