using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FormService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            if (!Environment.UserInteractive)
            {
                Console.WriteLine("Service");
                ServiceBase.Run(new batchService());
            }
            else
            {
                Console.WriteLine("Forms");
                Application.Run(new Main());
            }
        }
    }
}
