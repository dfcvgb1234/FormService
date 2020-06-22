using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormService
{
    static class Logging
    {
        public static string LogMessage(string message)
        {
            StringBuilder oStringBuilder = new StringBuilder();
            oStringBuilder.AppendFormat("{0} {1}{2}", DateTime.UtcNow, message, Environment.NewLine);
            if (XMLSettings.g_bDebug)
            {

                string sLogFilePath = XMLSettings.g_sWorkingPath + "log.txt";
                try
                {

                    if (!File.Exists(sLogFilePath))
                    {
                        File.Create(sLogFilePath).Close();
                    }
                    File.AppendAllText(sLogFilePath, oStringBuilder.ToString());
                    return oStringBuilder.ToString();
                }
                catch (Exception ex)
                {
                    return "";
                }
            }
            else
            {
                return oStringBuilder.ToString();
            }
        }
        public static bool WriteToEventLog(string logMessage, string source = "batchService",
                                    EventLogEntryType entryType = EventLogEntryType.Information,
                                    string logName = "Service")
        {
            EventLog log = new EventLog();
            try
            {
                if (!EventLog.SourceExists(source))
                {
                    EventLog.CreateEventSource(source, logName);
                }
                log.Source = source;
                log.WriteEntry(logMessage, entryType);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
