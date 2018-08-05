using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TimePunch
{

    public partial class frmSignInTypePicker : Form
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
 
        public List<string> signInTypes = new List<string>();
        public string selectedSignInType = "";
        public string userIdentity = "";

        public frmSignInTypePicker()
        {
            InitializeComponent();
        }

        private void frmSignInTypePicker_Load(object sender, EventArgs e)
        {
            log.Debug("IN");
            try
            {
                int buttons = 0;
                int buttonHeight = 30;
                int buttonWidth = 80;
                int buttonLeft = 20;
                int buttonFirstTop = 50;
                int buttonPadding = 10;
                int formPadding = 50;

                lblUser.Text = userIdentity;
                lblUser.Left = buttonLeft;

                foreach (string signInType in signInTypes)
                {
                    // add a big button
                    Button newButton = new Button();
                    newButton.Text = signInType;
                    newButton.Tag = signInType;

                    this.Controls.Add(newButton);
                    newButton.Left = buttonLeft;
                    newButton.Height = buttonHeight;
                    newButton.Top = buttonFirstTop + newButton.Height * buttons + buttonPadding * buttons;
                    newButton.Width = buttonWidth;

                    // add the event handler
                    newButton.Click += new System.EventHandler(this.newButtonClick);

                    buttons += 1;
                }

                // size the form to match the number of buttons
                this.Height = buttonFirstTop + buttons * buttonHeight + buttonPadding * buttons + formPadding;
            }
            catch (Exception ex)
            {
                log.Error("Error", ex);
                MessageBox.Show(ex.Message, "Error - " + System.Reflection.MethodBase.GetCurrentMethod().Name);
            }




        }
        private void newButtonClick(object sender, EventArgs e)
        {
            log.Debug("IN");

            try
            {
                Button thisButton = (Button)sender;
                selectedSignInType = thisButton.Tag.ToString();

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
