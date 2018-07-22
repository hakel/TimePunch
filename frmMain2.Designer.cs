namespace TimePunch
{
    partial class frmMain2
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain2));
            this.btnPunchIn = new System.Windows.Forms.Button();
            this.txtUserID = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.btnPunchOut = new System.Windows.Forms.Button();
            this.btnLogin = new System.Windows.Forms.Button();
            this.lblPunchIn = new System.Windows.Forms.Label();
            this.lblPunchOut = new System.Windows.Forms.Label();
            this.lblLastFullPunch = new System.Windows.Forms.Label();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.grpLogin = new System.Windows.Forms.GroupBox();
            this.btnFingerprintLogin = new System.Windows.Forms.Button();
            this.btnLogOut = new System.Windows.Forms.Button();
            this.rdTheory = new System.Windows.Forms.RadioButton();
            this.rdLab = new System.Windows.Forms.RadioButton();
            this.grpType = new System.Windows.Forms.GroupBox();
            this.mnuMain = new System.Windows.Forms.MenuStrip();
            this.mnuNewUser = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuAdmin = new System.Windows.Forms.ToolStripMenuItem();
            this.btnTouchLogin = new System.Windows.Forms.Button();
            this.grpLogin.SuspendLayout();
            this.grpType.SuspendLayout();
            this.mnuMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnPunchIn
            // 
            this.btnPunchIn.Location = new System.Drawing.Point(18, 252);
            this.btnPunchIn.Name = "btnPunchIn";
            this.btnPunchIn.Size = new System.Drawing.Size(75, 23);
            this.btnPunchIn.TabIndex = 3;
            this.btnPunchIn.Text = "Clock In";
            this.btnPunchIn.UseVisualStyleBackColor = true;
            this.btnPunchIn.Click += new System.EventHandler(this.btnPunchIn_Click);
            // 
            // txtUserID
            // 
            this.txtUserID.Location = new System.Drawing.Point(9, 38);
            this.txtUserID.Name = "txtUserID";
            this.txtUserID.Size = new System.Drawing.Size(100, 20);
            this.txtUserID.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "User";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(116, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Password";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(119, 39);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(100, 20);
            this.txtPassword.TabIndex = 2;
            // 
            // btnPunchOut
            // 
            this.btnPunchOut.Location = new System.Drawing.Point(18, 283);
            this.btnPunchOut.Name = "btnPunchOut";
            this.btnPunchOut.Size = new System.Drawing.Size(75, 23);
            this.btnPunchOut.TabIndex = 4;
            this.btnPunchOut.Text = "Clock Out";
            this.btnPunchOut.UseVisualStyleBackColor = true;
            this.btnPunchOut.Click += new System.EventHandler(this.btnPunchOut_Click);
            // 
            // btnLogin
            // 
            this.btnLogin.Location = new System.Drawing.Point(9, 67);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(75, 23);
            this.btnLogin.TabIndex = 8;
            this.btnLogin.Text = "Login";
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // lblPunchIn
            // 
            this.lblPunchIn.AutoSize = true;
            this.lblPunchIn.Location = new System.Drawing.Point(99, 258);
            this.lblPunchIn.Name = "lblPunchIn";
            this.lblPunchIn.Size = new System.Drawing.Size(34, 13);
            this.lblPunchIn.TabIndex = 9;
            this.lblPunchIn.Text = "12:00";
            // 
            // lblPunchOut
            // 
            this.lblPunchOut.AutoSize = true;
            this.lblPunchOut.Location = new System.Drawing.Point(99, 289);
            this.lblPunchOut.Name = "lblPunchOut";
            this.lblPunchOut.Size = new System.Drawing.Size(34, 13);
            this.lblPunchOut.TabIndex = 10;
            this.lblPunchOut.Text = "12:00";
            // 
            // lblLastFullPunch
            // 
            this.lblLastFullPunch.AutoSize = true;
            this.lblLastFullPunch.Location = new System.Drawing.Point(6, 319);
            this.lblLastFullPunch.Name = "lblLastFullPunch";
            this.lblLastFullPunch.Size = new System.Drawing.Size(157, 13);
            this.lblLastFullPunch.TabIndex = 11;
            this.lblLastFullPunch.Text = "Last Punch: 4/1/2018 10:00am";
            // 
            // timer
            // 
            this.timer.Interval = 1000;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // grpLogin
            // 
            this.grpLogin.Controls.Add(this.btnTouchLogin);
            this.grpLogin.Controls.Add(this.btnFingerprintLogin);
            this.grpLogin.Controls.Add(this.txtUserID);
            this.grpLogin.Controls.Add(this.label1);
            this.grpLogin.Controls.Add(this.txtPassword);
            this.grpLogin.Controls.Add(this.label2);
            this.grpLogin.Controls.Add(this.btnLogin);
            this.grpLogin.Controls.Add(this.btnLogOut);
            this.grpLogin.Location = new System.Drawing.Point(9, 120);
            this.grpLogin.Name = "grpLogin";
            this.grpLogin.Size = new System.Drawing.Size(227, 126);
            this.grpLogin.TabIndex = 14;
            this.grpLogin.TabStop = false;
            this.grpLogin.Text = "Login";
            this.grpLogin.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // btnFingerprintLogin
            // 
            this.btnFingerprintLogin.Location = new System.Drawing.Point(119, 65);
            this.btnFingerprintLogin.Name = "btnFingerprintLogin";
            this.btnFingerprintLogin.Size = new System.Drawing.Size(100, 23);
            this.btnFingerprintLogin.TabIndex = 14;
            this.btnFingerprintLogin.Text = "Fingerprint Login";
            this.btnFingerprintLogin.UseVisualStyleBackColor = true;
            this.btnFingerprintLogin.Click += new System.EventHandler(this.btnFingerprintLogin_Click);
            // 
            // btnLogOut
            // 
            this.btnLogOut.Location = new System.Drawing.Point(9, 67);
            this.btnLogOut.Name = "btnLogOut";
            this.btnLogOut.Size = new System.Drawing.Size(75, 23);
            this.btnLogOut.TabIndex = 13;
            this.btnLogOut.Text = "Logout";
            this.btnLogOut.UseVisualStyleBackColor = true;
            this.btnLogOut.Visible = false;
            this.btnLogOut.Click += new System.EventHandler(this.btnLogOut_Click);
            // 
            // rdTheory
            // 
            this.rdTheory.AutoSize = true;
            this.rdTheory.Location = new System.Drawing.Point(7, 52);
            this.rdTheory.Name = "rdTheory";
            this.rdTheory.Size = new System.Drawing.Size(58, 17);
            this.rdTheory.TabIndex = 10;
            this.rdTheory.Text = "Theory";
            this.rdTheory.UseVisualStyleBackColor = true;
            // 
            // rdLab
            // 
            this.rdLab.AutoSize = true;
            this.rdLab.Checked = true;
            this.rdLab.Location = new System.Drawing.Point(7, 25);
            this.rdLab.Name = "rdLab";
            this.rdLab.Size = new System.Drawing.Size(43, 17);
            this.rdLab.TabIndex = 9;
            this.rdLab.TabStop = true;
            this.rdLab.Text = "Lab";
            this.rdLab.UseVisualStyleBackColor = true;
            // 
            // grpType
            // 
            this.grpType.Controls.Add(this.rdLab);
            this.grpType.Controls.Add(this.rdTheory);
            this.grpType.Location = new System.Drawing.Point(9, 30);
            this.grpType.Name = "grpType";
            this.grpType.Size = new System.Drawing.Size(227, 84);
            this.grpType.TabIndex = 17;
            this.grpType.TabStop = false;
            this.grpType.Text = "Clockin Type";
            // 
            // mnuMain
            // 
            this.mnuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuNewUser,
            this.mnuAdmin});
            this.mnuMain.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.mnuMain.Location = new System.Drawing.Point(0, 0);
            this.mnuMain.Name = "mnuMain";
            this.mnuMain.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.mnuMain.Size = new System.Drawing.Size(353, 24);
            this.mnuMain.TabIndex = 18;
            this.mnuMain.Text = "menuStrip1";
            // 
            // mnuNewUser
            // 
            this.mnuNewUser.Name = "mnuNewUser";
            this.mnuNewUser.Size = new System.Drawing.Size(69, 20);
            this.mnuNewUser.Text = "New User";
            this.mnuNewUser.Click += new System.EventHandler(this.mnuNewUser_Click);
            // 
            // mnuAdmin
            // 
            this.mnuAdmin.Name = "mnuAdmin";
            this.mnuAdmin.Size = new System.Drawing.Size(55, 20);
            this.mnuAdmin.Text = "Admin";
            this.mnuAdmin.Click += new System.EventHandler(this.mnuAdmin_Click);
            // 
            // btnTouchLogin
            // 
            this.btnTouchLogin.Location = new System.Drawing.Point(119, 94);
            this.btnTouchLogin.Name = "btnTouchLogin";
            this.btnTouchLogin.Size = new System.Drawing.Size(100, 23);
            this.btnTouchLogin.TabIndex = 19;
            this.btnTouchLogin.Text = "Touch Login";
            this.btnTouchLogin.UseVisualStyleBackColor = true;
            this.btnTouchLogin.Click += new System.EventHandler(this.btnTouchLogin_Click);
            // 
            // frmMain2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(353, 342);
            this.Controls.Add(this.grpType);
            this.Controls.Add(this.grpLogin);
            this.Controls.Add(this.lblLastFullPunch);
            this.Controls.Add(this.lblPunchOut);
            this.Controls.Add(this.lblPunchIn);
            this.Controls.Add(this.btnPunchOut);
            this.Controls.Add(this.btnPunchIn);
            this.Controls.Add(this.mnuMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.mnuMain;
            this.MaximizeBox = false;
            this.Name = "frmMain2";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CosmoClock";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain2_FormClosing);
            this.Load += new System.EventHandler(this.frmMain2_Load);
            this.grpLogin.ResumeLayout(false);
            this.grpLogin.PerformLayout();
            this.grpType.ResumeLayout(false);
            this.grpType.PerformLayout();
            this.mnuMain.ResumeLayout(false);
            this.mnuMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnPunchIn;
        private System.Windows.Forms.TextBox txtUserID;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Button btnPunchOut;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Label lblPunchIn;
        private System.Windows.Forms.Label lblPunchOut;
        private System.Windows.Forms.Label lblLastFullPunch;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.GroupBox grpLogin;
        private System.Windows.Forms.RadioButton rdTheory;
        private System.Windows.Forms.RadioButton rdLab;
        private System.Windows.Forms.Button btnLogOut;
        private System.Windows.Forms.GroupBox grpType;
        private System.Windows.Forms.MenuStrip mnuMain;
        private System.Windows.Forms.ToolStripMenuItem mnuAdmin;
        private System.Windows.Forms.ToolStripMenuItem mnuNewUser;
        private System.Windows.Forms.Button btnFingerprintLogin;
        private System.Windows.Forms.Button btnTouchLogin;
    }
}

