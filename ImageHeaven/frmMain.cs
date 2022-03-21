using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
//using System.Linq;
using System.Text;
using System.Windows.Forms;
using NovaNet.Utils;
using System.Data.Odbc;
using System.Reflection;
using System.Data.OleDb;
using System.Globalization;
using LItems;
using NovaNet;
using NovaNet.wfe;


namespace ImageHeaven
{
    public partial class frmMain : Form
    {

        static wItem wi;
        //NovaNet.Utils.dbCon dbcon;
        frmMain mainForm;
        OdbcConnection sqlCon = null;
        public Credentials crd;
        static int colorMode;
        dbCon dbcon;
        //
        NovaNet.Utils.GetProfile pData;
        NovaNet.Utils.ChangePassword pCPwd;
        NovaNet.Utils.Profile p;
        public static NovaNet.Utils.IntrRBAC rbc;
        private short logincounter;
        //

        public static string projKey;
        public static string bundleKey;
        public static string projectName = null;
        public static string batchName = null;
        public static string boxNumber = null;
        public static string projectVal = null;
        public static string batchVal = null;

        public static string name;

        public static int height;
        public static int width;
        
        public frmMain(OdbcConnection pCon)
        {
            InitializeComponent();

            sqlCon = pCon;

            logincounter = 0;

            ImageHeaven.Program.Logout = false;
        }

        public frmMain()
        {
            //
            // The InitializeComponent() call is required for Windows Forms designer support.
            //
            AssemblyName assemName = Assembly.GetExecutingAssembly().GetName();
            this.Text = "Record Management" + "           Version: " + assemName.ToString();
            InitializeComponent();
            
            logincounter = 0;
            //
            // TODO: Add constructor code after the InitializeComponent() call.
            //
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            height = pictureBox1.Height;
            width = pictureBox1.Width;
            
            int k;
            dbcon = new NovaNet.Utils.dbCon();
            try
            {
                string dllPaths = string.Empty;

                menuStrip1.Visible = false;
                
                
                if (sqlCon.State == ConnectionState.Open)
                {
                    pData = getData;
                    pCPwd = getCPwd;
                    rbc = new NovaNet.Utils.RBAC(sqlCon, dbcon, pData, pCPwd);
                    //string test = sqlCon.Database;
                    GetChallenge gc = new GetChallenge(getData);
                    
                   
                    
                    
                    gc.ShowDialog(this);

                    crd = rbc.getCredentials(p);
                    AssemblyName assemName = Assembly.GetExecutingAssembly().GetName();
                    this.Text = "NRS Record Management" + "           Version: " + assemName.Version.ToString() + "    Database name: " + sqlCon.Database.ToString() + "    Logged in user: " + crd.userName;

                    name = crd.userName;
                    if (crd.role == ihConstants._ADMINISTRATOR_ROLE)
                    {
                        menuStrip1.Visible = true;
                        newToolStripMenuItem.Visible = true;
                        toolsToolStripMenuItem.Visible = true;
                        dataEntryToolStripMenuItem.Visible = true;
                        //imageImportToolStripMenuItem.Visible = true;
                        transactionsToolStripMenuItem.Visible = true;
                        exportToolStripMenuItem.Visible = true;
                        newToolStripMenuItem.Visible = true;
                        toolsToolStripMenuItem.Visible = true;
                        newPasswordToolStripMenuItem.Visible = true;
                        newUserToolStripMenuItem.Visible = true;
                        onlineUsersToolStripMenuItem.Visible = true;
                        dataEntryToolStripMenuItem.Visible = true;
                        imageImportToolStripMenuItem.Visible = false;
                        exportToolStripMenuItem.Visible = true;
                        batchUploadToolStripMenuItem.Visible = true;
                        toolStrip1.Visible = true;
                        toolStripButton1.Visible = true;
                        imageQualityControlToolStripMenuItem.Visible = true;
                        toolStripButton3.Visible = true;
                        indexingToolStripMenuItem.Visible = true;
                        toolStripButton2.Visible = true;
                        qualityControlFinalToolStripMenuItem.Visible = true;
                        toolStripButton4.Visible = true;
                        toolStripMenuItem1.Visible = true;
                        auditToolStripMenuItem.Visible = true;
                        nRSAuditToolStripMenuItem.Visible = true;
                        searchToolStripMenuItem.Visible = true;
                    }
                    else if(crd.role == ihConstants._LIC_ROLE)
                    {
                        menuStrip1.Visible = true;
                        newToolStripMenuItem.Visible = false;
                        transactionsToolStripMenuItem.Visible = false;
                        auditToolStripMenuItem.Visible = true;
                        toolsToolStripMenuItem.Visible = false;
                        logoutToolStripMenuItem.Visible = true;
                        nRSAuditToolStripMenuItem.Visible = true;
                    }
                    else
                    {
                        menuStrip1.Visible = true;
                        newToolStripMenuItem.Visible = false;
                        toolsToolStripMenuItem.Visible = true;
                        newPasswordToolStripMenuItem.Visible = true;
                        newUserToolStripMenuItem.Visible = false;
                        onlineUsersToolStripMenuItem.Visible = false;
                        dataEntryToolStripMenuItem.Visible = true;
                        imageImportToolStripMenuItem.Visible = false;
                        exportToolStripMenuItem.Visible = false;
                        batchUploadToolStripMenuItem.Visible = false;
                        auditToolStripMenuItem.Visible = false;
                        nRSAuditToolStripMenuItem.Visible = false;
                    }
                }
            }
            catch (DBConnectionException dbex)
            {
                //MessageBox.Show(dbex.Message, "Image Heaven", MessageBoxButtons.OK, MessageBoxIcon.Error);
                string err = dbex.Message;
                this.Close();
            }
        }
        void getData(ref NovaNet.Utils.Profile prmp)
        {
            int i;
            p = prmp;
            for (i = 1; i <= 2; i++)
            {
                if (rbc.authenticate(p.UserId, p.Password) == false)
                {
                    if (logincounter == 2)
                    {
                        Application.Exit();
                    }
                    else
                    {
                        logincounter++;
                        GetChallenge ogc = new GetChallenge(getData);
                        ogc.ShowDialog(this);
                    }
                }
                else
                {
                    if (rbc.CheckUserIsLogged(p.UserId))
                    {

                        p = rbc.getProfile();
                        crd = rbc.getCredentials(p);
                        if (crd.role != ihConstants._ADMINISTRATOR_ROLE)
                        {
                            rbc.LockedUser(p.UserId, crd.created_dttm);
                        }
                        break;
                    }
                    else
                    {
                        p.UserId = null;
                        p.UserName = null;
                        GetChallenge ogc = new GetChallenge(getData);
                        AssemblyName assemName = Assembly.GetExecutingAssembly().GetName();
                        this.Text = "NRS Record Management" + "           Version: " + assemName.Version.ToString() + "    Database name: " + sqlCon.Database.ToString() + "    Logged in user: " + crd.userName;
                        ogc.ShowDialog(this);
                    }
                }
            }
        }
        void getCPwd(ref NovaNet.Utils.Profile prmpwd)
        {
            p = prmpwd;
            rbc.changePassword(p.UserId, p.UserName, p.Password);
        }

        private void projectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                frmProject dispProject;
                wi = new wfeProject(sqlCon);
                dispProject = new frmProject(wi, sqlCon);
                dispProject.ShowDialog(this);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dataEntryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //frmEntry frm = new frmEntry(sqlCon);
            //frm.ShowDialog();
        }

        private void batchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                frmBatch dispProject;
                wi = new wfeBatch(sqlCon);
                dispProject = new frmBatch(wi, sqlCon);
                dispProject.ShowDialog(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void newPasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PwdChange pwdCh = new PwdChange(ref p, getCPwd);
            pwdCh.ShowDialog(this);
        }

        private void newUserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddNewUser nwUsr = new AddNewUser(getnwusrData, sqlCon);
            nwUsr.ShowDialog(this);
        }
        void getnwusrData(ref NovaNet.Utils.Profile prmp)
        {
            p = prmp;
            if (rbc.addUser(p.UserId, p.UserName, p.Role_des, p.Password) == false)
            {
                AddNewUser nwUsr = new AddNewUser(getnwusrData, sqlCon);
                nwUsr.ShowDialog(this);
            }
        }

        private void onlineUsersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmLoggedUser loged = new frmLoggedUser(rbc, crd);
            loged.ShowDialog(this);
        }

        public string _GetFileCount(string proj_code, string bundle_key)
        {
            DataTable dt = new DataTable();
            string sql = "select COUNT(*) from metadata_entry where proj_code = '" + proj_code + "' and bundle_key = '" + bundle_key + "' ";
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(dt);
            return dt.Rows[0][0].ToString();
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

        public DataTable _GetBundleStatus(string proj, string bundle)
        {
            DataTable dt = new DataTable();
            string sql = "select distinct status from bundle_master where proj_code = '" + proj + "' and bundle_key = '" + bundle + "' ";
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(dt);
            return dt;
        }

        private void dataEntryToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            

            frmEntrySummary fm = new frmEntrySummary(sqlCon);
            
            fm.ShowDialog(this);

           
            
        }

        private void logoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AssemblyName assemName = Assembly.GetExecutingAssembly().GetName();
            this.Text = "Record Management" + "           Version: " + assemName.Version.ToString() + "    Database name: " + sqlCon.Database.ToString();
            sqlCon.Close();


            sqlCon.Open();
            
            menuStrip1.Visible = false;

            frmMain_Load(sender, e);

        }

        private void imageImportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmDataImport data = new frmDataImport(sqlCon, crd);
            data.ShowDialog(this);
        }

        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmExport export = new frmExport(sqlCon, crd);
            export.ShowDialog(this);
        }

        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.nevaehtech.com/");
        }

        private void batchUploadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //frmBatchUpload frm = new frmBatchUpload(sqlCon);
            //frm.ShowDialog(this);


            frmBundleUpload frm = new frmBundleUpload(sqlCon);
            frm.ShowDialog(this);
        }

        private void caseFileCreationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmBundleSummary frm = new frmBundleSummary(sqlCon);
            frm.ShowDialog();
        }

        public void SetValues(wItem pBox, int prmMode)
        {
            wi = (wfeBox)pBox;
            colorMode = prmMode;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            eSTATES[] state = new eSTATES[1];
            state[0] = eSTATES.POLICY_CREATED;
            frmBundleSelect box = new frmBundleSelect(state, sqlCon);
            box.chkPhotoScan.Visible = true;
            box.ShowDialog(this);

            projKey = frmBundleSelect.projKey;
            bundleKey = frmBundleSelect.bundleKey;
            if (projKey != null && bundleKey != null)
            {
                //status check
                if (_GetBundleStatus(projKey, bundleKey).Rows[0][0].ToString() == "1")
                {
                    //file count greater than one and file count and meta count matches
                    if (Convert.ToInt32(_GetFileCount(projKey, bundleKey).ToString()) > 0 && Convert.ToInt32(_GetFileCount(projKey, bundleKey).ToString()) == Convert.ToInt32(_GetEntryCount(projKey, bundleKey).ToString()))
                    {
                        aePolicyScan frmScan = new aePolicyScan(sqlCon, crd, colorMode);

                        frmScan.ShowDialog(this);
                    }
                }
            }
        }

        private void bundleScanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            eSTATES[] state = new eSTATES[1];
            state[0] = eSTATES.POLICY_CREATED;
            frmBundleSelect box = new frmBundleSelect(state, sqlCon);
            box.chkPhotoScan.Visible = true;
            box.ShowDialog(this);

            projKey = frmBundleSelect.projKey;
            bundleKey = frmBundleSelect.bundleKey;

            if(projKey != null && bundleKey != null)
            {
                //status check
                if(_GetBundleStatus(projKey,bundleKey).Rows[0][0].ToString() == "1")
                {
                    //file count greater than one and file count and meta count matches
                    if(Convert.ToInt32(_GetFileCount(projKey,bundleKey).ToString()) > 0 && Convert.ToInt32(_GetFileCount(projKey,bundleKey).ToString()) == Convert.ToInt32(_GetEntryCount(projKey,bundleKey).ToString()))
                    {
                        aePolicyScan frmScan = new aePolicyScan(sqlCon, crd, colorMode);
                        
                        frmScan.ShowDialog(this);
                    }
                }
            }
        }

        private void imageQualityControlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            eSTATES[] state = new eSTATES[1];
            state[0] = eSTATES.POLICY_SCANNED;
            frmBundleSelect box = new frmBundleSelect(state, sqlCon);
            box.chkPhotoScan.Visible = false;
            box.ShowDialog(this);

            projKey = frmBundleSelect.projKey;
            bundleKey = frmBundleSelect.bundleKey;

            if (projKey != null && bundleKey != null)
            {
                //status check
                if (_GetBundleStatus(projKey, bundleKey).Rows[0][0].ToString() == "2")
                {
                    if (Convert.ToInt32(_GetFileCount(projKey, bundleKey).ToString()) > 0 && Convert.ToInt32(_GetFileCount(projKey, bundleKey).ToString()) == Convert.ToInt32(_GetEntryCount(projKey, bundleKey).ToString()))
                    {
                        aeImageQC frmQc = new aeImageQC(sqlCon, crd);
                        //frmQc.MdiParent = this;
                        //frmQc.Height = this.ClientRectangle.Height;
                        //frmQc.Width = this.ClientRectangle.Width;
                        frmQc.ShowDialog(this);
                    }
                }
            }
        }

        private void indexingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            eSTATES[] state = new eSTATES[2];
            state[0] = eSTATES.POLICY_QC;
            state[1] = eSTATES.POLICY_ON_HOLD;

            frmBundleSelect box = new frmBundleSelect(state, sqlCon);
            box.chkPhotoScan.Visible = false;
            box.ShowDialog(this);

            projKey = frmBundleSelect.projKey;
            bundleKey = frmBundleSelect.bundleKey;

            if (projKey != null && bundleKey != null)
            {
                //status check
                if (_GetBundleStatus(projKey, bundleKey).Rows[0][0].ToString() == "3")
                {
                    if (Convert.ToInt32(_GetFileCount(projKey, bundleKey).ToString()) > 0 && Convert.ToInt32(_GetFileCount(projKey, bundleKey).ToString()) == Convert.ToInt32(_GetEntryCount(projKey, bundleKey).ToString()))
                    {
                        aeIndexing frmIndex = new aeIndexing(sqlCon, crd);

                        frmIndex.ShowDialog(this);
                    }
                }
            }


        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            eSTATES[] state = new eSTATES[1];
            state[0] = eSTATES.POLICY_SCANNED;
            frmBundleSelect box = new frmBundleSelect(state, sqlCon);
            box.chkPhotoScan.Visible = false;
            box.ShowDialog(this);

            projKey = frmBundleSelect.projKey;
            bundleKey = frmBundleSelect.bundleKey;

            if (projKey != null && bundleKey != null)
            {
                //status check
                if (_GetBundleStatus(projKey, bundleKey).Rows[0][0].ToString() == "2")
                {
                    if (Convert.ToInt32(_GetFileCount(projKey, bundleKey).ToString()) > 0 && Convert.ToInt32(_GetFileCount(projKey, bundleKey).ToString()) == Convert.ToInt32(_GetEntryCount(projKey, bundleKey).ToString()))
                    {
                        aeImageQC frmQc = new aeImageQC(sqlCon, crd);
                        
                        frmQc.ShowDialog(this);
                    }
                }
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            eSTATES[] state = new eSTATES[2];
            state[0] = eSTATES.POLICY_QC;
            state[1] = eSTATES.POLICY_ON_HOLD;

            frmBundleSelect box = new frmBundleSelect(state, sqlCon);
            box.chkPhotoScan.Visible = false;
            box.ShowDialog(this);

            projKey = frmBundleSelect.projKey;
            bundleKey = frmBundleSelect.bundleKey;

            if (projKey != null && bundleKey != null)
            {
                //status check
                if (_GetBundleStatus(projKey, bundleKey).Rows[0][0].ToString() == "3")
                {
                    if (Convert.ToInt32(_GetFileCount(projKey, bundleKey).ToString()) > 0 && Convert.ToInt32(_GetFileCount(projKey, bundleKey).ToString()) == Convert.ToInt32(_GetEntryCount(projKey, bundleKey).ToString()))
                    {
                        aeIndexing frmIndex = new aeIndexing(sqlCon, crd);

                        frmIndex.ShowDialog(this);
                    }
                }
            }
        }

        private void qualityControlFinalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NovaNet.wfe.eSTATES[] state = new NovaNet.wfe.eSTATES[2];
            //state[0] = NovaNet.wfe.eSTATES.POLICY_EXCEPTION;
            state[0] = NovaNet.wfe.eSTATES.POLICY_INDEXED;
            //state[2] = NovaNet.wfe.eSTATES.POLICY_FQC;
            state[1] = NovaNet.wfe.eSTATES.POLICY_ON_HOLD;
            //state[4] = NovaNet.wfe.eSTATES.POLICY_CHECKED;
            //state[5] = NovaNet.wfe.eSTATES.POLICY_NOT_INDEXED;
            //state[6] = NovaNet.wfe.eSTATES.POLICY_EXPORTED;
            //state[7] = NovaNet.wfe.eSTATES.POLICY_QC;
            //state[8] = NovaNet.wfe.eSTATES.POLICY_SUBMITTED;

            frmBundleSelect box = new frmBundleSelect(state, sqlCon);
            box.chkPhotoScan.Visible = false;
            box.ShowDialog(this);

            projKey = frmBundleSelect.projKey;
            bundleKey = frmBundleSelect.bundleKey;

            if (projKey != null && bundleKey != null)
            {
                //status check
                if (_GetBundleStatus(projKey, bundleKey).Rows[0][0].ToString() == "4" || _GetBundleStatus(projKey, bundleKey).Rows[0][0].ToString() == "5" || _GetBundleStatus(projKey, bundleKey).Rows[0][0].ToString() == "6" || _GetBundleStatus(projKey, bundleKey).Rows[0][0].ToString() == "7" || _GetBundleStatus(projKey, bundleKey).Rows[0][0].ToString() == "8" || _GetBundleStatus(projKey, bundleKey).Rows[0][0].ToString() == "30" || _GetBundleStatus(projKey, bundleKey).Rows[0][0].ToString() == "31" || _GetBundleStatus(projKey, bundleKey).Rows[0][0].ToString() == "37" || _GetBundleStatus(projKey, bundleKey).Rows[0][0].ToString() == "40" || _GetBundleStatus(projKey, bundleKey).Rows[0][0].ToString() == "77")
                {
                    if (Convert.ToInt32(_GetFileCount(projKey, bundleKey).ToString()) > 0 && Convert.ToInt32(_GetFileCount(projKey, bundleKey).ToString()) == Convert.ToInt32(_GetEntryCount(projKey, bundleKey).ToString()))
                    {
                        aeFQC frm = new aeFQC(sqlCon, crd);
                        frm.ShowDialog(this);
                    }
                }
                else
                {
                    //MessageBox.Show("This Bundle is exported already... ", " ", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
            }
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            NovaNet.wfe.eSTATES[] state = new NovaNet.wfe.eSTATES[2];
            //state[0] = NovaNet.wfe.eSTATES.POLICY_EXCEPTION;
            state[0] = NovaNet.wfe.eSTATES.POLICY_INDEXED;
            //state[2] = NovaNet.wfe.eSTATES.POLICY_FQC;
            state[1] = NovaNet.wfe.eSTATES.POLICY_ON_HOLD;
            //state[4] = NovaNet.wfe.eSTATES.POLICY_CHECKED;
            //state[5] = NovaNet.wfe.eSTATES.POLICY_NOT_INDEXED;
            //state[6] = NovaNet.wfe.eSTATES.POLICY_EXPORTED;
            //state[7] = NovaNet.wfe.eSTATES.POLICY_QC;
            //state[8] = NovaNet.wfe.eSTATES.POLICY_SUBMITTED;

            frmBundleSelect box = new frmBundleSelect(state, sqlCon);
            box.chkPhotoScan.Visible = false;
            box.ShowDialog(this);

            projKey = frmBundleSelect.projKey;
            bundleKey = frmBundleSelect.bundleKey;

            if (projKey != null && bundleKey != null)
            {
                //status check
                if (_GetBundleStatus(projKey, bundleKey).Rows[0][0].ToString() == "4" || _GetBundleStatus(projKey, bundleKey).Rows[0][0].ToString() == "5" || _GetBundleStatus(projKey, bundleKey).Rows[0][0].ToString() == "6" || _GetBundleStatus(projKey, bundleKey).Rows[0][0].ToString() == "7" || _GetBundleStatus(projKey, bundleKey).Rows[0][0].ToString() == "8" || _GetBundleStatus(projKey, bundleKey).Rows[0][0].ToString() == "30" || _GetBundleStatus(projKey, bundleKey).Rows[0][0].ToString() == "31" || _GetBundleStatus(projKey, bundleKey).Rows[0][0].ToString() == "37" || _GetBundleStatus(projKey, bundleKey).Rows[0][0].ToString() == "40" || _GetBundleStatus(projKey, bundleKey).Rows[0][0].ToString() == "77")
                {
                    if (Convert.ToInt32(_GetFileCount(projKey, bundleKey).ToString()) > 0 && Convert.ToInt32(_GetFileCount(projKey, bundleKey).ToString()) == Convert.ToInt32(_GetEntryCount(projKey, bundleKey).ToString()))
                    {
                        aeFQC frm = new aeFQC(sqlCon, crd);
                        frm.ShowDialog(this);
                    }
                }
                else
                {
                    //MessageBox.Show("This Bundle is exported already... ", " ", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //frmSubmit frm = new frmSubmit(sqlCon, crd);
            //frm.ShowDialog(this);


            frmBundleSubmit frm = new frmBundleSubmit(sqlCon,crd);
            frm.ShowDialog(this);
        }

        private void nRSAuditToolStripMenuItem_Click(object sender, EventArgs e)
        {
            aeLicQa frmLicQA = new aeLicQa(sqlCon, crd);
            //frmLicQA.MdiParent=this;
            frmLicQA.Height = this.ClientRectangle.Height;
            frmLicQA.Width = this.ClientRectangle.Width;
            frmLicQA.ShowDialog(this);
        }

        private void searchToolStripMenuItem_Click(object sender, EventArgs e)
        {
        
            frmSearch fm= new frmSearch(sqlCon, crd);

            fm.ShowDialog(this);

        }

        
    }
}
