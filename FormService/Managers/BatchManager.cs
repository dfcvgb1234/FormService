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
                if(!AppSettings.IsService()) { Environment.Exit(0); }
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
            string sSetupXMLPath = AppSettings.App_Path() + AppSettings.App_Name() + "Setup.xml";
            try
            {
                g_oActiveBatch = (Batch)g_oRuntimeSession.NextBatchGet(g_oLogin.ProcessID, Kofax.Capture.SDK.CustomModule.KfxDbFilter.KfxDbFilterOnProcess | Kofax.Capture.SDK.CustomModule.KfxDbFilter.KfxDbFilterOnStates, Kofax.Capture.SDK.CustomModule.KfxDbState.KfxDbBatchReady | Kofax.Capture.SDK.CustomModule.KfxDbState.KfxDbBatchSuspended);

                if (g_oActiveBatch != null)
                {
                    g_oBatch = g_oActiveBatch;
                    XMLSettings.LoadXMLSettings(sSetupXMLPath, g_oBatch.BatchClassName.Replace(" ", ""));
                    NewBatchProcess?.Invoke(this, g_oBatch);
                    if (AppSettings.IsService()) { Logging.WriteToEventLog("ActiveBatch::XMLExport, " + XMLSettings.g_sWorkingPath + "ActiveBatch.xml"); }
                    Logging.LogMessage("ActiveBatch::XMLExport, " + XMLSettings.g_sWorkingPath + "ActiveBatch.xml");
                    g_oBatch.XMLExport(XMLSettings.g_sWorkingPath + "ActiveBatch.xml");
                    ProcessXMLBatchField(XMLSettings.g_sWorkingPath + "ActiveBatch.xml");
                    g_oBatch.XMLImport(XMLSettings.g_sWorkingPath + "ActiveBatch.xml");
                    File.Delete(XMLSettings.g_sWorkingPath + "ActiveBatch.xml");
                    if (AppSettings.IsService()) { Logging.WriteToEventLog("ProcessNewBatch::BatchClose"); }
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
                        try
                        {
                            xBatchField.Attributes.GetNamedItem("Value").Value = LocalizationManager.GetCurrentShortDateFormat();
                        }
                        catch (Exception ex)
                        {
                            XmlAttribute attr = xDoc.CreateAttribute("Value");
                            attr.Value = LocalizationManager.GetCurrentShortDateFormat();
                            xBatchField.Attributes.InsertAfter(attr, xBatchField.Attributes.GetNamedItem("Name") as XmlAttribute);
                        }
                        Logging.LogMessage("ProcessXMLBatchField::BatchFields::ShortDateFormat::" + LocalizationManager.GetCurrentShortDateFormat());
                    }
                    else if (xBatchField.Attributes.GetNamedItem("Name").Value.ToLower() == XMLSettings.g_sLongDateFormat.ToLower())
                    {
                        try
                        {
                            xBatchField.Attributes.GetNamedItem("Value").Value = LocalizationManager.GetCurrentLongDateFormat();
                        }
                        catch (Exception ex)
                        {
                            XmlAttribute attr = xDoc.CreateAttribute("Value");
                            attr.Value = LocalizationManager.GetCurrentLongDateFormat();
                            xBatchField.Attributes.InsertAfter(attr, xBatchField.Attributes.GetNamedItem("Name") as XmlAttribute);
                        }
                        Logging.LogMessage("ProcessXMLBatchField::BatchFields::LongDateFormat::" + LocalizationManager.GetCurrentLongDateFormat());
                    }
                    else if (xBatchField.Attributes.GetNamedItem("Name").Value.ToLower() == XMLSettings.g_sShortTimeFormat.ToLower())
                    {
                        try
                        {
                            xBatchField.Attributes.GetNamedItem("Value").Value = LocalizationManager.GetCurrentShortTimeFormat();
                        }
                        catch (Exception ex)
                        {
                            XmlAttribute attr = xDoc.CreateAttribute("Value");
                            attr.Value = LocalizationManager.GetCurrentShortTimeFormat();
                            xBatchField.Attributes.InsertAfter(attr, xBatchField.Attributes.GetNamedItem("Name") as XmlAttribute);
                        }
                        Logging.LogMessage("ProcessXMLBatchField::BatchFields::ShortTimeFormat::" + LocalizationManager.GetCurrentShortTimeFormat());
                    }
                    else if (xBatchField.Attributes.GetNamedItem("Name").Value.ToLower() == XMLSettings.g_sLongTimeFormat.ToLower())
                    {
                        try
                        {
                            xBatchField.Attributes.GetNamedItem("Value").Value = LocalizationManager.GetCurrentLongTimeFormat();
                        }
                        catch (Exception ex)
                        {
                            XmlAttribute attr = xDoc.CreateAttribute("Value");
                            attr.Value = LocalizationManager.GetCurrentLongTimeFormat();
                            xBatchField.Attributes.InsertAfter(attr, xBatchField.Attributes.GetNamedItem("Name") as XmlAttribute);
                        }
                        Logging.LogMessage("ProcessXMLBatchField::BatchFields::LongTimeFormat::" + LocalizationManager.GetCurrentLongTimeFormat());
                    }
                    else if (xBatchField.Attributes.GetNamedItem("Name").Value.ToLower() == XMLSettings.g_sLocalizationValue.ToLower())
                    {
                        try
                        {
                            xBatchField.Attributes.GetNamedItem("Value").Value = LocalizationManager.GetCurrentLocalization();
                        }
                        catch (Exception ex)
                        {
                            XmlAttribute attr = xDoc.CreateAttribute("Value");
                            attr.Value = LocalizationManager.GetCurrentLocalization();
                            xBatchField.Attributes.InsertAfter(attr, xBatchField.Attributes.GetNamedItem("Name") as XmlAttribute);
                        }
                        Logging.LogMessage("ProcessXMLBatchField::BatchFields::CurrentLocalization::" + LocalizationManager.GetCurrentLocalization());
                    }
                    foreach(BatchField batchField in XMLSettings.g_oStringToFormat)
                    {
                        if(xBatchField.Attributes.GetNamedItem("Name").Value.ToLower() == batchField.InName.ToLower())
                        {
                            Logging.LogMessage("ProcessXMLBatchField::CustomBatchFields::Input::" + batchField.InName);
                            sTimeToFormat = xBatchField.Attributes.GetNamedItem("Value").Value;

                            foreach (XmlElement localBatchField in xDoc.SelectSingleNode("//BatchFields").SelectNodes("BatchField"))
                            {
                                if (!String.IsNullOrWhiteSpace(sTimeToFormat) && localBatchField.Attributes.GetNamedItem("Name").Value.ToLower() == batchField.DateOutName.ToLower())
                                {
                                    try 
                                    {
                                        localBatchField.Attributes.GetNamedItem("Value").Value = LocalizationManager.ConvertDateToSpecificFormat(LocalizationManager.ParseDateTimeFromCurrentFormat(sTimeToFormat),
                                                                                                                                                            batchField.FormatDate);
                                    }
                                    catch(Exception ex)
                                    {
                                        XmlAttribute attr = xDoc.CreateAttribute("Value");
                                        attr.Value = LocalizationManager.ConvertDateToSpecificFormat(LocalizationManager.ParseDateTimeFromCurrentFormat(sTimeToFormat),
                                                                                                                batchField.FormatDate);
                                        localBatchField.Attributes.InsertAfter(attr, localBatchField.Attributes.GetNamedItem("Name") as XmlAttribute);
                                    }
                                                                                                                                       
                                    Logging.LogMessage("ProcessXMLBatchField::CustomBatchFields::CompletedOutput");
                                }
                                else if (!String.IsNullOrWhiteSpace(sTimeToFormat) && localBatchField.Attributes.GetNamedItem("Name").Value.ToLower() == batchField.TimeOutName.ToLower())
                                {
                                    try
                                    {
                                        localBatchField.Attributes.GetNamedItem("Value").Value = LocalizationManager.ConvertTimeToSpecificFormat(LocalizationManager.ParseDateTimeFromCurrentFormat(sTimeToFormat),
                                                                                                                                                            batchField.FormatTime);
                                    }
                                    catch (Exception ex)
                                    {
                                        XmlAttribute attr = xDoc.CreateAttribute("Value");
                                        attr.Value = LocalizationManager.ConvertTimeToSpecificFormat(LocalizationManager.ParseDateTimeFromCurrentFormat(sTimeToFormat),
                                                                                                                batchField.FormatTime);
                                        localBatchField.Attributes.InsertAfter(attr, localBatchField.Attributes.GetNamedItem("Name") as XmlAttribute);
                                    }

                                    Logging.LogMessage("ProcessXMLBatchField::CustomBatchFields::CompletedOutput");
                                }
                            }
                        }
                    }
                }
                Logging.LogMessage("ProcessXMLBatchField::SaveXML");
                xDoc.Save(sXMLPath);
                Logging.LogMessage("ProcessXMLBatchField::Done Processing XML");
            }
            catch (Exception ex)
            {
                Logging.LogMessage("ProcessXMLBatchField::ERROR, an error occured: " + ex.Message + "::" + ex.StackTrace);
                CloseBatchWithError();
            }
        }

        public void CloseBatchWithError(string message = "An Error Occurred",int errorCode = 1000)
        {
            if (AppSettings.IsService()) { Logging.WriteToEventLog(String.Format("An Error Occurred in a batch: {0}, closing batch", message)); }
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
