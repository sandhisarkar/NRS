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
    public partial class frmBundleUpload : Form
    {
        NovaNet.Utils.dbCon dbcon;
        OdbcConnection sqlCon = null;
        private OdbcDataAdapter sqlAdap = null;
        eSTATES[] state;
        DataSet dsdeed = null;
        public static string projKey;
        public static string bundleKey;

        public frmBundleUpload()
        {
            InitializeComponent();
        }

        public frmBundleUpload(OdbcConnection prmCon)
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
            sqlCon = prmCon;
			this.Text = "Record Management - Bundle Upload";
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}

        private void frmBundleUpload_Load(object sender, EventArgs e)
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

                populateBundle();
            }
            else
            {
                cmbProject.DataSource = null;
                // cmbProject.Text = "";
                MessageBox.Show("Add one project first...");
                this.Close();
            }


        }

        private void populateBundle()
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            string sql = "select a.bundle_key, a.bundle_code from bundle_master a, project_master b where a.proj_code = b.proj_key and a.proj_code = '" + cmbProject.SelectedValue.ToString() + "' and a.status = '0'";

            OdbcDataAdapter odap = new OdbcDataAdapter(sql, sqlCon);
            odap.Fill(dt);


            if (dt.Rows.Count > 0)
            {
                cmbBundle.DataSource = dt;
                cmbBundle.DisplayMember = "bundle_code";
                cmbBundle.ValueMember = "bundle_key";
            }
            else
            {

                cmbBundle.Text = string.Empty;
                cmbBundle.DataSource = null;
                cmbBundle.DisplayMember = "";
                cmbBundle.ValueMember = "";
                cmbProject.Select();

            }
        }

        private DataSet ReadDatabase()
        {
            DataSet ds = new DataSet();
            dsdeed = new DataSet();
            try
            {

                string sql = "select * from metadata_entry where proj_code = '" + cmbProject.SelectedValue.ToString() + "' and bundle_key = '" + cmbBundle.SelectedValue.ToString() + "' ";


                OdbcDataAdapter odap = new OdbcDataAdapter(sql, sqlCon);
                odap.Fill(ds);
                odap.Fill(dsdeed);

            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message.ToString());
                statusStrip1.Text = ex.Message.ToString();
            }
            return ds;
        }

        private void cmdsearch_Click(object sender, EventArgs e)
        {
            grdCsv.DataSource = null;
            grdCsv.DataSource = ReadDatabase().Tables[0];
            if (grdCsv.Rows.Count > 0)
            {
                FormatDataGridView();
                cmdExport.Enabled = true;
            }
            else
            {
                cmdExport.Enabled = false;
            }


        }

        private void FormatDataGridView()
        {
            //Format the Data Grid View
            grdCsv.Columns[0].Visible = false;
            grdCsv.Columns[1].Visible = false;
            grdCsv.Columns[2].Visible = false;            
            grdCsv.Columns[9].Visible = false;
            grdCsv.Columns[10].Visible = false;
            grdCsv.Columns[11].Visible = false;
            grdCsv.Columns[12].Visible = false;
            grdCsv.Columns[13].Visible = false;
            //dtGrdVol.Columns[2].Visible = false;
            //Format Colors


        }

        private void cmbProject_Leave(object sender, EventArgs e)
        {
            populateBundle();
        }

        
        
        private void cmdExport_Click(object sender, EventArgs e)
        {
            if ((cmbProject.Text == "" || cmbProject.Text == null) && (cmbBundle.Text == "" || cmbBundle.Text == null))
            {
                MessageBox.Show("Please select proper Project and Batch...");
                cmbProject.Focus();
                cmbProject.Select();
            }
            else
            {
                
                projKey = cmbProject.SelectedValue.ToString();
                
                bundleKey = cmbBundle.SelectedValue.ToString();

                //this.Hide();
                if (grdCsv.Rows.Count > 0)
                {
                    statusStrip1.Items.Add("Status: Wait While Uploading the Database......");
                    bool updatebundle = updateBundle();
                    bool updatemetada = updateMetaData();

                    if (updatebundle == true && updatemetada == true)
                    {

                        statusStrip1.Items.Clear();
                        statusStrip1.Items.Add("Status: Bundle SucessFully Uploaded");

                    }
                    else
                    {
                        statusStrip1.Items.Clear();
                        statusStrip1.Items.Add("Status: Uploading Cannot be Completed");
                    }
                }
                else
                {
                    MessageBox.Show("No files present for this bundle...", "", MessageBoxButtons.OK,MessageBoxIcon.Error);
                    return;
                }
            }
        }

        public DataSet GetBundlePath(string prmProjKey, string prmBundleKey)
        {
            string sqlStr = null;
            DataSet bundleDs = new DataSet();

            try
            {
                sqlStr = @"select bundle_path,bundle_code from bundle_master where proj_code = '"+ prmProjKey + "' and bundle_key = '"+prmBundleKey+"' ";
                sqlAdap = new OdbcDataAdapter(sqlStr, sqlCon);
                sqlAdap.Fill(bundleDs);

                //Test whether the path exists or not
                //throw new ValidationException(1234);
            }
            catch (OdbcException ex)
            {
                sqlAdap.Dispose();
                MessageBox.Show(ex.ToString());
            }
            return bundleDs;
        }

        public bool updateBundle()
        {
            bool ret = false;
            if (ret == false)
            {
                _UpdateBundle();

                ret = true;
            }
            return ret;
        }

        public bool _UpdateBundle()
        {
            bool retVal = false;
            string sql = string.Empty;
            string sqlStr = null;
           
            OdbcCommand sqlCmd = new OdbcCommand();


            sqlStr = "UPDATE bundle_master SET status = '1' WHERE proj_code = '" + projKey + "' AND bundle_key = '" + bundleKey + "'";
            System.Diagnostics.Debug.Print(sqlStr);
            OdbcCommand cmd = new OdbcCommand(sqlStr, sqlCon);


            if (cmd.ExecuteNonQuery() > 0)
            {
                retVal = true;
            }


            return retVal;
        }

        public bool updateMetaData()
        {
            bool ret = false;
            if (ret == false)
            {
                _UpdateMetaStatus();

                ret = true;
            }
            return ret;
        }

        public bool KeyCheck(string prmValue)
        {
            string sqlStr = null;
            OdbcCommand cmd = null;
            bool existsBol = true;

            sqlStr = "select bundle_code from bundle_master where batch_code='" + prmValue.ToUpper() + "'";
            cmd = new OdbcCommand(sqlStr, sqlCon);
            existsBol = cmd.ExecuteReader().HasRows;

            return existsBol;
        }

        public bool _UpdateMetaStatus()
        {
            string sqlStr = null;
            
            OdbcCommand sqlCmd = new OdbcCommand();

            bool retVal = false;
            string sql = string.Empty;
            

            sqlStr = "UPDATE metadata_entry SET status = '1' WHERE proj_code = '" + projKey + "' AND bundle_key = '" + bundleKey + "'";
            System.Diagnostics.Debug.Print(sqlStr);
            OdbcCommand cmd = new OdbcCommand(sqlStr, sqlCon);
            if (cmd.ExecuteNonQuery() > 0)
            {
                retVal = true;
            }


            return retVal;
        }
    }
}
