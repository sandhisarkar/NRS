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
    public partial class frmBatchUpload : Form
    {
        string name = frmMain.name;
        OdbcConnection sqlCon = null;
        
        private OdbcDataAdapter sqlAdap = null;
        public static string projName;
        public static string batchName;
        public static string projKey;
        public static string batchKey;

        public frmBatchUpload()
        {
            InitializeComponent();
        }

        public frmBatchUpload(OdbcConnection pCon)
        {
            InitializeComponent();

            sqlCon = pCon;
        }

        private void frmBatchUpload_Load(object sender, EventArgs e)
        {
            populateProject();
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

            string sql = "select a.batch_key, a.batch_name from batch_master a, project_master b where a.proj_code = b.proj_key and a.status = '0' and a.proj_code = '" + cmbProject.SelectedValue.ToString() + "'";

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
                bool updatebatch = updateBatch();
                bool updatemeta = updateMeta();
                if (updatemeta == true && updatebatch == true)
                {
                    MessageBox.Show(this, "Batch is Successfully Uploaded...", " ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                else
                {
                    MessageBox.Show(this, "Ooops!!! There is an Error - Batch isn't Uploaded...", " ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public bool updateBatch()
        {
            bool ret = false;
            if (ret == false)
            {
                _UpdateBatch();

                ret = true;
            }
            return ret;
        }
        
        public bool updateMeta()
        {
            bool ret = false;
            if (ret == false)
            {
                _UpdateMeta();

                ret = true;
            }
            return ret;
        }

        public bool _UpdateBatch()
        {
            bool retVal = false;
            string sql = string.Empty;


            
            sql = "UPDATE batch_master SET status = '1' WHERE proj_code = '" + projKey + "' AND batch_key = '" + batchKey + "'";
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


            sql = "UPDATE metadata_entry SET status = 'Y' WHERE proj_key = '" + projKey + "' AND batch_key = '" + batchKey + "'";
            System.Diagnostics.Debug.Print(sql);
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            if (cmd.ExecuteNonQuery() > 0)
            {
                retVal = true;
            }


            return retVal;
        }

        
    }
}
