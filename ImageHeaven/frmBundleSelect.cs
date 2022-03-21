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
    public partial class frmBundleSelect : Form
    {
        NovaNet.Utils.dbCon dbcon;
        OdbcConnection sqlCon = null;
        eSTATES[] state;
        public static string projKey;
        public static string bundleKey;

        public static string projName;
        public static string bundleName;

        public frmBundleSelect()
        {
            InitializeComponent();
        }

        public frmBundleSelect(eSTATES[] prmState, OdbcConnection prmCon)
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
            sqlCon = prmCon;
			this.Text = "NRS Record Management - Fetch Docket";
			state=prmState;
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}

       

        private void frmBundleSelect_Load(object sender, EventArgs e)
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
            if (state[0] == eSTATES.METADATA_ENTRY)
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();

                string sql = "select a.bundle_key, a.bundle_code from bundle_master a, project_master b where a.proj_code = b.proj_key and a.proj_code = '" + cmbProject.SelectedValue.ToString() + "'";

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
            if (state[0] == eSTATES.POLICY_CREATED)
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();

                string sql = "select a.bundle_key, a.bundle_code from bundle_master a, project_master b where a.proj_code = b.proj_key and a.proj_code = '" + cmbProject.SelectedValue.ToString() + "' and a.status = '1'";

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
            if (state[0] == eSTATES.POLICY_SCANNED)
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();

                string sql = "select a.bundle_key, a.bundle_code from bundle_master a, project_master b where a.proj_code = b.proj_key and a.proj_code = '" + cmbProject.SelectedValue.ToString() + "' and a.status = '2'";

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
            if(state[0] == eSTATES.POLICY_QC || state[0] == eSTATES.POLICY_ON_HOLD)
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();

                string sql = "select a.bundle_key, a.bundle_code from bundle_master a, project_master b where a.proj_code = b.proj_key and a.proj_code = '" + cmbProject.SelectedValue.ToString() + "' and a.status = '3'";

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
            if(state[0] == eSTATES.POLICY_INDEXED || state[0]== eSTATES.POLICY_ON_HOLD)
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();

                string sql = "select a.bundle_key, a.bundle_code from bundle_master a, project_master b where a.proj_code = b.proj_key and a.proj_code = '" + cmbProject.SelectedValue.ToString() + "' and (a.status = '4' or a.status = '5' or a.status = '6' or a.status = '7' or a.status = '8' or a.status = '30' or a.status = '31' or a.status = '37' or a.status = '40' or a.status = '77')";

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
        }

        private void cmbProject_Leave(object sender, EventArgs e)
        {
            populateBundle();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            projKey = null;
            bundleKey = null;
            this.Close();
        }



        private void btnOK_Click(object sender, EventArgs e)
        {
            frmMain frm = new frmMain();

            if (state[0] == eSTATES.METADATA_ENTRY)
            {


                if ((cmbProject.Text.ToString() != string.Empty) && (cmbBundle.Text.ToString() != string.Empty))
                {

                    projKey = Convert.ToString(cmbProject.SelectedValue.ToString());
                    bundleKey = Convert.ToString(cmbBundle.SelectedValue.ToString());

                    projName = cmbProject.Text;
                    bundleName = cmbBundle.Text;

                    this.Close();

                }
                else
                {
                    MessageBox.Show(this, "Please select Project and proper Bundle from the dropdown", "NRS Record Management - Selection Error !", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            if (state[0] == eSTATES.POLICY_CREATED)
            {
                if ((cmbProject.Text.ToString() != string.Empty) && (cmbBundle.Text.ToString() != string.Empty))
                {

                    projKey = Convert.ToString(cmbProject.SelectedValue.ToString());
                    bundleKey = Convert.ToString(cmbBundle.SelectedValue.ToString());
                    this.Close();

                }
                else
                {
                    MessageBox.Show(this, "Please select Project and proper Bundle from the dropdown", "NRS Record Management - Selection Error !", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

            }

            if (state[0] == eSTATES.POLICY_SCANNED)
            {
                if ((cmbProject.Text.ToString() != string.Empty) && (cmbBundle.Text.ToString() != string.Empty))
                {

                    projKey = Convert.ToString(cmbProject.SelectedValue.ToString());
                    bundleKey = Convert.ToString(cmbBundle.SelectedValue.ToString());
                    this.Close();

                }
                else
                {
                    MessageBox.Show(this, "Please select Project and proper Bundle from the dropdown", "NRS Record Management - Selection Error !", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            if (state[0] == eSTATES.POLICY_QC || state[0] == eSTATES.POLICY_ON_HOLD)
            {
                if ((cmbProject.Text.ToString() != string.Empty) && (cmbBundle.Text.ToString() != string.Empty))
                {

                    projKey = Convert.ToString(cmbProject.SelectedValue.ToString());
                    bundleKey = Convert.ToString(cmbBundle.SelectedValue.ToString());
                    this.Close();

                }
                else
                {
                    MessageBox.Show(this, "Please select Project and proper Bundle from the dropdown", "NRS Record Management - Selection Error !", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            if (state[0] == eSTATES.POLICY_INDEXED || state[0] == eSTATES.POLICY_ON_HOLD)
            {
                if ((cmbProject.Text.ToString() != string.Empty) && (cmbBundle.Text.ToString() != string.Empty))
                {

                    projKey = Convert.ToString(cmbProject.SelectedValue.ToString());
                    bundleKey = Convert.ToString(cmbBundle.SelectedValue.ToString());
                    this.Close();

                }
                else
                {
                    MessageBox.Show(this, "Please select Project and proper Bundle from the dropdown", "NRS Record Management - Selection Error !", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
        }
    }
}
