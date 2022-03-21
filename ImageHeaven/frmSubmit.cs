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

namespace ImageHeaven
{
    public partial class frmSubmit : Form
    {
        wfeProject tmpProj = null;
        wfePolicy tmpPolicy = null;        
        OdbcConnection sqlCon = new OdbcConnection();
        OdbcTransaction txn = null;
        string RunNo = string.Empty;
        public frmSubmit(OdbcConnection prmCon, Credentials prmCrd)
        {
            InitializeComponent();
            sqlCon = prmCon;
        }

        private void frmSubmit_Load(object sender, EventArgs e)
        {
            chkSelect.Enabled = false;
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
        private void PopulateBatch(string year,string book)
        {
            DataSet ds = new DataSet();

            tmpProj = new wfeProject(sqlCon);
            //cmbProject.Items.Add("Select");
            ds = tmpProj.GetAllBatchCode(cmbProject.SelectedValue.ToString(),year,book);
            dgvbatch.DataSource = ds.Tables[0];
            dgvbatch.Columns[0].Width = 25;
            dgvbatch.Columns[1].Width = 160;
            dgvbatch.Columns[1].ReadOnly = true;
            dgvbatch.Columns[3].Visible = false;
            dgvbatch.Columns[4].Visible = false;
            chkSelect.Enabled = true;
            //dgvbatch.Columns[2].Visible = false;
        }

        private void btnPopulate_Click(object sender, EventArgs e)
        {
            int chkCount = 0;
            PopulateBatch(txtYear.Text,txtBook.Text);
            //foreach (DataGridViewRow row in dgvbatch.Rows)
            //{
            //    if (Convert.ToBoolean(row.Cells[0].Value))
            //    {
            //        chkCount++;
            //    }
            //}
            chkCount = dgvbatch.Rows.Count;
            ///////////////////
            if (chkCount < 50)
            {
                chkSelectAll.Enabled = true;
                chkSelect.Enabled = false;
            }
            if(chkCount >= 50)
            {
                chkSelectAll.Enabled = false;
                chkSelect.Enabled = true;
            }

        }
        private OdbcDataAdapter sqlAdap = null;
        private Boolean validate_batchs(string proj_key, string batch_key)
        {
            bool flag = true;
            string sqlStr = string.Empty;
            DataSet batchDs = new DataSet();
            DataSet batchReady = new DataSet();
            int total_deed = 0;
            int ready_deed = 0;

            sqlStr = "select count(*) from policy_master where proj_key = '" + proj_key + "' and batch_key = '"+batch_key+"' and run_no is null";
            sqlAdap = new OdbcDataAdapter(sqlStr, sqlCon);
            sqlAdap.Fill(batchDs);
            if (batchDs.Tables[0].Rows.Count > 0)
            {
                total_deed = Convert.ToInt32(batchDs.Tables[0].Rows[0][0].ToString());
            }
            sqlStr = "select count(*) from policy_master where proj_key = '"+proj_key+"' and batch_key = '"+batch_key+"' and status >= '18' and run_no is null";
            sqlAdap = new OdbcDataAdapter(sqlStr, sqlCon);
            sqlAdap.Fill(batchReady);
            if (batchReady.Tables[0].Rows.Count > 0)
            {
                ready_deed = Convert.ToInt32(batchReady.Tables[0].Rows[0][0].ToString());
            }
            if (total_deed != ready_deed)
            {
                flag = false;
            }


            return flag;

        }
        //private Boolean validate_images(string proj_key, string batch_key)
        //{
        //    bool flag = true;
        //    string sqlStr = string.Empty;
        //    DataSet batchDs = new DataSet();
        //    DataSet batchReady = new DataSet();




        //    return flag;

        //}
        private void btnSubmit_Click(object sender, EventArgs e)
        {
            DataSet policyDS = new DataSet();
            DataSet imageds = new DataSet();
            string qry = string.Empty;
            if (dgvbatch.Rows.Count > 0)
            {
                for (int i = 0; i < dgvbatch.Rows.Count; i++)
                {
                    if (Convert.ToBoolean(dgvbatch.Rows[i].Cells[0].Value))
                    {
                        if (!validate_batchs(dgvbatch.Rows[i].Cells[3].Value.ToString(), dgvbatch.Rows[i].Cells[4].Value.ToString()))
                        {
                            MessageBox.Show("Batch not ready for submission Batch No: " + dgvbatch.Rows[i].Cells[1].Value.ToString() + " Volume No: " + dgvbatch.Rows[i].Cells[2].Value.ToString());
                            return;
                        }
                    }

                }
            }
            //validate images
            //if (dgvbatch.Rows.Count > 0)
            //{
            //    for (int i = 0; i < dgvbatch.Rows.Count; i++)
            //    {
            //        if (Convert.ToBoolean(dgvbatch.Rows[i].Cells[0].Value))
            //        {
            //            qry = "select";
            //        }
            //    }
            //}
            string DtRo = string.Empty;
            string CdKey = string.Empty;
            tmpPolicy = new wfePolicy(sqlCon);
            DtRo = tmpPolicy.GetDtRo();
            CdKey = tmpPolicy.GetCdKey();
            RunNo = DtRo + CdKey.PadLeft(6, '0');
           // MessageBox.Show(RunNo);
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
            if (chkCount < 40 && dgvbatch.Rows.Count >40)
            {
                MessageBox.Show(this, "You cann't Group less than 40 Batches at once ", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (chkCount > 40)
            {
                MessageBox.Show(this,"You cann't Group more than 40 Batches at once ","Warning",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }
            ///////////////////

            string district = dgvbatch.Rows[0].Cells[1].Value.ToString().Substring(0, 2);
            string ro = dgvbatch.Rows[0].Cells[1].Value.ToString().Substring(2, 2);
            string book = dgvbatch.Rows[0].Cells[1].Value.ToString().Substring(4, 1);
            string year = dgvbatch.Rows[0].Cells[1].Value.ToString().Substring(5, 4);
            for (int i = 0; i < dgvbatch.Rows.Count; i++)
            {
                if (Convert.ToBoolean(dgvbatch.Rows[i].Cells[0].Value))
                {
                    if (district != dgvbatch.Rows[i].Cells[1].Value.ToString().Substring(0, 2))
                    {
                        MessageBox.Show("District for All Records must me unique,Remove Folder: " + dgvbatch.Rows[i].Cells[1].Value.ToString(), "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    if (ro != dgvbatch.Rows[i].Cells[1].Value.ToString().Substring(2, 2))
                    {
                        MessageBox.Show("RO for All Records must me unique,Remove Folder: " + dgvbatch.Rows[i].Cells[1].Value.ToString(), "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    if (book != dgvbatch.Rows[i].Cells[1].Value.ToString().Substring(4, 1))
                    {
                        MessageBox.Show("Book Type for All Records must me unique,Remove Folder: " + dgvbatch.Rows[i].Cells[1].Value.ToString(), "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    if (year != dgvbatch.Rows[i].Cells[1].Value.ToString().Substring(5, 4))
                    {
                        MessageBox.Show("Year must me unique,Remove Folder: " + dgvbatch.Rows[i].Cells[1].Value.ToString(), "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }
            //***********************************
            //if (dgvbatch.Rows.Count > 50)
            //{
                //if (chkCount !=50)
                //{
                //    MessageBox.Show("Selected batch number should be 50");
                //    return;
                //}
                //else
                //{
                    updateVolumeStatus();
                    MessageBox.Show("Batch successfully submitted...");
                    UpdateCDCVD(CdKey);
                    dgvbatch.DataSource = null;
                    PopulateBatch(txtYear.Text.Trim(),txtBook.Text.Trim());
                    //dgvbatch.Columns[2].Visible = false;
                    chkSelect.Checked = false;
                //}
            //}
            //else
            //{
            //    updateVolumeStatus();
            //    MessageBox.Show("Batch successfully submitted...");
            //    UpdateCDCVD(CdKey);
            //    chkSelect.Checked = false;
            //    dgvbatch.DataSource = null;
            //    PopulateBatch();
            //}
        }
        public bool UpdateCDCVD(string cdno)
        {
            string sqlStr = null;
            bool commitBol = true;
            OdbcCommand sqlCmd = new OdbcCommand();
            int cdnumber = Convert.ToInt32(cdno) + 1;
            try
            {
                sqlStr = @"update SYSCONFIG set SYSVALUES = '" + cdnumber + "' where SYSKEYS='CD_NO'";
                sqlCmd.CommandText = sqlStr;
                sqlCmd.Connection = sqlCon;
                sqlCmd.ExecuteNonQuery();
                commitBol = true;
            }
            catch (Exception ex)
            {
                commitBol = true;
            }
            return commitBol;
        }
        private void PopulateSelectedBatchCount()
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

        private void dgvbatch_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)
                PopulateSelectedBatchCount();
        }

        private void dgvbatch_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                dgvbatch.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
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
                        tmpPolicy.UpdateRunNoBatchStatus(txn, dgvbatch.Rows[i].Cells[3].Value.ToString(), dgvbatch.Rows[i].Cells[4].Value.ToString(), RunNo);
                        tmpPolicy.UpdateRunPolicyStatus(txn, dgvbatch.Rows[i].Cells[3].Value.ToString(), dgvbatch.Rows[i].Cells[4].Value.ToString(), RunNo);
                        txn.Commit();
                       
                    }
                    catch(Exception ex)
                    {
                        txn.Rollback();
                        MessageBox.Show(ex.Message);
                    }
                }
               
            }
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void chkSelect_CheckedChanged(object sender, EventArgs e)
        {
            try
            {

                if (chkSelect.Checked == true)
                {
                    if (dgvbatch.Rows.Count > 40)
                    {
                        for (int i = 0; i < 40; i++)
                        {
                            dgvbatch.Rows[i].Cells[0].Value = true;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < 40; i++)
                    {
                        dgvbatch.Rows[i].Cells[0].Value = false;
                    }
                }
            }
            catch (Exception ex)
            {

            }
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
                        dgvbatch.Rows[i].Cells[0].Value = true;
                    }

                }
            }

            catch (Exception ex)
            {

            }
        }
    }
}
