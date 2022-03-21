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
using System.Collections;

namespace ImageHeaven
{
    public partial class frmEntrySummary : Form
    {
        public string name = frmMain.name;
        OdbcConnection sqlCon = null;
        NovaNet.Utils.Credentials crd;
        static wItem wi;
        public static string projKey;
        public static string bundleKey;

        public frmEntrySummary()
        {
            InitializeComponent();
        }

        public frmEntrySummary(OdbcConnection pCon)
        {
            InitializeComponent();

            sqlCon = pCon;

            init();
        }

        private void init()
        {
            DataTable Dt = new DataTable();
            Dt = _GetEntries();

            //Dt.Columns.Add("Number of Files");
            Dt.Columns.Add("No of Entries");

            for (int i = 0; i < Dt.Rows.Count; i++)
            {
                //Dt.Rows[i][4] = _GetFileCount(Dt.Rows[i][0].ToString(), Dt.Rows[i][1].ToString());
                Dt.Rows[i][4] = _GetEntryCount(Dt.Rows[i][0].ToString(),Dt.Rows[i][1].ToString());
            }

            dtGrdVol.DataSource = Dt;
                

            FormatDataGridView();

            this.dtGrdVol.Refresh();
            
            this.textBox2.Text = "";
            this.textBox2.Focus();
        }

        public DataTable _GetEntries()
        {
            DataTable dt = new DataTable();
            string sql = "select distinct proj_code,bundle_key,bundle_name as 'Bundle Name',creation_date as 'Date of Creation' from bundle_master group by proj_code,bundle_key";
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(dt);
            return dt;
        }

        public string _GetFileCount(string proj_code, string bundle_key)
        {
            DataTable dt = new DataTable();
            string sql = "select COUNT(*) from case_file_master where proj_code = '" + proj_code + "' and bundle_key = '" + bundle_key + "' ";
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(dt);
            return dt.Rows[0][0].ToString();
        }

        public string _GetEntryCount(string proj_code, string bundle_key)
        {
            DataTable dt = new DataTable();
            string sql = "select COUNT(*) from metadata_entry where proj_code = '"+proj_code+"' and bundle_key = '"+bundle_key+"' ";
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(dt);
            return dt.Rows[0][0].ToString();
        }

        private void FormatDataGridView()
        {
            //Format the Data Grid View
            dtGrdVol.Columns[0].Visible = false;
            dtGrdVol.Columns[1].Visible = false;
            //dtGrdVol.Columns[2].Visible = false;
            //Format Colors

            
            //Set Autosize on for all the columns
            for (int i = 0; i < dtGrdVol.Columns.Count; i++)
            {
                dtGrdVol.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill; 
            }

            
        }

        private void frmEntrySummary_Load(object sender, EventArgs e)
        {
            
            this.textBox2.AutoCompleteCustomSource = GetSuggestions("bundle_master", "bundle_code");
            this.textBox2.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            this.textBox2.AutoCompleteSource = AutoCompleteSource.CustomSource;

            ArrayList lst = GetTotalDaily(name);
            for (int i = 0; i < lst.Count; i++)
            {
                deLabel1.Text = "Today You Have Entered: " + lst[0].ToString() + " Files";
            }
            
        }

        public ArrayList GetTotalDaily(string name)
        {
            ArrayList totList = new ArrayList();
            string sql = "Select proj_code from metadata_entry where date_format(created_DTTM,'%Y-%m-%d')=date_format(now(),'%Y-%m-%d') and created_by = '" + name + "'";
            //string sql = "Select district_code from deed_details where created_DTTM like now() and created_by = '" + crd.created_by + "'";
            DataSet ds = new DataSet();
            OdbcDataAdapter odap = new OdbcDataAdapter(sql, sqlCon);
            odap.Fill(ds);
            if (ds.Tables.Count > 0)
            { totList.Add(ds.Tables[0].Rows.Count); }
            else { totList.Add("0"); }



            return totList;
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
            //x.Add("Others");
            //x.Add("NA");
            return x;
        }

        public DataTable _GetResult(string est, string bundle_no)
        {
            DataTable dt = new DataTable();
            string sql = "select distinct proj_code,bundle_key,bundle_name as 'Bundle Name',creation_date as 'Date of Creation' from bundle_master where establishment like '%" + est + "%' and bundle_no like '%" + bundle_no + "%' ";
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(dt);
            return dt;
        }
        public DataTable _GetResultEST(string est)
        {
            DataTable dt = new DataTable();
            string sql = "select distinct proj_code,bundle_key,bundle_name as 'Bundle Name',creation_date as 'Date of Creation' from bundle_master where establishment like '%" + est + "%'  ";
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(dt);
            return dt;
        }
        public DataTable _GetResultBundle(string bundle_no)
        {
            DataTable dt = new DataTable();
            string sql = "select distinct proj_code,bundle_key,bundle_name as 'Bundle Name',creation_date as 'Date of Creation' from bundle_master where bundle_code like '%" + bundle_no + "%' ";
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(dt);
            return dt;
        }

        private void cmdSearch_Click(object sender, EventArgs e)
        {
            //string est = textBox1.Text;
            string bundle_no = textBox2.Text;
            
            dtGrdVol.DataSource = null;
            DataTable Dt = new DataTable();

           
            if (bundle_no != null)
            {
                Dt = _GetResultBundle(bundle_no);

                //Dt.Columns.Add("Number of Files");
                Dt.Columns.Add("No of Entries");

                for (int i = 0; i < Dt.Rows.Count; i++)
                {
                    //Dt.Rows[i][4] = _GetFileCount(Dt.Rows[i][0].ToString(), Dt.Rows[i][1].ToString());
                    Dt.Rows[i][4] = _GetEntryCount(Dt.Rows[i][0].ToString(), Dt.Rows[i][1].ToString());
                }

                dtGrdVol.DataSource = Dt;


                FormatDataGridView();

                this.dtGrdVol.Refresh();
                this.textBox2.Focus();
            }
            
            else
            {
                init();
            }
        }

        private void cmdReset_Click(object sender, EventArgs e)
        {
            init();
        }

        private void frmEntrySummary_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                init();
            }
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
            
            if (e.Control == true && e.KeyCode == Keys.N)
            {
                cmdnew_Click(sender, e);
            }
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

        public DataTable _GetBundleStatus(string proj, string bundle)
        {
            DataTable dt = new DataTable();
            string sql = "select distinct status from bundle_master where proj_code = '" + proj + "' and bundle_key = '" + bundle + "' ";
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(dt);
            return dt;
        }

        private void cmdnew_Click(object sender, EventArgs e)
        {

            this.Hide();

            eSTATES[] state = new eSTATES[1];
            state[0] = NovaNet.wfe.eSTATES.METADATA_ENTRY;
            frmBundleSelect frm = new frmBundleSelect(state, sqlCon);
            frm.chkPhotoScan.Visible = false;
            frm.ShowDialog(this);
            
            projKey = frmBundleSelect.projKey;
            bundleKey = frmBundleSelect.bundleKey;


            if (projKey != null && bundleKey != null)
            {
                if (_GetBundleStatus(projKey, bundleKey).Rows[0][0].ToString() == "0")
                {
                    //entry count compare with file count
                    //int fileCount = Convert.ToInt32(_GetFileCount(Convert.ToString(projKey), Convert.ToString(bundleKey)).ToString());
                    int entryCount = Convert.ToInt32(_GetEntryCount(Convert.ToString(projKey), Convert.ToString(bundleKey)).ToString());

                    if (entryCount < 0)
                    {

                            MessageBox.Show(this, "There's no such Bundle...", "Record Management - Entry Check !", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            frmEntrySummary fm = new frmEntrySummary(sqlCon);
                            fm.ShowDialog(this);
                            return;
                        
                    }
                    else
                    {

                        //frmMain.projKey = projKey;
                        //frmMain.bundleKey = bundleKey;

                        Form activeChild = this.ActiveMdiChild;
                        if (activeChild == null)
                        {
                            //EntryForm frmEntry = new EntryForm(sqlCon, DataLayerDefs.Mode._Add);

                            //frmEntry.ShowDialog(this);
                            //Files fm = new Files(sqlCon, DataLayerDefs.Mode._Add);

                            //fm.ShowDialog(this);

                            fmEntry fm = new fmEntry(sqlCon, Convert.ToString(projKey), Convert.ToString(bundleKey),DataLayerDefs.Mode._Add);
                            fm.ShowDialog(this);


                        }

                    }
                }
                else
                {
                    MessageBox.Show(this, "This Bundle has been uploaded for the further process...", "NRS Record Management", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    frmEntrySummary fm = new frmEntrySummary(sqlCon);
                    fm.ShowDialog(this);
                    return;
                }
                
            }
            else
            {
                frmEntrySummary fm = new frmEntrySummary(sqlCon);
                fm.ShowDialog(this);
            }
            
        }

        private void dtGrdVol_DoubleClick(object sender, EventArgs e)
        {
            this.Hide();

            projKey = dtGrdVol.SelectedRows[0].Cells[0].Value.ToString();
            bundleKey = dtGrdVol.SelectedRows[0].Cells[1].Value.ToString();

            if (_GetBundleStatus(projKey, bundleKey).Rows[0][0].ToString() == "0")
            {
                int fileCount = Convert.ToInt32(dtGrdVol.SelectedRows[0].Cells[4].Value.ToString());
                //int entryCount = Convert.ToInt32(dtGrdVol.SelectedRows[0].Cells[5].Value.ToString());
                if (fileCount > 0)
                {
                    

                    if (projKey != null && bundleKey != null)
                    {

                        Form activeChild = this.ActiveMdiChild;
                        if (activeChild == null)
                        {

                            Files fm = new Files(sqlCon, DataLayerDefs.Mode._Edit);

                            fm.ShowDialog(this);
                        }
                    }
                    else
                    {
                        frmEntrySummary fm = new frmEntrySummary(sqlCon);
                        fm.ShowDialog(this);
                    }
                }
                else
                {
                    MessageBox.Show(this, "No file is enterd for this Bundle...", "NRS Record Management - Entry Check !", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    frmEntrySummary fm = new frmEntrySummary(sqlCon);
                    fm.ShowDialog(this);
                    return;
                }
            }
            else
            {
                MessageBox.Show(this, "This Bundle has been uploaded for the further process...", "NRS Record Management", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                frmEntrySummary fm = new frmEntrySummary(sqlCon);
                fm.ShowDialog(this);
                return;
            }
            
        }

        private void dtGrdVol_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Control == true && e.KeyCode == Keys.O)
            {
                dtGrdVol_DoubleClick(sender, e);
            }
        }
    }
}
