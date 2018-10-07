using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;
using System.Configuration;
using System.Collections;

namespace TimePunch
{
    public partial class frmPrintableTimesheet : Form
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private string connectionString = "Data Source=" + ConfigurationManager.AppSettings["dbFileName"].ToString() + ";Version=3;";
        SQLiteConnection m_dbConnection;

        // TODO - inputs
        public string userID = "libby.richards";
        public string punchType = "Lab";
        public int weekCount = 2;
        public DateTime timesheetStartDate = new DateTime(2018, 4, 4);

        public frmPrintableTimesheet()
        {
            InitializeComponent();
        }
        void connectToDatabase()
        {
            log.Debug("IN");

            if (m_dbConnection == null)
            {
                m_dbConnection = new SQLiteConnection(connectionString);
                m_dbConnection.Open();
            }
        }

        private void frmPrintableTimesheet_Load(object sender, EventArgs e)
        {
            log.Debug("IN");

            try
            {
                connectToDatabase();


                //create and initialize the stringbuilder that will house the rows for the detail part of this report
                StringBuilder[] weekRow = new StringBuilder[weekCount];
                for (int i = 0; i < weekCount; i++)
                {
                    weekRow[i] = new StringBuilder("");
                }
                string[] weekRange = new string[weekCount];
                int[] weekMinuteTotals = new int[weekCount];
                double[] weekHourTotals = new double[weekCount];
                double grandTotalHours = 0.00;

                // get the first day of the week for that week
                DateTime startdate = timesheetStartDate.AddDays(-(int)timesheetStartDate.DayOfWeek);
                DateTime enddate = startdate.AddDays(7 * weekCount);

                //calculate week ranges
                //string weekRange = "5/1/2018 - 6/1/2018";
                for (int i = 0; i < weekCount; i++)
                {
                    weekRange[i] = startdate.AddDays(7 * i).ToShortDateString() + " - " + startdate.AddDays(7 * i + 7 - 1).ToShortDateString();
                }

                // convert to unix stamps for the query
                Int32 unixTimestampStartDate = (Int32)(startdate.ToUniversalTime().Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                Int32 unixTimestampEndDate = (Int32)(enddate.ToUniversalTime().Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

                StringBuilder sqlDailyTotals = new StringBuilder();
                sqlDailyTotals.AppendLine("select ");
                sqlDailyTotals.AppendLine(" u.userLastName");
                sqlDailyTotals.AppendLine(", u.userFirstName");
                sqlDailyTotals.AppendLine(", e.signinMonth");
                sqlDailyTotals.AppendLine(", e.signinDay");
                sqlDailyTotals.AppendLine(", e.signinYear");
                sqlDailyTotals.AppendLine(", e.signinType");
                sqlDailyTotals.AppendLine(", e.signoutUnixTime");
                sqlDailyTotals.AppendLine(", e.signinUnixTime");
                sqlDailyTotals.AppendLine(", (e.signoutUnixTime - e.signinUnixTime)/60 as clockminutes ");
                sqlDailyTotals.Append(", (e.signoutUnixTime - e.signinUnixTime)/cast((60*60) as float) as clockhours ");
                sqlDailyTotals.AppendLine("from ");
                sqlDailyTotals.AppendLine("TimePunchEvents as e ");
                sqlDailyTotals.AppendLine("inner join ");
                sqlDailyTotals.AppendLine("TimePunchUserInfo as u on e.userIdentity = u.userIdentity ");

                sqlDailyTotals.AppendLine(" Where ");
                sqlDailyTotals.AppendLine(" e.userIdentity ='" + userID + "' ");
                sqlDailyTotals.AppendLine(" AND ");
                sqlDailyTotals.AppendLine(" e.signinType ='" + punchType + "' ");
                sqlDailyTotals.AppendLine(" AND ");

                sqlDailyTotals.AppendLine(" e.signinUnixTime between ");
                sqlDailyTotals.AppendLine(unixTimestampStartDate.ToString());
                sqlDailyTotals.AppendLine(" AND ");
                sqlDailyTotals.AppendLine(unixTimestampEndDate.ToString());

                //sqlDailyTotals.AppendLine(" group by ");
                //sqlDailyTotals.AppendLine("u.userGrade");
                //sqlDailyTotals.AppendLine(",e.userIdentity");
                //sqlDailyTotals.AppendLine(", e.signinYear");
                //sqlDailyTotals.AppendLine(",e.signinMonth");
                //sqlDailyTotals.AppendLine(",e.signinDay");
                //sqlDailyTotals.AppendLine(", e.signinType ");
                sqlDailyTotals.AppendLine("order by ");
                sqlDailyTotals.AppendLine("e.signinUnixTime");
                //sqlDailyTotals.AppendLine("u.userGrade");
                //sqlDailyTotals.AppendLine(",e.signinYear");
                //sqlDailyTotals.AppendLine(",e.signinMonth");
                //sqlDailyTotals.AppendLine(",e.signinDay");
                //sqlDailyTotals.AppendLine(",u.userLastName");
                //sqlDailyTotals.AppendLine(", u.userFirstName");
                //sqlDailyTotals.AppendLine(", e.signinType");

                string userName = userID;

                string sql = sqlDailyTotals.ToString();
                SQLiteCommand command2 = new SQLiteCommand(sql, m_dbConnection);
                SQLiteDataReader reader1 = command2.ExecuteReader();
                while (reader1.Read())
                {

                    userName = reader1["userFirstName"].ToString() + " " + reader1["userLastName"].ToString();
                    int SignoutUnixTime = Int32.Parse(reader1["signoutUnixTime"].ToString());
                    int SigninUnixTime = Int32.Parse(reader1["signinUnixTime"].ToString());

                    string inTime = convertUnixDateTimeToDisplayTime(SigninUnixTime.ToString());
                    string outTime = convertUnixDateTimeToDisplayTime(SignoutUnixTime.ToString());

                    string dayName = dayOfTheWeek(SigninUnixTime.ToString());
                    int thisWeek = 1;
                    string thisWeekStyle = "dailylist";

                    DateTime xx = convertUnixDateTimeToDateTime(SigninUnixTime.ToString());

                    double daysdiff = xx.Subtract(startdate).TotalDays;
                    if (daysdiff > 7)
                    {
                        thisWeek = 2;
                        thisWeekStyle = "dailylist1";
                    }
                    int clockMinutes = Int32.Parse(reader1["clockminutes"].ToString());
                    double clockHours = Double.Parse(reader1["clockhours"].ToString());
                    weekMinuteTotals[thisWeek - 1] += clockMinutes;
                    weekHourTotals[thisWeek - 1] += clockHours;
                    grandTotalHours += clockHours;

                    weekRow[thisWeek - 1].AppendLine("	                          <tr class='" + thisWeekStyle + "'>	");
                    weekRow[thisWeek - 1].AppendLine("	                              <td class='dailylist'>" + dayName + "</td>	");
                    weekRow[thisWeek - 1].AppendLine("	                              <td class='dailylist'>" + inTime + "</td>	");
                    weekRow[thisWeek - 1].AppendLine("	                              <td class='dailylist'>" + outTime + "</td>	");
                    weekRow[thisWeek - 1].AppendLine("	                              <td class='dailylist'>" + clockMinutes.ToString() + " </td>	");
                    weekRow[thisWeek - 1].AppendLine("	                              <td class='dailylist'>" + formatDouble(clockHours) + "</td>	");
                    weekRow[thisWeek - 1].AppendLine("	                          </tr>	");

                }
                reader1.Close();

                StringBuilder html = new StringBuilder();
                html.AppendLine("	<!DOCTYPE html PUBLIC '-//W3C//DTD HTML 4.01 Transitional//EN'>	");
                html.AppendLine("	<html>	");
                html.AppendLine("	  <head>	");
                html.AppendLine("	    <meta http-equiv='content-type' content='text/html;	");
                html.AppendLine("	      charset=windows-1252'>	");
                html.AppendLine("	    <title>Timesheet</title>	");
                html.AppendLine("	    <style>	");
                html.AppendLine("	        *{	");
                html.AppendLine("	            font-family: 'Arial'; font-size: 99%;	");
                html.AppendLine("	        }	");
                html.AppendLine("	        table.dailylist{	");
                html.AppendLine("	            border-collapse: collapse;	");
                html.AppendLine("	            width: 100%;	");
                html.AppendLine("	        }	");
                html.AppendLine("	        th.dailylist {	");
                html.AppendLine("	            text-align: left;	");
                html.AppendLine("	        }	");
                html.AppendLine("	        td.dailylist {	");
                html.AppendLine("	            text-align: left; ");
                html.AppendLine("	        }	");
                html.AppendLine("	        tr.dailylist1 {	");
                html.AppendLine("	            background-color:aliceblue; ");
                html.AppendLine("	        }	");
                html.AppendLine("	        tr.dailylist2 {	");
                html.AppendLine("	            background-color:white; ");
                html.AppendLine("	        }	");
                html.AppendLine("	        th.summary {	");
                html.AppendLine("	            text-align: left;	");
                html.AppendLine("	        }	");
                //html.AppendLine("	        tr.dailylist:nth-child(even) {	");
                //html.AppendLine("	            background-color:red;	");
                //html.AppendLine("	        }	");
                //html.AppendLine("	        tr.dailylist:nth-child(odd) {	");
                //html.AppendLine("	            background-color:white;	");
                //html.AppendLine("	        }	");
                html.AppendLine("	        table.master {	");
                html.AppendLine("	            width: 700px;	");
                html.AppendLine("	        }	");
                html.AppendLine("	        table.summary {	");
                html.AppendLine("	            width: 100%;	");
                html.AppendLine("	            border-collapse: collapse;	");
                html.AppendLine("	        }	");
                html.AppendLine("	        table.signatures {	");
                html.AppendLine("	            width: 100%;	");
                html.AppendLine("	            border-collapse: collapse;	");
                html.AppendLine("	        }	");
                html.AppendLine("	        td.header{	");
                html.AppendLine("	            font-size:x-large;	");
                html.AppendLine("	            font-weight:bold;	");
                html.AppendLine("	        }	");
                html.AppendLine("	        td.major {	");
                html.AppendLine("	            font-size: large;	");
                html.AppendLine("	            font-weight: bold;	");
                html.AppendLine("	        }	");
                html.AppendLine("	        td.topborderbold {	");
                html.AppendLine("	            border-top: thin solid;	");
                html.AppendLine("	            font-weight: bold;	");
                html.AppendLine("	        }	");
                html.AppendLine("	        td.topborderboldlarge {	");
                html.AppendLine("	            border-top: thin solid;	");
                html.AppendLine("	            font-weight: bold;	");
                html.AppendLine("	            font-size: large;	");
                html.AppendLine("	        }	");
                html.AppendLine("	    </style>	");
                html.AppendLine("	  </head>	");
                html.AppendLine("	  <body>	");
                html.AppendLine("	      <table class='master' border='0'>	");
                html.AppendLine("	          <tr>	");
                html.AppendLine("	              <td class='header'>	");
                html.AppendLine("	                  Early Childhood Education Log	");
                html.AppendLine("	              </td>	");
                html.AppendLine("	          </tr>	");
                html.AppendLine("	          <tr>	");
                html.AppendLine("	              <td>	");
                html.AppendLine("	                  Grant Career Center Head Start<br />	");
                html.AppendLine("	                  718 W. Plane St<br />	");
                html.AppendLine("	                  Bethel  OH 45106<br />	");
                html.AppendLine("	              </td>	");
                html.AppendLine("	          </tr>	");
                html.AppendLine("	          <tr>	");
                html.AppendLine("	              <td>	");
                html.AppendLine("	                  &nbsp;	");
                html.AppendLine("	              </td>	");
                html.AppendLine("	          </tr>	");
                html.AppendLine("	          <tr>	");
                html.AppendLine("	              <td class='major'>	");
                // name and type
                html.AppendLine("	                  " + userName + " - " + punchType + "  ");
                html.AppendLine("	              </td>	");
                html.AppendLine("	          </tr>	");
                html.AppendLine("	          <tr>	");
                html.AppendLine("	              <td>	");
                html.AppendLine("	                  &nbsp;	");
                html.AppendLine("	              </td>	");
                html.AppendLine("	          </tr>	");


                for (int i = 0; i < weekCount; i++)
                {

                    html.AppendLine("	          <tr>	");
                    html.AppendLine("	              <td class='major'>	");
                    html.AppendLine("	                  Week " + (i + 1).ToString() + " -> " + weekRange[i] + "  ");
                    html.AppendLine("	              </td>	");
                    html.AppendLine("	          </tr>	");
                    html.AppendLine("	          <tr>	");
                    html.AppendLine("	              <td>	");
                    html.AppendLine("	                  <table class='dailylist'>	");
                    html.AppendLine("	                      <tbody>	");
                    html.AppendLine("	                          <tr class='dailylist'>	");
                    html.AppendLine("	                              <th class='dailylist'>	");
                    html.AppendLine("	                                  &nbsp;	");
                    html.AppendLine("	                              </th>	");
                    html.AppendLine("	                              <th class='dailylist'>Clock In</th>	");
                    html.AppendLine("	                              <th class='dailylist'>Clock Out</th>	");
                    html.AppendLine("	                              <th class='dailylist'>Minutes</th>	");
                    html.AppendLine("	                              <th class='dailylist'>Hours</th>	");
                    html.AppendLine("	                          </tr>	");

                    html.Append(weekRow[i].ToString());

                    html.AppendLine("	                          <tr class='dailylist2'>	");
                    html.AppendLine("	                              <td>	");
                    html.AppendLine("	                                  &nbsp;	");
                    html.AppendLine("	                              </td>	");
                    html.AppendLine("	                              <td>	");
                    html.AppendLine("	                                  &nbsp;	");
                    html.AppendLine("	                              </td>	");
                    html.AppendLine("	                              <td>	");
                    html.AppendLine("	                                  &nbsp;	");
                    html.AppendLine("	                              </td>	");
                    html.AppendLine("	                              <td>	");
                    html.AppendLine("	                                  &nbsp;	");
                    html.AppendLine("	                              </td>	");
                    html.AppendLine("	                              <td>	");
                    html.AppendLine("	                                  &nbsp;	");
                    html.AppendLine("	                              </td>	");
                    html.AppendLine("	                          </tr>	");
                    html.AppendLine("	                          <tr class='dailylist1'>	");
                    html.AppendLine("	                              <td class='topborderbold'>	");
                    // use the proper week
                    html.AppendLine("	                                  Week " + (i + 1).ToString() + " Total	");
                    html.AppendLine("	                              </td>	");
                    html.AppendLine("	                              <td class='topborderbold'>	");
                    html.AppendLine("	                                  &nbsp;	");
                    html.AppendLine("	                              </td>	");
                    html.AppendLine("	                              <td class='topborderbold'>	");
                    html.AppendLine("	                                  &nbsp;	");
                    html.AppendLine("	                              </td>	");
                    //Week Totals
                    html.AppendLine("	                              <td  class='topborderbold'>" + weekMinuteTotals[i].ToString() + " </td>	");
                    html.AppendLine("	                              <td  class='topborderbold'>" + formatDouble(weekHourTotals[i]) + " </td>	");
                    html.AppendLine("	                          </tr>	");
                    html.AppendLine("	                      </tbody>	");
                    html.AppendLine("	                  </table>	");
                    html.AppendLine("	              </td>	");
                    html.AppendLine("	          </tr>	");

                    html.AppendLine("	          <tr>	");
                    html.AppendLine("	              <td>	");
                    html.AppendLine("	                  &nbsp;	");
                    html.AppendLine("	              </td>	");
                    html.AppendLine("	          </tr>	");
                }





                /*
                html.AppendLine("	          <tr>	");
                html.AppendLine("	              <td>	");
                html.AppendLine("	                  &nbsp;	");
                html.AppendLine("	              </td>	");
                html.AppendLine("	          </tr>	");
                html.AppendLine("	          <tr>	");
                html.AppendLine("	              <td>	");
                html.AppendLine("	                  &nbsp;	");
                html.AppendLine("	              </td>	");
                html.AppendLine("	          </tr>	");
                */
                html.AppendLine("	          <tr>	");
                html.AppendLine("	              <td class='major'>	");
                html.AppendLine("	                  Summary	");
                html.AppendLine("	              </td>	");
                html.AppendLine("	          </tr>	");
                html.AppendLine("	          <tr>	");
                html.AppendLine("	              <td>	");
                html.AppendLine("	                  <table class='summary'>	");
                html.AppendLine("	                      <tr>	");
                html.AppendLine("	                          <th class='summary'>&nbsp;</th>	");
                html.AppendLine("	                          <th class='summary'>Hours</th>	");
                html.AppendLine("	                      </tr>	");
                for (int i = 0; i < weekCount; i++)
                {
                    // Week summary - START
                    html.AppendLine("	                      <tr>	");
                    html.AppendLine("	                          <td>Week " + (i + 1).ToString() + " -> " + weekRange[i] + "</td>	");
                    html.AppendLine("	                          <td>" + formatDouble(weekHourTotals[i]) + "</td>	");
                    html.AppendLine("	                      </tr>	");
                    // Week summary - END
                }

                // Grand totals - START
                html.AppendLine("	                      <tr>	");
                html.AppendLine("	                          <td class='topborderbold'>Grand Total</td>	");
                //grand total hours
                html.AppendLine("	                          <td class='topborderbold'>" + formatDouble(grandTotalHours) + "</td>	");
                html.AppendLine("	                      </tr>	");
                // Grand totals - END

                html.AppendLine("	                  </table>	");
                html.AppendLine("	              </td>	");
                html.AppendLine("	          </tr>	");
                html.AppendLine("	          <tr>	");
                html.AppendLine("	              <td>	");
                html.AppendLine("	                  &nbsp;	");
                html.AppendLine("	              </td>	");
                html.AppendLine("	          </tr>	");
                html.AppendLine("	          <tr>	");
                html.AppendLine("	              <td>	");
                html.AppendLine("	                  &nbsp;	");
                html.AppendLine("	              </td>	");
                html.AppendLine("	          </tr>	");
                html.AppendLine("	          <tr>	");
                html.AppendLine("	              <td>	");
                html.AppendLine("	                  <table class='signatures'>	");
                html.AppendLine("	                      <tr>	");
                html.AppendLine("	                          <td>&nbsp;</td>	");
                html.AppendLine("	                          <td>&nbsp;</td>	");
                html.AppendLine("	                      </tr>	");
                html.AppendLine("	                      <tr>	");
                html.AppendLine("	                          <td>&nbsp;</td>	");
                html.AppendLine("	                          <td>&nbsp;</td>	");
                html.AppendLine("	                      </tr>	");
                html.AppendLine("	                      <tr>	");
                html.AppendLine("	                          <td class='topborderbold'>Employee Signature</td>	");
                html.AppendLine("	                          <td class='topborderbold'>Date</td>	");
                html.AppendLine("	                      </tr>	");
                html.AppendLine("	                      <tr>	");
                html.AppendLine("	                          <td>&nbsp;</td>	");
                html.AppendLine("	                          <td>&nbsp;</td>	");
                html.AppendLine("	                      </tr>	");
                html.AppendLine("	                      <tr>	");
                html.AppendLine("	                          <td>&nbsp;</td>	");
                html.AppendLine("	                          <td>&nbsp;</td>	");
                html.AppendLine("	                      </tr>	");
                html.AppendLine("	                      <tr>	");
                html.AppendLine("	                          <td>&nbsp;</td>	");
                html.AppendLine("	                          <td>&nbsp;</td>	");
                html.AppendLine("	                      </tr>	");
                html.AppendLine("	                      <tr>	");
                html.AppendLine("	                          <td class='topborderbold'>Manager Signature</td>	");
                html.AppendLine("	                          <td class='topborderbold'>Date</td>	");
                html.AppendLine("	                      </tr>	");
                html.AppendLine("	                  </table>	");
                html.AppendLine("	              </td>	");
                html.AppendLine("	          </tr>	");
                html.AppendLine("	      </table>	");
                html.AppendLine("	  </body>	");
                html.AppendLine("	</html>	");

                DisplayHtml(webBrowser1, html.ToString());
            }
            catch (Exception ex)
            {
                log.Error("Error", ex);
                MessageBox.Show(ex.Message, "Error - " + System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        private void btnPrintPreview_Click(object sender, EventArgs e)
        {
            log.Debug("IN");

            //printPreviewDialog1.page
            webBrowser1.ShowPrintPreviewDialog();
        }
        private void DisplayHtml(WebBrowser browserControl, string html)
        {
            log.Debug("IN");

            browserControl.Navigate("about:blank");
            try
            {
                if (browserControl.Document != null)
                {
                    browserControl.Document.Write(string.Empty);
                }
            }
            catch (Exception ex)
            { } // do nothing with this
            browserControl.DocumentText = html;
        }
        private void frmPrintableTimesheet_FormClosing(object sender, FormClosingEventArgs e)
        {
            log.Debug("IN");

            try
            {
                //m_dbConnection.Close();
            }
            catch (Exception ex)
            {
                log.Error("Error", ex);
                // dont care
                //MessageBox.Show(ex.Message, "Error - " + System.Reflection.MethodBase.GetCurrentMethod().Name);
            }

        }
        private string convertUnixDateTimeToDisplayDateTime(string unixDateTime)
        {
            log.Debug("IN");

            // Unix timestamp is seconds past epoch
            double unix = double.Parse(unixDateTime);
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unix).ToLocalTime();

            string result = dtDateTime.ToShortDateString() + " " + dtDateTime.ToShortTimeString();

            return result;

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
        private DateTime convertUnixDateTimeToDateTime(string unixDateTime)
        {
            log.Debug("IN");

            // Unix timestamp is seconds past epoch
            double unix = double.Parse(unixDateTime);
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unix).ToLocalTime();

            return dtDateTime;

        }
        private string dayOfTheWeek(string unixDateTime)
        {
            log.Debug("IN");

            // Unix timestamp is seconds past epoch
            double unix = double.Parse(unixDateTime);
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unix).ToLocalTime();

            //DayOfWeek xx = dtDateTime.DayOfWeek;
            string result = dtDateTime.DayOfWeek.ToString();

            return result;

        }
        private string formatDouble(double doubleToFormat)
        {
            log.Debug("IN");

            string result = doubleToFormat.ToString("#.##");

            if(doubleToFormat == 0)
            {
                result = "0";
            }
            //doubleToFormat.ToString()
            //string.Format("{0:0.00}", answer))
            return result;

        }

    }
}
