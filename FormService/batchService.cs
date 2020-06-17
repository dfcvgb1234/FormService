using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Xml;
using System.IO;
using System.Reflection;
using Kofax.Capture.DBLite;
using System.Threading;

namespace FormService
{
    partial class batchService : ServiceBase
    {
        System.Timers.Timer timer = new System.Timers.Timer();

        string g_sWorkingPath;
        string g_sBatchField;
        string g_sUsername;
        string g_sPassword;
        bool g_bDebug;

        // BatchFields
        string g_sShortDateFormat;
        string g_sLongDateFormat;
        string g_sShortTimeFormat;
        string g_sLongTimeFormat;
        string g_sLocalizationValue;
        string g_sTimeStringToFormat;
        string g_sFormattedDate;
        string g_sFormattedTime;

        // Formatter
        string g_sTargetDateFormat;
        string g_sTargetTimeFormat;

        Batch g_oActiveBatch;
        Batch g_oBatch;
        Login g_oLogin;
        Kofax.Capture.SDK.CustomModule.IRuntimeSession g_oRuntimeSession;

        Mutex g_oMutex;

        public batchService()
        {
            timer.Interval = 60000;
            timer.Elapsed += Timer_Elapsed; ;
            timer.Enabled = true;

            try
            {
                
            }
            catch (Exception ex)
            {
                WriteToEventLog(String.Format("Exception: {0}, occured in {1}: {2}", ex.InnerException, ex.TargetSite, ex.Message), "batchService", EventLogEntryType.Error);
                // Unable to write to log when the error can not find the workingPath
                //LogMessage(String.Format("Exception: {0}, occured in {1}: {2}", ex.InnerException, ex.TargetSite, ex.Message));
            }
            InitializeComponent();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            WriteToEventLog("Heartbeat at: " + DateTime.Now.ToString("HH:mm:ss"));
        }

        protected override void OnStart(string[] args)
        {
            
            timer.Start();
        }
        private void LoadXMLSettings(string XMLPath)
        {
            try
            {
                XmlDocument oSetupXML = new XmlDocument();
                oSetupXML.Load(XMLPath);

                string sXmlValue;

                sXmlValue = oSetupXML.SelectSingleNode("//Debug").InnerText;
                if (sXmlValue.Length == 0 || sXmlValue.ToLower().Contains("false"))
                {
                    g_bDebug = false;
                }
                else
                {
                    g_bDebug = true;
                }
                LogMessage("LoadXMLSettings::Debug, " + g_bDebug);

                sXmlValue = oSetupXML.SelectSingleNode("//BatchFields").SelectSingleNode("ShortDateFormat").InnerText;
                if (sXmlValue.Length != 0)
                {
                    g_sShortDateFormat = sXmlValue;
                }
                else
                {
                    g_sShortDateFormat = "ShortDateFormat";
                }
                LogMessage("LoadXMLSettings::ShortDateFormatBatchField, " + g_sShortDateFormat);

                sXmlValue = oSetupXML.SelectSingleNode("//BatchFields").SelectSingleNode("LongDateFormat").InnerText;
                if (sXmlValue.Length != 0)
                {
                    g_sLongDateFormat = sXmlValue;
                }
                else
                {
                    g_sLongDateFormat = "LongDateFormat";
                }
                LogMessage("LoadXMLSettings::LongDateFormatBatchField, " + g_sLongDateFormat);

                sXmlValue = oSetupXML.SelectSingleNode("//BatchFields").SelectSingleNode("ShortTimeFormat").InnerText;
                if (sXmlValue.Length != 0)
                {
                    g_sShortTimeFormat = sXmlValue;
                }
                else
                {
                    g_sShortTimeFormat = "ShortTimeFormat";
                }
                LogMessage("LoadXMLSettings::ShortTimeFormatBatchField, " + g_sShortTimeFormat);

                sXmlValue = oSetupXML.SelectSingleNode("//BatchFields").SelectSingleNode("LongTimeFormat").InnerText;
                if (sXmlValue.Length != 0)
                {
                    g_sLongTimeFormat = sXmlValue;
                }
                else
                {
                    g_sLongTimeFormat = "LongTimeFormat";
                }
                LogMessage("LoadXMLSettings::LongTimeFormatBatchField, " + g_sLongTimeFormat);

                sXmlValue = oSetupXML.SelectSingleNode("//BatchFields").SelectSingleNode("LocalizationValue").InnerText;
                if (sXmlValue.Length != 0)
                {
                    g_sLocalizationValue = sXmlValue;
                }
                else
                {
                    g_sLocalizationValue = "LocalizationValue";
                }
                LogMessage("LoadXMLSettings::LocalizationValueBatchField, " + g_sLocalizationValue);

                sXmlValue = oSetupXML.SelectSingleNode("//BatchFields").SelectSingleNode("TimeStringToFormat").InnerText;
                if (sXmlValue.Length != 0)
                {
                    g_sTimeStringToFormat = sXmlValue;
                }
                else
                {
                    g_sTimeStringToFormat = "TimeStringToFormat";
                }
                LogMessage("LoadXMLSettings::TimeStringToFormatBatchField, " + g_sTimeStringToFormat);

                sXmlValue = oSetupXML.SelectSingleNode("//BatchFields").SelectSingleNode("FormattedDate").InnerText;
                if (sXmlValue.Length != 0)
                {
                    g_sFormattedDate = sXmlValue;
                }
                else
                {
                    g_sFormattedDate = "FormattedDate";
                }
                LogMessage("LoadXMLSettings::FormattedDateBatchField, " + g_sFormattedDate);

                sXmlValue = oSetupXML.SelectSingleNode("//BatchFields").SelectSingleNode("FormattedTime").InnerText;
                if (sXmlValue.Length != 0)
                {
                    g_sFormattedTime = sXmlValue;
                }
                else
                {
                    g_sFormattedTime = "FormattedTime";
                }
                LogMessage("LoadXMLSettings::FormattedTimeBatchField, " + g_sFormattedTime);

                sXmlValue = oSetupXML.SelectSingleNode("//Formatter").SelectSingleNode("TargetDateFormat").InnerText;
                if (sXmlValue.Length != 0)
                {
                    g_sTargetDateFormat = sXmlValue;
                }
                else
                {
                    g_sTargetDateFormat = "TargetDateFormat";
                }
                LogMessage("LoadXMLSettings::TargetDateFormatBatchField, " + g_sTargetDateFormat);

                sXmlValue = oSetupXML.SelectSingleNode("//Formatter").SelectSingleNode("TargetTimeFormat").InnerText;
                if (sXmlValue.Length != 0)
                {
                    g_sTargetTimeFormat = sXmlValue;
                }
                else
                {
                    g_sTargetTimeFormat = "TargetTimeFormat";
                }
                LogMessage("LoadXMLSettings::TargetTimeFormatBatchField, " + g_sTargetTimeFormat);
                LogMessage("LoadXMLSettings::Done Loading Settings");
            }
            catch (Exception ex)
            {
                LogMessage("LoadXMLSettings::Error Loading XML Settings");
                if (g_oBatch != null)
                {
                    g_oBatch.BatchClose(Kofax.Capture.SDK.CustomModule.KfxDbState.KfxDbBatchReady, Kofax.Capture.SDK.CustomModule.KfxDbQueue.KfxDbQueueNext, 0, "");
                }
                if (g_oActiveBatch != null)
                {
                    g_oActiveBatch.BatchClose(Kofax.Capture.SDK.CustomModule.KfxDbState.KfxDbBatchReady, Kofax.Capture.SDK.CustomModule.KfxDbQueue.KfxDbQueueNext, 0, "");
                }
            }
        }

        protected override void OnStop()
        {
            timer.Stop();
            if (g_oBatch != null)
            {
                g_oBatch.BatchClose(Kofax.Capture.SDK.CustomModule.KfxDbState.KfxDbBatchReady, Kofax.Capture.SDK.CustomModule.KfxDbQueue.KfxDbQueueNext, 0, "");
            }
            if (g_oActiveBatch != null)
            {
                g_oActiveBatch.BatchClose(Kofax.Capture.SDK.CustomModule.KfxDbState.KfxDbBatchReady, Kofax.Capture.SDK.CustomModule.KfxDbQueue.KfxDbQueueNext, 0, "");
            }
        }

        private bool ProcessNewBatch()
        {
            g_oBatch = null;
            try
            {
                g_oActiveBatch = (Batch)g_oRuntimeSession.NextBatchGet(g_oLogin.ProcessID, Kofax.Capture.SDK.CustomModule.KfxDbFilter.KfxDbFilterOnProcess | Kofax.Capture.SDK.CustomModule.KfxDbFilter.KfxDbFilterOnStates, Kofax.Capture.SDK.CustomModule.KfxDbState.KfxDbBatchReady | Kofax.Capture.SDK.CustomModule.KfxDbState.KfxDbBatchSuspended);

                if (g_oActiveBatch != null)
                {
                    g_oBatch = g_oActiveBatch;
                    LogMessage("ActiveBatch::XMLExport, " + g_sWorkingPath + "ActiveBatch.xml");
                    g_oBatch.XMLExport(g_sWorkingPath + "ActiveBatch.xml");
                    ProcessXMLBatchField(g_sWorkingPath + "ActiveBatch.xml", g_sBatchField, "Cookies");
                    g_oBatch.XMLImport(g_sWorkingPath + "ActiveBatch.xml");
                    File.Delete(g_sWorkingPath + "ActiveBatch.xml");
                    g_oBatch.BatchClose(Kofax.Capture.SDK.CustomModule.KfxDbState.KfxDbBatchReady, Kofax.Capture.SDK.CustomModule.KfxDbQueue.KfxDbQueueNext, 0, "");
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                WriteToEventLog(String.Format("Exception: {0}, occured in {1}: {2}", ex.InnerException, ex.TargetSite, ex.Message), "batchService", EventLogEntryType.Error);
                if (g_oBatch != null)
                {
                    g_oBatch.BatchClose(Kofax.Capture.SDK.CustomModule.KfxDbState.KfxDbBatchError, Kofax.Capture.SDK.CustomModule.KfxDbQueue.KfxDbQueueException, 1000, ex.Message);
                }
                if (g_oActiveBatch != null)
                {
                    g_oActiveBatch.BatchClose(Kofax.Capture.SDK.CustomModule.KfxDbState.KfxDbBatchError, Kofax.Capture.SDK.CustomModule.KfxDbQueue.KfxDbQueueException, 1000, ex.Message);
                }
                return false;
            }
        }

        private void ProcessXMLBatchField(string sXMLPath, string sBatchField, string sNewValue)
        {
            try
            {
                XmlDocument xDoc = new XmlDocument();
                string sTimeToFormat = "";
                string sFormattedDate = "";
                string sFormattedTime = "";
                xDoc.Load(sXMLPath);

                LogMessage("ProcessXMLBatchField::LoopNodes");
                foreach (XmlElement xBatchField in xDoc.SelectSingleNode("//BatchFields").SelectNodes("BatchField"))
                {
                    if (xBatchField.Attributes.GetNamedItem("Name").Value.ToLower() == g_sShortDateFormat.ToLower())
                    {
                        xBatchField.Attributes.GetNamedItem("Value").Value = LocalizationManager.GetCurrentShortDateFormat();
                        LogMessage("ProcessXMLBatchField::BatchFields::ShortDateFormat::" + LocalizationManager.GetCurrentShortDateFormat());
                    }
                    else if (xBatchField.Attributes.GetNamedItem("Name").Value.ToLower() == g_sLongDateFormat.ToLower())
                    {
                        xBatchField.Attributes.GetNamedItem("Value").Value = LocalizationManager.GetCurrentLongDateFormat();
                        LogMessage("ProcessXMLBatchField::BatchFields::LongDateFormat::" + LocalizationManager.GetCurrentLongDateFormat());
                    }
                    else if (xBatchField.Attributes.GetNamedItem("Name").Value.ToLower() == g_sShortTimeFormat.ToLower())
                    {
                        xBatchField.Attributes.GetNamedItem("Value").Value = LocalizationManager.GetCurrentShortTimeFormat();
                        LogMessage("ProcessXMLBatchField::BatchFields::ShortTimeFormat::" + LocalizationManager.GetCurrentShortTimeFormat());
                    }
                    else if (xBatchField.Attributes.GetNamedItem("Name").Value.ToLower() == g_sLongTimeFormat.ToLower())
                    {
                        xBatchField.Attributes.GetNamedItem("Value").Value = LocalizationManager.GetCurrentLongTimeFormat();
                        LogMessage("ProcessXMLBatchField::BatchFields::LongTimeFormat::" + LocalizationManager.GetCurrentLongTimeFormat());
                    }
                    else if (xBatchField.Attributes.GetNamedItem("Name").Value.ToLower() == g_sLocalizationValue.ToLower())
                    {
                        xBatchField.Attributes.GetNamedItem("Value").Value = LocalizationManager.GetCurrentLocalization();
                        LogMessage("ProcessXMLBatchField::BatchFields::CurrentLocalization::" + LocalizationManager.GetCurrentLocalization());
                    }
                    else if (xBatchField.Attributes.GetNamedItem("Name").Value.ToLower() == g_sTimeStringToFormat.ToLower())
                    {
                        sTimeToFormat = xBatchField.Attributes.GetNamedItem("Value").Value;
                        LogMessage("ProcessXMLBatchField::BatchFields::TimeStringToFormat::" + xBatchField.Attributes.GetNamedItem("Value").Value);
                    }
                }

                if (sTimeToFormat.Length != 0)
                {
                    foreach (XmlElement xBatchField in xDoc.SelectSingleNode("//BatchFields").SelectNodes("BatchField"))
                    {
                        if (xBatchField.Attributes.GetNamedItem("Name").Value.ToLower() == g_sFormattedDate.ToLower())
                        {
                            xBatchField.Attributes.GetNamedItem("Value").Value = LocalizationManager.ConvertDateToSpecificFormat(LocalizationManager.ParseDateTimeFromCurrentFormat(sTimeToFormat),
                                                                                                                                 g_sTargetDateFormat);
                        }
                        else if (xBatchField.Attributes.GetNamedItem("Name").Value.ToLower() == g_sFormattedTime.ToLower())
                        {
                            xBatchField.Attributes.GetNamedItem("Value").Value = LocalizationManager.ConvertTimeToSpecificFormat(LocalizationManager.ParseDateTimeFromCurrentFormat(sTimeToFormat),
                                                                                                                                 g_sTargetTimeFormat);
                        }
                    }
                    LogMessage("ProcessXMLBatchField::SaveXML");
                    xDoc.Save(sXMLPath);
                }
                else
                {
                    LogMessage("ProcessXMLBatchField::There was no time to format, attempting to save...");
                    xDoc.Save(sXMLPath);
                    throw new Exception("ProcessXMLBatchField::There was no time to format");
                }
            }
            catch (Exception ex)
            {
                LogMessage("ProcessXMLBatchField::ERROR, an error occured: " + ex.Message);
                if (g_oBatch != null)
                {
                    g_oBatch.BatchClose(Kofax.Capture.SDK.CustomModule.KfxDbState.KfxDbBatchError, Kofax.Capture.SDK.CustomModule.KfxDbQueue.KfxDbQueueException, 1000, ex.Message);
                }
                if (g_oActiveBatch != null)
                {
                    g_oActiveBatch.BatchClose(Kofax.Capture.SDK.CustomModule.KfxDbState.KfxDbBatchError, Kofax.Capture.SDK.CustomModule.KfxDbQueue.KfxDbQueueException, 1000, ex.Message);
                }
            }
        }

        private void G_oRuntimeSession_BatchAvailable()
        {
            g_oMutex.WaitOne();
            try
            {
                // This is absolutely brilliant
                while (ProcessNewBatch())
                {

                }

            }
            catch (Exception ex)
            { }
            finally
            {
                g_oMutex.ReleaseMutex();
            }
        }

        private void LogMessage(string message)
        {
            if (g_bDebug)
            {
                StringBuilder oStringBuilder = new StringBuilder();
                string sLogFilePath = g_sWorkingPath + "log.txt";
                try
                {
                    oStringBuilder.AppendFormat("{0} {1}{2}", DateTime.UtcNow, message, Environment.NewLine);
                    if (!File.Exists(sLogFilePath))
                    {
                        File.Create(sLogFilePath).Close();
                    }
                    File.AppendAllText(sLogFilePath, oStringBuilder.ToString());
                }
                catch (Exception ex)
                {
                    WriteToEventLog(String.Format("Exception: {0}, occured in {1}: {2}", ex.InnerException, ex.TargetSite, ex.Message), "batchService", EventLogEntryType.Error);
                }
            }
        }

        private string App_Path()
        {
            try
            {
                return AppDomain.CurrentDomain.BaseDirectory;
            }
            catch (AppDomainUnloadedException ex)
            {
                WriteToEventLog(String.Format("Exception: {0}, occured in {1}: {2}", ex.InnerException, ex.TargetSite, ex.Message), "batchService", EventLogEntryType.Error);
                LogMessage("App_Path(): Exception: " + ex.Message);
                return "";
            }
        }
        private string App_Name()
        {
            return Assembly.GetExecutingAssembly().GetName().Name;
        }
        private string App_Version()
        {
            return Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        private bool WriteToEventLog(string logMessage, string source = "batchService",
                                    EventLogEntryType entryType = EventLogEntryType.Information,
                                    string logName = "Service")
        {
            try
            {
                if (!EventLog.SourceExists(source))
                {
                    EventLog.CreateEventSource(source, logName);
                }
                EventLog.Source = source;
                EventLog.WriteEntry(logMessage, entryType);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
