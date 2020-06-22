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

        BatchManager g_oBatman;

        public batchService()
        {
            g_oBatman = new BatchManager();

            timer.Interval = 60000;
            timer.Elapsed += Timer_Elapsed; ;
            timer.Enabled = true;

            try
            {
                string sSetupXMLPath = AppSettings.App_Path() + AppSettings.App_Name() + "Setup.xml";
                Logging.LogMessage("Constructor::SetupXMLPath, " + sSetupXMLPath);
                Logging.WriteToEventLog("Constructor::SetupXMLPath, " + sSetupXMLPath);
                XMLSettings.LoadBaseSettings(sSetupXMLPath);
                XMLSettings.LoadDefaults(sSetupXMLPath);
            }
            catch (Exception ex)
            {
                Logging.WriteToEventLog(String.Format("Exception: {0}, occured in {1}: {2}", ex.InnerException, ex.TargetSite, ex.Message), "batchService", EventLogEntryType.Error);
                // Unable to write to log when the error can not find the workingPath
                //LogMessage(String.Format("Exception: {0}, occured in {1}: {2}", ex.InnerException, ex.TargetSite, ex.Message));
            }
            InitializeComponent();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Logging.WriteToEventLog("Heartbeat at: " + DateTime.Now.ToString("HH:mm:ss"));
        }

        protected override void OnStart(string[] args)
        {
            timer.Start();
            Logging.LogMessage("OnStart::Start");
            Logging.WriteToEventLog("OnStart::Start");

            string sUserProfileSetupPath = AppSettings.App_Path() + "CustomModuleUserProfileSetup.xml";
            Logging.LogMessage("OnStart::UserProfilePath, " + sUserProfileSetupPath);
            Logging.WriteToEventLog("OnStart::UserProfilePath, " + sUserProfileSetupPath);
            XMLSettings.LoadUserSettings(sUserProfileSetupPath);

            Logging.LogMessage("OnStart::LoginToRuntime");
            Logging.WriteToEventLog("OnStart::LoginToRuntime");
            g_oBatman.LoginToRuntime();
        }

        protected override void OnStop()
        {
            Logging.LogMessage("OnStop::ProgramClose");
            g_oBatman.CloseBatch();
            timer.Stop();
        }

        
    }
}
