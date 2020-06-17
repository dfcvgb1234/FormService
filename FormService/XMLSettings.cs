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

        public static void LoadXMLSettings(string XMLPath)
        {
            try
            {
                XmlDocument oSetupXML = new XmlDocument();
                oSetupXML.Load(XMLPath);

                string sXmlValue;

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

                sXmlValue = oSetupXML.SelectSingleNode("//BatchFields").SelectSingleNode("ShortDateFormat").InnerText;
                if (sXmlValue.Length != 0)
                {
                    g_sShortDateFormat = sXmlValue;
                }
                else
                {
                    g_sShortDateFormat = "ShortDateFormat";
                }
                Logging.LogMessage("LoadXMLSettings::ShortDateFormatBatchField, " + g_sShortDateFormat);

                sXmlValue = oSetupXML.SelectSingleNode("//BatchFields").SelectSingleNode("LongDateFormat").InnerText;
                if (sXmlValue.Length != 0)
                {
                    g_sLongDateFormat = sXmlValue;
                }
                else
                {
                    g_sLongDateFormat = "LongDateFormat";
                }
                Logging.LogMessage("LoadXMLSettings::LongDateFormatBatchField, " + g_sLongDateFormat);

                sXmlValue = oSetupXML.SelectSingleNode("//BatchFields").SelectSingleNode("ShortTimeFormat").InnerText;
                if (sXmlValue.Length != 0)
                {
                    g_sShortTimeFormat = sXmlValue;
                }
                else
                {
                    g_sShortTimeFormat = "ShortTimeFormat";
                }
                Logging.LogMessage("LoadXMLSettings::ShortTimeFormatBatchField, " + g_sShortTimeFormat);

                sXmlValue = oSetupXML.SelectSingleNode("//BatchFields").SelectSingleNode("LongTimeFormat").InnerText;
                if (sXmlValue.Length != 0)
                {
                    g_sLongTimeFormat = sXmlValue;
                }
                else
                {
                    g_sLongTimeFormat = "LongTimeFormat";
                }
                Logging.LogMessage("LoadXMLSettings::LongTimeFormatBatchField, " + g_sLongTimeFormat);

                sXmlValue = oSetupXML.SelectSingleNode("//BatchFields").SelectSingleNode("LocalizationValue").InnerText;
                if (sXmlValue.Length != 0)
                {
                    g_sLocalizationValue = sXmlValue;
                }
                else
                {
                    g_sLocalizationValue = "LocalizationValue";
                }
                Logging.LogMessage("LoadXMLSettings::LocalizationValueBatchField, " + g_sLocalizationValue);

                sXmlValue = oSetupXML.SelectSingleNode("//BatchFields").SelectSingleNode("TimeStringToFormat").InnerText;
                if (sXmlValue.Length != 0)
                {
                    g_sTimeStringToFormat = sXmlValue;
                }
                else
                {
                    g_sTimeStringToFormat = "TimeStringToFormat";
                }
                Logging.LogMessage("LoadXMLSettings::TimeStringToFormatBatchField, " + g_sTimeStringToFormat);

                sXmlValue = oSetupXML.SelectSingleNode("//BatchFields").SelectSingleNode("FormattedDate").InnerText;
                if (sXmlValue.Length != 0)
                {
                    g_sFormattedDate = sXmlValue;
                }
                else
                {
                    g_sFormattedDate = "FormattedDate";
                }
                Logging.LogMessage("LoadXMLSettings::FormattedDateBatchField, " + g_sFormattedDate);

                sXmlValue = oSetupXML.SelectSingleNode("//BatchFields").SelectSingleNode("FormattedTime").InnerText;
                if (sXmlValue.Length != 0)
                {
                    g_sFormattedTime = sXmlValue;
                }
                else
                {
                    g_sFormattedTime = "FormattedTime";
                }
                Logging.LogMessage("LoadXMLSettings::FormattedTimeBatchField, " + g_sFormattedTime);

                sXmlValue = oSetupXML.SelectSingleNode("//Formatter").SelectSingleNode("TargetDateFormat").InnerText;
                if (sXmlValue.Length != 0)
                {
                    g_sTargetDateFormat = sXmlValue;
                }
                else
                {
                    g_sTargetDateFormat = "TargetDateFormat";
                }
                Logging.LogMessage("LoadXMLSettings::TargetDateFormatBatchField, " + g_sTargetDateFormat);

                sXmlValue = oSetupXML.SelectSingleNode("//Formatter").SelectSingleNode("TargetTimeFormat").InnerText;
                if (sXmlValue.Length != 0)
                {
                    g_sTargetTimeFormat = sXmlValue;
                }
                else
                {
                    g_sTargetTimeFormat = "TargetTimeFormat";
                }
                Logging.LogMessage("LoadXMLSettings::TargetTimeFormatBatchField, " + g_sTargetTimeFormat);

                sXmlValue = oSetupXML.SelectSingleNode("//FormatterSettings").SelectSingleNode("UseWaterfallFormatter").InnerText;
                if (sXmlValue.Length != 0 || !sXmlValue.ToLower().Contains("false"))
                {
                    g_bUseWaterfallFormatter = true;
                }
                else
                {
                    g_bUseWaterfallFormatter = false;
                }
                Logging.LogMessage("LoadXMLSettings::UseWaterfallFormatter, " + g_bUseWaterfallFormatter);

                if (g_bUseWaterfallFormatter)
                {
                    sXmlValue = oSetupXML.SelectSingleNode("//FormatterSettings").SelectSingleNode("WaterfallSettings")
                                                                                 .SelectSingleNode("UseDistanceRule").InnerText;
                    if (sXmlValue.Length != 0 || !sXmlValue.ToLower().Contains("false"))
                    {
                        g_bUseDistanceRule = true;
                    }
                    else
                    {
                        g_bUseDistanceRule = false;
                    }
                    Logging.LogMessage("LoadXMLSettings::UseDistanceRule, " + g_bUseDistanceRule);

                    if (g_bUseDistanceRule)
                    {
                        sXmlValue = oSetupXML.SelectSingleNode("//FormatterSettings").SelectSingleNode("WaterfallSettings")
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
                            g_iDistanceRuleDays = 0;
                        }
                        Logging.LogMessage("LoadXMLSettings::DistanceRuleDays, " + g_iDistanceRuleDays);

                        sXmlValue = oSetupXML.SelectSingleNode("//FormatterSettings").SelectSingleNode("WaterfallSettings")
                                                                                     .SelectSingleNode("DistanceRuleSettings")
                                                                                     .SelectSingleNode("DistanceRuleCanGoToFuture").InnerText;
                        if (sXmlValue.Length != 0 || !sXmlValue.ToLower().Contains("false"))
                        {
                            g_bDistanceRuleFuture = true;
                        }
                        else
                        {
                            g_bDistanceRuleFuture = false;
                        }
                        Logging.LogMessage("LoadXMLSettings::DistanceRuleFuture, " + g_bDistanceRuleFuture);
                    }
                }

                Logging.LogMessage("LoadXMLSettings::Done Loading Settings");
            }
            catch (Exception ex)
            {
                Logging.LogMessage("LoadXMLSettings::Error Loading XML Settings");
            }
        }
    }
}
