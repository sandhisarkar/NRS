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
    public partial class frmBatchSelect : Form
    {
        string name = frmMain.name;
        OdbcConnection sqlCon = null;

        public static string projName;
        public static string batchName;
        public static string projKey;
        public static string batchKey;
        public static int stat= frmFile.flag;

        public frmBatchSelect()
        {
            InitializeComponent();
        }

        public frmBatchSelect(OdbcConnection pCon)
        {
            InitializeComponent();

            sqlCon = pCon;
        }

        private void frmBatchSelect_Load(object sender, EventArgs e)
        {
            populateProject();
            

            button1.Enabled = false;
        }

        private void populateProject()
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            string sql = "select proj_key, proj_code from project_master  ";

            OdbcDataAdapter odap = new OdbcDataAdapter(sql, sqlCon);
            odap.Fill(dt);


            if (dt.Rows.Count > 0)
            {
                cmbProject.DataSource = dt;
                cmbProject.DisplayMember = "proj_code";
                cmbProject.ValueMember = "proj_key";

                populateBatch();
            }
            else
            {
                cmbProject.DataSource = null;
               // cmbProject.Text = "";
                MessageBox.Show("Add one project first...");
                this.Close();
            }

        }

        private void populateBatch()
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            string sql = "select a.batch_key, a.batch_name from batch_master a, project_master b where a.proj_code = b.proj_key and a.status = '0' and a.proj_code = '"+cmbProject.SelectedValue.ToString()+"'";

            OdbcDataAdapter odap = new OdbcDataAdapter(sql, sqlCon);
            odap.Fill(dt);


            if (dt.Rows.Count > 0)
            {
                cmbBatch.DataSource = dt;
                cmbBatch.DisplayMember = "batch_name";
                cmbBatch.ValueMember = "batch_key";
                button1.Enabled = true;
            }
            else
            {
                //cmbBatch.Text = "";
                //MessageBox.Show("No Batch found for this project...","Add Batch");
                cmbBatch.Text = string.Empty;
                cmbBatch.DataSource = null;
                cmbBatch.DisplayMember = "";
                cmbBatch.ValueMember = "";
                cmbProject.Select();
                button1.Enabled = false;
            }

        }

        private void cmbProject_Leave(object sender, EventArgs e)
        {
            populateBatch();
        }

        private void cmbProject_MouseLeave(object sender, EventArgs e)
        {
            populateBatch();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if ((cmbProject.Text == "" || cmbProject.Text == null) && (cmbBatch.Text == "" || cmbBatch.Text == null))
            {
                MessageBox.Show("Please select proper Project and Batch...");
                cmbProject.Focus();
                cmbProject.Select();
            }
            else
            {
                projName = cmbProject.Text;
                projKey = cmbProject.SelectedValue.ToString();
                batchName = cmbBatch.Text;
                batchKey = cmbBatch.SelectedValue.ToString();

                //this.Hide();

                DataSet ds = new DataSet();
                DataTable dt = new DataTable();

                string sql = "SELECT b.status FROM project_master a,batch_master b WHERE a.proj_key = b.proj_code AND a.Proj_code = '" + projName + "' AND b.batch_name = '" + batchName + "' and b.batch_key = '"+batchKey+"' and b.proj_code = '"+projKey+"'";

                OdbcDataAdapter odap = new OdbcDataAdapter(sql, sqlCon);
                odap.Fill(dt);

                if (dt.Rows[0][0].ToString() == "0")
                {
                    frmEntry frm = new frmEntry(sqlCon, DataLayerDefs.Mode._Add);
                    frm.ShowDialog(this);
                }
                else
                {
                    MessageBox.Show(this, "Batch isn't for Ready for Metadata Entry", "You cannot open this Batch", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            }
        }
    }
}
