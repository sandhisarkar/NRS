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
    public partial class frmFileSum : Form
    {
        string name = frmMain.name;
        OdbcConnection sqlCon = null;
        public static string proj_name;
        public static string batch_name;
        public static string filename;

        public frmFileSum()
        {
            InitializeComponent();
        }

        public frmFileSum(OdbcConnection pCon)
        {
            InitializeComponent();

            sqlCon = pCon;
        }

        public void fileList(string projname, string batchname)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            string sql = "SELECT c.letter_no as 'Letter No',c.issue_from as 'Issued From',c.issue_to as 'Issued To',c.issue_date as 'Issued Date',c.`filename` AS 'File Name' FROM project_master a,batch_master b,metadata_entry c WHERE a.proj_key = b.proj_code AND b.proj_code = c.proj_key AND b.batch_key = c.batch_key AND c.proj_key = a.proj_key AND a.Proj_code = '"+projname+"' AND b.batch_name = '"+batchname+"' ORDER BY c.item_no";

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

        private void frmFileSum_Load(object sender, EventArgs e)
        {
            txtProject.Text = frmFile.proj;
            txtBatch.Text = frmFile.batch;
            fileList(frmFile.proj,frmFile.batch);
        }

        private void frmFileSum_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
            if (e.KeyCode == Keys.F5)
            {
                fileList(frmFile.proj, frmFile.batch);
            }
        }

        private void dgvDash_DoubleClick(object sender, EventArgs e)
        {
            //this.Hide();
            proj_name = txtProject.Text;
            batch_name = txtBatch.Text;
            filename = dgvDash.SelectedRows[0].Cells[4].Value.ToString();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            string sql = "SELECT c.status,b.status FROM project_master a,batch_master b,metadata_entry c WHERE a.proj_key = b.proj_code AND b.proj_code = c.proj_key AND b.batch_key = c.batch_key AND c.proj_key = a.proj_key AND a.Proj_code = '" + proj_name + "' AND b.batch_name = '" + batch_name + "' and c.filename = '"+filename+"'";

            OdbcDataAdapter odap = new OdbcDataAdapter(sql, sqlCon);
            odap.Fill(dt);

            if (dt.Rows[0][0].ToString() == "N" && dt.Rows[0][1].ToString() == "0")
            {
                frmEntry frm = new frmEntry(sqlCon, DataLayerDefs.Mode._Edit);
                frm.ShowDialog();
            }
            else
            {
                MessageBox.Show(this, "Batch isn't for Ready for Metadata Entry", "You cannot open this Batch",MessageBoxButtons.OK,MessageBoxIcon.Stop);
            }
            
        }
    }
}
