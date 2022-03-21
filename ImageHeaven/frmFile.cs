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

namespace ImageHeaven
{
    public partial class frmFile : Form
    {
        string name = frmMain.name;
        OdbcConnection sqlCon = null;
        public static int flag;
        public static string proj;
        public static string batch;
        

        public frmFile()
        {
            InitializeComponent();
        }

        public frmFile(OdbcConnection pCon)
        {
            InitializeComponent();

            sqlCon = pCon;
        }


        public void dashlist()
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            string sql = "SELECT a.proj_code AS 'Project Name',b.batch_code AS 'Batch Name', COUNT(c.`batch_key`) AS 'Count' FROM project_master a,batch_master b,metadata_entry c WHERE a.proj_key = b.proj_code AND b.proj_code = c.proj_key AND b.batch_key = c.batch_key AND c.proj_key = a.proj_key GROUP BY c.`batch_key` ";

            OdbcDataAdapter odap = new OdbcDataAdapter(sql, sqlCon);
            odap.Fill(dt);


            if (dt.Rows.Count > 0)
            {

                dgvDash.DataSource = dt;

            }
            else
            {
                dgvDash.DataSource = null;
            }
        }

        private void frmFile_Load(object sender, EventArgs e)
        {
            dashlist();

            this.txtProj.AutoCompleteCustomSource = GetSuggestions("project_master", "proj_code");
            this.txtProj.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            this.txtProj.AutoCompleteSource = AutoCompleteSource.CustomSource;

            this.txtBatch.AutoCompleteCustomSource = GetSuggestions("batch_master", "batch_code");
            this.txtBatch.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            this.txtBatch.AutoCompleteSource = AutoCompleteSource.CustomSource;


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
        private void frmFile_KeyPress(object sender, KeyPressEventArgs e)
        {
            
        }

        private void frmFile_KeyUp(object sender, KeyEventArgs e)
        {
            
        }

        private void frmFile_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
            if (e.KeyCode == Keys.N && e.Modifiers == Keys.Control)
            {
                this.Hide();
                flag = 0;
                frmBatchSelect frm = new frmBatchSelect(sqlCon);
                frm.ShowDialog(this);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnNewEntry_Click(object sender, EventArgs e)
        {
            this.Hide();
            flag = 0;
            frmBatchSelect frm = new frmBatchSelect(sqlCon);
            frm.ShowDialog(this);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string proj_name = txtProj.Text;
            string batch_name = txtBatch.Text;
            if ((txtBatch.Text != "") && (txtProj.Text != ""))
            {
                dashlist(proj_name, batch_name);
            }
            if ((txtBatch.Text == "") && (txtProj.Text == ""))
            {
                dashlist();
            }
            if ((txtBatch.Text != "") || (txtProj.Text != ""))
            {

                if (txtProj.Text != "")
                {
                    dashlist(proj_name);
                }
                if(txtBatch.Text != "")
                {
                    dash(batch_name);
                }
            }
            
        }
        public void dashlist(string proj)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            string sql = "SELECT a.proj_code AS 'Project Name',b.batch_code AS 'Batch Name', COUNT(c.`batch_key`) AS 'Count' FROM project_master a,batch_master b,metadata_entry c WHERE a.proj_key = b.proj_code AND b.proj_code = c.proj_key AND b.batch_key = c.batch_key AND c.proj_key = a.proj_key and a.proj_code = '" + proj + "' GROUP BY c.`batch_key`,c.proj_key ";

            OdbcDataAdapter odap = new OdbcDataAdapter(sql, sqlCon);
            odap.Fill(dt);


            if (dt.Rows.Count > 0)
            {

                dgvDash.DataSource = dt;

            }
            else
            {
                dgvDash.DataSource = null;
            }
        }
        public void dash(string batch)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            string sql = "SELECT a.proj_code AS 'Project Name',b.batch_code AS 'Batch Name', COUNT(c.`batch_key`) AS 'Count' FROM project_master a,batch_master b,metadata_entry c WHERE a.proj_key = b.proj_code AND b.proj_code = c.proj_key AND b.batch_key = c.batch_key AND c.proj_key = a.proj_key and b.batch_code = '" + batch + "' GROUP BY c.`batch_key`,c.proj_key ";

            OdbcDataAdapter odap = new OdbcDataAdapter(sql, sqlCon);
            odap.Fill(dt);


            if (dt.Rows.Count > 0)
            {

                dgvDash.DataSource = dt;

            }
            else
            {
                dgvDash.DataSource = null;
            }
        }
        public void dashlist(string proj, string batch)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            string sql = "SELECT a.proj_code AS 'Project Name',b.batch_code AS 'Batch Name', COUNT(c.`batch_key`) AS 'Count' FROM project_master a,batch_master b,metadata_entry c WHERE a.proj_key = b.proj_code AND b.proj_code = c.proj_key AND b.batch_key = c.batch_key AND c.proj_key = a.proj_key and a.proj_code = '" + proj + "' and b.batch_code = '" + batch + "' GROUP BY c.`batch_key`,c.proj_key ";

            OdbcDataAdapter odap = new OdbcDataAdapter(sql, sqlCon);
            odap.Fill(dt);


            if (dt.Rows.Count > 0)
            {

                dgvDash.DataSource = dt;

            }
            else
            {
                dgvDash.DataSource = null;
            }
        }

        private void btnRef_Click(object sender, EventArgs e)
        {
            txtProj.Text = string.Empty;
            txtBatch.Text = string.Empty;
            dashlist();
        }

        private void dgvDash_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //this.Hide();
            proj = dgvDash.SelectedRows[0].Cells[0].Value.ToString();
            batch = dgvDash.SelectedRows[0].Cells[1].Value.ToString();
            frmFileSum frm = new frmFileSum(sqlCon);
            frm.ShowDialog();
        }

        private void dgvDash_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //this.Hide();
            proj = dgvDash.SelectedRows[0].Cells[0].Value.ToString();
            batch = dgvDash.SelectedRows[0].Cells[1].Value.ToString();
            frmFileSum frm = new frmFileSum(sqlCon);
            frm.ShowDialog();
        }

        private void dgvDash_DoubleClick(object sender, EventArgs e)
        {
            //this.Hide();
            proj = dgvDash.SelectedRows[0].Cells[0].Value.ToString();
            batch = dgvDash.SelectedRows[0].Cells[1].Value.ToString();
            frmFileSum frm = new frmFileSum(sqlCon);
            frm.ShowDialog();
        }
        
    }
}
