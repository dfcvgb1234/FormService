using Kofax.Capture.DBLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace FormService
{
    public partial class Main : Form
    {
        BatchManager g_oBatman;

        public Main()
        {
            g_oBatman = new BatchManager();
            try
            {
                string sSetupXMLPath = AppSettings.App_Path() + AppSettings.App_Name() + "Setup.xml";
                Logging.LogMessage("OnLoad::SetupXMLPath, " + sSetupXMLPath);
                XMLSettings.LoadXMLSettings(sSetupXMLPath);

                g_oBatman.NewBatchProcess += G_oBatman_NewBatchProcess;
            }
            catch(Exception ex)
            {
                // Unable to write to log when the error can not find the workingPath
                Logging.LogMessage(String.Format("Exception: {0}, occured in {1}: {2}", ex.InnerException, ex.TargetSite, ex.Message));
            }
            InitializeComponent();
        }

        private void G_oBatman_NewBatchProcess(object sender, Batch batch)
        {
            txt_BatchName.Text = batch.Name;
        }

        private void main_Load(object sender, EventArgs e)
        {
            txt_Action.Text = Logging.LogMessage("OnLoad::Start");

            string sUserProfileSetupPath = AppSettings.App_Path() + "CustomModuleUserProfileSetup.xml";
            txt_Action.Text = Logging.LogMessage("OnLoad::UserProfilePath, " + sUserProfileSetupPath);
            XMLSettings.LoadUserSettings(sUserProfileSetupPath);

            txt_Action.Text = Logging.LogMessage("OnLoad::LoginToRuntime");
            g_oBatman.LoginToRuntime();
            txt_Action.Text = Logging.LogMessage("OnLoad::ProcessesingBatch");
        }

        private void main_FormClosing(object sender, FormClosingEventArgs e)
        {
            txt_Action.Text = Logging.LogMessage("Main_FormClosing::ProgramClose");
            g_oBatman.CloseBatch();
        }
    }
}
