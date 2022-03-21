using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NovaNet.wfe;
using NovaNet.Utils;
using System.Data.Odbc;
using System.Net;
using LItems;
using System.IO;
using nControls;

namespace ImageHeaven
{
    public partial class frmEntry : Form
    {
        MemoryStream stateLog;
        byte[] tmpWrite;
        NovaNet.Utils.dbCon dbcon;
        int pos = 0;
        int posAdd = 0;
        
        eSTATES[] state;
        wfeProject tmpProj = null;
        DataSet ds = null;
        private double ZOOMFACTOR = 1.10;   // = 25% smaller or larger
        private int MINMAX = 5;
        Point mouseDown = new Point();
        private Size ImageSize = new Size(100, 200);
        Credentials crd = new Credentials();
        //OdbcTransaction sqlTrans = null;
        public Dictionary<string, ListViewItem> ListViewItems = new Dictionary<string, ListViewItem>();
        public Dictionary<string, ListViewItem> ListViewItems1 = new Dictionary<string, ListViewItem>();


        public static bool c1;
        public static bool c2;
        public static bool c3;
        public static bool c4;
        public static bool c5;
        public static bool c6;
        public static bool c7;
        public static bool c8;
        public static bool c9;

        string name = frmMain.name;
        OdbcConnection sqlCon = null;
        public static bool _modeBool;
        //DataLayerDefs.Mode _mode = _modeBool;

        public static DataLayerDefs.Mode _mode =  DataLayerDefs.Mode._Edit;

        public static string projKey;
        public static string batchKey;
        public static string filename;

        public static bool control_txtletterno;
        public static bool control_txtIssuedFron;
        public static bool control_txtIssuedTo;
        public static bool control_txtSubject;
        public static bool control_txtYear;
        public static bool control_txtDate;
        public static bool control_cmbSubCat;
        public static bool control_cmbDocType;
        public static bool control_cmbMonth; 


        public frmEntry()
        {
            InitializeComponent();
        }

        public frmEntry(OdbcConnection pCon,DataLayerDefs.Mode mode)
        {
            InitializeComponent();

            if(mode == DataLayerDefs.Mode._Add)
            {
                txtProject.Text = frmBatchSelect.projName;
                txtBatch.Text = frmBatchSelect.batchName;
                projKey = frmBatchSelect.projKey;
                batchKey = frmBatchSelect.batchKey;

                

                label1.Visible = false;
                label2.Visible = false;
                label3.Visible = false;
                label4.Visible = false;
                label5.Visible = false;
                label6.Visible = false;
                label7.Visible = false;

                this.Text = "Data Entry : Add ";
                _mode = mode;
                cmbMonth.SelectedIndex = 0;
            }
            if(mode == DataLayerDefs.Mode._Edit)
            {
                txtProject.Text = frmFileSum.proj_name;
                txtBatch.Text = frmFileSum.batch_name;
                //projKey = frmBatchSelect.projKey;
                //batchKey = frmBatchSelect.batchKey;

                

                label1.Visible = false;
                label2.Visible = false;
                label3.Visible = false;
                label4.Visible = false;
                label5.Visible = false;
                label6.Visible = false;
                label7.Visible = false;

                this.Text = "Data Entry : Edit ";
                _mode = mode;
            }
            sqlCon = pCon;
        }
        

        private void frmEntry_Load(object sender, EventArgs e)
        {
            
            deButton1.Visible = false;
            deButton2.Visible = false;
            if (txtPath.Text == string.Empty)
            {
                cmdBrowse.Enabled = true;
                cmdBrowse.Select();
            }
            else
            {
                cmdBrowse.Enabled = false;
                lstImage.Select();
            }
            txtPath.Enabled = false;
            populateSubCat();
            populateDocType();
            cmbMonth.SelectedIndex = 0;

            this.txtSubject.AutoCompleteCustomSource = GetSuggestions("metadata_entry", "sub_name");
            this.txtSubject.AutoCompleteMode = AutoCompleteMode.Suggest;
            this.txtSubject.AutoCompleteSource = AutoCompleteSource.CustomSource;
            if (_mode == DataLayerDefs.Mode._Add)
            {
                cmdBrowse.Select();
                //txtLetterNo.Select();
            }

            if(_mode == DataLayerDefs.Mode._Edit)
            {
                cmdBrowse.Select();
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();

                string sql = "select c.proj_key,c.batch_key from project_master a,batch_master b,metadata_entry c where a.proj_key = c.proj_key and a.proj_key = b.proj_code and b.batch_key = c.batch_key and a.proj_code = '" + frmFileSum.proj_name + "' and b.batch_code = '"+frmFileSum.batch_name+"'";

                OdbcDataAdapter odap = new OdbcDataAdapter(sql, sqlCon);
                odap.Fill(dt);

                projKey = dt.Rows[0][0].ToString();
                batchKey = dt.Rows[0][1].ToString();

                DataSet ds1 = new DataSet();
                DataTable dt1 = new DataTable();
                string sql1 = "select letter_no,issue_from,issue_to,sub_cat,sub_name,doc_type,issue_date,filename from metadata_entry where proj_key = '"+projKey+"' and batch_key = '"+batchKey+"' and filename = '"+frmFileSum.filename+"'";
                OdbcDataAdapter odap1 = new OdbcDataAdapter(sql1, sqlCon);
                odap1.Fill(dt1);

                txtLetterNo.Text = dt1.Rows[0][0].ToString();
                txtIssuedFrom.Text = dt1.Rows[0][1].ToString();
                txtIssuedTo.Text = dt1.Rows[0][2].ToString();
                cmbSubCat.Text = dt1.Rows[0][3].ToString();
                txtSubject.Text = dt1.Rows[0][4].ToString();
                cmbDocType.Text = dt1.Rows[0][5].ToString();
                string datefrmdt = dt1.Rows[0][6].ToString();
                txtYear.Text = datefrmdt.Substring(0, 4);
                cmbMonth.Text = datefrmdt.Substring(5,2);
                txtDate.Text = datefrmdt.Substring(8,2);
                filename = dt1.Rows[0][7].ToString();


                groupBox2.Select();
            }

        }

        public AutoCompleteStringCollection GetSuggestions(string tblName, string fldName)
        {
            AutoCompleteStringCollection x = new AutoCompleteStringCollection();
            string sql = "Select distinct " + fldName + " from " + tblName;
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
            x.Add("Others");
            //x.Add("NA");
            return x;
        }
        private void populateSubCat()
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            string sql = "select sub_cat_id, sub_cat_name from sub_cat_master ";

            OdbcDataAdapter odap = new OdbcDataAdapter(sql, sqlCon);
            odap.Fill(dt);


            if (dt.Rows.Count > 0)
            {
                cmbSubCat.DataSource = dt;
                cmbSubCat.DisplayMember = "sub_cat_name";
                cmbSubCat.ValueMember = "sub_cat_id";

            }
            else
            {
                cmbSubCat.Text = string.Empty;
                cmbSubCat.DataSource = null;
                cmbSubCat.DisplayMember = "";
                cmbSubCat.ValueMember = "";

            }
        }

        private void populateDocType()
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            string sql = "select doc_id, doc_type from document_master ";

            OdbcDataAdapter odap = new OdbcDataAdapter(sql, sqlCon);
            odap.Fill(dt);


            if (dt.Rows.Count > 0)
            {
                cmbDocType.DataSource = dt;
                cmbDocType.DisplayMember = "doc_type";
                cmbDocType.ValueMember = "doc_id";

            }
            else
            {
                cmbDocType.Text = string.Empty;
                cmbDocType.DataSource = null;
                cmbDocType.DisplayMember = "";
                cmbDocType.ValueMember = "";
            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
           // dateTimePicker1.CustomFormat = "yyyy-MM-dd";
            label7.ForeColor = Color.Black;
            label7.Visible = true;
            label7.Text = "When was the document issued";


            string currDate = DateTime.Now.ToString("yyyy-MM-dd");
            //if (DateTime.Parse(dateTimePicker1.Text) <= DateTime.Parse(currDate))
            //{
            //    label7.ForeColor = Color.Black;
            //    label7.Visible = true;
            //    label7.Text = "When was the document issued";
            //}
            //else
            //{
            //    label7.ForeColor = Color.Red;
            //    label7.Visible = true;
            //    label7.Text = "You cannot select any date\n beyond Current Date";
            //    return;
            //}
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are You Sure?? Do you Want to Exit???", "Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dialogResult == DialogResult.Yes)
            {
                this.Close();
            }
            else if (dialogResult == DialogResult.No)
            {
                //frmEntry_Load(sender, e);
                //txtLetterNo.Text = string.Empty;
                //txtIssuedFrom.Text = string.Empty;
                //txtIssuedTo.Text = string.Empty;
                //txtSubject.Text = string.Empty;
                
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (validate() == true)
            {
                if(_mode == DataLayerDefs.Mode._Add)
                {
                    bool insertmeta = insertFn();
                    if (insertmeta == true)
                    {
                        MessageBox.Show(this, "Successfully Inserted...", " ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        frmEntry_Load(sender, e);
                        txtLetterNo.Text = string.Empty;
                        txtIssuedFrom.Text = string.Empty;
                        txtIssuedTo.Text = string.Empty;
                        txtSubject.Text = string.Empty;
                        //txtYear.Text = string.Empty;
                        //txtDate.Text = string.Empty;
                        txtLetterNo.Select();
                    }
                    else
                    {
                        MessageBox.Show(this, "Ooops!!! There is an Error - Record not Saved...", " ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                if (_mode == DataLayerDefs.Mode._Edit)
                {
                    bool updatemeta = updateFn();
                    if (updatemeta == true)
                    {
                        MessageBox.Show(this, "Successfully Inserted...", " ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show(this, "Ooops!!! There is an Error - Record not Saved...", " ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            //else
            //{
            //    MessageBox.Show("Please select all the parameter...", " ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    //txtLetterNo.Focus();
            //    //txtLetterNo.Select();
            //}
        }

        public bool insertFn()
        {
            bool ret = false;
            if (ret == false)
            {
                _InsertMeta();

                ret = true;
            }
            return ret;
        }

        public bool updateFn()
        {
            bool ret = false;
            if (ret == false)
            {
                _UpdateMeta();

                ret = true;
            }
            return ret;
        }

        public bool _InsertMeta()
        {
            bool retVal = false;
            string sql = string.Empty;
            string sl_no;
            string item_no;
            string filenameAdd;
            string sqlcou = "select MAX(sl_no) from metadata_entry where proj_key = '" + frmBatchSelect.projKey + "' and batch_key = '" + frmBatchSelect.batchKey + "' and issue_from = '" + txtIssuedFrom.Text.ToUpper() + "' and issue_to = '" + txtIssuedTo.Text.ToUpper() + "' and issue_date = '" + txtYear.Text + "-" + cmbMonth.Text + "-" + txtDate.Text + "'";
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            OdbcDataAdapter odap = new OdbcDataAdapter(sqlcou, sqlCon);
            odap.Fill(dt);
            string xyz = dt.Rows[0][0].ToString();
            if ((xyz == null) || (xyz == ""))
            {

                sl_no = "0";
                filenameAdd = txtIssuedFrom.Text + "_" + txtIssuedTo.Text + "_" + txtYear.Text +  cmbMonth.Text +  txtDate.Text;
            }
            else
            {
                int tmp_sl = Convert.ToInt32(dt.Rows[0][0].ToString());
                int tmp_sl_1 = tmp_sl + 1;
                sl_no = Convert.ToString(tmp_sl_1);
                filenameAdd = txtIssuedFrom.Text + "_" + txtIssuedTo.Text + "_" + txtYear.Text + cmbMonth.Text +  txtDate.Text + "_" + sl_no;
            }

            string sqlcouItem = "select MAX(item_no) from metadata_entry where proj_key = '" + frmBatchSelect.projKey + "' and batch_key = '" + frmBatchSelect.batchKey + "' ";
            DataSet ds1 = new DataSet();
            DataTable dt1 = new DataTable();
            OdbcDataAdapter odap1 = new OdbcDataAdapter(sqlcouItem, sqlCon);
            odap1.Fill(dt1);
            string max_item = dt1.Rows[0][0].ToString();

            if (max_item == null || max_item == "")
            {
                item_no = "1";
            }
            else
            {
                int itemConv = Convert.ToInt32(max_item);
                int itemCounter = itemConv + 1;
                item_no = Convert.ToString(itemCounter);
            }


            sql = "insert into metadata_entry(proj_key,batch_key,sl_no,item_no,letter_no,issue_from,issue_to,sub_cat,sub_name,doc_type,issue_date,filename,created_by,created_dttm,status)" +
                    "values('" + frmBatchSelect.projKey + "', " +
                        "'" + frmBatchSelect.batchKey + "', " +
                        "'" + sl_no + "', " +
                        "'" + item_no + "', " +
                        "'" + txtLetterNo.Text.ToUpper() + "', " +
                        "'" + txtIssuedFrom.Text.ToUpper() + "', " +
                        "'" + txtIssuedTo.Text.ToUpper() + "','" + cmbSubCat.Text + "','" + txtSubject.Text + "','" + cmbDocType.Text + "','" + txtYear.Text + "-" + cmbMonth.Text + "-" + txtDate.Text + "','" + filenameAdd.ToUpper() + "','" + frmMain.name + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "','N')";


            System.Diagnostics.Debug.Print(sql);
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            if (cmd.ExecuteNonQuery() > 0)
            {
                retVal = true;
            }
            return retVal;
        }

        public bool _UpdateMeta()
        {
            bool retVal = false;
            string sql = string.Empty;
            string sl_no;
            string filenameEdit;

            string sqlcou = "select MAX(sl_no) from metadata_entry where proj_key = '" + projKey + "' and batch_key = '" + batchKey + "' and letter_no = '"+txtLetterNo.Text.ToUpper()+"' and issue_from = '" + txtIssuedFrom.Text.ToUpper() + "' and issue_to = '" + txtIssuedTo.Text.ToUpper() + "' and sub_cat = '"+cmbSubCat.Text+"' and sub_name = '"+txtSubject.Text.ToUpper()+"' and doc_type = '"+cmbDocType.Text+"' and issue_date = '" + txtYear.Text + "-" + cmbMonth.Text + "-" + txtDate.Text + "'";
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            OdbcDataAdapter odap = new OdbcDataAdapter(sqlcou, sqlCon);
            odap.Fill(dt);

            string xyz = dt.Rows[0][0].ToString();
            if ((xyz == null) || (xyz == ""))
            {

                sl_no = "0";
                filenameEdit = txtIssuedFrom.Text + "_" + txtIssuedTo.Text + "_" + txtYear.Text + cmbMonth.Text + txtDate.Text;
            }
            else
            {
                int tmp_sl = Convert.ToInt32(dt.Rows[0][0].ToString());
                int tmp_sl_1 = tmp_sl + 1;
                sl_no = Convert.ToString(tmp_sl_1);
                filenameEdit = txtIssuedFrom.Text + "_" + txtIssuedTo.Text + "_" + txtYear.Text + cmbMonth.Text + txtDate.Text + "_" + sl_no;
            }

            sql = "UPDATE metadata_entry SET sl_no = '" + sl_no + "',letter_no = '" + txtLetterNo.Text.ToUpper() + "',issue_from = '" + txtIssuedFrom.Text.ToUpper() + "',issue_to = '" + txtIssuedTo.Text.ToUpper() + "',sub_cat = '" + cmbSubCat.Text + "',sub_name= '" + txtSubject.Text + "',doc_type= '" + cmbDocType.Text + "',issue_date= '" + txtYear.Text + "-" + cmbMonth.Text + "-" + txtDate.Text + "',filename = '"+filenameEdit.ToUpper()+"' WHERE proj_key = '"+projKey+"' AND batch_key = '"+batchKey+"' AND filename ='"+filename+"'";
            System.Diagnostics.Debug.Print(sql);
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            if (cmd.ExecuteNonQuery() > 0)
            {
                retVal = true;
            }


            return retVal;
        }

        public bool validate()
        {
            bool retval = false;

            //if (txtLetterNo.Text == null || txtLetterNo.Text == "")
            //{
            //    //retval = true;
            //    retval = false;
            //    MessageBox.Show("You cannot leave this field blank..", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    txtLetterNo.Focus();
            //    txtLetterNo.Select();
            //    return retval;
            //}
            //else
            //{
            //    retval = true;
            //    //MessageBox.Show("", "You cannot leave this field blank..", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    //txtLetterNo.Focus();
            //    //txtLetterNo.Select();
            //    //return retval;
            //}
            if (txtIssuedFrom.Text == null || txtIssuedFrom.Text == "")
            {

                retval = false;
                MessageBox.Show("You cannot leave this field blank..", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtIssuedFrom.Focus();
                txtIssuedFrom.Select();
                return retval;
            }
            else
            {
                retval = true;
            }
            if (txtIssuedTo.Text == null || txtIssuedTo.Text == "")
            {

                retval = false;
                MessageBox.Show("You cannot leave this field blank..", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtIssuedTo.Focus();
                txtIssuedTo.Select();
                return retval;
            }
            else
            {
                retval = true;
            }
            if (txtSubject.Text == null || txtSubject.Text == "")
            {

                retval = false;
                MessageBox.Show("You cannot leave this field blank..", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSubject.Focus();
                txtSubject.Select();
                return retval;
            }
            else
            {
                retval = true;
            }
            //if (dateTimePicker1.Text == null || dateTimePicker1.Text == " ")
            //{

            //    retval = false;
            //    MessageBox.Show("You cannot leave this field blank..", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    dateTimePicker1.Focus();
            //    dateTimePicker1.Select();
            //    return retval;
            //}
            //else
            //{
            //    retval = true;
            //}
            if (txtYear.Text == null || txtYear.Text == "")
            {
                retval = false;
                MessageBox.Show("You cannot leave this field blank..", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtYear.Focus();
                txtYear.Select();
                return retval;
            }
            else
            {
                retval = true;
            }
            if (cmbMonth.Text == null || cmbMonth.Text == "")
            {
                retval = false;
                MessageBox.Show("You cannot leave this field blank..", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbMonth.Focus();
                cmbMonth.Select();
                return retval;
            }
            else
            {
                retval = true;
            }
            if (txtDate.Text == null || txtDate.Text == "")
            {
                retval = false;
                MessageBox.Show("You cannot leave this field blank..", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDate.Focus();
                txtDate.Select();
                return retval;
            }
            else
            {
                retval = true;
            }
            string currDate = DateTime.Now.ToString("yyyy-MM-dd");
            //if (DateTime.Parse(dateTimePicker1.Text) <= DateTime.Parse(currDate))
            //{
            //    retval = true;
            //}
            //else
            //{
            //    retval = false;
            //    MessageBox.Show("You Cannot Select any date beyond Current Date...", "Please select a valid date", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    dateTimePicker1.Focus();
            //    dateTimePicker1.Select();
            //    return retval;
            //}
            string isDate = txtYear.Text + "-" + cmbMonth.Text + "-" + txtDate.Text;
            if (DateTime.Parse(txtYear.Text+"-"+cmbMonth.Text+"-"+txtDate.Text) <= DateTime.Parse(currDate))
            {
                retval = true;
            }
            else
            {
                retval = false;
                MessageBox.Show("You Cannot Select any date beyond Current Date...", "Please select a valid date", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //dateTimePicker1.Focus();
                //dateTimePicker1.Select();
                txtYear.Select();
                return retval;
                
            }
            
            //string sqlcou = "select * from metadata_entry where proj_key = '" + frmBatchSelect.projKey + "' and batch_key = '" + frmBatchSelect.batchKey + "' and letter_no = '" + txtLetterNo.Text.ToUpper() + "' and issue_date = '" + txtYear.Text + "-" + cmbMonth.Text + "-" + txtDate.Text + "'";
            //DataSet ds = new DataSet();
            //DataTable dt = new DataTable();
            //OdbcDataAdapter odap = new OdbcDataAdapter(sqlcou, sqlCon);
            //odap.Fill(dt);
            //if (dt.Rows.Count > 0)
            //{
            //    retval = false;
            //    MessageBox.Show("Letter No and Issue Date is same...", "Please try again !", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    txtLetterNo.Focus();
            //    txtLetterNo.Select();
            //    return retval;
            //}
            //else
            //{
            //    retval = true;
            //}

            return retval;
        }

        private void txtLetterNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            label1.ForeColor = Color.Black;
            label1.Visible = true;
            label1.Text = "Reference No. of the document,\n Eg- Letter No./ Order No.";
            txtLetterNo.Select();
        }

        private void txtLetterNo_TextChanged(object sender, EventArgs e)
        {
            label1.ForeColor = Color.Black;
            label1.Visible = true;
            label1.Text = "Reference No. of the document,\n Eg- Letter No./ Order No.";

            try
            {

                if (System.Text.RegularExpressions.Regex.IsMatch(txtLetterNo.Text, "[\'\"]"))
                {

                    txtLetterNo.Text = txtLetterNo.Text.Remove(txtLetterNo.Text.Length - 1);
                    label1.ForeColor = Color.Red;
                    txtLetterNo.Focus();
                    txtLetterNo.SelectAll();

                    char x = '"';
                    label1.Text = "You Cannot Type Some keys, \nLike: ' and " + x.ToString() + " in this field";
                    return;

                }


            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.ToString());
            }
        }

        private void txtLetterNo_TabIndexChanged(object sender, EventArgs e)
        {
            label1.ForeColor = Color.Black;
            label1.Visible = true;
            label1.Text = "Reference No. of the document,\n Eg- Letter No./ Order No.";
        }

        private void txtLetterNo_MouseHover(object sender, EventArgs e)
        {
            label1.ForeColor = Color.Black;
            label1.Visible = true;
            label1.Text = "Reference No. of the document,\n Eg- Letter No./ Order No.";
        }

        private void txtLetterNo_MouseLeave(object sender, EventArgs e)
        {
            label1.Visible = false;
        }

        private void txtLetterNo_Leave(object sender, EventArgs e)
        {
            label1.Visible = false;
            
        }

        private void txtLetterNo_Click(object sender, EventArgs e)
        {
            label1.ForeColor = Color.Black;
            label1.Visible = true;
            label1.Text = "Reference No. of the document,\n Eg- Letter No./ Order No.";
            
        }

        private void txtLetterNo_Enter(object sender, EventArgs e)
        {
            label1.ForeColor = Color.Black;
            label1.Visible = true;
            label1.Text = "Reference No. of the document,\n Eg- Letter No./ Order No.";
            txtLetterNo.Focus();
        }

        private void txtIssuedFrom_TextChanged(object sender, EventArgs e)
        {
            label2.ForeColor = Color.Black;
            label2.Visible = true;
            label2.Text = "Who issued the document";

            try
            {

                if (System.Text.RegularExpressions.Regex.IsMatch(txtIssuedFrom.Text, "[\'\"]"))
                {

                    txtIssuedFrom.Text = txtIssuedFrom.Text.Remove(txtIssuedFrom.Text.Length - 1);
                    label2.ForeColor = Color.Red;
                    txtIssuedFrom.Focus();
                    txtIssuedFrom.SelectAll();

                    char x = '"';
                    label2.Text = "You Cannot Type Some keys, \nLike: ' and " + x.ToString() + " in this field";
                    return;

                }


            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.ToString());
            }
        }

        private void txtIssuedFrom_KeyPress(object sender, KeyPressEventArgs e)
        {
            label2.ForeColor = Color.Black;
            label2.Visible = true;
            label2.Text = "Who issued the document";
            //txtIssuedFrom.Select();
        }

        private void txtIssuedFrom_Enter(object sender, EventArgs e)
        {
            label2.ForeColor = Color.Black;
            label2.Visible = true;
            label2.Text = "Who issued the document";
            txtIssuedFrom.Focus();

        }

        private void txtIssuedFrom_TabIndexChanged(object sender, EventArgs e)
        {
            label2.ForeColor = Color.Black;
            label2.Visible = true;
            label2.Text = "Who issued the document";
        }

        private void txtIssuedFrom_MouseClick(object sender, MouseEventArgs e)
        {
            label2.ForeColor = Color.Black;
            label2.Visible = true;
            label2.Text = "Who issued the document";
        }

        private void txtIssuedFrom_Click(object sender, EventArgs e)
        {
            label2.ForeColor = Color.Black;
            label2.Visible = true;
            label2.Text = "Who issued the document";
        }

        private void txtLetterNo_MouseClick(object sender, MouseEventArgs e)
        {
            label1.ForeColor = Color.Black;
            label1.Visible = true;
            label1.Text = "Reference No. of the document,\n Eg- Letter No./ Order No.";
        }

        private void txtIssuedFrom_Leave(object sender, EventArgs e)
        {
            label2.Visible = false;
            
        }

        private void txtIssuedFrom_MouseLeave(object sender, EventArgs e)
        {
            label2.Visible = false;
        }

        private void txtIssuedFrom_MouseHover(object sender, EventArgs e)
        {
            label2.ForeColor = Color.Black;
            label2.Visible = true;
            label2.Text = "Who issued the document";
        }

        private void txtIssuedTo_TextChanged(object sender, EventArgs e)
        {
            label3.ForeColor = Color.Black;
            label3.Visible = true;
            label3.Text = "To whom the document issued";

            try
            {

                if (System.Text.RegularExpressions.Regex.IsMatch(txtIssuedTo.Text, "[\'\"]"))
                {

                    txtIssuedTo.Text = txtIssuedTo.Text.Remove(txtIssuedTo.Text.Length - 1);
                    label3.ForeColor = Color.Red;
                    txtIssuedTo.Focus();
                    txtIssuedTo.SelectAll();

                    char x = '"';
                    label3.Text = "You Cannot Type Some keys, \nLike: ' and " + x.ToString() + " in this field";
                    return;

                }


            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.ToString());
            }
        }

        private void txtIssuedTo_TabIndexChanged(object sender, EventArgs e)
        {
            label3.ForeColor = Color.Black;
            label3.Visible = true;
            label3.Text = "To whom the document issued";
        }

        private void txtIssuedTo_MouseLeave(object sender, EventArgs e)
        {
            label3.Visible = false;
        }

        private void txtIssuedTo_MouseHover(object sender, EventArgs e)
        {
            label3.ForeColor = Color.Black;
            label3.Visible = true;
            label3.Text = "To whom the document issued";
        }

        private void txtIssuedTo_MouseClick(object sender, MouseEventArgs e)
        {
            label3.ForeColor = Color.Black;
            label3.Visible = true;
            label3.Text = "To whom the document issued";
        }

        private void txtIssuedTo_Leave(object sender, EventArgs e)
        {
            label3.Visible = false;
            
        }

        private void txtIssuedTo_KeyPress(object sender, KeyPressEventArgs e)
        {
            label3.ForeColor = Color.Black;
            label3.Visible = true;
            label3.Text = "To whom the document issued";
            txtIssuedTo.Select();
        }

        private void txtIssuedTo_Enter(object sender, EventArgs e)
        {
            label3.ForeColor = Color.Black;
            label3.Visible = true;
            label3.Text = "To whom the document issued";
            txtIssuedTo.Select();
        }

        private void txtIssuedTo_Click(object sender, EventArgs e)
        {
            label3.ForeColor = Color.Black;
            label3.Visible = true;
            label3.Text = "To whom the document issued";
        }

        private void cmbSubCat_SelectedIndexChanged(object sender, EventArgs e)
        {
            label4.ForeColor = Color.Black;
            label4.Visible = true;
            label4.Text = "Select proper Subject Category \nfrom the dropdown";
        }

        private void cmbSubCat_TabIndexChanged(object sender, EventArgs e)
        {
            label4.ForeColor = Color.Black;
            label4.Visible = true;
            label4.Text = "Select proper Subject Category \nfrom the dropdown";
        }

        private void cmbSubCat_TextChanged(object sender, EventArgs e)
        {
            label4.ForeColor = Color.Black;
            label4.Visible = true;
            label4.Text = "Select proper Subject Category \nfrom the dropdown";
        }

        private void cmbSubCat_KeyDown(object sender, KeyEventArgs e)
        {
            //cmbSubCat.Select();
            label4.ForeColor = Color.Black;
            label4.Visible = true;
            label4.Text = "Select proper Subject Category \nfrom the dropdown";

           
        }

        private void cmbSubCat_KeyUp(object sender, KeyEventArgs e)
        {
            label4.ForeColor = Color.Black;
            label4.Visible = true;
            label4.Text = "Select proper Subject Category \nfrom the dropdown";
        }

        private void cmbSubCat_KeyPress(object sender, KeyPressEventArgs e)
        {
            label4.ForeColor = Color.Black;
            label4.Visible = true;
            label4.Text = "Select proper Subject Category \nfrom the dropdown";
        }

        private void cmbSubCat_MouseHover(object sender, EventArgs e)
        {
            label4.ForeColor = Color.Black;
            label4.Visible = true;
            label4.Text = "Select proper Subject Category \nfrom the dropdown";
        }

        private void cmbSubCat_Leave(object sender, EventArgs e)
        {
            label4.Visible = false;
            
            
        }

        private void cmbSubCat_MouseLeave(object sender, EventArgs e)
        {
            label4.Visible = false;
        }

        private void cmbSubCat_Enter(object sender, EventArgs e)
        {
            label4.ForeColor = Color.Black;
            label4.Visible = true;
            label4.Text = "Select proper Subject Category \nfrom the dropdown";
            cmbSubCat.Select();
        }

        private void cmbSubCat_MouseClick(object sender, MouseEventArgs e)
        {
            label4.ForeColor = Color.Black;
            label4.Visible = true;
            label4.Text = "Select proper Subject Category \nfrom the dropdown";
        }

        private void cmbSubCat_Click(object sender, EventArgs e)
        {
            label4.ForeColor = Color.Black;
            label4.Visible = true;
            label4.Text = "Select proper Subject Category \nfrom the dropdown";
        }

        private void cmbSubCat_CursorChanged(object sender, EventArgs e)
        {
            label4.Visible = true;
            label4.Text = "Select proper Subject Category \nfrom the dropdown";
        }

        private void txtSubject_TextChanged(object sender, EventArgs e)
        {
            label5.ForeColor = Color.Black;
            label5.Visible = true;
            label5.Text = "Subject of the document";

            try
            {

                if (System.Text.RegularExpressions.Regex.IsMatch(txtSubject.Text, "[\'\"]"))
                {

                    txtSubject.Text = txtSubject.Text.Remove(txtSubject.Text.Length - 1);
                    label5.ForeColor = Color.Red;
                    txtSubject.Focus();
                    txtSubject.SelectAll();

                    char x = '"';
                    label5.Text = "You Cannot Type Some keys, \nLike: ' and " + x.ToString() + " in this field";
                    return;

                }


            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.ToString());
            }
        }

        private void txtSubject_TabIndexChanged(object sender, EventArgs e)
        {
            label5.ForeColor = Color.Black;
            label5.Visible = true;
            label5.Text = "Subject of the document";
        }

        private void txtSubject_MouseLeave(object sender, EventArgs e)
        {
            label5.Visible = false;
        }

        private void txtSubject_MouseHover(object sender, EventArgs e)
        {
            label5.ForeColor = Color.Black;
            label5.Visible = true;
            label5.Text = "Subject of the document";
        }

        private void txtSubject_MouseClick(object sender, MouseEventArgs e)
        {
            label5.ForeColor = Color.Black;
            label5.Visible = true;
            label5.Text = "Subject of the document";
        }

        private void txtSubject_Leave(object sender, EventArgs e)
        {
            label5.Visible = false;
           
        }

        private void txtSubject_KeyPress(object sender, KeyPressEventArgs e)
        {
            label5.ForeColor = Color.Black;
            label5.Visible = true;
            label5.Text = "Subject of the document";
        }

        private void txtSubject_Enter(object sender, EventArgs e)
        {
            label5.ForeColor = Color.Black;
            label5.Visible = true;
            label5.Text = "Subject of the document";
            txtSubject.Select();
        }

        private void txtSubject_Click(object sender, EventArgs e)
        {
            label5.ForeColor = Color.Black;
            label5.Visible = true;
            label5.Text = "Subject of the document";
        }

        private void cmbDocType_SelectedIndexChanged(object sender, EventArgs e)
        {
            label6.ForeColor = Color.Black;
            label6.Visible = true;
            label6.Text = "Select proper document type\n from the dropdown";
        }

        private void cmbDocType_TextChanged(object sender, EventArgs e)
        {
            label6.ForeColor = Color.Black;
            label6.Visible = true;
            label6.Text = "Select proper document type\n from the dropdown";
        }

        private void cmbDocType_TabIndexChanged(object sender, EventArgs e)
        {
            label6.ForeColor = Color.Black;
            label6.Visible = true;
            label6.Text = "Select proper document type\n from the dropdown";
        }

        private void cmbDocType_MouseLeave(object sender, EventArgs e)
        {
            label6.Visible = false;
        }

        private void cmbDocType_MouseHover(object sender, EventArgs e)
        {
            label6.ForeColor = Color.Black;
            label6.Visible = true;
            label6.Text = "Select proper document type\n from the dropdown";
        }

        private void cmbDocType_MouseClick(object sender, MouseEventArgs e)
        {
            label6.ForeColor = Color.Black;
            label6.Visible = true;
            label6.Text = "Select proper document type\n from the dropdown";
        }

        private void cmbDocType_Leave(object sender, EventArgs e)
        {
            label6.Visible = false;
            

        }

        private void cmbDocType_KeyUp(object sender, KeyEventArgs e)
        {
            label6.ForeColor = Color.Black;
            label6.Visible = true;
            label6.Text = "Select proper document type\n from the dropdown";
        }

        private void cmbDocType_KeyPress(object sender, KeyPressEventArgs e)
        {
            label6.ForeColor = Color.Black;
            label6.Visible = true;
            label6.Text = "Select proper document type\n from the dropdown";
        }

        private void cmbDocType_Enter(object sender, EventArgs e)
        {
            label6.ForeColor = Color.Black;
            label6.Visible = true;
            label6.Text = "Select proper document type\n from the dropdown";
            cmbDocType.Select();
        }

        private void cmbDocType_KeyDown(object sender, KeyEventArgs e)
        {
            //cmbDocType.Select();
            label6.ForeColor = Color.Black;
            label6.Visible = true;
            label6.Text = "Select proper document type\n from the dropdown";
           
        }

        private void cmbDocType_Click(object sender, EventArgs e)
        {
            label6.ForeColor = Color.Black;
            label6.Visible = true;
            label6.Text = "Select proper document type\n from the dropdown";
        }

        private void cmbDocType_CursorChanged(object sender, EventArgs e)
        {
            label6.ForeColor = Color.Black;
            label6.Visible = true;
            label6.Text = "Select proper document type\n from the dropdown";
        }

        private void dateTimePicker1_TabIndexChanged(object sender, EventArgs e)
        {
            label7.ForeColor = Color.Black;
            label7.Visible = true;
            label7.Text = "When was the document issued";

        }

        private void dateTimePicker1_MouseLeave(object sender, EventArgs e)
        {
            label7.Visible = false;
        }

        private void dateTimePicker1_MouseHover(object sender, EventArgs e)
        {
            label7.ForeColor = Color.Black;
            label7.Visible = true;
            label7.Text = "When was the document issued";
        }

        private void dateTimePicker1_MouseEnter(object sender, EventArgs e)
        {
            label7.ForeColor = Color.Black;
            label7.Visible = true;
            label7.Text = "When was the document issued";

        }

        private void dateTimePicker1_Leave(object sender, EventArgs e)
        {
            label7.ForeColor = Color.Black;
            label7.Visible = false;
        }

        private void dateTimePicker1_KeyUp(object sender, KeyEventArgs e)
        {
            label7.ForeColor = Color.Black;
            label7.Visible = true;
            label7.Text = "When was the document issued";

        }

        private void dateTimePicker1_KeyPress(object sender, KeyPressEventArgs e)
        {
            label7.ForeColor = Color.Black;
            label7.Visible = true;
            label7.Text = "When was the document issued";
            //dateTimePicker1.CustomFormat = "yyyy-MM-dd";
        }

        private void dateTimePicker1_KeyDown(object sender, KeyEventArgs e)
        {
            label7.ForeColor = Color.Black;
            label7.Visible = true;
            label7.Text = "When was the document issued";

        }

        private void dateTimePicker1_Enter(object sender, EventArgs e)
        {
            label7.ForeColor = Color.Black;
            label7.Visible = true;
            label7.Text = "When was the document issued";

           // dateTimePicker1.Value = DateTime.Today;

        }

        private void dateTimePicker1_CursorChanged(object sender, EventArgs e)
        {
            label7.ForeColor = Color.Black;
            label7.Visible = true;
            label7.Text = "When was the document issued";
        }

        private void txtYear_TextChanged(object sender, EventArgs e)
        {
            label7.ForeColor = Color.Black;
            label7.Visible = true;
            label7.Text = "When was the document issued";

            try
            {

                if (System.Text.RegularExpressions.Regex.IsMatch(txtYear.Text, "[^0-9]"))
                {

                    txtYear.Text = txtYear.Text.Remove(txtYear.Text.Length - 1);
                    label7.ForeColor = Color.Red;
                    txtYear.Focus();
                    txtYear.SelectAll();

                    //char x = '"';
                    label7.Text = "You Cannot Type any alphabets\n or special charecters";
                    return;

                }
                else if (txtYear.Text.Length != 4)
                {
                    label7.ForeColor = Color.Red;
                    txtYear.Focus();

                    label7.Text = "You Cannot Type any invalid\n Year !";
                    //txtYear.Select();
                    return;
                }
                else if (txtYear.Text.Substring(0,1) == "0")
                {
                    label7.ForeColor = Color.Red;
                    txtYear.Focus();

                    label7.Text = "You Cannot start with 0\n ";
                    //txtYear.SelectAll();
                    return;
                }
                else
                {
                    label7.ForeColor = Color.Black;
                    label7.Visible = true;
                    label7.Text = "When was the document issued";
                }

            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.ToString());
            }
        }

        private void txtYear_Leave(object sender, EventArgs e)
        {

            if (txtYear.Text.Length != 4)
            {
                label7.ForeColor = Color.Red;
                txtYear.Focus();

                label7.Text = "You Cannot Type any invalid\n Year !";
                //txtYear.Select();
                return;
            }
            else if (txtYear.Text.Substring(0, 1) == "0")
            {
                label7.ForeColor = Color.Red;
                txtYear.Focus();

                label7.Text = "You Cannot start with 0\n ";
                //txtYear.SelectAll();
                return;
            }
            else
            {
                label7.ForeColor = Color.Black;
                label7.Visible = true;
                label7.Text = "When was the document issued";
               
            }
            
        }

        private void txtYear_MouseLeave(object sender, EventArgs e)
        {
            label7.Visible = false;
        }

        private void txtYear_Click(object sender, EventArgs e)
        {
            label7.ForeColor = Color.Black;
            label7.Visible = true;
            label7.Text = "When was the document issued";
        }

        private void txtYear_Enter(object sender, EventArgs e)
        {
            label7.ForeColor = Color.Black;
            label7.Visible = true;
            label7.Text = "When was the document issued";
            txtYear.Select();
        }

        private void txtYear_KeyPress(object sender, KeyPressEventArgs e)
        {
            label7.ForeColor = Color.Black;
            label7.Visible = true;
            label7.Text = "When was the document issued";
        }

        private void txtYear_MouseClick(object sender, MouseEventArgs e)
        {
            label7.ForeColor = Color.Black;
            label7.Visible = true;
            label7.Text = "When was the document issued";
        }

        private void txtYear_MouseHover(object sender, EventArgs e)
        {
            label7.ForeColor = Color.Black;
            label7.Visible = true;
            label7.Text = "When was the document issued";
        }

        private void txtYear_TabIndexChanged(object sender, EventArgs e)
        {
            label7.ForeColor = Color.Black;
            label7.Visible = true;
            label7.Text = "When was the document issued";
        }


        private void cmbMonth_TabIndexChanged(object sender, EventArgs e)
        {
            label7.ForeColor = Color.Black;
            label7.Visible = true;
            label7.Text = "When was the document issued";
        }

        private void cmbMonth_TextChanged(object sender, EventArgs e)
        {
            if (cmbMonth.Text == "" || cmbMonth.Text == null)
            {
                label7.ForeColor = Color.Red;
                label7.Visible = true;
                label7.Text = "You cannot leave this\n field blank ";
                cmbMonth.Select();
                return;
            }
            else
            {
                label7.ForeColor = Color.Black;
                label7.Visible = true;
                label7.Text = "When was the document issued";
            }
        }

        private void cmbMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbMonth.Text == "" || cmbMonth.Text == null)
            {
                label7.ForeColor = Color.Red;
                label7.Visible = true;
                label7.Text = "You cannot leave this\n field blank ";
                cmbMonth.Select();
                return;
            }
            else
            {
                label7.ForeColor = Color.Black;
                label7.Visible = true;
                label7.Text = "When was the document issued";
            }
        }

        private void cmbMonth_Click(object sender, EventArgs e)
        {
            label7.ForeColor = Color.Black;
            label7.Visible = true;
            label7.Text = "When was the document issued";
        }

        private void cmbMonth_CursorChanged(object sender, EventArgs e)
        {
            label7.ForeColor = Color.Black;
            label7.Visible = true;
            label7.Text = "When was the document issued";
        }

        private void cmbMonth_Enter(object sender, EventArgs e)
        {
            label7.ForeColor = Color.Black;
            label7.Visible = true;
            label7.Text = "When was the document issued";
            cmbMonth.Select();
        }

        private void cmbMonth_KeyDown(object sender, KeyEventArgs e)
        {
            //cmbMonth.Select();
            label7.ForeColor = Color.Black;
            label7.Visible = true;
            label7.Text = "When was the document issued";
           
        }

        private void cmbMonth_KeyPress(object sender, KeyPressEventArgs e)
        {
            label7.ForeColor = Color.Black;
            label7.Visible = true;
            label7.Text = "When was the document issued";
        }

        private void cmbMonth_KeyUp(object sender, KeyEventArgs e)
        {
            label7.ForeColor = Color.Black;
            label7.Visible = true;
            label7.Text = "When was the document issued";
        }

        private void cmbMonth_Leave(object sender, EventArgs e)
        {
            if (cmbMonth.Text == "" || cmbMonth.Text == null)
            {
                label7.ForeColor = Color.Red;
                label7.Visible = true;
                label7.Text = "You cannot leave this\n field blank ";
                cmbMonth.Select();
                return;
            }
            else
            {
                label7.ForeColor = Color.Black;
                label7.Visible = true;
                label7.Text = "When was the document issued";
                
            }
            
        }

        private void cmbMonth_MouseLeave(object sender, EventArgs e)
        {
            label7.Visible = false;
        }

        private void cmbMonth_MouseHover(object sender, EventArgs e)
        {
            label7.ForeColor = Color.Black;
            label7.Visible = true;
            label7.Text = "When was the document issued";
        }

        private void cmbMonth_MouseClick(object sender, MouseEventArgs e)
        {
            label7.ForeColor = Color.Black;
            label7.Visible = true;
            label7.Text = "When was the document issued";
        }

        private void txtDate_Enter(object sender, EventArgs e)
        {
            label7.ForeColor = Color.Black;
            label7.Visible = true;
            label7.Text = "When was the document issued";
            txtDate.Select();
        }

        private void txtDate_Click(object sender, EventArgs e)
        {
            label7.ForeColor = Color.Black;
            label7.Visible = true;
            label7.Text = "When was the document issued";
        }

        private void txtDate_KeyDown(object sender, KeyEventArgs e)
        {
            //txtDate.Select();
            label7.ForeColor = Color.Black;
            label7.Visible = true;
            label7.Text = "When was the document issued";
            
        }

        private void txtDate_KeyPress(object sender, KeyPressEventArgs e)
        {
            label7.ForeColor = Color.Black;
            label7.Visible = true;
            label7.Text = "When was the document issued";
        }

        private void txtDate_KeyUp(object sender, KeyEventArgs e)
        {
            label7.ForeColor = Color.Black;
            label7.Visible = true;
            label7.Text = "When was the document issued";
        }

        private void txtDate_Leave(object sender, EventArgs e)
        {
            if (txtDate.Text.Length != 2)
            {
                label7.ForeColor = Color.Red;
                txtDate.Focus();

                label7.Text = "You Cannot Type any invalid\n Date !";
                //txtYear.Select();
                return;
            }
            else
            {
                label7.ForeColor = Color.Black;
                label7.Visible = true;
                label7.Text = "When was the document issued";
                //buttonSave.Select();
            }
            
        }

        private void txtDate_MouseClick(object sender, MouseEventArgs e)
        {
            label7.ForeColor = Color.Black;
            label7.Visible = true;
            label7.Text = "When was the document issued";
        }

        private void txtDate_MouseHover(object sender, EventArgs e)
        {
            label7.ForeColor = Color.Black;
            label7.Visible = true;
            label7.Text = "When was the document issued";
        }

        private void txtDate_MouseLeave(object sender, EventArgs e)
        {
            label7.Visible = false;
        }

        private void txtDate_TabIndexChanged(object sender, EventArgs e)
        {
            label7.ForeColor = Color.Black;
            label7.Visible = true;
            label7.Text = "When was the document issued";
        }

        private void txtDate_TextChanged(object sender, EventArgs e)
        {
            label7.ForeColor = Color.Black;
            label7.Visible = true;
            label7.Text = "When was the document issued";
            try
            {

                if (System.Text.RegularExpressions.Regex.IsMatch(txtDate.Text, "[^0-9]"))
                {

                    txtDate.Text = txtDate.Text.Remove(txtDate.Text.Length - 1);
                    label7.ForeColor = Color.Red;
                    txtDate.Focus();
                    txtDate.SelectAll();

                    char x = '"';
                    label7.Text = "You Cannot Type any alphabets\n or special charecters";
                    return;

                }
                else if (txtDate.Text.Length != 2)
                {
                    label7.ForeColor = Color.Red;
                    txtDate.Focus();

                    label7.Text = "You Cannot Type any invalid\n Date !";
                    //txtYear.Select();
                    return;
                }
                else
                {
                    label7.ForeColor = Color.Black;
                    label7.Visible = true;
                    label7.Text = "When was the document issued";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void txtBatch_TextChanged(object sender, EventArgs e)
        {

        }

        private void labelBatch_Click(object sender, EventArgs e)
        {

        }

        private void txtProject_TextChanged(object sender, EventArgs e)
        {

        }

        private void labelProject_Click(object sender, EventArgs e)
        {

        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void label14_Click(object sender, EventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void labelIssuedDate_Click(object sender, EventArgs e)
        {

        }

        private void labelDocType_Click(object sender, EventArgs e)
        {

        }

        private void labelSubject_Click(object sender, EventArgs e)
        {

        }

        private void labelSubCat_Click(object sender, EventArgs e)
        {

        }

        private void labelIssuedTo_Click(object sender, EventArgs e)
        {

        }

        private void labelIssuedFrom_Click(object sender, EventArgs e)
        {

        }

        private void labelLetter_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void cmdBrowse_Click(object sender, EventArgs e)
        {
            try
            {
                
                List<string> fileNames = new List<string>();
                List<string> tempPath = new System.Collections.Generic.List<string>(1000);

                fbdPath.ShowDialog();
                txtPath.Text = fbdPath.SelectedPath;
                DirectoryInfo selectedPath = new DirectoryInfo(txtPath.Text);
                if (Directory.Exists(txtPath.Text + "\\Backup"))
                {

                }
                else
                {
                    Directory.CreateDirectory(txtPath.Text + "\\Backup");
                    DirectoryInfo selectedPath1 = new DirectoryInfo(txtPath.Text);
                    foreach (FileInfo file in selectedPath.GetFiles())
                    {
                        if (file.Extension == ".TIF" || file.Extension == ".tif")
                        {
                            file.CopyTo(txtPath.Text + "\\Backup\\" + file.Name);
                        }
                    }
                }

                if (selectedPath.GetFiles().Length > 0)

                    foreach (FileInfo file in selectedPath.GetFiles())
                    {
                        if (file.Extension.Equals(".tif") || file.Extension.Equals(".TIF"))
                        {
                            fileNames.Add(file.FullName);
                            tempPath.Add(txtPath.Text + "\\" + file.ToString());
                        }
                    }
                //lvwItem.SubItems.Add(CLAIMS.ToString());
                //lvwItem.SubItems.Add("0");
                ListViewItems.Clear();
                ListViewItems1.Clear();
                lstImage.Items.Clear();
                //lstTotalImage.Items.Clear();
                lstImage.BeginUpdate();
                //lstTotalImage.BeginUpdate();

                foreach (string fileName in fileNames)
                {

                    ListViewItem lvi = lstImage.Items.Add(System.IO.Path.GetFileNameWithoutExtension(fileName));
                    //ListViewItem lvi1 = lstTotalImage.Items.Add(System.IO.Path.GetFileNameWithoutExtension(fileName));
                    lvi.Tag = fileName;
                    //lvi1.Tag = fileName;
                    ListViewItems.Add(fileName, lvi);
                    

                    pos = pos + 1;
                    //ListViewItems1.Add(fileName, lvi1);
                }
                //foreach (string fileName in fileNames)
                //{
                //    ListViewItem lvi1 = lstTotalImage.Items.Add(System.IO.Path.GetFileNameWithoutExtension(fileName));
                //    lvi1.Tag = fileName;
                //    ListViewItems.Add(fileName, lvi1);
                //}
                lstImage.EndUpdate();
                // lstTotalImage.EndUpdate();
                
                //groupBox2.Enabled = false;

                if (lstImage.Items.Count > 0)
                {
                    toolTip1.ShowAlways = true;
                    lstImage.Items[0].Focused = true;
                    lstImage.Items[0].Selected = true;
                    picMain.Height = 647;
                    picMain.Width = 625;
                    picMain.Refresh();
                    picMain.ImageLocation = null;
                    string imgPath = txtPath.Text + "\\" + lstImage.Items[0].Text + ".TIF";
                    picMain.ImageLocation = imgPath;


                    Image newImage = Image.FromFile(imgPath);
                    picMain.Height = Convert.ToInt32(picMain.Height * 1.1);
                    picMain.Width = Convert.ToInt32(picMain.Height * newImage.Width / newImage.Height);

                    picMain.SizeMode = PictureBoxSizeMode.StretchImage;
                    //picMain.Width = Convert.ToInt32(picMain.Width * 1);
                    //picMain.Height = Convert.ToInt32(picMain.Height * 1);
                    picMain.Refresh();
                    newImage.Dispose();
                    // GC.Collect();
                    picMain.MouseWheel += new MouseEventHandler(picMain_MouseWheel);
                    picMain.MouseHover += new EventHandler(picMain_MouseHover);
                    //lstImage.Select();
                    picMain.Select();
                }
                else
                {
                    picMain.ImageLocation = null;
                    lstImage.Select();
                    picMain.Select();
                }
                //lstImage.Select();
                control_txtletterno = false;
                control_txtIssuedFron = false;
                control_txtIssuedTo = false;
                control_txtSubject = false;
                control_txtYear = false;
                control_txtDate = false;
                control_cmbSubCat = false;
                control_cmbDocType = false;
                control_cmbMonth = false;
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
        void picMain_MouseWheel(object sender, MouseEventArgs e)
        {
            //if (e.Delta < 0)
            //{
            //    ZoomIn();
            //}
            //else
            //{
            //    ZoomOut();
            //}

        }

        private void lstImage_SelectedIndexChanged(object sender, EventArgs e)
        {
            lstImage_MouseClick(sender, new MouseEventArgs(MouseButtons.Left, 1, 0, 0, 0));
        }
        private void lstImage_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (lstImage.Items.Count > 0)
                {

                    if (lstImage.SelectedItems.Count > 0)
                    {
                        toolTip1.ShowAlways = true;
                        //lstImage.SelectedItems[0].Focused = true;
                        //lstImage.SelectedItems[0].Selected = true;
                        picMain.Height = 647;
                        picMain.Width = 625;
                        picMain.Refresh();
                        picMain.ImageLocation = null;
                        string imgPath = txtPath.Text + "\\" + lstImage.SelectedItems[0].Text + ".TIF";
                        picMain.ImageLocation = imgPath;


                        Image newImage = Image.FromFile(imgPath);
                        picMain.Height = Convert.ToInt32(picMain.Height * 1.1);
                        picMain.Width = Convert.ToInt32(picMain.Height * newImage.Width / newImage.Height);

                        picMain.SizeMode = PictureBoxSizeMode.StretchImage;
                        //picMain.Width = Convert.ToInt32(picMain.Width * 1);
                        //picMain.Height = Convert.ToInt32(picMain.Height * 1);
                        picMain.Refresh();
                        newImage.Dispose();
                        // GC.Collect();
                        picMain.MouseWheel += new MouseEventHandler(picMain_MouseWheel);
                       // picMain.MouseHover += new EventHandler(picMain_MouseHover);
                        lstImage.Select();
                        picMain.Select();
                        toolTip1.SetToolTip(this.picMain, "\nTotal Image: " + lstImage.Items.Count + "\n \nCurrent Image: " + lstImage.SelectedItems[0].Text + ".TIF" + "\n \nPrevious Image: " + lstImage.Items[lstImage.Items.Count - 1].Text + ".TIF" + "\n \nNext Image: " + lstImage.Items[1].Text);
                    }
                    //control_txtletterno = false;
                    //control_txtIssuedFron = false;
                    //control_txtIssuedTo = false;
                    //control_txtSubject = false;
                    //control_txtYear = false;
                    //control_txtDate = false;
                    //control_cmbSubCat = false;
                    //control_cmbDocType = false;
                    //control_cmbMonth = false;
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message.ToString());
                GC.Collect();
            }
        }

        
        private void ZoomOut()
        {
            if (picMain.Height > (panel1.Height))
            {
                toolTip1.ShowAlways = true;
                panel3.Width = picMain.Width;
                picMain.SizeMode = PictureBoxSizeMode.StretchImage;
                picMain.Width = Convert.ToInt32(picMain.Width / ZOOMFACTOR);
                picMain.Height = Convert.ToInt32(picMain.Height / ZOOMFACTOR);
                picMain.Refresh();
                picMain.Select();
                //toolTip1.SetToolTip(this.picMain, "\n \n Total Image: " + lstImage.Items.Count + "\n \n" + "Current Image: " + lstImage.SelectedItems[0].Text + ".TIF");
            }
        }
        private void ZoomIn()
        {
            if ((picMain.Width < (MINMAX * panel3.Width)) &&
                (picMain.Height < (MINMAX * panel3.Height)))
            {
                toolTip1.ShowAlways = true;
                picMain.Width = Convert.ToInt32(picMain.Width * ZOOMFACTOR);
                picMain.Height = Convert.ToInt32(picMain.Height * ZOOMFACTOR);
                picMain.SizeMode = PictureBoxSizeMode.StretchImage;
                picMain.Refresh();
                picMain.Select();
                //toolTip1.SetToolTip(this.picMain, "\nTotal Image: " + lstImage.Items.Count + "\n \n" + "Current Image: " + lstImage.SelectedItems[0].Text + ".TIF");
            }
        }

       

        private void picMain_MouseClick_1(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                ZoomIn();
            
            if (e.Button == MouseButtons.Right)
                ZoomOut();
        }

        private void picMain_MouseHover(object sender, EventArgs e)
        {
            //picMain.Focus();
            //lstImage.Select();
            
        }

        private void picMain_MouseMove(object sender, MouseEventArgs e)
        {

            MouseEventArgs mouse = e as MouseEventArgs;

            if (mouse.Button == MouseButtons.Left)
            {
                Point mousePosNow = mouse.Location;

                int deltaX = mousePosNow.X - mouseDown.X;
                int deltaY = mousePosNow.Y - mouseDown.Y;

                int newX = picMain.Location.X + deltaX;
                int newY = picMain.Location.Y + deltaY;

                picMain.Location = new Point(newX, newY);
            }
            lstImage.Select();
        }

        private void picMain_MouseEnter(object sender, EventArgs e)
        {
            if (picMain.Focused == false)
            {
                picMain.Focus();
            }
        }

        private void picMain_MouseClick_2(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                ZoomIn();
            if (e.Button == MouseButtons.Right)
                ZoomOut();
        }

        private void deButton1_Click(object sender, EventArgs e)
        {
            try
            {
                if (lstImage.Items.Count > 0)
                {

                    if (lstImage.SelectedItems.Count > 0)
                    {
                        toolTip1.ShowAlways = true;

                        c1 = control_txtletterno;
                        c2 = control_txtIssuedFron;
                        c3 = control_txtIssuedTo;
                        c4 = control_cmbSubCat;
                        c5 = control_txtSubject;
                        c6 = control_cmbDocType;
                        c7 = control_txtYear;
                        c8 = control_cmbMonth;
                        c9 = control_txtDate;
                        //lstImage.SelectedItems[0].Focused = true;
                        //lstImage.SelectedItems[0].Selected = true;
                        picMain.Height = 647;
                        picMain.Width = 625;
                        picMain.Refresh();
                        picMain.ImageLocation = null;

                        
                        string x = lstImage.SelectedItems[0].Index.ToString();
                        int y = Convert.ToInt32(x);
                        if (y < lstImage.Items.Count - 1)
                        {
                            string imgPath = txtPath.Text + "\\" + lstImage.Items[y + 1].Text + ".TIF";
                            lstImage.Items[y].Selected = false;
                            lstImage.Items[y + 1].Selected = true;
                            picMain.ImageLocation = imgPath;
                            Image newImage = Image.FromFile(imgPath);
                            picMain.Height = Convert.ToInt32(picMain.Height * 1.1);
                            picMain.Width = Convert.ToInt32(picMain.Height * newImage.Width / newImage.Height);

                            picMain.SizeMode = PictureBoxSizeMode.StretchImage;
                            //picMain.Width = Convert.ToInt32(picMain.Width * 1);
                            //picMain.Height = Convert.ToInt32(picMain.Height * 1);
                            picMain.Refresh();
                            newImage.Dispose();
                            // GC.Collect();
                            picMain.MouseWheel += new MouseEventHandler(picMain_MouseWheel);
                            //picMain.MouseHover += new EventHandler(picMain_MouseHover);
                            lstImage.Select();
                            picMain.Select();
                            toolTip1.SetToolTip(this.picMain, "\nTotal Image: " + lstImage.Items.Count + "\n \nCurrent Image: " + lstImage.Items[y + 1].Text + ".TIF" + "\n \nPrevious Image: " + lstImage.Items[y].Text + ".TIF" + "\n\nNext Image: " + lstImage.Items[y+2].Text + ".TIF");
                        }
                        else
                        {
                            string imgPath = txtPath.Text + "\\" + lstImage.Items[0].Text + ".TIF";
                            lstImage.Items[0].Selected = true;
                            lstImage.Items[y].Selected = false;
                            picMain.ImageLocation = imgPath;
                            Image newImage = Image.FromFile(imgPath);
                            picMain.Height = Convert.ToInt32(picMain.Height * 1.1);
                            picMain.Width = Convert.ToInt32(picMain.Height * newImage.Width / newImage.Height);

                            picMain.SizeMode = PictureBoxSizeMode.StretchImage;
                            //picMain.Width = Convert.ToInt32(picMain.Width * 1);
                            //picMain.Height = Convert.ToInt32(picMain.Height * 1);
                            picMain.Refresh();
                            newImage.Dispose();
                            // GC.Collect();
                            picMain.MouseWheel += new MouseEventHandler(picMain_MouseWheel);
                            //picMain.MouseHover += new EventHandler(picMain_MouseHover);
                            lstImage.Select();
                            picMain.Select();
                            toolTip1.SetToolTip(this.picMain, "\nTotal Image: " + lstImage.Items.Count + "\n \nCurrent Image: " + lstImage.Items[0].Text + ".TIF" + "\n \nPrevious Image: " + lstImage.Items[y].Text + ".TIF" + "\n\nNext Image: " + lstImage.Items[1].Text + ".TIF");
                        }
                        
                        
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
                GC.Collect();
            }
        }

        private void deButton2_Click(object sender, EventArgs e)
        {
            try
            {
                if (lstImage.Items.Count > 0)
                {

                    if (lstImage.SelectedItems.Count > 0)
                    {
                        toolTip1.ShowAlways = true;

                        c1 = control_txtletterno;
                        c2 = control_txtIssuedFron;
                        c3 = control_txtIssuedTo;
                        c4 = control_cmbSubCat;
                        c5 = control_txtSubject;
                        c6 = control_cmbDocType;
                        c7 = control_txtYear;
                        c8 = control_cmbMonth;
                        c9 = control_txtDate;
                        //lstImage.SelectedItems[0].Focused = true;
                        //lstImage.SelectedItems[0].Selected = true;
                        picMain.Height = 647;
                        picMain.Width = 625;
                        picMain.Refresh();
                        picMain.ImageLocation = null;


                        string x = lstImage.SelectedItems[0].Index.ToString();
                        int y = Convert.ToInt32(x);
                        if (y == 0)
                        {
                            string imgPath = txtPath.Text + "\\" + lstImage.Items[lstImage.Items.Count-1].Text + ".TIF";
                            lstImage.Items[0].Selected = false;
                            lstImage.Items[lstImage.Items.Count - 1].Selected = true;
                            picMain.ImageLocation = imgPath;
                            Image newImage = Image.FromFile(imgPath);
                            picMain.Height = Convert.ToInt32(picMain.Height * 1.1);
                            picMain.Width = Convert.ToInt32(picMain.Height * newImage.Width / newImage.Height);

                            picMain.SizeMode = PictureBoxSizeMode.StretchImage;
                            //picMain.Width = Convert.ToInt32(picMain.Width * 1);
                            //picMain.Height = Convert.ToInt32(picMain.Height * 1);
                            picMain.Refresh();
                            newImage.Dispose();
                            // GC.Collect();
                            picMain.MouseWheel += new MouseEventHandler(picMain_MouseWheel);
                            //picMain.MouseHover += new EventHandler(picMain_MouseHover);
                            lstImage.Select();
                            picMain.Select();
                            toolTip1.SetToolTip(this.picMain, "\nTotal Image: " + lstImage.Items.Count + "\n \nCurrent Image: " + lstImage.Items[lstImage.Items.Count - 1].Text + ".TIF" + "\n \nPrevious Image: " + lstImage.Items[lstImage.Items.Count - 2].Text + ".TIF" + "\n \nNext Image: " + lstImage.Items[0].Text);
                        }
                        else
                        {
                            string imgPath = txtPath.Text + "\\" + lstImage.Items[y-1].Text + ".TIF";
                            lstImage.Items[y].Selected = false;
                            lstImage.Items[y-1].Selected = true;
                            picMain.ImageLocation = imgPath;
                            Image newImage = Image.FromFile(imgPath);
                            picMain.Height = Convert.ToInt32(picMain.Height * 1.1);
                            picMain.Width = Convert.ToInt32(picMain.Height * newImage.Width / newImage.Height);

                            picMain.SizeMode = PictureBoxSizeMode.StretchImage;
                            //picMain.Width = Convert.ToInt32(picMain.Width * 1);
                            //picMain.Height = Convert.ToInt32(picMain.Height * 1);
                            picMain.Refresh();
                            newImage.Dispose();
                            // GC.Collect();
                            picMain.MouseWheel += new MouseEventHandler(picMain_MouseWheel);
                            //picMain.MouseHover += new EventHandler(picMain_MouseHover);
                            lstImage.Select();
                            picMain.Select();
                            toolTip1.SetToolTip(this.picMain, "\nTotal Image: " + lstImage.Items.Count + "\n \nCurrent Image: " + lstImage.Items[y - 1].Text + ".TIF" + "\n \nPrevious Image: " + lstImage.Items[y - 2].Text + ".TIF" + "\n \nNext Image: " + lstImage.Items[y].Text + ".TIF");
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message.ToString());
                GC.Collect();
            }
        }

        private void frmEntry_KeyPress(object sender, KeyPressEventArgs e)
        {
            
        }

        private void frmEntry_KeyDown(object sender, KeyEventArgs e)
        {
            if (lstImage.Items.Count > 0)
            {
                if (picMain.ContainsFocus == true || lstImage.ContainsFocus == true || panel3.ContainsFocus== true)
                {
                        if (e.KeyCode == Keys.Left)
                        {
                            deButton2_Click(this, e);
                            picMain.Select();
                        }
                        else if (e.KeyCode == Keys.Right)
                        {
                            deButton1_Click(this, e);
                            picMain.Select();
                        }
                        else if (e.Modifiers == (Keys.Control))
                        {
                            
                           if(control_txtletterno == true || c1 == true)
                           {
                               txtLetterNo.Focus();
                               txtLetterNo_Enter(this, e);
                           }
                           else if (control_txtIssuedFron == true || c2 == true)
                           {
                               txtIssuedFrom.Focus();
                               txtIssuedFrom_Enter(this, e);
                           }
                           else if (control_txtIssuedTo == true || c3== true)
                           {
                               txtIssuedTo.Focus();
                               txtIssuedTo_Enter(this, e);
                           }
                           else if (control_cmbSubCat == true || c4 == true)
                           {
                               cmbSubCat.Focus();
                               cmbSubCat_Enter(this, e);
                           }
                           else if (control_txtSubject == true || c5 == true)
                           {
                               txtSubject.Focus();
                               txtSubject_Enter(this, e);
                           }
                           else if (control_cmbDocType == true || c6 == true)
                           {
                               cmbDocType.Focus();
                               cmbDocType_Enter(this, e);
                           }
                           else if (control_txtYear == true || c7 == true)
                           {
                               txtYear.Focus();
                               txtYear_Enter(this, e);
                           }
                           else if (control_cmbMonth == true || c8 == true)
                           {
                               cmbMonth.Focus();
                               cmbMonth_Enter(this, e);
                           }
                           else if (control_txtDate == true || c9== true)
                           {
                               txtDate.Focus();
                               txtDate_Enter(this, e);
                           }
                           else
                           { txtLetterNo.Focus(); }
                        }
                        else if (e.KeyData == Keys.Add)
                        {
                            ZoomIn();
                        }
                        else if (e.KeyData == Keys.Subtract)
                        {
                            ZoomOut();
                        }
                    }
                    else if (txtLetterNo.Focused == true || txtIssuedFrom.Focused == true || txtIssuedTo.Focused == true || cmbSubCat.Focused == true || txtSubject.Focused == true || cmbDocType.Focused == true || txtYear.Focused == true || cmbMonth.ContainsFocus == true || txtDate.ContainsFocus == true || buttonSave.ContainsFocus == true || buttonExit.Focused == true || groupBox2.Focused == true || picMain.ContainsFocus == false)
                    {
                        if (picMain.ContainsFocus == false)
                        {
                            if (e.Modifiers == (Keys.Control))
                            {

                                //lstImage.SelectedItems[0].Selected = true;
                                if (txtLetterNo.ContainsFocus == true)
                                {
                                    lstImage.Focus();
                                    lstImage.Select();
                                    picMain.Focus();
                                    picMain.Select();
                                    control_txtletterno = true;
                                    control_txtIssuedFron = false;
                                    control_txtIssuedTo = false;
                                    control_txtSubject = false;
                                    control_txtYear = false;
                                    control_txtDate = false;
                                    control_cmbSubCat = false;
                                    control_cmbDocType = false;
                                    control_cmbMonth = false;

                                }
                                else if (txtIssuedFrom.ContainsFocus == true)
                                {
                                    lstImage.Focus();
                                    lstImage.Select();
                                    picMain.Focus();
                                    picMain.Select();
                                    control_txtletterno = false;
                                    control_txtIssuedFron = true;
                                    control_txtIssuedTo = false;
                                    control_txtSubject = false;
                                    control_txtYear = false;
                                    control_txtDate = false;
                                    control_cmbSubCat = false;
                                    control_cmbDocType = false;
                                    control_cmbMonth = false;

                                }
                                else if (txtIssuedTo.ContainsFocus == true)
                                {
                                    lstImage.Focus();
                                    lstImage.Select();
                                    picMain.Focus();
                                    picMain.Select();
                                    control_txtletterno = false;
                                    control_txtIssuedFron = false;
                                    control_txtIssuedTo = true;
                                    control_txtSubject = false;
                                    control_txtYear = false;
                                    control_txtDate = false;
                                    control_cmbSubCat = false;
                                    control_cmbDocType = false;
                                    control_cmbMonth = false;

                                }
                                else if (cmbSubCat.ContainsFocus == true)
                                {
                                    lstImage.Focus();
                                    lstImage.Select();
                                    picMain.Focus();
                                    picMain.Select();
                                    control_txtletterno = false;
                                    control_txtIssuedFron = false;
                                    control_txtIssuedTo = false;
                                    control_txtSubject = false;
                                    control_txtYear = false;
                                    control_txtDate = false;
                                    control_cmbSubCat = true;
                                    control_cmbDocType = false;
                                    control_cmbMonth = false;

                                }
                                else if (txtSubject.ContainsFocus == true)
                                {
                                    lstImage.Focus();
                                    lstImage.Select();
                                    picMain.Focus();
                                    picMain.Select();
                                    control_txtletterno = false;
                                    control_txtIssuedFron = false;
                                    control_txtIssuedTo = false;
                                    control_txtSubject = true;
                                    control_txtYear = false;
                                    control_txtDate = false;
                                    control_cmbSubCat = false;
                                    control_cmbDocType = false;
                                    control_cmbMonth = false;

                                }
                                else if (cmbDocType.ContainsFocus == true)
                                {
                                    lstImage.Focus();
                                    lstImage.Select();
                                    picMain.Focus();
                                    picMain.Select();
                                    control_txtletterno = false;
                                    control_txtIssuedFron = false;
                                    control_txtIssuedTo = false;
                                    control_txtSubject = false;
                                    control_txtYear = false;
                                    control_txtDate = false;
                                    control_cmbSubCat = false;
                                    control_cmbDocType = true;
                                    control_cmbMonth = false;

                                }
                                else if (txtYear.ContainsFocus == true)
                                {
                                    lstImage.Focus();
                                    lstImage.Select();
                                    picMain.Focus();
                                    picMain.Select();
                                    control_txtletterno = false;
                                    control_txtIssuedFron = false;
                                    control_txtIssuedTo = false;
                                    control_txtSubject = false;
                                    control_txtYear = true;
                                    control_txtDate = false;
                                    control_cmbSubCat = false;
                                    control_cmbDocType = false;
                                    control_cmbMonth = false;

                                }
                                else if (cmbMonth.ContainsFocus == true)
                                {
                                    lstImage.Focus();
                                    lstImage.Select();
                                    picMain.Focus();
                                    picMain.Select();
                                    control_txtletterno = false;
                                    control_txtIssuedFron = false;
                                    control_txtIssuedTo = false;
                                    control_txtSubject = false;
                                    control_txtYear = false;
                                    control_txtDate = false;
                                    control_cmbSubCat = false;
                                    control_cmbDocType = false;
                                    control_cmbMonth = true;

                                }
                                else if (txtDate.ContainsFocus == true)
                                {
                                    lstImage.Focus();
                                    lstImage.Select();
                                    picMain.Focus();
                                    picMain.Select();
                                    control_txtletterno = false;
                                    control_txtIssuedFron = false;
                                    control_txtIssuedTo = false;
                                    control_txtSubject = false;
                                    control_txtYear = false;
                                    control_txtDate = true;
                                    control_cmbSubCat = false;
                                    control_cmbDocType = false;
                                    control_cmbMonth = false;

                                }
                                else
                                {
                                    lstImage.Focus();
                                    lstImage.Select();
                                    picMain.Focus();
                                    picMain.Select();
                                    control_txtletterno = false;
                                    control_txtIssuedFron = false;
                                    control_txtIssuedTo = false;
                                    control_txtSubject = false;
                                    control_txtYear = false;
                                    control_txtDate = false;
                                    control_cmbSubCat = false;
                                    control_cmbDocType = false;
                                    control_cmbMonth = false;
                                }
                            }
                        }
                        else if (e.Modifiers == Keys.Shift && txtLetterNo.ContainsFocus == true)
                        {
                            lstImage.Focus();
                            lstImage.Select();
                            picMain.Focus();
                            //lstImage.SelectedItems[0].Selected = true;

                        }
                        
                    }
                   
            }
            
            
        }

        private void picMain_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            
        }

        private void lstImage_Leave(object sender, EventArgs e)
        {
            //txtLetterNo.Select();
        }

        private void txtLetterNo_KeyDown(object sender, KeyEventArgs e)
        {
            //txtLetterNo.Select();
            label1.ForeColor = Color.Black;
            label1.Visible = true;
            label1.Text = "Reference No. of the document,\n Eg- Letter No./ Order No.";
            

        }

        private void txtIssuedFrom_KeyDown(object sender, KeyEventArgs e)
        {
            //txtIssuedFrom.Focus();
            label2.ForeColor = Color.Black;
            label2.Visible = true;
            label2.Text = "Who issued the document";
            
        }

        private void txtIssuedTo_KeyDown(object sender, KeyEventArgs e)
        {
            //txtIssuedTo.Select();
            label3.ForeColor = Color.Black;
            label3.Visible = true;
            label3.Text = "To whom the document issued";
            
        }

        private void txtSubject_KeyDown(object sender, KeyEventArgs e)
        {
            //txtSubject.Select();
            label5.ForeColor = Color.Black;
            label5.Visible = true;
            label5.Text = "Subject of the document";
            
        }

        private void txtYear_KeyDown(object sender, KeyEventArgs e)
        {
            //txtYear.Select();
            label7.ForeColor = Color.Black;
            label7.Visible = true;
            label7.Text = "When was the document issued";
           
        }

        private void buttonSave_Leave(object sender, EventArgs e)
        {
            //buttonExit.Select();
        }

        private void buttonExit_Leave(object sender, EventArgs e)
        {
            //txtLetterNo.Select();
        }

        
    }
}
