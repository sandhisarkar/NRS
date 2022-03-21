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
    public partial class fmEntry : Form
    {

        public static int index;

        Credentials crd = new Credentials();

        string name = frmMain.name;
        OdbcConnection sqlCon = null;
        public static bool _modeBool;

        public static DataLayerDefs.Mode _mode = DataLayerDefs.Mode._Edit;

        public static string projKey;
        public static string bundleKey;

        public static string caseFileNo;
        public static string patientId;

        public static string projName;
        public static string bundleName;

        public fmEntry()
        {
            InitializeComponent();
        }

        public string getBundleName(string projK, string bundleK)
        {
            DataTable dt = new DataTable();
            string sql = "select  bundle_code from bundle_master where proj_code = '" + projK + "' and bundle_key = '" + bundleK + "' ";
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(dt);
            return dt.Rows[0][0].ToString();
        }

        public string getProjName(string projK)
        {
            DataTable dt = new DataTable();
            string sql = "select  proj_code from project_master where proj_key = '" + projK + "'";
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(dt);
            return dt.Rows[0][0].ToString();
        }

        public fmEntry(OdbcConnection pCon, string projK, string bundleK, DataLayerDefs.Mode mode, string patient_id)
        {
            InitializeComponent();

            sqlCon = pCon;

            projKey = projK;

            bundleKey = bundleK;

            //projName = Files.projName;
            //bundleName = Files.bundleName;

            projName = getProjName(projKey);
            bundleName = getBundleName(projKey, bundleKey);

            _mode = mode;

            patientId = patient_id;
        }

        public fmEntry(OdbcConnection pCon, string projK, string bundleK, DataLayerDefs.Mode mode)
        {
            InitializeComponent();

            sqlCon = pCon;

            projKey = projK;

            bundleKey = bundleK;

            projName = frmBundleSelect.projName;
            bundleName = frmBundleSelect.bundleName;

            _mode = mode;
        }


        private int populateMaxCount(string project, string bundle)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            string sql = "select count(*) from metadata_entry where proj_code = '"+project+"' and bundle_key = '"+bundle+"' ";
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(dt);


            return Convert.ToInt32(dt.Rows[0][0].ToString());

            //if (dt.Rows.Count > 0)
            //{
            //    deComboBox1.DataSource = dt;
            //    deComboBox1.DisplayMember = "case_status";
            //    deComboBox1.ValueMember = "case_status_id";
            //}
        }

        public DataTable _GetMetaDataDetails(string proj, string bundle, string patientId)
        {
            DataTable dt = new DataTable();
            string sql = "select distinct proj_code, bundle_Key,item_no,patient_id,patient_name,gender, guardian_name,address,dep_name, reg_number, admission_date,death,death_date,discharge_date from metadata_entry where proj_code = '" + proj + "' and bundle_key = '" + bundle + "' and patient_id = '" + patientId + "' ";
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(dt);
            return dt;
        } 

        private void populateDepartment()
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            string sql = "select dept_id, dept_name from dept_details  ";
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(dt);


            if (dt.Rows.Count > 0)
            {
                deComboBox9.DataSource = dt;
                deComboBox9.DisplayMember = "dept_name";
                deComboBox9.ValueMember = "dept_id";
            }
           
        }


        private void fmEntry_Load(object sender, EventArgs e)
        {
            populateDepartment();
            deRadioButton1.Checked = true;
            dateTimePicker1.Value = DateTime.Now;
            dateTimePicker2.Value = DateTime.Now;
            dateTimePicker3.Value = DateTime.Now;
            checkBox1.Checked = false;
            dateTimePicker2.Enabled = false;
            
            if(_mode == DataLayerDefs.Mode._Add)
            {
                this.Text = "Entry Details:      Project : "+projName+"    Bundle : "+bundleName;
                dateTimePicker1.CustomFormat = " ";
                dateTimePicker2.CustomFormat = " ";
                dateTimePicker3.CustomFormat = " "; 
            }
            if (_mode == DataLayerDefs.Mode._Edit)
            {
                this.Text = "Entry Details:      Project : " + projName + "    Bundle : " + bundleName;

                deTextBox1.Text = _GetMetaDataDetails(projKey, bundleKey, patientId).Rows[0][4].ToString();
                string gender = _GetMetaDataDetails(projKey, bundleKey, patientId).Rows[0][5].ToString();

                if(gender == "Male")
                {
                    deRadioButton1.Checked = true;
                }
                else
                {
                    deRadioButton2.Checked = true;
                }

                deTextBox2.Text = _GetMetaDataDetails(projKey, bundleKey, patientId).Rows[0][6].ToString();
                deTextBox3.Text = _GetMetaDataDetails(projKey, bundleKey, patientId).Rows[0][7].ToString();

                deComboBox9.Text = _GetMetaDataDetails(projKey, bundleKey, patientId).Rows[0][8].ToString();

                deTextBox4.Text = _GetMetaDataDetails(projKey, bundleKey, patientId).Rows[0][9].ToString();

                dateTimePicker1.Text = _GetMetaDataDetails(projKey, bundleKey, patientId).Rows[0][10].ToString();

                string deathStat = _GetMetaDataDetails(projKey, bundleKey, patientId).Rows[0][11].ToString();

                if(deathStat == "Y")
                {
                    checkBox1.Checked = true;
                    dateTimePicker2.Enabled = true;
                    dateTimePicker2.Text = _GetMetaDataDetails(projKey, bundleKey, patientId).Rows[0][12].ToString();
                }
                else
                {
                    checkBox1.Checked = false;
                    dateTimePicker2.Enabled = false;
                    dateTimePicker2.CustomFormat = " ";
                    //dateTimePicker2.Text = " ";
                }
                string dischargeDate = _GetMetaDataDetails(projKey, bundleKey, patientId).Rows[0][13].ToString();

                if (dischargeDate == " " || dischargeDate == null || string.IsNullOrWhiteSpace(dischargeDate) || string.IsNullOrEmpty(dischargeDate))
                {
                    dateTimePicker3.CustomFormat = " "; 
                }
                else
                {
                    dateTimePicker3.CustomFormat = "yyyy-MM-dd";
                    dateTimePicker3.Text = _GetMetaDataDetails(projKey, bundleKey, patientId).Rows[0][13].ToString();
                }
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox1.Checked == true)
            {
                dateTimePicker2.Text = string.Empty;
                dateTimePicker2.CustomFormat = "yyyy-MM-dd";
                dateTimePicker2.Enabled = true;
            }
            else
            {
                dateTimePicker2.Text = string.Empty;
                dateTimePicker2.Enabled = false;
                dateTimePicker2.CustomFormat = " ";
            }
        }

        private void deButton21_Click(object sender, EventArgs e)
        {
            if (_mode == DataLayerDefs.Mode._Add)
            {
                DialogResult dr = MessageBox.Show(this, "Do you want to Exit ? ", "Record Management ", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dr == DialogResult.Yes)
                {
                    this.Hide();
                    Files fm = new Files(sqlCon, DataLayerDefs.Mode._Add);
                    fm.ShowDialog(this);
                }
                else
                {
                    return;
                }
            }
            if (_mode == DataLayerDefs.Mode._Edit)
            {
                DialogResult dr = MessageBox.Show(this, "Do you want to Exit ? ", "Record Management ", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dr == DialogResult.Yes)
                {
                    this.Hide();
                    //Files fm = new Files(sqlCon, DataLayerDefs.Mode._Edit);
                    //fm.ShowDialog(this);
                }
                else
                {
                    return;
                }
            }
        }

        private int generateRegNo(string regNumber)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            string sql = "select * from metadata_entry where reg_number = '"+regNumber+"'";
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(dt);


            return dt.Rows.Count;
        }

        private int generateRegNoforOthers(string regNumber, string pID)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            string sql = "select * from metadata_entry where reg_number = '" + regNumber + "' and patient_id <> '"+pID+"' ";
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(dt);


            return dt.Rows.Count;
        }


        public bool validateRegforEdit()
        {
            bool retval = false;
            //registration no check
            if (deTextBox4.Text != "")
            {
                if (generateRegNoforOthers(deTextBox4.Text.Trim(),patientId) > 0)
                {
                    retval = false;
                    MessageBox.Show("This Registration number is already exists...", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    deTextBox4.Select();
                    return retval;
                }
                else
                {
                    retval = true;
                }
            }
            else
            {
                retval = false;
                MessageBox.Show("Please Specify Unique Registration number...", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                deTextBox3.Select();
                return retval;
            }

            return retval;
        }


        public bool validateReg()
        {
            bool retval = false;
            //registration no check
            if (deTextBox4.Text != "")
            {
                if (generateRegNo(deTextBox4.Text.Trim()) > 0)
                {
                    retval = false;
                    MessageBox.Show("This Registration number is already exists...", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    deTextBox4.Select();
                    return retval;
                }
                else
                {
                    retval = true;
                }
            }
            else
            {
                retval = false;
                MessageBox.Show("Please Specify Unique Registration number...", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                deTextBox3.Select();
                return retval;
            }

            return retval;
        }

        public bool validate()
        {
            bool retval = false;

            string currDate = DateTime.Now.ToString("yyyy-MM-dd");
            string curYear = DateTime.Now.ToString("yyyy");
            int curIntYear = Convert.ToInt32(curYear);

            string dateFormat = "yyyy-MM-dd";
            string dateFromDtp = dateTimePicker1.Text;
            DateTime datetime;


            //patient's name check
            if(deTextBox1.Text == "")
            {
                retval = false;
                MessageBox.Show("Please Specify Patient's name...", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                deTextBox1.Select();
                return retval;
            }
            else
            {
                retval = true;
            }

            //gender check
            if(deRadioButton1.Checked == false && deRadioButton2.Checked == false)
            {
                retval = false;
                MessageBox.Show("You have to specify gender", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                deRadioButton1.Focus();
                return retval;
            }
            else
            {
                retval = true;
            }

            //guardian's name check
            if (deTextBox2.Text == "")
            {
                retval = false;
                MessageBox.Show("Please Specify Guardian's's name...", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                deTextBox2.Select();
                return retval;
            }
            else
            {
                retval = true;
            }

            //address check
            if (deTextBox3.Text == "")
            {
                retval = false;
                MessageBox.Show("Please Specify Address...", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                deTextBox3.Select();
                return retval;
            }
            else
            {
                retval = true;
            }


            //department check
            if (deComboBox9.Text == "")
            {
                retval = false;
                MessageBox.Show("Please Specify Specific Department name...", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                deTextBox3.Select();
                return retval;
            }
            else
            {
                retval = true;
            }

            

            //admission date check
            if (DateTime.TryParseExact(dateFromDtp, dateFormat, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out datetime))
            {

                string month = dateFromDtp.Substring(5, 2).ToString();
                string year = dateFromDtp.Substring(0, 4).ToString();

                string bundleCode = month + year;

                if (bundleCode != bundleName)
                {
                    retval = false;
                    MessageBox.Show("Admission Date isn't for this bundle", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    dateTimePicker1.Select();
                    return retval;
                }
                else
                {
                    retval = true;
                }
                   
            }
            else
            {
                retval = false;
                MessageBox.Show("Please Specify Proper Admission Date", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                   
                dateTimePicker1.Select();
                return retval;
            }


            //check death date
            if (checkBox1.Checked == true && dateTimePicker2.Text == "")
            {
                retval = false;
                MessageBox.Show("Please Specify Proper Death Date", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                dateTimePicker2.Select();
                return retval;
            }
            else if (checkBox1.Checked == true && dateTimePicker2.Text != "")
            {
                DateTime temp;
                string isDate = dateTimePicker2.Text;
               
                 //DateTime.Parse(isDate) <= DateTime.Parse(currDate)
                
                if (DateTime.TryParse(isDate, out temp) && isDate.CompareTo(currDate) <= 0)
                {
                    //DateTime.Parse(isDate) >= DateTime.Parse(dateFromDtp)
                    if (DateTime.TryParse(isDate, out temp) && isDate.CompareTo(dateFromDtp) >= 0)
                    {
                        retval = true;
                    }
                    else
                    {
                        retval = false;
                        MessageBox.Show("Please select a valid death date", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                        dateTimePicker2.Select();
                        return retval;

                    }
                }
                else
                {
                    retval = false;
                    MessageBox.Show("Please select a valid death date", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    dateTimePicker2.Select();
                    return retval;

                }

            }
            else
            {
                retval = true;
            }

            //death check box
            if (checkBox1.Checked == false && dateTimePicker2.Text != "")
            {
                retval = false;
                MessageBox.Show("Please Specify Proper Death Date", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                checkBox1.Select();
                return retval;
            }
            else
            {
                retval = true;
            }



            //discharge
            if (dateTimePicker3.Text == "" || dateTimePicker3.Text == null || string.IsNullOrWhiteSpace(dateTimePicker3.Text))
            {
                retval = true;
            }
            else
            {
                DateTime temp;
                string isDate = dateTimePicker3.Text;
                if (DateTime.TryParse(isDate, out temp) && isDate.CompareTo(currDate) <= 0)
                {
                    if (DateTime.TryParse(isDate, out temp) && isDate.CompareTo(dateFromDtp) >= 0)
                    {
                        retval = true;
                    }
                    else
                    {
                        retval = false;
                        MessageBox.Show("Please select a valid discharge date", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                        dateTimePicker2.Select();
                        return retval;

                    }
                }
                else
                {
                    retval = false;
                    MessageBox.Show("Please select a valid discharge date", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    dateTimePicker2.Select();
                    return retval;

                } 
            }

            

            return retval;
        }

        private void deButton20_Click(object sender, EventArgs e)
        {
            

            if (deTextBox1.Text == "" || deTextBox1.Text == null || String.IsNullOrEmpty(deTextBox1.Text) || String.IsNullOrWhiteSpace(deTextBox1.Text))
            {
                MessageBox.Show("You cannot leave this field blank....", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                deTextBox1.Focus();
                return;
            }
            else
            {
                if (deRadioButton1.Checked == false && deRadioButton2.Checked == false)
                {
                    MessageBox.Show("You have to choose gender....", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    deRadioButton1.Select();
                    return;
                }
                else
                {
                    if (deTextBox2.Text == "" || deTextBox2.Text == null || String.IsNullOrEmpty(deTextBox2.Text) || String.IsNullOrWhiteSpace(deTextBox2.Text))
                    {
                        MessageBox.Show("You cannot leave this field blank....", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        deTextBox2.Focus();
                        return;
                    }
                    else
                    {
                        if (deTextBox3.Text == "" || deTextBox3.Text == null || String.IsNullOrEmpty(deTextBox3.Text) || String.IsNullOrWhiteSpace(deTextBox3.Text))
                        {
                            MessageBox.Show("You cannot leave this field blank....", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            deTextBox3.Focus();
                            return;
                        }
                        else
                        {
                            if (deComboBox9.Text == "" || deComboBox9.Text == null || String.IsNullOrEmpty(deComboBox9.Text) || String.IsNullOrWhiteSpace(deComboBox9.Text))
                            {
                                MessageBox.Show("You have to specify Department name....", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                deComboBox9.Select();
                                return;
                            }
                            else
                            {
                                if (deTextBox4.Text == "" || deTextBox4.Text == null || String.IsNullOrEmpty(deTextBox4.Text) || String.IsNullOrWhiteSpace(deTextBox4.Text))
                                {
                                    MessageBox.Show("You cannot leave this field blank....", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    deTextBox4.Focus();
                                    return;
                                }
                                else
                                {
                                    if (dateTimePicker1.Text == "" || dateTimePicker1.Text == null || String.IsNullOrEmpty(dateTimePicker1.Text) || String.IsNullOrWhiteSpace(dateTimePicker1.Text))
                                    {
                                        MessageBox.Show("You cannot leave this field blank....", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        dateTimePicker1.Select();
                                        return;
                                    }
                                    else
                                    {
                                        if (validate() == true)
                                        { 
                                            if(_mode == DataLayerDefs.Mode._Add)
                                            {
                                                if (validateReg() == true)
                                                {
                                                    bool insertmeta = insertIntoDB();

                                                    if (insertmeta == true)
                                                    {
                                                        MessageBox.Show(this, "Record Saved Successfully...", "NRS Record Management", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                                        deTextBox1.Text = string.Empty;
                                                        deTextBox2.Text = string.Empty;
                                                        deTextBox3.Text = string.Empty;
                                                        deTextBox4.Text = string.Empty;
                                                        dateTimePicker1.CustomFormat = " ";
                                                        dateTimePicker2.CustomFormat = " ";
                                                        dateTimePicker3.CustomFormat = " ";


                                                        fmEntry_Load(sender, e);
                                                    }
                                                    else
                                                    {
                                                        MessageBox.Show(this, "Ooops!!! There is an Error - Record not Saved...", "NRS Record Management", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                                        return;
                                                    }
                                                }
                                            }
                                            else if (_mode == DataLayerDefs.Mode._Edit)
                                            {
                                                if (validateRegforEdit() == true)
                                                {
                                                    bool updatemeta = updateDB(_GetMetaDataDetails(projKey, bundleKey, patientId).Rows[0][2].ToString());
                                                    if (updatemeta == true)
                                                    {
                                                        MessageBox.Show(this, "Record Saved Successfully...", "NRS Record Management", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                                        this.Hide();
                                                        //Files fm = new Files(sqlCon, DataLayerDefs.Mode._Edit);
                                                        //fm.ShowDialog(this);
                                                    }
                                                    else
                                                    {
                                                        MessageBox.Show(this, "Ooops!!! There is an Error - Record not Saved...", "NRS Record Management", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                                        //this.Hide();
                                                        //Files fm = new Files(sqlCon, DataLayerDefs.Mode._Edit);
                                                        //fm.ShowDialog(this);
                                                        return;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                MessageBox.Show(this, "Ooops!!! There is an Error - Record not Saved...", "Record Management", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                                //this.Hide();
                                                return;
                                            }
                                        }

                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        private bool insertIntoDB()
        {
            bool commitBol = true;

            string sqlStr = string.Empty;

            OdbcCommand sqlCmd = new OdbcCommand();


            int item_no = populateMaxCount(projKey, bundleKey) + 1;

            string patient_id = bundleName + Convert.ToString(item_no);
            string gender;
            if (deRadioButton1.Checked == true)
            {
                gender = "Male";
            }
            else
            {
                gender = "Female";
            }
            string deathStat;
            if(checkBox1.Checked == true)
            {
                deathStat = "Y";
            }
            else
            {
                deathStat = "N";
            }


            sqlStr = @"insert into metadata_entry(proj_code,bundle_key,item_no,patient_id,patient_name,guardian_name,gender,address,dep_name,reg_number,admission_date,death,death_date,discharge_date,created_by,created_dttm) values('" +
                       projKey + "','" + bundleKey + "','" + item_no + "','" + patient_id +
                       "','" + deTextBox1.Text.Trim() + "','" + deTextBox2.Text.Trim() + "','" + gender + "','" + deTextBox3.Text.Trim().ToUpper() + "','" + deComboBox9.Text + "','" + deTextBox4.Text.Trim() + "','" + dateTimePicker1.Text + "','" + deathStat + "','" + dateTimePicker2.Text + "','" + dateTimePicker3.Text + "','" + frmMain.name + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
            sqlCmd.Connection = sqlCon;
            //sqlCmd.Transaction = trans;
            sqlCmd.CommandText = sqlStr;
            int j = sqlCmd.ExecuteNonQuery();
            if (j > 0)
            {
                commitBol = true;
            }
            else
            {
                commitBol = false;
            }

            return commitBol;

        }




        private bool updateDB(string item_no)
        {
            bool commitBol = true;

            string sqlStr = string.Empty;

            OdbcCommand sqlCmd = new OdbcCommand();


            string gender;
            if (deRadioButton1.Checked == true)
            {
                gender = "Male";
            }
            else
            {
                gender = "Female";
            }
            string deathStat;
            if (checkBox1.Checked == true)
            {
                deathStat = "Y";
            }
            else
            {
                deathStat = "N";
            }




            sqlStr = @"update metadata_entry set patient_name = '" + deTextBox1.Text.Trim() + "',gender ='" + gender + "',guardian_name='" + deTextBox2.Text.Trim() + "',address ='" + deTextBox3.Text.Trim().ToUpper() + "',dep_name='" + deComboBox9.Text.Trim() + "',reg_number ='" + deTextBox4.Text.Trim() + "',admission_date='" + dateTimePicker1.Text + "',death ='" + deathStat + "',death_date='" + dateTimePicker2.Text + "',discharge_date='" + dateTimePicker3.Text + "',modified_by ='" + frmMain.name + "',modified_dttm = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where proj_code ='" + projKey + "' and bundle_key = '" + bundleKey + "' and patient_id ='" + patientId + "' and item_no ='" + item_no + "' ";
            sqlCmd.Connection = sqlCon;
            //sqlCmd.Transaction = trans;
            sqlCmd.CommandText = sqlStr;
            int j = sqlCmd.ExecuteNonQuery();
            if (j > 0)
            {
                commitBol = true;
            }
            else
            {
                commitBol = false;
            }

            return commitBol;
        }



        private void deComboBox9_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            
            dateTimePicker1.CustomFormat = "yyyy-MM-dd";
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
    
            dateTimePicker2.CustomFormat = "yyyy-MM-dd";
        }

        private void dateTimePicker3_ValueChanged(object sender, EventArgs e)
        {
            
            dateTimePicker3.CustomFormat = "yyyy-MM-dd";
        }

        private void dateTimePicker1_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Back || e.KeyCode == Keys.Delete)
            {
                dateTimePicker1.CustomFormat = " ";
            }
            if(e.KeyCode == Keys.Up || e.KeyCode == Keys.Down || e.KeyCode == Keys.Enter)
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
                checkBox1.Checked = false;
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

        private void fmEntry_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                if (_mode == DataLayerDefs.Mode._Add)
                {
                    DialogResult dr = MessageBox.Show(this, "Do you want to Exit ? ", "Record Management ", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (dr == DialogResult.Yes)
                    {
                        this.Hide();
                        Files fm = new Files(sqlCon, DataLayerDefs.Mode._Add);
                        fm.ShowDialog(this);
                    }
                    else
                    {
                        return;
                    }
                }

                if (_mode == DataLayerDefs.Mode._Edit)
                {
                    DialogResult dr = MessageBox.Show(this, "Do you want to Exit ? ", "Record Management ", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (dr == DialogResult.Yes)
                    {
                        this.Hide();
                        //Files fm = new Files(sqlCon, DataLayerDefs.Mode._Edit);
                        //fm.ShowDialog(this);
                    }
                    else
                    {
                        return;
                    }
                }
            }

            if (e.Control == true && e.KeyCode == Keys.S)
            {
                deButton20_Click(sender, e);
            }
        }
    }
}
