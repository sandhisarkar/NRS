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
    public partial class frmCaseFile : Form
    {
        public string name = frmMain.name;
        OdbcConnection sqlCon = null;
        NovaNet.Utils.Credentials crd;
        //static wItem wi;
        public int projKey = frmBundleSummary.projKey;
        public int bundleKey =  frmBundleSummary.bundleKey;

        public frmCaseFile()
        {
            InitializeComponent();
        }

        public frmCaseFile(OdbcConnection pCon)
        {
            InitializeComponent();

            sqlCon = pCon;

            init();
        }

        public void init()
        {
            textBox1.Text = _GetBundleDetails().Rows[0][0].ToString();
            textBox2.Text = _GetBundleDetails().Rows[0][1].ToString();
            textBox3.Text = _GetBundleDetails().Rows[0][2].ToString();
            textBox4.Text = _GetBundleDetails().Rows[0][3].ToString();
            txtCreateDate.Text = _GetBundleDetails().Rows[0][4].ToString();
            txtHandoverDate.Text = _GetBundleDetails().Rows[0][5].ToString();

            if(_GetFileDetails().Rows.Count > 0)
            {
                for (int i = 0; i < _GetFileDetails().Rows.Count;i++)
                {
                    string itemno = _GetFileDetails().Rows[i][3].ToString();
                    string casefileno = _GetFileDetails().Rows[i][4].ToString();
                    string casestatus = _GetFileDetails().Rows[i][5].ToString();
                    string casenature = _GetFileDetails().Rows[i][6].ToString();
                    string casetype = _GetFileDetails().Rows[i][7].ToString();
                    string caseyear = _GetFileDetails().Rows[i][8].ToString();
                    string[] row = { itemno, casefileno, casestatus, casenature, casetype, caseyear };
                    var listItem = new ListViewItem(row);
                    listView1.Items.Add(listItem);
                }
                button2.Enabled = true;
            }
            //else
            //{

            //}

        }

        public DataTable _GetFileDetails()
        {
            DataTable dt = new DataTable();
            string sql = "select distinct proj_code,bundle_key,sl_no,item_no,case_file_no,case_status,case_nature,case_type,case_year,date_format(created_dttm,'%Y-%m-%d'),created_by,status from case_file_master where proj_code = '" + projKey + "' and bundle_key = '" + bundleKey + "'";
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(dt);
            return dt;
        }

        public int _GetTotalCount()
        {
            DataTable dt = new DataTable();
            string sql = "select Count(*) from case_file_master";
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(dt);
            int count = Convert.ToInt32(dt.Rows[0][0].ToString());
            return count;
        }

        public DataTable _GetBundleDetails()
        {
            DataTable dt = new DataTable();
            string sql = "select distinct establishment as Establishment, Bundle_no as 'Bundle Number',bundle_code,bundle_name as 'Bundle Name',date_format(creation_date,'%Y-%m-%d'),date_format(handover_date,'%Y-%m-%d') as 'Handover Date' from bundle_master where proj_code = '" + projKey + "' and bundle_key = '" + bundleKey + "'";
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(dt);
            return dt;
        }

        public void txtClearAll()
        {
            deTextBox1.Text = "";
            deTextBox2.Text = "";
            deTextBox5.Text = "";
            deTextBox3.Text = "";
            //deTextBox4.Text = "";
            deTextBox1.Focus();

            if(listView1.Items.Count > 0)
            {
                button2.Enabled = true;
            }
        }

        private void populateCaseStatus()
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            string sql = "select case_status_id, case_status from case_status_master  ";

            OdbcDataAdapter odap = new OdbcDataAdapter(sql, sqlCon);
            odap.Fill(dt);


            if (dt.Rows.Count > 0)
            {
                deTextBox1.DataSource = dt;
                deTextBox1.DisplayMember = "case_status";
                deTextBox1.ValueMember = "case_status_id";             
            }
        }

        private void populateCaseNature()
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            string sql = "select case_nature_id, case_nature from case_nature_master  ";

            OdbcDataAdapter odap = new OdbcDataAdapter(sql, sqlCon);
            odap.Fill(dt);


            if (dt.Rows.Count > 0)
            {
                deTextBox2.DataSource = dt;
                deTextBox2.DisplayMember = "case_nature";
                deTextBox2.ValueMember = "case_nature_id";
            }
        }

        private void populateCaseType()
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            string sql = "select case_type_id, case_type_code from case_type_master  ";

            OdbcDataAdapter odap = new OdbcDataAdapter(sql, sqlCon);
            odap.Fill(dt);


            if (dt.Rows.Count > 0)
            {
                deTextBox3.DataSource = dt;
                deTextBox3.DisplayMember = "case_type_code";
                deTextBox3.ValueMember = "case_type_id";
            }
        }

        private void frmCaseFile_Load(object sender, EventArgs e)
        {
            button2.Enabled = false;
            populateCaseStatus();
            populateCaseNature();
            populateCaseType();
            if(listView1.Items.Count > 0)
            {
                button2.Enabled = true;
            }
        }

        private void cmdnew_Click(object sender, EventArgs e)
        {
           
            if (!System.Text.RegularExpressions.Regex.IsMatch(deTextBox4.Text, "[^0-9]"))
            {
                if (deTextBox1.Text != "" && deTextBox2.Text != "" && deTextBox5.Text != "" && deTextBox3.Text != "" && deTextBox4.Text != "")
                {
                    if (listView1.Items.Count > 0)
                    {
                        for (int i = 0; i < listView1.Items.Count; i++)
                        {
                            if (deTextBox5.Text == listView1.Items[i].SubItems[1].Text)
                            {
                                MessageBox.Show("This File/Case already exists...", "Record Management", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                deTextBox5.Focus();
                                return;
                            }
                            else
                            {
                                continue;
                            }
                        }
                        string cou = Convert.ToString(listView1.Items.Count + 1);

                        string[] row = { cou, deTextBox5.Text, deTextBox1.Text, deTextBox2.Text, deTextBox3.Text, deTextBox4.Text };
                        var listItem = new ListViewItem(row);
                        listView1.Items.Add(listItem);
                        txtClearAll();
                    }
                    else
                    {
                        string cou = Convert.ToString(listView1.Items.Count + 1);

                        string[] row = { cou, deTextBox5.Text, deTextBox1.Text, deTextBox2.Text, deTextBox3.Text, deTextBox4.Text };
                        var listItem = new ListViewItem(row);
                        listView1.Items.Add(listItem);
                        txtClearAll();
                    }
                }
                else
                {
                    MessageBox.Show("You have to fill all these fields", "Record Management", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    deTextBox1.Focus();
                    return;
                }
            }
            else
            {
                MessageBox.Show("Case Year isn't a number...", "Record Management", MessageBoxButtons.OK, MessageBoxIcon.Error);
                deTextBox4.Focus();
                return;
            }
            
            
        }

        private void deButton1_Click(object sender, EventArgs e)
        {
            
           
            if (!System.Text.RegularExpressions.Regex.IsMatch(deTextBox4.Text, "[^0-9]"))
            {
                if (deTextBox1.Text != "" && deTextBox2.Text != "" && deTextBox5.Text != "" && deTextBox3.Text != "" && deTextBox4.Text != "")
                {
                    if (listView1.Items.Count > 0)
                    {
                        for (int i = 0; i < listView1.Items.Count; i++)
                        {
                            if (deTextBox5.Text == listView1.Items[i].SubItems[1].Text && listView1.SelectedItems[0].SubItems[1].Text != listView1.Items[i].SubItems[1].Text)
                            {
                                MessageBox.Show("This File/Case already exists...", "Record Management", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                deTextBox5.Focus();
                                return;
                            }
                            else
                            {
                                continue;
                            }
                        }
                        if (listView1.SelectedItems[0].Selected == true)
                        {
                            listView1.SelectedItems[0].SubItems[1].Text = deTextBox5.Text;
                            listView1.SelectedItems[0].SubItems[2].Text = deTextBox1.Text;
                            listView1.SelectedItems[0].SubItems[3].Text = deTextBox2.Text;
                            listView1.SelectedItems[0].SubItems[4].Text = deTextBox3.Text;
                            listView1.SelectedItems[0].SubItems[5].Text = deTextBox4.Text;

                            txtClearAll();
                            cmdnew.Enabled = true;
                        }
                        else
                        {
                            MessageBox.Show("You have to select alteast one file...", "Record Management", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            listView1.Focus();
                            return;
                        }
                    }
                    else
                    {
                        MessageBox.Show("You must add alteast one file...", "Record Management", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        deTextBox1.Focus();
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("You have to fill all these fields", "Record Management", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    deTextBox1.Focus();
                    return;
                }
            }
            else
            {
                MessageBox.Show("Case Year isn't a number...", "Record Management", MessageBoxButtons.OK, MessageBoxIcon.Error);
                deTextBox4.Focus();
                return;

            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            listView1.Select();
            //if (listView1.Items.Count > 0)
            //{
            //    //listView1.Focus();
            //    //listView1.Items[0].Focused = true;
            //    //listView1.Items[0].Selected = true;
            //    listView1_DoubleClick(sender, e);
            //}
            //else
            //{
            //    cmdnew.Focus();
            //}
        }

        private void frmCaseFile_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
            if(e.KeyCode == Keys.Add)
            {
                cmdnew_Click(sender, e);
            }
            if (e.Control == true && e.KeyCode == Keys.E)
            {
                deButton1_Click(sender, e);
            }
            if ((e.Shift == true && e.KeyCode == Keys.Oemplus) || (e.KeyCode == Keys.Add))
            {
                cmdnew_Click(sender, e);
            }
            if (e.Control == true && e.KeyCode == Keys.S)
            {
                button2_Click(sender, e);
            }
        }
        
        private void listView1_KeyUp(object sender, KeyEventArgs e)
        {
            if(listView1.Items.Count>0)
            {
                if (listView1.SelectedItems.Count > 0)
                {
                    if (e.KeyCode == Keys.F2 || e.KeyCode == Keys.Enter)
                    {
                        listView1_DoubleClick(sender, e);
                    }
                }
                
            }
            
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            //cmdnew.Enabled = false;
            deButton1.Enabled = true;
            listView1.Select();
            //listView1.SelectedItems[0].Selected = true;
            try
            {
                deTextBox5.Text = listView1.SelectedItems[0].SubItems[1].Text;
                deTextBox1.Text = listView1.SelectedItems[0].SubItems[2].Text;
                deTextBox2.Text = listView1.SelectedItems[0].SubItems[3].Text;
                deTextBox3.Text = listView1.SelectedItems[0].SubItems[4].Text;
                deTextBox4.Text = listView1.SelectedItems[0].SubItems[5].Text;
                deTextBox1.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cmdnew_Leave(object sender, EventArgs e)
        {
            //if (deButton1.Enabled == true)
            //{
            //    deButton1.Focus();
            //}
            //else
            //{
            //    listView1.Select();
            //    //listView1.Items[0].Focused = true;
            //}
           
        }
        private bool insertIntoCaseFileDB(int itemno, string case_file_no, string case_status,string case_nature,string case_type,string case_year)
        {
            bool commitBol = true;

            string sqlStr = string.Empty;

            OdbcCommand sqlCmd = new OdbcCommand();

            int sl = _GetTotalCount();
            int sl_no = sl + 1;
            sqlStr = @"insert into case_file_master(proj_code,bundle_key, item_no,sl_no,case_file_no,case_status,case_nature,case_type,case_year,created_by,created_dttm,status) values('" +
                        projKey + "','" + bundleKey + "','" + itemno +
                        "','"+sl_no+"','"+case_file_no+"','"+case_status+"','"+case_nature+"','"+case_type+"','"+case_year+"','" + frmMain.name + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',0)";
            sqlCmd.Connection = sqlCon;
            //sqlCmd.Transaction = trans;
            sqlCmd.CommandText = sqlStr;
            int i = sqlCmd.ExecuteNonQuery();
            if (i > 0)
            {
                commitBol = true;
            }
            else
            {
                commitBol = false;
            }

            return commitBol;
        }
        private bool deletefromDB()
        {
            bool commitBol = true;

            string sqlStr = string.Empty;

            OdbcCommand sqlCmd = new OdbcCommand();

            sqlStr = @"Delete from case_file_master where proj_code = '"+projKey+"' and bundle_key = '"+bundleKey+"'";
            sqlCmd.Connection = sqlCon;
            //sqlCmd.Transaction = trans;
            sqlCmd.CommandText = sqlStr;
            int i = sqlCmd.ExecuteNonQuery();
            if (i > 0)
            {
                commitBol = true;
            }
            else
            {
                commitBol = false;
            }

            return commitBol;
        }

        public DataTable _getSlNO()
        {
            DataTable dt = new DataTable();
            string sql = "select sl_no from case_file_master";
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(dt);
            return dt;
        }

        private bool updateDB(int x,int y)
        {
            bool commitBol = true;

            string sqlStr = string.Empty;

            OdbcCommand sqlCmd = new OdbcCommand();

            sqlStr = @"update case_file_master set sl_no = '"+x+"' where sl_no = '"+y+"'";
            sqlCmd.Connection = sqlCon;
            //sqlCmd.Transaction = trans;
            sqlCmd.CommandText = sqlStr;
            int i = sqlCmd.ExecuteNonQuery();
            if (i > 0)
            {
                commitBol = true;
            }
            else
            {
                commitBol = false;
            }

            return commitBol;
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

        public DataTable _GetFileCaseDetails(string proj, string bundle)
        {
            DataTable dt = new DataTable();
            string sql = "select distinct proj_code, bundle_Key,item_no,case_file_no,case_status, case_nature, case_type, case_year from case_file_master where proj_code = '" + proj + "' and bundle_key = '" + bundle + "' and case_file_no not in (select case_file_no from metadata_entry where proj_code = '" + proj + "' and bundle_key = '" + bundle + "')";
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(dt);
            return dt;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OdbcTransaction sqlTrans = null;
            if(listView1.Items.Count > 0)
            {
               
                DialogResult result = MessageBox.Show("Do you want to save changes ? ", "Record Management - Confirmation !", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    //sqlTrans = sqlCon.BeginTransaction();
                    deletefromDB();
                    for (int j = 0; j < _GetTotalCount(); j++)
                    {
                        updateDB(j + 1, Convert.ToInt32(_getSlNO().Rows[j][0].ToString()));
                    }
                    for (int i = 0; i < listView1.Items.Count; i++)
                    {
                        int itemno = Convert.ToInt32(listView1.Items[i].SubItems[0].Text);
                        string case_file_no = listView1.Items[i].SubItems[1].Text;
                        string case_status = listView1.Items[i].SubItems[2].Text;
                        string case_nature = listView1.Items[i].SubItems[3].Text;
                        string case_type = listView1.Items[i].SubItems[4].Text;
                        string case_year = listView1.Items[i].SubItems[5].Text;
                        insertIntoCaseFileDB(itemno, case_file_no, case_status, case_nature, case_type, case_year);
                    }
                    // sqlTrans.Commit();

                    MessageBox.Show(this, "Case/Files added Successfully ", "Case / File Addition", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                else
                {
                    button2.Focus();
                    return;
                }
                    
            }
            else
            {
                MessageBox.Show("Please add atleast one file for Bundle No : "+textBox2.Text,"Record Management",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                deTextBox1.Focus();
                return;
            }
        }

        private void deTextBox4_Leave(object sender, EventArgs e)
        {
            string curYear = DateTime.Now.ToString("yyyy");
            int curIntYear = Convert.ToInt32(curYear);
            bool res = System.Text.RegularExpressions.Regex.IsMatch(deTextBox4.Text, "[^0-9]");
            if (res != true && Convert.ToInt32(deTextBox4.Text) >= 1900 && Convert.ToInt32(deTextBox4.Text) <= curIntYear && deTextBox4.Text.Length == 4 && deTextBox4.Text.Substring(0, 1) != "0" && deTextBox4.Text != "")
            {

            }
            else
            {
                MessageBox.Show("Please input Valid Year, In Between 1900 and "+curIntYear+"...");
                deTextBox4.Focus();
            }
        }

        private void deTextBox4_TextChanged(object sender, EventArgs e)
        {
            //string curYear = DateTime.Now.ToString("yyyy");
            //int curIntYear = Convert.ToInt32(curYear);
            //bool res = System.Text.RegularExpressions.Regex.IsMatch(deTextBox4.Text, "[^0-9]");
            //if (res != true && Convert.ToInt32(deTextBox4.Text) >= 1900 && Convert.ToInt32(deTextBox4.Text) <= curIntYear && deTextBox4.Text.Length == 4 && deTextBox4.Text.Substring(0, 1) != "0")
            //{

            //}
            //else
            //{
            //    //MessageBox.Show("Please input Valid Year, In Between 1900 and " + curIntYear + "...");
            //    deTextBox4.Focus();
            //}
        }
    }
}
