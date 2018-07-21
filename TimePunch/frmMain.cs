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

        private void btnPunchIn_Click(object sender, EventArgs e)
        {
            log.Debug("IN");

            try
            {
                //string userIdentity = txtUserID.Text;
                string userIdentity = txtUserID.Tag.ToString();
                string signinType = "";
                if (rdLab.Checked)
                {
                    signinType = ConfigurationManager.AppSettings["SignInType_Lab"].ToString();
                }
                else
                {
                    signinType = ConfigurationManager.AppSettings["SignInType_Theory"].ToString();
                }

                Int32 punchtime = normalPunchIN(userIdentity, signinType);

                // show modal that you are logged in
                MessageBox.Show("Clocked In! - " + convertUnixDateTimeToDisplayDateTime(punchtime.ToString()),"Info");

                logOut();
                //defaultUIElements();
            }
            catch (Exception ex)
            {
                log.Error("Error", ex);
                MessageBox.Show(ex.Message, "Error - " + System.Reflection.MethodBase.GetCurrentMethod().Name);
            }

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
                        // TODO - this is unexpected, probably need to show a message
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

                // Validate that they selected a punch type
                if (!rdLab.Checked && !rdTheory.Checked && dbAdminFlag == 0)
                {
                    MessageBox.Show("You must select " + rdLab.Text + " or " + rdTheory.Text + " before logging in.", "Info");
                    return;
                }

                mnuAdmin.Visible = (dbAdminFlag == 1);
                btnLogOut.Visible = true;
                btnLogin.Visible = false;
                //grpType.Enabled = btnLogin.Visible;
                rdLab.Enabled = btnLogin.Visible;
                rdTheory.Enabled = btnLogin.Visible;

                if (dbAdminFlag == 1)
                {

                    // this is a first time setup, so show the admin screen
                    var adminForm = new frmAdmin();
                    //assign db variables so we dont have them duplicated
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
                    doLogin(userIdentity);
                }

            }
            catch (Exception ex)
            {
                log.Error("Error", ex);
                MessageBox.Show(ex.Message, "Error - " + System.Reflection.MethodBase.GetCurrentMethod().Name);
            }

        }

        private void doLogin(string userIdentity)
        {
            txtUserID.Tag = userIdentity;


            //determine which kind of punch this is
            string signinType = "";
            if (rdLab.Checked)
            {
                signinType = ConfigurationManager.AppSettings["SignInType_Lab"].ToString();
            }
            else
            {
                signinType = ConfigurationManager.AppSettings["SignInType_Theory"].ToString();
            }

            // Get the last punch
            string sql = "select * from TimePunchEvents where userIdentity = '" + userIdentity + "' and signinType = '" + signinType + "' and manualPunch = 0  order by createUnixTimeStamp desc";

            SQLiteCommand command2 = new SQLiteCommand(sql, m_dbConnection);
            SQLiteDataReader reader2 = command2.ExecuteReader();
            StringBuilder output = new StringBuilder();

            bool foundPunch = false;
            string signInTime = "";
            string signOutTime = "";

            while (reader2.Read() && !foundPunch)
            {
                signInTime = reader2["signinUnixTime"].ToString();
                signOutTime = reader2["signoutUnixTime"].ToString();
                foundPunch = true;
            }
            reader2.Close();

            // logic here is if sign out and in time are equal, they have not yet signed out
            if (foundPunch && signOutTime == signInTime)
            {
                // Do Sign Out
                // if punched in, show punch out, show punch in time, show punch out time
                btnPunchIn.Enabled = false;
                btnPunchOut.Enabled = !btnPunchIn.Enabled;
                lblPunchIn.Visible = true;
                lblPunchOut.Visible = true;
                lblLastFullPunch.Visible = false;
                timer.Enabled = true;

                // get last punch in time

                // convert signInTime to a human readable date time
                lblPunchIn.Text = "Last in for " + signinType + ": " + convertUnixDateTimeToDisplayDateTime(signInTime.ToString());

            }
            else
            {
                // Do Sign In
                // if punched out, show punch in, show punch in time label, hide punch out label, show last punched in label
                btnPunchIn.Enabled = true;
                btnPunchOut.Enabled = !btnPunchIn.Enabled;
                lblPunchIn.Visible = true;
                lblPunchOut.Visible = false;
                timer.Enabled = true;

                lblLastFullPunch.Visible = true;
                // populate last full punch label with something
                if (signInTime == "")
                {
                    // user has never signed in, so just blank this out
                    lblLastFullPunch.Text = "";
                }
                else
                {
                    lblLastFullPunch.Text = "Last " + signinType + " - In: " + convertUnixDateTimeToDisplayDateTime(signInTime) + " - Out: " + convertUnixDateTimeToDisplayDateTime(signOutTime);
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
            //btnLogin.Visible = true;
            //btnLogOut.Visible = false;
        }

        private void defaultUIElements()
        {
            log.Debug("IN");

            mnuAdmin.Visible = false;
            timer.Enabled = false;
            lblPunchIn.Visible = false;
            lblPunchOut.Visible = false;
            lblLastFullPunch.Visible = false;
            btnPunchIn.Enabled = false;
            btnPunchOut.Enabled = false;
            lblPunchIn.Text = "";
            lblPunchOut.Text = "";
            lblLastFullPunch.Text = "";
            rdLab.Checked = true;
            rdTheory.Checked = false;
            btnLogin.Visible = true;
            btnLogOut.Visible = false;
            //grpType.Enabled = btnLogin.Visible;
            rdLab.Enabled = btnLogin.Visible;
            rdTheory.Enabled = btnLogin.Visible;
            skipPassword = false;
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            try
            {
                // tick the lables

                // get the current date and time stamp
                string nowTime = DateTime.Now.ToLongTimeString();

                //update the labels
                if (!(lblPunchIn.Text.IndexOf("Last") >= 0))
                {
                    lblPunchIn.Text = nowTime;
                }
                lblPunchOut.Text = nowTime;
            }
            catch (Exception ex)
            {
                log.Error("Error", ex);
                //MessageBox.Show(ex.Message, "Error - " + System.Reflection.MethodBase.GetCurrentMethod().Name);
            }

        }

        private void btnPunchOut_Click(object sender, EventArgs e)
        {
            log.Debug("IN");

            try
            {
                //string userIdentity = txtUserID.Text;
                string userIdentity = txtUserID.Tag.ToString();
                string signinType = "";
                if (rdLab.Checked)
                {
                    signinType = ConfigurationManager.AppSettings["SignInType_Lab"].ToString();
                }
                else
                {
                    signinType = ConfigurationManager.AppSettings["SignInType_Theory"].ToString();
                }

                Int32 punchtime = normalPunchOUT(userIdentity, signinType);

                // show modal that you are punched out
                //TODO - show more details here, like the full punch info, how long they were in, etc.
                MessageBox.Show("Clocked Out! - " + convertUnixDateTimeToDisplayDateTime(punchtime.ToString()), "Info");

                logOut();
                //defaultUIElements();
            }
            catch (Exception ex)
            {
                log.Error("Error", ex);
                MessageBox.Show(ex.Message, "Error - " + System.Reflection.MethodBase.GetCurrentMethod().Name);
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
                //TODO - throw an error here, you shouldnt have the ability to sign out if there is no sign in
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


        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void btnLogOut_Click(object sender, EventArgs e)
        {
            try
            {
                logOut();
            }
            catch (Exception ex)
            {
                log.Error("Error", ex);
                MessageBox.Show(ex.Message, "Error - " + System.Reflection.MethodBase.GetCurrentMethod().Name);
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

                //TODO - i should pass the db info so it doesnt have to be in the form directly
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
                //TODO - assign db variables so we dont have them duplicated
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

        private void lblLoggedInAs_Click(object sender, EventArgs e)
        {

        }

        private void btnFingerprintLogin_Click(object sender, EventArgs e)
        {
            log.Debug("IN");

            try
            {

                //TODO - i should pass the db info so it doesnt have to be in the form directly
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
                    //btnLogin_Click(sender, e);
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

                //TODO - i should pass the db info so it doesnt have to be in the form directly
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
                    //btnLogin_Click(sender, e);
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
