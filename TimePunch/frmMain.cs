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

//DB Browswer
//http://sqlitebrowser.org/

//SQLite Studio
//https://sqlitestudio.pl/index.rvt

//SQLite tutorials
//http://blog.tigrangasparian.com/2012/02/09/getting-started-with-sqlite-in-c-part-one/
//https://www.youtube.com/watch?v=TrJcKHMe6Y8
//https://code.msdn.microsoft.com/windowsapps/How-to-use-SQLite-with-96f22fac
//https://www.codeproject.com/Tips/1057992/Using-SQLite-An-Example-of-CRUD-Operations-in-Csha

//Date time info
//https://www.sqlite.org/lang_datefunc.html

namespace TimePunch
{
    public partial class frmMain : Form
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private string connectionString = "Data Source=" + ConfigurationManager.AppSettings["dbFileName"].ToString() + ";Version=3;";
        SQLiteConnection m_dbConnection;

        private bool skipPassword = false;

        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            log.Debug("IN");

            try
            {
                defaultUIElements();

                //Load app version
                loadAppVersion();

                //Load db version
                loadDBVersion();

                // set the icon and the form title
                try
                {
                    string formTitle = ConfigurationManager.AppSettings["FormTitle"].ToString();
                    string formIcon = ConfigurationManager.AppSettings["FormIcon"].ToString();
                    this.Text = formTitle;
                    this.Icon = new Icon(formIcon);
                }
                catch (Exception ex)
                {
                    log.Error("Error", ex);
                    // dont care enough about this error to make it show an error message
                    //MessageBox.Show(ex.Message, "Error - " + System.Reflection.MethodBase.GetCurrentMethod().Name);
                }

            }
            catch (Exception ex)
            {
                log.Error("Error", ex);
                MessageBox.Show(ex.Message, "Error - " + System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        void loadAppVersion()
        {
            log.Debug("IN");
            try
            {
                Globals.AppVersion = Application.ProductVersion;
                log.Info("AppVersion=" + Globals.AppVersion);
            }
            catch (Exception ex)
            {
            }
        }
        void loadDBVersion()
        {
            log.Debug("IN");
            // Make a connection to the database if it hasnt already
            // this wont fail, even if it doesnt exist.  it will create the db file
            connectToDatabase();
            try
            {
                string sql = "Select * from TimePunchDBVersion";

                SQLiteCommand command1 = new SQLiteCommand(sql, m_dbConnection);
                SQLiteDataReader reader1 = command1.ExecuteReader();

                while (reader1.Read())
                {
                    Globals.DBVersion = reader1["dbVersion"].ToString();
                    log.Info("DBVersion=" + Globals.DBVersion);
                }
                reader1.Close();

            }
            catch (Exception ex)
            {
            }
        }
        void connectToDatabase()
        {
            log.Debug("IN");
            if (m_dbConnection == null)
            {
                log.Debug("Database Connection: Attempt: [" + connectionString + "]");
                m_dbConnection = new SQLiteConnection(connectionString);
                m_dbConnection.Open();
                log.Debug("Database Connection: Success: [" + connectionString + "]");
            }
        }

        Int32 normalPunchOUT(string userIdentity, string signinType)
        {
            log.Debug("IN");

            // grab a timestamp
            Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

            // grab the last punch in
            // Get the last punch
            string sql = "select * from TimePunchEvents where userIdentity = '" + userIdentity + "' and signinType = '" + signinType + "' and manualPunch = 0 order by createUnixTimeStamp desc";

            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            StringBuilder output = new StringBuilder();

            bool found = false;
            string signInTime = "";
            string signOutTime = "";

            while (reader.Read() && !found)
            {
                signInTime = reader["signinUnixTime"].ToString();
                signOutTime = reader["signoutUnixTime"].ToString();
                found = true;
            }
            reader.Close();

            // logic here is if sign out and in time are equal, they have not yet signed out
            if (found && signOutTime == signInTime)
            {
                //dont need to do anything here, we have the data we need from above
            }
            else
            {
                // throw an error here, you shouldnt have the ability to sign out if there is no sign in
                MessageBox.Show("No valid sign in found", "Info");
                return 0;
            }

            // do the punch out
            string adminIdentity = "";
            signOutTime = unixTimestamp.ToString();
            string signOutComputer = System.Environment.MachineName;

            sql = "update TimePunchEvents set" +
                " adminIdentity = '" + adminIdentity + "', " +
                " signoutUnixTime = " + signOutTime.ToString() + ", " +
                " signoutYear = " + convertUnixDateTimeToYear(signOutTime.ToString()) + ", " +
                " signoutMonth = " + convertUnixDateTimeToMonth(signOutTime.ToString()) + ", " +
                " signoutDay = " + convertUnixDateTimeToDay(signOutTime.ToString()) + ", " +
                " signoutComputer = '" + signOutComputer + "', " +
                " updateUnixTimeStamp = " + unixTimestamp.ToString() + " " +
                " where userIdentity = '" + userIdentity + "' " +
                " and signinUnixTime = " + signInTime +
                " and signinType = '" + signinType + "'";

            SQLiteCommand command2 = new SQLiteCommand(sql, m_dbConnection);
            log.Info("SQL: " + sql.Replace(Environment.NewLine, " "));
            command2.ExecuteNonQuery();

            return unixTimestamp;

        }

        Int32 normalPunchIN(string userIdentity, string signinType)
        {
            log.Debug("IN");

            // grab a timestamp
            Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

            string adminIdentity = "";
            Int32 nowStamp = unixTimestamp;
            Int32 signInTime = nowStamp;
            Int32 signOutTime = nowStamp;
            //get signInComputer from somewhere
            string signInComputer = System.Environment.MachineName; 
            string signOutComputer = System.Environment.MachineName;
            string sql = "insert into TimePunchEvents" +
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
                " '" + userIdentity + "', " +
                " '" + adminIdentity + "', " +
                " 0, " +
                signInTime.ToString() + ", " + 
                signOutTime.ToString() + ", " +
                convertUnixDateTimeToYear(signInTime.ToString()) + ", " +
                convertUnixDateTimeToMonth(signInTime.ToString()) + ", " +
                convertUnixDateTimeToDay(signInTime.ToString()) + ", " +
                convertUnixDateTimeToYear(signOutTime.ToString()) + ", " +
                convertUnixDateTimeToMonth(signOutTime.ToString()) + ", " +
                convertUnixDateTimeToDay(signOutTime.ToString()) + ", " +
                " '" + signinType + "', " +
                " '" + signInComputer + "', " +
                " '" + signOutComputer + "', " +
                nowStamp.ToString() + ", " + 
                nowStamp.ToString() +
                " ) ";

            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            log.Info("SQL: " + sql.Replace(Environment.NewLine, " "));
            command.ExecuteNonQuery();
            return signInTime;

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            log.Debug("IN");

            try
            {

                // Login
                string userIdentity = txtUserID.Text;
                string userPassword = txtPassword.Text;
               
                // Make a connection to the database if it hasnt already
                // this wont fail, even if it doesnt exist.  it will create the db file
                connectToDatabase();

                // wrap up the login call in an exception handler in case the identity table doesnt exist
                // and we know we need to create it
                bool foundLogin = false;
                string dbUserID = "";
                string dbUserPassword = "";
                int dbAdminFlag = 0;

                try
                {
                    string sqlLogin = "select * from TimePunchUserIdentities where userIdentity = '" + userIdentity + "' order by createUnixTimeStamp desc";

                    SQLiteCommand command1 = new SQLiteCommand(sqlLogin, m_dbConnection);
                    SQLiteDataReader reader1 = command1.ExecuteReader();

                    while (reader1.Read() && !foundLogin)
                    {
                        dbUserID = reader1["userIdentity"].ToString();
                        dbUserPassword = reader1["userPassword"].ToString();
                        // set the local password to the real password if we are skipping, so they will match
                        if (skipPassword)
                        {
                            userPassword = dbUserPassword;
                        }
                        dbAdminFlag = int.Parse(reader1["isAdmin"].ToString());
                        foundLogin = true;
                    }
                    reader1.Close();

                }
                catch (Exception ex)
                {
                    // show the admin screen
                    if (userIdentity == ConfigurationManager.AppSettings["AdminUserID"].ToString() && userPassword == ConfigurationManager.AppSettings["AdminDefaultUserPassword"].ToString())
                    {
                        // close the database connection
                        try
                        {
                            m_dbConnection.Close();
                        }
                        catch
                        { }
                        m_dbConnection = null;

                        // this is a first time setup, so show the admin screen
                        var adminForm = new frmAdmin();
                        // assign db variables so we dont have them duplicated
                        adminForm.AdminUserID = userIdentity;
                        adminForm.AdminUserPassword = userPassword;

                        adminForm.ShowDialog(this);

                        // reset the form
                        defaultUIElements();
                        txtUserID.Text = "";
                        txtUserID.Tag = txtUserID.Text;
                        txtPassword.Text = "";

                    }
                    else
                    {
                        // this is unexpected, probably need to show a message
                        log.Error("Error", ex);
                        MessageBox.Show(ex.Message, "Error - " + System.Reflection.MethodBase.GetCurrentMethod().Name);
                    }
                    return;
                }

                //validate credentials and set isadmin flag - case sensitive 
                if (!foundLogin ||(userIdentity != dbUserID || userPassword != dbUserPassword))
                {
                    // invalid login, kick them out
                    MessageBox.Show("Invalid Login", "Info");
                    return;
                }

                mnuAdmin.Visible = (dbAdminFlag == 1);
                btnLogOut.Visible = true;
                btnLogin.Visible = false;

                if (dbAdminFlag == 1)
                {

                    // this is a first time setup, so show the admin screen
                    var adminForm = new frmAdmin();
                    //assign db variables so we dont have them duplicated
                    adminForm.AdminUserID = userIdentity;
                    adminForm.AdminUserPassword = userPassword;

                    adminForm.ShowDialog(this);


                }
                else
                {
                    doSmartPunch(userIdentity);

                }

                // reset the form
                logOut();

            }
            catch (Exception ex)
            {
                log.Error("Error", ex);
                MessageBox.Show(ex.Message, "Error - " + System.Reflection.MethodBase.GetCurrentMethod().Name);
            }

        }

        private void doSmartPunch(string userIdentity)
        {
            txtUserID.Tag = userIdentity;

            //determine which kind of punch this is
            string signinType = "";

            // See if there are any clocked in records, so we can clock them out.
            // logic here is if sign out and in time are equal, they have not yet clocked out
            string sql = "select * from TimePunchEvents where signinUnixTime = signoutUnixTime and userIdentity = '" + userIdentity + "' and manualPunch = 0  order by createUnixTimeStamp desc";

            SQLiteCommand command2 = new SQLiteCommand(sql, m_dbConnection);
            SQLiteDataReader reader2 = command2.ExecuteReader();
            //StringBuilder output = new StringBuilder();
            List<string> signOuts = new List<string>();

            while (reader2.Read())
            {
                signinType = reader2["signinType"].ToString();
                // clock them out
                normalPunchOUT(userIdentity, signinType);
                // keep track of this
                signOuts.Add(signinType);
            }
            reader2.Close();
            signinType = "";

            if(signOuts.Count == 0)
            {
                // if there have been no clock outs done to this point, then clock them in
                // Get all of the clockin types
                List<string> signInTypes = new List<string>();
                for (int i = 0; i < ConfigurationManager.AppSettings.Keys.Count; i++)
                {
                    string keyName = ConfigurationManager.AppSettings.Keys[i].ToString();
                    if (keyName.Contains("SignInType_"))
                    {
                        string sss = ConfigurationManager.AppSettings[keyName].ToString();
                        // parse out the name from the time, we only need the name
                        //string[] vals = sss.Split(new string[] { "," }, StringSplitOptions.None);
                        signInTypes.Add(sss);
                    }
                }

                // if there are more then one then we need them to choose
                if(signInTypes.Count > 1)
                {
                    var signInPicker = new frmSignInTypePicker();
                    signInPicker.signInTypes = signInTypes;
                    signInPicker.userIdentity = userIdentity;

                    signInPicker.ShowDialog(this);

                    // grab the sign in type from the form
                    signinType = signInPicker.selectedSignInType;

                    if(signinType == "")
                    {
                        //notify the user they canceled from picker
                        txtResults.Text = userIdentity + " cancelled login." + Environment.NewLine + txtResults.Text;
                    }
                }
                else
                {
                    signinType = signInTypes[0];
                }

                // finally, punch them in if we have a signin type
                if(signinType != "")
                {
                    normalPunchIN(userIdentity, signinType);
                    // notify user the sign in happened
                    txtResults.Text = userIdentity + " clocked in for " + signinType +"." + Environment.NewLine + txtResults.Text;

                }
            }
            else
            {
                // notify the user that the signout(s) happened
                foreach(string signout in signOuts)
                {
                    txtResults.Text = userIdentity + " clocked out for " + signout + "." + Environment.NewLine + txtResults.Text;
                }
            }

        }

        private void logOut()
        {
            log.Debug("IN");

            txtUserID.Text = "";
            txtUserID.Tag = txtUserID.Text;
            txtPassword.Text = "";
            defaultUIElements();
        }

        private void defaultUIElements()
        {
            log.Debug("IN");

            mnuAdmin.Visible = false;
            //timer.Enabled = false;
            //lblPunchIn.Visible = false;
            //lblPunchOut.Visible = false;
            //lblLastFullPunch.Visible = false;
            //btnPunchIn.Enabled = false;
            //btnPunchOut.Enabled = false;
            //lblPunchIn.Text = "";
            //lblPunchOut.Text = "";
            //lblLastFullPunch.Text = "";
            //rdLab.Checked = true;
            //rdTheory.Checked = false;
            btnLogin.Visible = true;
            btnLogOut.Visible = false;
            //grpType.Enabled = btnLogin.Visible;
            //rdLab.Enabled = btnLogin.Visible;
            //rdTheory.Enabled = btnLogin.Visible;
            skipPassword = false;
        }


        private string convertUnixDateTimeToDisplayDateTime(string unixDateTime)
        {
            log.Debug("IN");

            // Unix timestamp is seconds past epoch
            double unix = double.Parse(unixDateTime);
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unix).ToLocalTime();

            string result =  dtDateTime.ToShortDateString() + " " + dtDateTime.ToShortTimeString();

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

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            log.Debug("IN");

            try
            {
                m_dbConnection.Close();
            }
            catch (Exception ex)
            {
                log.Error("Error", ex);
                //dont care
                //MessageBox.Show(ex.Message, "Error - " + System.Reflection.MethodBase.GetCurrentMethod().Name);
            }

        }


        private void mnuNewUser_Click(object sender, EventArgs e)
        {
            log.Debug("IN");

            try
            {


                // close the database connection
                try
                {
                    m_dbConnection.Close();
                }
                catch
                { }
                m_dbConnection = null;

                // this is a first time setup, so show the admin screen
                var newUserForm = new frmUserInfo();
                newUserForm.isUpdate = false;

                newUserForm.ShowDialog(this);

                // reset the form
                defaultUIElements();

                string xxx = newUserForm.userIDForForm;

                txtUserID.Text = xxx;
                txtUserID.Tag = txtUserID.Text;
                txtPassword.Select();
            }
            catch (Exception ex)
            {
                log.Error("Error", ex);
                MessageBox.Show(ex.Message, "Error - " + System.Reflection.MethodBase.GetCurrentMethod().Name);
            }

        }

        private void mnuAdmin_Click(object sender, EventArgs e)
        {
            try
            {
                // close the database connection
                try
                {
                    m_dbConnection.Close();
                }
                catch
                { }
                m_dbConnection = null;

                // this is a first time setup, so show the admin screen
                var adminForm = new frmAdmin();
                adminForm.AdminUserID = ConfigurationManager.AppSettings["AdminUserID"].ToString();
                adminForm.ShowDialog(this);

                // reset the form
                defaultUIElements();
                txtUserID.Text = "";
                txtUserID.Tag = txtUserID.Text;
                txtPassword.Text = "";

            }
            catch (Exception ex)
            {
                log.Error("Error", ex);
                MessageBox.Show(ex.Message, "Error - " + System.Reflection.MethodBase.GetCurrentMethod().Name);
            }

        }

        private void btnFingerprintLogin_Click(object sender, EventArgs e)
        {
            log.Debug("IN");

            try
            {

                // this is a first time setup, so show the admin screen
                var fingerprintForm = new frmFingerPrintLogin();

                fingerprintForm.ShowDialog(this);

                // grab the user from the form
                string userID = fingerprintForm.userIDForForm;

                // auto log them in
                if(userID != "")
                {
                    log.Info("Fingerprint login for : " + userID);
                    txtUserID.Text = userID;
                    txtUserID.Tag = txtUserID.Text;
                    txtPassword.Text = "";
                    skipPassword = true;
                    btnLogin.PerformClick();
                    skipPassword = false;
                }
            }
            catch (Exception ex)
            {
                log.Error("Error", ex);
                MessageBox.Show(ex.Message, "Error - " + System.Reflection.MethodBase.GetCurrentMethod().Name);
            }

        }

        private void btnTouchLogin_Click(object sender, EventArgs e)
        {
            log.Debug("IN");

            try
            {

                // this is a first time setup, so show the admin screen
                var touchForm = new frmTouchLogin();

                touchForm.ShowDialog(this);

                // grab the user from the form
                string userID = touchForm.userIDForForm;
                string userPWD = touchForm.userPWDForForm;
                
                //auto log them in
                if (userID != "")
                {
                    log.Info("Touch login for : " + userID);
                    txtUserID.Text = userID;
                    txtUserID.Tag = txtUserID.Text;
                    txtPassword.Text = userPWD;
                    skipPassword = false;
                    btnLogin.PerformClick();
                    skipPassword = false;
                }
            }
            catch (Exception ex)
            {
                log.Error("Error", ex);
                MessageBox.Show(ex.Message, "Error - " + System.Reflection.MethodBase.GetCurrentMethod().Name);
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string tableName = "ReportWeeklySummary";
            StringBuilder sql = new StringBuilder();

            connectToDatabase();

            createSummaryTable();

            // get the types
            List<string> signInTypes = new List<string>();
            for (int i = 0; i < ConfigurationManager.AppSettings.Keys.Count; i++)
            {
                string keyName = ConfigurationManager.AppSettings.Keys[i].ToString();
                if (keyName.Contains("SignInType_"))
                {
                    string sss = ConfigurationManager.AppSettings[keyName].ToString();
                    signInTypes.Add(sss);
                }
            }

            // get all of the date fields
            List<string> dateColumns = new List<string>();

            sql.Clear();
            sql.AppendLine(" SELECT * ");
            sql.AppendLine(" FROM " + tableName);

            SQLiteCommand command = new SQLiteCommand(sql.ToString(), m_dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();

            for (int i = 0; i < reader.FieldCount - 1; i++)
            {
                string columnName = reader.GetName(i);
                int n;
                bool isNumeric = int.TryParse(columnName.Substring(1, 1), out n);
                if (isNumeric)
                {
                    // store off the date columns
                    dateColumns.Add(columnName);
                }

            }
            reader.Close();

            // get all of the users
            List<string> users = new List<string>();

            sql.Clear();
            sql.AppendLine("SELECT userIdentity ");
            sql.AppendLine("FROM TimePunchUserIdentities ");
            sql.AppendLine("WHERE isAdmin = 0 ");
            SQLiteCommand command2 = new SQLiteCommand(sql.ToString(), m_dbConnection);
            SQLiteDataReader reader1 = command2.ExecuteReader();
            while (reader1.Read())
            {
                string userID = reader1["userIdentity"].ToString();
                users.Add(userID);
            }
            reader1.Close();


            // grab the column names and parse out the dates from the date columns
            // and create the update statements
            List<string> sqlUpdateQueries = new List<string>();

            foreach (string columnName in dateColumns)
            {

                // parse out the dates
                string[] dates = columnName.Replace("w", "").Split(new string[] { "-" }, StringSplitOptions.None);
                DateTime startdate = DateTime.Parse(dates[0]);
                DateTime enddate = DateTime.Parse(dates[1]);
                Int32 unixTimestampStartDate = (Int32)(startdate.ToUniversalTime().Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                Int32 unixTimestampEndDate = (Int32)(enddate.ToUniversalTime().Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

                // Left off here - should we do this all in the insert
                // or do this in an update?  if update, we are repeating the loop to get users and punch types
                StringBuilder sqlSelectQueryForUpdate = new StringBuilder();
                sqlSelectQueryForUpdate.AppendLine("select ");
                sqlSelectQueryForUpdate.AppendLine("round(sum(signoutUnixTime - signinUnixTime)/cast((60*60) as float),2) hours ");
                sqlSelectQueryForUpdate.AppendLine("from TimePunchEvents ");
                sqlSelectQueryForUpdate.AppendLine("where ");
                sqlSelectQueryForUpdate.AppendLine("userIdentity = '<<userIdentity>>' ");
                sqlSelectQueryForUpdate.AppendLine("AND signinType = '<<signinType>>' ");
                sqlSelectQueryForUpdate.AppendLine("and signinUnixTime between ");
                sqlSelectQueryForUpdate.AppendLine(unixTimestampStartDate.ToString());
                sqlSelectQueryForUpdate.AppendLine(" and ");
                sqlSelectQueryForUpdate.AppendLine(unixTimestampEndDate.ToString());

                StringBuilder sqlUpdateQueryForUpdate = new StringBuilder();
                sqlUpdateQueryForUpdate.AppendLine("update " + tableName);
                sqlUpdateQueryForUpdate.AppendLine(" set [" + columnName + "] = ");
                sqlUpdateQueryForUpdate.AppendLine(" ( ");
                sqlUpdateQueryForUpdate.AppendLine(sqlSelectQueryForUpdate.ToString());
                sqlUpdateQueryForUpdate.AppendLine(" ) ");
                sqlUpdateQueryForUpdate.AppendLine("where ");
                sqlUpdateQueryForUpdate.AppendLine("userIdentity = '<<userIdentity>>' ");
                sqlUpdateQueryForUpdate.AppendLine("AND signinType = '<<signinType>>' ");

                sqlUpdateQueries.Add(sqlUpdateQueryForUpdate.ToString());

            }

            DateTime sqlrunStartTime = new DateTime();
            sqlrunStartTime = DateTime.Now;

            //delete all rows from table
            sql.Clear();
            sql.AppendLine(" DELETE ");
            sql.AppendLine(" FROM " + tableName);
            SQLiteCommand command1 = new SQLiteCommand(sql.ToString(), m_dbConnection);
            log.Info("SQL: " + sql.Replace(Environment.NewLine, " "));
            command1.ExecuteNonQuery();

            // insert all the students in the table for each type
            foreach (string userID in users)
            {
                foreach (string signInType in signInTypes)
                {
                    string totalHours = "0.0";

                    StringBuilder sql2 = new StringBuilder();
                    sql2.AppendLine("insert into " + tableName);
                    sql2.AppendLine(" (userIdentity,signinType,totalHours) ");
                    sql2.AppendLine(" VALUES ");
                    sql2.AppendLine(" ( ");
                    sql2.AppendLine("  '" + userID + "' ");
                    sql2.AppendLine(" ,'" + signInType + "' ");
                    sql2.AppendLine(" ,'" + totalHours + "' ");
                    sql2.AppendLine(" ) ");

                    SQLiteCommand command9 = new SQLiteCommand(sql2.ToString(), m_dbConnection);
                    log.Info("SQL: " + sql2.Replace(Environment.NewLine, " "));
                    command9.ExecuteNonQuery();

                }
            }

            // loop through the users and types again and do all the updates
            foreach (string userID in users)
            {
                foreach (string signInType in signInTypes)
                {
                    foreach (string sqlQuery in sqlUpdateQueries)
                    {
                        string executeSQL = sqlQuery.Replace("<<userIdentity>>", userID).Replace("<<signinType>>", signInType);
                        SQLiteCommand command4 = new SQLiteCommand(executeSQL, m_dbConnection);
                        command4.ExecuteNonQuery();
                    }
                }
            }


            // update the totals
            // loop through the users and types again and do all the updates
            foreach (string userID in users)
            {
                foreach (string signInType in signInTypes)
                {
                    StringBuilder sqlTotal = new StringBuilder();
                    sqlTotal.AppendLine("update " + tableName);
                    sqlTotal.AppendLine(" set totalHours = ");
                    sqlTotal.AppendLine(" ( ");
                    sqlTotal.AppendLine(" select");

                    int colCount = 0;
                    foreach (string columnName in dateColumns)
                    {
                        sqlTotal.AppendLine("ifnull([" + columnName + "],0.0)");
                        colCount += 1;
                        if(colCount != dateColumns.Count)
                        {
                            sqlTotal.AppendLine(" + ");
                        }
                    }

                    sqlTotal.AppendLine(" from " + tableName);
                    sqlTotal.AppendLine(" where ");
                    sqlTotal.AppendLine(" userIdentity = '<<userIdentity>>' ");
                    sqlTotal.AppendLine(" AND signinType = '<<signinType>>' ");
                    sqlTotal.AppendLine(" ) ");
                    sqlTotal.AppendLine(" where ");
                    sqlTotal.AppendLine(" userIdentity = '<<userIdentity>>' ");
                    sqlTotal.AppendLine(" AND signinType = '<<signinType>>' ");

                    string executeSQL = sqlTotal.ToString().Replace("<<userIdentity>>", userID).Replace("<<signinType>>", signInType);
                    SQLiteCommand command8 = new SQLiteCommand(executeSQL, m_dbConnection);
                    command8.ExecuteNonQuery();
                }
            }

            DateTime sqlrunEndTime = new DateTime();
            sqlrunEndTime = DateTime.Now;

            //222 SECONDS
            txtResults.Text = sqlrunEndTime.Subtract(sqlrunStartTime).TotalSeconds.ToString();

        }
        void createSummaryTable()
        {
            log.Debug("IN");
            
            //TODO - get these from the config
            int startWeekNumber = 30;
            int endWeekNumber = 20;
            int startYear = 2017;
            int endYear = 2018;
            string tableName = "ReportWeeklySummary";
            int dayChunksForColumns = 7;


            StringBuilder sql = new StringBuilder();

            sql.Append("drop table " + tableName);
            SQLiteCommand command0 = new SQLiteCommand(sql.ToString(), m_dbConnection);
            log.Info("SQL: " + sql.Replace(Environment.NewLine, " "));
            command0.ExecuteNonQuery();


            sql.Clear();
            
            sql.AppendLine("create table " + tableName);
            sql.AppendLine(" (");
            sql.AppendLine(" userIdentity varchar(20), ");
            sql.AppendLine(" signinType varchar(20), ");

            string dayOneStart = "1/1/" + startYear.ToString();
            DateTime startDate = DateTime.Parse(dayOneStart);
            startDate = startDate.AddDays(startWeekNumber * 7);
            var dateStart = startDate.AddDays(-(int)startDate.DayOfWeek);

            string dayOneEnd = "1/1/" + endYear.ToString();
            DateTime endDate = DateTime.Parse(dayOneEnd);
            endDate = endDate.AddDays(endWeekNumber * 7);
            var dateEnd = endDate.AddDays(-(int)endDate.DayOfWeek);

            DateTime loopDate = startDate;
            while (loopDate < endDate)
            {
                string part1 = loopDate.ToString("MM/dd/yy");
                string part2 = loopDate.AddDays(dayChunksForColumns).AddMinutes(-1).ToString("MM/dd/yy");
                string together = "[w" + part1 + "-" + part2 + "]";
                sql.AppendLine(together + " varchar(20), ");

                // increment the date
                loopDate = loopDate.AddDays(7);
            }

            sql.AppendLine("totalHours varchar(20) ");

            sql.AppendLine(" )");

            SQLiteCommand command = new SQLiteCommand(sql.ToString(), m_dbConnection);
            log.Info("SQL: " + sql.Replace(Environment.NewLine, " "));
            command.ExecuteNonQuery();

            log.Info("Table Created: " + tableName);

        }

    }

}
