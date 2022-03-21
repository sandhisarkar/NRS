/*
 * Created by SharpDevelop.
 * User: SubhajitB
 * Date: 1/4/2008
 * Time: 4:51 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace ImageHeaven
{
	partial class aePolicyScan
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(aePolicyScan));
            this.panel1 = new System.Windows.Forms.Panel();
            this.grpAction = new System.Windows.Forms.GroupBox();
            this.cmdstopdeed = new System.Windows.Forms.Button();
            this.cmdDelete = new System.Windows.Forms.Button();
            this.cmdCancelScan = new System.Windows.Forms.Button();
            this.cmdScan = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.txtNotification = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.scanPic = new System.Windows.Forms.PictureBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.rdFlatBed = new System.Windows.Forms.RadioButton();
            this.rdADF = new System.Windows.Forms.RadioButton();
            this.label15 = new System.Windows.Forms.Label();
            this.txtTotalpages = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lblSize = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lstImageName = new System.Windows.Forms.ListBox();
            this.lblPicSize = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtsecondparty = new System.Windows.Forms.TextBox();
            this.Txtfirstparty = new System.Windows.Forms.TextBox();
            this.lbldeedyear = new System.Windows.Forms.Label();
            this.lbldeedno = new System.Windows.Forms.Label();
            this.lblsecondParty = new System.Windows.Forms.Label();
            this.lblfirstparty = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.cmdUpdate = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.lblNextPolicy = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.lblCurrentPolicy = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lblBox = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblBatch = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblProjectName = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.conHold = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.conMarkHold = new System.Windows.Forms.ToolStripMenuItem();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.grpAction.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scanPic)).BeginInit();
            this.groupBox5.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.conHold.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.panel1.Controls.Add(this.grpAction);
            this.panel1.Controls.Add(this.groupBox3);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.groupBox5);
            this.panel1.Controls.Add(this.lblPicSize);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(998, 766);
            this.panel1.TabIndex = 0;
            this.panel1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseClick);
            // 
            // grpAction
            // 
            this.grpAction.BackColor = System.Drawing.SystemColors.Control;
            this.grpAction.Controls.Add(this.cmdstopdeed);
            this.grpAction.Controls.Add(this.cmdDelete);
            this.grpAction.Controls.Add(this.cmdCancelScan);
            this.grpAction.Controls.Add(this.cmdScan);
            this.grpAction.Location = new System.Drawing.Point(6, 607);
            this.grpAction.Name = "grpAction";
            this.grpAction.Size = new System.Drawing.Size(255, 38);
            this.grpAction.TabIndex = 25;
            this.grpAction.TabStop = false;
            // 
            // cmdstopdeed
            // 
            this.cmdstopdeed.BackColor = System.Drawing.SystemColors.Control;
            this.cmdstopdeed.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdstopdeed.Location = new System.Drawing.Point(129, 8);
            this.cmdstopdeed.Name = "cmdstopdeed";
            this.cmdstopdeed.Size = new System.Drawing.Size(60, 24);
            this.cmdstopdeed.TabIndex = 4;
            this.cmdstopdeed.Text = "&Finish";
            this.cmdstopdeed.UseVisualStyleBackColor = false;
            this.cmdstopdeed.Click += new System.EventHandler(this.button2_Click);
            // 
            // cmdDelete
            // 
            this.cmdDelete.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdDelete.ForeColor = System.Drawing.Color.Red;
            this.cmdDelete.Location = new System.Drawing.Point(195, 8);
            this.cmdDelete.Name = "cmdDelete";
            this.cmdDelete.Size = new System.Drawing.Size(60, 24);
            this.cmdDelete.TabIndex = 5;
            this.cmdDelete.Text = "&Delete";
            this.cmdDelete.UseVisualStyleBackColor = true;
            this.cmdDelete.Click += new System.EventHandler(this.cmdDelete_Click);
            // 
            // cmdCancelScan
            // 
            this.cmdCancelScan.BackColor = System.Drawing.SystemColors.Control;
            this.cmdCancelScan.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdCancelScan.Location = new System.Drawing.Point(67, 8);
            this.cmdCancelScan.Name = "cmdCancelScan";
            this.cmdCancelScan.Size = new System.Drawing.Size(60, 24);
            this.cmdCancelScan.TabIndex = 3;
            this.cmdCancelScan.Text = "&Cancel";
            this.cmdCancelScan.UseVisualStyleBackColor = false;
            this.cmdCancelScan.Click += new System.EventHandler(this.cmdCancelScan_Click);
            // 
            // cmdScan
            // 
            this.cmdScan.BackColor = System.Drawing.SystemColors.Control;
            this.cmdScan.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdScan.Location = new System.Drawing.Point(2, 8);
            this.cmdScan.Name = "cmdScan";
            this.cmdScan.Size = new System.Drawing.Size(60, 24);
            this.cmdScan.TabIndex = 2;
            this.cmdScan.Text = "Star&t";
            this.cmdScan.UseVisualStyleBackColor = false;
            this.cmdScan.Click += new System.EventHandler(this.CmdScanClick);
            // 
            // groupBox3
            // 
            this.groupBox3.BackColor = System.Drawing.SystemColors.Control;
            this.groupBox3.Controls.Add(this.txtNotification);
            this.groupBox3.Location = new System.Drawing.Point(3, 648);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(255, 112);
            this.groupBox3.TabIndex = 24;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Notification";
            // 
            // txtNotification
            // 
            this.txtNotification.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtNotification.Location = new System.Drawing.Point(3, 16);
            this.txtNotification.Multiline = true;
            this.txtNotification.Name = "txtNotification";
            this.txtNotification.Size = new System.Drawing.Size(249, 93);
            this.txtNotification.TabIndex = 25;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.panel2.Controls.Add(this.scanPic);
            this.panel2.Location = new System.Drawing.Point(336, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(631, 760);
            this.panel2.TabIndex = 18;
            // 
            // scanPic
            // 
            this.scanPic.Dock = System.Windows.Forms.DockStyle.Right;
            this.scanPic.Location = new System.Drawing.Point(3, 0);
            this.scanPic.Name = "scanPic";
            this.scanPic.Size = new System.Drawing.Size(628, 760);
            this.scanPic.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.scanPic.TabIndex = 0;
            this.scanPic.TabStop = false;
            // 
            // groupBox5
            // 
            this.groupBox5.BackColor = System.Drawing.SystemColors.Control;
            this.groupBox5.Controls.Add(this.groupBox2);
            this.groupBox5.Controls.Add(this.label15);
            this.groupBox5.Controls.Add(this.txtTotalpages);
            this.groupBox5.Controls.Add(this.label14);
            this.groupBox5.Controls.Add(this.label8);
            this.groupBox5.Controls.Add(this.label7);
            this.groupBox5.Controls.Add(this.lblSize);
            this.groupBox5.Controls.Add(this.label2);
            this.groupBox5.Controls.Add(this.lstImageName);
            this.groupBox5.Location = new System.Drawing.Point(0, 299);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(258, 310);
            this.groupBox5.TabIndex = 17;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Image List";
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.groupBox2.Controls.Add(this.button1);
            this.groupBox2.Controls.Add(this.rdFlatBed);
            this.groupBox2.Controls.Add(this.rdADF);
            this.groupBox2.Location = new System.Drawing.Point(8, 279);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(147, 31);
            this.groupBox2.TabIndex = 22;
            this.groupBox2.TabStop = false;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.Lime;
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(139, 19);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(60, 30);
            this.button1.TabIndex = 2;
            this.button1.Text = "Apply";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // rdFlatBed
            // 
            this.rdFlatBed.AutoSize = true;
            this.rdFlatBed.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdFlatBed.Location = new System.Drawing.Point(73, 8);
            this.rdFlatBed.Name = "rdFlatBed";
            this.rdFlatBed.Size = new System.Drawing.Size(60, 17);
            this.rdFlatBed.TabIndex = 1;
            this.rdFlatBed.TabStop = true;
            this.rdFlatBed.Text = "&Single";
            this.rdFlatBed.UseVisualStyleBackColor = true;
            // 
            // rdADF
            // 
            this.rdADF.AutoSize = true;
            this.rdADF.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdADF.Location = new System.Drawing.Point(1, 8);
            this.rdADF.Name = "rdADF";
            this.rdADF.Size = new System.Drawing.Size(58, 17);
            this.rdADF.TabIndex = 0;
            this.rdADF.Text = "&Batch";
            this.rdADF.UseVisualStyleBackColor = true;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.Location = new System.Drawing.Point(4, 333);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(105, 13);
            this.label15.TabIndex = 33;
            this.label15.Text = "Remaining Pages";
            this.label15.Visible = false;
            // 
            // txtTotalpages
            // 
            this.txtTotalpages.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTotalpages.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTotalpages.Location = new System.Drawing.Point(115, 300);
            this.txtTotalpages.Name = "txtTotalpages";
            this.txtTotalpages.ReadOnly = true;
            this.txtTotalpages.Size = new System.Drawing.Size(35, 20);
            this.txtTotalpages.TabIndex = 32;
            this.txtTotalpages.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtTotalpages.Visible = false;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(9, 303);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(75, 13);
            this.label14.TabIndex = 31;
            this.label14.Text = "Total Pages";
            this.label14.Visible = false;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(85, 199);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(0, 16);
            this.label8.TabIndex = 2;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(91, 199);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(0, 16);
            this.label7.TabIndex = 2;
            // 
            // lblSize
            // 
            this.lblSize.AutoSize = true;
            this.lblSize.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSize.Location = new System.Drawing.Point(93, 328);
            this.lblSize.Name = "lblSize";
            this.lblSize.Size = new System.Drawing.Size(0, 16);
            this.lblSize.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(6, 300);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(78, 16);
            this.label2.TabIndex = 1;
            this.label2.Text = "Image Size:";
            this.label2.Visible = false;
            // 
            // lstImageName
            // 
            this.lstImageName.FormattingEnabled = true;
            this.lstImageName.Location = new System.Drawing.Point(6, 15);
            this.lstImageName.Name = "lstImageName";
            this.lstImageName.Size = new System.Drawing.Size(246, 264);
            this.lstImageName.TabIndex = 0;
            this.lstImageName.SelectedIndexChanged += new System.EventHandler(this.lstImageName_SelectedIndexChanged);
            this.lstImageName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lstImageName_KeyDown);
            this.lstImageName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.lstImageName_KeyPress);
            // 
            // lblPicSize
            // 
            this.lblPicSize.AutoSize = true;
            this.lblPicSize.Location = new System.Drawing.Point(74, 182);
            this.lblPicSize.Name = "lblPicSize";
            this.lblPicSize.Size = new System.Drawing.Size(0, 13);
            this.lblPicSize.TabIndex = 15;
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.SystemColors.Control;
            this.groupBox1.Controls.Add(this.textBox4);
            this.groupBox1.Controls.Add(this.label18);
            this.groupBox1.Controls.Add(this.textBox3);
            this.groupBox1.Controls.Add(this.label17);
            this.groupBox1.Controls.Add(this.textBox2);
            this.groupBox1.Controls.Add(this.label16);
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.txtsecondparty);
            this.groupBox1.Controls.Add(this.Txtfirstparty);
            this.groupBox1.Controls.Add(this.lbldeedyear);
            this.groupBox1.Controls.Add(this.lbldeedno);
            this.groupBox1.Controls.Add(this.lblsecondParty);
            this.groupBox1.Controls.Add(this.lblfirstparty);
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.cmdUpdate);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.lblNextPolicy);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.lblCurrentPolicy);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.lblBox);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.lblBatch);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.lblProjectName);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(0, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(258, 299);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Control Details";
            // 
            // txtsecondparty
            // 
            this.txtsecondparty.Location = new System.Drawing.Point(16, 176);
            this.txtsecondparty.Name = "txtsecondparty";
            this.txtsecondparty.ReadOnly = true;
            this.txtsecondparty.Size = new System.Drawing.Size(236, 20);
            this.txtsecondparty.TabIndex = 20;
            // 
            // Txtfirstparty
            // 
            this.Txtfirstparty.Location = new System.Drawing.Point(16, 132);
            this.Txtfirstparty.Name = "Txtfirstparty";
            this.Txtfirstparty.ReadOnly = true;
            this.Txtfirstparty.Size = new System.Drawing.Size(236, 20);
            this.Txtfirstparty.TabIndex = 19;
            // 
            // lbldeedyear
            // 
            this.lbldeedyear.AutoSize = true;
            this.lbldeedyear.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbldeedyear.Location = new System.Drawing.Point(93, 96);
            this.lbldeedyear.Name = "lbldeedyear";
            this.lbldeedyear.Size = new System.Drawing.Size(0, 16);
            this.lbldeedyear.TabIndex = 18;
            // 
            // lbldeedno
            // 
            this.lbldeedno.AutoSize = true;
            this.lbldeedno.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbldeedno.Location = new System.Drawing.Point(93, 80);
            this.lbldeedno.Name = "lbldeedno";
            this.lbldeedno.Size = new System.Drawing.Size(0, 16);
            this.lbldeedno.TabIndex = 17;
            // 
            // lblsecondParty
            // 
            this.lblsecondParty.AutoSize = true;
            this.lblsecondParty.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblsecondParty.Location = new System.Drawing.Point(15, 156);
            this.lblsecondParty.Name = "lblsecondParty";
            this.lblsecondParty.Size = new System.Drawing.Size(61, 16);
            this.lblsecondParty.TabIndex = 16;
            this.lblsecondParty.Text = "Gurdian :";
            // 
            // lblfirstparty
            // 
            this.lblfirstparty.AutoSize = true;
            this.lblfirstparty.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblfirstparty.Location = new System.Drawing.Point(12, 114);
            this.lblfirstparty.Name = "lblfirstparty";
            this.lblfirstparty.Size = new System.Drawing.Size(61, 16);
            this.lblfirstparty.TabIndex = 15;
            this.lblfirstparty.Text = "Reg No :";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(13, 93);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(59, 16);
            this.label13.TabIndex = 14;
            this.label13.Text = "Gender :";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(13, 69);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(51, 16);
            this.label12.TabIndex = 13;
            this.label12.Text = "Name :";
            // 
            // cmdUpdate
            // 
            this.cmdUpdate.Location = new System.Drawing.Point(157, 176);
            this.cmdUpdate.Name = "cmdUpdate";
            this.cmdUpdate.Size = new System.Drawing.Size(61, 23);
            this.cmdUpdate.TabIndex = 12;
            this.cmdUpdate.Text = "Update";
            this.cmdUpdate.UseVisualStyleBackColor = true;
            this.cmdUpdate.Visible = false;
            this.cmdUpdate.Click += new System.EventHandler(this.cmdUpdate_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(24, 177);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(106, 16);
            this.label9.TabIndex = 10;
            this.label9.Text = "File Page Count:";
            this.label9.Visible = false;
            // 
            // lblNextPolicy
            // 
            this.lblNextPolicy.AutoSize = true;
            this.lblNextPolicy.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNextPolicy.Location = new System.Drawing.Point(85, 45);
            this.lblNextPolicy.Name = "lblNextPolicy";
            this.lblNextPolicy.Size = new System.Drawing.Size(0, 15);
            this.lblNextPolicy.TabIndex = 9;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(13, 46);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(63, 16);
            this.label6.TabIndex = 8;
            this.label6.Text = "Next File:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(99, 105);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(0, 16);
            this.label10.TabIndex = 7;
            // 
            // lblCurrentPolicy
            // 
            this.lblCurrentPolicy.AutoSize = true;
            this.lblCurrentPolicy.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrentPolicy.Location = new System.Drawing.Point(100, 21);
            this.lblCurrentPolicy.Name = "lblCurrentPolicy";
            this.lblCurrentPolicy.Size = new System.Drawing.Size(0, 15);
            this.lblCurrentPolicy.TabIndex = 7;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(13, 23);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(78, 16);
            this.label5.TabIndex = 6;
            this.label5.Text = "Current File:";
            this.label5.Click += new System.EventHandler(this.label5_Click);
            // 
            // lblBox
            // 
            this.lblBox.AutoSize = true;
            this.lblBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBox.Location = new System.Drawing.Point(65, 96);
            this.lblBox.Name = "lblBox";
            this.lblBox.Size = new System.Drawing.Size(0, 16);
            this.lblBox.TabIndex = 5;
            this.lblBox.Visible = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(-42, 64);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(34, 16);
            this.label4.TabIndex = 4;
            this.label4.Text = "Box:";
            this.label4.Visible = false;
            // 
            // lblBatch
            // 
            this.lblBatch.AutoSize = true;
            this.lblBatch.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBatch.Location = new System.Drawing.Point(65, 32);
            this.lblBatch.Name = "lblBatch";
            this.lblBatch.Size = new System.Drawing.Size(0, 16);
            this.lblBatch.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(-39, 32);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 16);
            this.label3.TabIndex = 2;
            this.label3.Text = "Lot:";
            this.label3.Visible = false;
            // 
            // lblProjectName
            // 
            this.lblProjectName.AutoSize = true;
            this.lblProjectName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProjectName.Location = new System.Drawing.Point(72, 16);
            this.lblProjectName.Name = "lblProjectName";
            this.lblProjectName.Size = new System.Drawing.Size(0, 16);
            this.lblProjectName.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(-58, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Project:";
            this.label1.Visible = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.SystemColors.Control;
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(271, 766);
            this.pictureBox1.TabIndex = 6;
            this.pictureBox1.TabStop = false;
            // 
            // conHold
            // 
            this.conHold.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.conMarkHold});
            this.conHold.Name = "conHold";
            this.conHold.Size = new System.Drawing.Size(131, 26);
            // 
            // conMarkHold
            // 
            this.conMarkHold.Name = "conMarkHold";
            this.conMarkHold.Size = new System.Drawing.Size(130, 22);
            this.conMarkHold.Text = "Mark Hold";
            this.conMarkHold.Click += new System.EventHandler(this.conMarkHold_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(126, 221);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(126, 20);
            this.textBox1.TabIndex = 22;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(13, 225);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(106, 16);
            this.label11.TabIndex = 21;
            this.label11.Text = "Admission Date:";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(126, 245);
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(126, 20);
            this.textBox2.TabIndex = 24;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.Location = new System.Drawing.Point(41, 249);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(79, 16);
            this.label16.TabIndex = 23;
            this.label16.Text = "Death Date:";
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(126, 270);
            this.textBox3.Name = "textBox3";
            this.textBox3.ReadOnly = true;
            this.textBox3.Size = new System.Drawing.Size(126, 20);
            this.textBox3.TabIndex = 26;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.Location = new System.Drawing.Point(15, 274);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(105, 16);
            this.label17.TabIndex = 25;
            this.label17.Text = "Discharge Date:";
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(96, 199);
            this.textBox4.Name = "textBox4";
            this.textBox4.ReadOnly = true;
            this.textBox4.Size = new System.Drawing.Size(157, 20);
            this.textBox4.TabIndex = 28;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label18.Location = new System.Drawing.Point(13, 201);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(81, 16);
            this.label18.TabIndex = 27;
            this.label18.Text = "Department:";
            // 
            // aePolicyScan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(998, 766);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "aePolicyScan";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "aePolicyScan";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AePolicyScanFormClosing);
            this.Load += new System.EventHandler(this.AePolicyScanLoad);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.grpAction.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scanPic)).EndInit();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.conHold.ResumeLayout(false);
            this.ResumeLayout(false);

		}
        private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblSize;
		private System.Windows.Forms.Label lblNextPolicy;
		private System.Windows.Forms.ListBox lstImageName;
		private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Label lblPicSize;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label lblProjectName;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label lblBatch;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label lblBox;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label lblCurrentPolicy;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.PictureBox scanPic;
        private System.Windows.Forms.ContextMenuStrip conHold;
        private System.Windows.Forms.ToolStripMenuItem conMarkHold;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.RadioButton rdFlatBed;
        private System.Windows.Forms.RadioButton rdADF;
        private System.Windows.Forms.Button cmdUpdate;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label lbldeedyear;
        private System.Windows.Forms.Label lbldeedno;
        private System.Windows.Forms.Label lblsecondParty;
        private System.Windows.Forms.Label lblfirstparty;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtsecondparty;
        private System.Windows.Forms.TextBox Txtfirstparty;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox txtTotalpages;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox txtNotification;
        private System.Windows.Forms.GroupBox grpAction;
        private System.Windows.Forms.Button cmdstopdeed;
        private System.Windows.Forms.Button cmdDelete;
        private System.Windows.Forms.Button cmdCancelScan;
        private System.Windows.Forms.Button cmdScan;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.Label label18;
        //private OnlyNumbers.NumberTextBox lblPageCount;
	}
}
