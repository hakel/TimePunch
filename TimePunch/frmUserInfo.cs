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
using FlexCodeSDK;

namespace TimePunch
{
    public partial class frmUserInfo : Form
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private string connectionString = "Data Source=" + ConfigurationManager.AppSettings["dbFileName"].ToString() + ";Version=3;";

        SQLiteConnection m_dbConnection;

        FlexCodeSDK.FinFPReg reg;
        string fingerprint = "";

        public string userIDForForm = "";
        public bool isUpdate = false;

        public frmUserInfo()
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

        private void btnCreateUser_Click(object sender, EventArgs e)
        {
            log.Debug("IN");

            try
            {

                // grab a timestamp
                Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

                // Make a connection to the database if it hasnt already
                connectToDatabase();

                //  validate the data
                string thisUserID = txtUserID.Text;
                string thisUserPassword = txtPassword.Text;
                string thisUserPassword2 = txtPassword2.Text;
                string thisUserGrade = txtGrade.Text;

                if (thisUserPassword != thisUserPassword2)
                {
                    MessageBox.Show("Passwords do not match.  User not created", "Info");
                    return;
                }

                // check that the user doesnt already exist
                Boolean foundLogin = false;

                string sqlLogin = "select * from TimePunchUserIdentities where userIdentity = '" + thisUserID + "' order by createUnixTimeStamp desc";

                SQLiteCommand command1 = new SQLiteCommand(sqlLogin, m_dbConnection);
                SQLiteDataReader reader1 = command1.ExecuteReader();

                while (reader1.Read() && !foundLogin)
                {
                    foundLogin = true;
                }
                reader1.Close();


                if (foundLogin && !isUpdate)
                {
                    MessageBox.Show("User already exists.  User not created", "Info");
                    return;
                }

                if (isUpdate)
                {

                    string sql = "update TimePunchUserIdentities set" +
                        " userPassword = '" + thisUserPassword + "', " +
                        " updateUnixTimeStamp = " + unixTimestamp.ToString() + " " +
                        " where userIdentity = '" + thisUserID + "' ";

                    // update password
                    if (fingerprint != "")
                    {
                        sql = "update TimePunchUserIdentities set" +
                            " userPassword = '" + thisUserPassword + "', " +
                            " userFingerprint = '" + fingerprint + "', " +
                            " updateUnixTimeStamp = " + unixTimestamp.ToString() + " " +
                            " where userIdentity = '" + thisUserID + "' ";
                        
                    }
                    SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
                    log.Info("SQL: " + sql.Replace(Environment.NewLine, " "));
                    command.ExecuteNonQuery();

                    // update user info
                    string firstName = txtFirstName.Text;
                    string lastName = txtLastName.Text;
                    sql = "update TimePunchUserInfo" +
                        " set " +
                        " userFirstName = '" + firstName + "'  , " +
                        " userLastName = '" + lastName + "' , " +
                        " userGrade = " + thisUserGrade + " , " +
                        " updateUnixTimeStamp = " + unixTimestamp.ToString() + " " +
                        " Where userIdentity = '" + thisUserID + "' ";

                    SQLiteCommand command3 = new SQLiteCommand(sql, m_dbConnection);
                    log.Info("SQL: " + sql.Replace(Environment.NewLine, " "));
                    command3.ExecuteNonQuery();

                    MessageBox.Show("User Updated!", "Info");

                }
                else
                {
                    // add the user 
                    string sql = "insert into TimePunchUserIdentities" +
                        " (" +
                        " userIdentity, " +
                        " userPassword, " +
                        " userFingerprint, " +
                        " isAdmin, " +
                        " createUnixTimeStamp, " +
                        " updateUnixTimeStamp" +
                        " ) values " +
                        " (" +
                        " '" + thisUserID + "', " +
                        " '" + thisUserPassword + "', " +
                        " '" + fingerprint + "', " +
                         "0, " +
                        unixTimestamp.ToString() + ", " +
                        unixTimestamp.ToString() +
                        " ) ";

                    SQLiteCommand command2 = new SQLiteCommand(sql, m_dbConnection);
                    log.Info("SQL: " + sql.Replace(Environment.NewLine, " "));
                    command2.ExecuteNonQuery();

                    // Add user info
                    string firstName = txtFirstName.Text;
                    string lastName = txtLastName.Text;
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
                        " '" + thisUserID + "', " +
                        " '" + firstName + "', " +
                        " '" + lastName + "', " +
                        " '" + thisUserGrade + "', " +
                        unixTimestamp.ToString() + ", " +
                        unixTimestamp.ToString() +
                        " ) ";

                    SQLiteCommand command3 = new SQLiteCommand(sql, m_dbConnection);
                    log.Info("SQL: " + sql.Replace(Environment.NewLine," "));
                    command3.ExecuteNonQuery();

                    MessageBox.Show("User Created!", "Info");

                }
                // reset the fingerprint
                fingerprint = "";
                // Close the form and pass back the user id and prefill it if possible
                userIDForForm = thisUserID;
                this.Close();
            }
            catch (Exception ex)
            {
                log.Error("Error", ex);
                MessageBox.Show(ex.Message, "Error - " + System.Reflection.MethodBase.GetCurrentMethod().Name);
            }

        }

        private void frmUserInfo_FormClosing(object sender, FormClosingEventArgs e)
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

        private void frmUserInfo_Load(object sender, EventArgs e)
        {
            log.Debug("IN");

            try
            {

                // if this is an update, we need to prefill the form
                if (isUpdate)
                {
                    btnCreateUser.Text = "Update User";
                    txtUserID.Enabled = false;

                    txtUserID.Text = userIDForForm;

                    // Make a connection to the database if it hasnt already
                    connectToDatabase();

                    // check that the user doesnt already exist
                    Boolean foundLogin = false;

                    string sql = "select * from TimePunchUserInfo where userIdentity = '" + userIDForForm + "' order by createUnixTimeStamp desc";

                    SQLiteCommand command1 = new SQLiteCommand(sql, m_dbConnection);
                    SQLiteDataReader reader1 = command1.ExecuteReader();

                    while (reader1.Read() && !foundLogin)
                    {
                        txtFirstName.Text = reader1["userFirstName"].ToString();
                        txtLastName.Text = reader1["userLastName"].ToString();
                        txtGrade.Text = reader1["userGrade"].ToString();

                        foundLogin = true;
                    }
                    reader1.Close();


                    if (!foundLogin)
                    {
                        //do something here if the user we are trying to edit does not exist
                        throw new Exception("User Not Found");
                    }
                    else
                    {
                        // Get the credentials
                        string sqlLogin = "select * from TimePunchUserIdentities where userIdentity = '" + userIDForForm + "' order by createUnixTimeStamp desc";

                        SQLiteCommand command2 = new SQLiteCommand(sqlLogin, m_dbConnection);
                        SQLiteDataReader reader2 = command2.ExecuteReader();

                        while (reader2.Read())
                        {
                            txtPassword.Text = reader2["userPassword"].ToString();
                            txtPassword2.Text = txtPassword.Text;
                            //TODOlater - should we add the isAdmin flag at some point.  
                            // Meaning, at this point, this form doest support adding an admin or making someone an admin
                        }
                        reader2.Close();



                    }
                }

                lblFingerprintCount.Text = "";

                //Initialize FlexCodeSDK for Registration
                //1. Initialize Event Handler
                reg = new FlexCodeSDK.FinFPReg();
                reg.FPSamplesNeeded += new __FinFPReg_FPSamplesNeededEventHandler(reg_FPSamplesNeeded);
                reg.FPRegistrationTemplate += new __FinFPReg_FPRegistrationTemplateEventHandler(reg_FPRegistrationTemplate);
                reg.FPRegistrationStatus += new __FinFPReg_FPRegistrationStatusEventHandler(reg_FPRegistrationStatus);

                //2. Input the activation code
                reg.AddDeviceInfo("HY20E20453", "996E93F0285F7D1", "2M13A7D9A0CF41DB625AB1ED");

            }
            catch (Exception ex)
            {
                log.Error("Error", ex);
                MessageBox.Show(ex.Message, "Error - " + System.Reflection.MethodBase.GetCurrentMethod().Name);
            }

        }

        private void btnFingerprint_Click(object sender, EventArgs e)
        {
            log.Debug("IN");

            try
            {

                if (txtUserID.Text == "")
                {
                    MessageBox.Show("Please enter ID before recording fingerprint.");
                }
                else
                {
                    btnFingerprint.Enabled = false;
                    reg.FPRegistrationStart("MySecretKey" + txtUserID.Text);
                }
            }
            catch (Exception ex)
            {
                log.Error("Error", ex);
                MessageBox.Show(ex.Message, "Error - " + System.Reflection.MethodBase.GetCurrentMethod().Name);
            }

        }
        void reg_FPSamplesNeeded(short Samples)
        {
            log.Debug("IN");

            try
            {
                lblFingerprintCount.Text = "Samples Needed : " + Convert.ToString(Samples);
            }
            catch (Exception ex)
            {
                log.Error("Error", ex);
                MessageBox.Show(ex.Message, "Error - " + System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        void reg_FPRegistrationTemplate(string FPTemplate)
        {
            log.Debug("IN");

            try
            {
                fingerprint = FPTemplate;
            }
            catch (Exception ex)
            {
                log.Error("Error", ex);
                MessageBox.Show(ex.Message, "Error - " + System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        void reg_FPRegistrationStatus(RegistrationStatus Status)
        {

            log.Debug("IN");

            try
            {
                log.Debug("Status:" + Status.ToString());

                if (Status == RegistrationStatus.r_OK)
                {
                    MessageBox.Show("Fingerprint Recorded!", "Info");
                    string preview = fingerprint;
                    lblFingerprintCount.Text = "Fingerprint recorded.";
                    btnFingerprint.Enabled = true;
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
