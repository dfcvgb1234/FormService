using Kofax.Capture.DBLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace FormService
{
    class BatchManager
    {
        Login g_oLogin;
        Kofax.Capture.SDK.CustomModule.IRuntimeSession g_oRuntimeSession;
        Batch g_oActiveBatch;
        Batch g_oBatch;

        Mutex g_oMutex;

        /// <summary>
        /// Login to Capture runtime with information from XMLSettings
        /// </summary>
        public void LoginToRuntime()
        {
            g_oMutex = new Mutex();
            if (g_oLogin == null)
            {
                try
                {
                    g_oLogin = new Login();
                    g_oLogin.EnableSecurityBoost = true;
                    g_oLogin.Login();
                    Console.WriteLine("App Name: " + AppSettings.App_Name());
                    Console.WriteLine("App Version: " + AppSettings.App_Version());
                    g_oLogin.ApplicationName = AppSettings.App_Name();
                    g_oLogin.Version = AppSettings.App_Version();
                    Console.WriteLine("Username: " + XMLSettings.g_sUsername);
                    Console.WriteLine("Password: " + XMLSettings.g_sPassword);
                    g_oLogin.ValidateUser("batchService.retreive", false, XMLSettings.g_sUsername, XMLSettings.g_sPassword);
                }
                catch (Exception ex)
                {
                    Logging.LogMessage("OnLoad::Exception: " + ex.Message);
                }

                g_oRuntimeSession = g_oLogin.RuntimeSession;
                g_oRuntimeSession.BatchAvailable += G_oRuntimeSession_BatchAvailable;
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
                Environment.Exit(0);
            }
        }

        public delegate void NewBatchProcessHandler(object sender, Batch batch);
        public event NewBatchProcessHandler NewBatchProcess;

        /// <summary>
        /// Runs every time a batch is processed, this is entry level of batch processesing
        /// </summary>
        /// <returns>true if successful</returns>
        private bool ProcessNewBatch()
        {
            g_oBatch = null;
            try
            {
                g_oActiveBatch = (Batch)g_oRuntimeSession.NextBatchGet(g_oLogin.ProcessID, Kofax.Capture.SDK.CustomModule.KfxDbFilter.KfxDbFilterOnProcess | Kofax.Capture.SDK.CustomModule.KfxDbFilter.KfxDbFilterOnStates, Kofax.Capture.SDK.CustomModule.KfxDbState.KfxDbBatchReady | Kofax.Capture.SDK.CustomModule.KfxDbState.KfxDbBatchSuspended);

                if (g_oActiveBatch != null)
                {
                    g_oBatch = g_oActiveBatch;
                    NewBatchProcess?.Invoke(this, g_oBatch);
                    Logging.LogMessage("ActiveBatch::XMLExport, " + XMLSettings.g_sWorkingPath + "ActiveBatch.xml");
                    g_oBatch.XMLExport(XMLSettings.g_sWorkingPath + "ActiveBatch.xml");
                    ProcessXMLBatchField(XMLSettings.g_sWorkingPath + "ActiveBatch.xml");
                    g_oBatch.XMLImport(XMLSettings.g_sWorkingPath + "ActiveBatch.xml");
                    File.Delete(XMLSettings.g_sWorkingPath + "ActiveBatch.xml");
                    Logging.LogMessage("ProcessNewBatch::BatchClose");
                    CloseBatch();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Logging.LogMessage("ProcessNewBatch::ERROR, an error occured: " + ex.Message);
                CloseBatchWithError();
                return false;
            }
        }

        private void ProcessXMLBatchField(string sXMLPath)
        {
            try
            {
                XmlDocument xDoc = new XmlDocument();
                string sTimeToFormat = "";
                xDoc.Load(sXMLPath);

                Logging.LogMessage("ProcessXMLBatchField::LoopNodes");
                foreach (XmlElement xBatchField in xDoc.SelectSingleNode("//BatchFields").SelectNodes("BatchField"))
                {
                    if (xBatchField.Attributes.GetNamedItem("Name").Value.ToLower() == XMLSettings.g_sShortDateFormat.ToLower())
                    {
                        xBatchField.Attributes.GetNamedItem("Value").Value = LocalizationManager.GetCurrentShortDateFormat();
                        Logging.LogMessage("ProcessXMLBatchField::BatchFields::ShortDateFormat::" + LocalizationManager.GetCurrentShortDateFormat());
                    }
                    else if (xBatchField.Attributes.GetNamedItem("Name").Value.ToLower() == XMLSettings.g_sLongDateFormat.ToLower())
                    {
                        xBatchField.Attributes.GetNamedItem("Value").Value = LocalizationManager.GetCurrentLongDateFormat();
                        Logging.LogMessage("ProcessXMLBatchField::BatchFields::LongDateFormat::" + LocalizationManager.GetCurrentLongDateFormat());
                    }
                    else if (xBatchField.Attributes.GetNamedItem("Name").Value.ToLower() == XMLSettings.g_sShortTimeFormat.ToLower())
                    {
                        xBatchField.Attributes.GetNamedItem("Value").Value = LocalizationManager.GetCurrentShortTimeFormat();
                        Logging.LogMessage("ProcessXMLBatchField::BatchFields::ShortTimeFormat::" + LocalizationManager.GetCurrentShortTimeFormat());
                    }
                    else if (xBatchField.Attributes.GetNamedItem("Name").Value.ToLower() == XMLSettings.g_sLongTimeFormat.ToLower())
                    {
                        xBatchField.Attributes.GetNamedItem("Value").Value = LocalizationManager.GetCurrentLongTimeFormat();
                        Logging.LogMessage("ProcessXMLBatchField::BatchFields::LongTimeFormat::" + LocalizationManager.GetCurrentLongTimeFormat());
                    }
                    else if (xBatchField.Attributes.GetNamedItem("Name").Value.ToLower() == XMLSettings.g_sLocalizationValue.ToLower())
                    {
                        xBatchField.Attributes.GetNamedItem("Value").Value = LocalizationManager.GetCurrentLocalization();
                        Logging.LogMessage("ProcessXMLBatchField::BatchFields::CurrentLocalization::" + LocalizationManager.GetCurrentLocalization());
                    }
                    else if (xBatchField.Attributes.GetNamedItem("Name").Value.ToLower() == XMLSettings.g_sTimeStringToFormat.ToLower())
                    {
                        sTimeToFormat = xBatchField.Attributes.GetNamedItem("Value").Value;
                        Logging.LogMessage("ProcessXMLBatchField::BatchFields::TimeStringToFormat::" + xBatchField.Attributes.GetNamedItem("Value").Value);
                    }
                }

                if (sTimeToFormat.Length != 0)
                {
                    foreach (XmlElement xBatchField in xDoc.SelectSingleNode("//BatchFields").SelectNodes("BatchField"))
                    {
                        if (xBatchField.Attributes.GetNamedItem("Name").Value.ToLower() == XMLSettings.g_sFormattedDate.ToLower())
                        {
                            xBatchField.Attributes.GetNamedItem("Value").Value = LocalizationManager.ConvertDateToSpecificFormat(LocalizationManager.ParseDateTimeFromCurrentFormat(sTimeToFormat),
                                                                                                                                 XMLSettings.g_sTargetDateFormat);
                        }
                        else if (xBatchField.Attributes.GetNamedItem("Name").Value.ToLower() == XMLSettings.g_sFormattedTime.ToLower())
                        {
                            xBatchField.Attributes.GetNamedItem("Value").Value = LocalizationManager.ConvertTimeToSpecificFormat(LocalizationManager.ParseDateTimeFromCurrentFormat(sTimeToFormat),
                                                                                                                                 XMLSettings.g_sTargetTimeFormat);
                        }
                    }
                    Logging.LogMessage("ProcessXMLBatchField::SaveXML");
                    xDoc.Save(sXMLPath);
                }
                else
                {
                    Logging.LogMessage("ProcessXMLBatchField::There was no time to format, attempting to save...");
                    xDoc.Save(sXMLPath);
                    throw new Exception("ProcessXMLBatchField::There was no time to format");
                }
            }
            catch (Exception ex)
            {
                Logging.LogMessage("ProcessXMLBatchField::ERROR, an error occured: " + ex.Message);
                CloseBatchWithError();
            }
        }

        public void CloseBatchWithError(string message = "An Error Occurred",int errorCode = 1000)
        {
            if (g_oBatch != null)
            {
                g_oBatch.BatchClose(Kofax.Capture.SDK.CustomModule.KfxDbState.KfxDbBatchError, Kofax.Capture.SDK.CustomModule.KfxDbQueue.KfxDbQueueException, errorCode, message);
            }
            if (g_oActiveBatch != null)
            {
                g_oActiveBatch.BatchClose(Kofax.Capture.SDK.CustomModule.KfxDbState.KfxDbBatchError, Kofax.Capture.SDK.CustomModule.KfxDbQueue.KfxDbQueueException, errorCode, message);
            }
        }

        public void CloseBatch()
        {
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
}
