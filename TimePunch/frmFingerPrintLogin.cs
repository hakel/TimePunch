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
    public partial class frmFingerPrintLogin : Form
    {

        FlexCodeSDK.FinFPVer ver;
        public string userIDForForm = "";

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private string connectionString = "Data Source=" + ConfigurationManager.AppSettings["dbFileName"].ToString() + ";Version=3;";

        SQLiteConnection m_dbConnection;


        public frmFingerPrintLogin()
        {
            InitializeComponent();
        }

        private void frmFingerPrintLogin_Load(object sender, EventArgs e)
        {
            log.Debug("IN");

            try
            {
                //Initialize FlexCodeSDK for Verification
                //1. Initialize Event Handler
                ver = new FlexCodeSDK.FinFPVer();
                ver.FPVerificationID += new __FinFPVer_FPVerificationIDEventHandler(ver_FPVerificationID);
                ver.FPVerificationStatus += new __FinFPVer_FPVerificationStatusEventHandler(ver_FPVerificationStatus);

                //2. Input the activation code
                ver.AddDeviceInfo("HY20E20453", "996E93F0285F7D1", "2M13A7D9A0CF41DB625AB1ED");

                //4. Load templates from database to FlexCodeSDK
                string sql = "SELECT userIdentity, userFingerprint FROM TimePunchUserIdentities where userFingerprint <>'' ";

                // Make a connection to the database if it hasnt already
                connectToDatabase();

                SQLiteCommand command1 = new SQLiteCommand(sql, m_dbConnection);
                SQLiteDataReader reader1 = command1.ExecuteReader();

                while (reader1.Read())
                {
                    ver.FPLoad(reader1.GetString(0), 0, reader1.GetString(1), "MySecretKey" + reader1.GetString(0));
                }
                reader1.Close();

                //5. Start verification process
                ver.FPVerificationStart();
            }
            catch (Exception ex)
            {
                log.Error("Error", ex);
                MessageBox.Show(ex.Message, "Error - " + System.Reflection.MethodBase.GetCurrentMethod().Name);
            }

        }
        void ver_FPVerificationStatus(VerificationStatus Status)
        {
            log.Debug("IN");

            try
            {
                log.Debug("Status:" + Status.ToString());

                if (Status == VerificationStatus.v_OK)
                {
                    MessageBox.Show("Fingerprint found for: " + userIDForForm, "Success");
                    ver.FPVerificationStop();
                }
                else if (Status == VerificationStatus.v_NotMatch)
                {
                    MessageBox.Show("Fingerprint did not match any users on record.", "No Match");
                    //ver.FPVerificationStop();
                }
            }
            catch (Exception ex)
            {
                log.Error("Error", ex);
                MessageBox.Show(ex.Message, "Error - " + System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        void ver_FPVerificationID(string ID, FingerNumber FingerNr)
        {
            log.Debug("IN");

            try
            {
                userIDForForm = ID;
            }
            catch (Exception ex)
            {
                log.Error("Error", ex);
                MessageBox.Show(ex.Message, "Error - " + System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        void connectToDatabase()
        {
            log.Debug("IN");
            try
            {
                if (m_dbConnection == null)
                {
                    m_dbConnection = new SQLiteConnection(connectionString);
                    m_dbConnection.Open();
                }
            }
            catch (Exception ex)
            {
                log.Error("Error", ex);
                MessageBox.Show(ex.Message, "Error - " + System.Reflection.MethodBase.GetCurrentMethod().Name);
            }

        }
        private void frmFingerPrintLogin_FormClosing(object sender, FormClosingEventArgs e)
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
            try
            {
                ver.FPVerificationStop();
            }
            catch (Exception ex)
            {
                log.Error("Error", ex);
                //dont care
                //MessageBox.Show(ex.Message, "Error - " + System.Reflection.MethodBase.GetCurrentMethod().Name);
            }

        }

    }
}
