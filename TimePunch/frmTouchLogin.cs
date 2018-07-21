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

namespace TimePunch
{
    public partial class frmTouchLogin : Form
    {
        public string userIDForForm = "";
        public string userPWDForForm = "";

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private string connectionString = "Data Source=" + ConfigurationManager.AppSettings["dbFileName"].ToString() + ";Version=3;";

        SQLiteConnection m_dbConnection;

        public frmTouchLogin()
        {
            InitializeComponent();
        }

        private void frmTouchLogin_Load(object sender, EventArgs e)
        {
            log.Debug("IN");

            try
            {
                connectToDatabase();
                fillComboWithUsers(cboUsersForPassword);
                cboUsersForPassword.Select();
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
        void connectToDatabase()
        {
            log.Debug("IN");

            if (m_dbConnection == null)
            {
                m_dbConnection = new SQLiteConnection(connectionString);
                m_dbConnection.Open();
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            log.Debug("IN");

            try
            {

                ListViewItem xx = (ListViewItem)cboUsersForPassword.SelectedItem;
                userIDForForm = xx.Tag.ToString();
                userPWDForForm = txtPassword.Text;
                this.Close();

            }
            catch (Exception ex)
            {
                log.Error("Error", ex);
                MessageBox.Show(ex.Message, "Error - " + System.Reflection.MethodBase.GetCurrentMethod().Name);
            }

        }
        private void frmTouchLogin_FormClosing(object sender, FormClosingEventArgs e)
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

        private void maskedTextBox1_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void btnOne_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Button thisButton = (System.Windows.Forms.Button)sender;
            txtPassword.Text = txtPassword.Text + thisButton.Tag.ToString();
        }

        private void btnTwo_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Button thisButton = (System.Windows.Forms.Button)sender;
            txtPassword.Text = txtPassword.Text + thisButton.Tag.ToString();
        }

        private void btnThree_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Button thisButton = (System.Windows.Forms.Button)sender;
            txtPassword.Text = txtPassword.Text + thisButton.Tag.ToString();

        }

        private void btnFour_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Button thisButton = (System.Windows.Forms.Button)sender;
            txtPassword.Text = txtPassword.Text + thisButton.Tag.ToString();

        }

        private void btnFive_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Button thisButton = (System.Windows.Forms.Button)sender;
            txtPassword.Text = txtPassword.Text + thisButton.Tag.ToString();
            btnDummy.PerformClick();

        }

        private void btnSix_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Button thisButton = (System.Windows.Forms.Button)sender;
            txtPassword.Text = txtPassword.Text + thisButton.Tag.ToString();

        }

        private void btnSeven_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Button thisButton = (System.Windows.Forms.Button)sender;
            txtPassword.Text = txtPassword.Text + thisButton.Tag.ToString();

        }

        private void btnEight_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Button thisButton = (System.Windows.Forms.Button)sender;
            txtPassword.Text = txtPassword.Text + thisButton.Tag.ToString();

        }

        private void btnNine_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Button thisButton = (System.Windows.Forms.Button)sender;
            txtPassword.Text = txtPassword.Text + thisButton.Tag.ToString();

        }

        private void btnZero_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Button thisButton = (System.Windows.Forms.Button)sender;
            txtPassword.Text = txtPassword.Text + thisButton.Tag.ToString();

        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtPassword.Text = "";

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if(txtPassword.Text.Length > 0)
            {
                txtPassword.Text = txtPassword.Text.Substring(0, txtPassword.Text.Length - 1);
            }

        }

        private void btnDummy_Click(object sender, EventArgs e)
        {

        }
    }
}
