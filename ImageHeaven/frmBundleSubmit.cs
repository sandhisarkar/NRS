using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using LItems;
using System.Data.Odbc;
using NovaNet.Utils;
using NovaNet.wfe;

namespace ImageHeaven
{
    public partial class frmBundleSubmit : Form
    {

        wfeProject tmpProj = null;
        wfePolicy tmpPolicy = null;
        OdbcConnection sqlCon = new OdbcConnection();
        OdbcTransaction txn = null;
        string RunNo = string.Empty;
        private OdbcDataAdapter sqlAdap = null;
        byte[] tmpWrite;
        System.IO.MemoryStream stateLog;
        public string err = null;
        public static NovaNet.Utils.exLog.Logger exMailLog = new NovaNet.Utils.exLog.emailLogger("./errLog.log", NovaNet.Utils.exLog.LogLevel.Dev, Constants._MAIL_TO, Constants._MAIL_FROM, Constants._SMTP);
        public static NovaNet.Utils.exLog.Logger exTxtLog = new NovaNet.Utils.exLog.txtLogger("./errLog.log", NovaNet.Utils.exLog.LogLevel.Dev);	

        public frmBundleSubmit()
        {
            InitializeComponent();
        }

        public frmBundleSubmit(OdbcConnection prmCon, Credentials prmCrd)
        {
            InitializeComponent();
            sqlCon = prmCon;
        }

        private void frmBundleSubmit_Load(object sender, EventArgs e)
        {
            chkSelectNone.Enabled = false;
            chkSelectAll.Enabled = false;
            PopulateProjectCombo();
        }
        private void PopulateProjectCombo()
        {
            DataSet ds = new DataSet();
            tmpProj = new wfeProject(sqlCon);
            ds = tmpProj.GetAllValues();
            cmbProject.DataSource = ds.Tables[0];
            cmbProject.DisplayMember = ds.Tables[0].Columns[1].ToString();
            cmbProject.ValueMember = ds.Tables[0].Columns[0].ToString();
        }

        private void cmbProject_Leave(object sender, EventArgs e)
        {
            DataSet ds = new DataSet();
            tmpProj = new wfeProject(sqlCon);
            ds = populateBundle();
            cmbBundle.DataSource = ds.Tables[0];
            cmbBundle.DisplayMember = ds.Tables[0].Columns[1].ToString();
            cmbBundle.ValueMember = ds.Tables[0].Columns[0].ToString();
        }

        public System.Data.DataSet populateBundle()
        {
            string sqlStr = null;

            DataSet bundleDs = new DataSet();

            try
            {
                sqlStr = "select a.bundle_key, a.bundle_code from bundle_master a, project_master b where a.proj_code = b.proj_key and a.status='5' and a.proj_code = '"+cmbProject.SelectedValue.ToString()+"'";
                sqlAdap = new OdbcDataAdapter(sqlStr, sqlCon);
                sqlAdap.Fill(bundleDs);
            }
            catch (Exception ex)
            {
                sqlAdap.Dispose();
                stateLog = new System.IO.MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes(sqlStr + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex);
            }
            return bundleDs;
        }

        private void btnPopulate_Click(object sender, EventArgs e)
        {
            int chkCount = 0;

            try
            {
                if (cmbBundle.SelectedValue.ToString() == null || cmbBundle.SelectedValue.ToString() == "")
                {
                    return;
                }
                else
                {
                    PopulateCaseFileWithBundle(cmbProject.SelectedValue.ToString(), cmbBundle.SelectedValue.ToString());

                    chkCount = dgvbatch.Rows.Count;
                    ///////////////////
                    if (chkCount < 0)
                    {
                        chkSelectAll.Enabled = false;
                        chkSelectNone.Enabled = true;
                    }
                    if (chkCount > 0)
                    {
                        chkSelectAll.Enabled = true;
                        chkSelectNone.Enabled = false;
                    }
                }
            }
            catch(Exception ex)
            {
                return;
            }

        }

        private void PopulateCaseFileWithBundle(string projkey, string bundlekey)
        {

            try
            {
                DataSet ds = new DataSet();

                tmpProj = new wfeProject(sqlCon);
                //cmbProject.Items.Add("Select");
                ds = GetFileName(cmbProject.SelectedValue.ToString(), cmbBundle.SelectedValue.ToString());
                dgvbatch.DataSource = ds.Tables[0];
                dgvbatch.Columns[0].Width = 25;
                dgvbatch.Columns[1].Width = 160;
                dgvbatch.Columns[1].ReadOnly = true;
                dgvbatch.Columns[3].Visible = false;
                dgvbatch.Columns[4].Visible = false;
                chkSelectAll.Enabled = true;
                chkSelectNone.Enabled = true;
            }
            catch (Exception ex)
            {
                return;
            }
        }

        public System.Data.DataSet GetFileName(string proj_key, string bundle_key)
        {
            string sqlStr = null;

            sqlStr = "select distinct b.bundle_code,a.patient_id,a.proj_code,a.bundle_key from metadata_entry a,bundle_master b where (a.status = '" + 3 + "' or a.status= '" + (int)eSTATES.POLICY_FQC + "' or a.status = '" + (int)eSTATES.POLICY_NOT_INDEXED + "' or a.status= '" + 4 + "' or a.status= '" + (int)eSTATES.POLICY_CHECKED + "' or a.status='" + (int)eSTATES.POLICY_EXCEPTION + "') and a.proj_code='" + proj_key + "' and a.bundle_key = '"+bundle_key+"' and a.bundle_key=b.bundle_key and b.status <> '8' group by a.item_no order by a.bundle_key";
            DataSet projDs = new DataSet();

            try
            {
                sqlAdap = new OdbcDataAdapter(sqlStr, sqlCon);
                sqlAdap.Fill(projDs);
            }
            catch (Exception ex)
            {
                sqlAdap.Dispose();
                stateLog = new System.IO.MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes(sqlStr + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex);
            }
            return projDs;
        }

        private void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (chkSelectAll.Checked == true)
                {
                    for (int i = 0; i < dgvbatch.Rows.Count; i++)
                    {
                        dgvbatch.Rows[i].Cells[0].Value = true;
                    }
                }
                else
                {
                    for (int i = 0; i < dgvbatch.Rows.Count; i++)
                    {
                        dgvbatch.Rows[i].Cells[0].Value = false;
                    }

                }
            }

            catch (Exception ex)
            {

            }
        }

        private void chkSelectNone_CheckedChanged(object sender, EventArgs e)
        {
            try
            {

                if (chkSelectNone.Checked == true)
                {
                    //if (dgvbatch.Rows.Count > 40)
                    //{
                        for (int i = 0; i < dgvbatch.Rows.Count; i++)
                        {
                            dgvbatch.Rows[i].Cells[0].Value = false;
                        }
                    //}
                }
                else
                {
                    for (int i = 0; i < dgvbatch.Rows.Count; i++)
                    {
                        dgvbatch.Rows[i].Cells[0].Value = true;
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void dgvbatch_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)
                PopulateSelectedFileCount();
        }

        private void dgvbatch_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                dgvbatch.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void PopulateSelectedFileCount()
        {
            int StatusStep = 0;
            try
            {
                if (dgvbatch.Rows.Count > 0)
                {
                    for (int x = 0; x < dgvbatch.Rows.Count; x++)
                    {
                        if (Convert.ToBoolean(dgvbatch.Rows[x].Cells[0].Value))
                        {
                            StatusStep = StatusStep + 1;
                        }

                    }
                    lblBatchSelected.Text = StatusStep.ToString();
                }
            }
            catch (Exception ex)
            {
                lblBatchSelected.Text = "0";
            }
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private Boolean validate_batchs(string proj_key, string batch_key)
        {
            bool flag = true;
            string sqlStr = string.Empty;
            DataSet batchDs = new DataSet();
            DataSet batchReady = new DataSet();
            int total_file = 0;
            int ready_file = 0;

            sqlStr = "select count(*) from metadata_entry where proj_code = '" + proj_key + "' and bundle_key = '" + batch_key + "' ";
            sqlAdap = new OdbcDataAdapter(sqlStr, sqlCon);
            sqlAdap.Fill(batchDs);
            if (batchDs.Tables[0].Rows.Count > 0)
            {
                total_file = Convert.ToInt32(batchDs.Tables[0].Rows[0][0].ToString());
            }
            sqlStr = "select count(*) from metadata_entry where proj_code = '" + proj_key + "' and bundle_key = '" + batch_key + "' and (status = '3' or status = '4' or status = '5' or status = '30' or status = '31' or status = '40')";
            sqlAdap = new OdbcDataAdapter(sqlStr, sqlCon);
            sqlAdap.Fill(batchReady);
            if (batchReady.Tables[0].Rows.Count > 0)
            {
                ready_file = Convert.ToInt32(batchReady.Tables[0].Rows[0][0].ToString());
            }
            if (total_file != ready_file)
            {
                flag = false;
            }
            int lblCount;
            if(lblBatchSelected.Text == null || lblBatchSelected.Text == "")
            {
                lblCount = 0;
            }
            else
            {
                lblCount = Convert.ToInt32(lblBatchSelected.Text);
            }
            if (lblCount != total_file)
            {
                flag = false;
            }

            return flag;

        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            DataSet policyDS = new DataSet();
            DataSet imageds = new DataSet();
            string qry = string.Empty;
            if (dgvbatch.Rows.Count > 0)
            {
                
                for (int i = 0; i < dgvbatch.Rows.Count; i++)
                {
                    if ((dgvbatch.Rows[i].Cells[1].Value.ToString())!=null)
                    {
                        if (!validate_batchs(dgvbatch.Rows[i].Cells[3].Value.ToString(), dgvbatch.Rows[i].Cells[4].Value.ToString()))
                        {
                            MessageBox.Show("Bundle not ready for submission Bundle No: " + dgvbatch.Rows[i].Cells[1].Value.ToString() + "Patient File No: " + dgvbatch.Rows[i].Cells[2].Value.ToString());
                            return;
                        }
                    }

                }
            }

            int chkCount = 0;
            if (dgvbatch.Rows.Count == 0)
            {
                btnSubmit.Enabled = false;
                return;
            }
            foreach (DataGridViewRow row in dgvbatch.Rows)
            {
                if (Convert.ToBoolean(row.Cells[0].Value))
                {
                    chkCount++;
                }
            }

            string bundleName = dgvbatch.Rows[0].Cells[1].Value.ToString();
            for (int i = 0; i < dgvbatch.Rows.Count; i++)
            {
                if (Convert.ToBoolean(dgvbatch.Rows[i].Cells[0].Value))
                {
                    if (bundleName != dgvbatch.Rows[i].Cells[1].Value.ToString())
                    {
                        MessageBox.Show("Bundle Name for All Records must me unique,Remove Folder: " + dgvbatch.Rows[i].Cells[1].Value.ToString(), "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }


            updateVolumeStatus();
            MessageBox.Show("Batch successfully submitted...");
           // UpdateCDCVD(CdKey);
            dgvbatch.DataSource = null;
            PopulateCaseFileWithBundle(cmbProject.SelectedValue.ToString(), cmbBundle.SelectedValue.ToString());
            //dgvbatch.Columns[2].Visible = false;
            chkSelectAll.Checked = false;
        }

        public bool UpdateRunNoBatchStatus(OdbcTransaction pTxn, string proj_key, string batch_key)
        {
            try
            {
                OdbcTransaction sqlTrans = null;
                OdbcCommand sqlCmdPolicy = new OdbcCommand();
                OdbcCommand sqlRawdata = new OdbcCommand();
                string sqlStr = @"update bundle_master set status='" + 6 + "' where "
                                 + "proj_code='" + proj_key + "' and bundle_key='" + batch_key + "'";
                sqlTrans = pTxn;
                sqlCmdPolicy.Connection = sqlCon;
                sqlCmdPolicy.Transaction = sqlTrans;
                sqlCmdPolicy.CommandText = sqlStr;
                sqlCmdPolicy.ExecuteNonQuery();
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool UpdateRunPolicyStatus(OdbcTransaction pTxn, string proj_key, string batch_key)
        {
            try
            {
                OdbcTransaction sqlTrans = null;
                OdbcCommand sqlCmdPolicy = new OdbcCommand();
                OdbcCommand sqlRawdata = new OdbcCommand();
                string sqlStr = @"update metadata_entry set status='" + 6 + "' where "
                                 + "proj_code='" + proj_key + "' and bundle_key='" + batch_key + "'";
                sqlTrans = pTxn;
                sqlCmdPolicy.Connection = sqlCon;
                sqlCmdPolicy.Transaction = sqlTrans;
                sqlCmdPolicy.CommandText = sqlStr;
                sqlCmdPolicy.ExecuteNonQuery();
            }
            catch
            {
                return false;
            }
            return true;
        }

        private void updateVolumeStatus()
        {

            for (int i = 0; i < dgvbatch.Rows.Count; i++)
            {
                if (Convert.ToBoolean(dgvbatch.Rows[i].Cells[0].Value))
                {
                    txn = sqlCon.BeginTransaction();
                    try
                    {
                        UpdateRunNoBatchStatus(txn, dgvbatch.Rows[i].Cells[3].Value.ToString(), dgvbatch.Rows[i].Cells[4].Value.ToString());
                        //UpdateRunPolicyStatus(txn, dgvbatch.Rows[i].Cells[3].Value.ToString(), dgvbatch.Rows[i].Cells[4].Value.ToString());
                        txn.Commit();

                    }
                    catch (Exception ex)
                    {
                        txn.Rollback();
                        MessageBox.Show(ex.Message);
                    }
                }

            }
        }

    }
}
