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
    public partial class frmBundleSummary : Form
    {
        public string name = frmMain.name;
        OdbcConnection sqlCon = null;
        NovaNet.Utils.Credentials crd;
        static wItem wi;
        public static int projKey;
        public static int bundleKey;

        public frmBundleSummary()
        {
            InitializeComponent();
        }

        public frmBundleSummary(OdbcConnection pCon)
        {
            InitializeComponent();

            sqlCon = pCon;

            init();
        }

        private void init()
        {
            DataTable Dt = new DataTable();
            Dt = _GetBundles();

            Dt.Columns.Add("Number of Files");

            for (int i = 0; i < Dt.Rows.Count; i++)
            {
                Dt.Rows[i][6] = _GetFileCount(Dt.Rows[i][0].ToString(), Dt.Rows[i][1].ToString());
            }
            dtGrdVol.DataSource = Dt;

            FormatDataGridView();

            this.dtGrdVol.Refresh();
            this.deTextBox1.Text = "";
            this.deTextBox1.Focus();
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

        public DataTable _GetBundles()
        {
            DataTable dt = new DataTable();
            string sql = "select distinct proj_code,bundle_Key,establishment as Establishment, bundle_name as 'Bundle Name', Bundle_no as 'Bundle Number',date_format(handover_date,'%Y-%m-%d') as 'Handover Date' from bundle_master group by proj_code, bundle_key";
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(dt);
            return dt;
        }

        private void frmBundleSummary_Load(object sender, EventArgs e)
        {
            ArrayList lst = GetTotalDaily(name);
            for (int i = 0; i < lst.Count; i++)
            {
                deLabel2.Text = "Today You Have Done: " + lst[0].ToString() + " Bundles";
            }
            deLabel3.Text = "Total Number of Bundles: " + GetTotal();
        }

        public ArrayList GetTotalDaily(string name)
        {
            ArrayList totList = new ArrayList();
            string sql = "Select bundle_name from bundle_master where date_format(created_DTTM,'%Y-%m-%d')=date_format(now(),'%Y-%m-%d') and created_by = '" + name + "'";
            //string sql = "Select district_code from deed_details where created_DTTM like now() and created_by = '" + crd.created_by + "'";
            DataSet ds = new DataSet();
            OdbcDataAdapter odap = new OdbcDataAdapter(sql, sqlCon);
            odap.Fill(ds);
            if (ds.Tables.Count > 0)
            { totList.Add(ds.Tables[0].Rows.Count); }
            else { totList.Add("0"); }



            return totList;
        }

        public string GetTotal()
        {
            string sql = "Select Count(*) from bundle_master";
            //string sql = "Select district_code from deed_details where created_DTTM like now() and created_by = '" + crd.created_by + "'";
            DataSet ds = new DataSet();
            OdbcDataAdapter odap = new OdbcDataAdapter(sql, sqlCon);
            odap.Fill(ds);
            return ds.Tables[0].Rows[0][0].ToString();
        }

        private void FormatDataGridView()
        {
            //Format the Data Grid View
            dtGrdVol.Columns[0].Visible = false;
            dtGrdVol.Columns[1].Visible = false;
            //Format Colors

            //Set Autosize on for all the columns
            for (int i = 0; i < dtGrdVol.Columns.Count; i++)
            {
                dtGrdVol.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
        }

        private void cmdReset_Click(object sender, EventArgs e)
        {
            init();
        }

        private void cmdAbort_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmBundleSummary_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
            if (e.Control == true && e.KeyCode == Keys.N)
            {
                this.Hide();
                //frmBatch fm = new frmBatch();
                //fm.ShowDialog();
                try
                {
                    frmBatch dispProject;
                    wi = new wfeBatch(sqlCon);
                    dispProject = new frmBatch(wi, sqlCon);
                    dispProject.ShowDialog(this);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            if (e.Control == true && e.KeyCode == Keys.O)
            {
                dtGrdVol_DoubleClick(sender, e);
            }
        }

        private void frmBundleSummary_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
            if (e.Control == true && e.KeyCode == Keys.N)
            {
                this.Hide();
                //frmBatch fm = new frmBatch();
                //fm.ShowDialog();
                try
                {
                    frmBatch dispProject;
                    wi = new wfeBatch(sqlCon);
                    dispProject = new frmBatch(wi, sqlCon);
                    dispProject.ShowDialog(this);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            if (e.Control == true && e.KeyCode == Keys.O)
            {
                dtGrdVol_DoubleClick(sender, e);
            }
        }

        private void deTextBox1_KeyUp(object sender, KeyEventArgs e)
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
                this.Hide();
                //frmBatch fm = new frmBatch();
                //fm.ShowDialog();
                try
                {
                    frmBatch dispProject;
                    wi = new wfeBatch(sqlCon);
                    dispProject = new frmBatch(wi, sqlCon);
                    dispProject.ShowDialog(this);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            if (e.Control == true && e.KeyCode == Keys.O)
            {
                dtGrdVol_DoubleClick(sender, e);
            }
        }

        private void cmdSearch_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                init();
            }
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
            if(e.Control == true && e.KeyCode == Keys.N)
            {
                this.Hide();
                //frmBatch fm = new frmBatch();
                //fm.ShowDialog();
                try
                {
                    frmBatch dispProject;
                    wi = new wfeBatch(sqlCon);
                    dispProject = new frmBatch(wi, sqlCon);
                    dispProject.ShowDialog(this);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            
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

        private void dtGrdVol_KeyUp(object sender, KeyEventArgs e)
        {
            
            if (e.KeyCode == Keys.F5)
            {
                init();
            }
                if(e.KeyCode == Keys.F2)
            {
                dtGrdVol_DoubleClick(sender, e);
            }
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
            if(e.Control == true && e.KeyCode == Keys.N)
            {
                this.Hide();
                //frmBatch fm = new frmBatch();
                //fm.ShowDialog();
                try
                {
                    frmBatch dispProject;
                    wi = new wfeBatch(sqlCon);
                    dispProject = new frmBatch(wi, sqlCon);
                    dispProject.ShowDialog(this);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            if (e.Control == true && e.KeyCode == Keys.O)
            {
                dtGrdVol_DoubleClick(sender, e);
            }
        }

        private void cmdAbort_KeyUp(object sender, KeyEventArgs e)
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
                this.Hide();
                
                try
                {
                    frmBatch dispProject;
                    wi = new wfeBatch(sqlCon);
                    dispProject = new frmBatch(wi, sqlCon);
                    dispProject.ShowDialog(this);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            
        }

        private void cmdSearch_Click(object sender, EventArgs e)
        {
            if(deTextBox1.Text != "")
            {
                dtGrdVol.DataSource = null;

                DataTable dt = new DataTable();
                dt = _GetBundle(deTextBox1.Text);

                dt.Columns.Add("Number of Files");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i][6] = _GetFileCount(dt.Rows[i][0].ToString(), dt.Rows[i][1].ToString());
                }
                dtGrdVol.DataSource = dt;

                FormatDataGridView();

                this.dtGrdVol.Refresh();
                //this.deTextBox1.Text = "";
                this.deTextBox1.Focus();
            }
        }

       
        public DataTable _GetBundle(string bundle_no)
        {
            DataTable dt = new DataTable();
            string sql = "select distinct proj_code, bundle_Key,establishment as Establishment, bundle_name as 'Bundle Name', Bundle_no as 'Bundle Number', date_format(handover_date,'%Y-%m-%d') as 'Handover Date' from bundle_master  where bundle_no = '" + bundle_no.Trim() + "' group by proj_code, bundle_key";
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(dt);
            return dt;
        }

        private void cmdnew_Click(object sender, EventArgs e)
        {
            this.Hide();
            //frmBatch fm = new frmBatch();
            //fm.ShowDialog();
            try
            {
                frmBatch dispProject;
                wi = new wfeBatch(sqlCon);
                dispProject = new frmBatch(wi, sqlCon);
                dispProject.ShowDialog(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cmdnew_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
            if (e.Control == true && e.KeyCode == Keys.O)
            {
                dtGrdVol_DoubleClick(sender, e);
            }
        }

        public string _GetEntryCount(string proj_code, string bundle_key)
        {
            DataTable dt = new DataTable();
            string sql = "select COUNT(*) from metadata_entry where proj_code = '" + proj_code + "' and bundle_key = '" + bundle_key + "' ";
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(dt);
            return dt.Rows[0][0].ToString();
        }

        

        private void dtGrdVol_DoubleClick(object sender, EventArgs e)
        {
            projKey = Convert.ToInt32(dtGrdVol.SelectedRows[0].Cells[0].Value.ToString());
            bundleKey = Convert.ToInt32(dtGrdVol.SelectedRows[0].Cells[1].Value.ToString());

            if (_GetBundleStatus(Convert.ToString(projKey), Convert.ToString(bundleKey)).Rows[0][0].ToString() == "0")
            {
                if (Convert.ToInt32(_GetEntryCount(Convert.ToString(projKey), Convert.ToString(bundleKey))) == 0)
                {
                    frmCaseFile fm = new frmCaseFile(sqlCon);

                    this.Hide();

                    fm.ShowDialog(this);
                }
                else
                {
                    MessageBox.Show("Metadata already added for this Bundle...", "Record Management", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    deTextBox1.Focus();
                    return;
                }
            }
            else
            {
                MessageBox.Show("This Bundle have already uploaded...", "Record Management", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                deTextBox1.Focus();
                return;
            }

        }

       
    }
}
