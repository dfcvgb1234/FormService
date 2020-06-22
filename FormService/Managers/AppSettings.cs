using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FormService
{
    static class AppSettings
    {
        public static string App_Path()
        {
            try
            {
                return AppDomain.CurrentDomain.BaseDirectory;
            }
            catch (AppDomainUnloadedException ex)
            {
                Logging.LogMessage("App_Path(): Exception: " + ex.Message);
                return "";
            }
        }
        public static string App_Name()
        {
            try
            {
                return Assembly.GetExecutingAssembly().GetName().Name;
            }
            catch(Exception ex)
            {
                Logging.LogMessage("App_Name(): Exception: " + ex.Message);
                return "";
            }
        }
        public static string App_Version()
        {
            return Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }
        public static bool IsService()
        {
            return !Environment.UserInteractive;
        }
    }
}
