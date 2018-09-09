using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;

namespace TimePunch
{
    public partial class frmDateTimePicker : Form
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public DateTime chosenDate = DateTime.UtcNow;
        public bool Cancelled = true;

        public frmDateTimePicker()
        {
            InitializeComponent();
        }
        private string convertUnixDateTimeToDisplayTime(string unixDateTime)
        {
            log.Debug("IN");

            // Unix timestamp is seconds past epoch
            double unix = double.Parse(unixDateTime);
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unix).ToLocalTime();

            string result = dtDateTime.ToShortTimeString();

            return result;

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Cancelled = true;
            this.Close();
        }

        private void frmDateTimePicker_Load(object sender, EventArgs e)
        {
            log.Debug("IN");

            try
            {

                Int32 unixTimestamp = (Int32)(chosenDate.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

                // format the custom datetime picker for time
                System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                double unix = double.Parse(unixTimestamp.ToString());
                dtDateTime = dtDateTime.AddSeconds(unix).ToLocalTime();
                dtClockOutAll.Value = dtDateTime;
                dtClockOutAll.Format = DateTimePickerFormat.Time;
                dtClockOutAll.ShowUpDown = true;

                // show the now time on the now option
                rbNow.Text += " - " + convertUnixDateTimeToDisplayTime(unixTimestamp.ToString());

                // this is very mvp/terrible, i should build the radio button controls dynamically
                // but display the clockout times from the config, we know for MVP there are only 2
                List<string> clockOutTimes = new List<string>();
                for (int i = 0; i < ConfigurationManager.AppSettings.Keys.Count; i++)
                {
                    string keyName = ConfigurationManager.AppSettings.Keys[i].ToString();
                    if (keyName.Contains("ClockOutTime_"))
                    {
                        string configValue = ConfigurationManager.AppSettings[keyName].ToString();
                        // parse out the name from the time, we only need the name
                        string[] vals = keyName.Split(new string[] { "_" }, StringSplitOptions.None);
                        clockOutTimes.Add(configValue);
                        if (vals[1] == "1")
                        {
                            rbOption1.Text = configValue;
                            rbOption1.Tag = configValue;
                        }
                        else
                        {
                            rbOption2.Text = configValue;
                            rbOption2.Tag = configValue;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                log.Error("Error", ex);
                MessageBox.Show(ex.Message, "Error - " + System.Reflection.MethodBase.GetCurrentMethod().Name);
            }

        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            log.Debug("IN");

            try
            {
                Cancelled = false;

                if(rbNow.Checked)
                {
                    // do nothing, the chosen date is already what we need
                }
                if (rbOption1.Checked)
                {
                    // chop up the tag and get the pieces

                    // hour
                    string[] vals = rbOption1.Tag.ToString().Split(new string[] { ":" }, StringSplitOptions.None);
                    int hour = int.Parse(vals[0]);

                    // minutes
                    string[] vals2 = vals[1].Split(new string[] { " " }, StringSplitOptions.None);
                    int minute = int.Parse(vals2[0]);

                    // get the am/pm piece and adjust the hours if necessary
                    if (vals2[1] == "pm")
                    {
                        hour += 12;
                    }

                    DateTime newDate = new DateTime(chosenDate.Year, chosenDate.Month, chosenDate.Day, hour, minute, 0);

                    // set the form variable with the new date that the user selected
                    chosenDate = newDate.ToUniversalTime();
                }
                if (rbOption2.Checked)
                {
                    // chop up the tag and get the pieces

                    // hour
                    string[] vals = rbOption2.Tag.ToString().Split(new string[] { ":" }, StringSplitOptions.None);
                    int hour = int.Parse(vals[0]);

                    // minutes
                    string[] vals2 = vals[1].Split(new string[] { " " }, StringSplitOptions.None);
                    int minute = int.Parse(vals2[0]);

                    // get the am/pm piece and adjust the hours if necessary
                    if (vals2[1] == "pm")
                    {
                        hour += 12;
                    }

                    DateTime newDate = new DateTime(chosenDate.Year, chosenDate.Month, chosenDate.Day, hour, minute, 0);

                    // set the form variable with the new date that the user selected
                    chosenDate = newDate.ToUniversalTime();

                }
                if (rbCustom.Checked)
                {
                    int hour = dtClockOutAll.Value.Hour;
                    int minute = dtClockOutAll.Value.Minute;

                    DateTime xxx = new DateTime(chosenDate.Year, chosenDate.Month, chosenDate.Day, hour, minute, 0);

                    chosenDate = xxx.ToUniversalTime();

                }

                this.Close();

            }
            catch (Exception ex)
            {
                log.Error("Error", ex);
                MessageBox.Show(ex.Message, "Error - " + System.Reflection.MethodBase.GetCurrentMethod().Name);
            }

        }
    }
}
