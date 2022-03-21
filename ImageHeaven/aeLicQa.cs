using System;
using System.Drawing;
using System.Windows.Forms;
using System.Data;
using System.Data.Odbc;
using NovaNet.Utils;
using NovaNet.wfe;
using LItems;
using System.Collections;
using System.Drawing.Imaging;
using System.IO;
using DataLayerDefs;
using System.Drawing.Drawing2D;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Linq;

namespace ImageHeaven
{
    public partial class aeLicQa : Form
    {
        MemoryStream stateLog;
        byte[] tmpWrite;
        OdbcConnection sqlCon = null;
        NovaNet.Utils.dbCon dbcon = null;
        CtrlPolicy pPolicy = null;
        private CtrlImage pImage = null;
        wfePolicy wPolicy = null;
        wfeImage wImage = null;
        private string boxNo = null;
        private string policyNumber = null;
        private string projCode = null;
        private string batchCode = null;
        private string picPath = null;
        private udtPolicy policyData = null;
        string policyPath = null;
        private int policyStatus = 0;
        private int clickedIndexValue;
        private CtrlBox pBox = null;
        private int selBoxNo;
        string[] imageName;
        int policyRowIndex;
        int total_Count = 0;
        DataSet dsimage11 = new DataSet();
        //private CtrlBatch pBatch = null;
        OdbcDataAdapter sqlAdap;
        //private MagickNet.Image imgQc;
        string imagePath = null;
        string photoPath = null;
        //private CtrlBox pBox=null;
        private Imagery img;
        private Imagery imgAll;
        private Credentials crd = new Credentials();
        public static NovaNet.Utils.exLog.Logger exMailLog = new NovaNet.Utils.exLog.emailLogger("./errLog.log", NovaNet.Utils.exLog.LogLevel.Dev, Constants._MAIL_TO, Constants._MAIL_FROM, Constants._SMTP);
        public static NovaNet.Utils.exLog.Logger exTxtLog = new NovaNet.Utils.exLog.txtLogger("./errLog.log", NovaNet.Utils.exLog.LogLevel.Dev);
        private string imgFileName = string.Empty;
        private int zoomWidth;
        private int zoomHeight;
        private Size zoomSize = new Size();
        private int keyPressed = 1;
        private DataTable gTable;
        ihwQuery wQ;
        private string selDocType = string.Empty;
        private int currntPg = 0;
        private bool firstDoc = true;
        private string prevDoc;
        private int policyLen = 0;


        public static string projKey;
        public static string bundleKey;

        public aeLicQa()
        {
            InitializeComponent();
        }

        public aeLicQa(OdbcConnection prmCon, Credentials prmCrd)
        {
            //
            // The InitializeComponent() call is required for Windows Forms designer support.
            //
            this.Name = "NRS Quality Control";
            InitializeComponent();
            sqlCon = prmCon;
            img = IgrFactory.GetImagery(Constants.IGR_CLEARIMAGE);
            //img = IgrFactory.GetImagery(Constants.IGR_GDPICTURE);
            imgAll = IgrFactory.GetImagery(Constants.IGR_CLEARIMAGE);
            crd = prmCrd;
            exMailLog.SetNextLogger(exTxtLog);
            dgvProperty.BringToFront();

            //img = IgrFactory.GetImagery(Constants.IGR_GDPICTURE);			
            //
            // TODO: Add constructor code after the InitializeComponent() call.
            //
        }

        private void splitContainer4_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void PopulateProjectCombo()
        {
            DataSet ds = new DataSet();

            dbcon = new NovaNet.Utils.dbCon();

            wfeProject tmpProj = new wfeProject(sqlCon);
            //cmbProject.Items.Add("Select");
            ds = tmpProj.GetAllValues();
            if (ds.Tables[0].Rows.Count > 0)
            {
                cmbProject.DataSource = ds.Tables[0];
                cmbProject.DisplayMember = ds.Tables[0].Columns[1].ToString();
                cmbProject.ValueMember = ds.Tables[0].Columns[0].ToString();
            }
        }

        private void aeLicQa_Load(object sender, EventArgs e)
        {
            System.Diagnostics.Process.GetCurrentProcess().PriorityClass = System.Diagnostics.ProcessPriorityClass.RealTime;
            System.Windows.Forms.ToolTip bttnToolTip = new System.Windows.Forms.ToolTip();
            System.Windows.Forms.ToolTip otherToolTip = new System.Windows.Forms.ToolTip();
            this.WindowState = FormWindowState.Maximized;
            //PopulateProjectCombo();
            rdoShowAll.Checked = true;
            cmdZoomIn.ForeColor = Color.Black;
            cmdZoomOut.ForeColor = Color.Black;
            chkRejectBatch.Visible = false;
            bttnToolTip.SetToolTip(cmdZoomIn, "Shortcut Key- (+)");
            bttnToolTip.SetToolTip(cmdZoomOut, "Shortcut Key- (-)");
            label6.ForeColor = Color.Black;
            label7.ForeColor = Color.Black;
            label8.ForeColor = Color.Black;
            label9.ForeColor = Color.Black;
            txtPolicyNumber.ForeColor = Color.DarkRed;
            txtName.ForeColor = Color.DarkRed;
            //populateDistrict();
            populateRunNum();
        }

        private void populateRunNum()
        {
            DataSet ds = new DataSet();
            wfeBox dly = new wfeBox(sqlCon);
            //ds = dly.GetRunnum();
            string sql = "select bundle_code, status from bundle_master where status = '6' or status = '7' ";
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(ds);
            if (ds.Tables[0].Rows.Count > 0)
            {
                cmbRunnum.DataSource = ds.Tables[0];
                cmbRunnum.DisplayMember = "bundle_code";
                cmbRunnum.ValueMember = "bundle_code";
            }

        }

        public DataTable GetAllMetadata(string batchCode)
        {
            string sqlStr = null;
            DataSet dsImage = new DataSet();
            OdbcDataAdapter sqlAdap = null;
            string indexPageName = string.Empty;

            //sqlStr = "select a.district_name,b.ro_NAME,c.book,c.deed_year,c.deed_no,c.serial_no,c.serial_year,d.tran_maj_name,e.tran_name,c.volume_no,c.page_from,c.page_to,c.date_of_completion,c.date_of_delivery,c.deed_remarks from district a,ro_master b, deed_details c,party d,tranlist_code e where c.district_code = a.district_code and c.ro_code = b.ro_code and d.tran_maj_code = c.tran_maj_code and e.tran_min_code = c.tran_min_code and c.district_code = '" + Do_code + "' and c.Ro_code = '" + RO_Code + "' and c.book = '" + year + "' and c.deed_year = '" + deed_year + "'";
            //sqlStr = "select * from deed_details where district_code = '" + Do_code + "' and Ro_code = '" + RO_Code + "' and book = '" + year + "' and deed_year = '" + deed_year + "' and volume_no = '"+vol+"'";
            sqlStr = "select distinct proj_code,bundle_key,item_no,patient_id,patient_name,gender,reg_number,guardian_name,address,dep_name,date_format(admission_date,'%d-%M-%y'),death,date_format(death_date,'%d-%M-%y'),date_format(discharge_date,'%d-%M-%y'),created_by,created_dttm,modified_by,modified_dttm from metadata_entry where proj_code = '" + projKey + "' and bundle_key = '" + bundleKey + "' order by item_no";

            try
            {
                sqlAdap = new OdbcDataAdapter(sqlStr, sqlCon);
                sqlAdap.Fill(dsImage);
            }
            catch (Exception ex)
            {
                sqlAdap.Dispose();
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes(sqlStr + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex);
            }
            //DataRow dr = dsImage.Tables[0].Rows[0];
            //dsImage.Dispose();
            return dsImage.Tables[0];
        }

        public DataTable GetAllMetadataParcent(string batchCode, string limit)
        {
            string sqlStr = null;
            DataSet dsImage = new DataSet();
            OdbcDataAdapter sqlAdap = null;
            string indexPageName = string.Empty;

            //sqlStr = "select a.district_name,b.ro_NAME,c.book,c.deed_year,c.deed_no,c.serial_no,c.serial_year,d.tran_maj_name,e.tran_name,c.volume_no,c.page_from,c.page_to,c.date_of_completion,c.date_of_delivery,c.deed_remarks from district a,ro_master b, deed_details c,party d,tranlist_code e where c.district_code = a.district_code and c.ro_code = b.ro_code and d.tran_maj_code = c.tran_maj_code and e.tran_min_code = c.tran_min_code and c.district_code = '" + Do_code + "' and c.Ro_code = '" + RO_Code + "' and c.book = '" + year + "' and c.deed_year = '" + deed_year + "'";
            //sqlStr = "select * from deed_details where district_code = '" + Do_code + "' and Ro_code = '" + RO_Code + "' and book = '" + year + "' and deed_year = '" + deed_year + "' and volume_no = '"+vol+"'";
            sqlStr = "select distinct proj_code,bundle_key,item_no as 'Sl No',patient_id as 'Patient Id',patient_name as 'Patient Name',gender as 'Gender',reg_number as 'Registration Number',guardian_name as 'Guardian Name',address as 'Address',dep_name as 'Department',date_format(admission_date,'%d-%M-%y') as 'Admission Date',death as 'Death Status',date_format(death_date,'%d-%M-%y') as 'Death Date',date_format(discharge_date,'%d-%M-%y') as 'Discharge Date',created_by as 'Created By',created_dttm as 'Created DTTM',modified_by as 'Modified By',modified_dttm as 'Modified DTTM' from metadata_entry where proj_code = '" + projKey + "' and bundle_key = '" + bundleKey + "' order by RAND() limit " + limit + "";

            try
            {
                sqlAdap = new OdbcDataAdapter(sqlStr, sqlCon);
                sqlAdap.Fill(dsImage);
            }
            catch (Exception ex)
            {
                sqlAdap.Dispose();
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes(sqlStr + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex);
            }
            //DataRow dr = dsImage.Tables[0].Rows[0];
            //dsImage.Dispose();
            return dsImage.Tables[0];
        }

        public DataSet GetRunnumStatus(string runNum)
        {
            string sql = "select distinct status from bundle_master where bundle_code = '" + runNum + "' ";
            DataSet ds = new DataSet();
            OdbcDataAdapter odap = new OdbcDataAdapter(sql, sqlCon);
            odap.Fill(ds);
            return ds;
        }

        public string deedCheck(string deedNo)
        {
            string sql = null;
            string deedStatus = null;
            OdbcCommand sqlCmd = new OdbcCommand();
            OdbcDataAdapter sqlAdap;
            DataSet ds = new DataSet();
            try
            {
                sqlCmd.Connection = sqlCon;
                sql = "select status from metadata_entry where patient_id ='" + deedNo + "'";
                sqlCmd.CommandText = sql;
                sqlAdap = new OdbcDataAdapter(sqlCmd);
                sqlAdap.Fill(ds);

            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
            if (ds.Tables[0].Rows.Count > 0)
            {
                deedStatus = ds.Tables[0].Rows[0]["status"].ToString();
                return deedStatus;
            }
            else
            { return string.Empty; }

        }

        private void cmdRefine_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            string sql = "select proj_code, bundle_key from bundle_master where bundle_code = '" + cmbRunnum.SelectedValue.ToString() + "' ";
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(dt);

            if(dt.Rows.Count > 0)
            {
                projKey = dt.Rows[0][0].ToString();
                bundleKey = dt.Rows[0][1].ToString();
            }

            wfeImage img = new wfeImage(sqlCon);
            if (GetAllMetadata(cmbRunnum.SelectedValue.ToString()).Rows.Count > 0)
            {
                total_Count = GetAllMetadata(cmbRunnum.SelectedValue.ToString()).Rows.Count;

            }
            try
            {
                if (Convert.ToInt32(txtPercent.Text.Trim()) <= 100)
                {
                    int percent = (total_Count * Convert.ToInt32(txtPercent.Text.Trim()) / 100);
                    dt = GetAllMetadataParcent(cmbRunnum.SelectedValue.ToString(), percent.ToString());
                    lblperCount.Text = "Showing " + dt.Rows.Count.ToString() + " out of " + total_Count + " Records";
                    wfeBox wfeb = new wfeBox(sqlCon);
                    if (GetRunnumStatus(cmbRunnum.SelectedValue.ToString()).Tables[0].Rows[0][0].ToString() == "7")
                    {
                        checkBox1.Checked = true;
                        checkBox1.Enabled = false;
                        cmdAccepted.Enabled = true;
                        cmdRejected.Enabled = true;
                    }
                    if (GetRunnumStatus(cmbRunnum.SelectedValue.ToString()).Tables[0].Rows[0][0].ToString() == "6")
                    {
                        checkBox1.Checked = false;
                        checkBox1.Enabled = true;
                        cmdAccepted.Enabled = true;
                        cmdRejected.Enabled = true;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        grdBox.DataSource = null;
                        dgvname.DataSource = null;
                        dgvProperty.DataSource = null;
                        grdBox.DataSource = dt;
                        grdBox.Columns[0].Visible = false;
                        grdBox.Columns[1].Visible = false;
                        /*grdBox.Columns[5].Visible = false;
                        grdBox.Columns[6].Visible = false;
                        grdBox.Columns[7].Visible = false;
                        grdBox.Columns[8].Visible = false;

                        grdBox.Columns[10].Width = 250;*/
                        //grdBox.Columns[6].Width = 250;
                        for (int i = 0; i < grdBox.Rows.Count; i++)
                        {
                            //string deedno = grdBox.Rows[i].Cells[0].Value.ToString() + grdBox.Rows[i].Cells[1].Value.ToString() + grdBox.Rows[i].Cells[2].Value.ToString() + grdBox.Rows[i].Cells[3].Value.ToString() + "[" + grdBox.Rows[i].Cells[4].Value.ToString() + "]";
                            string deedno = grdBox.Rows[i].Cells[3].Value.ToString();
                            wfePolicy wfepol = new wfePolicy(sqlCon);
                            if (deedCheck(deedno) == "30")
                            {
                                grdBox.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                            }
                            else if (deedCheck(deedno) == "31")
                            {
                                grdBox.Rows[i].DefaultCellStyle.BackColor = Color.Green;
                            }
                            /*else if (grdBox.Rows[i].Cells[19].Value.ToString() == "Y")
                            {
                                grdBox.Rows[i].DefaultCellStyle.BackColor = Color.Yellow;
                            }*/
                            /*else if (grdBox.Rows[i].Cells[16].Value.ToString() != "")
                            {
                                grdBox.Rows[i].DefaultCellStyle.BackColor = Color.Pink;
                            }*/
                            /* else if (grdBox.Rows[i].Cells[20].Value.ToString() != "")
                             {
                                 grdBox.Rows[i].DefaultCellStyle.BackColor = Color.Yellow;
                             }
                             else if (grdBox.Rows[i].Cells[16].Value.ToString() != "" && grdBox.Rows[i].Cells[20].Value.ToString() != "")
                             {
                                 grdBox.Rows[i].DefaultCellStyle.BackColor = Color.Orange;
                             }*/
                            else
                            {
                                grdBox.Rows[i].DefaultCellStyle.BackColor = Color.White;
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("No Record Found .....");
                    }
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void grdBox_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        public DataTable GetAllNameEX(string projK, string bundleK, string patientID)
        {
            string sqlStr = null;
            DataSet dsImage = new DataSet();
            OdbcDataAdapter sqlAdap = null;
            string indexPageName = string.Empty;

            //sqlStr = "select * from index_of_name where district_code = '" + Do_code + "' and Ro_code = '" + RO_Code + "' and book = '" + year + "' and deed_year = '" + deed_year + "' and deed_no = '" + deed_no + "'";
            //sqlStr = "select a.District_Code,a.RO_Code,a.Book,a.Deed_year,a.Deed_no,b.district_name,c.ro_name,a.Item_no,a.initial_name,a.First_name,a.Last_name,D.EC_NAME,a.Admit_code,a.Address,a.Address_district_code,a.Address_district_name,a.Address_ps_code,a.Address_ps_name,a.Father_mother,a.Rel_code,a.Relation,a.occupation_code,a.religion_code from index_of_name a,district b, ro_master c,party_CODE d where a.District_Code = b.district_code  and a.ro_code = c.ro_code  and a.party_code = d.ec_code  and a.district_code = '" + Do_code + "' and c.district_code = '" + Do_code + "' and a.Ro_code = '" + RO_Code + "' and c.Ro_code = '" + RO_Code + "' and a.book = '" + year + "' and a.deed_year = '" + deed_year + "' and a.deed_no = '" + deed_no + "' order by a.item_no";
            //sqlStr = "select Concat('Name: ',a.initial_name,' ',a.First_name,' ',a.Last_name,'\r',a.Relation,'\r','\r','Address: ',a.address,'\r','District: ',a.Address_district_name,' Pin: ',a.pin,'\r','State: West Bangal, Country: India') as 'Name & Address',Concat('Status : '),a.item_no,D.EC_NAME,a.Admit_code,a.Address,a.Address_district_code,a.Address_district_name,a.Address_ps_code,a.Address_ps_name,a.Father_mother,a.Rel_code,a.Relation,a.occupation_code,a.religion_code from index_of_name a,district b, ro_master c,party_CODE d where a.District_Code = b.district_code  and a.ro_code = c.ro_code  and a.party_code = d.ec_code  and a.district_code = '" + Do_code + "' and c.district_code = '" + Do_code + "' and a.Ro_code = '" + RO_Code + "' and c.Ro_code = '" + RO_Code + "' and a.book = '" + year + "' and a.deed_year = '" + deed_year + "' and a.deed_no = '" + deed_no + "' order by a.item_no";
            sqlStr = "select Concat('Name: ',patient_name,' ','\r','\r','Gender: ',gender,' ','\r','\r','Guardian: ',guardian_name,' ','\r','\r','Address: ',address) as 'Name and Address',Concat('Department: ',dep_name,' ','\r','\r','Registartion Number: ',reg_number,' ','\r',\r',Admission Date: ',date_format(admission_date,'%D-%M-%Y')) as 'Department,Registration and Admission',date_format(death_date,'%D-%M-%Y') as 'Death Details',date_format(discharge_date,'%D-%M-%Y') as 'Discharge Details',death from metadata_entry where proj_code = '" + projK + "' and bundle_key = '" + bundleKey + "' and patient_id = '" + patientID + "'";
            try
            {
                sqlAdap = new OdbcDataAdapter(sqlStr, sqlCon);
                sqlAdap.Fill(dsImage);
                for (int i = 0; i < dsImage.Tables[0].Rows.Count; i++)
                {
                    if ((dsImage.Tables[0].Rows[i][2].ToString() != "" || dsImage.Tables[0].Rows[i][2].ToString() != null) && dsImage.Tables[0].Rows[i][4].ToString() == "Y")
                    {
                        dsImage.Tables[0].Rows[i][2] = "Death Date: " + dsImage.Tables[0].Rows[i][2];
                    }
                    else
                    {
                        dsImage.Tables[0].Rows[i][2] = "Not Dead...";
                    }
                    if (dsImage.Tables[0].Rows[i][3].ToString() == "" || dsImage.Tables[0].Rows[i][3].ToString() == null)
                    {
                        dsImage.Tables[0].Rows[i][3] = "Not Discharge...";
                    }
                    else
                    {
                        dsImage.Tables[0].Rows[i][3] = "Discharge Date: " + dsImage.Tables[0].Rows[i][3];
                    }
                    //if (dsImage.Tables[0].Rows[i][3].ToString() == "Y")
                    //{
                    //    dsImage.Tables[0].Rows[i][0] = dsImage.Tables[0].Rows[i][0] + "And More Persons.";
                    //}
                }
            }
            catch (Exception ex)
            {
                sqlAdap.Dispose();
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes(sqlStr + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex);
            }
            //DataRow dr = dsImage.Tables[0].Rows[0];
            //dsImage.Dispose();
            return dsImage.Tables[0];
        }


        private string policy_number = string.Empty;
        int selectedIndex = 0;
        private void grdBox_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            wfeImage wfeimg = new wfeImage(sqlCon);
            selectedIndex = e.RowIndex;
            //string deedno = grdBox.Rows[e.RowIndex].Cells[0].Value.ToString() + grdBox.Rows[e.RowIndex].Cells[1].Value.ToString() + grdBox.Rows[e.RowIndex].Cells[2].Value.ToString() + grdBox.Rows[e.RowIndex].Cells[3].Value.ToString() + "[" + grdBox.Rows[e.RowIndex].Cells[4].Value.ToString() + "]";
            string deedno = grdBox.Rows[e.RowIndex].Cells[3].Value.ToString();
            policy_number = deedno;
            string deed_no = grdBox.Rows[e.RowIndex].Cells[3].Value.ToString();
            DataTable dt1 = GetAllNameEX(projKey, bundleKey, grdBox.Rows[e.RowIndex].Cells[3].Value.ToString());
            dgvname.DataSource = dt1;
            dgvname.Columns[0].Width = dgvname.Width / 4;
            dgvname.Columns[1].Width = dgvname.Width / 4;
            dgvname.Columns[2].Width = dgvname.Width / 4;
            dgvname.Columns[3].Width = dgvname.Width / 4;
            //dgvname.Columns[1].Width = dgvname.Width / 4;
            dgvname.Columns[4].Visible = false;
            for (int i = 0; i < dgvname.Rows.Count; i++)
            {
                //dgvname.Rows[i].Height = 100;
                dgvname.Rows[i].Selected = true;
            }

            if (LicLogCount(grdBox.Rows[e.RowIndex].Cells[0].Value.ToString(), grdBox.Rows[e.RowIndex].Cells[1].Value.ToString(), grdBox.Rows[e.RowIndex].Cells[3].Value.ToString()) == 0)
            {
                InitiateQaPolicyException(grdBox.Rows[e.RowIndex].Cells[0].Value.ToString(), grdBox.Rows[e.RowIndex].Cells[1].Value.ToString(), grdBox.Rows[e.RowIndex].Cells[3].Value.ToString(), crd);
            }
           

            ShowImages(grdBox.Rows[e.RowIndex].Cells[0].Value.ToString(), grdBox.Rows[e.RowIndex].Cells[1].Value.ToString(), grdBox.Rows[e.RowIndex].Cells[3].Value.ToString());
            
        }

        public bool InitiateQaPolicyException(string proj, string bundle,string pID,NovaNet.Utils.Credentials prmCrd)
        {
            string sqlStr = null;
            OdbcTransaction sqlTrans = null;
            bool commitBol = true;
            OdbcCommand sqlCmd = new OdbcCommand();
            try
            {
                sqlTrans = sqlCon.BeginTransaction();
                sqlCmd.Connection = sqlCon;
                sqlCmd.Transaction = sqlTrans;

                sqlStr = @"insert into lic_qa_log (proj_key,box_number,policy_number,batch_key,created_by,created_dttm) values(" + proj + ",'" + 1 + "','" + pID + "'," + bundle + ",'" + prmCrd.created_by + "','" + prmCrd.created_dttm + "')";
                sqlCmd.CommandText = sqlStr;
                sqlCmd.ExecuteNonQuery();

                sqlTrans.Commit();
                commitBol = true;
            }
            catch (Exception ex)
            {
                commitBol = false;
                sqlTrans.Rollback();
                sqlCmd.Dispose();
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes(sqlStr + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex);
            }
            return commitBol;
        }

        public int LicLogCount(string proj, string bundle, string pID)
        {
           
            string sqlStr = null;
            DataSet dsImage = new DataSet();
            OdbcDataAdapter sqlAdap = null;

            sqlStr = "select Count(policy_number) from lic_qa_log where proj_key  = '" + proj + "' and batch_key ='" + bundle + "' and  policy_number ='" + pID + "' ";
                   

            try
            {
                sqlAdap = new OdbcDataAdapter(sqlStr, sqlCon);
                sqlAdap.Fill(dsImage);
            }
            catch (Exception ex)
            {
                sqlAdap.Dispose();
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes(sqlStr + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex);
            }
            return Convert.ToInt32(dsImage.Tables[0].Rows[0][0]);

        }

        private void ShowImages(string projK, string bundleK, string pID)
        {
            try
            {
                tabControl1.TabPages[1].Enabled = true;
                lstImage.Items.Clear();
                picControl.Refresh();
                string deed_no = pID;
                string policyname = deed_no;
                wfeImage wfeimg = new wfeImage(sqlCon);
                dsimage11 = wfeimg.GetAllExportedImagewithProj(policyname);


                /* string sql = "select sum(a.qc_size) from image_master a, deed_details b where b.deed_no ='" + deed_no + "' and b.district_code ='"+dist+"'and b.ro_code='"+ro+"' and b.book='"+book+"' and b.deed_year ='"+deed_year+"' and a.proj_key=b.proj_key and a.batch_key = b.batch_key and a.box_number = b.box_key and a.status<>29 order by a.serial_no";
                    
                 DataTable dt = new DataTable();
                 OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
                 OdbcDataAdapter sqlAdap = new OdbcDataAdapter(cmd);
                 
                 sqlAdap.Fill(dt);
                 comboBox2.DataSource = dt;*/
                //  string sql = "select sum(qc_size) from image_master where policy_number ='" + policyname + "'  and status<>29 ";
                string sqlstr = "select sum(qc_size) from image_master where policy_number ='" + policyname + "'  and status<>29 ";
                DataTable dt = new DataTable();
                OdbcCommand cmd = new OdbcCommand(sqlstr, sqlCon);
                OdbcDataAdapter sqlAdap = new OdbcDataAdapter(cmd);

                sqlAdap.Fill(dt);

                //comboBox2.SelectedIndex = 0;
                comboBox2.DataSource = dt;
                string abc = dt.Rows[0][0].ToString();

                decimal t = Math.Round(decimal.Parse(abc));
                int v = (int)t;
                //int abcd = int.Parse(abc.Substring(0, abc.IndexOf('.')));
                /*double decimalNumber = Convert.ToDouble(dt.Rows[0][0].ToString());
                var numbersBeforeDecimalPoint = Math.Truncate(decimalNumber);
                var numbersAfterDecimalPoint = decimalNumber - numbersBeforeDecimalPoint;*/
                labelDeedTotalSize.Text = "Total Size: " + v + " " + "KB";


                wfePolicy wPol = new wfePolicy(sqlCon);
                int polStatus = GetPolicyStatus(policyname);
                if ((polStatus == 0) || (polStatus == 1) || (polStatus == 2) || (polStatus == 3) || (polStatus == 4))
                {
                    MessageBox.Show("Image information not available/ not processed, try again...");
                    tabControl1.TabPages[1].Enabled = false;
                    picControl.Image = null;
                    return;
                }
                wfePolicy wfepol = new wfePolicy(sqlCon);
                string proj = GetPolicyPath(policyname, 1).Tables[0].Rows[0][0].ToString();
                string batch = GetPolicyPath(policyname, 1).Tables[0].Rows[0][1].ToString();
                policyPath = GetPolicyPath(proj, batch);
                policyPath = policyPath +"\\" +policyname+ "\\QC";

                string[] files = Directory.GetFiles(policyPath);
                if (dsimage11.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsimage11.Tables[0].Rows.Count; i++)
                    {
                        lstImage.Items.Add(dsimage11.Tables[0].Rows[i][3].ToString());
                    }
                    tabControl1.SelectedTab = tbImages;
                    lstImage.SelectedIndex = 0;
                    /*txtVol.Text = grdBox.Rows[grdBox.CurrentRow.Index].Cells[11].Value.ToString();
                    txtVol.Enabled = false;
                    txtPgFrom.Text = grdBox.Rows[grdBox.CurrentRow.Index].Cells[12].Value.ToString();
                    txtPgFrom.Enabled = false;
                    txtPgTo.Text = grdBox.Rows[grdBox.CurrentRow.Index].Cells[13].Value.ToString();
                    txtPgTo.Enabled = false;*/
                }
                else
                {
                    MessageBox.Show("Document is not Scanned Yet, Please Scan the Document First...");
                    tabControl1.TabPages[1].Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
                tabControl1.TabPages[1].Enabled = false;
                picControl.Image = null;
            }
        }

        public string GetPolicyPath(string proj, string batch)
        {
            string sqlStr = null;
            string path = string.Empty;

            DataSet policyDs = new DataSet();

            try
            {
                sqlStr = "select B.bundle_path from bundle_master B  where B.proj_code = '" + proj + "' and B.bundle_key = '"+batch+"' ";
                sqlAdap = new OdbcDataAdapter(sqlStr, sqlCon);
                sqlAdap.Fill(policyDs);
            }
            catch (Exception ex)
            {
                sqlAdap.Dispose();
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes(sqlStr + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex);
            }

            if (policyDs.Tables[0].Rows.Count > 0)
            {
                path = policyDs.Tables[0].Rows[0]["bundle_path"].ToString();
            }
            return path;
        }

        public DataSet GetPolicyPath(string policy_no, int temp)
        {
            string sqlStr = null;
            string path = string.Empty;

            DataSet policyDs = new DataSet();

            try
            {
                sqlStr = "select B.proj_code,B.bundle_key from metadata_entry B  where B.patient_id = '" + policy_no + "'";
                sqlAdap = new OdbcDataAdapter(sqlStr, sqlCon);
                sqlAdap.Fill(policyDs);
            }
            catch (Exception ex)
            {
                sqlAdap.Dispose();
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes(sqlStr + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex);
            }

            if (policyDs.Tables[0].Rows.Count > 0)
            {
                // path = policyDs.Tables[0].Rows[0]["policy_path"].ToString();
            }
            return policyDs;
        }

        public int GetPolicyStatus(string policyNo)
        {
            string sqlStr = null;
            DataSet dsImage = new DataSet();
            OdbcDataAdapter sqlAdap = null;

            sqlStr = "select status from metadata_entry " +
                    " where patient_id='" + policyNo + "'";

            try
            {
                sqlAdap = new OdbcDataAdapter(sqlStr, sqlCon);
                sqlAdap.Fill(dsImage);
            }
            catch (Exception ex)
            {
                sqlAdap.Dispose();
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes(sqlStr + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex);
            }
            return Convert.ToInt32(dsImage.Tables[0].Rows[0]["status"]);
        }

        private void grdBox_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void grdBox_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void grdBox_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ContextMenu mCheck = new ContextMenu();
                mCheck.MenuItems.Add(new MenuItem("Patient details"));
                mCheck.MenuItems.Add(new MenuItem("Images"));
                mCheck.MenuItems[0].Click += new EventHandler(mCheck_Click);
                mCheck.MenuItems[1].Click += new EventHandler(mImages_Click);
                int currentMouseOverRow = grdBox.HitTest(e.X, e.Y).RowIndex;
                selectedIndex = currentMouseOverRow;
                //if (currentMouseOverRow >= 0)
                //{
                //    mCheck.MenuItems.Add(new MenuItem(string.Format("", currentMouseOverRow.ToString())));
                //}
                mCheck.Show(grdBox, new Point(e.X, e.Y));
            }
        }

        private void mImages_Click(object sender, EventArgs e)
        {
            ShowImages(grdBox.CurrentRow.Cells[0].Value.ToString(), grdBox.CurrentRow.Cells[1].Value.ToString(), grdBox.CurrentRow.Cells[2].Value.ToString(), grdBox.CurrentRow.Cells[3].Value.ToString());
        }

        private void ShowImages(string projKey, string bundleKey, string item, string pID)
        {
            try
            {
                tabControl1.TabPages[1].Enabled = true;
                lstImage.Items.Clear();
                picControl.Refresh();
                string deed_no = pID;
                string policyname =  deed_no ;
                wfeImage wfeimg = new wfeImage(sqlCon);
                dsimage11 = wfeimg.GetAllExportedImagewithProj(policyname);


                /* string sql = "select sum(a.qc_size) from image_master a, deed_details b where b.deed_no ='" + deed_no + "' and b.district_code ='"+dist+"'and b.ro_code='"+ro+"' and b.book='"+book+"' and b.deed_year ='"+deed_year+"' and a.proj_key=b.proj_key and a.batch_key = b.batch_key and a.box_number = b.box_key and a.status<>29 order by a.serial_no";
                    
                 DataTable dt = new DataTable();
                 OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
                 OdbcDataAdapter sqlAdap = new OdbcDataAdapter(cmd);
                 
                 sqlAdap.Fill(dt);
                 comboBox2.DataSource = dt;*/
                //  string sql = "select sum(qc_size) from image_master where policy_number ='" + policyname + "'  and status<>29 ";
                string sqlstr = "select sum(qc_size) from image_master where policy_number ='" + policyname + "'  and status<>29 ";
                DataTable dt = new DataTable();
                OdbcCommand cmd = new OdbcCommand(sqlstr, sqlCon);
                OdbcDataAdapter sqlAdap = new OdbcDataAdapter(cmd);

                sqlAdap.Fill(dt);

                //comboBox2.SelectedIndex = 0;
                comboBox2.DataSource = dt;
                string abc = dt.Rows[0][0].ToString();

                decimal t = Math.Round(decimal.Parse(abc));
                int v = (int)t;
                //int abcd = int.Parse(abc.Substring(0, abc.IndexOf('.')));
                /*double decimalNumber = Convert.ToDouble(dt.Rows[0][0].ToString());
                var numbersBeforeDecimalPoint = Math.Truncate(decimalNumber);
                var numbersAfterDecimalPoint = decimalNumber - numbersBeforeDecimalPoint;*/
                labelDeedTotalSize.Text = "Total Size: " + v + " " + "KB";


                wfePolicy wPol = new wfePolicy(sqlCon);
                int polStatus = GetPolicyStatus(policyname);
                if ((polStatus == 0) || (polStatus == 1) || (polStatus == 2) || (polStatus == 3) || (polStatus == 4))
                {
                    MessageBox.Show("Image information not available/ not processed, try again...");
                    tabControl1.TabPages[1].Enabled = false;
                    picControl.Image = null;
                    return;
                }
                wfePolicy wfepol = new wfePolicy(sqlCon);
                string proj = GetPolicyPath(policyname, 1).Tables[0].Rows[0][0].ToString();
                string batch = GetPolicyPath(policyname, 1).Tables[0].Rows[0][1].ToString();
                policyPath = GetPolicyPath(proj, batch);
                policyPath = policyPath + "\\" + policyname + "\\QC";

                string[] files = Directory.GetFiles(policyPath);
                if (dsimage11.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsimage11.Tables[0].Rows.Count; i++)
                    {
                        lstImage.Items.Add(dsimage11.Tables[0].Rows[i][3].ToString());
                    }
                    tabControl1.SelectedTab = tbImages;
                    lstImage.SelectedIndex = 0;
                    /*txtVol.Text = grdBox.Rows[grdBox.CurrentRow.Index].Cells[11].Value.ToString();
                    txtVol.Enabled = false;
                    txtPgFrom.Text = grdBox.Rows[grdBox.CurrentRow.Index].Cells[12].Value.ToString();
                    txtPgFrom.Enabled = false;
                    txtPgTo.Text = grdBox.Rows[grdBox.CurrentRow.Index].Cells[13].Value.ToString();
                    txtPgTo.Enabled = false;*/
                }
                else
                {
                    MessageBox.Show("Document is not Scanned Yet, Please Scan the Document First...");
                    tabControl1.TabPages[1].Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
                tabControl1.TabPages[1].Enabled = false;
                picControl.Image = null;
            }
        }


        private void mCheck_Click(object sender, EventArgs e)
        {
            try
            {
                //DataLayerDefs.DeedControl pdc = new DataLayerDefs.DeedControl();
                //string policy_no = policyLst.SelectedItem.ToString();
                string proj = grdBox.Rows[selectedIndex].Cells[0].Value.ToString();
                string bundle = grdBox.Rows[selectedIndex].Cells[1].Value.ToString();
                string item = grdBox.Rows[selectedIndex].Cells[2].Value.ToString();
                string patientID = grdBox.Rows[selectedIndex].Cells[3].Value.ToString();
                //string deed_no = grdBox.Rows[selectedIndex].Cells[4].Value.ToString();
                if (proj != null && bundle != null && item != null && patientID != null)
                {
                    OdbcTransaction trans = sqlCon.BeginTransaction();
                    

                    //EntryForm frm = new EntryForm(sqlCon, projKey, bundleKey, _mode, patientID);
                    //frm.ShowDialog(this);

                    fmEntry fm = new fmEntry(sqlCon, projKey, bundleKey, DataLayerDefs.Mode._Edit, patientID);
                    fm.ShowDialog(this);

                }
                else
                {
                    MessageBox.Show(this, "Error ....", "Patient Switboard", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while fetching the details... " + ex.Message);
            }
        }

        public System.Drawing.Image CreateThumbnail(System.Drawing.Image pImage, int lnWidth, int lnHeight)
        {

            Bitmap bmp = new Bitmap(lnWidth, lnHeight);
            try
            {

                DateTime stdt = DateTime.Now;

                //create a new Bitmap the size of the new image

                //create a new graphic from the Bitmap
                Graphics graphic = Graphics.FromImage((System.Drawing.Image)bmp);
                graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
                //draw the newly resized image
                graphic.DrawImage(pImage, 0, 0, lnWidth, lnHeight);
                //dispose and free up the resources
                graphic.Dispose();
                DateTime dt = DateTime.Now;
                TimeSpan tp = dt - stdt;
                //MessageBox.Show(tp.Milliseconds.ToString());
                //return the image

            }
            catch
            {
                return null;
            }
            return (System.Drawing.Image)bmp;
        }
        private void ChangeSize()
        {
            System.Drawing.Image imgTot = null;
            try
            {
                if (img.IsValid() == true)
                {
                    System.Drawing.Image newImage = img.GetBitmap();
                    if (newImage.PixelFormat == PixelFormat.Format1bppIndexed)
                    {
                        picControl.Height = tabControl2.Height;
                        picControl.Width = tabControl2.Width;
                        //if (!System.IO.File.Exists(imgFileName)) return;

                        //imgAll.LoadBitmapFromFile(imgFileName);
                        //if (imgAll.GetBitmap().PixelFormat == PixelFormat.Format24bppRgb)
                        //{
                        //    imgAll.GetLZW("tmp1.TIF");
                        //    imgTot = Image.FromFile("tmp1.TIF");
                        //    newImage = imgTot;
                        //    //File.Delete("tmp1.TIF");
                        //}
                        //else
                        //{
                        //    newImage = System.Drawing.Image.FromFile(imgFileName);
                        //}

                        double scaleX = (double)picControl.Width / (double)newImage.Width;
                        double scaleY = (double)picControl.Height / (double)newImage.Height;
                        double Scale = Math.Min(scaleX, scaleY);
                        int w = (int)(newImage.Width * Scale) * 4;
                        int h = (int)(newImage.Height * Scale) * 2;
                        picControl.Width = w;
                        picControl.Height = h;
                        picControl.Image = CreateThumbnail(newImage, w, h); //newImage.GetThumbnailImage(w, h, new System.Drawing.Image.GetThumbnailImageAbort(GetThumbnailImageAbort), IntPtr.Zero);
                        newImage.Dispose();
                        picControl.Refresh();
                        if (imgTot != null)
                        {
                            imgTot.Dispose();
                            imgTot = null;
                            if (File.Exists("tmp1.tif"))
                                File.Delete("tmp1.TIF");
                        }
                    }
                    else
                    {
                        picControl.Height = tabControl1.Height - 75;
                        picControl.Width = tabControl2.Width - 100;
                        //img.LoadBitmapFromFile(imgFileName);
                        picControl.Image = img.GetBitmap();
                        picControl.SizeMode = PictureBoxSizeMode.StretchImage;
                        picControl.Refresh();
                    }
                }
            }
            catch (Exception ex)
            {
                exMailLog.Log(ex);
                MessageBox.Show("Error ..." + ex.Message, "Error");
            }
        }

        private void lstImage_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                comboBox3.Hide();
                string aa = policyPath;
                imgFileName = policyPath + @"\" + lstImage.SelectedItem.ToString();
                string sqlStr = "select qc_size from image_master where page_index_name ='" + lstImage.SelectedItem.ToString() + "' and status<>29 ";
                DataTable dt = new DataTable();
                OdbcCommand cmd = new OdbcCommand(sqlStr, sqlCon);
                OdbcDataAdapter sqlAdap = new OdbcDataAdapter(cmd);

                sqlAdap.Fill(dt);
                comboBox3.DataSource = dt;
                comboBox3.SelectedIndex = 0;

                string abc = dt.Rows[0][0].ToString();

                decimal t = Math.Round(decimal.Parse(abc));
                int v = (int)t;
                /* string abc = dt.Rows[0][0].ToString();
                 decimal t = Math.Round(decimal.Parse(abc));
                 int v = (int)t;*/
                labelImageSize.Text = "Image Size: " + v + " " + "KB";
                img.LoadBitmapFromFile(imgFileName);
                picControl.Image = img.GetBitmap();
                //picControl.SizeMode = PictureBoxSizeMode.StretchImage;
                ChangeSize();
                //ZoomIn();
                //picControl.Refresh();
                //ZoomIn();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Document is not Scanned Yet, Please Scan the Document First..." + ex.Message.ToString());
            }
            //int pos;
            //string changedImage=null;
            //double fileSize;
            //string currntDoc ;
            //wfeImage wImage = null;
            ////string photoImageName=null;

            //try
            //{
            //    pos = lstImage.SelectedItem.ToString().IndexOf("-");
            //    changedImage = lstImage.SelectedItem.ToString().Substring(0, pos);
            //    //changedImage=lstImage.SelectedItem.ToString().Substring(0,pos);
            //    pImage = new CtrlImage(Convert.ToInt32(cmbProject.SelectedValue.ToString()), Convert.ToInt32(cmbBatch.SelectedValue.ToString()), boxNo.ToString(), policyNumber, changedImage, string.Empty);
            //    wImage = new wfeImage(sqlCon, pImage);
            //    changedImage = wImage.GetIndexedImageName();

            //    if ((policyStatus == (int)eSTATES.POLICY_INDEXED) || (policyStatus == (int)eSTATES.POLICY_CHECKED) || (policyStatus == (int)eSTATES.POLICY_EXCEPTION) || (policyStatus == (int)eSTATES.POLICY_EXPORTED))
            //    {
            //        if (Directory.Exists(policyPath + "\\" + ihConstants._FQC_FOLDER))
            //        {
            //            picPath = policyPath + "\\" + ihConstants._FQC_FOLDER;
            //            imagePath = policyPath + "\\" + ihConstants._FQC_FOLDER + "\\" + changedImage;
            //            if (changedImage.Substring(policyLen, 6) == "_000_A")
            //            {
            //                imgFileName = policyPath + "\\" + ihConstants._FQC_FOLDER + "\\" + changedImage;
            //                if (File.Exists(imgFileName) == false)
            //                {
            //                    imgFileName = policyPath + "\\" + ihConstants._INDEXING_FOLDER + "\\" + changedImage;
            //                    picPath = policyPath + "\\" + ihConstants._INDEXING_FOLDER;
            //                }
            //                //img.SaveAsTiff(policyPath + "\\" + ihConstants._FQC_FOLDER + "\\" + changedImage, IGRComressionTIFF.JPEG);
            //                photoPath = imagePath;
            //            }
            //            else
            //            {
            //                imgFileName = policyPath + "\\" + ihConstants._FQC_FOLDER + "\\" + changedImage;
            //                if (File.Exists(imgFileName) == false)
            //                {
            //                    imgFileName = policyPath + "\\" + ihConstants._INDEXING_FOLDER + "\\" + changedImage;
            //                    picPath = policyPath + "\\" + ihConstants._INDEXING_FOLDER;
            //                }
            //            }
            //        }
            //        else
            //        {
            //            imagePath = policyPath + "\\" + ihConstants._INDEXING_FOLDER + "\\" + changedImage;
            //            picPath = policyPath + "\\" + ihConstants._INDEXING_FOLDER;
            //            if (changedImage.Substring(policyLen, 6) == "_000_A")
            //            {
            //                imgFileName = policyPath + "\\" + ihConstants._INDEXING_FOLDER + "\\" + changedImage;
            //                img.LoadBitmapFromFile(imgFileName);
            //                //img.SaveAsTiff(policyPath + "\\" + ihConstants._INDEXING_FOLDER + "\\" + changedImage, IGRComressionTIFF.JPEG);
            //                photoPath = imagePath;
            //            }
            //            else
            //            {
            //                imgFileName = policyPath + "\\" + ihConstants._INDEXING_FOLDER + "\\" + changedImage;

            //            }
            //        }

            //    }
            //    else
            //    {
            //        picPath = policyPath + "\\" + ihConstants._FQC_FOLDER;
            //        imagePath = policyPath + "\\" + ihConstants._FQC_FOLDER + "\\" + changedImage;
            //        if (changedImage.Substring(policyLen, 6) == "_000_A")
            //        {
            //            imgFileName = policyPath + "\\" + ihConstants._FQC_FOLDER + "\\" + changedImage;
            //            if (File.Exists(imgFileName) == false)
            //            {
            //                imgFileName = policyPath + "\\" + ihConstants._INDEXING_FOLDER + "\\" + changedImage;
            //                picPath = policyPath + "\\" + ihConstants._INDEXING_FOLDER;
            //            }
            //            //img.SaveAsTiff(policyPath + "\\" + ihConstants._FQC_FOLDER + "\\" + changedImage, IGRComressionTIFF.JPEG);
            //            photoPath = imagePath;
            //        }
            //        else
            //        {
            //            imgFileName = policyPath + "\\" + ihConstants._FQC_FOLDER + "\\" + changedImage;
            //            if (File.Exists(imgFileName) == false)
            //            {
            //                imgFileName = policyPath + "\\" + ihConstants._INDEXING_FOLDER + "\\" + changedImage;
            //                picPath = policyPath + "\\" + ihConstants._INDEXING_FOLDER;
            //            }
            //        }

            //    }
            //    System.IO.FileInfo info = new System.IO.FileInfo(imgFileName);

            //    fileSize = info.Length;
            //    fileSize = fileSize / 1024;
            //    lblImageSize.Text = Convert.ToString(Math.Round(fileSize, 2)) + " KB";
            //    img.LoadBitmapFromFile(imgFileName);
            //    int dashPos = lstImage.SelectedItem.ToString().IndexOf("-") + 1;
            //    //currntDoc = lstImage.Items[lstImage.SelectedIndex].ToString().Substring(dashPos);

            //    //if ((prevDoc != currntDoc))
            //    //{
            //    //    ListViewItem lvwItem = lvwDockTypes.FindItemWithText(currntDoc);
            //    //    lvwDockTypes.Items[lvwItem.Index].Selected = true;
            //    //}
            //    //firstDoc = false;
            //    if (imgFileName != string.Empty)
            //    {
            //        ChangeSize();
            //    }
            //    //prevDoc = currntDoc;
            //    //ChangeSize();
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("Error while generating the preview....");
            //    exMailLog.Log(ex);
            //}
        }

        private void cmdNext_Click(object sender, EventArgs e)
        {
            ListViewItem lvwItem;
            
            if (tabControl2.SelectedIndex == 0)
            {
                if (lstImage.Items.Count > 0)
                {
                    if ((lstImage.Items.Count - 1) != lstImage.SelectedIndex)
                    {
                        lstImage.SelectedIndex = lstImage.SelectedIndex + 1;
                    }
                }
                if (tabControl2.SelectedIndex == 1)
                {
                    if (lstImage.SelectedIndex != 0)
                    {
                        int dashPos = lstImage.SelectedItem.ToString().IndexOf("-") + 1;
                        string currntDoc = lstImage.Items[lstImage.SelectedIndex - 1].ToString().Substring(dashPos);
                        string prevDoc = lstImage.Items[lstImage.SelectedIndex].ToString().Substring(dashPos);
                        if (currntDoc != prevDoc)
                        {
                            lvwItem = lvwDockTypes.FindItemWithText(prevDoc);
                            lvwDockTypes.Items[lvwItem.Index].Selected = true;
                            lvwDockTypes.Focus();
                            //lstImage.Focus();
                        }
                    }
                }
            }
        }

        private void cmdPrevious_Click(object sender, EventArgs e)
        {
            ListViewItem lvwItem;
            if (tabControl2.SelectedIndex == 0)
            {
                if (lstImage.SelectedIndex != 0)
                {
                    lstImage.SelectedIndex = lstImage.SelectedIndex - 1;
                }
                if (tabControl2.SelectedIndex == 1)
                {
                    if (lstImage.SelectedIndex != 0)
                    {
                        int dashPos = lstImage.SelectedItem.ToString().IndexOf("-") + 1;
                        string currntDoc = lstImage.Items[lstImage.SelectedIndex].ToString().Substring(dashPos);
                        string prevDoc = lstImage.Items[lstImage.SelectedIndex + 1].ToString().Substring(dashPos);
                        if (currntDoc != prevDoc)
                        {
                            lvwItem = lvwDockTypes.FindItemWithText(currntDoc);
                            lvwDockTypes.Items[lvwItem.Index].Selected = true;
                            lvwDockTypes.Focus();
                        }
                    }
                }
            }
        }

        private void cmdin_Click(object sender, EventArgs e)
        {
            ZoomIn();
        }

        private void cmdout_Click(object sender, EventArgs e)
        {
            ZoomOut();
        }

        int ZoomIn()
        {
            try
            {
                if (img.IsValid() == true)
                {
                    //picControl.Dock = DockStyle.None;
                    //OperationInProgress = ihConstants._OTHER_OPERATION;
                    keyPressed = keyPressed + 1;
                    zoomHeight = Convert.ToInt32(img.GetBitmap().Height * (1.2));
                    zoomWidth = Convert.ToInt32(img.GetBitmap().Width * (1.2));
                    zoomSize.Height = zoomHeight;
                    zoomSize.Width = zoomWidth;

                    picControl.Width = Convert.ToInt32(Convert.ToDouble(picControl.Width) * 1.2);
                    picControl.Height = Convert.ToInt32(Convert.ToDouble(picControl.Height) * 1.2);
                    //picControl.Refresh();
                    ChangeZoomSize();

                    //delinsrtBol = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while zooming the image " + ex.Message, "Zoom Error");
                exMailLog.Log(ex);
            }
            return 0;
        }
        private void ChangeZoomSize()
        {
            if (!System.IO.File.Exists(imgFileName)) return;
            System.Drawing.Image newImage = System.Drawing.Image.FromFile(imgFileName);
            double scaleX = (double)picControl.Width / (double)newImage.Width;
            double scaleY = (double)picControl.Height / (double)newImage.Height;
            double Scale = Math.Min(scaleX, scaleY);
            int w = (int)(newImage.Width * Scale);
            int h = (int)(newImage.Height * Scale);
            picControl.Width = w;
            picControl.Height = h;
            picControl.Image = newImage.GetThumbnailImage(w, h, new System.Drawing.Image.GetThumbnailImageAbort(GetThumbnailImageAbort), IntPtr.Zero);
            //picControl.Invalidate();
            newImage.Dispose();
        }
        private bool GetThumbnailImageAbort()
        {
            return false;
        }
        int ZoomOut()
        {
            try
            {
                if (keyPressed > 0)
                {
                    picControl.Dock = DockStyle.None;
                    //OperationInProgress = ihConstants._OTHER_OPERATION;
                    keyPressed = keyPressed + 1;
                    zoomHeight = Convert.ToInt32(img.GetBitmap().Height / (1.2));
                    zoomWidth = Convert.ToInt32(img.GetBitmap().Width / (1.2));
                    zoomSize.Height = zoomHeight;
                    zoomSize.Width = zoomWidth;

                    picControl.Width = Convert.ToInt32(Convert.ToDouble(picControl.Width) / 1.2);
                    picControl.Height = Convert.ToInt32(Convert.ToDouble(picControl.Height) / 1.2);
                    picControl.Refresh();
                    ChangeZoomSize();
                    //delinsrtBol = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while zooming the image " + ex.Message, "Zoom Error");
            }
            return 0;
        }

        private void lvwDockTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            //pgOne.Visible = false;
            //pgTwo.Visible = false;
            //pgThree.Visible = false;
            currntPg = 0;
            //if (tabControl2.SelectedIndex == 1)
            //{
            for (int i = 0; i < lvwDockTypes.Items.Count; i++)
            {
                if (lvwDockTypes.Items[i].Selected == true)
                {
                    selDocType = lvwDockTypes.Items[i].SubItems[0].Text;
                    ShowThumbImage(selDocType);
                    for (int j = 0; j < lstImage.Items.Count; j++)
                    {
                        string srchStr = lstImage.Items[j].ToString();
                        if (srchStr.IndexOf(selDocType) > 0)
                        {
                            lstImage.SelectedIndex = j;
                            break;
                        }
                    }
                    lstImage.Focus();
                }
            }
        }

        private void ShowThumbImage(string pDocType)
        {
            DataSet ds = new DataSet();
            string imageFileName;
            System.Drawing.Image imgNew = null;
            IContainerControl icc = tabControl2.GetContainerControl();

            //tabControl2.SelectedIndex = 1;
            //picBig.Visible = false;
            //panelBig.Visible = false;
            //picBig.Image = null;
            System.Drawing.Image imgThumbNail = null;

            pImage = new CtrlImage(Convert.ToInt32(projCode), Convert.ToInt32(batchCode), boxNo.ToString(), policyNumber, string.Empty, pDocType);
            wfeImage wImage = new wfeImage(sqlCon, pImage);
            ds = wImage.GetAllIndexedImageName();
            ClearPicBox();
            if (ds.Tables[0].Rows.Count > 0)
            {
                imageName = new string[ds.Tables[0].Rows.Count];
                if (ds.Tables[0].Rows.Count <= 6)
                {
                    //pgOne.Visible = true;
                    //pgTwo.Visible = false;
                    //pgThree.Visible = false;
                }
                if ((ds.Tables[0].Rows.Count > 6) && (ds.Tables[0].Rows.Count <= 12))
                {
                    //pgOne.Visible = true;
                    //pgTwo.Visible = true;
                    //pgThree.Visible = false;
                }
                if ((ds.Tables[0].Rows.Count > 12) && (ds.Tables[0].Rows.Count <= 14))
                {
                    //pgOne.Visible = true;
                    //pgTwo.Visible = true;
                    //pgThree.Visible = true;
                }
                for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                {
                    imageFileName = picPath + "\\" + ds.Tables[0].Rows[j][0].ToString();
                    imgAll.LoadBitmapFromFile(imageFileName);

                    if (imgAll.GetBitmap().PixelFormat == PixelFormat.Format24bppRgb)
                    {
                        try
                        {
                            imgAll.GetLZW("tmp.TIF");
                            imgNew = System.Drawing.Image.FromFile("tmp.TIF");
                            imgThumbNail = imgNew;
                        }
                        catch (Exception ex)
                        {
                            string err = ex.Message;
                        }
                    }
                    else
                    {
                        imgThumbNail = System.Drawing.Image.FromFile(imageFileName);
                    }
                    imageName[j] = imageFileName;
                    if (!System.IO.File.Exists(imageFileName)) return;
                    //imgThumbNail = Image.FromFile(imageFileName);
                    double scaleX = 0;//(double)pictureBox1.Width / (double)imgThumbNail.Width;
                    double scaleY = 0;//(double)pictureBox1.Height / (double)imgThumbNail.Height;
                    double Scale = Math.Min(scaleX, scaleY);
                    int w = (int)(imgThumbNail.Width * Scale);
                    int h = (int)(imgThumbNail.Height * Scale);
                    w = w - 5;
                    imgThumbNail = imgThumbNail.GetThumbnailImage(w, h, new System.Drawing.Image.GetThumbnailImageAbort(GetThumbnailImageAbort), IntPtr.Zero);

                    if (j == 0)
                    {
                        //pictureBox1.Image = imgThumbNail;
                        //pictureBox1.Tag = imageFileName;
                    }
                    if (j == 1)
                    {
                        //pictureBox2.Image = imgThumbNail;
                        //pictureBox2.Tag = imageFileName;
                    }
                    if (j == 2)
                    {
                        //pictureBox3.Image = imgThumbNail;
                        //pictureBox3.Tag = imageFileName;
                    }
                    if (j == 3)
                    {
                        //pictureBox4.Image = imgThumbNail;
                        //pictureBox4.Tag = imageFileName;
                    }
                    if (j == 4)
                    {
                        //pictureBox5.Image = imgThumbNail;
                        //pictureBox5.Tag = imageFileName;
                    }
                    if (j == 5)
                    {
                        //pictureBox6.Image = imgThumbNail;
                        //pictureBox6.Tag = imageFileName;
                    }
                    if (imgNew != null)
                    {
                        imgNew.Dispose();
                        imgNew = null;
                        if (File.Exists("tmp.tif"))
                            File.Delete("tmp.TIF");
                    }
                }
            }
            else
            {
                ClearPicBox();
                imageName = null;
            }

        }
        void ClearPicBox()
        {
            //pictureBox1.Image = null;
            //pictureBox2.Image = null;
            //pictureBox3.Image = null;
            //pictureBox4.Image = null;
            //pictureBox5.Image = null;
            //pictureBox6.Image = null;
        }
        private int GetDocTypePos()
        {
            string currntDoc;
            int index = 0;
            string srchStr;
            for (int i = 0; i < lvwDockTypes.Items.Count; i++)
            {
                if (lvwDockTypes.Items[i].Selected == true)
                {
                    currntDoc = lvwDockTypes.Items[i].SubItems[0].Text;
                    for (int j = 0; j < lstImage.Items.Count; j++)
                    {
                        srchStr = lstImage.Items[j].ToString();
                        if (srchStr.IndexOf(currntDoc) > 0)
                        {
                            index = j;
                            break;
                        }
                    }
                    break;
                }
            }
            return index;
        }

        private void pictureBox1_DoubleClick(object sender, EventArgs e)
        {

            //Bitmap bmp;
            //picBig.Image = null;
            if (imageName != null)
            {
                if (imageName.Length >= 1)
                {
                    //ThumbnailChangeSize(pictureBox1.Tag.ToString());

                    int lstIndex;
                    lstIndex = (currntPg * 6) + 0 + GetDocTypePos();

                    if (lstIndex < lstImage.Items.Count)
                    {
                        lstImage.SelectedIndex = lstIndex;
                    }
                    tabControl2.SelectedIndex = 0;
                    lvwDockTypes.Focus();
                    //picBig.Visible = true;
                    //panelBig.Visible = true;
                }
            }
        }

        private void pictureBox2_DoubleClick(object sender, EventArgs e)
        {

            //Bitmap bmp;
            //picBig.Image = null;
            if (imageName != null)
            {
                if (imageName.Length >= 2)
                {

                    //ThumbnailChangeSize(pictureBox2.Tag.ToString());
                    int lstIndex;
                    lstIndex = (currntPg * 6) + 1 + GetDocTypePos();
                    if (lstIndex < lstImage.Items.Count)
                    {
                        lstImage.SelectedIndex = lstIndex;
                    }
                    tabControl2.SelectedIndex = 0;
                    lvwDockTypes.Focus();
                    //picBig.Visible = true;
                    //panelBig.Visible = true;
                }
            }
        }

        private void pictureBox3_DoubleClick(object sender, EventArgs e)
        {
            //picBig.Image = null;
            if (imageName != null)
            {
                if (imageName.Length >= 3)
                {

                    //ThumbnailChangeSize(pictureBox3.Tag.ToString());
                    int lstIndex;
                    lstIndex = (currntPg * 6) + 2 + GetDocTypePos();
                    if (lstIndex < lstImage.Items.Count)
                    {
                        lstImage.SelectedIndex = lstIndex;
                    }
                    tabControl2.SelectedIndex = 0;
                    lvwDockTypes.Focus();
                    //picBig.Visible = true;
                    //panelBig.Visible = true;
                }
            }
        }

        private void pictureBox4_DoubleClick(object sender, EventArgs e)
        {
            //Bitmap bmp;
            //picBig.Image = null;
            if (imageName != null)
            {
                if (imageName.Length >= 4)
                {

                    //ThumbnailChangeSize(pictureBox4.Tag.ToString());
                    int lstIndex;
                    lstIndex = (currntPg * 6) + 3 + GetDocTypePos();
                    if (lstIndex < lstImage.Items.Count)
                    {
                        lstImage.SelectedIndex = lstIndex;
                    }
                    tabControl2.SelectedIndex = 0;
                    lvwDockTypes.Focus();
                    //picBig.Visible = true;
                    //panelBig.Visible = true;
                }
            }
        }

        private void pictureBox5_DoubleClick(object sender, EventArgs e)
        {
            //Bitmap bmp;
            //picBig.Image = null;
            if (imageName != null)
            {
                if (imageName.Length >= 5)
                {

                    //ThumbnailChangeSize(pictureBox5.Tag.ToString());
                    int lstIndex;
                    lstIndex = (currntPg * 6) + 4 + GetDocTypePos();
                    if (lstIndex < lstImage.Items.Count)
                    {
                        lstImage.SelectedIndex = lstIndex;
                    }
                    tabControl2.SelectedIndex = 0;
                    lvwDockTypes.Focus();
                    //picBig.Visible = true;
                    //panelBig.Visible = true;
                }
            }
        }

        private void pictureBox6_DoubleClick(object sender, EventArgs e)
        {
            //Bitmap bmp;
            //picBig.Image = null;
            if (imageName != null)
            {
                if (imageName.Length >= 6)
                {

                    //ThumbnailChangeSize(pictureBox6.Tag.ToString());
                    int lstIndex;
                    lstIndex = (currntPg * 6) + 5 + GetDocTypePos();
                    if (lstIndex < lstImage.Items.Count)
                    {
                        lstImage.SelectedIndex = lstIndex;
                    }
                    tabControl2.SelectedIndex = 0;
                    lvwDockTypes.Focus();
                    //picBig.Visible = true;
                    //panelBig.Visible = true;
                }
            }
        }

        private void aeLicQa_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                //if (picBig.Visible == true)
                //{
                //    picBig.Visible = false;
                //    panelBig.Visible = false;
                //    picBig.Image = null;
                //}
            }
        }

        private void aeLicQa_Resize(object sender, EventArgs e)
        {
            if (this.WindowState != FormWindowState.Minimized)
            {
                //groupBox2.Height = (this.ClientSize.Height - groupBox1.Height) - 40;
                //groupBox2.Width = (this.ClientSize.Width - 10);
                tabControl2.Width = tabControl1.Width - (pictureBox.Width + 50);
                //tabControl2.Height = pictureBox.Height;
                //MessageBox.Show("Height - " + pictureBox1.Height + " Width - " + pictureBox1.Width);
                //panel3.Dock = DockStyle.None;
                //panel3.Width = tabControl2.Width;
                //panel3.Height = tabControl2.Height;
                //picControl.Height = tabControl2.Height - 100;
                //picControl.Width = tabControl2.Width - 80;
                //MessageBox.Show("Height - " + picControl.Height + " Width - " + picControl.Width);
                //MessageBox.Show("Height - " + tabControl2.Height + " Width - " + tabControl2.Width);
            }
        }

        private void tabControl1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                //if (picBig.Visible == true)
                //{
                //    picBig.Visible = false;
                //    panelBig.Visible = false;
                //    picBig.Image = null;
                //}
            }
            if (e.KeyCode == Keys.Subtract)
            {
                ZoomOut();
            }
            if (e.KeyCode == Keys.Add)
            {
                ZoomIn();
            }
        }

        private void chkMissingImg_CheckedChanged(object sender, EventArgs e)
        {
            int tifPos;
            string origDoctype = string.Empty;
            if (lstImage.SelectedIndex >= 0)
            {
                tifPos = lstImage.SelectedItem.ToString().IndexOf("-") + 1;
                string imgNumber;
                imgNumber = lstImage.SelectedItem.ToString().Substring((lstImage.SelectedItem.ToString().IndexOf("_") + 1), 5);
                if (tifPos > 0)
                {
                    origDoctype = lstImage.SelectedItem.ToString().Substring(tifPos);
                }
                if (chkMissingImg.Checked)
                {
                    txtComments.Text = txtComments.Text + imgNumber + "-" + origDoctype + " Missing image \r\n";
                    txtComments.SelectionStart = txtComments.Text.Length;
                    txtComments.ScrollToCaret();
                    txtComments.Refresh();
                }
                else
                {
                    string strToReplace;
                    strToReplace = imgNumber + "-" + origDoctype + " Missing image \r\n";
                    txtComments.Text = txtComments.Text.Replace(strToReplace, "");
                }
            }
        }

        private void chkExtraPage_CheckedChanged(object sender, EventArgs e)
        {
            int tifPos;
            string origDoctype = string.Empty;
            if (lstImage.SelectedIndex >= 0)
            {
                tifPos = lstImage.SelectedItem.ToString().IndexOf("-") + 1;
                string imgNumber;
                imgNumber = lstImage.SelectedItem.ToString().Substring((lstImage.SelectedItem.ToString().IndexOf("_") + 1), 5);
                if (tifPos > 0)
                {
                    origDoctype = lstImage.SelectedItem.ToString().Substring(tifPos);
                }
                if (chkExtraPage.Checked)
                {
                    txtComments.Text = txtComments.Text + imgNumber + "-" + origDoctype + " Extra page \r\n";
                    txtComments.SelectionStart = txtComments.Text.Length;
                    txtComments.ScrollToCaret();
                    txtComments.Refresh();
                }
                else
                {
                    string strToReplace;
                    strToReplace = imgNumber + "-" + origDoctype + " Extra page \r\n";
                    txtComments.Text = txtComments.Text.Replace(strToReplace, "");
                }
            }
        }

        private void chkDesicion_CheckedChanged(object sender, EventArgs e)
        {
            int tifPos;
            string origDoctype = string.Empty;
            string imgNumber;
            if (lstImage.SelectedIndex >= 0)
            {
                tifPos = lstImage.SelectedItem.ToString().IndexOf("-") + 1;
                imgNumber = lstImage.SelectedItem.ToString().Substring((lstImage.SelectedItem.ToString().IndexOf("_") + 1), 5);
                if (tifPos > 0)
                {
                    origDoctype = lstImage.SelectedItem.ToString().Substring(tifPos);
                }
                if (chkDesicion.Checked)
                {
                    txtComments.Text = txtComments.Text + imgNumber + "-" + origDoctype + " Desicion misd \r\n";
                    txtComments.SelectionStart = txtComments.Text.Length;
                    txtComments.ScrollToCaret();
                    txtComments.Refresh();
                }
                else
                {
                    string strToReplace;
                    strToReplace = imgNumber + "-" + origDoctype + " Desicion misd \r\n";
                    txtComments.Text = txtComments.Text.Replace(strToReplace, "");
                }
            }
        }

        private void chkIndexing_CheckedChanged(object sender, EventArgs e)
        {
            int tifPos;
            string origDoctype = string.Empty;
            if (lstImage.SelectedIndex >= 0)
            {
                tifPos = lstImage.SelectedItem.ToString().IndexOf("-") + 1;
                string imgNumber;
                imgNumber = lstImage.SelectedItem.ToString().Substring((lstImage.SelectedItem.ToString().IndexOf("_") + 1), 5);
                if (tifPos > 0)
                {
                    origDoctype = lstImage.SelectedItem.ToString().Substring(tifPos);
                }
                if (chkIndexing.Checked)
                {
                    txtComments.Text = txtComments.Text + imgNumber + "-" + origDoctype + " Wrong indexing \r\n";
                    txtComments.SelectionStart = txtComments.Text.Length;
                    txtComments.ScrollToCaret();
                    txtComments.Refresh();
                }
                else
                {
                    string strToReplace;
                    strToReplace = imgNumber + "-" + origDoctype + " Wrong indexing \r\n";
                    txtComments.Text = txtComments.Text.Replace(strToReplace, "");
                }
            }
        }

        private void txtComments_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {

            }
        }

        private void chkMove_CheckedChanged(object sender, EventArgs e)
        {
            int tifPos;
            string origDoctype = string.Empty;
            if (lstImage.SelectedIndex >= 0)
            {
                tifPos = lstImage.SelectedItem.ToString().IndexOf("-") + 1;
                string imgNumber;
                imgNumber = lstImage.SelectedItem.ToString().Substring((lstImage.SelectedItem.ToString().IndexOf("_") + 1), 5);
                if (tifPos > 0)
                {
                    origDoctype = lstImage.SelectedItem.ToString().Substring(tifPos);
                }
                if (chkMove.Checked)
                {
                    txtComments.Text = txtComments.Text + imgNumber + "-" + origDoctype + " Move to respective patient \r\n";
                    txtComments.SelectionStart = txtComments.Text.Length;
                    txtComments.ScrollToCaret();
                    txtComments.Refresh();
                }
                else
                {
                    string strToReplace;
                    strToReplace = imgNumber + "-" + origDoctype + " Move to respective patient \r\n";
                    txtComments.Text = txtComments.Text.Replace(strToReplace, "");
                }
            }
        }

        private void chkRearrange_CheckedChanged(object sender, EventArgs e)
        {
            int tifPos;
            string origDoctype = string.Empty;
            if (lstImage.SelectedIndex >= 0)
            {
                tifPos = lstImage.SelectedItem.ToString().IndexOf("-") + 1;
                string imgNumber;
                imgNumber = lstImage.SelectedItem.ToString().Substring((lstImage.SelectedItem.ToString().IndexOf("_") + 1), 5);
                if (tifPos > 0)
                {
                    origDoctype = lstImage.SelectedItem.ToString().Substring(tifPos);
                }
                if (chkRearrange.Checked)
                {
                    txtComments.Text = txtComments.Text + imgNumber + "-" + origDoctype + " Rearrange error \r\n";
                    txtComments.SelectionStart = txtComments.Text.Length;
                    txtComments.ScrollToCaret();
                    txtComments.Refresh();
                }
                else
                {
                    string strToReplace;
                    strToReplace = imgNumber + "-" + origDoctype + " Rearrange error \r\n";
                    txtComments.Text = txtComments.Text.Replace(strToReplace, "");
                }
            }
        }

        private void chkLinkedPolicy_CheckedChanged(object sender, EventArgs e)
        {
            int tifPos;
            string origDoctype = string.Empty;

            if (lstImage.SelectedIndex >= 0)
            {
                tifPos = lstImage.SelectedItem.ToString().IndexOf("-") + 1;
                string imgNumber;
                imgNumber = lstImage.SelectedItem.ToString().Substring((lstImage.SelectedItem.ToString().IndexOf("_") + 1), 5);
                if (tifPos > 0)
                {
                    origDoctype = lstImage.SelectedItem.ToString().Substring(tifPos);
                }
                if (chkLinkedPolicy.Checked)
                {
                    txtComments.Text = txtComments.Text + imgNumber + "-" + origDoctype + " Linked patient problem \r\n";
                    txtComments.SelectionStart = txtComments.Text.Length;
                    txtComments.ScrollToCaret();
                    txtComments.Refresh();
                }
                else
                {
                    string strToReplace;
                    strToReplace = imgNumber + "-" + origDoctype + " Linked patient problem \r\n";
                    txtComments.Text = txtComments.Text.Replace(strToReplace, "");
                }
            }
        }

        private void chkCropClean_CheckedChanged(object sender, EventArgs e)
        {
            int tifPos;
            string origDoctype = string.Empty;
            string imgNumber;
            if (lstImage.SelectedIndex >= 0)
            {
                tifPos = lstImage.SelectedItem.ToString().IndexOf("-") + 1;
                imgNumber = lstImage.SelectedItem.ToString().Substring((lstImage.SelectedItem.ToString().IndexOf("_") + 1), 5);
                if (tifPos > 0)
                {
                    origDoctype = lstImage.SelectedItem.ToString().Substring(tifPos);
                }
                if (chkCropClean.Checked)
                {
                    txtComments.Text = txtComments.Text + imgNumber + "-" + origDoctype + " Crop clean problem \r\n";
                    txtComments.SelectionStart = txtComments.Text.Length;
                    txtComments.ScrollToCaret();
                    txtComments.Refresh();
                }
                else
                {
                    string strToReplace;
                    strToReplace = imgNumber + "-" + origDoctype + " Crop clean problem \r\n";
                    txtComments.Text = txtComments.Text.Replace(strToReplace, "");
                }
            }
        }

        private void chkPoorScan_CheckedChanged(object sender, EventArgs e)
        {
            int tifPos;
            string origDoctype = string.Empty;
            string imgNumber;
            if (lstImage.SelectedIndex >= 0)
            {
                tifPos = lstImage.SelectedItem.ToString().IndexOf("-") + 1;
                imgNumber = lstImage.SelectedItem.ToString().Substring((lstImage.SelectedItem.ToString().IndexOf("_") + 1), 5);
                if (tifPos > 0)
                {
                    origDoctype = lstImage.SelectedItem.ToString().Substring(tifPos);
                }
                if (chkPoorScan.Checked)
                {
                    txtComments.Text = txtComments.Text + imgNumber + "-" + origDoctype + " Poor scan quality \r\n";
                    txtComments.SelectionStart = txtComments.Text.Length;
                    txtComments.ScrollToCaret();
                    txtComments.Refresh();
                }
                else
                {
                    string strToReplace;
                    strToReplace = imgNumber + "-" + origDoctype + " Poor scan quality \r\n";
                    txtComments.Text = txtComments.Text.Replace(strToReplace, "");
                }
            }
        }

        private void chkOther_CheckedChanged(object sender, EventArgs e)
        {
            int tifPos;
            string origDoctype = string.Empty;
            string imgNumber;
            if (lstImage.SelectedIndex >= 0)
            {
                tifPos = lstImage.SelectedItem.ToString().IndexOf("-") + 1;
                imgNumber = lstImage.SelectedItem.ToString().Substring((lstImage.SelectedItem.ToString().IndexOf("_") + 1), 5);
                if (tifPos > 0)
                {
                    origDoctype = lstImage.SelectedItem.ToString().Substring(tifPos);
                }
                if (chkOther.Checked)
                {
                    txtComments.Text = txtComments.Text + imgNumber + "-" + origDoctype + " Other \r\n";
                    txtComments.SelectionStart = txtComments.Text.Length;
                    txtComments.ScrollToCaret();
                    txtComments.Refresh();
                }
                else
                {
                    string strToReplace;
                    strToReplace = imgNumber + "-" + origDoctype + " Other \r\n";
                    txtComments.Text = txtComments.Text.Replace(strToReplace, "");
                }
            }
        }
        public bool PolicyWithLICException(int prmProjKey, int prmBatchKey)
        {
            string sqlStr = null;
            DataSet projDs = new DataSet();

            try
            {
                sqlStr = @"select patient_id from metadata_entry where proj_code=" + prmProjKey + " and bundle_key=" + prmBatchKey + " and status=" + (int)eSTATES.POLICY_EXCEPTION;
                sqlAdap = new OdbcDataAdapter(sqlStr, sqlCon);
                sqlAdap.Fill(projDs);
            }
            catch (Exception ex)
            {
                sqlAdap.Dispose();
                //err = ex.Message;
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes(sqlStr + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex);
            }
            if (projDs.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            else
                return false;
        }
        private void CheckBatchRejection(string pBatchKey)
        {
            wfeBatch wBatch = new wfeBatch(sqlCon);
            wQ = new ihwQuery(sqlCon);
            if (chkReadyUat.Checked == false)
            {
                if (wQ.GetSysConfigValue(ihConstants.BATCH_REJECTION_KEY) != ihConstants.BATCH_REJECTION_VALUE)
                {
                    if (PolicyWithLICException(Convert.ToInt32(dsimage11.Tables[0].Rows[0][0].ToString()), Convert.ToInt32(dsimage11.Tables[0].Rows[0][1].ToString())) == true)
                    {
                        chkRejectBatch.Visible = true;
                    }
                    else
                    {
                        chkRejectBatch.Visible = false;
                    }
                }
            }
            else
            {
                chkRejectBatch.Visible = false;
            }
        }
        void GrdPolicyCellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            //if((grdPolicy.Rows.Count > 0) && (e.RowIndex != null))
            //{
            //    if(Convert.ToInt32(grdPolicy.Rows[e.RowIndex].Cells[8].Value.ToString()) == (int) eSTATES.POLICY_CHECKED)
            //    {
            //        e.CellStyle.ForeColor = Color.Green; 
            //    }
            //    if((Convert.ToInt32(grdPolicy.Rows[e.RowIndex].Cells[8].Value.ToString()) == (int) eSTATES.POLICY_EXCEPTION) || (Convert.ToInt32(grdPolicy.Rows[e.RowIndex].Cells[8].Value.ToString()) == (int) eSTATES.POLICY_EXCEPTION))
            //    {
            //        e.CellStyle.ForeColor = Color.Red; 
            //    }

            //}
        }

        public bool UpdateStatus(int project,int bundle,int item,string pID, eSTATES state, Credentials prmCrd)
        {
            string sqlStr = null;
            OdbcTransaction sqlTrans = null;
            bool commitBol = true;
            OdbcCommand sqlCmd = new OdbcCommand();

            sqlStr = @"update metadata_entry" +
                " set status=" + (int)state + ",modified_by='" + prmCrd.created_by + "',modified_dttm='" + prmCrd.created_dttm + "' where proj_code=" + project +
                " and bundle_key=" + bundle + " " +
                " and patient_id='" + pID + "' and status<>" + 8;

            try
            {

                sqlTrans = sqlCon.BeginTransaction();
                sqlCmd.Connection = sqlCon;
                sqlCmd.Transaction = sqlTrans;
                sqlCmd.CommandText = sqlStr;
                int i = sqlCmd.ExecuteNonQuery();
                sqlTrans.Commit();
                if (i > 0)
                {
                    commitBol = true;
                }
                else
                {
                    commitBol = false;
                }
            }
            catch (Exception ex)
            {
                commitBol = false;
                sqlTrans.Rollback();
                sqlCmd.Dispose();
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes(sqlStr + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex);
            }
            return commitBol;
        }

        private void cmdAccepted_Click(object sender, EventArgs e)
        {
            string pageName;
            try
            {
                if (crd.role == ihConstants._LIC_ROLE)
                {
                    if (chkReadyUat.Checked == false)
                    {
                        if (lstImage.Items.Count > 0)
                        {
                            pPolicy = new CtrlPolicy(Convert.ToInt32(dsimage11.Tables[0].Rows[0][0].ToString()), Convert.ToInt32(dsimage11.Tables[0].Rows[0][1].ToString()), "1", dsimage11.Tables[0].Rows[0][7].ToString());
                            wfePolicy wPolicy = new wfePolicy(sqlCon, pPolicy);
                            UpdateStatus(Convert.ToInt32(dsimage11.Tables[0].Rows[0][0].ToString()), Convert.ToInt32(dsimage11.Tables[0].Rows[0][1].ToString()), Convert.ToInt32(dsimage11.Tables[0].Rows[0][2].ToString()), dsimage11.Tables[0].Rows[0][7].ToString(), eSTATES.POLICY_CHECKED, crd);

                            //for (int i = 0; i < lstImage.Items.Count; i++)
                            //{
                            //    pageName = lstImage.Items[i].ToString().Substring(0, lstImage.Items[i].ToString().IndexOf("-"));
                            //    pImage = new CtrlImage(Convert.ToInt32(cmbProject.SelectedValue.ToString()), Convert.ToInt32(cmbBatch.SelectedValue.ToString()), boxNo.ToString(), Convert.ToInt32(policyNumber), pageName, string.Empty);
                            //    wfeImage wImage = new wfeImage(sqlCon, pImage);
                            //    wImage.UpdateStatus(eSTATES.PAGE_CHECKED, crd);
                            //}
                            CtrlImage exppImage = new CtrlImage(Convert.ToInt32(dsimage11.Tables[0].Rows[0][0].ToString()), Convert.ToInt32(dsimage11.Tables[0].Rows[0][1].ToString()), "1", dsimage11.Tables[0].Rows[0][7].ToString(), string.Empty, string.Empty);
                            wfeImage expwImage = new wfeImage(sqlCon, exppImage);
                            expwImage.UpdateAllImageStatus(eSTATES.PAGE_CHECKED, crd);

                            wPolicy.QaExceptionStatus(ihConstants._POLICY_EXCEPTION_SOLVED, ihConstants._LIC_QA_POLICY_CHECKED);
                            grdBox.Rows[selectedIndex].DefaultCellStyle.BackColor = Color.Green;
                            //if ((wPolicy.GetPolicyStatus() == (int)eSTATES.POLICY_INDEXED) || (wPolicy.GetPolicyStatus() == (int)eSTATES.POLICY_FQC) || (wPolicy.GetPolicyStatus() == (int)eSTATES.POLICY_NOT_INDEXED))
                            //{
                            //    grdPolicy.Rows[policyRowIndex].Cells[9].Value = "Indexed";
                            //}
                            //if ((wPolicy.GetPolicyStatus() == (int)eSTATES.POLICY_ON_HOLD))
                            //{
                            //    grdPolicy.Rows[policyRowIndex].Cells[9].Value = "On hold";
                            //}
                            //if (wPolicy.GetPolicyStatus() == (int)eSTATES.POLICY_MISSING)
                            //{
                            //    grdPolicy.Rows[policyRowIndex].Cells[9].Value = "Missing";
                            //}
                            //if (wPolicy.GetPolicyStatus() == (int)eSTATES.POLICY_EXCEPTION)
                            //{
                            //    grdPolicy.Rows[policyRowIndex].Cells[9].Value = "In exception";
                            //}
                            //if (wPolicy.GetPolicyStatus() == (int)eSTATES.POLICY_CHECKED)
                            //{
                            //    grdPolicy.Rows[policyRowIndex].Cells[9].Value = "Checked";
                            //}
                            tabControl1.SelectedIndex = 0;
                            //tabControl2.SelectedIndex = 0;
                            CheckBatchRejection(dsimage11.Tables[0].Rows[0][1].ToString());
                        }
                    }
                    else
                    {
                        MessageBox.Show("This batch is already marked as ready for UAT.....");
                    }
                }
                else
                {
                    MessageBox.Show("You are not authorized to do this.....");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cmdRejected_Click(object sender, EventArgs e)
        {
            bool expBol = false;
            policyException udtExp = new policyException();
            NovaNet.Utils.dbCon dbcon = new NovaNet.Utils.dbCon();
            string pageName = null;
            if (crd.role == ihConstants._LIC_ROLE)
            {
                if (chkReadyUat.Checked == false)
                {
                    if (lstImage.Items.Count > 0)
                    {
                        //                        pPolicy = new CtrlPolicy(Convert.ToInt32(cmbProject.SelectedValue.ToString()), Convert.ToInt32(cmbBatch.SelectedValue.ToString()), boxNo.ToString(), policyNumber.ToString());
                        pPolicy = new CtrlPolicy(Convert.ToInt32(dsimage11.Tables[0].Rows[0][0].ToString()), Convert.ToInt32(dsimage11.Tables[0].Rows[0][1].ToString()), "1", dsimage11.Tables[0].Rows[0][7].ToString());
                        wfePolicy policy = new wfePolicy(sqlCon, pPolicy);
                        if (chkCropClean.Checked == true)
                        {
                            udtExp.crop_clean_exp = 1;
                            expBol = true;
                        }
                        else
                        {
                            udtExp.crop_clean_exp = 0;
                        }

                        if (chkSpelmis.Checked == true)
                        {
                            udtExp.decision_misd_exp = 1;
                            expBol = true;
                        }
                        else
                        {
                            udtExp.decision_misd_exp = 0;
                        }

                        if (chkExtraPage.Checked == true)
                        {
                            udtExp.extra_page_exp = 1;
                            expBol = true;
                        }
                        else
                        {
                            udtExp.extra_page_exp = 0;
                        }

                        if (chkWrongProperty.Checked == true)
                        {
                            udtExp.linked_policy_exp = 1;
                            expBol = true;
                        }
                        else
                        {
                            udtExp.linked_policy_exp = 0;
                        }

                        if (chkMissingImg.Checked == true)
                        {
                            udtExp.missing_img_exp = 1;
                            expBol = true;
                        }
                        else
                        {
                            udtExp.missing_img_exp = 0;
                        }
                        if (chkMove.Checked == true)
                        {
                            udtExp.move_to_respective_policy_exp = 1;
                            expBol = true;
                        }
                        else
                        {
                            udtExp.move_to_respective_policy_exp = 0;
                        }
                        if (chkOther.Checked == true)
                        {
                            udtExp.other_exp = 1;
                            expBol = true;
                        }
                        else
                        {
                            udtExp.other_exp = 0;
                        }

                        if (chkPoorScan.Checked == true)
                        {
                            udtExp.poor_scan_exp = 1;
                            expBol = true;
                        }
                        else
                        {
                            udtExp.poor_scan_exp = 0;
                        }
                        if (chkRearrange.Checked == true)
                        {
                            udtExp.rearrange_exp = 1;
                            expBol = true;
                        }
                        else
                        {
                            udtExp.rearrange_exp = 0;
                        }
                        if (chkIndexing.Checked == true)
                        {
                            udtExp.wrong_indexing_exp = 1;
                            expBol = true;
                        }
                        else
                        {
                            udtExp.wrong_indexing_exp = 0;
                        }
                        udtExp.comments = txtComments.Text;
                        //udtExp.status = ihConstants._LIC_QA_POLICY_EXCEPTION;
                        if (expBol == true)
                        {
                            udtExp.solved = ihConstants._POLICY_EXCEPTION_NOT_SOLVED;
                            if (policy.UpdateQaPolicyException(crd, udtExp) == true)
                            {
                                if (policy.QaExceptionStatus(ihConstants._POLICY_EXCEPTION_NOT_SOLVED, ihConstants._LIC_QA_POLICY_EXCEPTION) == true)
                                {
                                    UpdateStatus(Convert.ToInt32(dsimage11.Tables[0].Rows[0][0].ToString()), Convert.ToInt32(dsimage11.Tables[0].Rows[0][1].ToString()), Convert.ToInt32(dsimage11.Tables[0].Rows[0][2].ToString()), dsimage11.Tables[0].Rows[0][7].ToString(), eSTATES.POLICY_EXCEPTION, crd);
                                    
                                    CtrlImage exppImage = new CtrlImage(Convert.ToInt32(dsimage11.Tables[0].Rows[0][0].ToString()), Convert.ToInt32(dsimage11.Tables[0].Rows[0][1].ToString()), "1", dsimage11.Tables[0].Rows[0][7].ToString(), string.Empty, string.Empty);
                                    wfeImage expwImage = new wfeImage(sqlCon, exppImage);
                                    expwImage.UpdateAllImageStatus(eSTATES.PAGE_EXCEPTION, crd);
                                    grdBox.Rows[selectedIndex].DefaultCellStyle.BackColor = Color.Red;
                                }
                            }
                            tabControl1.SelectedIndex = 0;
                            CheckBatchRejection(dsimage11.Tables[0].Rows[0][1].ToString());
                        }
                        else
                        {
                            MessageBox.Show("Provide atleast one exception type", "NRS Record Management", MessageBoxButtons.OK);
                        }

                    }
                }
                else
                {
                    MessageBox.Show("This batch is already marked as ready for UAT.....");
                }
            }
            else
            {
                MessageBox.Show("You are not authorized to do this.....");
            }
        }
        private void GetAuditException()
        {
            try
            {
                ClearPicBox();

                //firstDoc = true;
                DataSet expDs = new DataSet();
                //////clickedIndexValue = e.RowIndex;
                //picControl.Image = null;
                //lstImage.Items.Clear();
                //DisplayDockType();
                //policyNumber = grdPolicy.Rows[e.RowIndex].Cells[1].Value.ToString();
                //policyLen = policyNumber.Length;
                //txtPolicyNumber.Text = policyNumber;
                //txtName.Text = grdPolicy.Rows[e.RowIndex].Cells[2].Value.ToString();
                //txtDOB.Text = grdPolicy.Rows[e.RowIndex].Cells[4].Value.ToString();
                //txtCommDt.Text = grdPolicy.Rows[e.RowIndex].Cells[3].Value.ToString();
                //policyRowIndex = e.RowIndex;
                //if (Convert.ToDouble(grdPolicy.Rows[e.RowIndex].Cells[5].Value.ToString()) > 0)
                //{
                //for (int i = 0; i < grdPolicy.Columns.Count - 10; i++)
                //{
                //    lvwDockTypes.Items[i].SubItems[1].Text = grdPolicy.Rows[e.RowIndex].Cells[i + 10].Value.ToString();
                //}
                //lblTotFiles.Text = Convert.ToString(Math.Round(Convert.ToDouble(grdPolicy.Rows[e.RowIndex].Cells[5].Value.ToString()), 2));
                //lblAvgSize.Text = Convert.ToString(Math.Round(Convert.ToDouble(grdPolicy.Rows[e.RowIndex].Cells[7].Value.ToString()), 2)) + " KB";
                //lblDock.Text = Convert.ToString(Math.Round(Convert.ToDouble(grdPolicy.Rows[e.RowIndex].Cells[6].Value.ToString()), 2)) + " KB";
                //policyStatus = Convert.ToInt32(grdPolicy.Rows[e.RowIndex].Cells[8].Value.ToString());
                //if (policyStatus == (int)eSTATES.POLICY_EXPORTED)
                //{
                //    cmdAccepted.Enabled = false;
                //    cmdRejected.Enabled = false;
                //}
                //else
                //{
                //    cmdAccepted.Enabled = true;
                //    cmdRejected.Enabled = true;
                //}
                //lstImage.Items.Clear();
                pPolicy = new CtrlPolicy(Convert.ToInt32(dsimage11.Tables[0].Rows[0][0].ToString()), Convert.ToInt32(dsimage11.Tables[0].Rows[0][1].ToString()), "1", policy_number);
                //pPolicy = new CtrlPolicy(Convert.ToInt32(cmbProject.SelectedValue.ToString()), Convert.ToInt32(cmbBatch.SelectedValue.ToString()), boxNo.ToString(), grdPolicy.Rows[e.RowIndex].Cells[1].Value.ToString());
                wfePolicy policy = new wfePolicy(sqlCon, pPolicy);
                //policyData = (udtPolicy)policy.LoadValuesFromDB();
                //policyPath = GetPolicyPath(Convert.ToInt32(policyNumber)); //policyData.policy_path;
                expDs = policy.GetAllException();
                if (expDs.Tables[0].Rows.Count > 0)
                {
                    if (Convert.ToInt32(expDs.Tables[0].Rows[0]["missing_img_exp"].ToString()) == 1)
                    {
                        chkMissingImg.Checked = true;
                    }
                    else
                    {
                        chkMissingImg.Checked = false;
                    }

                    if (Convert.ToInt32(expDs.Tables[0].Rows[0]["crop_clean_exp"].ToString()) == 1)
                    {
                        chkCropClean.Checked = true;
                    }
                    else
                    {
                        chkCropClean.Checked = false;
                    }

                    if (Convert.ToInt32(expDs.Tables[0].Rows[0]["poor_scan_exp"].ToString()) == 1)
                    {
                        chkPoorScan.Checked = true;
                    }
                    else
                    {
                        chkPoorScan.Checked = false;
                    }
                    if (Convert.ToInt32(expDs.Tables[0].Rows[0]["wrong_indexing_exp"].ToString()) == 1)
                    {
                        chkIndexing.Checked = true;
                    }
                    else
                    {
                        chkIndexing.Checked = false;
                    }
                    if (Convert.ToInt32(expDs.Tables[0].Rows[0]["linked_policy_exp"].ToString()) == 1)
                    {
                        chkWrongProperty.Checked = true;
                    }
                    else
                    {
                        chkWrongProperty.Checked = false;
                    }
                    if (Convert.ToInt32(expDs.Tables[0].Rows[0]["decision_misd_exp"].ToString()) == 1)
                    {
                        chkSpelmis.Checked = true;
                    }
                    else
                    {
                        chkSpelmis.Checked = false;
                    }
                    if (Convert.ToInt32(expDs.Tables[0].Rows[0]["extra_page_exp"].ToString()) == 1)
                    {
                        chkExtraPage.Checked = true;
                    }
                    else
                    {
                        chkExtraPage.Checked = false;
                    }
                    if (Convert.ToInt32(expDs.Tables[0].Rows[0]["decision_misd_exp"].ToString()) == 1)
                    {
                        chkDesicion.Checked = true;
                    }
                    else
                    {
                        chkDesicion.Checked = false;
                    }
                    if (Convert.ToInt32(expDs.Tables[0].Rows[0]["rearrange_exp"].ToString()) == 1)
                    {
                        chkRearrange.Checked = true;
                    }
                    else
                    {
                        chkRearrange.Checked = false;
                    }
                    if (Convert.ToInt32(expDs.Tables[0].Rows[0]["other_exp"].ToString()) == 1)
                    {
                        chkOther.Checked = true;
                    }
                    else
                    {
                        chkOther.Checked = false;
                    }
                    if (Convert.ToInt32(expDs.Tables[0].Rows[0]["move_to_respective_policy_exp"].ToString()) == 1)
                    {
                        chkMove.Checked = true;
                    }
                    else
                    {
                        chkMove.Checked = false;
                    }
                    txtComments.Text = expDs.Tables[0].Rows[0]["comments"].ToString() + "\r\n";
                    txtComments.SelectionStart = txtComments.Text.Length;
                    txtComments.ScrollToCaret();
                    txtComments.Refresh();
                }
                else
                {
                    chkMissingImg.Checked = false;
                    chkCropClean.Checked = false;
                    chkPoorScan.Checked = false;
                    chkIndexing.Checked = false;
                    chkLinkedPolicy.Checked = false;
                    chkDesicion.Checked = false;
                    chkExtraPage.Checked = false;
                    chkDesicion.Checked = false;
                    chkRearrange.Checked = false;
                    chkOther.Checked = false;
                    chkMove.Checked = false;
                    txtComments.Text = string.Empty;
                }

                ArrayList arrImage = new ArrayList();
                wQuery pQuery = new ihwQuery(sqlCon);
                eSTATES[] state = new eSTATES[5];
                state[0] = eSTATES.POLICY_CHECKED;
                state[1] = eSTATES.POLICY_FQC;
                state[2] = eSTATES.POLICY_INDEXED;
                state[3] = eSTATES.POLICY_EXCEPTION;
                state[4] = eSTATES.POLICY_EXPORTED;
                CtrlImage ctrlImage;
                arrImage = pQuery.GetItems(eITEMS.LIC_QA_PAGE, state, policy);
                for (int i = 0; i < arrImage.Count; i++)
                {
                    ctrlImage = (CtrlImage)arrImage[i];
                    if (ctrlImage.DocType != string.Empty)
                    {
                        lstImage.Items.Add(ctrlImage.ImageName + "-" + ctrlImage.DocType);
                    }
                    else
                        lstImage.Items.Add(ctrlImage.ImageName);
                }
                tabControl1.SelectedIndex = 1;
                if (lstImage.Items.Count > 0)
                {
                    lstImage.SelectedIndex = 0;
                    if (checkBox1.Checked == true)
                    {
                        cmdAccepted.Enabled = true;
                        cmdRejected.Enabled = true;
                    }
                    else
                    {
                        cmdAccepted.Enabled = true;
                        cmdRejected.Enabled = true;
                    }
                }

                //}
                //else
                //{
                //    cmdAccepted.Enabled = false;
                //    cmdRejected.Enabled = false;
                //}

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while getting the information of the selected policy.....");
                exMailLog.Log(ex);
            }
        }
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((grdBox.Rows.Count > 0) && (dgvname.Rows.Count > 0) || (grdBox.Rows.Count > 0) && (dgvProperty.Rows.Count > 0))
            {
                if (tabControl1.SelectedIndex == 0)
                {
                    if (grdBox.Rows.Count > 0)
                    {
                        if (selectedIndex < (grdBox.Rows.Count - 1))
                        {
                            if (grdBox.Rows[selectedIndex].Cells[1] != null)
                            {
                                if (grdBox.Rows[selectedIndex + 1].Displayed == false)
                                {
                                    grdBox.FirstDisplayedScrollingRowIndex = selectedIndex;
                                }
                                grdBox.Rows[selectedIndex + 1].Selected = true;
                                //grdBox.CurrentCell = grdBox.Rows[selectedIndex + 1].Cells[1];
                                //policyRowIndex = policyRowIndex + 1;
                            }
                        }
                    }
                }
                if (tabControl1.SelectedIndex == 1)
                {
                    GetAuditException();

                    if (lstImage.Items.Count > 0)
                    {
                        pPolicy = new CtrlPolicy(Convert.ToInt32(dsimage11.Tables[0].Rows[0][0].ToString()), Convert.ToInt32(dsimage11.Tables[0].Rows[0][1].ToString()), "1", policy_number);
                        wfePolicy policy = new wfePolicy(sqlCon, pPolicy);
                        if (policy.GetLicExpCount() == 0)
                        {
                            if (policy.InitiateQaPolicyException(crd) == true)
                            {
                                policy.QaExceptionStatus(ihConstants._POLICY_EXCEPTION_INITIALIZED, ihConstants._LIC_QA_POLICY_VIEWED);
                            }
                        }
                    }
                }
                CtrlBox pBox = new CtrlBox(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), "0");
                wfeBox wBox = new wfeBox(sqlCon, pBox);
                lblTotPol.Text = wBox.GetLICCheckedCount().ToString();
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox1_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(this, "Do You Want to Mark this Batch as Ready for UAT...?", "NRS Record Management...", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (crd.role == ihConstants._LIC_ROLE)
            {
                if (grdBox.Rows.Count > 0)
                {
                    if (result == DialogResult.Yes)
                    {
                        DataSet ds = new DataSet();

                        //if (cmbDistrict.SelectedValue.ToString() != null && cmbWhereReg.SelectedValue.ToString() != null && cmbBook.SelectedValue.ToString() != null && cmbyear.Text != "" && cmbVol.Text != "")
                        //{
                        if (checkBox1.Checked == true)
                        {
                            //wfePolicy wfe = new wfePolicy(sqlCon);
                            //ds = wfe.GetProject(cmbDistrict.SelectedValue.ToString(), cmbWhereReg.SelectedValue.ToString(), cmbBook.SelectedValue.ToString(), cmbyear.Text, cmbVol.Text);
                            //string proj_key = ds.Tables[0].Rows[0]["proj_key"].ToString();
                            //string batch_key = ds.Tables[0].Rows[0]["Batch_key"].ToString();

                            //groupBox5.Enabled = false;
                            for (int i = 0; i < grdBox.Rows.Count; i++)
                            {
                                string policyname = grdBox.Rows[i].Cells[3].Value.ToString() ;
                                if (deed_check(policyname) == false)
                                {
                                    MessageBox.Show(this, "There is Exception in Patient ID " + policyname, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    checkBox1.Checked = false;
                                    return;
                                }
                            }
                            wfePolicy wfe = new wfePolicy(sqlCon);
                            if (ReadyForUAT(cmbRunnum.SelectedValue.ToString()) == true)
                            {
                                if (saveUATinfo() == true)
                                {
                                    MessageBox.Show(this, "Volumes Successfully Marked For UAT...", "NRS Record Management...", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                            }
                            else
                            {
                                MessageBox.Show(this, "Error...", "NRS Record Management...", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        //}
                        //else
                        //{
                        //    MessageBox.Show(this, "Please select at least one Deed and try agin...", "IGR...", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        //}
                    }
                    else
                    {
                        checkBox1.Checked = false;
                    }
                }
            }
            else
            {
                MessageBox.Show("You are not authorized to do this.....");
                checkBox1.Checked = false;
            }
        }
        private bool saveUATinfo()
        {
            Boolean flag = false;
            wfePolicy wpol = new wfePolicy(sqlCon);
            if (SaveUatInfo(cmbRunnum.Text, txtPercent.Text.Trim(), crd.created_by, crd.created_dttm) == true)
            {
                flag = true;
            }

            return flag;
        }
        public bool SaveUatInfo(string runno, string percent, string created_by, string created_dttm)
        {
            OdbcCommand Cmd = new OdbcCommand();
            try
            {
                string sql = "insert into tbl_uat_info(percent_checked,run_no,cretaed_by,created_dttm) values ('" + percent + "','" + runno + "','" + created_by + "','" + created_dttm + "')";
                Cmd.Connection = sqlCon;
                Cmd.CommandText = sql;
                Cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        public bool ReadyForUAT(string runnum)
        {
            string sqlStr = null;
            OdbcTransaction sqlTrans = null;
            bool commitBol = true;
            OdbcCommand sqlCmd = new OdbcCommand();
            try
            {
                sqlStr = "update bundle_master set status = '7' where bundle_code = '" + runnum + "'";
                sqlTrans = sqlCon.BeginTransaction();
                sqlCmd.Connection = sqlCon;
                sqlCmd.Transaction = sqlTrans;
                sqlCmd.CommandText = sqlStr;
                int i = sqlCmd.ExecuteNonQuery();
                sqlTrans.Commit();
                commitBol = true;

            }
            catch (Exception ex)
            {
                commitBol = false;
                sqlTrans.Rollback();
                sqlCmd.Dispose();
            }
            return commitBol;
        }
        public int GetMetaStatus(string policyNo)
        {
            string sqlStr = null;
            DataSet dsImage = new DataSet();
            OdbcDataAdapter sqlAdap = null;

            sqlStr = "select status from metadata_entry " +
                    " where patient_id='" + policyNo + "'";

            try
            {
                sqlAdap = new OdbcDataAdapter(sqlStr, sqlCon);
                sqlAdap.Fill(dsImage);
            }
            catch (Exception ex)
            {
                sqlAdap.Dispose();
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes(sqlStr + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex);
            }
            return Convert.ToInt32(dsImage.Tables[0].Rows[0]["status"]);
        }
        private bool deed_check(string deedNo)
        {
            Boolean flag = false;

            //wfePolicy wfe = new wfePolicy(sqlCon);
            try
            {
                int status = GetMetaStatus(deedNo);
                if (status != 30)
                {
                    flag = true;
                }
                else
                {
                    flag = false;
                }
            }
            catch (Exception ex)
            {
                flag = false;
            }
            return flag;
        }
    }
}
