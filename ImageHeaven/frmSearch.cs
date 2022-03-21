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
    public partial class frmSearch : Form
    {
        MemoryStream stateLog;
        byte[] tmpWrite;
        OdbcConnection sqlCon = null;
        NovaNet.Utils.dbCon dbcon = null;
        CtrlPolicy pPolicy = null;
        private CtrlImage pImage = null;
        wfePolicy wPolicy = null;
        wfeImage wImage = null;
        public Credentials crd;

        public static string projKey;
        public static string bundleKey;

        public static string patientID;

        public frmSearch()
        {
            InitializeComponent();
        }

        public frmSearch(OdbcConnection prmCon, Credentials prmCrd)
        {
            this.Name = "NRS Search Control";
            InitializeComponent();
            sqlCon = prmCon;
           
        }

        public DataTable getSearchReasult()
        {
            DataTable dt = new DataTable();

            string sql = "select a.proj_code,a.bundle_key,a.item_no,a.patient_id,a.patient_name as 'Patient Name',a.gender as 'Gender',a.guardian_name as 'Guardian Name',a.address as 'Address',a.reg_number as 'Registration Number',a.dep_name as 'Department Name',date_format(a.admission_date,'%d-%M-%Y') as 'Date of Admission',a.death as 'Death Status',date_format(a.death_date,'%d-%M-%Y') as 'Date of Death',date_format(a.discharge_date,'%d-%M-%Y') as 'Date of Discharge' from metadata_entry a,bundle_master b where a.proj_code = b.proj_code and a.bundle_key = b.bundle_key and b.status = 8 ";
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(dt);

            return dt;
        }

        public DataTable getSearchReasultName(string name)
        {
            DataTable dt = new DataTable();

            string sql = "select a.proj_code,a.bundle_key,a.item_no,a.patient_id,a.patient_name as 'Patient Name',a.gender as 'Gender',a.guardian_name as 'Guardian Name',a.address as 'Address',a.reg_number as 'Registration Number',a.dep_name as 'Department Name',date_format(a.admission_date,'%d-%M-%Y') as 'Date of Admission',a.death as 'Death Status',date_format(a.death_date,'%d-%M-%Y') as 'Date of Death',date_format(a.discharge_date,'%d-%M-%Y') as 'Date of Discharge' from metadata_entry a,bundle_master b where a.proj_code = b.proj_code and a.bundle_key = b.bundle_key and b.status = 8 and a.patient_name = '"+name+"'";
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(dt);

            return dt;
        }

        public DataTable getSearchReasultGName(string gname)
        {
            DataTable dt = new DataTable();

            string sql = "select a.proj_code,a.bundle_key,a.item_no,a.patient_id,a.patient_name as 'Patient Name',a.gender as 'Gender',a.guardian_name as 'Guardian Name',a.address as 'Address',a.reg_number as 'Registration Number',a.dep_name as 'Department Name',date_format(a.admission_date,'%d-%M-%Y') as 'Date of Admission',a.death as 'Death Status',date_format(a.death_date,'%d-%M-%Y') as 'Date of Death',date_format(a.discharge_date,'%d-%M-%Y') as 'Date of Discharge' from metadata_entry a,bundle_master b where a.proj_code = b.proj_code and a.bundle_key = b.bundle_key and b.status = 8 and a.guardian_name = '" + gname + "'";
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(dt);

            return dt;
        }

        public DataTable getSearchReasultAddress(string address)
        {
            DataTable dt = new DataTable();

            string sql = "select a.proj_code,a.bundle_key,a.item_no,a.patient_id,a.patient_name as 'Patient Name',a.gender as 'Gender',a.guardian_name as 'Guardian Name',a.address as 'Address',a.reg_number as 'Registration Number',a.dep_name as 'Department Name',date_format(a.admission_date,'%d-%M-%Y') as 'Date of Admission',a.death as 'Death Status',date_format(a.death_date,'%d-%M-%Y') as 'Date of Death',date_format(a.discharge_date,'%d-%M-%Y') as 'Date of Discharge' from metadata_entry a,bundle_master b where a.proj_code = b.proj_code and a.bundle_key = b.bundle_key and b.status = 8 and a.address = '" + address + "'";
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(dt);

            return dt;
        }

        public DataTable getSearchReasultAdS(string adstart)
        {
            DataTable dt = new DataTable();

            string sql = "select a.proj_code,a.bundle_key,a.item_no,a.patient_id,a.patient_name as 'Patient Name',a.gender as 'Gender',a.guardian_name as 'Guardian Name',a.address as 'Address',a.reg_number as 'Registration Number',a.dep_name as 'Department Name',date_format(a.admission_date,'%d-%M-%Y') as 'Date of Admission',a.death as 'Death Status',date_format(a.death_date,'%d-%M-%Y') as 'Date of Death',date_format(a.discharge_date,'%d-%M-%Y') as 'Date of Discharge' from metadata_entry a,bundle_master b where a.proj_code = b.proj_code and a.bundle_key = b.bundle_key and b.status = 8 and a.admission_date = '" + adstart + "'";
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(dt);

            return dt;
        }

        public DataTable getSearchReasultDisS(string disstart)
        {
            DataTable dt = new DataTable();

            string sql = "select a.proj_code,a.bundle_key,a.item_no,a.patient_id,a.patient_name as 'Patient Name',a.gender as 'Gender',a.guardian_name as 'Guardian Name',a.address as 'Address',a.reg_number as 'Registration Number',a.dep_name as 'Department Name',date_format(a.admission_date,'%d-%M-%Y') as 'Date of Admission',a.death as 'Death Status',date_format(a.death_date,'%d-%M-%Y') as 'Date of Death',date_format(a.discharge_date,'%d-%M-%Y') as 'Date of Discharge' from metadata_entry a,bundle_master b where a.proj_code = b.proj_code and a.bundle_key = b.bundle_key and b.status = 8 and a.discharge_date = '" + disstart + "'";
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(dt);

            return dt;
        }

        public DataTable getSearchReasultDeathS(string deathstart)
        {
            DataTable dt = new DataTable();

            string sql = "select a.proj_code,a.bundle_key,a.item_no,a.patient_id,a.patient_name as 'Patient Name',a.gender as 'Gender',a.guardian_name as 'Guardian Name',a.address as 'Address',a.reg_number as 'Registration Number',a.dep_name as 'Department Name',date_format(a.admission_date,'%d-%M-%Y') as 'Date of Admission',a.death as 'Death Status',date_format(a.death_date,'%d-%M-%Y') as 'Date of Death',date_format(a.discharge_date,'%d-%M-%Y') as 'Date of Discharge' from metadata_entry a,bundle_master b where a.proj_code = b.proj_code and a.bundle_key = b.bundle_key and b.status = 8 and a.death_date = '" + deathstart + "'";
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(dt);

            return dt;
        }

        public DataTable getSearchReasultNG(string name, string gnamne)
        {
            DataTable dt = new DataTable();

            string sql = "select a.proj_code,a.bundle_key,a.item_no,a.patient_id,a.patient_name as 'Patient Name',a.gender as 'Gender',a.guardian_name as 'Guardian Name',a.address as 'Address',a.reg_number as 'Registration Number',a.dep_name as 'Department Name',date_format(a.admission_date,'%d-%M-%Y') as 'Date of Admission',a.death as 'Death Status',date_format(a.death_date,'%d-%M-%Y') as 'Date of Death',date_format(a.discharge_date,'%d-%M-%Y') as 'Date of Discharge' from metadata_entry a,bundle_master b where a.proj_code = b.proj_code and a.bundle_key = b.bundle_key and b.status = 8 and a.patient_name = '" + name + "' and a.guardian_name = '" + gnamne + "'";
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(dt);

            return dt;
        }

        public DataTable getSearchReasultNA(string name, string address)
        {
            DataTable dt = new DataTable();

            string sql = "select a.proj_code,a.bundle_key,a.item_no,a.patient_id,a.patient_name as 'Patient Name',a.gender as 'Gender',a.guardian_name as 'Guardian Name',a.address as 'Address',a.reg_number as 'Registration Number',a.dep_name as 'Department Name',date_format(a.admission_date,'%d-%M-%Y') as 'Date of Admission',a.death as 'Death Status',date_format(a.death_date,'%d-%M-%Y') as 'Date of Death',date_format(a.discharge_date,'%d-%M-%Y') as 'Date of Discharge' from metadata_entry a,bundle_master b where a.proj_code = b.proj_code and a.bundle_key = b.bundle_key and b.status = 8 and a.patient_name = '" + name + "' and a.address = '" + address + "'";
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(dt);

            return dt;
        }

        public DataTable getSearchReasultGA(string gname, string address)
        {
            DataTable dt = new DataTable();

            string sql = "select a.proj_code,a.bundle_key,a.item_no,a.patient_id,a.patient_name as 'Patient Name',a.gender as 'Gender',a.guardian_name as 'Guardian Name',a.address as 'Address',a.reg_number as 'Registration Number',a.dep_name as 'Department Name',date_format(a.admission_date,'%d-%M-%Y') as 'Date of Admission',a.death as 'Death Status',date_format(a.death_date,'%d-%M-%Y') as 'Date of Death',date_format(a.discharge_date,'%d-%M-%Y') as 'Date of Discharge' from metadata_entry a,bundle_master b where a.proj_code = b.proj_code and a.bundle_key = b.bundle_key and b.status = 8 and a.guardian_name = '" + gname + "' and a.address = '" + address + "'";
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(dt);

            return dt;
        }

        public DataTable getSearchReasultNGA(string name, string gname, string address)
        {
            DataTable dt = new DataTable();

            string sql = "select a.proj_code,a.bundle_key,a.item_no,a.patient_id,a.patient_name as 'Patient Name',a.gender as 'Gender',a.guardian_name as 'Guardian Name',a.address as 'Address',a.reg_number as 'Registration Number',a.dep_name as 'Department Name',date_format(a.admission_date,'%d-%M-%Y') as 'Date of Admission',a.death as 'Death Status',date_format(a.death_date,'%d-%M-%Y') as 'Date of Death',date_format(a.discharge_date,'%d-%M-%Y') as 'Date of Discharge' from metadata_entry a,bundle_master b where a.proj_code = b.proj_code and a.bundle_key = b.bundle_key and b.status = 8 and a.guardian_name = '" + gname + "' and a.address = '" + address + "' and a.patient_name = '"+name+"'";
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(dt);

            return dt;
        }

        public DataTable getSearchReasultAdStEn(string adstart, string adend)
        {
            DataTable dt = new DataTable();

            string sql = "select a.proj_code,a.bundle_key,a.item_no,a.patient_id,a.patient_name as 'Patient Name',a.gender as 'Gender',a.guardian_name as 'Guardian Name',a.address as 'Address',a.reg_number as 'Registration Number',a.dep_name as 'Department Name',date_format(a.admission_date,'%d-%M-%Y') as 'Date of Admission',a.death as 'Death Status',date_format(a.death_date,'%d-%M-%Y') as 'Date of Death',date_format(a.discharge_date,'%d-%M-%Y') as 'Date of Discharge' from metadata_entry a,bundle_master b where a.proj_code = b.proj_code and a.bundle_key = b.bundle_key and b.status = 8 and a.admission_date between '" + adstart + "' and '" + adend + "'";
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(dt);

            return dt;
        }

        public DataTable getSearchReasultDisStEn(string disstart, string disend)
        {
            DataTable dt = new DataTable();

            string sql = "select a.proj_code,a.bundle_key,a.item_no,a.patient_id,a.patient_name as 'Patient Name',a.gender as 'Gender',a.guardian_name as 'Guardian Name',a.address as 'Address',a.reg_number as 'Registration Number',a.dep_name as 'Department Name',date_format(a.admission_date,'%d-%M-%Y') as 'Date of Admission',a.death as 'Death Status',date_format(a.death_date,'%d-%M-%Y') as 'Date of Death',date_format(a.discharge_date,'%d-%M-%Y') as 'Date of Discharge' from metadata_entry a,bundle_master b where a.proj_code = b.proj_code and a.bundle_key = b.bundle_key and b.status = 8 and a.discharge_date between '" + disstart + "' and '" + disend + "'";
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(dt);

            return dt;
        }

        public DataTable getSearchReasultDeathStEn(string deathstart, string deathend)
        {
            DataTable dt = new DataTable();

            string sql = "select a.proj_code,a.bundle_key,a.item_no,a.patient_id,a.patient_name as 'Patient Name',a.gender as 'Gender',a.guardian_name as 'Guardian Name',a.address as 'Address',a.reg_number as 'Registration Number',a.dep_name as 'Department Name',date_format(a.admission_date,'%d-%M-%Y') as 'Date of Admission',a.death as 'Death Status',date_format(a.death_date,'%d-%M-%Y') as 'Date of Death',date_format(a.discharge_date,'%d-%M-%Y') as 'Date of Discharge' from metadata_entry a,bundle_master b where a.proj_code = b.proj_code and a.bundle_key = b.bundle_key and b.status = 8 and a.death_date between '" + deathstart + "' and '" + deathend + "'";
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(dt);

            return dt;
        }

        public DataTable getSearchReasultAdDis_StEn(string adstart, string adend, string disstart, string disend)
        {
            DataTable dt = new DataTable();

            string sql = "select a.proj_code,a.bundle_key,a.item_no,a.patient_id,a.patient_name as 'Patient Name',a.gender as 'Gender',a.guardian_name as 'Guardian Name',a.address as 'Address',a.reg_number as 'Registration Number',a.dep_name as 'Department Name',date_format(a.admission_date,'%d-%M-%Y') as 'Date of Admission',a.death as 'Death Status',date_format(a.death_date,'%d-%M-%Y') as 'Date of Death',date_format(a.discharge_date,'%d-%M-%Y') as 'Date of Discharge' from metadata_entry a,bundle_master b where a.proj_code = b.proj_code and a.bundle_key = b.bundle_key and b.status = 8 and a.admission_date between '" + adstart + "' and '" + adend + "' and a.discharge_date between '"+disstart+"' and '"+disend+"'";
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(dt);

            return dt;
        }

        public DataTable getSearchReasultAdDeath_StEn(string adstart, string adend, string deathstart, string deathsend)
        {
            DataTable dt = new DataTable();

            string sql = "select a.proj_code,a.bundle_key,a.item_no,a.patient_id,a.patient_name as 'Patient Name',a.gender as 'Gender',a.guardian_name as 'Guardian Name',a.address as 'Address',a.reg_number as 'Registration Number',a.dep_name as 'Department Name',date_format(a.admission_date,'%d-%M-%Y') as 'Date of Admission',a.death as 'Death Status',date_format(a.death_date,'%d-%M-%Y') as 'Date of Death',date_format(a.discharge_date,'%d-%M-%Y') as 'Date of Discharge' from metadata_entry a,bundle_master b where a.proj_code = b.proj_code and a.bundle_key = b.bundle_key and b.status = 8 and a.admission_date between '" + adstart + "' and '" + adend + "' and a.death_date between '" + deathstart + "' and '" + deathsend + "'";
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(dt);

            return dt;
        }

        public DataTable getSearchReasultDisDeath_StEn(string disstart, string disend, string deathstart, string deathsend)
        {
            DataTable dt = new DataTable();

            string sql = "select a.proj_code,a.bundle_key,a.item_no,a.patient_id,a.patient_name as 'Patient Name',a.gender as 'Gender',a.guardian_name as 'Guardian Name',a.address as 'Address',a.reg_number as 'Registration Number',a.dep_name as 'Department Name',date_format(a.admission_date,'%d-%M-%Y') as 'Date of Admission',a.death as 'Death Status',date_format(a.death_date,'%d-%M-%Y') as 'Date of Death',date_format(a.discharge_date,'%d-%M-%Y') as 'Date of Discharge' from metadata_entry a,bundle_master b where a.proj_code = b.proj_code and a.bundle_key = b.bundle_key and b.status = 8 and a.discharge_date between '" + disstart + "' and '" + disend + "' and a.death_date between '" + deathstart + "' and '" + deathsend + "'";
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(dt);

            return dt;
        }

        public DataTable getSearchReasultAdDisDeath_StEn(string adstart, string adend, string disstart, string disend, string deathstart, string deathsend)
        {
            DataTable dt = new DataTable();

            string sql = "select a.proj_code,a.bundle_key,a.item_no,a.patient_id,a.patient_name as 'Patient Name',a.gender as 'Gender',a.guardian_name as 'Guardian Name',a.address as 'Address',a.reg_number as 'Registration Number',a.dep_name as 'Department Name',date_format(a.admission_date,'%d-%M-%Y') as 'Date of Admission',a.death as 'Death Status',date_format(a.death_date,'%d-%M-%Y') as 'Date of Death',date_format(a.discharge_date,'%d-%M-%Y') as 'Date of Discharge' from metadata_entry a,bundle_master b where a.proj_code = b.proj_code and a.bundle_key = b.bundle_key and b.status = 8 and a.discharge_date between '" + disstart + "' and '" + disend + "' and a.death_date between '" + deathstart + "' and '" + deathsend + "' and a.admission_date between '"+adstart+"' and '"+adend+"'";
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(dt);

            return dt;
        }

        private void formatForm()
        {

            this.deTextBox1.AutoCompleteCustomSource = GetSuggestions("metadata_entry", "patient_name");
            this.deTextBox1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            this.deTextBox1.AutoCompleteSource = AutoCompleteSource.CustomSource;

            this.deTextBox2.AutoCompleteCustomSource = GetSuggestions("metadata_entry", "guardian_name");
            this.deTextBox2.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            this.deTextBox2.AutoCompleteSource = AutoCompleteSource.CustomSource;

            this.deTextBox3.AutoCompleteCustomSource = GetSuggestions("metadata_entry", "address");
            this.deTextBox3.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            this.deTextBox3.AutoCompleteSource = AutoCompleteSource.CustomSource;

        }

        public AutoCompleteStringCollection GetSuggestions(string tblName, string fldName)
        {
            AutoCompleteStringCollection x = new AutoCompleteStringCollection();
            string sql = "Select distinct " + fldName + " from " + tblName + " where status = 8 ";
            DataSet ds = new DataSet();
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(ds);
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    x.Add(ds.Tables[0].Rows[i][0].ToString().Trim());
                }
            }

            return x;
        }

        public void currentDateSet()
        {
            dateTimePicker1.Value = DateTime.Now;
            dateTimePicker2.Value = DateTime.Now;
            dateTimePicker3.Value = DateTime.Now;
            dateTimePicker4.Value = DateTime.Now;
            dateTimePicker5.Value = DateTime.Now;
            dateTimePicker6.Value = DateTime.Now;
        }

        public void CustomformatChange()
        {
            dateTimePicker1.CustomFormat = " ";
            dateTimePicker2.CustomFormat = " ";
            dateTimePicker3.CustomFormat = " ";
            dateTimePicker4.CustomFormat = " ";
            dateTimePicker5.CustomFormat = " ";
            dateTimePicker6.CustomFormat = " ";
        }

        private void frmSearch_Load(object sender, EventArgs e)
        {
            formatForm();

            currentDateSet();

            CustomformatChange();

            if (getSearchReasult().Rows.Count > 0)
            {
                grdBox.DataSource = getSearchReasult();
                grdBox.Columns[0].Visible = false;
                grdBox.Columns[1].Visible = false;
                grdBox.Columns[2].Visible = false;
                grdBox.Columns[3].Visible = false;

                for (int i = 0; i < grdBox.Columns.Count; i++)
                {
                    grdBox.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
                label1.Visible = false;
            }
            else
            {
                grdBox.DataSource = null;
                label1.Visible = true;
            }

            deTextBox1.Focus();
            deTextBox1.Select();
        }

        

        private void grdBox_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

            projKey = grdBox.Rows[e.RowIndex].Cells[0].Value.ToString();
            bundleKey = grdBox.Rows[e.RowIndex].Cells[1].Value.ToString();
            patientID = grdBox.Rows[e.RowIndex].Cells[3].Value.ToString();

            frmPDFView frm = new frmPDFView(sqlCon, crd);
            frm.ShowDialog(this);
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            dateTimePicker1.CustomFormat = "yyyy-MM-dd";
        }

        private void cmdSearch_Click(object sender, EventArgs e)
        {
            if (dateTimePicker1.Text == " " && dateTimePicker2.Text != " ")
            {
                grdBox.DataSource = null;
                label1.Visible = true;
                label1.ForeColor = Color.Red;
                label1.Text = "Please, Specify Admission Start Date";
                return;
            }
            if (dateTimePicker3.Text == " " && dateTimePicker4.Text != " ")
            {
                grdBox.DataSource = null;
                label1.Visible = true;
                label1.ForeColor = Color.Red;
                label1.Text = "Please, Specify Discharge Start Date";
                return;
            }
            if (dateTimePicker5.Text == " " && dateTimePicker6.Text != " ")
            {
                grdBox.DataSource = null;
                label1.Visible = true;
                label1.ForeColor = Color.Red;
                label1.Text = "Please, Specify Death Start Date";
                return;
            }
            

            if (deTextBox1.Text == "" && deTextBox2.Text == "" && deTextBox3.Text == "" && dateTimePicker1.Text == " "
                && dateTimePicker2.Text == " " && dateTimePicker3.Text == " " && dateTimePicker4.Text == " " &&
                dateTimePicker5.Text == " " && dateTimePicker6.Text == " ")
            {
                if (getSearchReasult().Rows.Count > 0)
                {
                    grdBox.DataSource = getSearchReasult();
                    grdBox.Columns[0].Visible = false;
                    grdBox.Columns[1].Visible = false;
                    grdBox.Columns[2].Visible = false;
                    grdBox.Columns[3].Visible = false;

                    for (int i = 0; i < grdBox.Columns.Count; i++)
                    {
                        grdBox.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    }
                    label1.Visible = false;
                }
                else
                {
                    grdBox.DataSource = null;
                    label1.Visible = true;
                    label1.Text = "No Result Found . . .";
                }

                deTextBox1.Focus();
                deTextBox1.Select();
            }
            else
            {
                if (deTextBox1.Text != "" && deTextBox2.Text == "" && deTextBox3.Text == "" && dateTimePicker1.Text == " "
                && dateTimePicker2.Text == " " && dateTimePicker3.Text == " " && dateTimePicker4.Text == " " &&
                dateTimePicker5.Text == " " && dateTimePicker6.Text == " ")
                {
                    if (getSearchReasultName(deTextBox1.Text.Trim()).Rows.Count > 0)
                    {
                        grdBox.DataSource = getSearchReasultName(deTextBox1.Text.Trim());
                        grdBox.Columns[0].Visible = false;
                        grdBox.Columns[1].Visible = false;
                        grdBox.Columns[2].Visible = false;
                        grdBox.Columns[3].Visible = false;

                        for (int i = 0; i < grdBox.Columns.Count; i++)
                        {
                            grdBox.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        }
                        label1.Visible = false;
                    }
                    else
                    {
                        grdBox.DataSource = null;
                        label1.Visible = true;
                        label1.Text = "No Result Found . . .";
                    }
                }
                if (deTextBox1.Text == "" && deTextBox2.Text != "" && deTextBox3.Text == "" && dateTimePicker1.Text == " "
                && dateTimePicker2.Text == " " && dateTimePicker3.Text == " " && dateTimePicker4.Text == " " &&
                dateTimePicker5.Text == " " && dateTimePicker6.Text == " ")
                {
                    if (getSearchReasultGName(deTextBox2.Text.Trim()).Rows.Count > 0)
                    {
                        grdBox.DataSource = getSearchReasultGName(deTextBox2.Text.Trim());
                        grdBox.Columns[0].Visible = false;
                        grdBox.Columns[1].Visible = false;
                        grdBox.Columns[2].Visible = false;
                        grdBox.Columns[3].Visible = false;

                        for (int i = 0; i < grdBox.Columns.Count; i++)
                        {
                            grdBox.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        }
                        label1.Visible = false;
                    }
                    else
                    {
                        grdBox.DataSource = null;
                        label1.Visible = true;
                        label1.Text = "No Result Found . . .";
                    }
                }
                if (deTextBox1.Text == "" && deTextBox2.Text == "" && deTextBox3.Text != "" && dateTimePicker1.Text == " "
                && dateTimePicker2.Text == " " && dateTimePicker3.Text == " " && dateTimePicker4.Text == " " &&
                dateTimePicker5.Text == " " && dateTimePicker6.Text == " ")
                {
                    if (getSearchReasultAddress(deTextBox3.Text.Trim()).Rows.Count > 0)
                    {
                        grdBox.DataSource = getSearchReasultAddress(deTextBox3.Text.Trim());
                        grdBox.Columns[0].Visible = false;
                        grdBox.Columns[1].Visible = false;
                        grdBox.Columns[2].Visible = false;
                        grdBox.Columns[3].Visible = false;

                        for (int i = 0; i < grdBox.Columns.Count; i++)
                        {
                            grdBox.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        }
                        label1.Visible = false;
                    }
                    else
                    {
                        grdBox.DataSource = null;
                        label1.Visible = true;
                        label1.Text = "No Result Found . . .";
                    }
                }

                if (deTextBox1.Text == "" && deTextBox2.Text == "" && deTextBox3.Text == "" && dateTimePicker1.Text != " "
                && dateTimePicker2.Text == " " && dateTimePicker3.Text == " " && dateTimePicker4.Text == " " &&
                dateTimePicker5.Text == " " && dateTimePicker6.Text == " ")
                {
                    if (getSearchReasultAdS(dateTimePicker1.Text.Trim()).Rows.Count > 0)
                    {
                        grdBox.DataSource = getSearchReasultAdS(dateTimePicker1.Text.Trim());
                        grdBox.Columns[0].Visible = false;
                        grdBox.Columns[1].Visible = false;
                        grdBox.Columns[2].Visible = false;
                        grdBox.Columns[3].Visible = false;

                        for (int i = 0; i < grdBox.Columns.Count; i++)
                        {
                            grdBox.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        }
                        label1.Visible = false;
                    }
                    else
                    {
                        grdBox.DataSource = null;
                        label1.Visible = true;
                        label1.Text = "No Result Found . . .";
                    }
                }
               
                if (deTextBox1.Text == "" && deTextBox2.Text == "" && deTextBox3.Text == "" && dateTimePicker1.Text == " "
               && dateTimePicker2.Text == " " && dateTimePicker3.Text != " " && dateTimePicker4.Text == " " &&
               dateTimePicker5.Text == " " && dateTimePicker6.Text == " ")
                {
                    if (getSearchReasultDisS(dateTimePicker3.Text.Trim()).Rows.Count > 0)
                    {
                        grdBox.DataSource = getSearchReasultDisS(dateTimePicker3.Text.Trim());
                        grdBox.Columns[0].Visible = false;
                        grdBox.Columns[1].Visible = false;
                        grdBox.Columns[2].Visible = false;
                        grdBox.Columns[3].Visible = false;

                        for (int i = 0; i < grdBox.Columns.Count; i++)
                        {
                            grdBox.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        }
                        label1.Visible = false;
                    }
                    else
                    {
                        grdBox.DataSource = null;
                        label1.Visible = true;
                        label1.Text = "No Result Found . . .";
                    }
                }
               
                if (deTextBox1.Text == "" && deTextBox2.Text == "" && deTextBox3.Text == "" && dateTimePicker1.Text == " "
               && dateTimePicker2.Text == " " && dateTimePicker3.Text == " " && dateTimePicker4.Text == " " &&
               dateTimePicker5.Text != " " && dateTimePicker6.Text == " ")
                {
                    if (getSearchReasultDeathS(dateTimePicker5.Text.Trim()).Rows.Count > 0)
                    {
                        grdBox.DataSource = getSearchReasultDeathS(dateTimePicker5.Text.Trim());
                        grdBox.Columns[0].Visible = false;
                        grdBox.Columns[1].Visible = false;
                        grdBox.Columns[2].Visible = false;
                        grdBox.Columns[3].Visible = false;

                        for (int i = 0; i < grdBox.Columns.Count; i++)
                        {
                            grdBox.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        }
                        label1.Visible = false;
                    }
                    else
                    {
                        grdBox.DataSource = null;
                        label1.Visible = true;
                        label1.Text = "No Result Found . . .";
                    }
                }

                if (deTextBox1.Text != "" && deTextBox2.Text != "" && deTextBox3.Text == "" && dateTimePicker1.Text == " "
               && dateTimePicker2.Text == " " && dateTimePicker3.Text == " " && dateTimePicker4.Text == " " &&
               dateTimePicker5.Text == " " && dateTimePicker6.Text == " ")
                {
                    if (getSearchReasultNG(deTextBox1.Text.Trim(), deTextBox2.Text.Trim()).Rows.Count > 0)
                    {
                        grdBox.DataSource = getSearchReasultNG(deTextBox1.Text.Trim(), deTextBox2.Text.Trim());
                        grdBox.Columns[0].Visible = false;
                        grdBox.Columns[1].Visible = false;
                        grdBox.Columns[2].Visible = false;
                        grdBox.Columns[3].Visible = false;

                        for (int i = 0; i < grdBox.Columns.Count; i++)
                        {
                            grdBox.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        }
                        label1.Visible = false;
                    }
                    else
                    {
                        grdBox.DataSource = null;
                        label1.Visible = true;
                        label1.Text = "No Result Found . . .";
                    }
                }

                if (deTextBox1.Text != "" && deTextBox2.Text == "" && deTextBox3.Text != "" && dateTimePicker1.Text == " "
               && dateTimePicker2.Text == " " && dateTimePicker3.Text == " " && dateTimePicker4.Text == " " &&
               dateTimePicker5.Text == " " && dateTimePicker6.Text == " ")
                {
                    if (getSearchReasultNA(deTextBox1.Text.Trim(), deTextBox3.Text.Trim()).Rows.Count > 0)
                    {
                        grdBox.DataSource = getSearchReasultNA(deTextBox1.Text.Trim(), deTextBox3.Text.Trim());
                        grdBox.Columns[0].Visible = false;
                        grdBox.Columns[1].Visible = false;
                        grdBox.Columns[2].Visible = false;
                        grdBox.Columns[3].Visible = false;

                        for (int i = 0; i < grdBox.Columns.Count; i++)
                        {
                            grdBox.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        }
                        label1.Visible = false;
                    }
                    else
                    {
                        grdBox.DataSource = null;
                        label1.Visible = true;
                        label1.Text = "No Result Found . . .";
                    }
                }

                if (deTextBox1.Text == "" && deTextBox2.Text != "" && deTextBox3.Text != "" && dateTimePicker1.Text == " "
               && dateTimePicker2.Text == " " && dateTimePicker3.Text == " " && dateTimePicker4.Text == " " &&
               dateTimePicker5.Text == " " && dateTimePicker6.Text == " ")
                {
                    if (getSearchReasultGA(deTextBox2.Text.Trim(), deTextBox3.Text.Trim()).Rows.Count > 0)
                    {
                        grdBox.DataSource = getSearchReasultGA(deTextBox2.Text.Trim(), deTextBox3.Text.Trim());
                        grdBox.Columns[0].Visible = false;
                        grdBox.Columns[1].Visible = false;
                        grdBox.Columns[2].Visible = false;
                        grdBox.Columns[3].Visible = false;

                        for (int i = 0; i < grdBox.Columns.Count; i++)
                        {
                            grdBox.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        }
                        label1.Visible = false;
                    }
                    else
                    {
                        grdBox.DataSource = null;
                        label1.Visible = true;
                        label1.Text = "No Result Found . . .";
                    }
                }

                if (deTextBox1.Text == "" && deTextBox2.Text == "" && deTextBox3.Text == "" && dateTimePicker1.Text != " "
               && dateTimePicker2.Text != " " && dateTimePicker3.Text == " " && dateTimePicker4.Text == " " &&
               dateTimePicker5.Text == " " && dateTimePicker6.Text == " ")
                {
                    if (getSearchReasultAdStEn(dateTimePicker1.Text.Trim(), dateTimePicker2.Text.Trim()).Rows.Count > 0)
                    {
                        grdBox.DataSource = getSearchReasultAdStEn(dateTimePicker1.Text.Trim(), dateTimePicker2.Text.Trim());
                        grdBox.Columns[0].Visible = false;
                        grdBox.Columns[1].Visible = false;
                        grdBox.Columns[2].Visible = false;
                        grdBox.Columns[3].Visible = false;

                        for (int i = 0; i < grdBox.Columns.Count; i++)
                        {
                            grdBox.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        }
                        label1.Visible = false;
                    }
                    else
                    {
                        grdBox.DataSource = null;
                        label1.Visible = true;
                        label1.Text = "No Result Found . . .";
                    }
                }

                if (deTextBox1.Text == "" && deTextBox2.Text == "" && deTextBox3.Text == "" && dateTimePicker1.Text == " "
               && dateTimePicker2.Text == " " && dateTimePicker3.Text != " " && dateTimePicker4.Text != " " &&
               dateTimePicker5.Text == " " && dateTimePicker6.Text == " ")
                {
                    if (getSearchReasultDisStEn(dateTimePicker3.Text.Trim(), dateTimePicker4.Text.Trim()).Rows.Count > 0)
                    {
                        grdBox.DataSource = getSearchReasultDisStEn(dateTimePicker3.Text.Trim(), dateTimePicker4.Text.Trim());
                        grdBox.Columns[0].Visible = false;
                        grdBox.Columns[1].Visible = false;
                        grdBox.Columns[2].Visible = false;
                        grdBox.Columns[3].Visible = false;

                        for (int i = 0; i < grdBox.Columns.Count; i++)
                        {
                            grdBox.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        }
                        label1.Visible = false;
                    }
                    else
                    {
                        grdBox.DataSource = null;
                        label1.Visible = true;
                        label1.Text = "No Result Found . . .";
                    }
                }

                if (deTextBox1.Text == "" && deTextBox2.Text == "" && deTextBox3.Text == "" && dateTimePicker1.Text == " "
               && dateTimePicker2.Text == " " && dateTimePicker3.Text == " " && dateTimePicker4.Text == " " &&
               dateTimePicker5.Text != " " && dateTimePicker6.Text != " ")
                {
                    if (getSearchReasultDeathStEn(dateTimePicker5.Text.Trim(), dateTimePicker6.Text.Trim()).Rows.Count > 0)
                    {
                        grdBox.DataSource = getSearchReasultDeathStEn(dateTimePicker5.Text.Trim(), dateTimePicker6.Text.Trim());
                        grdBox.Columns[0].Visible = false;
                        grdBox.Columns[1].Visible = false;
                        grdBox.Columns[2].Visible = false;
                        grdBox.Columns[3].Visible = false;

                        for (int i = 0; i < grdBox.Columns.Count; i++)
                        {
                            grdBox.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        }
                        label1.Visible = false;
                    }
                    else
                    {
                        grdBox.DataSource = null;
                        label1.Visible = true;
                        label1.Text = "No Result Found . . .";
                    }
                }

                if (deTextBox1.Text == "" && deTextBox2.Text == "" && deTextBox3.Text == "" && dateTimePicker1.Text != " "
               && dateTimePicker2.Text != " " && dateTimePicker3.Text != " " && dateTimePicker4.Text != " " &&
               dateTimePicker5.Text == " " && dateTimePicker6.Text == " ")
                {
                    if (getSearchReasultAdDis_StEn(dateTimePicker1.Text.Trim(), dateTimePicker2.Text.Trim(), dateTimePicker3.Text.Trim(), dateTimePicker4.Text.Trim()).Rows.Count > 0)
                    {
                        grdBox.DataSource = getSearchReasultAdDis_StEn(dateTimePicker1.Text.Trim(), dateTimePicker2.Text.Trim(), dateTimePicker3.Text.Trim(), dateTimePicker4.Text.Trim());
                        grdBox.Columns[0].Visible = false;
                        grdBox.Columns[1].Visible = false;
                        grdBox.Columns[2].Visible = false;
                        grdBox.Columns[3].Visible = false;

                        for (int i = 0; i < grdBox.Columns.Count; i++)
                        {
                            grdBox.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        }
                        label1.Visible = false;
                    }
                    else
                    {
                        grdBox.DataSource = null;
                        label1.Visible = true;
                        label1.Text = "No Result Found . . .";
                    }
                }

                if (deTextBox1.Text == "" && deTextBox2.Text == "" && deTextBox3.Text == "" && dateTimePicker1.Text != " "
               && dateTimePicker2.Text != " " && dateTimePicker3.Text == " " && dateTimePicker4.Text == " " &&
               dateTimePicker5.Text != " " && dateTimePicker6.Text != " ")
                {
                    if (getSearchReasultAdDeath_StEn(dateTimePicker1.Text.Trim(), dateTimePicker2.Text.Trim(), dateTimePicker5.Text.Trim(), dateTimePicker6.Text.Trim()).Rows.Count > 0)
                    {
                        grdBox.DataSource = getSearchReasultAdDeath_StEn(dateTimePicker1.Text.Trim(), dateTimePicker2.Text.Trim(), dateTimePicker5.Text.Trim(), dateTimePicker6.Text.Trim());
                        grdBox.Columns[0].Visible = false;
                        grdBox.Columns[1].Visible = false;
                        grdBox.Columns[2].Visible = false;
                        grdBox.Columns[3].Visible = false;

                        for (int i = 0; i < grdBox.Columns.Count; i++)
                        {
                            grdBox.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        }
                        label1.Visible = false;
                    }
                    else
                    {
                        grdBox.DataSource = null;
                        label1.Visible = true;
                        label1.Text = "No Result Found . . .";
                    }
                }

                if (deTextBox1.Text == "" && deTextBox2.Text == "" && deTextBox3.Text == "" && dateTimePicker1.Text == " "
               && dateTimePicker2.Text == " " && dateTimePicker3.Text != " " && dateTimePicker4.Text != " " &&
               dateTimePicker5.Text != " " && dateTimePicker6.Text != " ")
                {
                    if (getSearchReasultDisDeath_StEn(dateTimePicker3.Text.Trim(), dateTimePicker4.Text.Trim(), dateTimePicker5.Text.Trim(), dateTimePicker6.Text.Trim()).Rows.Count > 0)
                    {
                        grdBox.DataSource = getSearchReasultDisDeath_StEn(dateTimePicker3.Text.Trim(), dateTimePicker4.Text.Trim(), dateTimePicker5.Text.Trim(), dateTimePicker6.Text.Trim());
                        grdBox.Columns[0].Visible = false;
                        grdBox.Columns[1].Visible = false;
                        grdBox.Columns[2].Visible = false;
                        grdBox.Columns[3].Visible = false;

                        for (int i = 0; i < grdBox.Columns.Count; i++)
                        {
                            grdBox.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        }
                        label1.Visible = false;
                    }
                    else
                    {
                        grdBox.DataSource = null;
                        label1.Visible = true;
                        label1.Text = "No Result Found . . .";
                    }
                }

                if (deTextBox1.Text == "" && deTextBox2.Text == "" && deTextBox3.Text == "" && dateTimePicker1.Text != " "
               && dateTimePicker2.Text != " " && dateTimePicker3.Text != " " && dateTimePicker4.Text != " " &&
               dateTimePicker5.Text != " " && dateTimePicker6.Text != " ")
                {
                    if (getSearchReasultAdDisDeath_StEn(dateTimePicker1.Text.Trim(), dateTimePicker2.Text.Trim(), dateTimePicker3.Text.Trim(), dateTimePicker4.Text.Trim(), dateTimePicker5.Text.Trim(), dateTimePicker6.Text.Trim()).Rows.Count > 0)
                    {
                        grdBox.DataSource = getSearchReasultAdDisDeath_StEn(dateTimePicker1.Text.Trim(), dateTimePicker2.Text.Trim(), dateTimePicker3.Text.Trim(), dateTimePicker4.Text.Trim(), dateTimePicker5.Text.Trim(), dateTimePicker6.Text.Trim());
                        grdBox.Columns[0].Visible = false;
                        grdBox.Columns[1].Visible = false;
                        grdBox.Columns[2].Visible = false;
                        grdBox.Columns[3].Visible = false;

                        for (int i = 0; i < grdBox.Columns.Count; i++)
                        {
                            grdBox.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        }
                        label1.Visible = false;
                    }
                    else
                    {
                        grdBox.DataSource = null;
                        label1.Visible = true;
                        label1.Text = "No Result Found . . .";
                    }
                }

                if (deTextBox1.Text != "" && deTextBox2.Text != "" && deTextBox3.Text != "" && dateTimePicker1.Text == " "
               && dateTimePicker2.Text == " " && dateTimePicker3.Text == " " && dateTimePicker4.Text == " " &&
               dateTimePicker5.Text == " " && dateTimePicker6.Text == " ")
                {
                    if (getSearchReasultNGA(deTextBox1.Text.Trim(), deTextBox2.Text.Trim(), deTextBox3.Text.Trim()).Rows.Count > 0)
                    {
                        grdBox.DataSource = getSearchReasultNGA(deTextBox1.Text.Trim(), deTextBox2.Text.Trim(), deTextBox3.Text.Trim());
                        grdBox.Columns[0].Visible = false;
                        grdBox.Columns[1].Visible = false;
                        grdBox.Columns[2].Visible = false;
                        grdBox.Columns[3].Visible = false;

                        for (int i = 0; i < grdBox.Columns.Count; i++)
                        {
                            grdBox.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        }
                        label1.Visible = false;
                    }
                    else
                    {
                        grdBox.DataSource = null;
                        label1.Visible = true;
                        label1.Text = "No Result Found . . .";
                    }
                }
            }
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            dateTimePicker2.CustomFormat = "yyyy-MM-dd";
        }

        private void dateTimePicker3_ValueChanged(object sender, EventArgs e)
        {
            dateTimePicker3.CustomFormat = "yyyy-MM-dd";
        }

        private void dateTimePicker4_ValueChanged(object sender, EventArgs e)
        {
            dateTimePicker4.CustomFormat = "yyyy-MM-dd";
        }

        private void dateTimePicker5_ValueChanged(object sender, EventArgs e)
        {
            dateTimePicker5.CustomFormat = "yyyy-MM-dd";
        }

        private void dateTimePicker6_ValueChanged(object sender, EventArgs e)
        {
            dateTimePicker6.CustomFormat = "yyyy-MM-dd";
        }

        private void dateTimePicker1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Back || e.KeyCode == Keys.Delete)
            {
                dateTimePicker1.CustomFormat = " ";
            }
            if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down || e.KeyCode == Keys.Enter)
            {
                dateTimePicker1.CustomFormat = "yyyy-MM-dd";
                dateTimePicker1.Select();
            }
        }

        private void dateTimePicker2_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Back || e.KeyCode == Keys.Delete)
            {
                dateTimePicker2.CustomFormat = " ";
            }
            if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down || e.KeyCode == Keys.Enter)
            {
                dateTimePicker2.CustomFormat = "yyyy-MM-dd";
                dateTimePicker2.Select();
            }
        }

        private void dateTimePicker3_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Back || e.KeyCode == Keys.Delete)
            {
                dateTimePicker3.CustomFormat = " ";
            }
            if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down || e.KeyCode == Keys.Enter)
            {
                dateTimePicker3.CustomFormat = "yyyy-MM-dd";
                dateTimePicker3.Select();
            }
        }

        private void dateTimePicker4_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Back || e.KeyCode == Keys.Delete)
            {
                dateTimePicker4.CustomFormat = " ";
            }
            if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down || e.KeyCode == Keys.Enter)
            {
                dateTimePicker4.CustomFormat = "yyyy-MM-dd";
                dateTimePicker4.Select();
            }
        }

        private void dateTimePicker5_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Back || e.KeyCode == Keys.Delete)
            {
                dateTimePicker5.CustomFormat = " ";
            }
            if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down || e.KeyCode == Keys.Enter)
            {
                dateTimePicker5.CustomFormat = "yyyy-MM-dd";
                dateTimePicker5.Select();
            }
        }

        private void dateTimePicker6_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Back || e.KeyCode == Keys.Delete)
            {
                dateTimePicker6.CustomFormat = " ";
            }
            if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down || e.KeyCode == Keys.Enter)
            {
                dateTimePicker6.CustomFormat = "yyyy-MM-dd";
                dateTimePicker6.Select();
            }
        }
    }
}
