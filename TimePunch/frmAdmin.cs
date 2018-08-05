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

// publish info
//https://social.msdn.microsoft.com/Forums/windows/en-US/7f9462c2-5c03-49e3-aa96-f3d09cbe9fa2/clickonce-certificate-creation-error?forum=winformssetup
//https://docs.microsoft.com/en-us/visualstudio/deployment/how-to-publish-a-clickonce-application-using-the-publish-wizard

//TODO - send database
//TODO - send logs -- SmtpAppender 
//TODO - combo box type ahead - built in AutoComplete*

//TODO - Unit tests and/or functional tests

//TODO - validate form fields before submitting (which ones?)
//TODO - do not allow commas to be entered when creating a user
//TODO - i should pass the db info to each form as i launch it so it doesnt have to be in each form directly
//TODO - create a smart picker punch type picker based on the time of day (lab is in morning, so auto-select lab if its the morning, etc.).
//TODO - make classes for all objects, and replace the strings with the class properties in all the sql
//TODO - create data layer classes to replace all of the inline sql updates and inserts
//TODO - crete data layer functions to retrieve data for the classes that have been built



//--------------------------------------------------
/*
 *  
 *  DONE - I only have to track lab and theory hours 
 *  DONE - I need to be able to add/deduct time for each student on any occasion
 *  DONE - I need to be able to clock everyone out at the end of class
 *  IMPROVE - I need to be able to see whom has not clocked out
 *  DONE - I need to be able to clock someone out if they forgot to clock out
 *  IMPROVE - I need it to break down daily to report to state board monthly.  
 *  IMPROVE - State Board has their own form when submitting hours monthly.
 */

//Logging - https://stackify.com/log4net-guide-dotnet-logging/
//I highly recommend using Prefix, Stackify’s FREE .NET Profiler to view your logs per web request, along with SQL queries, HTTP calls and much more.
//https://stackify.com/three-types-of-net-profilers/
//Log Parser
//https://archive.codeplex.com/?p=visuallogparser

namespace TimePunch
{
    public partial class frmAdmin : Form
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private string connectionString = "Data Source=" + ConfigurationManager.AppSettings["dbFileName"].ToString() + ";Version=3;";

        SQLiteConnection m_dbConnection;

        public string AdminUserID = "";
        public string AdminUserPassword = "";

        public frmAdmin()
        {
            InitializeComponent();
        }

        void OneTimeSetup()
        {
            log.Debug("IN");

            createNewDatabase();
            createTimePunchTable();
            createUserIdentitiesTable();
            createUserInfoTable();
            createVersionTable();
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


        // Creates an empty database file
        void createNewDatabase()
        {
            log.Debug("IN");

            connectToDatabase();
            // this wasnt necessary, it happens automatically
            //SQLiteConnection.CreateFile(dbFileName);
        }
        void createTimePunchTable()
        {
            log.Debug("IN");

            string sql = "create table TimePunchEvents" +
                " (" +
                " userIdentity varchar(20), " +
                " adminIdentity varchar(20), " +
                " manualPunch int, " +
                " signinUnixTime int, " +
                " signoutUnixTime int, " +
                " signinYear int, " +
                " signinMonth int, " +
                " signinDay int, " +
                " signoutYear int, " +
                " signoutMonth int, " +
                " signoutDay int, " +
                " signinType varchar(20), " +
                " signinComputer varchar(20), " +
                " signoutComputer varchar(20), " +
                " createUnixTimeStamp int, " +
                " updateUnixTimeStamp int " +
                " )";

            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            log.Info("SQL: " + sql.Replace(Environment.NewLine, " "));
            command.ExecuteNonQuery();

            log.Info("Table Created: TimePunchEvents");
        }
        void createUserIdentitiesTable()
        {
            log.Debug("IN");

            string sql = "create table TimePunchUserIdentities" +
                " (" +
                " userIdentity varchar(20), " +
                " userPassword varchar(20), " +
                " userFingerprint text, " +
                " isAdmin int, " +
                " createUnixTimeStamp int, " +
                " updateUnixTimeStamp int " +
                " )";

            SQLiteCommand command1 = new SQLiteCommand(sql, m_dbConnection);
            log.Info("SQL: " + sql.Replace(Environment.NewLine, " "));
            command1.ExecuteNonQuery();
            log.Info("Table Created: TimePunchUserIdentities");

            // add the admin user

            // grab a timestamp
            Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

            sql = "insert into TimePunchUserIdentities" +
                " (" +
                " userIdentity, " +
                " userPassword, " +
                " userFingerprint, " +
                " isAdmin, " +
                " createUnixTimeStamp, " +
                " updateUnixTimeStamp" +
                " ) values " +
                " (" +
                " '" + AdminUserID + "', " +
                " '" + AdminUserPassword + "', " +
                 "'', " +
                 "1, " +
                unixTimestamp.ToString() + ", " +
                unixTimestamp.ToString() +
                " ) ";

            SQLiteCommand command2 = new SQLiteCommand(sql, m_dbConnection);
            log.Info("SQL: " + sql.Replace(Environment.NewLine, " "));
            command2.ExecuteNonQuery();

            log.Info("User Added: " + AdminUserID);

        }
        void createUserInfoTable()
        {
            log.Debug("IN");

            string sql = "create table TimePunchUserInfo" +
                " (" +
                " userIdentity varchar(20), " +
                " userFirstName varchar(100), " +
                " userLastName varchar(100), " +
                " userGrade varchar(20), " +
                " createUnixTimeStamp int, " +
                " updateUnixTimeStamp int " +
                " )";

            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            log.Info("SQL: " + sql.Replace(Environment.NewLine, " "));
            command.ExecuteNonQuery();

            log.Info("Table Created: TimePunchUserInfo");

            // add the admin user

            // grab a timestamp
            Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

            sql = "insert into TimePunchUserInfo" +
                " (" +
                " userIdentity, " +
                " userFirstName , " +
                " userLastName , " +
                " userGrade, " +
                " createUnixTimeStamp, " +
                " updateUnixTimeStamp" +
                " ) values " +
                " (" +
                " '" + AdminUserID + "', " +
                 "'Admin', " +
                 "'Admin', " +
                 "13, " +
                unixTimestamp.ToString() + ", " +
                unixTimestamp.ToString() +
                " ) ";

            SQLiteCommand command2 = new SQLiteCommand(sql, m_dbConnection);
            log.Info("SQL: " + sql.Replace(Environment.NewLine, " "));
            command2.ExecuteNonQuery();
            log.Info("User Added: " + AdminUserID);
        }
        void createVersionTable()
        {
            log.Debug("IN");

            string sql = "create table TimePunchDBVersion" +
                " (" +
                " dbVersion varchar(20), " +
                " createUnixTimeStamp int, " +
                " updateUnixTimeStamp int " +
                " )";

            SQLiteCommand command1 = new SQLiteCommand(sql, m_dbConnection);
            log.Info("SQL: " + sql.Replace(Environment.NewLine, " "));
            command1.ExecuteNonQuery();

            log.Info("Table Created: TimePunchDBVersion");

            // add the current version

            // grab a timestamp
            Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            string dbVersion = "1";
            sql = "insert into TimePunchDBVersion" +
                " (" +
                " dbVersion, " +
                " createUnixTimeStamp, " +
                " updateUnixTimeStamp " +
                " ) values " +
                " (" +
                " '" + dbVersion + "', " +
                unixTimestamp.ToString() + ", " +
                unixTimestamp.ToString() +
                " ) ";

            SQLiteCommand command2 = new SQLiteCommand(sql, m_dbConnection);
            log.Info("SQL: " + sql.Replace(Environment.NewLine, " "));
            command2.ExecuteNonQuery();
            Globals.DBVersion = dbVersion;
            log.Info("DBVersion=" + Globals.DBVersion);

            log.Info("Table Row Added - TimePunchDBVersion " + dbVersion);

        }

        private void btnDataDump_Click(object sender, EventArgs e)
        {
            log.Debug("IN");

            try
            {

                ListViewItem xx = (ListViewItem)cboDataDump.SelectedItem;
                string sql = xx.Tag.ToString();

                // if this is the custom type, get the sql from the textbox
                if (xx.Text == "Custom...")
                {
                    sql = txtDataQuery.Text;
                }

                bool showDates = chkIncludeReadableDates.Checked;

                StringBuilder output = new StringBuilder();
                StringBuilder headerRow = new StringBuilder();
                StringBuilder sqlStar = new StringBuilder();

                // Make a connection to the database if it hasnt already
                connectToDatabase();

                SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
                SQLiteDataReader reader = command.ExecuteReader();
                int rowCount = 0;
                while (reader.Read())
                {
                    int columnCount = reader.FieldCount;

                    for (int i = 0; i < columnCount; i++)
                    {
                        string fieldName = reader.GetName(i);
                        // create the header if this is the first row
                        if (rowCount == 0)
                        {
                            if (i != 0)
                            {
                                headerRow.Append(",");
                                sqlStar.Append(",");
                            }
                            headerRow.Append(fieldName.Replace(",", ""));
                            sqlStar.Append(fieldName.Replace(",", ""));
                            if (fieldName.IndexOf("UnixTime") > 0 && showDates)
                            {
                                headerRow.Append(",");
                                sqlStar.Append(",");
                                headerRow.Append(fieldName.Replace(",", "") + "_ToDate");
                                sqlStar.Append(" datetime(" + fieldName.Replace(",", "") + ", 'unixepoch', 'localtime') as dt" + fieldName.Replace(",", "").Replace("UnixTime", ""));
                            }
                        }

                        // row data
                        if (i != 0)
                        {
                            output.Append(",");
                        }
                        output.Append(reader[i].ToString());
                        if (fieldName.IndexOf("UnixTime") > 0 && showDates)
                        {
                            output.Append(",");
                            output.Append(convertUnixDateTimeToDisplayDateTime(reader[i].ToString()));
                        }
                    }
                    output.Append(Environment.NewLine);
                    rowCount += 1;
                }
                reader.Close();

                txtDataQuery.Text = sql.Replace(" * ", " " + sqlStar.ToString() + " ");
                headerRow.Append(Environment.NewLine);
                txtDataResults.Text = headerRow.ToString() + output.ToString();
                if (txtDataResults.Text.Replace(Environment.NewLine, "") == "")
                {
                    txtDataResults.Text = "No Data Found";
                }

                //now fill the grid from the text data
                grdDataResults.Columns.Clear();
                if (txtDataResults.Text.Replace(Environment.NewLine, "") != "")
                {
                    string[] rows = txtDataResults.Text.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
                    int rowIndex = 0;
                    foreach (string row in rows)
                    {
                        if (rowIndex == 0)
                        {
                            // column header row
                            string[] columns = row.Split(new string[] { "," }, StringSplitOptions.None);
                            foreach (string column in columns)
                            {
                                grdDataResults.Columns.Add(column, column);
                            }
                        }
                        else
                        {
                            // data row
                            DataGridViewRow gridrow = new DataGridViewRow();

                            string[] fields = row.Split(new string[] { "," }, StringSplitOptions.None);
                            foreach (string field in fields)
                            {
                                DataGridViewTextBoxCell cell = new DataGridViewTextBoxCell();
                                cell.Value = field;
                                gridrow.Cells.Add(cell);
                            }


                            grdDataResults.Rows.Add(gridrow);
                        }

                        rowIndex += 1;
                    }
                }

                // create the columns for the grid as placeholders, we will put the names in later
                /*
                if (rowCount == 0)
                {
                    for (int j = 0; j < columnCount; j++)
                    {
                        grdDataResults.Columns.Add(j.ToString(), j.ToString());
                    }
                }
                grdDataResults.Columns[i].HeaderText = fieldName;
                grdDataResults.Columns[i].Name = fieldName;
                // row for the data grid
                DataGridViewRow row = new DataGridViewRow();


                DataGridViewTextBoxCell cell = new DataGridViewTextBoxCell();
                cell.Value = reader[i].ToString();
                row.Cells.Add(cell);
                grdDataResults.Rows.Add(row);
                */



            }
            catch (Exception ex)
            {
                log.Error("Error", ex);
                MessageBox.Show(ex.Message, "Error - " + System.Reflection.MethodBase.GetCurrentMethod().Name);
            }

        }

        private void btnOneTimeSetup_Click(object sender, EventArgs e)
        {
            log.Debug("IN");

            try
            {
                OneTimeSetup();
                MessageBox.Show("Setup Complete!", "Info");
            }
            catch (Exception ex)
            {
                log.Error("Error", ex);
                MessageBox.Show(ex.Message, "Error - " + System.Reflection.MethodBase.GetCurrentMethod().Name);
            }


        }

        private void frmAdmin_FormClosing(object sender, FormClosingEventArgs e)
        {
            log.Debug("IN");

            try
            {
                m_dbConnection.Close();
            }
            catch (Exception ex)
            {
                log.Error("Error", ex);
                // dont care
                //MessageBox.Show(ex.Message, "Error - " + System.Reflection.MethodBase.GetCurrentMethod().Name);
            }

        }

        private void frmAdmin_Load(object sender, EventArgs e)
        {
            log.Debug("IN");

            try
            {
                //string exePath = Application.ExecutablePath;
                string appPath = Application.StartupPath;
                connectToDatabase();
                fillComboWithUsers(cboUsers);
                fillComboWithUsers(cboUsersForPassword);
                setupQueries();
                dtUpdateHours.Value = DateTime.Now;
                dtClockOutAll.Value = DateTime.Now;
                txtDBBackup.Text = ConfigurationManager.AppSettings["BackupPath"].ToString();
                txtLogBackup.Text = ConfigurationManager.AppSettings["BackupPath"].ToString();

            }
            catch (Exception ex)
            {
                log.Error("Error", ex);
                MessageBox.Show(ex.Message, "Error - " + System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        private void fillComboWithUsers(ComboBox cboToFill)
        {
            log.Debug("IN");

            // fill the combo box with users
            cboToFill.Items.Clear();
            // grab the last punch in
            // Get the last punch
            string sql = "select * from TimePunchUserInfo order by userLastName, userFirstName";

            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                string displayName = reader["userLastName"].ToString() + ", " + reader["userFirstName"].ToString() + " (" + reader["userGrade"].ToString() + ")";
                string userIdentity = reader["userIdentity"].ToString();

                ListViewItem xx = new ListViewItem();
                xx.Tag = userIdentity;
                xx.Text = displayName;
                cboToFill.Items.Add(xx);

            }
            reader.Close();

            if (cboToFill.Items.Count > 0)
            {
                cboToFill.SelectedIndex = 0;
                cboToFill.DisplayMember = "Text";
                cboToFill.ValueMember = "Tag";
            }
        }

        private void btnShowDateMinutes_Click(object sender, EventArgs e)
        {
            log.Debug("IN");

            try
            {
                ListViewItem xx = (ListViewItem)cboUsers.SelectedItem;

                string userIdentity = xx.Tag.ToString();
                string signinType = "";
                if (rdLab.Checked)
                {
                    signinType = ConfigurationManager.AppSettings["SignInType_Lab"].ToString();
                }
                else
                {
                    signinType = ConfigurationManager.AppSettings["SignInType_Theory"].ToString();
                }


                // add columns to grid
                grdUserHours.Columns.Clear();
                grdUserHours.Columns.Add("userIdentity", "User");
                grdUserHours.Columns.Add("signinUnixTime", "Time In");
                grdUserHours.Columns.Add("signoutUnixTime", "Time Out");
                grdUserHours.Columns.Add("createUnixTimeStamp", "Unique Key");
                grdUserHours.Columns.Add("manualPunch", "Manual");

                foreach (DataGridViewTextBoxColumn col in grdUserHours.Columns)
                {
                    if (col.Name == "userIdentity" || col.Name == "createUnixTimeStamp" || col.Name == "manualPunch")
                    {
                        col.ReadOnly = true;
                    }
                    if (col.Name == "signinUnixTime" || col.Name == "signoutUnixTime")
                    {
                        col.Width = 130;
                    }
                    if (col.Name == "userIdentity")
                    {
                        col.Width = 100;
                    }
                    if (col.Name == "createUnixTimeStamp")
                    {
                        col.Width = 85;
                    }
                    if (col.Name == "manualPunch")
                    {
                        col.Width = 50;
                    }


                }

                // Auto-resize - doesnt seem to work
                //grdUserHours.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

                // Make a connection to the database if it hasnt already
                connectToDatabase();

                string sql = "select *, datetime(signinUnixTime, 'unixepoch', 'localtime') dtSignIn, datetime(signoutUnixTime, 'unixepoch', 'localtime') dtSignOut  from TimePunchEvents " +
                    " where userIdentity = '" + userIdentity + "' and signinType = '" + signinType + "' " +
                    " and signinYear = " + dtUpdateHours.Value.Year.ToString() +
                    " and signinMonth = " + dtUpdateHours.Value.Month.ToString() +
                    " and signinDay = " + dtUpdateHours.Value.Day.ToString() +
                    " order by createUnixTimeStamp asc";

                SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
                SQLiteDataReader reader = command.ExecuteReader();
                StringBuilder output = new StringBuilder();
                int secondsTotal = 0;
                int minutesTotal = 0;

                while (reader.Read())
                {
                    Punch punch = new Punch();

                    punch.SignoutUnixTime = Int32.Parse(reader["signoutUnixTime"].ToString());
                    punch.SigninUnixTime = Int32.Parse(reader["signinUnixTime"].ToString());
                    punch.UserIdentity = reader["userIdentity"].ToString();
                    punch.ManualPunch = Int32.Parse(reader["manualPunch"].ToString());
                    punch.CreateUnixTimeStamp = Int32.Parse(reader["createUnixTimeStamp"].ToString());

                    secondsTotal += (punch.SignoutUnixTime - punch.SigninUnixTime);

                    output.Append("ID: " + punch.UserIdentity);
                    output.Append(" - ");
                    output.Append("In Date: " + convertUnixDateTimeToDisplayDateTime(punch.SigninUnixTime.ToString()));
                    output.Append(" - ");
                    output.Append("Out Date: " + convertUnixDateTimeToDisplayDateTime(punch.SignoutUnixTime.ToString()));
                    output.Append(" - ");
                    output.Append("Manual: ");
                    if (punch.ManualPunch == 1)
                    {
                        output.Append("true");
                    }
                    else
                    {
                        output.Append("false");
                    }

                    output.Append(" - ");
                    output.Append("Minutes: ");
                    output.Append(((punch.SignoutUnixTime - punch.SigninUnixTime) / 60).ToString());

                    output.Append(Environment.NewLine);

                    grdUserHours.Rows.Add(new object[] {
                        punch.UserIdentity,
                        convertUnixDateTimeToDisplayDateTime(punch.SigninUnixTime.ToString()),
                        convertUnixDateTimeToDisplayDateTime(punch.SignoutUnixTime.ToString()),
                        punch.CreateUnixTimeStamp.ToString(),
                        punch.ManualPunch.ToString()
                    });

                }
                reader.Close();

                output.Append(Environment.NewLine);
                if (secondsTotal > 0)
                {
                    minutesTotal = secondsTotal / 60;
                }
                output.Append("Total " + signinType + " minutes: " + minutesTotal.ToString());

                txtDayResults.Text = output.ToString();

            }
            catch (Exception ex)
            {
                log.Error("Error", ex);
                MessageBox.Show(ex.Message, "Error - " + System.Reflection.MethodBase.GetCurrentMethod().Name);
            }

        }


        private void btnAddMinutes_Click(object sender, EventArgs e)
        {
            log.Debug("IN");

            //ASSUMPTION - There is only one modification per day per type per person.  
            //If a modification is made, and a modification already exists, 
            //it will simply be updated with a new end date to reflect the minutes added or subtracted (start date stays the same)

            try
            {
                ListViewItem xx = (ListViewItem)cboUsers.SelectedItem;

                // create punch placeholder for update/add punch
                Punch modPunch = new Punch();
                modPunch.ManualPunch = 1;

                modPunch.UserIdentity = xx.Tag.ToString();

                if (rdLab.Checked)
                {
                    modPunch.SigninType = ConfigurationManager.AppSettings["SignInType_Lab"].ToString();
                }
                else
                {
                    modPunch.SigninType = ConfigurationManager.AppSettings["SignInType_Theory"].ToString();
                }

                // Make a connection to the database if it hasnt already
                connectToDatabase();

                // grab the records for the day selected
                string sql = "select *, datetime(signinUnixTime, 'unixepoch', 'localtime') dtSignIn, datetime(signoutUnixTime, 'unixepoch', 'localtime') dtSignOut  from TimePunchEvents " +
                    " where userIdentity = '" + modPunch.UserIdentity + "' and signinType = '" + modPunch.SigninType + "' " +
                    " and signinYear = " + dtUpdateHours.Value.Year.ToString() +
                    " and signinMonth = " + dtUpdateHours.Value.Month.ToString() +
                    " and signinDay = " + dtUpdateHours.Value.Day.ToString() +
                    " order by createUnixTimeStamp asc";

                SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
                SQLiteDataReader reader = command.ExecuteReader();
                StringBuilder output = new StringBuilder();

                Int32 checkInUX = 0;
                Int32 checkOutUX = 0;
                bool manualFound = false;
                Int32 checkInUXManual = 0;
                Int32 checkOutUXManual = 0;
                Int32 uniqueKey = 0;

                while (reader.Read())
                {
                    Punch punch = new Punch();

                    punch.SignoutUnixTime = Int32.Parse(reader["signoutUnixTime"].ToString());
                    punch.SigninUnixTime = Int32.Parse(reader["signinUnixTime"].ToString());
                    punch.UserIdentity = reader["userIdentity"].ToString();
                    punch.ManualPunch = Int32.Parse(reader["manualPunch"].ToString());
                    punch.CreateUnixTimeStamp = Int32.Parse(reader["createUnixTimeStamp"].ToString());
                    punch.UpdateUnixTimeStamp = Int32.Parse(reader["updateUnixTimeStamp"].ToString());

                    checkInUX = punch.SigninUnixTime;
                    checkOutUX = punch.SignoutUnixTime;
                    uniqueKey = punch.UpdateUnixTimeStamp;
                    // check to see if a manual punch already exists, and store this off seperately
                    if (punch.ManualPunch == 1)
                    {
                        manualFound = true;
                        checkInUXManual = punch.SigninUnixTime;
                        checkOutUXManual = checkOutUX;
                        uniqueKey = punch.UpdateUnixTimeStamp;
                    }
                }
                reader.Close();

                //Prep our inputs

                //get admin id from the form
                modPunch.AdminIdentity = AdminUserID;

                modPunch.UpdateUnixTimeStamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                modPunch.CreateUnixTimeStamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

                //get computer name from somewhere
                modPunch.SigninComputer = System.Environment.MachineName;
                modPunch.SignoutComputer = System.Environment.MachineName;

                if (manualFound)
                {
                    //Just update the existing manual record, so we assume there is only one manual punch per day
                    modPunch.SignoutUnixTime = checkInUXManual + Int32.Parse(txtMinutesToAdd.Text) * 60;

                    string sql3x = "update TimePunchEvents set" +
                        " adminIdentity = '" + modPunch.AdminIdentity + "', " +
                        " signoutUnixTime = " + modPunch.SignoutUnixTime + ", " +
                        " signoutComputer = '" + modPunch.SignoutComputer + "', " +
                        " updateUnixTimeStamp = " + modPunch.UpdateUnixTimeStamp + " " +
                        " where userIdentity = '" + modPunch.UserIdentity + "' " +
                        " and updateUnixTimeStamp = " + uniqueKey.ToString() +
                        " and signinType = '" + modPunch.SigninType + "'";

                    SQLiteCommand command3 = new SQLiteCommand(sql3x, m_dbConnection);
                    log.Info("SQL: " + sql3x.Replace(Environment.NewLine, " "));
                    command3.ExecuteNonQuery();
                }
                else
                {
                    if (checkInUX == checkOutUX)
                    {
                        // there was never a checkout, so return an error or something??
                        // maybe we are ok, we can test this
                        log.Info("Unexpected Condition, there was never a checkout");
                    }

                    // add a manual record

                    // Manual time punches should be at some arbitrary time for the given amount of time.

                    if (checkOutUX > 0)
                    {
                        // just add a minute to the last punch for our manual punch start time
                        modPunch.SigninUnixTime = checkOutUX + 60;
                    }
                    else
                    {
                        //we dont have a punch for this day so we need to create a sign in time, and we will just use the time on the date picker
                        modPunch.SigninUnixTime = (Int32)(dtUpdateHours.Value.ToUniversalTime().Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                    }

                    // Add or subtract the minutes
                    modPunch.SignoutUnixTime = modPunch.SigninUnixTime + Int32.Parse(txtMinutesToAdd.Text) * 60;

                    string sql2x = "insert into TimePunchEvents" +
                        " (" +
                        " userIdentity, " +
                        " adminIdentity, " +
                        " manualPunch, " +
                        " signinUnixTime, " +
                        " signoutUnixTime, " +
                        " signinYear , " +
                        " signinMonth , " +
                        " signinDay , " +
                        " signoutYear , " +
                        " signoutMonth , " +
                        " signoutDay , " +
                        " signinType, " +
                        " signinComputer, " +
                        " signoutComputer, " +
                        " createUnixTimeStamp, " +
                        " updateUnixTimeStamp" +
                        " ) values " +
                        " (" +
                        " '" + modPunch.UserIdentity + "', " +
                        " '" + modPunch.AdminIdentity + "', " +
                        " " + modPunch.ManualPunch + ", " +
                        modPunch.SigninUnixTime + ", " +
                        modPunch.SignoutUnixTime + ", " +
                        convertUnixDateTimeToYear(modPunch.SigninUnixTime.ToString()) + ", " +
                        convertUnixDateTimeToMonth(modPunch.SigninUnixTime.ToString()) + ", " +
                        convertUnixDateTimeToDay(modPunch.SigninUnixTime.ToString()) + ", " +
                        convertUnixDateTimeToYear(modPunch.SignoutUnixTime.ToString()) + ", " +
                        convertUnixDateTimeToMonth(modPunch.SignoutUnixTime.ToString()) + ", " +
                        convertUnixDateTimeToDay(modPunch.SignoutUnixTime.ToString()) + ", " +
                        " '" + modPunch.SigninType + "', " +
                        " '" + modPunch.SigninComputer + "', " +
                        " '" + modPunch.SignoutComputer + "', " +
                        modPunch.CreateUnixTimeStamp + ", " +
                        modPunch.UpdateUnixTimeStamp +
                        " ) ";

                    SQLiteCommand command2 = new SQLiteCommand(sql2x, m_dbConnection);
                    log.Info("SQL: " + sql2x.Replace(Environment.NewLine, " "));
                    command2.ExecuteNonQuery();
                }

                // update the results
                btnShowDateMinutes.PerformClick();

            }
            catch (Exception ex)
            {
                log.Error("Error", ex);
                MessageBox.Show(ex.Message, "Error - " + System.Reflection.MethodBase.GetCurrentMethod().Name);
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
        private string convertUnixDateTimeToYear(string unixDateTime)
        {
            log.Debug("IN");

            // Unix timestamp is seconds past epoch
            double unix = double.Parse(unixDateTime);
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unix).ToLocalTime();

            string result = dtDateTime.Year.ToString();

            return result;

        }
        private string convertUnixDateTimeToMonth(string unixDateTime)
        {
            log.Debug("IN");

            // Unix timestamp is seconds past epoch
            double unix = double.Parse(unixDateTime);
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unix).ToLocalTime();

            string result = dtDateTime.Month.ToString();

            return result;

        }
        private string convertUnixDateTimeToDay(string unixDateTime)
        {
            log.Debug("IN");

            // Unix timestamp is seconds past epoch
            double unix = double.Parse(unixDateTime);
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unix).ToLocalTime();

            string result = dtDateTime.Day.ToString();

            return result;

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void btnUpdateUser_Click(object sender, EventArgs e)
        {
            log.Debug("IN");

            try
            {
                // this is a first time setup, so show the admin screen
                var updateUserForm = new frmUserInfo();
                updateUserForm.isUpdate = true;

                ListViewItem xx = (ListViewItem)cboUsersForPassword.SelectedItem;
                string userIdentity = xx.Tag.ToString();

                updateUserForm.userIDForForm = userIdentity;

                updateUserForm.ShowDialog(this);

                // reset the form
                fillComboWithUsers(cboUsers);
                fillComboWithUsers(cboUsersForPassword);

            }
            catch (Exception ex)
            {
                log.Error("Error", ex);
                MessageBox.Show(ex.Message, "Error - " + System.Reflection.MethodBase.GetCurrentMethod().Name);
            }

        }

        private void btnUpdateHours_Click(object sender, EventArgs e)
        {
            log.Debug("IN");

            try
            {
                // loop throught the grid
                foreach (DataGridViewRow row in grdUserHours.Rows)
                {
                    // look for row changes
                    if (row.Tag != null)
                    {
                        if (row.Tag.ToString() == ConfigurationManager.AppSettings["RowModifiedTag"].ToString())
                        {
                            // Grab the changes
                            string signOutComputer = System.Environment.MachineName;
                            string adminIdentity = AdminUserID;
                            string userID = "";
                            string uniqueKey = "";
                            string timeIn = "";
                            string timeOut = "";

                            foreach (DataGridViewCell cell in row.Cells)
                            {
                                if (grdUserHours.Columns[cell.ColumnIndex].Name == "createUnixTimeStamp")
                                {
                                    uniqueKey = cell.Value.ToString();
                                }
                                if (grdUserHours.Columns[cell.ColumnIndex].Name == "userIdentity")
                                {
                                    userID = cell.Value.ToString();
                                }
                                if (grdUserHours.Columns[cell.ColumnIndex].Name == "signinUnixTime")
                                {
                                    timeIn = cell.Value.ToString();
                                }
                                if (grdUserHours.Columns[cell.ColumnIndex].Name == "signoutUnixTime")
                                {
                                    timeOut = cell.Value.ToString();
                                }

                            }

                            DateTime dtTimeIn = DateTime.Parse(timeIn);
                            DateTime dtTimeOut = DateTime.Parse(timeOut);

                            // validate changes
                            if(dtTimeIn > dtTimeOut)
                            {
                                throw new Exception("Time In cannot be greater than Time Out");
                            }

                            Int32 modifiedsignInTime = (Int32)(dtTimeIn.ToUniversalTime().Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                            Int32 modifiedsignOutTime = (Int32)(dtTimeOut.ToUniversalTime().Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                            // grab a timestamp
                            Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

                            // apply changes
                            connectToDatabase();

                            string sql3 = "update TimePunchEvents set" +
                                " adminIdentity = '" + adminIdentity + "', " +
                                " signinUnixTime = " + modifiedsignInTime.ToString() + ", " +
                                " signoutUnixTime = " + modifiedsignOutTime.ToString() + ", " +
                                " signoutComputer = '" + signOutComputer + "', " +
                                " updateUnixTimeStamp = " + unixTimestamp.ToString() + " " +
                                " where userIdentity = '" + userID + "' " +
                                " and createUnixTimeStamp = " + uniqueKey;

                            SQLiteCommand command3 = new SQLiteCommand(sql3, m_dbConnection);
                            log.Info("SQL: " + sql3.Replace(Environment.NewLine, " "));
                            command3.ExecuteNonQuery();

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

        private void grdUserHours_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            log.Debug("IN");

            try
            {
                grdUserHours.Rows[e.RowIndex].Tag = ConfigurationManager.AppSettings["RowModifiedTag"].ToString();

            }
            catch (Exception ex)
            {
                log.Error("Error", ex);
                MessageBox.Show(ex.Message, "Error - " + System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        private void setupQueries()
        {
            log.Debug("IN");
            cboDataDump.Items.Clear();

            //daily totals per person in minutes, by type, by grade
            StringBuilder sqlDailyTotals = new StringBuilder();
            sqlDailyTotals.Append("select ");
            sqlDailyTotals.Append(Environment.NewLine);
            sqlDailyTotals.Append("u.userGrade");
            sqlDailyTotals.Append(Environment.NewLine);
            sqlDailyTotals.Append(", u.userLastName");
            sqlDailyTotals.Append(Environment.NewLine);
            sqlDailyTotals.Append(", u.userFirstName");
            sqlDailyTotals.Append(Environment.NewLine);
            sqlDailyTotals.Append(",e.signinMonth");
            sqlDailyTotals.Append(Environment.NewLine);
            sqlDailyTotals.Append(",e.signinDay");
            sqlDailyTotals.Append(Environment.NewLine);
            sqlDailyTotals.Append(", e.signinYear");
            sqlDailyTotals.Append(Environment.NewLine);
            sqlDailyTotals.Append(", e.signinType");
            sqlDailyTotals.Append(Environment.NewLine);
            sqlDailyTotals.Append(", sum(signoutUnixTime - signinUnixTime)/60 as minutes ");
            sqlDailyTotals.Append(Environment.NewLine);
            sqlDailyTotals.Append("from ");
            sqlDailyTotals.Append(Environment.NewLine);
            sqlDailyTotals.Append("TimePunchEvents as e ");
            sqlDailyTotals.Append(Environment.NewLine);
            sqlDailyTotals.Append("inner join ");
            sqlDailyTotals.Append(Environment.NewLine);
            sqlDailyTotals.Append("TimePunchUserInfo as u on e.userIdentity = u.userIdentity ");
            sqlDailyTotals.Append(Environment.NewLine);
            sqlDailyTotals.Append("group by ");
            sqlDailyTotals.Append(Environment.NewLine);
            sqlDailyTotals.Append("u.userGrade");
            sqlDailyTotals.Append(Environment.NewLine);
            sqlDailyTotals.Append(",e.userIdentity");
            sqlDailyTotals.Append(Environment.NewLine);
            sqlDailyTotals.Append(", e.signinYear");
            sqlDailyTotals.Append(Environment.NewLine);
            sqlDailyTotals.Append(",e.signinMonth");
            sqlDailyTotals.Append(Environment.NewLine);
            sqlDailyTotals.Append(",e.signinDay");
            sqlDailyTotals.Append(Environment.NewLine);
            sqlDailyTotals.Append(", e.signinType ");
            sqlDailyTotals.Append(Environment.NewLine);
            sqlDailyTotals.Append("order by ");
            sqlDailyTotals.Append(Environment.NewLine);
            sqlDailyTotals.Append("u.userGrade");
            sqlDailyTotals.Append(Environment.NewLine);
            sqlDailyTotals.Append(",e.signinYear");
            sqlDailyTotals.Append(Environment.NewLine);
            sqlDailyTotals.Append(",e.signinMonth");
            sqlDailyTotals.Append(Environment.NewLine);
            sqlDailyTotals.Append(",e.signinDay");
            sqlDailyTotals.Append(Environment.NewLine);
            sqlDailyTotals.Append(",u.userLastName");
            sqlDailyTotals.Append(Environment.NewLine);
            sqlDailyTotals.Append(", u.userFirstName");
            sqlDailyTotals.Append(Environment.NewLine);
            sqlDailyTotals.Append(", e.signinType");
            sqlDailyTotals.Append(Environment.NewLine);

            //daily totals per person in minutes, monthly totals per person in hours - by type, by grade
            StringBuilder sqlMonthlyTotals = new StringBuilder();
            sqlMonthlyTotals.Append("select ");
            sqlMonthlyTotals.Append(Environment.NewLine);
            sqlMonthlyTotals.Append("u.userGrade");
            sqlMonthlyTotals.Append(Environment.NewLine);
            sqlMonthlyTotals.Append(", u.userLastName");
            sqlMonthlyTotals.Append(Environment.NewLine);
            sqlMonthlyTotals.Append(", u.userFirstName");
            sqlMonthlyTotals.Append(Environment.NewLine);
            sqlMonthlyTotals.Append(",e.signinMonth");
            sqlMonthlyTotals.Append(Environment.NewLine);
            sqlMonthlyTotals.Append(", e.signinYear");
            sqlMonthlyTotals.Append(Environment.NewLine);
            sqlMonthlyTotals.Append(", e.signinType");
            sqlMonthlyTotals.Append(Environment.NewLine);
            sqlMonthlyTotals.Append(", sum(signoutUnixTime - signinUnixTime)/(60*60) as hours ");
            sqlMonthlyTotals.Append(Environment.NewLine);
            sqlMonthlyTotals.Append("from ");
            sqlMonthlyTotals.Append(Environment.NewLine);
            sqlMonthlyTotals.Append("TimePunchEvents as e ");
            sqlMonthlyTotals.Append(Environment.NewLine);
            sqlMonthlyTotals.Append("inner join ");
            sqlMonthlyTotals.Append(Environment.NewLine);
            sqlMonthlyTotals.Append("TimePunchUserInfo as u on e.userIdentity = u.userIdentity ");
            sqlMonthlyTotals.Append(Environment.NewLine);
            sqlMonthlyTotals.Append("group by ");
            sqlMonthlyTotals.Append(Environment.NewLine);
            sqlMonthlyTotals.Append("u.userGrade");
            sqlMonthlyTotals.Append(Environment.NewLine);
            sqlMonthlyTotals.Append(",e.userIdentity");
            sqlMonthlyTotals.Append(Environment.NewLine);
            sqlMonthlyTotals.Append(", e.signinYear");
            sqlMonthlyTotals.Append(Environment.NewLine);
            sqlMonthlyTotals.Append(",e.signinMonth");
            sqlMonthlyTotals.Append(Environment.NewLine);
            sqlMonthlyTotals.Append(", e.signinType ");
            sqlMonthlyTotals.Append(Environment.NewLine);
            sqlMonthlyTotals.Append("order by ");
            sqlMonthlyTotals.Append(Environment.NewLine);
            sqlMonthlyTotals.Append("u.userGrade");
            sqlMonthlyTotals.Append(Environment.NewLine);
            sqlMonthlyTotals.Append(",e.signinYear");
            sqlMonthlyTotals.Append(Environment.NewLine);
            sqlMonthlyTotals.Append(",e.signinMonth");
            sqlMonthlyTotals.Append(Environment.NewLine);
            sqlMonthlyTotals.Append(",u.userLastName");
            sqlMonthlyTotals.Append(Environment.NewLine);
            sqlMonthlyTotals.Append(", u.userFirstName");
            sqlMonthlyTotals.Append(Environment.NewLine);
            sqlMonthlyTotals.Append(", e.signinType");
            sqlMonthlyTotals.Append(Environment.NewLine);

            //daily totals per person in minutes, monthly totals per person in hours - by type, by grade
            ListViewItem xx = new ListViewItem();
            xx.Tag = sqlDailyTotals.ToString();
            xx.Text = "Daily Totals";
            cboDataDump.Items.Add(xx);

            //daily totals per person in minutes, monthly totals per person in hours - by type, by grade
            xx = new ListViewItem();
            xx.Tag = sqlMonthlyTotals;
            xx.Text = "Monthly Totals";
            cboDataDump.Items.Add(xx);


            // this is the custom one
            ListViewItem itmCustom = new ListViewItem();
            itmCustom.Tag = "<Enter Query SQL Here>";
            itmCustom.Text = "Custom...";
            cboDataDump.Items.Add(itmCustom);

            //TimePunchEvents
            xx = new ListViewItem();
            xx.Tag = "Select * from TimePunchEvents";
            xx.Text = "Events";
            cboDataDump.Items.Add(xx);

            //TimePunchUserIdentities
            xx = new ListViewItem();
            xx.Tag = "Select * from TimePunchUserIdentities";
            xx.Text = "User IDs";
            cboDataDump.Items.Add(xx);

            //TimePunchUserInfo
            xx = new ListViewItem();
            xx.Tag = "Select * from TimePunchUserInfo";
            xx.Text = "User Info";
            cboDataDump.Items.Add(xx);

            //TimePunchDBVersion
            xx = new ListViewItem();
            xx.Tag = "Select * from TimePunchDBVersion";
            xx.Text = "DB Version";
            cboDataDump.Items.Add(xx);

            if (cboDataDump.Items.Count > 0)
            {
                cboDataDump.SelectedIndex = 0;
                cboDataDump.DisplayMember = "Text";
                cboDataDump.ValueMember = "Tag";
            }
        }

        private void btnDBBackup_Click(object sender, EventArgs e)
        {
            log.Debug("IN");

            try
            {
                connectToDatabase();

                string dbFileNameAndPath = m_dbConnection.FileName;
                string fileName = dbFileNameAndPath.Substring(dbFileNameAndPath.LastIndexOf("\\") + 1);
                string sourcePath = dbFileNameAndPath.Replace("\\" + ConfigurationManager.AppSettings["dbFileName"].ToString(), "");

                m_dbConnection.Close();
                m_dbConnection = null;

                //string fileName = "test.txt";
                //string sourcePath = @"C:\Users\Public\TestFolder";
                string targetPath = @txtDBBackup.Text;
                string targetFileName = DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss_") + fileName;

                // Use Path class to manipulate file and directory paths.
                string sourceFile = System.IO.Path.Combine(sourcePath, fileName);
                string destFile = System.IO.Path.Combine(targetPath, targetFileName);

                // To copy a folder's contents to a new location:
                // Create a new target folder, if necessary.
                if (!System.IO.Directory.Exists(targetPath))
                {
                    System.IO.Directory.CreateDirectory(targetPath);
                }

                // To copy a file to another location and 
                // overwrite the destination file if it already exists.
                System.IO.File.Copy(sourceFile, destFile, true);

                MessageBox.Show("Database Copied", "Info");

                connectToDatabase();

                //string exePath = Application.ExecutablePath;
                //string appPath = Application.StartupPath;

            }
            catch (Exception ex)
            {
                log.Error("Error", ex);
                MessageBox.Show(ex.Message, "Error - " + System.Reflection.MethodBase.GetCurrentMethod().Name);
            }



        }

        private void btnLogBackup_Click(object sender, EventArgs e)
        {
            log.Debug("IN");

            try
            {

                string targetPath = @txtLogBackup.Text;

                // To copy a folder's contents to a new location:
                // Create a new target folder, if necessary.
                if (!System.IO.Directory.Exists(targetPath))
                {
                    System.IO.Directory.CreateDirectory(targetPath);
                }

                // get all of the log files
                string appPath = Application.StartupPath;
                String[] files = System.IO.Directory.GetFiles(appPath, "*.log");

                // loop through the files and copy them
                foreach (string file in files)
                {
                    // parse out just the file name
                    string fName = file.Substring(appPath.Length + 1);

                    string destfile = System.IO.Path.Combine(targetPath, fName);
                    System.IO.File.Copy(file, destfile, true);
                }

                MessageBox.Show("Log Files Copied", "Info");
            }
            catch (Exception ex)
            {
                log.Error("Error", ex);
                MessageBox.Show(ex.Message, "Error - " + System.Reflection.MethodBase.GetCurrentMethod().Name);
            }

        }

        private void btnClockOutAll_Click(object sender, EventArgs e)
        {
            log.Debug("IN");

            try
            {
                Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                string signOutComputer = System.Environment.MachineName;

                string sql3 = "update TimePunchEvents set" + 
                " signoutUnixTime = " + unixTimestamp.ToString() + ", " +
                " signoutComputer = '" + signOutComputer + "', " +
                " updateUnixTimeStamp = " + unixTimestamp.ToString() + " " +
                " where signoutUnixTime = signinUnixTime " +  
                " and signinYear = " + dtClockOutAll.Value.Year.ToString() +
                " and signinMonth = " + dtClockOutAll.Value.Month.ToString() +
                " and signinDay = " + dtClockOutAll.Value.Day.ToString();

                SQLiteCommand command3 = new SQLiteCommand(sql3, m_dbConnection);
                log.Info("SQL: " + sql3.Replace(Environment.NewLine, " "));
                command3.ExecuteNonQuery();

                MessageBox.Show("All Users signed out for selected date using the current date and time.", "Info");

                // update the grid since we made updates
                updateClockOutGrid();

            }
            catch (Exception ex)
            {
                log.Error("Error", ex);
                MessageBox.Show(ex.Message, "Error - " + System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        private void dtClockOutAll_ValueChanged(object sender, EventArgs e)
        {

            log.Debug("IN");

            try
            {
                updateClockOutGrid();
                //MessageBox.Show("Date Changed!", "Info");
            }
            catch (Exception ex)
            {
                log.Error("Error", ex);
                MessageBox.Show(ex.Message, "Error - " + System.Reflection.MethodBase.GetCurrentMethod().Name);
            }

        }
        private void updateClockOutGrid()
        {
            log.Debug("IN");

            try
            {

                StringBuilder sqlDailyTotals = new StringBuilder();
                sqlDailyTotals.Append("select ");
                sqlDailyTotals.Append(Environment.NewLine);
                sqlDailyTotals.Append("u.userGrade");
                sqlDailyTotals.Append(Environment.NewLine);
                sqlDailyTotals.Append(", u.userLastName");
                sqlDailyTotals.Append(Environment.NewLine);
                sqlDailyTotals.Append(", u.userFirstName");
                sqlDailyTotals.Append(Environment.NewLine);
                sqlDailyTotals.Append(",e.signinMonth");
                sqlDailyTotals.Append(Environment.NewLine);
                sqlDailyTotals.Append(",e.signinDay");
                sqlDailyTotals.Append(Environment.NewLine);
                sqlDailyTotals.Append(", e.signinYear");
                sqlDailyTotals.Append(Environment.NewLine);
                sqlDailyTotals.Append(", e.signinType");
                sqlDailyTotals.Append(Environment.NewLine);
                sqlDailyTotals.Append("from ");
                sqlDailyTotals.Append(Environment.NewLine);
                sqlDailyTotals.Append("TimePunchEvents as e ");
                sqlDailyTotals.Append(Environment.NewLine);
                sqlDailyTotals.Append("inner join ");
                sqlDailyTotals.Append(Environment.NewLine);
                sqlDailyTotals.Append("TimePunchUserInfo as u on e.userIdentity = u.userIdentity ");
                sqlDailyTotals.Append(Environment.NewLine);

                sqlDailyTotals.Append(" where e.signoutUnixTime = e.signinUnixTime ");
                sqlDailyTotals.Append(" and e.signinYear = " + dtClockOutAll.Value.Year.ToString());
                sqlDailyTotals.Append(" and e.signinMonth = " + dtClockOutAll.Value.Month.ToString());
                sqlDailyTotals.Append(" and e.signinDay = " + dtClockOutAll.Value.Day.ToString());

                sqlDailyTotals.Append(" order by ");
                sqlDailyTotals.Append(Environment.NewLine);
                sqlDailyTotals.Append("u.userGrade");
                sqlDailyTotals.Append(Environment.NewLine);
                sqlDailyTotals.Append(",e.signinYear");
                sqlDailyTotals.Append(Environment.NewLine);
                sqlDailyTotals.Append(",e.signinMonth");
                sqlDailyTotals.Append(Environment.NewLine);
                sqlDailyTotals.Append(",e.signinDay");
                sqlDailyTotals.Append(Environment.NewLine);
                sqlDailyTotals.Append(",u.userLastName");
                sqlDailyTotals.Append(Environment.NewLine);
                sqlDailyTotals.Append(", u.userFirstName");
                sqlDailyTotals.Append(Environment.NewLine);
                sqlDailyTotals.Append(", e.signinType");
                sqlDailyTotals.Append(Environment.NewLine);

                string sql = sqlDailyTotals.ToString();

                /*
                string sql = "select * from TimePunchEvents " +
                " where signoutUnixTime = signinUnixTime " +
                " and signinYear = " + dtClockOutAll.Value.Year.ToString() +
                " and signinMonth = " + dtClockOutAll.Value.Month.ToString() +
                " and signinDay = " + dtClockOutAll.Value.Day.ToString();
                */

                bool showDates = true; // chkIncludeReadableDates.Checked;

                StringBuilder output = new StringBuilder();
                StringBuilder headerRow = new StringBuilder();
                StringBuilder sqlStar = new StringBuilder();

                // Make a connection to the database if it hasnt already
                connectToDatabase();

                SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
                SQLiteDataReader reader = command.ExecuteReader();
                int rowCount = 0;
                while (reader.Read())
                {
                    int columnCount = reader.FieldCount;

                    for (int i = 0; i < columnCount; i++)
                    {
                        string fieldName = reader.GetName(i);
                        // create the header if this is the first row
                        if (rowCount == 0)
                        {
                            if (i != 0)
                            {
                                headerRow.Append(",");
                                sqlStar.Append(",");
                            }
                            headerRow.Append(fieldName.Replace(",", ""));
                            sqlStar.Append(fieldName.Replace(",", ""));
                            if (fieldName.IndexOf("UnixTime") > 0 && showDates)
                            {
                                headerRow.Append(",");
                                sqlStar.Append(",");
                                headerRow.Append(fieldName.Replace(",", "") + "_ToDate");
                                sqlStar.Append(" datetime(" + fieldName.Replace(",", "") + ", 'unixepoch', 'localtime') as dt" + fieldName.Replace(",", "").Replace("UnixTime", ""));
                            }
                        }

                        // row data
                        if (i != 0)
                        {
                            output.Append(",");
                        }
                        output.Append(reader[i].ToString());
                        if (fieldName.IndexOf("UnixTime") > 0 && showDates)
                        {
                            output.Append(",");
                            output.Append(convertUnixDateTimeToDisplayDateTime(reader[i].ToString()));
                        }
                    }
                    output.Append(Environment.NewLine);
                    rowCount += 1;
                }
                reader.Close();

                headerRow.Append(Environment.NewLine);

                string strQuery = sql.Replace(" * ", " " + sqlStar.ToString() + " ");
                string strResults = headerRow.ToString() + output.ToString();

                if (strResults.Replace(Environment.NewLine, "") == "")
                {
                    strResults = "No Data Found";
                }

                //now fill the grid from the text data
                grdClockOut.Columns.Clear();
                if (strResults.Replace(Environment.NewLine, "") != "")
                {
                    string[] rows = strResults.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
                    int rowIndex = 0;
                    foreach (string row in rows)
                    {
                        if (rowIndex == 0)
                        {
                            // column header row
                            string[] columns = row.Split(new string[] { "," }, StringSplitOptions.None);
                            foreach (string column in columns)
                            {
                                grdClockOut.Columns.Add(column, column);
                            }
                        }
                        else
                        {
                            // data row
                            DataGridViewRow gridrow = new DataGridViewRow();

                            string[] fields = row.Split(new string[] { "," }, StringSplitOptions.None);
                            foreach (string field in fields)
                            {
                                DataGridViewTextBoxCell cell = new DataGridViewTextBoxCell();
                                cell.Value = field;
                                gridrow.Cells.Add(cell);
                            }


                            grdClockOut.Rows.Add(gridrow);
                        }

                        rowIndex += 1;
                    }
                }

            }
            catch (Exception ex)
            {
                log.Error("Error", ex);
                MessageBox.Show(ex.Message, "Error - " + System.Reflection.MethodBase.GetCurrentMethod().Name);
            }

        }



    }
}
