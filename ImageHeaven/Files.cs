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
   
    public partial class Files : Form
    {

        public static int index;

        Credentials crd = new Credentials();
        
        string name = frmMain.name;
        OdbcConnection sqlCon = null;
        public static bool _modeBool;
        
        public static DataLayerDefs.Mode _mode = DataLayerDefs.Mode._Edit;

        public static string projKey;
        public static string bundleKey;

        public static string projName;
        public static string bundleName;

        public Files()
        {
            InitializeComponent();
        }

        public DataTable _GetBundleDetails(string proj, string bundle)
        {
            DataTable dt = new DataTable();
            string sql = "select distinct proj_code, bundle_Key,bundle_name as 'Bundle Name' from bundle_master where proj_code = '" + proj + "' and bundle_key = '" + bundle + "' ";
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(dt);
            return dt;
        }

        public DataTable _GetFileCaseDetails(string proj, string bundle)
        {
            DataTable dt = new DataTable();
            string sql = "select distinct proj_code, bundle_Key,item_no,case_file_no,case_status, case_nature, case_type, case_year from case_file_master where proj_code = '" + proj + "' and bundle_key = '" + bundle + "' and case_file_no not in (select case_file_no from metadata_entry where proj_code = '" + proj + "' and bundle_key = '" + bundle + "')";
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(dt);
            return dt;
        }

        public DataTable _GetFileCaseInDetails(string proj, string bundle)
        {
            DataTable dt = new DataTable();
            string sql = "select distinct proj_code, bundle_Key,item_no,case_file_no,case_status, case_nature, case_type, case_year from case_file_master where proj_code = '" + proj + "' and bundle_key = '" + bundle + "' and case_file_no in (select case_file_no from metadata_entry where proj_code = '" + proj + "' and bundle_key = '" + bundle + "')";
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(dt);
            return dt;
        }

        public DataTable _GetFileCaseDetailsIndividual(string proj, string bundle, string casefileno)
        {
            DataTable dt = new DataTable();
            string sql = "select distinct proj_code, bundle_Key,item_no,case_file_no,case_status, case_nature, case_type, case_year from case_file_master where proj_code = '" + proj + "' and bundle_key = '" + bundle + "' and case_file_no = '"+casefileno+"'";
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(dt);
            return dt;
        }

        public DataTable _GetMetaDataDetailsIndividual(string proj, string bundle, string patientID)
        {
            DataTable dt = new DataTable();
            string sql = "select distinct proj_code, bundle_Key,patient_name,gender, reg_number, dep_name, admission_date,death,death_date,discharge_date,guardian_name,address from metadata_entry where proj_code = '" + proj + "' and bundle_key = '" + bundle + "' and patient_id = '"+patientID+"'";
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(dt);
            return dt;
        }

        public DataTable _GetMetaDataDetails(string proj, string bundle)
        {
            DataTable dt = new DataTable();
            string sql = "select distinct proj_code, bundle_Key,item_no,patient_id,patient_name,gender, reg_number, dep_name, admission_date,death,death_date,discharge_date,guardian_name,address from metadata_entry where proj_code = '" + proj + "' and bundle_key = '" + bundle + "'";
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(dt);
            return dt;
        } 


        public Files(OdbcConnection pCon, DataLayerDefs.Mode mode)
        {
            InitializeComponent();
            sqlCon = pCon;

            if (mode == DataLayerDefs.Mode._Add)
            {
                projKey = frmEntrySummary.projKey;
                bundleKey = frmEntrySummary.bundleKey;

                deLabel3.Text = _GetBundleDetails(projKey, bundleKey).Rows[0][2].ToString();

                int count = _GetMetaDataDetails(projKey, bundleKey).Rows.Count;

                for (int i = 0; i < count; i++)
                {

                    string patientid = _GetMetaDataDetails(projKey, bundleKey).Rows[i][3].ToString();
                    

                    //add row
                    string[] row = { patientid };
                    var listItem = new ListViewItem(row);

                    lstDeeds.Items.Add(listItem);
                }
                if (lstDeeds.Items.Count == 0)
                {
                    MessageBox.Show(this, "No More patient present for this bundle ...", "NRS Record Management !", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    frmEntrySummary fm = new frmEntrySummary(sqlCon);
                    fm.ShowDialog(this);
                }
                _mode = mode;
            }

            if (mode == DataLayerDefs.Mode._Edit)
            {
                projKey = frmEntrySummary.projKey;
                bundleKey = frmEntrySummary.bundleKey;

                deLabel3.Text = _GetBundleDetails(projKey, bundleKey).Rows[0][2].ToString();

                int count = _GetMetaDataDetails(projKey, bundleKey).Rows.Count;

                for (int i = 0; i < count; i++)
                {

                    string patientid = _GetMetaDataDetails(projKey, bundleKey).Rows[i][3].ToString();


                    //add row
                    string[] row = { patientid };
                    var listItem = new ListViewItem(row);

                    lstDeeds.Items.Add(listItem);
                }

                if(lstDeeds.Items.Count == 0)
                {
                    MessageBox.Show(this, "No More patient present for this bundle ...", "NRS Record Management !", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    frmEntrySummary fm = new frmEntrySummary(sqlCon);
                    fm.ShowDialog(this);
                }

                _mode = mode;
            }
        }

        

        private void Files_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Hide();

                frmEntrySummary fm = new frmEntrySummary(sqlCon);
                fm.ShowDialog(this);
            }
            
        }

        public AutoCompleteStringCollection GetSuggestions(string tblName, string fldName, string projKey, string bundleKey)
        {
            AutoCompleteStringCollection x = new AutoCompleteStringCollection();
            string sql = "Select distinct " + fldName + " from " + tblName + " where proj_code = '"+projKey+"' AND bundle_key = '"+bundleKey+"'";
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

        private void formatForm()
        {

            this.deTextBox1.AutoCompleteCustomSource = GetSuggestions("metadata_entry", "patient_id", projKey, bundleKey);
            this.deTextBox1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            this.deTextBox1.AutoCompleteSource = AutoCompleteSource.CustomSource;

        }

        private void formatEntryForm()
        {

            this.deTextBox1.AutoCompleteCustomSource = GetSuggestions("metadata_entry", "patient_id", projKey, bundleKey);
            this.deTextBox1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            this.deTextBox1.AutoCompleteSource = AutoCompleteSource.CustomSource;

        }

        private void Files_Load(object sender, EventArgs e)
        {
            if (_mode == DataLayerDefs.Mode._Add)
            {
                formatForm();
            }
            if (_mode == DataLayerDefs.Mode._Edit)
            {
                formatEntryForm();
            }
        }

        private void cmdSearch_Click(object sender, EventArgs e)
        {
            string text = deTextBox1.Text;

            for (int i = 0; i < lstDeeds.Items.Count; i++)
            {
                if (lstDeeds.Items[i].Text.Equals(text))
                {
                    
                    lstDeeds.Items[i].Selected = true;
                    lstDeeds.Items[i].Focused = true;
                    lstDeeds.Select();
                    lstDeeds.Items[i].EnsureVisible();
                    return;
                }
                else
                {
                    lstDeeds.Items[i].Selected = false;
                }

            }
           
        }

        private void lstDeeds_SelectedIndexChanged(object sender, EventArgs e)
        {
            //index = lstDeeds.FocusedItem.Index;


            if (lstDeeds.SelectedItems.Count > 0)
            {
                string patientid = lstDeeds.SelectedItems[0].SubItems[0].Text;


                string patient_name = _GetMetaDataDetailsIndividual(projKey, bundleKey, patientid).Rows[0][2].ToString();
                string gender = _GetMetaDataDetailsIndividual(projKey, bundleKey, patientid).Rows[0][3].ToString();
                string reg_numb = _GetMetaDataDetailsIndividual(projKey, bundleKey, patientid).Rows[0][4].ToString();
                string dept = _GetMetaDataDetailsIndividual(projKey, bundleKey, patientid).Rows[0][5].ToString();
                string admission_date = _GetMetaDataDetailsIndividual(projKey, bundleKey, patientid).Rows[0][6].ToString();
                string gurName = _GetMetaDataDetailsIndividual(projKey, bundleKey, patientid).Rows[0][10].ToString();
                string address = _GetMetaDataDetailsIndividual(projKey, bundleKey, patientid).Rows[0][11].ToString();
                string deathstat = _GetMetaDataDetailsIndividual(projKey, bundleKey, patientid).Rows[0][7].ToString();
                string deathdate = _GetMetaDataDetailsIndividual(projKey, bundleKey, patientid).Rows[0][8].ToString();
                string dis_date = _GetMetaDataDetailsIndividual(projKey, bundleKey, patientid).Rows[0][9].ToString();


                string remarks = "";

                remarks = "Patient ID : " + patientid +"\rRegistration Number :"+reg_numb+ "\n\nPatient Name : " + patient_name +"\nGender : "+gender+ "\nGurdian Name : "+gurName+"\nAddress : "+address;

                if(deathstat == "Y")
                {
                    remarks = remarks + "\n\nDeath Date : "+deathdate;
                }

                if(!string.IsNullOrWhiteSpace(dis_date))
                {
                    remarks = remarks + "\n\nDischarge Date : " + dis_date;
                }

                fileRemarks.Text = remarks;
            }
            else
            {
                fileRemarks.Text = "";
            }
            

        }

        public string _GetProject(string proj)
        {
            DataTable dt = new DataTable();
            string sql = "select distinct proj_key, proj_code from project_master where proj_key = '" + proj + "' ";
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(dt);
            return dt.Rows[0][1].ToString();
        }

        public string _GetBundle(string proj, string bundle)
        {
            DataTable dt = new DataTable();
            string sql = "select distinct proj_code,bundle_key,bundle_code from bundle_master where proj_code = '" + proj + "' and bundle_key = '"+bundle+"' ";
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(dt);
            return dt.Rows[0][2].ToString();
        }

        private void lstDeeds_DoubleClick(object sender, EventArgs e)
        {
            if(lstDeeds.SelectedItems.Count > 0)
            {
                index = lstDeeds.FocusedItem.Index;

                string patientID = lstDeeds.Items[index].SubItems[0].Text;

                if(_mode == DataLayerDefs.Mode._Add)
                {
                    this.Hide();
                    //EntryForm frm = new EntryForm(sqlCon, _mode, patientID);
                    //frm.ShowDialog(this);

                    projName = _GetProject(projKey).ToString();
                    bundleName = _GetBundle(projKey,bundleKey).ToString();

                    fmEntry fm = new fmEntry(sqlCon, projKey, bundleKey, _mode, patientID);
                    fm.ShowDialog(this);
                }

                if (_mode == DataLayerDefs.Mode._Edit)
                {
                    this.Hide();
                    //EntryForm frm = new EntryForm(sqlCon, _mode, patientID);
                    //frm.ShowDialog(this);

                    projName = _GetProject(projKey).ToString();
                    bundleName = _GetBundle(projKey, bundleKey).ToString();

                    fmEntry fm = new fmEntry(sqlCon, projKey, bundleKey, _mode, patientID);
                    fm.ShowDialog(this);
                }
            }
            
        }

        private void lstDeeds_KeyUp(object sender, KeyEventArgs e)
        {
            if (lstDeeds.Items.Count > 0)
            {
                if (lstDeeds.SelectedItems.Count > 0)
                {
                    if (e.Control == true && e.KeyCode == Keys.O)
                    {
                        lstDeeds_DoubleClick(sender, e);
                    }
                }
            }
        }
    }
}
