namespace ImageHeaven
{
    partial class frmSearch
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSearch));
            this.panel1 = new System.Windows.Forms.Panel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.Selection = new System.Windows.Forms.TabPage();
            this.panel3 = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.grdBox = new System.Windows.Forms.DataGridView();
            this.panel2 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dateTimePicker6 = new System.Windows.Forms.DateTimePicker();
            this.deLabel8 = new nControls.deLabel();
            this.dateTimePicker5 = new System.Windows.Forms.DateTimePicker();
            this.deLabel9 = new nControls.deLabel();
            this.dateTimePicker4 = new System.Windows.Forms.DateTimePicker();
            this.deLabel6 = new nControls.deLabel();
            this.dateTimePicker3 = new System.Windows.Forms.DateTimePicker();
            this.deLabel7 = new nControls.deLabel();
            this.dateTimePicker2 = new System.Windows.Forms.DateTimePicker();
            this.deLabel4 = new nControls.deLabel();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.deLabel5 = new nControls.deLabel();
            this.deTextBox3 = new nControls.deTextBox();
            this.deLabel3 = new nControls.deLabel();
            this.deTextBox2 = new nControls.deTextBox();
            this.deLabel2 = new nControls.deLabel();
            this.deTextBox1 = new nControls.deTextBox();
            this.deLabel1 = new nControls.deLabel();
            this.cmdSearch = new nControls.deButton();
            this.panel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.Selection.SuspendLayout();
            this.panel3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdBox)).BeginInit();
            this.panel2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tabControl1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1614, 891);
            this.panel1.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.Selection);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1614, 891);
            this.tabControl1.TabIndex = 1;
            // 
            // Selection
            // 
            this.Selection.Controls.Add(this.panel3);
            this.Selection.Controls.Add(this.panel2);
            this.Selection.Location = new System.Drawing.Point(4, 22);
            this.Selection.Name = "Selection";
            this.Selection.Padding = new System.Windows.Forms.Padding(3);
            this.Selection.Size = new System.Drawing.Size(1606, 865);
            this.Selection.TabIndex = 0;
            this.Selection.Text = "Selection";
            this.Selection.UseVisualStyleBackColor = true;
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.groupBox2);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(3, 165);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1600, 697);
            this.panel3.TabIndex = 1;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.grdBox);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1598, 695);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Search Result. . . ";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(619, 174);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(247, 32);
            this.label1.TabIndex = 4;
            this.label1.Text = "No Result Found . . .";
            this.label1.Visible = false;
            // 
            // grdBox
            // 
            this.grdBox.AllowUserToAddRows = false;
            this.grdBox.AllowUserToDeleteRows = false;
            this.grdBox.AllowUserToResizeRows = false;
            this.grdBox.BackgroundColor = System.Drawing.Color.White;
            this.grdBox.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdBox.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.grdBox.GridColor = System.Drawing.SystemColors.ControlText;
            this.grdBox.Location = new System.Drawing.Point(3, 23);
            this.grdBox.MultiSelect = false;
            this.grdBox.Name = "grdBox";
            this.grdBox.ReadOnly = true;
            this.grdBox.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.grdBox.RowHeadersVisible = false;
            dataGridViewCellStyle12.NullValue = null;
            dataGridViewCellStyle12.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grdBox.RowsDefaultCellStyle = dataGridViewCellStyle12;
            this.grdBox.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdBox.Size = new System.Drawing.Size(1592, 669);
            this.grdBox.TabIndex = 80;
            this.grdBox.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdBox_CellDoubleClick);
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.groupBox1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(3, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1600, 162);
            this.panel2.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cmdSearch);
            this.groupBox1.Controls.Add(this.dateTimePicker6);
            this.groupBox1.Controls.Add(this.deLabel8);
            this.groupBox1.Controls.Add(this.dateTimePicker5);
            this.groupBox1.Controls.Add(this.deLabel9);
            this.groupBox1.Controls.Add(this.dateTimePicker4);
            this.groupBox1.Controls.Add(this.deLabel6);
            this.groupBox1.Controls.Add(this.dateTimePicker3);
            this.groupBox1.Controls.Add(this.deLabel7);
            this.groupBox1.Controls.Add(this.dateTimePicker2);
            this.groupBox1.Controls.Add(this.deLabel4);
            this.groupBox1.Controls.Add(this.dateTimePicker1);
            this.groupBox1.Controls.Add(this.deLabel5);
            this.groupBox1.Controls.Add(this.deTextBox3);
            this.groupBox1.Controls.Add(this.deLabel3);
            this.groupBox1.Controls.Add(this.deTextBox2);
            this.groupBox1.Controls.Add(this.deLabel2);
            this.groupBox1.Controls.Add(this.deTextBox1);
            this.groupBox1.Controls.Add(this.deLabel1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1598, 160);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Search Criteria . . .";
            // 
            // dateTimePicker6
            // 
            this.dateTimePicker6.CustomFormat = "   ";
            this.dateTimePicker6.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker6.Location = new System.Drawing.Point(1160, 108);
            this.dateTimePicker6.Name = "dateTimePicker6";
            this.dateTimePicker6.Size = new System.Drawing.Size(299, 27);
            this.dateTimePicker6.TabIndex = 20;
            this.dateTimePicker6.Value = new System.DateTime(2020, 9, 7, 0, 0, 0, 0);
            this.dateTimePicker6.ValueChanged += new System.EventHandler(this.dateTimePicker6_ValueChanged);
            this.dateTimePicker6.KeyUp += new System.Windows.Forms.KeyEventHandler(this.dateTimePicker6_KeyUp);
            // 
            // deLabel8
            // 
            this.deLabel8.AutoSize = true;
            this.deLabel8.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.deLabel8.Location = new System.Drawing.Point(1044, 120);
            this.deLabel8.Name = "deLabel8";
            this.deLabel8.Size = new System.Drawing.Size(94, 15);
            this.deLabel8.TabIndex = 19;
            this.deLabel8.Text = "Death End Date :";
            // 
            // dateTimePicker5
            // 
            this.dateTimePicker5.CustomFormat = "   ";
            this.dateTimePicker5.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker5.Location = new System.Drawing.Point(1160, 69);
            this.dateTimePicker5.Name = "dateTimePicker5";
            this.dateTimePicker5.Size = new System.Drawing.Size(299, 27);
            this.dateTimePicker5.TabIndex = 18;
            this.dateTimePicker5.Value = new System.DateTime(2020, 9, 7, 0, 0, 0, 0);
            this.dateTimePicker5.ValueChanged += new System.EventHandler(this.dateTimePicker5_ValueChanged);
            this.dateTimePicker5.KeyUp += new System.Windows.Forms.KeyEventHandler(this.dateTimePicker5_KeyUp);
            // 
            // deLabel9
            // 
            this.deLabel9.AutoSize = true;
            this.deLabel9.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.deLabel9.Location = new System.Drawing.Point(1040, 78);
            this.deLabel9.Name = "deLabel9";
            this.deLabel9.Size = new System.Drawing.Size(98, 15);
            this.deLabel9.TabIndex = 17;
            this.deLabel9.Text = "Death Start Date :";
            // 
            // dateTimePicker4
            // 
            this.dateTimePicker4.CustomFormat = "   ";
            this.dateTimePicker4.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker4.Location = new System.Drawing.Point(662, 112);
            this.dateTimePicker4.Name = "dateTimePicker4";
            this.dateTimePicker4.Size = new System.Drawing.Size(300, 27);
            this.dateTimePicker4.TabIndex = 16;
            this.dateTimePicker4.Value = new System.DateTime(2020, 9, 7, 0, 0, 0, 0);
            this.dateTimePicker4.ValueChanged += new System.EventHandler(this.dateTimePicker4_ValueChanged);
            this.dateTimePicker4.KeyUp += new System.Windows.Forms.KeyEventHandler(this.dateTimePicker4_KeyUp);
            // 
            // deLabel6
            // 
            this.deLabel6.AutoSize = true;
            this.deLabel6.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.deLabel6.Location = new System.Drawing.Point(523, 121);
            this.deLabel6.Name = "deLabel6";
            this.deLabel6.Size = new System.Drawing.Size(115, 15);
            this.deLabel6.TabIndex = 15;
            this.deLabel6.Text = "Discharge End Date :";
            // 
            // dateTimePicker3
            // 
            this.dateTimePicker3.CustomFormat = "   ";
            this.dateTimePicker3.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker3.Location = new System.Drawing.Point(662, 70);
            this.dateTimePicker3.Name = "dateTimePicker3";
            this.dateTimePicker3.Size = new System.Drawing.Size(300, 27);
            this.dateTimePicker3.TabIndex = 14;
            this.dateTimePicker3.Value = new System.DateTime(2020, 9, 7, 0, 0, 0, 0);
            this.dateTimePicker3.ValueChanged += new System.EventHandler(this.dateTimePicker3_ValueChanged);
            this.dateTimePicker3.KeyUp += new System.Windows.Forms.KeyEventHandler(this.dateTimePicker3_KeyUp);
            // 
            // deLabel7
            // 
            this.deLabel7.AutoSize = true;
            this.deLabel7.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.deLabel7.Location = new System.Drawing.Point(519, 78);
            this.deLabel7.Name = "deLabel7";
            this.deLabel7.Size = new System.Drawing.Size(119, 15);
            this.deLabel7.TabIndex = 13;
            this.deLabel7.Text = "Discharge Start Date :";
            // 
            // dateTimePicker2
            // 
            this.dateTimePicker2.CustomFormat = "   ";
            this.dateTimePicker2.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker2.Location = new System.Drawing.Point(154, 112);
            this.dateTimePicker2.Name = "dateTimePicker2";
            this.dateTimePicker2.Size = new System.Drawing.Size(302, 27);
            this.dateTimePicker2.TabIndex = 12;
            this.dateTimePicker2.Value = new System.DateTime(2020, 9, 7, 0, 0, 0, 0);
            this.dateTimePicker2.ValueChanged += new System.EventHandler(this.dateTimePicker2_ValueChanged);
            this.dateTimePicker2.KeyUp += new System.Windows.Forms.KeyEventHandler(this.dateTimePicker2_KeyUp);
            // 
            // deLabel4
            // 
            this.deLabel4.AutoSize = true;
            this.deLabel4.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.deLabel4.Location = new System.Drawing.Point(14, 121);
            this.deLabel4.Name = "deLabel4";
            this.deLabel4.Size = new System.Drawing.Size(119, 15);
            this.deLabel4.TabIndex = 11;
            this.deLabel4.Text = "Admission End Date :";
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.CustomFormat = "   ";
            this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker1.Location = new System.Drawing.Point(154, 70);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(302, 27);
            this.dateTimePicker1.TabIndex = 10;
            this.dateTimePicker1.Value = new System.DateTime(2020, 9, 7, 0, 0, 0, 0);
            this.dateTimePicker1.ValueChanged += new System.EventHandler(this.dateTimePicker1_ValueChanged);
            this.dateTimePicker1.KeyUp += new System.Windows.Forms.KeyEventHandler(this.dateTimePicker1_KeyUp);
            // 
            // deLabel5
            // 
            this.deLabel5.AutoSize = true;
            this.deLabel5.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.deLabel5.Location = new System.Drawing.Point(10, 78);
            this.deLabel5.Name = "deLabel5";
            this.deLabel5.Size = new System.Drawing.Size(123, 15);
            this.deLabel5.TabIndex = 9;
            this.deLabel5.Text = "Admission Start Date :";
            // 
            // deTextBox3
            // 
            this.deTextBox3.BackColor = System.Drawing.Color.White;
            this.deTextBox3.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.deTextBox3.ForeColor = System.Drawing.Color.Black;
            this.deTextBox3.Location = new System.Drawing.Point(1160, 30);
            this.deTextBox3.Mandatory = true;
            this.deTextBox3.Name = "deTextBox3";
            this.deTextBox3.Size = new System.Drawing.Size(364, 23);
            this.deTextBox3.TabIndex = 5;
            // 
            // deLabel3
            // 
            this.deLabel3.AutoSize = true;
            this.deLabel3.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.deLabel3.Location = new System.Drawing.Point(1083, 34);
            this.deLabel3.Name = "deLabel3";
            this.deLabel3.Size = new System.Drawing.Size(55, 15);
            this.deLabel3.TabIndex = 4;
            this.deLabel3.Text = "Address :";
            // 
            // deTextBox2
            // 
            this.deTextBox2.BackColor = System.Drawing.Color.White;
            this.deTextBox2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.deTextBox2.ForeColor = System.Drawing.Color.Black;
            this.deTextBox2.Location = new System.Drawing.Point(662, 30);
            this.deTextBox2.Mandatory = true;
            this.deTextBox2.Name = "deTextBox2";
            this.deTextBox2.Size = new System.Drawing.Size(300, 23);
            this.deTextBox2.TabIndex = 3;
            // 
            // deLabel2
            // 
            this.deLabel2.AutoSize = true;
            this.deLabel2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.deLabel2.Location = new System.Drawing.Point(542, 34);
            this.deLabel2.Name = "deLabel2";
            this.deLabel2.Size = new System.Drawing.Size(96, 15);
            this.deLabel2.TabIndex = 2;
            this.deLabel2.Text = "Guardian Name :";
            // 
            // deTextBox1
            // 
            this.deTextBox1.BackColor = System.Drawing.Color.White;
            this.deTextBox1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.deTextBox1.ForeColor = System.Drawing.Color.Black;
            this.deTextBox1.Location = new System.Drawing.Point(154, 31);
            this.deTextBox1.Mandatory = true;
            this.deTextBox1.Name = "deTextBox1";
            this.deTextBox1.Size = new System.Drawing.Size(302, 23);
            this.deTextBox1.TabIndex = 1;
            // 
            // deLabel1
            // 
            this.deLabel1.AutoSize = true;
            this.deLabel1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.deLabel1.Location = new System.Drawing.Point(48, 34);
            this.deLabel1.Name = "deLabel1";
            this.deLabel1.Size = new System.Drawing.Size(85, 15);
            this.deLabel1.TabIndex = 0;
            this.deLabel1.Text = "Patient Name :";
            // 
            // cmdSearch
            // 
            this.cmdSearch.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("cmdSearch.BackgroundImage")));
            this.cmdSearch.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.cmdSearch.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.cmdSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmdSearch.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdSearch.Location = new System.Drawing.Point(1486, 102);
            this.cmdSearch.Name = "cmdSearch";
            this.cmdSearch.Size = new System.Drawing.Size(94, 33);
            this.cmdSearch.TabIndex = 21;
            this.cmdSearch.Text = "&Search";
            this.cmdSearch.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cmdSearch.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.cmdSearch.UseCompatibleTextRendering = true;
            this.cmdSearch.UseVisualStyleBackColor = true;
            this.cmdSearch.Click += new System.EventHandler(this.cmdSearch_Click);
            // 
            // frmSearch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1614, 891);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "frmSearch";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Search for NRS Record Management";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmSearch_Load);
            this.panel1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.Selection.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdBox)).EndInit();
            this.panel2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.GroupBox groupBox1;
        private nControls.deLabel deLabel1;
        private nControls.deTextBox deTextBox1;
        private nControls.deTextBox deTextBox2;
        private nControls.deLabel deLabel2;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage Selection;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridView grdBox;
        private System.Windows.Forms.Label label1;
        private nControls.deTextBox deTextBox3;
        private nControls.deLabel deLabel3;
        private System.Windows.Forms.DateTimePicker dateTimePicker2;
        private nControls.deLabel deLabel4;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private nControls.deLabel deLabel5;
        private System.Windows.Forms.DateTimePicker dateTimePicker4;
        private nControls.deLabel deLabel6;
        private System.Windows.Forms.DateTimePicker dateTimePicker3;
        private nControls.deLabel deLabel7;
        private System.Windows.Forms.DateTimePicker dateTimePicker6;
        private nControls.deLabel deLabel8;
        private System.Windows.Forms.DateTimePicker dateTimePicker5;
        private nControls.deLabel deLabel9;
        private nControls.deButton cmdSearch;
    }
}