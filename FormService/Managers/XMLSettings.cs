using Kofax.Capture.DBLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace FormService
{
    public static class XMLSettings
    {
        // User
        public static string g_sUsername;
        public static string g_sPassword;

        // BatchFields
        public static string g_sShortDateFormat;
        public static string g_sLongDateFormat;
        public static string g_sShortTimeFormat;
        public static string g_sLongTimeFormat;
        public static string g_sLocalizationValue;
        public static string g_sTimeStringToFormat;
        public static BatchField[] g_oStringToFormat;
        public static string g_sFormattedDate;
        public static string g_sFormattedTime;

        // Formatter
        public static string g_sTargetDateFormat;
        public static string g_sTargetTimeFormat;

        // FormatterSettings
        public static bool g_bUseWaterfallFormatter;
        public static bool g_bUseDistanceRule;
        public static int g_iDistanceRuleDays;
        public static bool g_bDistanceRuleFuture;

        // OtherSettings
        public static bool  g_bDebug;
        public static string g_sWorkingPath;

        // Defaults
        public static string[] g_aShortDateFormat_DEF;
        public static string[] g_aLongDateFormat_DEF;
        public static string[] g_aShortTimeFormat_DEF;
        public static string[] g_aLongTimeFormat_DEF;
        public static string[] g_aLocalizationValue_DEF;
        public static string[] g_aTimeStringToFormat_DEF;
        public static string[] g_aFormattedDate_DEF;
        public static string[] g_aFormattedTime_DEF;
        public static string[] g_aTargetDateFormat_DEF;
        public static string[] g_aTargetTimeFormat_DEF;
        public static string[] g_aUseWaterfallFormater_DEF;
        public static string[] g_aUseDistanceRule_DEF;
        public static string[] g_aDistanceRuleDays_DEF;
        public static string[] g_aDistanceRuleFuture_DEF;
        public static List<List<string>> g_aMethodOrder_DEF;

        // WaterfallPrioritization
        public static List<string> g_aMethodOrder;

        public static void LoadUserSettings(string XMLPath)
        {
            XmlDocument oUserProfileSetupXML = new XmlDocument();
            string sXmlValue;

            oUserProfileSetupXML.Load(XMLPath);

            sXmlValue = oUserProfileSetupXML.SelectSingleNode("//Username").InnerText;
            if (sXmlValue.Length != 0)
            {
                g_sUsername = sXmlValue;
            }
            else
            {
                g_sUsername = "";
            }
            Logging.LogMessage("OnLoad::Username, " + g_sUsername);

            sXmlValue = oUserProfileSetupXML.SelectSingleNode("//Password").InnerText;
            if (sXmlValue.Length != 0)
            {
                g_sPassword = sXmlValue;
            }
            else
            {
                g_sPassword = "";
            }
            Logging.LogMessage("OnLoad::Password, " + g_sPassword);
        }

        public static void LoadDefaults(string XMLPath)
        {
            XmlDocument oSetupXML = new XmlDocument();
            oSetupXML.Load(XMLPath);

            string sXmlValue;
            XmlNode selectedNode;

            selectedNode = oSetupXML.SelectSingleNode("//Defaults").SelectSingleNode("ShortDateFormat");
            g_aShortDateFormat_DEF = new string[selectedNode.ChildNodes.Count];
            for (int i = 0; i < selectedNode.ChildNodes.Count; i++)
            {
                g_aShortDateFormat_DEF[i] = selectedNode.ChildNodes[i].InnerText;
                Logging.LogMessage("LoadDefaults::ShortDateFormat::" + selectedNode.ChildNodes[i].InnerText);
            }

            selectedNode = oSetupXML.SelectSingleNode("//Defaults").SelectSingleNode("LongDateFormat");
            g_aLongDateFormat_DEF = new string[selectedNode.ChildNodes.Count];
            for (int i = 0; i < selectedNode.ChildNodes.Count; i++)
            {
                g_aLongDateFormat_DEF[i] = selectedNode.ChildNodes[i].InnerText;
                Logging.LogMessage("LoadDefaults::LongDateFormat::" + selectedNode.ChildNodes[i].InnerText);
            }

            selectedNode = oSetupXML.SelectSingleNode("//Defaults").SelectSingleNode("ShortTimeFormat");
            g_aShortTimeFormat_DEF = new string[selectedNode.ChildNodes.Count];
            for (int i = 0; i < selectedNode.ChildNodes.Count; i++)
            {
                g_aShortTimeFormat_DEF[i] = selectedNode.ChildNodes[i].InnerText;
                Logging.LogMessage("LoadDefaults::ShortTimeFormat::" + selectedNode.ChildNodes[i].InnerText);
            }

            selectedNode = oSetupXML.SelectSingleNode("//Defaults").SelectSingleNode("LongTimeFormat");
            g_aLongTimeFormat_DEF = new string[selectedNode.ChildNodes.Count];
            for (int i = 0; i < selectedNode.ChildNodes.Count; i++)
            {
                g_aLongTimeFormat_DEF[i] = selectedNode.ChildNodes[i].InnerText;
                Logging.LogMessage("LoadDefaults::LongTimeFormat::" + selectedNode.ChildNodes[i].InnerText);
            }

            selectedNode = oSetupXML.SelectSingleNode("//Defaults").SelectSingleNode("LocalizationValue");
            g_aLocalizationValue_DEF = new string[selectedNode.ChildNodes.Count];
            for (int i = 0; i < selectedNode.ChildNodes.Count; i++)
            {
                g_aLocalizationValue_DEF[i] = selectedNode.ChildNodes[i].InnerText;
                Logging.LogMessage("LoadDefaults::LocalizationValue::" + selectedNode.ChildNodes[i].InnerText);
            }

            selectedNode = oSetupXML.SelectSingleNode("//Defaults").SelectSingleNode("TimeStringToFormat");
            g_aTimeStringToFormat_DEF = new string[selectedNode.ChildNodes.Count];
            for (int i = 0; i < selectedNode.ChildNodes.Count; i++)
            {
                g_aTimeStringToFormat_DEF[i] = selectedNode.ChildNodes[i].InnerText;
                Logging.LogMessage("LoadDefaults::TimeStringToFormat::" + selectedNode.ChildNodes[i].InnerText);
            }

            selectedNode = oSetupXML.SelectSingleNode("//Defaults").SelectSingleNode("FormattedDate");
            g_aFormattedDate_DEF = new string[selectedNode.ChildNodes.Count];
            for (int i = 0; i < selectedNode.ChildNodes.Count; i++)
            {
                g_aFormattedDate_DEF[i] = selectedNode.ChildNodes[i].InnerText;
                Logging.LogMessage("LoadDefaults::FormattedDate::" + selectedNode.ChildNodes[i].InnerText);
            }

            selectedNode = oSetupXML.SelectSingleNode("//Defaults").SelectSingleNode("FormattedTime");
            g_aFormattedTime_DEF = new string[selectedNode.ChildNodes.Count];
            for (int i = 0; i < selectedNode.ChildNodes.Count; i++)
            {
                g_aFormattedTime_DEF[i] = selectedNode.ChildNodes[i].InnerText;
                Logging.LogMessage("LoadDefaults::FormattedTime::" + selectedNode.ChildNodes[i].InnerText);
            }

            selectedNode = oSetupXML.SelectSingleNode("//Defaults").SelectSingleNode("TargetDateFormat");
            g_aTargetDateFormat_DEF = new string[selectedNode.ChildNodes.Count];
            for (int i = 0; i < selectedNode.ChildNodes.Count; i++)
            {
                g_aTargetDateFormat_DEF[i] = selectedNode.ChildNodes[i].InnerText;
                Logging.LogMessage("LoadDefaults::TargetDateFormat::" + selectedNode.ChildNodes[i].InnerText);
            }

            selectedNode = oSetupXML.SelectSingleNode("//Defaults").SelectSingleNode("TargetTimeFormat");
            g_aTargetTimeFormat_DEF = new string[selectedNode.ChildNodes.Count];
            for (int i = 0; i < selectedNode.ChildNodes.Count; i++)
            {
                g_aTargetTimeFormat_DEF[i] = selectedNode.ChildNodes[i].InnerText;
                Logging.LogMessage("LoadDefaults::TargetTimeFormat::" + selectedNode.ChildNodes[i].InnerText);
            }

            selectedNode = oSetupXML.SelectSingleNode("//Defaults").SelectSingleNode("UseWaterfallFormatter");
            g_aUseWaterfallFormater_DEF = new string[selectedNode.ChildNodes.Count];
            for (int i = 0; i < selectedNode.ChildNodes.Count; i++)
            {
                g_aUseWaterfallFormater_DEF[i] = selectedNode.ChildNodes[i].InnerText;
                Logging.LogMessage("LoadDefaults::UseWaterfallFormatter::" + selectedNode.ChildNodes[i].InnerText);
            }

            selectedNode = oSetupXML.SelectSingleNode("//Defaults").SelectSingleNode("FormatCheckPriority");
            g_aMethodOrder_DEF = new List<List<string>>();
            for (int i = 0; i < selectedNode.ChildNodes.Count; i++)
            { 
                g_aMethodOrder_DEF.Add(new List<string>());
                for (int j = 0; j < selectedNode.ChildNodes[i].ChildNodes.Count; j++)
                {
                    g_aMethodOrder_DEF[i].Add(selectedNode.ChildNodes[i].ChildNodes[j].InnerText);
                    Logging.LogMessage("LoadDefaults::FormatCheckPriority::" + selectedNode.ChildNodes[i].ChildNodes[j].InnerText);
                }
            }

            selectedNode = oSetupXML.SelectSingleNode("//Defaults").SelectSingleNode("UseDistanceRule");
            g_aUseDistanceRule_DEF = new string[selectedNode.ChildNodes.Count];
            for (int i = 0; i < selectedNode.ChildNodes.Count; i++)
            {
                g_aUseDistanceRule_DEF[i] = selectedNode.ChildNodes[i].InnerText;
                Logging.LogMessage("LoadDefaults::UseDistanceRule::" + selectedNode.ChildNodes[i].InnerText);
            }

            selectedNode = oSetupXML.SelectSingleNode("//Defaults").SelectSingleNode("DistanceRuleDays");
            g_aDistanceRuleDays_DEF = new string[selectedNode.ChildNodes.Count];
            for (int i = 0; i < selectedNode.ChildNodes.Count; i++)
            {
                g_aDistanceRuleDays_DEF[i] = selectedNode.ChildNodes[i].InnerText;
                Logging.LogMessage("LoadDefaults::DistanceRuleDays::" + selectedNode.ChildNodes[i].InnerText);
            }

            selectedNode = oSetupXML.SelectSingleNode("//Defaults").SelectSingleNode("DistanceRuleCanGoToFuture");
            g_aDistanceRuleFuture_DEF = new string[selectedNode.ChildNodes.Count];
            for (int i = 0; i < selectedNode.ChildNodes.Count; i++)
            {
                g_aDistanceRuleFuture_DEF[i] = selectedNode.ChildNodes[i].InnerText;
                Logging.LogMessage("LoadDefaults::DistanceRuleCanGoToFuture::" + selectedNode.ChildNodes[i].InnerText);
            }

        }

        public static void LoadBaseSettings(string XMLPath)
        {
            XmlDocument oSetupXML = new XmlDocument();
            oSetupXML.Load(XMLPath);

            string sXmlValue;
            try
            {
                sXmlValue = oSetupXML.SelectSingleNode("//WorkPath").InnerText;
                if (sXmlValue.Length != 0 && Directory.Exists(sXmlValue))
                {
                    g_sWorkingPath = sXmlValue;
                }
                else
                {
                    throw new Exception(String.Format("{0}({1}): The setupXML file was not found({2}) or the workingPath was not defined or found, {3}", AppSettings.App_Name(), AppSettings.App_Version(), XMLPath, sXmlValue));
                }

                sXmlValue = oSetupXML.SelectSingleNode("//Debug").InnerText;
                if (sXmlValue.Length == 0 || sXmlValue.ToLower().Contains("false"))
                {
                    g_bDebug = false;
                }
                else
                {
                    g_bDebug = true;
                }
                Logging.LogMessage("LoadXMLSettings::Debug, " + g_bDebug);
            }
            catch (Exception ex)
            {
                Logging.LogMessage("LoadXMLSettings::Error Loading XML Settings");
            }
        }

        public static void LoadXMLSettings(string XMLPath, string batchClassName)
        {
            string className = "//" + batchClassName;

            if(!XMLElementExists(XMLPath, batchClassName))
            {
                Logging.LogMessage("LoadXMLSettings::CreatingNewBatchClassNodes");
                CreateNewBatchClassNodes(XMLPath, batchClassName);
                return;
            }

            XmlDocument oSetupXML = new XmlDocument();
            oSetupXML.Load(XMLPath);

            string sXmlValue;

            bool bUsingFile = false;
            g_aMethodOrder = new List<string>();
            
            try
            {
                sXmlValue = oSetupXML.SelectSingleNode(className).SelectSingleNode("BatchFields").SelectSingleNode("ShortDateFormat").InnerText;
                if (sXmlValue.Length != 0)
                {
                    g_sShortDateFormat = sXmlValue;
                }
                else
                {
                    g_sShortDateFormat = g_aShortDateFormat_DEF[0];
                }
                Logging.LogMessage("LoadXMLSettings::ShortDateFormatBatchField, " + g_sShortDateFormat);

                sXmlValue = oSetupXML.SelectSingleNode(className).SelectSingleNode("BatchFields").SelectSingleNode("LongDateFormat").InnerText;
                if (sXmlValue.Length != 0)
                {
                    g_sLongDateFormat = sXmlValue;
                }
                else
                {
                    g_sLongDateFormat = g_aLongDateFormat_DEF[0];
                }
                Logging.LogMessage("LoadXMLSettings::LongDateFormatBatchField, " + g_sLongDateFormat);

                sXmlValue = oSetupXML.SelectSingleNode(className).SelectSingleNode("BatchFields").SelectSingleNode("ShortTimeFormat").InnerText;
                if (sXmlValue.Length != 0)
                {
                    g_sShortTimeFormat = sXmlValue;
                }
                else
                {
                    g_sShortTimeFormat = g_aShortTimeFormat_DEF[0];
                }
                Logging.LogMessage("LoadXMLSettings::ShortTimeFormatBatchField, " + g_sShortTimeFormat);

                sXmlValue = oSetupXML.SelectSingleNode(className).SelectSingleNode("BatchFields").SelectSingleNode("LongTimeFormat").InnerText;
                if (sXmlValue.Length != 0)
                {
                    g_sLongTimeFormat = sXmlValue;
                }
                else
                {
                    g_sLongTimeFormat = g_aLongTimeFormat_DEF[0];
                }
                Logging.LogMessage("LoadXMLSettings::LongTimeFormatBatchField, " + g_sLongTimeFormat);

                sXmlValue = oSetupXML.SelectSingleNode(className).SelectSingleNode("BatchFields").SelectSingleNode("LocalizationValue").InnerText;
                if (sXmlValue.Length != 0)
                {
                    g_sLocalizationValue = sXmlValue;
                }
                else
                {
                    g_sLocalizationValue = g_aLocalizationValue_DEF[0];
                }
                Logging.LogMessage("LoadXMLSettings::LocalizationValueBatchField, " + g_sLocalizationValue);

                sXmlValue = oSetupXML.SelectSingleNode(className).SelectSingleNode("BatchFields").SelectSingleNode("TimeStringToFormat").InnerText;
                if (sXmlValue.Length != 0)
                {
                    g_sTimeStringToFormat = sXmlValue;

                    if(File.Exists(g_sTimeStringToFormat))
                    {
                        bUsingFile = true;
                        XmlDocument batchFieldSetup = new XmlDocument();
                        batchFieldSetup.Load(g_sTimeStringToFormat);
                        XmlNode fields = batchFieldSetup.SelectSingleNode("Fields");
                        g_oStringToFormat = new BatchField[fields.ChildNodes.Count];
                        for (int i = 0; i < fields.ChildNodes.Count; i++)
                        {
                            g_oStringToFormat[i] = new BatchField(
                                fields.ChildNodes[i].SelectSingleNode("InName").InnerText,
                                fields.ChildNodes[i].SelectSingleNode("FormatDate").InnerText,
                                fields.ChildNodes[i].SelectSingleNode("FormatTime").InnerText,
                                fields.ChildNodes[i].SelectSingleNode("DateOutName").InnerText,
                                fields.ChildNodes[i].SelectSingleNode("TimeOutName").InnerText
                            );
                        }
                    }
                    else
                    {
                        bUsingFile = false;
                    }
                }
                else
                {
                    g_sTimeStringToFormat = "";
                }
                Logging.LogMessage("LoadXMLSettings::TimeStringToFormatBatchField, " + g_sTimeStringToFormat);

                sXmlValue = oSetupXML.SelectSingleNode(className).SelectSingleNode("BatchFields").SelectSingleNode("FormattedDate").InnerText;
                if (sXmlValue.Length != 0)
                {
                    g_sFormattedDate = sXmlValue;
                }
                else
                {
                    g_sFormattedDate = g_aFormattedDate_DEF[0];
                }
                Logging.LogMessage("LoadXMLSettings::FormattedDateBatchField, " + g_sFormattedDate);

                sXmlValue = oSetupXML.SelectSingleNode(className).SelectSingleNode("BatchFields").SelectSingleNode("FormattedTime").InnerText;
                if (sXmlValue.Length != 0)
                {
                    g_sFormattedTime = sXmlValue;
                }
                else
                {
                    g_sFormattedTime = g_aFormattedTime_DEF[0];
                }
                Logging.LogMessage("LoadXMLSettings::FormattedTimeBatchField, " + g_sFormattedTime);

                sXmlValue = oSetupXML.SelectSingleNode(className).SelectSingleNode("Formatter").SelectSingleNode("TargetDateFormat").InnerText;
                if (sXmlValue.Length != 0)
                {
                    g_sTargetDateFormat = sXmlValue;
                }
                else
                {
                    g_sTargetDateFormat = g_aTargetDateFormat_DEF[0];
                }
                Logging.LogMessage("LoadXMLSettings::TargetDateFormatBatchField, " + g_sTargetDateFormat);

                sXmlValue = oSetupXML.SelectSingleNode(className).SelectSingleNode("Formatter").SelectSingleNode("TargetTimeFormat").InnerText;
                if (sXmlValue.Length != 0)
                {
                    g_sTargetTimeFormat = sXmlValue;
                }
                else
                {
                    g_sTargetTimeFormat = g_aTargetTimeFormat_DEF[0];
                }
                Logging.LogMessage("LoadXMLSettings::TargetTimeFormatBatchField, " + g_sTargetTimeFormat);

                sXmlValue = oSetupXML.SelectSingleNode(className).SelectSingleNode("FormatterSettings").SelectSingleNode("UseWaterfallFormatter").InnerText;
                if (sXmlValue.Length != 0 || !sXmlValue.ToLower().Contains("false"))
                {
                    g_bUseWaterfallFormatter = true;
                }
                else
                {
                    g_bUseWaterfallFormatter = Boolean.Parse(g_aUseWaterfallFormater_DEF[0]);
                }
                Logging.LogMessage("LoadXMLSettings::UseWaterfallFormatter, " + g_bUseWaterfallFormatter);

                if (g_bUseWaterfallFormatter)
                {
                    sXmlValue = oSetupXML.SelectSingleNode(className).SelectSingleNode("FormatterSettings").SelectSingleNode("WaterfallSettings")
                                                                                 .SelectSingleNode("UseDistanceRule").InnerText;
                    if (sXmlValue.Length != 0 || !sXmlValue.ToLower().Contains("false"))
                    {
                        g_bUseDistanceRule = true;
                    }
                    else
                    {
                        g_bUseDistanceRule = Boolean.Parse(g_aUseDistanceRule_DEF[0]);
                    }
                    Logging.LogMessage("LoadXMLSettings::UseDistanceRule, " + g_bUseDistanceRule);

                    foreach (XmlNode formatOrder in oSetupXML.SelectSingleNode(className).SelectSingleNode("FormatterSettings").SelectSingleNode("WaterfallSettings").SelectSingleNode("FormatCheckPriority").ChildNodes)
                    {
                        sXmlValue = formatOrder.InnerText;
                        if (sXmlValue.Length != 0)
                        {
                            g_aMethodOrder.Add(sXmlValue);
                        }
                        Logging.LogMessage("LoadXMLSettings::NewFormatOrder, " + sXmlValue);
                    }

                    if (g_bUseDistanceRule)
                    {
                        sXmlValue = oSetupXML.SelectSingleNode(className).SelectSingleNode("FormatterSettings").SelectSingleNode("WaterfallSettings")
                                                                                     .SelectSingleNode("DistanceRuleSettings")
                                                                                     .SelectSingleNode("DistanceRuleDays").InnerText;
                        if (sXmlValue.Length != 0)
                        {
                            if (!Int32.TryParse(sXmlValue, out g_iDistanceRuleDays))
                            {
                                Logging.LogMessage("LoadXMLSettings::DistanceRuleDays is not a number, resetting value to 0");
                                g_iDistanceRuleDays = 0;
                            }
                        }
                        else
                        {
                            g_iDistanceRuleDays = Int32.Parse(g_aDistanceRuleDays_DEF[0]);
                        }
                        Logging.LogMessage("LoadXMLSettings::DistanceRuleDays, " + g_iDistanceRuleDays);

                        sXmlValue = oSetupXML.SelectSingleNode(className).SelectSingleNode("FormatterSettings").SelectSingleNode("WaterfallSettings")
                                                                                     .SelectSingleNode("DistanceRuleSettings")
                                                                                     .SelectSingleNode("DistanceRuleCanGoToFuture").InnerText;
                        if (sXmlValue.Length != 0 || !sXmlValue.ToLower().Contains("false"))
                        {
                            g_bDistanceRuleFuture = true;
                        }
                        else
                        {
                            g_bDistanceRuleFuture = Boolean.Parse(g_aDistanceRuleFuture_DEF[0]);
                        }
                        Logging.LogMessage("LoadXMLSettings::DistanceRuleFuture, " + g_bDistanceRuleFuture);
                    }
                }

                if (!bUsingFile)
                {
                    if (g_sTimeStringToFormat != "")
                    {
                        g_oStringToFormat = new BatchField[1];
                        g_oStringToFormat[0] = new BatchField(g_sTimeStringToFormat, g_sTargetDateFormat, g_sTargetTimeFormat, g_sFormattedDate, g_sFormattedTime);
                    }
                    else
                    {
                        g_oStringToFormat = new BatchField[1];
                        g_oStringToFormat[0] = new BatchField(g_aTimeStringToFormat_DEF[0], g_sTargetDateFormat, g_sTargetTimeFormat, g_sFormattedDate, g_sFormattedTime);
                    }
                }
                if (g_aMethodOrder.Count == 0)
                {
                    g_aMethodOrder = g_aMethodOrder_DEF[0];
                }
                Logging.LogMessage("LoadXMLSettings::Done Loading Settings");
            }
            catch (Exception ex)
            {
                Logging.LogMessage("LoadXMLSettings::Error Loading XML Settings");
            }
        }

        public static void SaveXMLSettings(string XMLPath, string batchClassName, List<string> formatcheckpriority,
                                           string shortdateformat = "", string longdateformat = "", string shorttimeformat = "", string longtimeformat = "",
                                           string localizationvalue = "", string timestringtoformat = "", string formatteddate = "", string formattedtime = "",
                                           string targetdateformat = "", string targettimeformat = "", string usewaterfallformatter = "", string usedistancerule = "",
                                           string distanceruledays = "", string distancerulefuture = "")
        {
            XmlDocument oSetupXML = new XmlDocument();
            oSetupXML.Load(XMLPath);

            string sXmlValue;

            string className = "//" + batchClassName;
            try
            {
                if (shortdateformat != "")
                {
                    oSetupXML.SelectSingleNode(className).SelectSingleNode("BatchFields").SelectSingleNode("ShortDateFormat").InnerText = shortdateformat;
                    Logging.LogMessage("SaveXMLSettings::ShortDateFormatBatchField, " + shortdateformat);
                }

                if (longdateformat != "")
                {
                    oSetupXML.SelectSingleNode(className).SelectSingleNode("BatchFields").SelectSingleNode("LongDateFormat").InnerText = longdateformat;
                    Logging.LogMessage("SaveXMLSettings::LongDateFormatBatchField, " + longdateformat);
                }

                if (shorttimeformat != "")
                {
                    oSetupXML.SelectSingleNode(className).SelectSingleNode("BatchFields").SelectSingleNode("ShortTimeFormat").InnerText = shorttimeformat;
                    Logging.LogMessage("SaveXMLSettings::ShortTimeFormatBatchField, " + shorttimeformat);
                }

                if (longtimeformat != "")
                {
                    oSetupXML.SelectSingleNode(className).SelectSingleNode("BatchFields").SelectSingleNode("LongTimeFormat").InnerText = longtimeformat;
                    Logging.LogMessage("SaveXMLSettings::LongTimeFormatBatchField, " + longtimeformat);
                }

                if (localizationvalue != "")
                {
                    oSetupXML.SelectSingleNode(className).SelectSingleNode("BatchFields").SelectSingleNode("LocalizationValue").InnerText = localizationvalue;
                    Logging.LogMessage("SaveXMLSettings::LocalizationValueBatchField, " + localizationvalue);
                }

                if (timestringtoformat != "")
                {
                    oSetupXML.SelectSingleNode(className).SelectSingleNode("BatchFields").SelectSingleNode("TimeStringToFormat").InnerText = timestringtoformat;
                    Logging.LogMessage("SaveXMLSettings::TimeStringToFormatBatchField, " + timestringtoformat);
                }

                if (formatteddate != "")
                {
                    oSetupXML.SelectSingleNode(className).SelectSingleNode("BatchFields").SelectSingleNode("FormattedDate").InnerText = formatteddate;
                    Logging.LogMessage("SaveXMLSettings::FormattedDateBatchField, " + formatteddate);
                }

                if (formattedtime != "")
                {
                    oSetupXML.SelectSingleNode(className).SelectSingleNode("BatchFields").SelectSingleNode("FormattedTime").InnerText = formattedtime;
                    Logging.LogMessage("SaveXMLSettings::FormattedTimeBatchField, " + formattedtime);
                }

                if (targetdateformat != "")
                {
                    oSetupXML.SelectSingleNode(className).SelectSingleNode("Formatter").SelectSingleNode("TargetDateFormat").InnerText = targetdateformat;
                    Logging.LogMessage("SaveXMLSettings::TargetDateFormatBatchField, " + targetdateformat);
                }

                if (targettimeformat != "")
                {
                    oSetupXML.SelectSingleNode(className).SelectSingleNode("Formatter").SelectSingleNode("TargetTimeFormat").InnerText = targettimeformat;
                    Logging.LogMessage("SaveXMLSettings::TargetTimeFormatBatchField, " + targettimeformat);
                }

                if (usewaterfallformatter != "")
                {
                    oSetupXML.SelectSingleNode(className).SelectSingleNode("FormatterSettings").SelectSingleNode("UseWaterfallFormatter").InnerText = usewaterfallformatter;
                    Logging.LogMessage("SaveXMLSettings::UseWaterfallFormatterBatchField, " + usewaterfallformatter);
                }

                if (formatcheckpriority != null || formatcheckpriority.Count != 0)
                {
                    oSetupXML.SelectSingleNode(className).SelectSingleNode("FormatterSettings").SelectSingleNode("WaterfallSettings").SelectSingleNode("FormatCheckPriority").RemoveAll();
                    foreach (string format in formatcheckpriority)
                    {
                        oSetupXML.SelectSingleNode(className).SelectSingleNode("FormatterSettings").SelectSingleNode("WaterfallSettings").SelectSingleNode("FormatCheckPriority").AppendChild(oSetupXML.CreateElement("Format"));
                        oSetupXML.SelectSingleNode(className).SelectSingleNode("FormatterSettings").SelectSingleNode("WaterfallSettings").SelectSingleNode("FormatCheckPriority").LastChild.InnerText = format;
                    }
                }

                if (usedistancerule != "")
                {
                    oSetupXML.SelectSingleNode(className).SelectSingleNode("FormatterSettings").SelectSingleNode("WaterfallSettings").SelectSingleNode("UseDistanceRule").InnerText = usedistancerule;
                    Logging.LogMessage("SaveXMLSettings::UseDistanceRuleBatchField, " + usedistancerule);
                }

                if (distanceruledays != "")
                {
                    oSetupXML.SelectSingleNode(className).SelectSingleNode("FormatterSettings").SelectSingleNode("WaterfallSettings").SelectSingleNode("DistanceRuleSettings").SelectSingleNode("DistanceRuleDays").InnerText = distanceruledays;
                    Logging.LogMessage("SaveXMLSettings::DistanceRuleBatchField, " + distanceruledays);
                }

                if (distancerulefuture != "")
                {
                    oSetupXML.SelectSingleNode(className).SelectSingleNode("FormatterSettings").SelectSingleNode("WaterfallSettings").SelectSingleNode("DistanceRuleSettings").SelectSingleNode("DistanceRuleCanGoToFuture").InnerText = distancerulefuture;
                    Logging.LogMessage("SaveXMLSettings::DistanceRuleCanGoToFutureBatchField, " + distancerulefuture);
                }

                oSetupXML.Save(XMLPath);
                Logging.LogMessage("SaveXMLSettings::Done Applying New Settings");
            }
            catch (Exception ex)
            {
                Logging.LogMessage("SaveXMLSettings::Error Applying New XML Settings, " + ex.Message);
            }
        }

        public static bool XMLElementExists(string XMLPath, string elementName)
        {
            XmlDocument xml = new XmlDocument();

            xml.Load(XMLPath);
            XmlNode node = xml.SelectSingleNode("//" + elementName);

            if(node == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static void CreateNewBatchClassNodes(string XMLPath, string batchClassName)
        {
            XmlDocument xml = new XmlDocument();
            string sXmlValue;

            xml.Load(XMLPath);

            g_aMethodOrder = new List<string>();

            XmlElement batchClass = xml.CreateElement(batchClassName);
            XmlElement batchFields = xml.CreateElement("BatchFields");
            XmlElement shortdateformat = xml.CreateElement("ShortDateFormat");
            shortdateformat.InnerText = g_aShortDateFormat_DEF[0];
            g_sShortDateFormat = g_aShortDateFormat_DEF[0];

            XmlElement longdateformat = xml.CreateElement("LongDateFormat");
            longdateformat.InnerText = g_aLongDateFormat_DEF[0];
            g_sLongDateFormat = g_aLongDateFormat_DEF[0];

            XmlElement shorttimeformat = xml.CreateElement("ShortTimeFormat");
            shorttimeformat.InnerText = g_aShortTimeFormat_DEF[0];
            g_sShortTimeFormat = g_aShortTimeFormat_DEF[0];

            XmlElement longtimeformat = xml.CreateElement("LongTimeFormat");
            longtimeformat.InnerText = g_aLongTimeFormat_DEF[0];
            g_sLongTimeFormat = g_aLongTimeFormat_DEF[0];

            XmlElement localizationvalue = xml.CreateElement("LocalizationValue");
            localizationvalue.InnerText = g_aLocalizationValue_DEF[0];
            g_sLocalizationValue = g_aLocalizationValue_DEF[0];

            XmlElement timestringtoformat = xml.CreateElement("TimeStringToFormat");
            timestringtoformat.InnerText = g_aTimeStringToFormat_DEF[0];
            g_sTimeStringToFormat = g_aTimeStringToFormat_DEF[0];

            XmlElement formatteddate = xml.CreateElement("FormattedDate");
            formatteddate.InnerText = g_aFormattedDate_DEF[0];
            g_sFormattedDate = g_aFormattedDate_DEF[0];

            XmlElement formattedtime = xml.CreateElement("FormattedTime");
            formattedtime.InnerText = g_aFormattedTime_DEF[0];
            g_sFormattedTime = g_aFormattedTime_DEF[0];

            XmlElement formatter = xml.CreateElement("Formatter");

            XmlElement targetdateformat = xml.CreateElement("TargetDateFormat");
            targetdateformat.InnerText = g_aTargetDateFormat_DEF[0];
            g_sTargetDateFormat = g_aTargetDateFormat_DEF[0];

            XmlElement targettimeformat = xml.CreateElement("TargetTimeFormat");
            targettimeformat.InnerText = g_aTargetTimeFormat_DEF[0];
            g_sTargetTimeFormat = g_aTargetTimeFormat_DEF[0];

            XmlElement formattersettings = xml.CreateElement("FormatterSettings");

            XmlElement usewaterfallformatter = xml.CreateElement("UseWaterfallFormatter");
            usewaterfallformatter.InnerText = g_aUseWaterfallFormater_DEF[0];
            Boolean.TryParse(g_aUseWaterfallFormater_DEF[0], out g_bUseWaterfallFormatter);

            XmlElement waterfallsettings = xml.CreateElement("WaterfallSettings");

            XmlElement usedistancerule = xml.CreateElement("UseDistanceRule");
            usedistancerule.InnerText = g_aUseDistanceRule_DEF[0];
            Boolean.TryParse(g_aUseDistanceRule_DEF[0], out g_bUseDistanceRule);

            XmlElement formatcheckpriority = xml.CreateElement("FormatCheckPriority");

            XmlElement format1 = xml.CreateElement("Format");
            format1.InnerText = g_aMethodOrder_DEF[0][0];
            g_aMethodOrder.Add(g_aMethodOrder_DEF[0][0]);
            formatcheckpriority.AppendChild(format1);
           
            if (g_aMethodOrder_DEF[0].Count > 1)
            {
                XmlElement format2 = xml.CreateElement("Format");
                format2.InnerText = g_aMethodOrder_DEF[0][1];
                g_aMethodOrder.Add(g_aMethodOrder_DEF[0][1]);
                formatcheckpriority.AppendChild(format2);
            }

            if (g_aMethodOrder_DEF[0].Count > 2)
            {
                XmlElement format3 = xml.CreateElement("Format");
                format3.InnerText = g_aMethodOrder_DEF[0][2];
                g_aMethodOrder.Add(g_aMethodOrder_DEF[0][2]);
                formatcheckpriority.AppendChild(format3);
            }

            if (g_aMethodOrder_DEF[0].Count > 3)
            {
                XmlElement format4 = xml.CreateElement("Format");
                format4.InnerText = g_aMethodOrder_DEF[0][3];
                g_aMethodOrder.Add(g_aMethodOrder_DEF[0][3]);
                formatcheckpriority.AppendChild(format4);
            }

            XmlElement distancerulesettings = xml.CreateElement("DistanceRuleSettings");

            XmlElement distanceruledays = xml.CreateElement("DistanceRuleDays");
            distanceruledays.InnerText = g_aDistanceRuleDays_DEF[0];
            Int32.TryParse(g_aDistanceRuleDays_DEF[0], out g_iDistanceRuleDays);

            XmlElement distancerulefuture = xml.CreateElement("DistanceRuleCanGoToFuture");
            distancerulefuture.InnerText = g_aDistanceRuleFuture_DEF[0];
            Boolean.TryParse(g_aDistanceRuleFuture_DEF[0], out g_bDistanceRuleFuture);

            batchFields.AppendChild(shortdateformat);
            batchFields.AppendChild(longdateformat);
            batchFields.AppendChild(shorttimeformat);
            batchFields.AppendChild(longtimeformat);
            batchFields.AppendChild(localizationvalue);
            batchFields.AppendChild(timestringtoformat);
            batchFields.AppendChild(formatteddate);
            batchFields.AppendChild(formattedtime);

            formatter.AppendChild(targetdateformat);
            formatter.AppendChild(targettimeformat);

            distancerulesettings.AppendChild(distanceruledays);
            distancerulesettings.AppendChild(distancerulefuture);

            

            waterfallsettings.AppendChild(formatcheckpriority);
            waterfallsettings.AppendChild(usedistancerule);
            waterfallsettings.AppendChild(distancerulesettings);

            formattersettings.AppendChild(usewaterfallformatter);
            formattersettings.AppendChild(waterfallsettings);

            batchClass.AppendChild(batchFields);
            batchClass.AppendChild(formatter);
            batchClass.AppendChild(formattersettings);

            xml.SelectSingleNode("Settings").AppendChild(batchClass);

            xml.Save(XMLPath);
        }
    }
}
