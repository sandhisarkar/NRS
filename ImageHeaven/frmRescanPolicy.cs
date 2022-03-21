using System;
using System.Drawing;
using System.Windows.Forms;
using NovaNet.Utils;
using NovaNet.wfe;
using System.Data;
using System.Data.Odbc;
using System.IO;
using LItems;
using System.Collections;
using TwainLib;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;

namespace ImageHeaven
{
    public partial class frmRescanPolicy : Form
    {
        private wfeBox wBox = null;
        private OdbcConnection sqlCon;
        private OdbcDataAdapter sqlAdap = null;
        private wfeBatch wBatch = null;
        NovaNet.Utils.dbCon dbcon = new NovaNet.Utils.dbCon();
        string scanFolder = null;
        //private IContainer components;
        public static NovaNet.Utils.exLog.Logger exMailLog = new NovaNet.Utils.exLog.emailLogger("./errLog.log", NovaNet.Utils.exLog.LogLevel.Dev, Constants._MAIL_TO, Constants._MAIL_FROM, Constants._SMTP);
        public static NovaNet.Utils.exLog.Logger exTxtLog = new NovaNet.Utils.exLog.txtLogger("./errLog.log", NovaNet.Utils.exLog.LogLevel.Dev);
        string batchPath = null;
        ci tmpImg;
        Credentials crd = new Credentials();
        private int zoomWidth;
        private int zoomHeight;
        private Size zoomSize = new Size();
        string imagename = null;
        private int keyPressed = 1;
        int scanMode;
        public static string projKey;
        public static string bundleKey;

        public frmRescanPolicy(wfeBox pBox,OdbcConnection pSql,Credentials prmCrd,int pScanMode)
        {
            wBox = pBox;
            scanMode = pScanMode;
            tmpImg = (ci)IgrFactory.GetImagery(Constants.IGR_CLEARIMAGE);
            sqlCon = pSql;
            crd = prmCrd;
            InitializeComponent();
        }
        public frmRescanPolicy(OdbcConnection pSql, Credentials prmCrd, int pScanMode)
        {
            
            scanMode = pScanMode;
            tmpImg = (ci)IgrFactory.GetImagery(Constants.IGR_CLEARIMAGE);
            sqlCon = pSql;
            crd = prmCrd;
            InitializeComponent();
        }
        private void frmRescanPolicy_Load(object sender, EventArgs e)
        {
            projKey = aePolicyScan.projKey;
            bundleKey = aePolicyScan.bundleKey;
            PopulateList();
        }
        public void PopulateList()
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            eSTATES[] state = new eSTATES[1];
            CtrlPolicy ctrlPolicy = null;

            dbcon = new NovaNet.Utils.dbCon();
            ArrayList arrPolicy = new ArrayList();
            wQuery pQuery = new ihwQuery(sqlCon, scanMode,ihConstants._RESCAN);
            state[0] = eSTATES.POLICY_SCANNED;
            arrPolicy = GetItems(eITEMS.POLICY, state);
            lvwExportList.Items.Clear();
            for (int i = 0; i < arrPolicy.Count; i++)
            {
                ctrlPolicy =(CtrlPolicy) arrPolicy[i];
                ListViewItem lvwItem = lvwExportList.Items.Add(ctrlPolicy.PolicyNumber.ToString());
            }
         }

        public ArrayList GetItems(eITEMS item, eSTATES[] state)
        {
            OdbcDataAdapter wAdap;
            OdbcTransaction trns = null;
            OdbcCommand oCom = new OdbcCommand();
            string strQuery = null;
            wItemControl wic = null;
            DataSet ds = new DataSet();
            string strQr = string.Empty;
            //wfePolicy queryPolicy = (wfePolicy)wi;
            ArrayList arrItem = new ArrayList();

            if (item == eITEMS.POLICY)
            {
                if (state.Length != 0)
                {
                    strQuery = "select proj_code,bundle_key,patient_id from metadata_entry where proj_code= '" + projKey + "' and bundle_key='" + bundleKey + "' and status ='1'";

                    oCom.Connection = sqlCon;
                    oCom.CommandText = strQuery;
                    wAdap = new OdbcDataAdapter(oCom);
                    wAdap.Fill(ds);

                    if (ds.Tables[0].Rows.Count > 0)
                    {

                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            wic = new CtrlPolicy((int)ds.Tables[0].Rows[i]["proj_code"], (int)ds.Tables[0].Rows[i]["bundle_key"], "1", ds.Tables[0].Rows[i]["patient_id"].ToString());
                            arrItem.Add(wic);
                        }
                    }
                }
            }

            return arrItem;
        }

        public DataSet GetBundlePath(string prmProjKey, string prmBundleKey)
        {
            string sqlStr = null;
            DataSet bundleDs = new DataSet();

            try
            {
                sqlStr = @"select bundle_path,bundle_code from bundle_master where proj_code = '" + prmProjKey + "' and bundle_key = '" + prmBundleKey + "' ";
                sqlAdap = new OdbcDataAdapter(sqlStr, sqlCon);
                sqlAdap.Fill(bundleDs);

                //Test whether the path exists or not
                //throw new ValidationException(1234);
            }
            catch (OdbcException ex)
            {
                sqlAdap.Dispose();

            }
            return bundleDs;
        }

        private void lvwExportList_SelectedIndexChanged(object sender, EventArgs e)
        {
            eSTATES[] prmPolicyState;
            ArrayList arrImage = new ArrayList();
            wQuery pQuery = new ihwQuery(sqlCon);
            CtrlPolicy ctrlPolicy = new CtrlPolicy(Convert.ToInt32(projKey),Convert.ToInt32(bundleKey), "1", lvwExportList.FocusedItem.Text);
            wItem policy = new wfePolicy(sqlCon, ctrlPolicy);
            wBatch = new wfeBatch(sqlCon);
            batchPath = GetBundlePath(projKey,bundleKey).Tables[0].Rows[0][0].ToString();
            CtrlImage pImage = new CtrlImage(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), "1", lvwExportList.FocusedItem.Text, string.Empty, string.Empty);
            wfeImage wImage = new wfeImage(sqlCon, pImage);
            CtrlImage ctrlImage;

            prmPolicyState = new eSTATES[1];
            prmPolicyState[0] = eSTATES.POLICY_SCANNED;
            arrImage = GetItemsSelected(eITEMS.PAGE, prmPolicyState, lvwExportList.FocusedItem.Text);

            lstImageName.Items.Clear();
            if (arrImage.Count > 0)
            {
                for (int l = 0; l < arrImage.Count; l++)
                {
                    ctrlImage = (CtrlImage)arrImage[l];
                    lstImageName.Items.Add(ctrlImage.ImageName);
                }
            }
        }

        public ArrayList GetItemsSelected(eITEMS item, eSTATES[] state, string casefileno)
        {
            OdbcDataAdapter wAdap;
            OdbcTransaction trns = null;
            OdbcCommand oCom = new OdbcCommand();
            string strQuery = null;
            wItemControl wic = null;
            DataSet ds = new DataSet();
            string strQr = string.Empty;
            //wfePolicy queryPolicy = (wfePolicy)wi;
            ArrayList arrItem = new ArrayList();

            if (item == eITEMS.PAGE)
            {
                if (state.Length != 0)
                {
                    strQuery = "select distinct A.proj_key,A.batch_key,A.box_number,A.policy_number,A.page_name,A.page_index_name,A.doc_type from image_master A,metadata_entry B where A.proj_key=B.proj_code and A.batch_key = B.bundle_key  and A.policy_number = B.patient_id and A.photo <> 1 and A.proj_key=" + projKey + " and A.batch_key=" + bundleKey + " and  A.policy_number='" + casefileno + "'";

                    oCom.Connection = sqlCon;
                    oCom.CommandText = strQuery;
                    wAdap = new OdbcDataAdapter(oCom);
                    wAdap.Fill(ds);

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {

                            wic = new CtrlImage(Convert.ToInt32(ds.Tables[0].Rows[i]["proj_key"].ToString()), Convert.ToInt32(ds.Tables[0].Rows[i]["batch_key"].ToString()), "1", ds.Tables[0].Rows[i]["policy_number"].ToString(), ds.Tables[0].Rows[i]["page_name"].ToString(), ds.Tables[0].Rows[i]["doc_type"].ToString());
                            arrItem.Add(wic);
                        }
                    }
                }
            }

            return arrItem;
        }

        private void ChangeSize(string fName)
        {
            Image imgTot = null;
            try
            {
                if (tmpImg.IsValid() == true)
                {
                    if (picDel.Dock != DockStyle.Fill)
                    {
                        //picDel.Dock = DockStyle.Fill;
                    }
                    picDel.Width = panel1.Width - 2;
                    picDel.Height = panel1.Height - 5;
                    if (!System.IO.File.Exists(fName)) return;
                    Image newImage;
                    tmpImg.LoadBitmapFromFile(fName);
                    if (tmpImg.GetBitmap().PixelFormat == PixelFormat.Format24bppRgb)
                    {
                        tmpImg.GetLZW("tmp.TIF");
                        imgTot = Image.FromFile("tmp.TIF");
                        newImage = imgTot;
                        //File.Delete("tmp1.TIF");
                    }
                    else
                    {
                        newImage = System.Drawing.Image.FromFile(fName);
                    }
                    double scaleX = (double)picDel.Width / (double)newImage.Width;
                    double scaleY = (double)picDel.Height / (double)newImage.Height;
                    double Scale = Math.Min(scaleX, scaleY);
                    int w = (int)(newImage.Width * Scale);
                    int h = (int)(newImage.Height * Scale);
                    picDel.Width = w;
                    picDel.Height = h - 5;
                    picDel.Image = newImage.GetThumbnailImage(w, h, new System.Drawing.Image.GetThumbnailImageAbort(GetThumbnailImageAbort), IntPtr.Zero);
                    if (imgTot != null)
                    {
                        imgTot.Dispose();
                        imgTot = null;
                        if (File.Exists("tmp.tif"))
                            File.Delete("tmp.TIF");
                    }
                    if (newImage != null)
                    {
                        newImage.Dispose();
                        newImage = null;
                    }
                }
            }
            catch (Exception ex)
            {
                exMailLog.Log(ex);
                MessageBox.Show("Error while cropping the image" + ex.Message, "Crop error");
            }
        }

        private void lstImageName_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            try
            {
                if (lstImageName.SelectedIndex >= 0)
                {
                    imagename = lstImageName.SelectedItem.ToString();
                    imagename = batchPath + "\\" + lvwExportList.FocusedItem.Text + "\\" + ihConstants._SCAN_FOLDER + "\\" + imagename;
                    tmpImg.LoadBitmapFromFile(imagename);
                    picDel.Image = tmpImg.GetBitmap();
                    ChangeSize(imagename);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while showing the image.." + ex.Message);
                exMailLog.Log(ex);
            }
        }

        public bool UpdateBundleStatus(int prmProjKey, int prmBatchKey)
        {
            string sqlStr = null;
            OdbcTransaction sqlTrans = null;
            bool commitBol = true;

            OdbcCommand sqlCmd = new OdbcCommand();

            sqlStr = @"update bundle_master" +
                " set status=" + 1 + " where " +
                " bundle_key=" + prmBatchKey + " and proj_code = '" + prmProjKey + "' ";

            try
            {

                sqlTrans = sqlCon.BeginTransaction();
                sqlCmd.Connection = sqlCon;
                sqlCmd.Transaction = sqlTrans;
                sqlCmd.CommandText = sqlStr;
                int i = sqlCmd.ExecuteNonQuery();
                sqlTrans.Commit();
                commitBol = true;
            }
            catch (Exception ex)
            {
                commitBol = false;
                sqlTrans.Rollback();
                sqlCmd.Dispose();
                MessageBox.Show(ex.Message);
            }
            return commitBol;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult result;
            CtrlPolicy pPolicy=null;
            wfePolicy wPolicy = null;
            string policyFolder;
            string scanFolder;
            try
            {
                result = MessageBox.Show(this, "Are you sure you want to delete all the selected case file folder..?", "Confirmation", MessageBoxButtons.YesNo);
                if (lvwExportList.Items.Count > 0)
                {
                    if (result == DialogResult.Yes)
                    {
                        for (int o = 0; o < lvwExportList.Items.Count; o++)
                        {
                            if (lvwExportList.Items[o].Checked == true)
                            {
                                pPolicy = new CtrlPolicy(Convert.ToInt32(projKey),Convert.ToInt32(bundleKey), "1", lvwExportList.Items[o].Text.ToString());
                                wPolicy = new wfePolicy(sqlCon, pPolicy);
                                if (wPolicy.UpdateFilePrevStatus(eSTATES.POLICY_CREATED, crd) == true && UpdateBundleStatus(Convert.ToInt32(projKey),Convert.ToInt32(bundleKey)) == true)
                                {
                                
                                    if (wPolicy.DeleteAllPage())
                                    {
                                        policyFolder = batchPath + "\\" + lvwExportList.Items[o].Text;
                                        scanFolder = policyFolder + "\\" + ihConstants._SCAN_FOLDER;
                                        if (Directory.Exists(policyFolder))
                                        {
                                            //Directory.Delete(scanFolder);
                                            Directory.Delete(policyFolder, true);
                                        }
                                    }
                                }
                                   
                            }
                        }
                        PopulateList();
                        lstImageName.Items.Clear();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while deleting the folder.. " + ex.Message);
            }
        }
        int ZoomIn()
        {
            try
            {
                if (tmpImg.IsValid() == true)
                {
                    picDel.Dock = DockStyle.None;
                    keyPressed = keyPressed + 1;
                    zoomHeight = Convert.ToInt32(tmpImg.GetBitmap().Height * (1.2));
                    zoomWidth = Convert.ToInt32(tmpImg.GetBitmap().Width * (1.2));
                    zoomSize.Height = zoomHeight;
                    zoomSize.Width = zoomWidth;

                    picDel.Width = Convert.ToInt32(Convert.ToDouble(picDel.Width) * 1.2);
                    picDel.Height = Convert.ToInt32(Convert.ToDouble(picDel.Height) * 1.2);
                    picDel.Refresh();
                    ChangeZoomSize();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while zooming the image " + ex.Message, "Zoom Error");
                exMailLog.Log(ex);
            }
            return 0;
        }
        private void ChangeZoomSize()
        {
            if (!System.IO.File.Exists(imagename)) return;
            Image newImage = Image.FromFile(imagename);
            double scaleX = (double)picDel.Width / (double)newImage.Width;
            double scaleY = (double)picDel.Height / (double)newImage.Height;
            double Scale = Math.Min(scaleX, scaleY);
            int w = (int)(newImage.Width * Scale);
            int h = (int)(newImage.Height * Scale);
            //picDel.Width = w;
            //picDel.Height = h;
            picDel.Image = newImage.GetThumbnailImage(w, h, new System.Drawing.Image.GetThumbnailImageAbort(GetThumbnailImageAbort), IntPtr.Zero);
            newImage.Dispose();
        }
        private bool GetThumbnailImageAbort()
        {
            return false;
        }
        int ZoomOut()
        {
            try
            {
                if (keyPressed > 0)
                {
                    picDel.Dock = DockStyle.None;
                    keyPressed = keyPressed + 1;
                    zoomHeight = Convert.ToInt32(tmpImg.GetBitmap().Height / (1.2));
                    zoomWidth = Convert.ToInt32(tmpImg.GetBitmap().Width / (1.2));
                    zoomSize.Height = zoomHeight;
                    zoomSize.Width = zoomWidth;

                    picDel.Width = Convert.ToInt32(Convert.ToDouble(picDel.Width) / 1.2);
                    picDel.Height = Convert.ToInt32(Convert.ToDouble(picDel.Height) / 1.2);
                    picDel.Refresh();
                    ChangeZoomSize();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while zooming the image " + ex.Message, "Zoom Error");
            }
            return 0;
        }

        private void frmRescanPolicy_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {
                ZoomIn();
            }
            if (e.KeyCode == Keys.F2)
            {
                ZoomOut();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ZoomIn();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ZoomOut();
        }

    }
}