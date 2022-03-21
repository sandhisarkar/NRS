namespace ImageHeaven
{
    partial class frmEntry
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmEntry));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtBatch = new System.Windows.Forms.TextBox();
            this.labelBatch = new System.Windows.Forms.Label();
            this.txtProject = new System.Windows.Forms.TextBox();
            this.labelProject = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cmbMonth = new System.Windows.Forms.ComboBox();
            this.label15 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.txtDate = new System.Windows.Forms.TextBox();
            this.txtYear = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.labelIssuedDate = new System.Windows.Forms.Label();
            this.cmbDocType = new System.Windows.Forms.ComboBox();
            this.labelDocType = new System.Windows.Forms.Label();
            this.txtSubject = new System.Windows.Forms.TextBox();
            this.labelSubject = new System.Windows.Forms.Label();
            this.cmbSubCat = new System.Windows.Forms.ComboBox();
            this.labelSubCat = new System.Windows.Forms.Label();
            this.txtIssuedTo = new System.Windows.Forms.TextBox();
            this.labelIssuedTo = new System.Windows.Forms.Label();
            this.txtIssuedFrom = new System.Windows.Forms.TextBox();
            this.labelIssuedFrom = new System.Windows.Forms.Label();
            this.txtLetterNo = new System.Windows.Forms.TextBox();
            this.labelLetter = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.buttonExit = new System.Windows.Forms.Button();
            this.buttonSave = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.cmbFile = new System.Windows.Forms.ComboBox();
            this.label19 = new System.Windows.Forms.Label();
            this.cmdBrowse = new System.Windows.Forms.Button();
            this.txtPath = new System.Windows.Forms.TextBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.lstImage = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.fbdPath = new System.Windows.Forms.FolderBrowserDialog();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.picMain = new System.Windows.Forms.PictureBox();
            this.deButton2 = new nControls.deButton();
            this.deButton1 = new nControls.deButton();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picMain)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.SystemColors.Control;
            this.groupBox1.Controls.Add(this.txtBatch);
            this.groupBox1.Controls.Add(this.labelBatch);
            this.groupBox1.Controls.Add(this.txtProject);
            this.groupBox1.Controls.Add(this.labelProject);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(4, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(554, 102);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Project-Batch Details :";
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // txtBatch
            // 
            this.txtBatch.Enabled = false;
            this.txtBatch.Location = new System.Drawing.Point(148, 61);
            this.txtBatch.Name = "txtBatch";
            this.txtBatch.Size = new System.Drawing.Size(340, 21);
            this.txtBatch.TabIndex = 3;
            this.txtBatch.TextChanged += new System.EventHandler(this.txtBatch_TextChanged);
            // 
            // labelBatch
            // 
            this.labelBatch.AutoSize = true;
            this.labelBatch.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelBatch.Location = new System.Drawing.Point(49, 64);
            this.labelBatch.Name = "labelBatch";
            this.labelBatch.Size = new System.Drawing.Size(88, 13);
            this.labelBatch.TabIndex = 2;
            this.labelBatch.Text = "Batch Name : ";
            this.labelBatch.Click += new System.EventHandler(this.labelBatch_Click);
            // 
            // txtProject
            // 
            this.txtProject.Enabled = false;
            this.txtProject.Location = new System.Drawing.Point(148, 26);
            this.txtProject.Name = "txtProject";
            this.txtProject.Size = new System.Drawing.Size(341, 21);
            this.txtProject.TabIndex = 1;
            this.txtProject.TextChanged += new System.EventHandler(this.txtProject_TextChanged);
            // 
            // labelProject
            // 
            this.labelProject.AutoSize = true;
            this.labelProject.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelProject.Location = new System.Drawing.Point(42, 30);
            this.labelProject.Name = "labelProject";
            this.labelProject.Size = new System.Drawing.Size(95, 13);
            this.labelProject.TabIndex = 0;
            this.labelProject.Text = "Project Name : ";
            this.labelProject.Click += new System.EventHandler(this.labelProject_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.SystemColors.Control;
            this.groupBox2.Controls.Add(this.cmbMonth);
            this.groupBox2.Controls.Add(this.label15);
            this.groupBox2.Controls.Add(this.label14);
            this.groupBox2.Controls.Add(this.label13);
            this.groupBox2.Controls.Add(this.label12);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.txtDate);
            this.groupBox2.Controls.Add(this.txtYear);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.labelIssuedDate);
            this.groupBox2.Controls.Add(this.cmbDocType);
            this.groupBox2.Controls.Add(this.labelDocType);
            this.groupBox2.Controls.Add(this.txtSubject);
            this.groupBox2.Controls.Add(this.labelSubject);
            this.groupBox2.Controls.Add(this.cmbSubCat);
            this.groupBox2.Controls.Add(this.labelSubCat);
            this.groupBox2.Controls.Add(this.txtIssuedTo);
            this.groupBox2.Controls.Add(this.labelIssuedTo);
            this.groupBox2.Controls.Add(this.txtIssuedFrom);
            this.groupBox2.Controls.Add(this.labelIssuedFrom);
            this.groupBox2.Controls.Add(this.txtLetterNo);
            this.groupBox2.Controls.Add(this.labelLetter);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(4, 110);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(554, 359);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Metadata Fields :";
            this.groupBox2.Enter += new System.EventHandler(this.groupBox2_Enter);
            // 
            // cmbMonth
            // 
            this.cmbMonth.BackColor = System.Drawing.SystemColors.Window;
            this.cmbMonth.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMonth.FormattingEnabled = true;
            this.cmbMonth.Items.AddRange(new object[] {
            "01",
            "02",
            "03",
            "04",
            "05",
            "06",
            "07",
            "08",
            "09",
            "10",
            "11",
            "12"});
            this.cmbMonth.Location = new System.Drawing.Point(231, 286);
            this.cmbMonth.Name = "cmbMonth";
            this.cmbMonth.Size = new System.Drawing.Size(50, 23);
            this.cmbMonth.TabIndex = 23;
            this.cmbMonth.SelectedIndexChanged += new System.EventHandler(this.cmbMonth_SelectedIndexChanged);
            this.cmbMonth.CursorChanged += new System.EventHandler(this.cmbMonth_CursorChanged);
            this.cmbMonth.TabIndexChanged += new System.EventHandler(this.cmbMonth_TabIndexChanged);
            this.cmbMonth.TextChanged += new System.EventHandler(this.cmbMonth_TextChanged);
            this.cmbMonth.Click += new System.EventHandler(this.cmbMonth_Click);
            this.cmbMonth.Enter += new System.EventHandler(this.cmbMonth_Enter);
            this.cmbMonth.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cmbMonth_KeyDown);
            this.cmbMonth.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cmbMonth_KeyPress);
            this.cmbMonth.KeyUp += new System.Windows.Forms.KeyEventHandler(this.cmbMonth_KeyUp);
            this.cmbMonth.Leave += new System.EventHandler(this.cmbMonth_Leave);
            this.cmbMonth.MouseClick += new System.Windows.Forms.MouseEventHandler(this.cmbMonth_MouseClick);
            this.cmbMonth.MouseLeave += new System.EventHandler(this.cmbMonth_MouseLeave);
            this.cmbMonth.MouseHover += new System.EventHandler(this.cmbMonth_MouseHover);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.Location = new System.Drawing.Point(297, 329);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(58, 12);
            this.label15.TabIndex = 32;
            this.label15.Text = "[Format : dd]";
            this.label15.Click += new System.EventHandler(this.label15_Click);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(221, 329);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(64, 12);
            this.label14.TabIndex = 31;
            this.label14.Text = "[Format : mm]";
            this.label14.Click += new System.EventHandler(this.label14_Click);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(139, 328);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(68, 12);
            this.label13.TabIndex = 30;
            this.label13.Text = "[Format : yyyy]";
            this.label13.Click += new System.EventHandler(this.label13_Click);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(306, 309);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(36, 13);
            this.label12.TabIndex = 29;
            this.label12.Text = "(Date)";
            this.label12.Click += new System.EventHandler(this.label12_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(234, 310);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(43, 13);
            this.label11.TabIndex = 28;
            this.label11.Text = "(Month)";
            this.label11.Click += new System.EventHandler(this.label11_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(159, 310);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(35, 13);
            this.label10.TabIndex = 27;
            this.label10.Text = "(Year)";
            this.label10.Click += new System.EventHandler(this.label10_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(283, 288);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(13, 18);
            this.label9.TabIndex = 26;
            this.label9.Text = "-";
            this.label9.Click += new System.EventHandler(this.label9_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(216, 288);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(13, 18);
            this.label8.TabIndex = 25;
            this.label8.Text = "-";
            this.label8.Click += new System.EventHandler(this.label8_Click);
            // 
            // txtDate
            // 
            this.txtDate.Location = new System.Drawing.Point(299, 287);
            this.txtDate.MaxLength = 2;
            this.txtDate.Name = "txtDate";
            this.txtDate.Size = new System.Drawing.Size(48, 21);
            this.txtDate.TabIndex = 24;
            this.txtDate.Click += new System.EventHandler(this.txtDate_Click);
            this.txtDate.MouseClick += new System.Windows.Forms.MouseEventHandler(this.txtDate_MouseClick);
            this.txtDate.TabIndexChanged += new System.EventHandler(this.txtDate_TabIndexChanged);
            this.txtDate.TextChanged += new System.EventHandler(this.txtDate_TextChanged);
            this.txtDate.Enter += new System.EventHandler(this.txtDate_Enter);
            this.txtDate.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtDate_KeyDown);
            this.txtDate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtDate_KeyPress);
            this.txtDate.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtDate_KeyUp);
            this.txtDate.Leave += new System.EventHandler(this.txtDate_Leave);
            this.txtDate.MouseLeave += new System.EventHandler(this.txtDate_MouseLeave);
            this.txtDate.MouseHover += new System.EventHandler(this.txtDate_MouseHover);
            // 
            // txtYear
            // 
            this.txtYear.Location = new System.Drawing.Point(141, 287);
            this.txtYear.MaxLength = 4;
            this.txtYear.Name = "txtYear";
            this.txtYear.Size = new System.Drawing.Size(71, 21);
            this.txtYear.TabIndex = 22;
            this.txtYear.Tag = "(Year)";
            this.txtYear.Click += new System.EventHandler(this.txtYear_Click);
            this.txtYear.MouseClick += new System.Windows.Forms.MouseEventHandler(this.txtYear_MouseClick);
            this.txtYear.TabIndexChanged += new System.EventHandler(this.txtYear_TabIndexChanged);
            this.txtYear.TextChanged += new System.EventHandler(this.txtYear_TextChanged);
            this.txtYear.Enter += new System.EventHandler(this.txtYear_Enter);
            this.txtYear.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtYear_KeyDown);
            this.txtYear.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtYear_KeyPress);
            this.txtYear.Leave += new System.EventHandler(this.txtYear_Leave);
            this.txtYear.MouseLeave += new System.EventHandler(this.txtYear_MouseLeave);
            this.txtYear.MouseHover += new System.EventHandler(this.txtYear_MouseHover);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft YaHei", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label7.Location = new System.Drawing.Point(354, 292);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(0, 16);
            this.label7.TabIndex = 21;
            this.label7.Click += new System.EventHandler(this.label7_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft YaHei", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label6.Location = new System.Drawing.Point(358, 249);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(0, 16);
            this.label6.TabIndex = 20;
            this.label6.Click += new System.EventHandler(this.label6_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft YaHei", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label5.Location = new System.Drawing.Point(358, 204);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(0, 16);
            this.label5.TabIndex = 19;
            this.label5.Click += new System.EventHandler(this.label5_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft YaHei", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label4.Location = new System.Drawing.Point(361, 160);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(0, 16);
            this.label4.TabIndex = 18;
            this.label4.Click += new System.EventHandler(this.label4_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft YaHei", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label3.Location = new System.Drawing.Point(357, 116);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(0, 16);
            this.label3.TabIndex = 17;
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label2.Font = new System.Drawing.Font("Microsoft YaHei", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label2.Location = new System.Drawing.Point(359, 77);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(0, 16);
            this.label2.TabIndex = 16;
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft YaHei", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label1.Location = new System.Drawing.Point(354, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 16);
            this.label1.TabIndex = 15;
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // labelIssuedDate
            // 
            this.labelIssuedDate.AutoSize = true;
            this.labelIssuedDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelIssuedDate.Location = new System.Drawing.Point(48, 293);
            this.labelIssuedDate.Name = "labelIssuedDate";
            this.labelIssuedDate.Size = new System.Drawing.Size(83, 13);
            this.labelIssuedDate.TabIndex = 13;
            this.labelIssuedDate.Text = "Issued Date :";
            this.labelIssuedDate.Click += new System.EventHandler(this.labelIssuedDate_Click);
            // 
            // cmbDocType
            // 
            this.cmbDocType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDocType.FormattingEnabled = true;
            this.cmbDocType.Location = new System.Drawing.Point(141, 246);
            this.cmbDocType.Name = "cmbDocType";
            this.cmbDocType.Size = new System.Drawing.Size(207, 23);
            this.cmbDocType.TabIndex = 12;
            this.cmbDocType.SelectedIndexChanged += new System.EventHandler(this.cmbDocType_SelectedIndexChanged);
            this.cmbDocType.CursorChanged += new System.EventHandler(this.cmbDocType_CursorChanged);
            this.cmbDocType.TabIndexChanged += new System.EventHandler(this.cmbDocType_TabIndexChanged);
            this.cmbDocType.TextChanged += new System.EventHandler(this.cmbDocType_TextChanged);
            this.cmbDocType.Click += new System.EventHandler(this.cmbDocType_Click);
            this.cmbDocType.Enter += new System.EventHandler(this.cmbDocType_Enter);
            this.cmbDocType.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cmbDocType_KeyDown);
            this.cmbDocType.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cmbDocType_KeyPress);
            this.cmbDocType.KeyUp += new System.Windows.Forms.KeyEventHandler(this.cmbDocType_KeyUp);
            this.cmbDocType.Leave += new System.EventHandler(this.cmbDocType_Leave);
            this.cmbDocType.MouseClick += new System.Windows.Forms.MouseEventHandler(this.cmbDocType_MouseClick);
            this.cmbDocType.MouseLeave += new System.EventHandler(this.cmbDocType_MouseLeave);
            this.cmbDocType.MouseHover += new System.EventHandler(this.cmbDocType_MouseHover);
            // 
            // labelDocType
            // 
            this.labelDocType.AutoSize = true;
            this.labelDocType.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelDocType.Location = new System.Drawing.Point(27, 249);
            this.labelDocType.Name = "labelDocType";
            this.labelDocType.Size = new System.Drawing.Size(104, 13);
            this.labelDocType.TabIndex = 11;
            this.labelDocType.Text = "Document Type :";
            this.labelDocType.Click += new System.EventHandler(this.labelDocType_Click);
            // 
            // txtSubject
            // 
            this.txtSubject.Location = new System.Drawing.Point(140, 195);
            this.txtSubject.Multiline = true;
            this.txtSubject.Name = "txtSubject";
            this.txtSubject.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtSubject.Size = new System.Drawing.Size(208, 39);
            this.txtSubject.TabIndex = 10;
            this.txtSubject.Click += new System.EventHandler(this.txtSubject_Click);
            this.txtSubject.MouseClick += new System.Windows.Forms.MouseEventHandler(this.txtSubject_MouseClick);
            this.txtSubject.TabIndexChanged += new System.EventHandler(this.txtSubject_TabIndexChanged);
            this.txtSubject.TextChanged += new System.EventHandler(this.txtSubject_TextChanged);
            this.txtSubject.Enter += new System.EventHandler(this.txtSubject_Enter);
            this.txtSubject.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSubject_KeyDown);
            this.txtSubject.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSubject_KeyPress);
            this.txtSubject.Leave += new System.EventHandler(this.txtSubject_Leave);
            this.txtSubject.MouseLeave += new System.EventHandler(this.txtSubject_MouseLeave);
            this.txtSubject.MouseHover += new System.EventHandler(this.txtSubject_MouseHover);
            // 
            // labelSubject
            // 
            this.labelSubject.AutoSize = true;
            this.labelSubject.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelSubject.Location = new System.Drawing.Point(73, 201);
            this.labelSubject.Name = "labelSubject";
            this.labelSubject.Size = new System.Drawing.Size(58, 13);
            this.labelSubject.TabIndex = 9;
            this.labelSubject.Text = "Subject :";
            this.labelSubject.Click += new System.EventHandler(this.labelSubject_Click);
            // 
            // cmbSubCat
            // 
            this.cmbSubCat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSubCat.FormattingEnabled = true;
            this.cmbSubCat.Location = new System.Drawing.Point(142, 156);
            this.cmbSubCat.Name = "cmbSubCat";
            this.cmbSubCat.Size = new System.Drawing.Size(206, 23);
            this.cmbSubCat.TabIndex = 8;
            this.cmbSubCat.SelectedIndexChanged += new System.EventHandler(this.cmbSubCat_SelectedIndexChanged);
            this.cmbSubCat.CursorChanged += new System.EventHandler(this.cmbSubCat_CursorChanged);
            this.cmbSubCat.TabIndexChanged += new System.EventHandler(this.cmbSubCat_TabIndexChanged);
            this.cmbSubCat.TextChanged += new System.EventHandler(this.cmbSubCat_TextChanged);
            this.cmbSubCat.Click += new System.EventHandler(this.cmbSubCat_Click);
            this.cmbSubCat.Enter += new System.EventHandler(this.cmbSubCat_Enter);
            this.cmbSubCat.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cmbSubCat_KeyDown);
            this.cmbSubCat.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cmbSubCat_KeyPress);
            this.cmbSubCat.KeyUp += new System.Windows.Forms.KeyEventHandler(this.cmbSubCat_KeyUp);
            this.cmbSubCat.Leave += new System.EventHandler(this.cmbSubCat_Leave);
            this.cmbSubCat.MouseClick += new System.Windows.Forms.MouseEventHandler(this.cmbSubCat_MouseClick);
            this.cmbSubCat.MouseLeave += new System.EventHandler(this.cmbSubCat_MouseLeave);
            this.cmbSubCat.MouseHover += new System.EventHandler(this.cmbSubCat_MouseHover);
            // 
            // labelSubCat
            // 
            this.labelSubCat.AutoSize = true;
            this.labelSubCat.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelSubCat.Location = new System.Drawing.Point(20, 161);
            this.labelSubCat.Name = "labelSubCat";
            this.labelSubCat.Size = new System.Drawing.Size(112, 13);
            this.labelSubCat.TabIndex = 7;
            this.labelSubCat.Text = "Subject Category :";
            this.labelSubCat.Click += new System.EventHandler(this.labelSubCat_Click);
            // 
            // txtIssuedTo
            // 
            this.txtIssuedTo.Location = new System.Drawing.Point(142, 114);
            this.txtIssuedTo.Name = "txtIssuedTo";
            this.txtIssuedTo.Size = new System.Drawing.Size(205, 21);
            this.txtIssuedTo.TabIndex = 6;
            this.txtIssuedTo.Click += new System.EventHandler(this.txtIssuedTo_Click);
            this.txtIssuedTo.MouseClick += new System.Windows.Forms.MouseEventHandler(this.txtIssuedTo_MouseClick);
            this.txtIssuedTo.TabIndexChanged += new System.EventHandler(this.txtIssuedTo_TabIndexChanged);
            this.txtIssuedTo.TextChanged += new System.EventHandler(this.txtIssuedTo_TextChanged);
            this.txtIssuedTo.Enter += new System.EventHandler(this.txtIssuedTo_Enter);
            this.txtIssuedTo.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtIssuedTo_KeyDown);
            this.txtIssuedTo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtIssuedTo_KeyPress);
            this.txtIssuedTo.Leave += new System.EventHandler(this.txtIssuedTo_Leave);
            this.txtIssuedTo.MouseLeave += new System.EventHandler(this.txtIssuedTo_MouseLeave);
            this.txtIssuedTo.MouseHover += new System.EventHandler(this.txtIssuedTo_MouseHover);
            // 
            // labelIssuedTo
            // 
            this.labelIssuedTo.AutoSize = true;
            this.labelIssuedTo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelIssuedTo.Location = new System.Drawing.Point(61, 117);
            this.labelIssuedTo.Name = "labelIssuedTo";
            this.labelIssuedTo.Size = new System.Drawing.Size(71, 13);
            this.labelIssuedTo.TabIndex = 5;
            this.labelIssuedTo.Text = "Issued To :";
            this.labelIssuedTo.Click += new System.EventHandler(this.labelIssuedTo_Click);
            // 
            // txtIssuedFrom
            // 
            this.txtIssuedFrom.Location = new System.Drawing.Point(143, 74);
            this.txtIssuedFrom.Name = "txtIssuedFrom";
            this.txtIssuedFrom.Size = new System.Drawing.Size(205, 21);
            this.txtIssuedFrom.TabIndex = 4;
            this.txtIssuedFrom.Click += new System.EventHandler(this.txtIssuedFrom_Click);
            this.txtIssuedFrom.MouseClick += new System.Windows.Forms.MouseEventHandler(this.txtIssuedFrom_MouseClick);
            this.txtIssuedFrom.TabIndexChanged += new System.EventHandler(this.txtIssuedFrom_TabIndexChanged);
            this.txtIssuedFrom.TextChanged += new System.EventHandler(this.txtIssuedFrom_TextChanged);
            this.txtIssuedFrom.Enter += new System.EventHandler(this.txtIssuedFrom_Enter);
            this.txtIssuedFrom.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtIssuedFrom_KeyDown);
            this.txtIssuedFrom.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtIssuedFrom_KeyPress);
            this.txtIssuedFrom.Leave += new System.EventHandler(this.txtIssuedFrom_Leave);
            this.txtIssuedFrom.MouseLeave += new System.EventHandler(this.txtIssuedFrom_MouseLeave);
            this.txtIssuedFrom.MouseHover += new System.EventHandler(this.txtIssuedFrom_MouseHover);
            // 
            // labelIssuedFrom
            // 
            this.labelIssuedFrom.AutoSize = true;
            this.labelIssuedFrom.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelIssuedFrom.Location = new System.Drawing.Point(49, 76);
            this.labelIssuedFrom.Name = "labelIssuedFrom";
            this.labelIssuedFrom.Size = new System.Drawing.Size(83, 13);
            this.labelIssuedFrom.TabIndex = 3;
            this.labelIssuedFrom.Text = "Issued From :";
            this.labelIssuedFrom.Click += new System.EventHandler(this.labelIssuedFrom_Click);
            // 
            // txtLetterNo
            // 
            this.txtLetterNo.Location = new System.Drawing.Point(142, 34);
            this.txtLetterNo.Name = "txtLetterNo";
            this.txtLetterNo.Size = new System.Drawing.Size(206, 21);
            this.txtLetterNo.TabIndex = 2;
            this.txtLetterNo.Click += new System.EventHandler(this.txtLetterNo_Click);
            this.txtLetterNo.MouseClick += new System.Windows.Forms.MouseEventHandler(this.txtLetterNo_MouseClick);
            this.txtLetterNo.TabIndexChanged += new System.EventHandler(this.txtLetterNo_TabIndexChanged);
            this.txtLetterNo.TextChanged += new System.EventHandler(this.txtLetterNo_TextChanged);
            this.txtLetterNo.Enter += new System.EventHandler(this.txtLetterNo_Enter);
            this.txtLetterNo.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtLetterNo_KeyDown);
            this.txtLetterNo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtLetterNo_KeyPress);
            this.txtLetterNo.Leave += new System.EventHandler(this.txtLetterNo_Leave);
            this.txtLetterNo.MouseLeave += new System.EventHandler(this.txtLetterNo_MouseLeave);
            this.txtLetterNo.MouseHover += new System.EventHandler(this.txtLetterNo_MouseHover);
            // 
            // labelLetter
            // 
            this.labelLetter.AutoSize = true;
            this.labelLetter.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelLetter.Location = new System.Drawing.Point(59, 36);
            this.labelLetter.Name = "labelLetter";
            this.labelLetter.Size = new System.Drawing.Size(72, 13);
            this.labelLetter.TabIndex = 0;
            this.labelLetter.Text = "Letter No. :";
            this.labelLetter.Click += new System.EventHandler(this.labelLetter_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.buttonExit);
            this.panel1.Controls.Add(this.buttonSave);
            this.panel1.Location = new System.Drawing.Point(278, 474);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(278, 44);
            this.panel1.TabIndex = 4;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // buttonExit
            // 
            this.buttonExit.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.buttonExit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonExit.Location = new System.Drawing.Point(152, 5);
            this.buttonExit.Name = "buttonExit";
            this.buttonExit.Size = new System.Drawing.Size(105, 32);
            this.buttonExit.TabIndex = 1;
            this.buttonExit.Text = "&Exit";
            this.buttonExit.UseVisualStyleBackColor = false;
            this.buttonExit.Click += new System.EventHandler(this.buttonExit_Click);
            this.buttonExit.Leave += new System.EventHandler(this.buttonExit_Leave);
            // 
            // buttonSave
            // 
            this.buttonSave.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.buttonSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonSave.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.buttonSave.Location = new System.Drawing.Point(22, 5);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(106, 32);
            this.buttonSave.TabIndex = 0;
            this.buttonSave.Text = "&Save";
            this.buttonSave.UseVisualStyleBackColor = false;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            this.buttonSave.Leave += new System.EventHandler(this.buttonSave_Leave);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.panel2);
            this.groupBox3.Location = new System.Drawing.Point(8, 4);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(568, 548);
            this.groupBox3.TabIndex = 5;
            this.groupBox3.TabStop = false;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.groupBox2);
            this.panel2.Controls.Add(this.panel1);
            this.panel2.Controls.Add(this.groupBox1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 16);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(562, 529);
            this.panel2.TabIndex = 0;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.cmbFile);
            this.groupBox4.Controls.Add(this.label19);
            this.groupBox4.Controls.Add(this.cmdBrowse);
            this.groupBox4.Controls.Add(this.txtPath);
            this.groupBox4.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.groupBox4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox4.Location = new System.Drawing.Point(7, 4);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(581, 75);
            this.groupBox4.TabIndex = 6;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Folder- Selection :";
            // 
            // cmbFile
            // 
            this.cmbFile.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFile.FormattingEnabled = true;
            this.cmbFile.Location = new System.Drawing.Point(554, -50);
            this.cmbFile.Name = "cmbFile";
            this.cmbFile.Size = new System.Drawing.Size(175, 21);
            this.cmbFile.TabIndex = 22;
            this.cmbFile.Visible = false;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(33, 33);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(73, 13);
            this.label19.TabIndex = 10;
            this.label19.Text = "Image Source";
            // 
            // cmdBrowse
            // 
            this.cmdBrowse.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmdBrowse.Location = new System.Drawing.Point(495, 28);
            this.cmdBrowse.Name = "cmdBrowse";
            this.cmdBrowse.Size = new System.Drawing.Size(31, 23);
            this.cmdBrowse.TabIndex = 3;
            this.cmdBrowse.Text = "...";
            this.cmdBrowse.UseVisualStyleBackColor = true;
            this.cmdBrowse.Click += new System.EventHandler(this.cmdBrowse_Click);
            // 
            // txtPath
            // 
            this.txtPath.Location = new System.Drawing.Point(116, 30);
            this.txtPath.Name = "txtPath";
            this.txtPath.Size = new System.Drawing.Size(366, 20);
            this.txtPath.TabIndex = 2;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Location = new System.Drawing.Point(2, 86);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(591, 582);
            this.tabControl1.TabIndex = 7;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox5);
            this.tabPage1.Controls.Add(this.groupBox3);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(583, 556);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Image with Entry";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox5
            // 
            this.groupBox5.Location = new System.Drawing.Point(-155, 3);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(149, 270);
            this.groupBox5.TabIndex = 48;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Total Images";
            // 
            // lstImage
            // 
            this.lstImage.Alignment = System.Windows.Forms.ListViewAlignment.Left;
            this.lstImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lstImage.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.lstImage.FullRowSelect = true;
            this.lstImage.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lstImage.HideSelection = false;
            this.lstImage.Location = new System.Drawing.Point(1335, 328);
            this.lstImage.Name = "lstImage";
            this.lstImage.Size = new System.Drawing.Size(175, 204);
            this.lstImage.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.lstImage.TabIndex = 5;
            this.lstImage.UseCompatibleStateImageBehavior = false;
            this.lstImage.View = System.Windows.Forms.View.Details;
            this.lstImage.SelectedIndexChanged += new System.EventHandler(this.lstImage_SelectedIndexChanged);
            this.lstImage.Leave += new System.EventHandler(this.lstImage_Leave);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Images";
            this.columnHeader1.Width = 190;
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.panel3);
            this.groupBox7.Location = new System.Drawing.Point(597, 6);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(723, 671);
            this.groupBox7.TabIndex = 9;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Image";
            // 
            // panel3
            // 
            this.panel3.AutoScroll = true;
            this.panel3.Controls.Add(this.deButton2);
            this.panel3.Controls.Add(this.deButton1);
            this.panel3.Controls.Add(this.picMain);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(3, 16);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(717, 652);
            this.panel3.TabIndex = 0;
            // 
            // picMain
            // 
            this.picMain.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picMain.Location = new System.Drawing.Point(5, 1);
            this.picMain.Name = "picMain";
            this.picMain.Size = new System.Drawing.Size(696, 638);
            this.picMain.TabIndex = 0;
            this.picMain.TabStop = false;
            this.picMain.MouseClick += new System.Windows.Forms.MouseEventHandler(this.picMain_MouseClick_2);
            this.picMain.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.picMain_PreviewKeyDown);
            // 
            // deButton2
            // 
            this.deButton2.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.deButton2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.deButton2.Font = new System.Drawing.Font("Segoe UI", 72F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.deButton2.Location = new System.Drawing.Point(0, 306);
            this.deButton2.Name = "deButton2";
            this.deButton2.Size = new System.Drawing.Size(40, 89);
            this.deButton2.TabIndex = 24;
            this.deButton2.Text = "<";
            this.deButton2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.deButton2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.deButton2.UseCompatibleTextRendering = true;
            this.deButton2.UseVisualStyleBackColor = true;
            this.deButton2.Click += new System.EventHandler(this.deButton2_Click);
            // 
            // deButton1
            // 
            this.deButton1.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.deButton1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.deButton1.Font = new System.Drawing.Font("Segoe UI", 72F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.deButton1.Location = new System.Drawing.Point(672, 307);
            this.deButton1.Name = "deButton1";
            this.deButton1.Size = new System.Drawing.Size(40, 89);
            this.deButton1.TabIndex = 23;
            this.deButton1.Text = ">";
            this.deButton1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.deButton1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.deButton1.UseCompatibleTextRendering = true;
            this.deButton1.UseVisualStyleBackColor = true;
            this.deButton1.Click += new System.EventHandler(this.deButton1_Click);
            // 
            // toolTip1
            // 
            this.toolTip1.AutoPopDelay = 5000;
            this.toolTip1.ForeColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.toolTip1.InitialDelay = 10;
            this.toolTip1.IsBalloon = true;
            this.toolTip1.ReshowDelay = 10;
            this.toolTip1.ShowAlways = true;
            this.toolTip1.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.toolTip1.ToolTipTitle = "Image Status";
            this.toolTip1.UseFading = false;
            // 
            // frmEntry
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1327, 684);
            this.Controls.Add(this.lstImage);
            this.Controls.Add(this.groupBox7);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.groupBox4);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmEntry";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Data Entry";
            this.Load += new System.EventHandler(this.frmEntry_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmEntry_KeyDown);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.frmEntry_KeyPress);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.groupBox7.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picMain)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label labelProject;
        private System.Windows.Forms.TextBox txtProject;
        private System.Windows.Forms.TextBox txtBatch;
        private System.Windows.Forms.Label labelBatch;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label labelLetter;
        private System.Windows.Forms.TextBox txtLetterNo;
        private System.Windows.Forms.TextBox txtIssuedFrom;
        private System.Windows.Forms.Label labelIssuedFrom;
        private System.Windows.Forms.TextBox txtIssuedTo;
        private System.Windows.Forms.Label labelIssuedTo;
        private System.Windows.Forms.Label labelSubCat;
        private System.Windows.Forms.ComboBox cmbSubCat;
        private System.Windows.Forms.Label labelSubject;
        private System.Windows.Forms.TextBox txtSubject;
        private System.Windows.Forms.Label labelDocType;
        private System.Windows.Forms.ComboBox cmbDocType;
        private System.Windows.Forms.Label labelIssuedDate;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Button buttonExit;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtYear;
        private System.Windows.Forms.TextBox txtDate;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.ComboBox cmbMonth;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.ComboBox cmbFile;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Button cmdBrowse;
        private System.Windows.Forms.TextBox txtPath;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.ListView lstImage;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.FolderBrowserDialog fbdPath;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.PictureBox picMain;
        private nControls.deButton deButton1;
        private nControls.deButton deButton2;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}