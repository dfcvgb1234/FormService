using Kofax.Capture.AdminModule.InteropServices;
using Kofax.TAPCommon.TAPControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FormService
{
    public partial class SetupForm : Form
    {
        public IBatchClass BatchClass;
        public bool state;
        string XMLPath;
        bool allowStateChange = false;

        List<string> g_aNewMethodOrder;
        List<TextBox> g_aTxtMethodOrder;
        TextBox g_oActiveTextBox;

        public SetupForm()
        {
            InitializeComponent();
        }

        public DialogResult ShowDialog(IBatchClass batchClass)
        {
            BatchClass = batchClass;
            // load previous settings
            return this.ShowDialog();
        }

        private void SetupForm_Load(object sender, EventArgs e)
        {
            txt_batchclass.Text = BatchClass.Name;
           
            XMLPath = AppSettings.App_Path() + AppSettings.App_Name() + "Setup.xml";
            XMLSettings.LoadBaseSettings(XMLPath);
            XMLSettings.LoadDefaults(XMLPath);
            XMLSettings.LoadXMLSettings(XMLPath, BatchClass.Name.Replace(" ", ""));

            Logging.LogMessage("SetupForm_Load::DoneLoadingXMLSettings");

            // Defaults
            cb_shortdateformat.Items.AddRange(XMLSettings.g_aShortDateFormat_DEF);
            cb_longdateformat.Items.AddRange(XMLSettings.g_aLongDateFormat_DEF);
            cb_shorttimeformat.Items.AddRange(XMLSettings.g_aShortTimeFormat_DEF);
            cb_longtimeformat.Items.AddRange(XMLSettings.g_aLongTimeFormat_DEF);
            cb_localizationvalue.Items.AddRange(XMLSettings.g_aLocalizationValue_DEF);
            cb_timestringtoformat.Items.AddRange(XMLSettings.g_aTimeStringToFormat_DEF);
            cb_formatteddate.Items.AddRange(XMLSettings.g_aFormattedDate_DEF);
            cb_formattedtime.Items.AddRange(XMLSettings.g_aFormattedTime_DEF);
            cb_targetdateformat.Items.AddRange(XMLSettings.g_aTargetDateFormat_DEF);
            cb_targettimeformat.Items.AddRange(XMLSettings.g_aTargetTimeFormat_DEF);
            cb_usewaterfallformatter.Items.AddRange(XMLSettings.g_aUseWaterfallFormater_DEF);
            cb_usedistancerule.Items.AddRange(XMLSettings.g_aUseDistanceRule_DEF);
            cb_distanceruledays.Items.AddRange(XMLSettings.g_aDistanceRuleDays_DEF);
            cb_distancerulefuture.Items.AddRange(XMLSettings.g_aDistanceRuleFuture_DEF);

            // Reset combobox selected index
            cb_shortdateformat.SelectedIndex = 0;
            cb_longdateformat.SelectedIndex = 0;
            cb_shorttimeformat.SelectedIndex = 0;
            cb_longtimeformat.SelectedIndex = 0;
            cb_localizationvalue.SelectedIndex = 0;
            cb_timestringtoformat.SelectedIndex = 0;
            cb_timestringtoformat.Items.Add("Browse ...");
            cb_formatteddate.SelectedIndex = 0;
            cb_formattedtime.SelectedIndex = 0;
            cb_targetdateformat.SelectedIndex = 0;
            cb_targettimeformat.SelectedIndex = 0;
            cb_usewaterfallformatter.SelectedIndex = 0;
            cb_usedistancerule.SelectedIndex = 0;
            cb_distanceruledays.SelectedIndex = 0;
            cb_distancerulefuture.SelectedIndex = 0;

            // Value Assignments
            txt_shortdateformat.Text = XMLSettings.g_sShortDateFormat;
            txt_longdateformat.Text = XMLSettings.g_sLongDateFormat;
            txt_shorttimeformat.Text = XMLSettings.g_sShortTimeFormat;
            txt_longtimeformat.Text = XMLSettings.g_sLongTimeFormat;
            txt_localizationvalue.Text = XMLSettings.g_sLocalizationValue;
            txt_stringtoformat.Text = XMLSettings.g_sTimeStringToFormat;
            txt_formatteddate.Text = XMLSettings.g_sFormattedDate;
            txt_formattedtime.Text = XMLSettings.g_sFormattedTime;

            // Settings
            txt_targetdateformat.Text = XMLSettings.g_sTargetDateFormat;
            txt_targettimeformat.Text = XMLSettings.g_sTargetTimeFormat;
            txt_usewaterfallformatter.Text = XMLSettings.g_bUseWaterfallFormatter.ToString();
            txt_usedistancerule.Text = XMLSettings.g_bUseDistanceRule.ToString();
            txt_ruledays.Text = XMLSettings.g_iDistanceRuleDays.ToString();
            txt_distancerulefuture.Text = XMLSettings.g_bDistanceRuleFuture.ToString();

            //Waterfall Check Priority
            txt_format1.Text = XMLSettings.g_aMethodOrder[0];
            if(XMLSettings.g_aMethodOrder.Count > 1)
            {
                txt_format2.Text = XMLSettings.g_aMethodOrder[1];
            }
            if (XMLSettings.g_aMethodOrder.Count > 2)
            {
                txt_format3.Text = XMLSettings.g_aMethodOrder[2];
            }
            if (XMLSettings.g_aMethodOrder.Count > 3)
            {
                txt_format4.Text = XMLSettings.g_aMethodOrder[3];
            }

            // Internal variable setup
            allowStateChange = true;
            g_aNewMethodOrder = XMLSettings.g_aMethodOrder;
            g_aTxtMethodOrder = new List<TextBox>();
            g_aTxtMethodOrder.Add(txt_format1);
            g_aTxtMethodOrder.Add(txt_format2);
            g_aTxtMethodOrder.Add(txt_format3);
            g_aTxtMethodOrder.Add(txt_format4);
        }

        private void ApplyState(bool state)
        {
            if (allowStateChange)
            {
                btn_apply.Enabled = state;
            }
        }

        private bool GetState()
        {
            return btn_apply.Enabled;
        }

        #region StateChange
        private void txt_shortdateformat_TextChanged(object sender, EventArgs e)
        {
            ApplyState(true);
        }

        private void txt_longdateformat_TextChanged(object sender, EventArgs e)
        {
            ApplyState(true);
        }

        private void txt_shorttimeformat_TextChanged(object sender, EventArgs e)
        {
            ApplyState(true);
        }

        private void txt_longtimeformat_TextChanged(object sender, EventArgs e)
        {
            ApplyState(true);
        }

        private void txt_localizationvalue_TextChanged(object sender, EventArgs e)
        {
            ApplyState(true);
        }

        private void txt_stringtoformat_TextChanged(object sender, EventArgs e)
        {
            ApplyState(true);
        }

        private void txt_formatteddate_TextChanged(object sender, EventArgs e)
        {
            ApplyState(true);
        }

        private void txt_formattedtime_TextChanged(object sender, EventArgs e)
        {
            ApplyState(true);
        }

        private void txt_targetdateformat_TextChanged(object sender, EventArgs e)
        {
            ApplyState(true);
        }

        private void txt_targettimeformat_TextChanged(object sender, EventArgs e)
        {
            ApplyState(true);
        }

        private void txt_usewaterfallformatter_TextChanged(object sender, EventArgs e)
        {
            ApplyState(true);
        }

        private void txt_usedistancerule_TextChanged(object sender, EventArgs e)
        {
            ApplyState(true);
        }

        private void txt_ruledays_ValueChanged(object sender, EventArgs e)
        {
            ApplyState(true);
        }

        private void txt_distancerulefuture_TextChanged(object sender, EventArgs e)
        {
            ApplyState(true);
        }
        #endregion

        private void btn_apply_Click(object sender, EventArgs e)
        {
            string message = "";
            bool validated = ValidateFields(out message);
            if (message != "" && validated == false)
            {
                MessageBox.Show(message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if(message != "" && validated == true)
            {
                MessageBox.Show(message, "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                if (!XMLSettings.XMLBatchClassElementExists(XMLPath, BatchClass.Name.Replace(" ", "")))
                {
                    XMLSettings.CreateNewBatchClassNodes(XMLPath, BatchClass.Name.Replace(" ", ""));
                }

                XMLSettings.SaveXMLSettings(XMLPath, BatchClass.Name.Replace(" ", ""), g_aNewMethodOrder,
                                            txt_shortdateformat.Text, txt_longdateformat.Text, txt_shorttimeformat.Text,
                                            txt_longtimeformat.Text, txt_localizationvalue.Text, txt_stringtoformat.Text,
                                            txt_formatteddate.Text, txt_formattedtime.Text, txt_targetdateformat.Text,
                                            txt_targettimeformat.Text, txt_usewaterfallformatter.Text, txt_usedistancerule.Text,
                                            txt_ruledays.Text, txt_distancerulefuture.Text);
                ApplyState(false);
            }
            else
            {
                if (!XMLSettings.XMLBatchClassElementExists(XMLPath, BatchClass.Name.Replace(" ", "")))
                {
                    XMLSettings.CreateNewBatchClassNodes(XMLPath, BatchClass.Name.Replace(" ", ""));
                }

                XMLSettings.SaveXMLSettings(XMLPath, BatchClass.Name.Replace(" ", ""), g_aNewMethodOrder,
                                            txt_shortdateformat.Text, txt_longdateformat.Text, txt_shorttimeformat.Text,
                                            txt_longtimeformat.Text, txt_localizationvalue.Text, txt_stringtoformat.Text,
                                            txt_formatteddate.Text, txt_formattedtime.Text, txt_targetdateformat.Text,
                                            txt_targettimeformat.Text, txt_usewaterfallformatter.Text, txt_usedistancerule.Text,
                                            txt_ruledays.Text, txt_distancerulefuture.Text);
                ApplyState(false);
            }                                                                          
        }                                                                              
                                                                                       
        private bool ValidateFields(out string message)
        {                                                                              
            message = "";                                                              
            bool temp = false;                                                         
            int iTemp = 0;

            if(!Boolean.TryParse(txt_usewaterfallformatter.Text, out temp))
            {
                message = "The field: UseWaterfallFormatter is not of type: boolean, " + txt_usewaterfallformatter.Text;
                return false;
            }
            if(!Boolean.TryParse(txt_usedistancerule.Text, out temp))
            {
                message = "The field: UseDistanceRule is not of type: boolean, " + txt_usedistancerule.Text;
                return false;
            }
            if(!Int32.TryParse(txt_ruledays.Value.ToString(), out iTemp))
            {
                message = "The field: DistanceRuleDays is not of type: Integer, " + txt_ruledays.Text;
                return false;
            }
            if (!Boolean.TryParse(txt_distancerulefuture.Text, out temp))
            {
                message = "The field: DistanceRuleCanGoToFuture is not of type: boolean, " + txt_distancerulefuture.Text;
                return false;
            }

            if(String.IsNullOrWhiteSpace(txt_shortdateformat.Text))
            {
                message = "You have empty fields, this is not recommended, but default values will be used";
                return true;
            }
            else if (String.IsNullOrWhiteSpace(txt_longdateformat.Text))
            {
                message = "You have empty fields, this is not recommended, but default values will be used";
                return true;
            }
            else if (String.IsNullOrWhiteSpace(txt_shorttimeformat.Text))
            {
                message = "You have empty fields, this is not recommended, but default values will be used";
                return true;
            }
            else if (String.IsNullOrWhiteSpace(txt_longtimeformat.Text))
            {
                message = "You have empty fields, this is not recommended, but default values will be used";
                return true;
            }
            else if (String.IsNullOrWhiteSpace(txt_localizationvalue.Text))
            {
                message = "You have empty fields, this is not recommended, but default values will be used";
                return true;
            }
            else if (String.IsNullOrWhiteSpace(txt_stringtoformat.Text))
            {
                message = "You have empty fields, this is not recommended, but default values will be used";
                return true;
            }
            else if (String.IsNullOrWhiteSpace(txt_formatteddate.Text))
            {
                message = "You have empty fields, this is not recommended, but default values will be used";
                return true;
            }
            else if (String.IsNullOrWhiteSpace(txt_formattedtime.Text))
            {
                message = "You have empty fields, this is not recommended, but default values will be used";
                return true;
            }
            else if (String.IsNullOrWhiteSpace(txt_targetdateformat.Text))
            {
                message = "You have empty fields, this is not recommended, but default values will be used";
                return true;
            }
            else if (String.IsNullOrWhiteSpace(txt_targettimeformat.Text))
            {
                message = "You have empty fields, this is not recommended, but default values will be used";
                return true;
            }
            else if (String.IsNullOrWhiteSpace(txt_usewaterfallformatter.Text))
            {
                message = "You have empty fields, this is not recommended, but default values will be used";
                return true;
            }
            else if (String.IsNullOrWhiteSpace(txt_usedistancerule.Text))
            {
                message = "You have empty fields, this is not recommended, but default values will be used";
                return true;
            }
            else if (String.IsNullOrWhiteSpace(txt_ruledays.Text))
            {
                message = "You have empty fields, this is not recommended, but default values will be used";
                return true;
            }
            else if (String.IsNullOrWhiteSpace(txt_distancerulefuture.Text))
            {
                message = "You have empty fields, this is not recommended, but default values will be used";
                return true;
            }
            return true;
        }

        #region AutoCompleteDefaults

        private void cb_shortdateformat_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox oBox = (ComboBox)sender;
            txt_shortdateformat.Text = oBox.SelectedItem.ToString();
        }

        private void cb_longdateformat_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox oBox = (ComboBox)sender;
            txt_longdateformat.Text = oBox.SelectedItem.ToString();
        }

        private void cb_shorttimeformat_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox oBox = (ComboBox)sender;
            txt_shorttimeformat.Text = oBox.SelectedItem.ToString();
        }

        private void cb_longtimeformat_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox oBox = (ComboBox)sender;
            txt_longtimeformat.Text = oBox.SelectedItem.ToString();
        }

        private void cb_localizationvalue_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox oBox = (ComboBox)sender;
            txt_localizationvalue.Text = oBox.SelectedItem.ToString();
        }

        private void cb_timestringtoformat_SelectedIndexChanged(object sender, EventArgs e)
        {

            ComboBox oBox = (ComboBox)sender;
            if (oBox.SelectedIndex == oBox.Items.Count - 1)
            {
                ofd_timestringDiag.InitialDirectory = AppSettings.App_Path();
                ofd_timestringDiag.ShowDialog();
                if(!String.IsNullOrWhiteSpace(ofd_timestringDiag.FileName))
                {
                    txt_stringtoformat.Text = ofd_timestringDiag.FileName;
                }
                else
                {
                    oBox.SelectedIndex = 0;
                    txt_stringtoformat.Text = oBox.SelectedItem.ToString();
                }
            }
            else
            {
                txt_stringtoformat.Text = oBox.SelectedItem.ToString();
            }
            
        }

        private void cb_formatteddate_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox oBox = (ComboBox)sender;
            txt_formatteddate.Text = oBox.SelectedItem.ToString();
        }

        private void cb_formattedtime_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox oBox = (ComboBox)sender;
            txt_formattedtime.Text = oBox.SelectedItem.ToString();
        }

        private void cb_targetdateformat_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox oBox = (ComboBox)sender;
            txt_targetdateformat.Text = oBox.SelectedItem.ToString();
        }

        private void cb_targettimeformat_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox oBox = (ComboBox)sender;
            txt_targettimeformat.Text = oBox.SelectedItem.ToString();
        }

        private void cb_usewaterfallformatter_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox oBox = (ComboBox)sender;
            txt_usewaterfallformatter.Text = oBox.SelectedItem.ToString();
        }

        private void cb_usedistancerule_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox oBox = (ComboBox)sender;
            txt_usedistancerule.Text = oBox.SelectedItem.ToString();
        }

        private void cb_distanceruledays_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox oBox = (ComboBox)sender;
            txt_ruledays.Text = oBox.SelectedItem.ToString();
        }

        private void cb_distancerulefuture_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox oBox = (ComboBox)sender;
            txt_distancerulefuture.Text = oBox.SelectedItem.ToString();
        }

        #endregion

        private void ChangeSelectedTextBox(TextBox sender)
        {
            txt_format1.BackColor = Color.White;
            txt_format1.ForeColor = Color.Black;

            txt_format2.BackColor = Color.White;
            txt_format2.ForeColor = Color.Black;

            txt_format3.BackColor = Color.White;
            txt_format3.ForeColor = Color.Black;

            txt_format4.BackColor = Color.White;
            txt_format4.ForeColor = Color.Black;

            sender.BackColor = Color.Black;
            sender.ForeColor = Color.White;
            g_oActiveTextBox = sender;
        }

        private void ApplyCorrectTextboxOrder(List<string> newOrder)
        {
            txt_format1.Text = newOrder[0];

            if(newOrder.Count > 1)
            {
                txt_format2.Text = newOrder[1];
            }
            if (newOrder.Count > 2)
            {
                txt_format3.Text = newOrder[2];
            }
            if (newOrder.Count > 3)
            {
                txt_format4.Text = newOrder[3];
            }
        }

        private void txt_format1_Click(object sender, EventArgs e)
        {
            ChangeSelectedTextBox(sender as TextBox);
        }

        private void txt_format2_Click(object sender, EventArgs e)
        {
            ChangeSelectedTextBox(sender as TextBox);
        }

        private void txt_format3_Click(object sender, EventArgs e)
        {
            ChangeSelectedTextBox(sender as TextBox);
        }

        private void txt_format4_Click(object sender, EventArgs e)
        {
            ChangeSelectedTextBox(sender as TextBox);
        }

        private void btn_txtListUP_Click(object sender, EventArgs e)
        {
            if(g_oActiveTextBox != null)
            {
                int iActiveIndex = g_aNewMethodOrder.IndexOf(g_oActiveTextBox.Text);
                if (iActiveIndex != 0)
                {
                    string sTemp = g_aNewMethodOrder[iActiveIndex];
                    g_aNewMethodOrder[iActiveIndex] = g_aNewMethodOrder[iActiveIndex - 1];
                    g_aNewMethodOrder[iActiveIndex - 1] = sTemp;
                    ApplyCorrectTextboxOrder(g_aNewMethodOrder);
                    ChangeSelectedTextBox(g_aTxtMethodOrder[iActiveIndex - 1]);
                }
            }
            else
            {
                return;
            }
        }

        private void btn_txtListDOWN_Click(object sender, EventArgs e)
        {
            if (g_oActiveTextBox != null)
            {
                int iActiveIndex = g_aNewMethodOrder.IndexOf(g_oActiveTextBox.Text);
                if (iActiveIndex != g_aNewMethodOrder.Count-1)
                {
                    string sTemp = g_aNewMethodOrder[iActiveIndex];
                    g_aNewMethodOrder[iActiveIndex] = g_aNewMethodOrder[iActiveIndex + 1];
                    g_aNewMethodOrder[iActiveIndex + 1] = sTemp;
                    ApplyCorrectTextboxOrder(g_aNewMethodOrder);
                    ChangeSelectedTextBox(g_aTxtMethodOrder[iActiveIndex + 1]);
                }
            }
            else
            {
                return;
            }
        }

        private void txt_format1_TextChanged(object sender, EventArgs e)
        {
            ApplyState(true);
        }

        private void txt_format2_TextChanged(object sender, EventArgs e)
        {
            ApplyState(true);
        }

        private void txt_format3_TextChanged(object sender, EventArgs e)
        {
            ApplyState(true);
        }

        private void txt_format4_TextChanged(object sender, EventArgs e)
        {
            ApplyState(true);
        }

        private void bnt_OK_Click(object sender, EventArgs e)
        {
            if(GetState())
            {
                btn_apply.PerformClick();
            }
            Environment.Exit(0);
        }

        private void btn_CANCEL_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void btn_verify_Click(object sender, EventArgs e)
        {
            if(XMLSettings.VerifyXMLStructure(XMLPath, BatchClass.Name.Replace(" ", ""), out string message))
            {
                MessageBox.Show(message, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                DialogResult result = MessageBox.Show(message + "\n\nDo you want to reset all values to default?", "ERROR", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                
                if(result == DialogResult.Yes)
                {
                    result = MessageBox.Show("Are you sure you want to reset everything back to default?\n\nAll data will be wiped!", "WARNING", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    
                    if(result == DialogResult.Yes)
                    {
                        XMLSettings.ResetBatchClassValuesToDefault(XMLPath, BatchClass.Name.Replace(" ", ""));
                        UpdateUI();
                        ApplyState(false);
                    }
                }
            }
        }

        private void UpdateUI()
        {
            txt_batchclass.Text = BatchClass.Name;

            XMLPath = AppSettings.App_Path() + AppSettings.App_Name() + "Setup.xml";
            XMLSettings.LoadBaseSettings(XMLPath);
            XMLSettings.LoadDefaults(XMLPath);
            XMLSettings.LoadXMLSettings(XMLPath, BatchClass.Name.Replace(" ", ""));

            Logging.LogMessage("UpdateUI::DoneLoadingXMLSettings");

            // Defaults
            cb_shortdateformat.Items.AddRange(XMLSettings.g_aShortDateFormat_DEF);
            cb_longdateformat.Items.AddRange(XMLSettings.g_aLongDateFormat_DEF);
            cb_shorttimeformat.Items.AddRange(XMLSettings.g_aShortTimeFormat_DEF);
            cb_longtimeformat.Items.AddRange(XMLSettings.g_aLongTimeFormat_DEF);
            cb_localizationvalue.Items.AddRange(XMLSettings.g_aLocalizationValue_DEF);
            cb_timestringtoformat.Items.AddRange(XMLSettings.g_aTimeStringToFormat_DEF);
            cb_formatteddate.Items.AddRange(XMLSettings.g_aFormattedDate_DEF);
            cb_formattedtime.Items.AddRange(XMLSettings.g_aFormattedTime_DEF);
            cb_targetdateformat.Items.AddRange(XMLSettings.g_aTargetDateFormat_DEF);
            cb_targettimeformat.Items.AddRange(XMLSettings.g_aTargetTimeFormat_DEF);
            cb_usewaterfallformatter.Items.AddRange(XMLSettings.g_aUseWaterfallFormater_DEF);
            cb_usedistancerule.Items.AddRange(XMLSettings.g_aUseDistanceRule_DEF);
            cb_distanceruledays.Items.AddRange(XMLSettings.g_aDistanceRuleDays_DEF);
            cb_distancerulefuture.Items.AddRange(XMLSettings.g_aDistanceRuleFuture_DEF);

            // Reset combobox selected index
            cb_shortdateformat.SelectedIndex = 0;
            cb_longdateformat.SelectedIndex = 0;
            cb_shorttimeformat.SelectedIndex = 0;
            cb_longtimeformat.SelectedIndex = 0;
            cb_localizationvalue.SelectedIndex = 0;
            cb_timestringtoformat.SelectedIndex = 0;
            cb_timestringtoformat.Items.Add("Browse ...");
            cb_formatteddate.SelectedIndex = 0;
            cb_formattedtime.SelectedIndex = 0;
            cb_targetdateformat.SelectedIndex = 0;
            cb_targettimeformat.SelectedIndex = 0;
            cb_usewaterfallformatter.SelectedIndex = 0;
            cb_usedistancerule.SelectedIndex = 0;
            cb_distanceruledays.SelectedIndex = 0;
            cb_distancerulefuture.SelectedIndex = 0;

            // Value Assignments
            txt_shortdateformat.Text = XMLSettings.g_sShortDateFormat;
            txt_longdateformat.Text = XMLSettings.g_sLongDateFormat;
            txt_shorttimeformat.Text = XMLSettings.g_sShortTimeFormat;
            txt_longtimeformat.Text = XMLSettings.g_sLongTimeFormat;
            txt_localizationvalue.Text = XMLSettings.g_sLocalizationValue;
            txt_stringtoformat.Text = XMLSettings.g_sTimeStringToFormat;
            txt_formatteddate.Text = XMLSettings.g_sFormattedDate;
            txt_formattedtime.Text = XMLSettings.g_sFormattedTime;

            // Settings
            txt_targetdateformat.Text = XMLSettings.g_sTargetDateFormat;
            txt_targettimeformat.Text = XMLSettings.g_sTargetTimeFormat;
            txt_usewaterfallformatter.Text = XMLSettings.g_bUseWaterfallFormatter.ToString();
            txt_usedistancerule.Text = XMLSettings.g_bUseDistanceRule.ToString();
            txt_ruledays.Text = XMLSettings.g_iDistanceRuleDays.ToString();
            txt_distancerulefuture.Text = XMLSettings.g_bDistanceRuleFuture.ToString();

            //Waterfall Check Priority
            txt_format1.Text = XMLSettings.g_aMethodOrder[0];
            if (XMLSettings.g_aMethodOrder.Count > 1)
            {
                txt_format2.Text = XMLSettings.g_aMethodOrder[1];
            }
            if (XMLSettings.g_aMethodOrder.Count > 2)
            {
                txt_format3.Text = XMLSettings.g_aMethodOrder[2];
            }
            if (XMLSettings.g_aMethodOrder.Count > 3)
            {
                txt_format4.Text = XMLSettings.g_aMethodOrder[3];
            }

            // Internal variable setup
            g_aNewMethodOrder = XMLSettings.g_aMethodOrder;
            g_aTxtMethodOrder = new List<TextBox>();
            g_aTxtMethodOrder.Add(txt_format1);
            g_aTxtMethodOrder.Add(txt_format2);
            g_aTxtMethodOrder.Add(txt_format3);
            g_aTxtMethodOrder.Add(txt_format4);

            Logging.LogMessage("UpdateUI::FinishedUpdating");
        }
    }
}
