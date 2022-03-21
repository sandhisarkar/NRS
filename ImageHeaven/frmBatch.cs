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
    public partial class frmBatch : Form
    {
        protected int mode;
        MemoryStream stateLog;
        private udtBatch objBatch;
        private OdbcDataAdapter sqlAdap = null;
        private DataSet dsPath = null;
        private wfeProject objProj = null;
        private INIReader rd = null;
        private KeyValueStruct udtKeyValue;
        public string err = null;
        private int projCode;
        wfeBatch crtBatch = null;
        OdbcConnection sqlCon = null;
        byte[] tmpWrite;
        public static NovaNet.Utils.exLog.Logger exMailLog = new NovaNet.Utils.exLog.emailLogger("./errLog.log", NovaNet.Utils.exLog.LogLevel.Dev, Constants._MAIL_TO, Constants._MAIL_FROM, Constants._SMTP);
        public static NovaNet.Utils.exLog.Logger exTxtLog = new NovaNet.Utils.exLog.txtLogger("./errLog.log", NovaNet.Utils.exLog.LogLevel.Dev);

        string name = frmMain.name;


        public string currentDate;
        public string handoverDate;

        public frmBatch()
        {
            InitializeComponent();
        }

        public frmBatch(wItem prmCmd, OdbcConnection prmCon)
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
            //this.Icon = 
            exMailLog.SetNextLogger(exTxtLog);
            
			crtBatch = (wfeBatch) prmCmd;
            sqlCon = prmCon;
			if (crtBatch.GetMode()==Constants._ADDING)
                this.Text = "Record Management - Add Bundle";
			else
                this.Text = "Record Management - Edit Bundle";
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}


        

        private void frmBatch_Load(object sender, EventArgs e)
        {
           
            groupBox3.Enabled = false;
            populateProject();
            button2.Enabled = false;
            
            
            currentDate = DateTime.Now.ToString("dd-MM-yyyy");
            handoverDate = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
        }

        private void populateProject()
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            string sql = "select proj_key, proj_code from project_master ";
            
            OdbcDataAdapter odap = new OdbcDataAdapter(sql, sqlCon);
            odap.Fill(dt);


            if (dt.Rows.Count > 0)
            {
                cmbProject.DataSource = dt;
                cmbProject.DisplayMember = "proj_code";
                cmbProject.ValueMember = "proj_key";
            }
            else
            {
                MessageBox.Show("Add one project first...");
            }

        }

        

        private void ClearAllField()
        {
           
            txtCreateDate.Text = string.Empty;
            dtpBatchMonYear.Text = string.Empty;
            textBox3.Text = string.Empty;
            textBox4.Text = string.Empty;
            button2.Enabled = false;
            cmbProject.Focus();
            groupBox3.Enabled = false;
        }

        private void cmbProject_Leave_1(object sender, EventArgs e)
        {
            
            if (cmbProject.Text == "" || cmbProject.Text == null)
            {
                MessageBox.Show("Please select a project name");
                cmbProject.Focus();
                cmbProject.Select();
            }
            else
            {
                

                groupBox3.Enabled = true;

                dtpBatchMonYear.Focus();
                dtpBatchMonYear.Select();

                txtCreateDate.Text = currentDate;
               
            }
        }

        private void cmbProject_MouseLeave(object sender, EventArgs e)
        {
            
            if (cmbProject.Text == "" || cmbProject.Text == null)
            {
                MessageBox.Show("Please select a project name");
                cmbProject.Focus();
                cmbProject.Select();
            }
            else
            {
               

                groupBox3.Enabled = true;

                dtpBatchMonYear.Focus();
                dtpBatchMonYear.Select();

                txtCreateDate.Text = currentDate;
               
            }
        }

        private void deButton1_Click(object sender, EventArgs e)
        {
            string dateFormat = "MM-yyyy";
            string dateFromDtp = dtpBatchMonYear.Text;
            DateTime datetime;
            if(dtpBatchMonYear.Text == "" || dtpBatchMonYear.Text == null)
            {
                MessageBox.Show("Please Specify Proper Month-Year");
                dtpBatchMonYear.Focus();
                dtpBatchMonYear.Select();
                return;
            }
            else
            {
                if (DateTime.TryParseExact(dateFromDtp, dateFormat, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out datetime))
                {
                    
                    string month = dateFromDtp.Substring(0, 2).ToString();
                    string year = dateFromDtp.Substring(3, 4).ToString();

                    string bundleCode = month + year;

                    textBox3.Text = bundleCode;
                    textBox4.Text = bundleCode;

                    button2.Enabled = true;
                }
                else
                {
                    MessageBox.Show("Please Specify Proper Month-Year");
                    dtpBatchMonYear.Focus();
                    dtpBatchMonYear.Select();
                    return;
                }
            }
            
        }

        

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        public bool KeyCheck(string prmValue)
        {
            string sqlStr = null;
            OdbcCommand cmd = null;
            bool existsBol = true;

            sqlStr = "select bundle_code from bundle_master where bundle_code='" + prmValue.ToUpper() + "'";
            cmd = new OdbcCommand(sqlStr, sqlCon);
            existsBol = cmd.ExecuteReader().HasRows;

            return existsBol;
        }
        private bool Validate(udtBatch cmd)
        {
            bool validateBol = true;
            //errList = new Hashtable();
            if (cmd.batch_code == string.Empty || KeyCheck(cmd.batch_code) == true)
            {
                validateBol = false;
                //errList.Add("Code", Constants.NOT_VALID);
            }

            if (cmd.batch_name == string.Empty)
            {
                validateBol = false;
                //errList.Add("Name", Constants.NOT_VALID);
            }

            if (cmd.Created_By == string.Empty && mode == Constants._ADDING)
            {
                validateBol = false;
                //errList.Add("Created_By", Constants.NOT_VALID);
            }

            if (cmd.Created_DTTM == string.Empty && mode == Constants._ADDING)
            {
                validateBol = false;
               // errList.Add("Created_DTTM", Constants.NOT_VALID);
            }

            ///Required at the time of editing
            if (cmd.Modified_By == string.Empty && mode == Constants._EDITING)
            {
                validateBol = false;
                //errList.Add("Modified_By", Constants.NOT_VALID);
            }

            if (cmd.Modified_DTTM == string.Empty && mode == Constants._EDITING)
            {
                validateBol = false;
                //errList.Add("Modified_DTTM", Constants.NOT_VALID);
            }

            return validateBol;
        }
        public bool Commit_Bundle(string createDt)
        {
            string sqlStr = null;
            OdbcTransaction sqlTrans = null;
            bool commitBol = true;
            OdbcCommand sqlCmd = new OdbcCommand();
            string scanbatchPath = null;

            //errList = new Hashtable();
            objProj = new wfeProject(sqlCon);

            dsPath = objProj.GetPath(objBatch.proj_code);

            if (dsPath.Tables[0].Rows.Count > 0)
            {
                scanbatchPath = dsPath.Tables[0].Rows[0]["project_Path"] + "\\" + objBatch.batch_code;
            }

            sqlStr = @"insert into bundle_master(proj_code,bundle_code,bundle_name,created_by" +
                ",Created_DTTM,creation_date,bundle_path) values(" +
                objBatch.proj_code + ",'" + objBatch.batch_code.ToUpper() + "','" + objBatch.batch_name + "'," +
                "'" + objBatch.Created_By + "','" + objBatch.Created_DTTM + "','" + createDt + "','" +
                scanbatchPath.Replace("\\", "\\\\") + "')";
            try
            {
                if (KeyCheck(objBatch.batch_code) == false)
                {
                    sqlTrans = sqlCon.BeginTransaction();
                    sqlCmd.Connection = sqlCon;
                    sqlCmd.Transaction = sqlTrans;
                    sqlCmd.CommandText = sqlStr;
                    sqlCmd.ExecuteNonQuery();

                    if (mode == Constants._ADDING)
                    {
                        if (FileorFolder.CreateFolder(scanbatchPath) == true)
                        {
                            commitBol = true;
                            sqlTrans.Commit();
                        }
                        else
                        {
                            commitBol = false;
                            sqlTrans.Rollback();
                            rd = new INIReader(Constants.EXCEPTION_INI_FILE_PATH);
                            udtKeyValue.Key = Constants.BATCH_FOLDER_CREATE_ERROR.ToString();
                            udtKeyValue.Section = Constants.BATCH_EXCEPTION_SECTION;
                            string ErrMsg = rd.Read(udtKeyValue);
                            throw new CreateFolderException(ErrMsg);
                        }
                    }
                    else
                    {
                        commitBol = true;
                        sqlTrans.Commit();
                    }
                }
                else
                    commitBol = false;
            }
            catch (Exception ex)
            {
                //errList.Add(Constants.DBERRORTYPE, ex.Message);
                commitBol = false;
                sqlTrans.Rollback();
                sqlCmd.Dispose();
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes(sqlStr + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex);
            }
            return commitBol;
        }
        public bool TransferValuesBundle(udtCmd cmd,string creatdate)
        {

            objBatch = (udtBatch)(cmd);
            if (KeyCheck(objBatch.batch_code) == false)
            {
                if (Validate(objBatch) == true)
                {
                    if (Commit_Bundle(creatdate) == true)
                    {
                        return true;
                    }
                    else
                    {
                        rd = new INIReader(Constants.EXCEPTION_INI_FILE_PATH);
                        udtKeyValue.Key = Constants.SAVE_ERROR.ToString();
                        udtKeyValue.Section = Constants.COMMON_EXCEPTION_SECTION;
                        string ErrMsg = rd.Read(udtKeyValue);
                        throw new DbCommitException(ErrMsg);
                    }
                }
                else
                {
                    //throw new ValidationException(Constants.ValidationException) ;
                    return false;
                }
            }
            else
            {
                rd = new INIReader(Constants.EXCEPTION_INI_FILE_PATH);
                udtKeyValue.Key = Constants.DUPLICATE_KEY_CHECK.ToString();
                udtKeyValue.Section = Constants.COMMON_EXCEPTION_SECTION;
                string ErrMsg = rd.Read(udtKeyValue);
                throw new KeyCheckException(ErrMsg);
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox3.Text == null || textBox3.Text == "")
            {
                MessageBox.Show("Please generate a Batch Code...");
                dtpBatchMonYear.Focus();
                dtpBatchMonYear.Select();
            }
            else
            {
                
                string bundleNo = textBox3.Text;

                string dtpmonth = dtpBatchMonYear.Text.Substring(0, 2);
                string dtpyear = dtpBatchMonYear.Text.Substring(3, 4);

                string dtpDate = dtpmonth + dtpyear;

                if(dtpDate == bundleNo)
                {
                    NovaNet.Utils.dbCon dbcon = new NovaNet.Utils.dbCon();
                    udtBatch objBatch = new udtBatch();
                    try
                    {
                        statusStrip1.Items.Clear();
                        crtBatch = new wfeBatch(sqlCon);

                        objBatch.proj_code = Convert.ToInt32(cmbProject.SelectedValue);
                        objBatch.batch_code = textBox3.Text;
                        objBatch.batch_name = textBox4.Text;
                        objBatch.Created_By = name;
                        objBatch.Created_DTTM = dbcon.GetCurrenctDTTM(1, sqlCon);

                        if (TransferValuesBundle(objBatch, txtCreateDate.Text) == true)
                        {
                            statusStrip1.Items.Add("Status: Data SucessFully Saved");
                            statusStrip1.ForeColor = System.Drawing.Color.Black;
                            ClearAllField();
                        }
                        else
                        {
                            statusStrip1.Items.Add("Status: Data Can not be Saved");
                            statusStrip1.ForeColor = System.Drawing.Color.Red;
                        }
                    }
                    catch (KeyCheckException ex)
                    {
                        MessageBox.Show(ex.Message, "Record Management", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        stateLog = new MemoryStream();
                        tmpWrite = new System.Text.ASCIIEncoding().GetBytes("Bundle Key-" + objBatch.batch_key + "\n" + "project Key-" + objBatch.proj_code + "\n");
                        stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                        //exMailLog.Log(ex, this);
                    }
                    catch (DbCommitException dbex)
                    {
                        MessageBox.Show(dbex.Message, "Record Management", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        stateLog = new MemoryStream();
                        tmpWrite = new System.Text.ASCIIEncoding().GetBytes("Error while Commit" + "Bundle Key-" + objBatch.batch_key + "\n" + "project Key-" + objBatch.proj_code + "\n");
                        stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                        // exMailLog.Log(dbex, this);
                    }
                    catch (CreateFolderException folex)
                    {
                        MessageBox.Show(folex.Message, "Record Management", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        stateLog = new MemoryStream();
                        tmpWrite = new System.Text.ASCIIEncoding().GetBytes("Error while Create Folder" + "Bundle Key-" + objBatch.batch_key + "\n" + "project Key-" + objBatch.proj_code + "\n");
                        stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                        // exMailLog.Log(folex, this);
                    }
                    catch (DBConnectionException conex)
                    {
                        MessageBox.Show(conex.Message, "Record Management", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        stateLog = new MemoryStream();
                        tmpWrite = new System.Text.ASCIIEncoding().GetBytes("Error while Connection error" + "Bundle Key-" + objBatch.batch_key + "\n" + "project Key-" + objBatch.proj_code + "\n");
                        stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                        //exMailLog.Log(conex, this);
                    }
                    catch (INIFileException iniex)
                    {
                        MessageBox.Show(iniex.Message, "Record Management", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        stateLog = new MemoryStream();
                        tmpWrite = new System.Text.ASCIIEncoding().GetBytes("Error while INI read error" + "Bundle Key-" + objBatch.batch_key + "\n" + "project Key-" + objBatch.proj_code + "\n");
                        stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                        //exMailLog.Log(iniex, this);
                    }
                }
                else
                {
                    MessageBox.Show("Bundle Number Mismatch", "Record Management", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    dtpBatchMonYear.Focus();
                    dtpBatchMonYear.Select();
                    return;
                }

               
                
            }
        }

        private void dtpBatchMonYear_ValueChanged(object sender, EventArgs e)
        {
            dtpBatchMonYear.CustomFormat = "MM-yyyy";
        }

        private void dtpBatchMonYear_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Back || e.KeyCode == Keys.Delete)
            {
                dtpBatchMonYear.CustomFormat = " ";
            }
            if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down || e.KeyCode == Keys.Enter)
            {
                dtpBatchMonYear.CustomFormat = "MM-yyyy";
                dtpBatchMonYear.Select();
            }
        }
    }
}
