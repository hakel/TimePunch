namespace TimePunch
{
    partial class frmAdmin
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
            this.txtDataResults = new System.Windows.Forms.TextBox();
            this.btnDataDump = new System.Windows.Forms.Button();
            this.btnOneTimeSetup = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.tabAdmin = new System.Windows.Forms.TabControl();
            this.tabPage9 = new System.Windows.Forms.TabPage();
            this.label3 = new System.Windows.Forms.Label();
            this.grdClockOut = new System.Windows.Forms.DataGridView();
            this.dtClockOutAll = new System.Windows.Forms.DateTimePicker();
            this.btnClockOutAll = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tbShowDay = new System.Windows.Forms.TabControl();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.txtDayResults = new System.Windows.Forms.TextBox();
            this.tabPage6 = new System.Windows.Forms.TabPage();
            this.grdUserHours = new System.Windows.Forms.DataGridView();
            this.btnUpdateHours = new System.Windows.Forms.Button();
            this.cboUsers = new System.Windows.Forms.ComboBox();
            this.rdLab = new System.Windows.Forms.RadioButton();
            this.rdTheory = new System.Windows.Forms.RadioButton();
            this.btnShowDateMinutes = new System.Windows.Forms.Button();
            this.btnAddMinutes = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.txtMinutesToAdd = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.dtUpdateHours = new System.Windows.Forms.DateTimePicker();
            this.label6 = new System.Windows.Forms.Label();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.btnUpdateUser = new System.Windows.Forms.Button();
            this.cboUsersForPassword = new System.Windows.Forms.ComboBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.chkIncludeReadableDates = new System.Windows.Forms.CheckBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage8 = new System.Windows.Forms.TabPage();
            this.grdDataResults = new System.Windows.Forms.DataGridView();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.tabPage7 = new System.Windows.Forms.TabPage();
            this.txtDataQuery = new System.Windows.Forms.TextBox();
            this.cboDataDump = new System.Windows.Forms.ComboBox();
            this.tabPage10 = new System.Windows.Forms.TabPage();
            this.btnLogBackup = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.txtLogBackup = new System.Windows.Forms.TextBox();
            this.btnDBBackup = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtDBBackup = new System.Windows.Forms.TextBox();
            this.tabPage11 = new System.Windows.Forms.TabPage();
            this.btnTimesheet = new System.Windows.Forms.Button();
            this.cboUsersTimesheet = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.dtTimesheet = new System.Windows.Forms.DateTimePicker();
            this.label9 = new System.Windows.Forms.Label();
            this.tabAdmin.SuspendLayout();
            this.tabPage9.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdClockOut)).BeginInit();
            this.tabPage2.SuspendLayout();
            this.tbShowDay.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.tabPage6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdUserHours)).BeginInit();
            this.tabPage1.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdDataResults)).BeginInit();
            this.tabPage4.SuspendLayout();
            this.tabPage7.SuspendLayout();
            this.tabPage10.SuspendLayout();
            this.tabPage11.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtDataResults
            // 
            this.txtDataResults.Location = new System.Drawing.Point(3, 6);
            this.txtDataResults.Multiline = true;
            this.txtDataResults.Name = "txtDataResults";
            this.txtDataResults.Size = new System.Drawing.Size(526, 177);
            this.txtDataResults.TabIndex = 7;
            // 
            // btnDataDump
            // 
            this.btnDataDump.Location = new System.Drawing.Point(130, 15);
            this.btnDataDump.Name = "btnDataDump";
            this.btnDataDump.Size = new System.Drawing.Size(75, 23);
            this.btnDataDump.TabIndex = 5;
            this.btnDataDump.Text = "Run Query";
            this.btnDataDump.UseVisualStyleBackColor = true;
            this.btnDataDump.Click += new System.EventHandler(this.btnDataDump_Click);
            // 
            // btnOneTimeSetup
            // 
            this.btnOneTimeSetup.Location = new System.Drawing.Point(430, 13);
            this.btnOneTimeSetup.Name = "btnOneTimeSetup";
            this.btnOneTimeSetup.Size = new System.Drawing.Size(109, 23);
            this.btnOneTimeSetup.TabIndex = 6;
            this.btnOneTimeSetup.Text = "One Time Setup";
            this.btnOneTimeSetup.UseVisualStyleBackColor = true;
            this.btnOneTimeSetup.Click += new System.EventHandler(this.btnOneTimeSetup_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(2, 8);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(29, 13);
            this.label5.TabIndex = 31;
            this.label5.Text = "User";
            // 
            // tabAdmin
            // 
            this.tabAdmin.Controls.Add(this.tabPage9);
            this.tabAdmin.Controls.Add(this.tabPage2);
            this.tabAdmin.Controls.Add(this.tabPage1);
            this.tabAdmin.Controls.Add(this.tabPage3);
            this.tabAdmin.Controls.Add(this.tabPage10);
            this.tabAdmin.Controls.Add(this.tabPage11);
            this.tabAdmin.Location = new System.Drawing.Point(12, 10);
            this.tabAdmin.Name = "tabAdmin";
            this.tabAdmin.SelectedIndex = 0;
            this.tabAdmin.Size = new System.Drawing.Size(559, 291);
            this.tabAdmin.TabIndex = 31;
            // 
            // tabPage9
            // 
            this.tabPage9.Controls.Add(this.label3);
            this.tabPage9.Controls.Add(this.grdClockOut);
            this.tabPage9.Controls.Add(this.dtClockOutAll);
            this.tabPage9.Controls.Add(this.btnClockOutAll);
            this.tabPage9.Location = new System.Drawing.Point(4, 22);
            this.tabPage9.Name = "tabPage9";
            this.tabPage9.Size = new System.Drawing.Size(551, 265);
            this.tabPage9.TabIndex = 5;
            this.tabPage9.Text = "Clock Out All";
            this.tabPage9.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 40);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(127, 13);
            this.label3.TabIndex = 49;
            this.label3.Text = "Users who are clocked in";
            // 
            // grdClockOut
            // 
            this.grdClockOut.AllowUserToAddRows = false;
            this.grdClockOut.AllowUserToDeleteRows = false;
            this.grdClockOut.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.grdClockOut.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            this.grdClockOut.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdClockOut.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.grdClockOut.Location = new System.Drawing.Point(13, 56);
            this.grdClockOut.Name = "grdClockOut";
            this.grdClockOut.RowHeadersWidth = 20;
            this.grdClockOut.Size = new System.Drawing.Size(522, 167);
            this.grdClockOut.TabIndex = 48;
            // 
            // dtClockOutAll
            // 
            this.dtClockOutAll.Location = new System.Drawing.Point(14, 12);
            this.dtClockOutAll.Name = "dtClockOutAll";
            this.dtClockOutAll.Size = new System.Drawing.Size(200, 20);
            this.dtClockOutAll.TabIndex = 42;
            this.dtClockOutAll.ValueChanged += new System.EventHandler(this.dtClockOutAll_ValueChanged);
            // 
            // btnClockOutAll
            // 
            this.btnClockOutAll.Location = new System.Drawing.Point(13, 232);
            this.btnClockOutAll.Name = "btnClockOutAll";
            this.btnClockOutAll.Size = new System.Drawing.Size(109, 23);
            this.btnClockOutAll.TabIndex = 41;
            this.btnClockOutAll.Text = "Clock Out All";
            this.btnClockOutAll.UseVisualStyleBackColor = true;
            this.btnClockOutAll.Click += new System.EventHandler(this.btnClockOutAll_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.tbShowDay);
            this.tabPage2.Controls.Add(this.cboUsers);
            this.tabPage2.Controls.Add(this.rdLab);
            this.tabPage2.Controls.Add(this.rdTheory);
            this.tabPage2.Controls.Add(this.btnShowDateMinutes);
            this.tabPage2.Controls.Add(this.btnAddMinutes);
            this.tabPage2.Controls.Add(this.label8);
            this.tabPage2.Controls.Add(this.txtMinutesToAdd);
            this.tabPage2.Controls.Add(this.label7);
            this.tabPage2.Controls.Add(this.dtUpdateHours);
            this.tabPage2.Controls.Add(this.label6);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(551, 265);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Update Hours";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tbShowDay
            // 
            this.tbShowDay.Controls.Add(this.tabPage5);
            this.tbShowDay.Controls.Add(this.tabPage6);
            this.tbShowDay.Location = new System.Drawing.Point(6, 118);
            this.tbShowDay.Name = "tbShowDay";
            this.tbShowDay.SelectedIndex = 0;
            this.tbShowDay.Size = new System.Drawing.Size(539, 144);
            this.tbShowDay.TabIndex = 32;
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.txtDayResults);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(531, 118);
            this.tabPage5.TabIndex = 0;
            this.tabPage5.Text = "Info";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // txtDayResults
            // 
            this.txtDayResults.Location = new System.Drawing.Point(5, 6);
            this.txtDayResults.Multiline = true;
            this.txtDayResults.Name = "txtDayResults";
            this.txtDayResults.Size = new System.Drawing.Size(520, 106);
            this.txtDayResults.TabIndex = 45;
            // 
            // tabPage6
            // 
            this.tabPage6.Controls.Add(this.grdUserHours);
            this.tabPage6.Controls.Add(this.btnUpdateHours);
            this.tabPage6.Location = new System.Drawing.Point(4, 22);
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage6.Size = new System.Drawing.Size(531, 118);
            this.tabPage6.TabIndex = 1;
            this.tabPage6.Text = "Edit";
            this.tabPage6.UseVisualStyleBackColor = true;
            // 
            // grdUserHours
            // 
            this.grdUserHours.AllowUserToAddRows = false;
            this.grdUserHours.AllowUserToDeleteRows = false;
            this.grdUserHours.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdUserHours.Location = new System.Drawing.Point(4, 3);
            this.grdUserHours.Name = "grdUserHours";
            this.grdUserHours.RowHeadersVisible = false;
            this.grdUserHours.Size = new System.Drawing.Size(522, 80);
            this.grdUserHours.TabIndex = 46;
            this.grdUserHours.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdUserHours_CellValueChanged);
            // 
            // btnUpdateHours
            // 
            this.btnUpdateHours.Location = new System.Drawing.Point(6, 89);
            this.btnUpdateHours.Name = "btnUpdateHours";
            this.btnUpdateHours.Size = new System.Drawing.Size(75, 23);
            this.btnUpdateHours.TabIndex = 47;
            this.btnUpdateHours.Text = "Update";
            this.btnUpdateHours.UseVisualStyleBackColor = true;
            this.btnUpdateHours.Click += new System.EventHandler(this.btnUpdateHours_Click);
            // 
            // cboUsers
            // 
            this.cboUsers.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboUsers.FormattingEnabled = true;
            this.cboUsers.Location = new System.Drawing.Point(6, 25);
            this.cboUsers.Name = "cboUsers";
            this.cboUsers.Size = new System.Drawing.Size(178, 21);
            this.cboUsers.TabIndex = 44;
            // 
            // rdLab
            // 
            this.rdLab.AutoSize = true;
            this.rdLab.Checked = true;
            this.rdLab.Location = new System.Drawing.Point(204, 10);
            this.rdLab.Name = "rdLab";
            this.rdLab.Size = new System.Drawing.Size(43, 17);
            this.rdLab.TabIndex = 42;
            this.rdLab.TabStop = true;
            this.rdLab.Text = "Lab";
            this.rdLab.UseVisualStyleBackColor = true;
            // 
            // rdTheory
            // 
            this.rdTheory.AutoSize = true;
            this.rdTheory.Location = new System.Drawing.Point(204, 37);
            this.rdTheory.Name = "rdTheory";
            this.rdTheory.Size = new System.Drawing.Size(58, 17);
            this.rdTheory.TabIndex = 43;
            this.rdTheory.Text = "Theory";
            this.rdTheory.UseVisualStyleBackColor = true;
            // 
            // btnShowDateMinutes
            // 
            this.btnShowDateMinutes.Location = new System.Drawing.Point(6, 91);
            this.btnShowDateMinutes.Name = "btnShowDateMinutes";
            this.btnShowDateMinutes.Size = new System.Drawing.Size(75, 23);
            this.btnShowDateMinutes.TabIndex = 40;
            this.btnShowDateMinutes.Text = "Show Day";
            this.btnShowDateMinutes.UseVisualStyleBackColor = true;
            this.btnShowDateMinutes.Click += new System.EventHandler(this.btnShowDateMinutes_Click);
            // 
            // btnAddMinutes
            // 
            this.btnAddMinutes.Location = new System.Drawing.Point(400, 52);
            this.btnAddMinutes.Name = "btnAddMinutes";
            this.btnAddMinutes.Size = new System.Drawing.Size(75, 23);
            this.btnAddMinutes.TabIndex = 39;
            this.btnAddMinutes.Text = "Add Minutes";
            this.btnAddMinutes.UseVisualStyleBackColor = true;
            this.btnAddMinutes.Click += new System.EventHandler(this.btnAddMinutes_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(397, 10);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(78, 13);
            this.label8.TabIndex = 37;
            this.label8.Text = "Minutes to Add";
            // 
            // txtMinutesToAdd
            // 
            this.txtMinutesToAdd.Location = new System.Drawing.Point(400, 26);
            this.txtMinutesToAdd.Name = "txtMinutesToAdd";
            this.txtMinutesToAdd.Size = new System.Drawing.Size(100, 20);
            this.txtMinutesToAdd.TabIndex = 36;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 49);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(30, 13);
            this.label7.TabIndex = 35;
            this.label7.Text = "Date";
            // 
            // dtUpdateHours
            // 
            this.dtUpdateHours.Location = new System.Drawing.Point(6, 65);
            this.dtUpdateHours.Name = "dtUpdateHours";
            this.dtUpdateHours.Size = new System.Drawing.Size(200, 20);
            this.dtUpdateHours.TabIndex = 34;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(3, 10);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(40, 13);
            this.label6.TabIndex = 33;
            this.label6.Text = "UserID";
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.btnUpdateUser);
            this.tabPage1.Controls.Add(this.cboUsersForPassword);
            this.tabPage1.Controls.Add(this.label5);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(551, 265);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "User Info";
            this.tabPage1.UseVisualStyleBackColor = true;
            this.tabPage1.Click += new System.EventHandler(this.tabPage1_Click);
            // 
            // btnUpdateUser
            // 
            this.btnUpdateUser.Location = new System.Drawing.Point(6, 48);
            this.btnUpdateUser.Name = "btnUpdateUser";
            this.btnUpdateUser.Size = new System.Drawing.Size(75, 23);
            this.btnUpdateUser.TabIndex = 46;
            this.btnUpdateUser.Text = "Update";
            this.btnUpdateUser.UseVisualStyleBackColor = true;
            this.btnUpdateUser.Click += new System.EventHandler(this.btnUpdateUser_Click);
            // 
            // cboUsersForPassword
            // 
            this.cboUsersForPassword.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboUsersForPassword.FormattingEnabled = true;
            this.cboUsersForPassword.Location = new System.Drawing.Point(6, 21);
            this.cboUsersForPassword.Name = "cboUsersForPassword";
            this.cboUsersForPassword.Size = new System.Drawing.Size(178, 21);
            this.cboUsersForPassword.TabIndex = 45;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.chkIncludeReadableDates);
            this.tabPage3.Controls.Add(this.tabControl1);
            this.tabPage3.Controls.Add(this.cboDataDump);
            this.tabPage3.Controls.Add(this.btnDataDump);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(551, 265);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Reports";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // chkIncludeReadableDates
            // 
            this.chkIncludeReadableDates.AutoSize = true;
            this.chkIncludeReadableDates.Checked = true;
            this.chkIncludeReadableDates.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkIncludeReadableDates.Location = new System.Drawing.Point(211, 19);
            this.chkIncludeReadableDates.Name = "chkIncludeReadableDates";
            this.chkIncludeReadableDates.Size = new System.Drawing.Size(125, 17);
            this.chkIncludeReadableDates.TabIndex = 33;
            this.chkIncludeReadableDates.Text = "Add Readable Dates";
            this.chkIncludeReadableDates.UseVisualStyleBackColor = true;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage8);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Controls.Add(this.tabPage7);
            this.tabControl1.Location = new System.Drawing.Point(5, 48);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(543, 214);
            this.tabControl1.TabIndex = 32;
            // 
            // tabPage8
            // 
            this.tabPage8.Controls.Add(this.grdDataResults);
            this.tabPage8.Location = new System.Drawing.Point(4, 22);
            this.tabPage8.Name = "tabPage8";
            this.tabPage8.Size = new System.Drawing.Size(535, 188);
            this.tabPage8.TabIndex = 2;
            this.tabPage8.Text = "Results Grid";
            this.tabPage8.UseVisualStyleBackColor = true;
            // 
            // grdDataResults
            // 
            this.grdDataResults.AllowUserToAddRows = false;
            this.grdDataResults.AllowUserToDeleteRows = false;
            this.grdDataResults.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.grdDataResults.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            this.grdDataResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdDataResults.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.grdDataResults.Location = new System.Drawing.Point(6, 3);
            this.grdDataResults.Name = "grdDataResults";
            this.grdDataResults.RowHeadersWidth = 20;
            this.grdDataResults.Size = new System.Drawing.Size(522, 182);
            this.grdDataResults.TabIndex = 47;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.txtDataResults);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(535, 188);
            this.tabPage4.TabIndex = 0;
            this.tabPage4.Text = "Results Text";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // tabPage7
            // 
            this.tabPage7.Controls.Add(this.txtDataQuery);
            this.tabPage7.Location = new System.Drawing.Point(4, 22);
            this.tabPage7.Name = "tabPage7";
            this.tabPage7.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage7.Size = new System.Drawing.Size(535, 188);
            this.tabPage7.TabIndex = 1;
            this.tabPage7.Text = "Query";
            this.tabPage7.UseVisualStyleBackColor = true;
            // 
            // txtDataQuery
            // 
            this.txtDataQuery.Location = new System.Drawing.Point(4, 6);
            this.txtDataQuery.Multiline = true;
            this.txtDataQuery.Name = "txtDataQuery";
            this.txtDataQuery.Size = new System.Drawing.Size(526, 177);
            this.txtDataQuery.TabIndex = 8;
            // 
            // cboDataDump
            // 
            this.cboDataDump.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDataDump.FormattingEnabled = true;
            this.cboDataDump.Location = new System.Drawing.Point(3, 17);
            this.cboDataDump.Name = "cboDataDump";
            this.cboDataDump.Size = new System.Drawing.Size(121, 21);
            this.cboDataDump.TabIndex = 8;
            // 
            // tabPage10
            // 
            this.tabPage10.Controls.Add(this.btnLogBackup);
            this.tabPage10.Controls.Add(this.label2);
            this.tabPage10.Controls.Add(this.txtLogBackup);
            this.tabPage10.Controls.Add(this.btnDBBackup);
            this.tabPage10.Controls.Add(this.label1);
            this.tabPage10.Controls.Add(this.txtDBBackup);
            this.tabPage10.Controls.Add(this.btnOneTimeSetup);
            this.tabPage10.Location = new System.Drawing.Point(4, 22);
            this.tabPage10.Name = "tabPage10";
            this.tabPage10.Size = new System.Drawing.Size(551, 265);
            this.tabPage10.TabIndex = 4;
            this.tabPage10.Text = "Utilities";
            this.tabPage10.UseVisualStyleBackColor = true;
            // 
            // btnLogBackup
            // 
            this.btnLogBackup.Location = new System.Drawing.Point(12, 137);
            this.btnLogBackup.Name = "btnLogBackup";
            this.btnLogBackup.Size = new System.Drawing.Size(109, 23);
            this.btnLogBackup.TabIndex = 43;
            this.btnLogBackup.Text = "Backup Logs";
            this.btnLogBackup.UseVisualStyleBackColor = true;
            this.btnLogBackup.Click += new System.EventHandler(this.btnLogBackup_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 95);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(90, 13);
            this.label2.TabIndex = 42;
            this.label2.Text = "Log Backup Path";
            // 
            // txtLogBackup
            // 
            this.txtLogBackup.Location = new System.Drawing.Point(12, 111);
            this.txtLogBackup.Name = "txtLogBackup";
            this.txtLogBackup.Size = new System.Drawing.Size(264, 20);
            this.txtLogBackup.TabIndex = 41;
            // 
            // btnDBBackup
            // 
            this.btnDBBackup.Location = new System.Drawing.Point(12, 53);
            this.btnDBBackup.Name = "btnDBBackup";
            this.btnDBBackup.Size = new System.Drawing.Size(109, 23);
            this.btnDBBackup.TabIndex = 40;
            this.btnDBBackup.Text = "Backup Database";
            this.btnDBBackup.UseVisualStyleBackColor = true;
            this.btnDBBackup.Click += new System.EventHandler(this.btnDBBackup_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(118, 13);
            this.label1.TabIndex = 39;
            this.label1.Text = "Database Backup Path";
            // 
            // txtDBBackup
            // 
            this.txtDBBackup.Location = new System.Drawing.Point(12, 27);
            this.txtDBBackup.Name = "txtDBBackup";
            this.txtDBBackup.Size = new System.Drawing.Size(264, 20);
            this.txtDBBackup.TabIndex = 38;
            // 
            // tabPage11
            // 
            this.tabPage11.Controls.Add(this.btnTimesheet);
            this.tabPage11.Controls.Add(this.cboUsersTimesheet);
            this.tabPage11.Controls.Add(this.label4);
            this.tabPage11.Controls.Add(this.dtTimesheet);
            this.tabPage11.Controls.Add(this.label9);
            this.tabPage11.Location = new System.Drawing.Point(4, 22);
            this.tabPage11.Name = "tabPage11";
            this.tabPage11.Size = new System.Drawing.Size(551, 265);
            this.tabPage11.TabIndex = 6;
            this.tabPage11.Text = "Timesheet";
            this.tabPage11.UseVisualStyleBackColor = true;
            // 
            // btnTimesheet
            // 
            this.btnTimesheet.Location = new System.Drawing.Point(13, 116);
            this.btnTimesheet.Name = "btnTimesheet";
            this.btnTimesheet.Size = new System.Drawing.Size(88, 23);
            this.btnTimesheet.TabIndex = 52;
            this.btnTimesheet.Text = "Preview";
            this.btnTimesheet.UseVisualStyleBackColor = true;
            this.btnTimesheet.Click += new System.EventHandler(this.button1_Click);
            // 
            // cboUsersTimesheet
            // 
            this.cboUsersTimesheet.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboUsersTimesheet.FormattingEnabled = true;
            this.cboUsersTimesheet.Location = new System.Drawing.Point(13, 31);
            this.cboUsersTimesheet.Name = "cboUsersTimesheet";
            this.cboUsersTimesheet.Size = new System.Drawing.Size(178, 21);
            this.cboUsersTimesheet.TabIndex = 51;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 55);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(30, 13);
            this.label4.TabIndex = 48;
            this.label4.Text = "Date";
            // 
            // dtTimesheet
            // 
            this.dtTimesheet.Location = new System.Drawing.Point(13, 71);
            this.dtTimesheet.Name = "dtTimesheet";
            this.dtTimesheet.Size = new System.Drawing.Size(200, 20);
            this.dtTimesheet.TabIndex = 47;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(10, 16);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(40, 13);
            this.label9.TabIndex = 46;
            this.label9.Text = "UserID";
            // 
            // frmAdmin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(577, 305);
            this.Controls.Add(this.tabAdmin);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "frmAdmin";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "CosmoClock Admin";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmAdmin_FormClosing);
            this.Load += new System.EventHandler(this.frmAdmin_Load);
            this.tabAdmin.ResumeLayout(false);
            this.tabPage9.ResumeLayout(false);
            this.tabPage9.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdClockOut)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tbShowDay.ResumeLayout(false);
            this.tabPage5.ResumeLayout(false);
            this.tabPage5.PerformLayout();
            this.tabPage6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdUserHours)).EndInit();
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage8.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdDataResults)).EndInit();
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            this.tabPage7.ResumeLayout(false);
            this.tabPage7.PerformLayout();
            this.tabPage10.ResumeLayout(false);
            this.tabPage10.PerformLayout();
            this.tabPage11.ResumeLayout(false);
            this.tabPage11.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtDataResults;
        private System.Windows.Forms.Button btnDataDump;
        private System.Windows.Forms.Button btnOneTimeSetup;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TabControl tabAdmin;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button btnAddMinutes;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtMinutesToAdd;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.DateTimePicker dtUpdateHours;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnShowDateMinutes;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.ComboBox cboDataDump;
        private System.Windows.Forms.RadioButton rdLab;
        private System.Windows.Forms.RadioButton rdTheory;
        private System.Windows.Forms.ComboBox cboUsers;
        private System.Windows.Forms.TextBox txtDayResults;
        private System.Windows.Forms.ComboBox cboUsersForPassword;
        private System.Windows.Forms.Button btnUpdateUser;
        private System.Windows.Forms.DataGridView grdUserHours;
        private System.Windows.Forms.Button btnUpdateHours;
        private System.Windows.Forms.TabControl tbShowDay;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.TabPage tabPage6;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.TabPage tabPage7;
        private System.Windows.Forms.TextBox txtDataQuery;
        private System.Windows.Forms.CheckBox chkIncludeReadableDates;
        private System.Windows.Forms.TabPage tabPage8;
        private System.Windows.Forms.DataGridView grdDataResults;
        private System.Windows.Forms.TabPage tabPage10;
        private System.Windows.Forms.Button btnDBBackup;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtDBBackup;
        private System.Windows.Forms.Button btnLogBackup;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtLogBackup;
        private System.Windows.Forms.TabPage tabPage9;
        private System.Windows.Forms.Button btnClockOutAll;
        private System.Windows.Forms.DateTimePicker dtClockOutAll;
        private System.Windows.Forms.DataGridView grdClockOut;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TabPage tabPage11;
        private System.Windows.Forms.Button btnTimesheet;
        private System.Windows.Forms.ComboBox cboUsersTimesheet;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker dtTimesheet;
        private System.Windows.Forms.Label label9;
    }
}