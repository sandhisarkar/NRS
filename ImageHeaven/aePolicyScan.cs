/*
 * Created by SharpDevelop.
 * User: SubhajitB
 * Date: 1/4/2008
 * Time: 4:51 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.ComponentModel;
using System.Data.Odbc;
using System.Data;
using System.Collections;
using System.Threading;
using System.IO;
using LItems;
using NovaNet.Utils;
using NovaNet.wfe;
using GdiPlusLib;
using TwainLib;
using System.Runtime.InteropServices;

namespace ImageHeaven
{
	/// <summary>
	/// Description of aePolicyScan.
	/// </summary>
    public partial class aePolicyScan : Form, IMessageFilter
    {
        private wfeBox wBox = null;
        private OdbcConnection sqlCon;
        private wfeBatch pBatch = null;
        private wfeProject pProject = null;
        public static string projKey = null;
        public static string bundleKey = null;

        private OdbcDataAdapter sqlAdap = null;
        //private ADFScanUtils scanUtil=null;
        private wfeBatch wBatch = null;
        NovaNet.Utils.dbCon dbcon = new NovaNet.Utils.dbCon();
        private CtrlPolicy pPolicy = null;
        private CtrlBox pBox = null;
        //private TImgDisp timg = new TImgDisp();
        string scanFolder = null;
        //private IContainer components;
        private bool msgfilter;
        private Twain tw;
        private bool FlatBedScan = false;
        //private int		picnumber = 0;
        private int i;
        private int j;
        ArrayList policyList;
        private CtrlImage pImage = null;
        public static NovaNet.Utils.exLog.Logger exMailLog = new NovaNet.Utils.exLog.emailLogger("./errLog.log", NovaNet.Utils.exLog.LogLevel.Dev, Constants._MAIL_TO, Constants._MAIL_FROM, Constants._SMTP);
        public static NovaNet.Utils.exLog.Logger exTxtLog = new NovaNet.Utils.exLog.txtLogger("./errLog.log", NovaNet.Utils.exLog.LogLevel.Dev);
        string batchPath = null;
        string scanDate;
        int pageCount;
        private short scanDpi;
        private short scangreyDpi;
        bool hasphoto;
        bool isOk = false;
        frmdialog dg;
        bool policyChanged = true;
        private bool colorMode;
        int blackBol;
        private long Page2 = 0;
        //Bitmap picBmp;
        ci tmpImg;
        bool SaveInColor = true;
        Credentials crd = new Credentials();
        bool hasImage = false;
        string scanSeparatorType = "1";
        string acquiretimemessage = "0";
        private int scanMode;
        
        public aePolicyScan(wfeBox prmBox, OdbcConnection prmCon, Credentials prmCrd, int prmMode)
        {
            //
            // The InitializeComponent() call is required for Windows Forms designer support.
            //
            sqlCon = prmCon;
            wBox = prmBox;
            scanMode = prmMode;
            InitializeComponent();
            this.Text = "Lot Scanning"+ "Connection: "+sqlCon.ConnectionString.ToString();
            tmpImg = (ci)IgrFactory.GetImagery(Constants.IGR_CLEARIMAGE);
            crd = prmCrd;
            exMailLog.SetNextLogger(exTxtLog);

            //
            // TODO: Add constructor code after the InitializeComponent() call.
            //
        }

        public aePolicyScan(OdbcConnection prmCon, Credentials prmCrd, int prmMode)
        {
            //
            // The InitializeComponent() call is required for Windows Forms designer support.
            //
            sqlCon = prmCon;
            //wBox = prmBox;
            scanMode = prmMode;
            InitializeComponent();
            this.Text = "Bundle Scanning" + "Connection: " + sqlCon.ConnectionString.ToString();
            tmpImg = (ci)IgrFactory.GetImagery(Constants.IGR_CLEARIMAGE);
            crd = prmCrd;
            exMailLog.SetNextLogger(exTxtLog);

            //
            // TODO: Add constructor code after the InitializeComponent() call.
            //
        }

        public aePolicyScan()
        {
            InitializeComponent();
            this.Text = "Scan Centre";
            exMailLog.SetNextLogger(exTxtLog);
        }
        

        void AePolicyScanLoad(object sender, EventArgs e)
        {
            DisplayValues();
            tw = new Twain();
            tw.Init(this.Handle);
            TwainLib.Twain.TwainEventNotification delTEvn = new TwainLib.Twain.TwainEventNotification(GetNotification);
            tw.SetNotification(delTEvn);
            ShowPolicy();
            PrevImages();
            cmdCancelScan.ForeColor = Color.Black;
            cmdScan.ForeColor = Color.Black;
            lblBatch.ForeColor = Color.RoyalBlue;
            lblBox.ForeColor = Color.RoyalBlue;
            lblCurrentPolicy.ForeColor = Color.RoyalBlue;
            lblNextPolicy.ForeColor = Color.RoyalBlue;
            //lblPageCount.ForeColor = Color.RoyalBlue;
            lblPicSize.ForeColor = Color.RoyalBlue;
            lblProjectName.ForeColor = Color.RoyalBlue;
            lblSize.ForeColor = Color.Black;
            label1.ForeColor = Color.Black;
            label2.ForeColor = Color.Black;
            label3.ForeColor = Color.Black;
            label4.ForeColor = Color.Black;
            label5.ForeColor = Color.Black;
            label6.ForeColor = Color.Black;
            label9.ForeColor = Color.Black;
            ImageConfig config = new ImageConfig(ihConstants.CONFIG_FILE_PATH);
            scanSeparatorType = config.GetValue(ihConstants._SCAN_SECTION, ihConstants._SCAN_KEY).Replace("\0", "");
            acquiretimemessage = config.GetValue(ihConstants._SCAN_SECTION, ihConstants._ACQUIRE_TIME_MESSAGE_KEY).Replace("\0", "");

            //if (txtTotalpages.Text != "")
            //{
            //    txtRemainingPages.Text = (Convert.ToInt32(txtTotalpages.Text) - lstImageName.Items.Count).ToString();
            //}
            //int dpi = Convert.ToInt32(config.GetValue(ihConstants._SCAN_SECTION, ihConstants._SCAN_DPI));
        }
        void DisplayValues()
        {
            //pBatch = new wfeBatch(sqlCon);
            //pProject = new wfeProject(sqlCon);
            projKey = frmMain.projKey;
            bundleKey = frmMain.bundleKey;
            //lblProjectName.Text = pProject.GetProjectName(wBox.ctrlBox.ProjectCode);
            //lblBatch.Text = pBatch.GetBatchName(wBox.ctrlBox.ProjectCode, wBox.ctrlBox.BatchKey);
            //lblBox.Text = wBox.ctrlBox.BoxNumber.ToString();
        }
        /// <summary>
        /// It's a message filter interface
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        bool IMessageFilter.PreFilterMessage(ref Message m)
        {
            TwainCommand cmd = tw.PassMessage(ref m);
            if ((cmd == TwainCommand.Not))
            {
                //this.Refresh();
                //Application.RemoveMessageFilter(this);
                //msgfilter = false;
                //this.Enabled = true;
                //this.Activate();
                //Thread.Sleep(2000);
                return false;
            }
            if ((cmd == TwainCommand.Null))
            {
                //EndingScan();
                //tw.CloseSrc();
                return false;
            }
            switch (cmd)
            {
                case TwainCommand.CloseRequest:
                    {
                        EndingScan();
                        tw.CloseSrc();
                        break;
                    }
                case TwainCommand.CloseOk:
                    {
                        EndingScan();
                        tw.CloseSrc();
                        break;
                    }
                case TwainCommand.DeviceEvent:
                    {
                        System.Diagnostics.Debug.Print("here");
                        break;
                    }
                case TwainCommand.TransferReady:
                    {
                        Twain.ImageNotification delMan = new Twain.ImageNotification(GetImage);
                        int pics = 0;
                        if (isOk) { grpAction.Enabled = false; }
                        //Sumit
                        if (FlatBedScan == false)
                        {
                            pics = tw.TransferPictures(GetImage, this);
                        }
                        else
                        {
                            pics = tw.TransferPicturesFixed(GetImage, this);
                        }
                        //EndingScan();
                        if (pics != -1)
                        {
                            if (colorMode)
                            {
                                if (!msgfilter)
                                {
                                    //this.Enabled = false;
                                    msgfilter = true;
                                    Application.AddMessageFilter(this);
                                }
                                if (rdADF.Checked == true)
                                {
                                    tw.Acquire(true, colorMode,scanDpi); //AcquireFixed(false, colorMode, 1, 0);
                                }
                            }
                            else
                            {
                                if (pics > 0)
                                {
                                    if (!msgfilter)
                                    {
                                        //this.Enabled = false;
                                        msgfilter = true;
                                        Application.AddMessageFilter(this);
                                    }
                                    if (rdADF.Checked == true)
                                    {
                                        //tw.Acquire(true, colorMode, scanDpi); //AcquireFixed(false, colorMode, 1, 0);
                                    }
                                }
                            }
                        }
                        break;
                    }
            }

            return true;
        }
        private void GetNotification(string pNotification)
        {
            grpAction.Enabled = true;
        }

        public void GetImage(System.IntPtr prmHBmp)
        {
            bool blankBol = true;
            bool exitPoly = true;
            string tifFileName;
            wfePolicy wPolicy = null;
            char leftPad = Convert.ToChar("0");
            CtrlPolicy ctrPolCurrent = null;
            CtrlPolicy ctrPolNext = null;
            bool success = false;
            if (blackBol == 0)
            {
                //if (colorMode==true)
                //{
                //    tw.ReqChangeMode(TwainLib.Twain.__BLACKWHITE);
                //}
                if (policyList.Count > 0)
                {
                    ////if (txtTotalpages.Text != "")
                    ////{
                    ////    txtRemainingPages.Text = (Convert.ToInt32(txtTotalpages.Text) - lstImageName.Items.Count).ToString();
                    ////}
                    //For showing the next and current policy
                    if ((policyList.Count > (j + 1)))
                    {
                        ctrPolCurrent = (CtrlPolicy)policyList[j];
                        ctrPolNext = (CtrlPolicy)policyList[j + 1];
                    }
                    else if (policyList.Count == (j + 1))
                    {
                        ctrPolCurrent = (CtrlPolicy)policyList[j];
                        ctrPolNext = (CtrlPolicy)policyList[j];
                    }
                    else
                    {
                        
                        //dg.Close();
                        MessageBox.Show("No more case files are ready to be scanned....");
                        
                        
                        if (prmHBmp != IntPtr.Zero)
                        {
                            Marshal.FreeHGlobal(prmHBmp);
                            prmHBmp = IntPtr.Zero;
                        }
                        tw.CloseSrc();
                        
                        EndingScan();
                        return;
                    }

                    lblCurrentPolicy.Text = ctrPolCurrent.PolicyNumber.ToString();
                    lblNextPolicy.Text = ctrPolNext.PolicyNumber.ToString();
                    lblCurrentPolicy.Refresh();
                    lblNextPolicy.Refresh();
                    //if (txtTotalpages.Text != "")
                    //{
                    //    txtRemainingPages.Text = (Convert.ToInt32(txtTotalpages.Text) - lstImageName.Items.Count).ToString();
                    //}
                    this.Text = "Bundle Scanning            " + " Current File- " + lblCurrentPolicy.Text + " Next File- " + lblNextPolicy.Text;
                    scanFolder = batchPath + "\\" + ctrPolCurrent.PolicyNumber.ToString() + "\\" + ihConstants._SCAN_FOLDER;
                    if (FileorFolder.CreateFolder(scanFolder) == true)
                    {
                        if (policyChanged == true)
                        {
                            pPolicy = new CtrlPolicy(Convert.ToInt32(projKey),Convert.ToInt32(bundleKey), "1", ctrPolCurrent.PolicyNumber);
                            wPolicy = new wfePolicy(sqlCon, pPolicy);
                            //pageCount = wPolicy.GetPolicyPageCount();
                            //lblPageCount.Text = pageCount.ToString();
                            policyChanged = false;
                            if (GetPolicyPhotoStatus(ctrPolCurrent.PolicyNumber) == ihConstants._POLICY_CONTAINS_PHOTO)
                            {
                                hasphoto = true;
                            }
                            else
                            {
                                hasphoto = false;
                            }
                        }
                        i = i + 1;
                        //bitmapFileName = scanFolder + "\\" + ctrPolCurrent.PolicyNumber + "_" + i.ToString().PadLeft(3, leftPad) + "_" + "A" + ".BMP";
                        tifFileName = scanFolder + "\\" + ctrPolCurrent.PolicyNumber + "_" + i.ToString().PadLeft(3, leftPad) + "_" + "A" + ".TIF";
                        if (tw.GetScanMode() == TwainLib.Twain.__COLOUR)
                        {
                            Page2++;
                            if ((hasphoto == false))
                            {
                                if (tmpImg.selfcheck == 1)
                                {
                                    MessageBox.Show("check fail");
                                }
                                tmpImg.LoadBitmapFromDIB(prmHBmp);
                                //tmpImg.ToBitonal();
                                if (tmpImg.SaveFile(tifFileName) == IGRStatus.Success)
                                {
                                    success = true;
                                }
                                else
                                {
                                    success = false;
                                }
                            }
                            else
                            {
                                if (tmpImg.selfcheck == 1)
                                {
                                    MessageBox.Show("check fail");
                                }
                                tmpImg.LoadBitmapFromDIB(prmHBmp);
                                if ((SaveInColor == false) && (hasImage == false))
                                {
                                    tmpImg.ToBitonal();
                                }
                                if ((SaveInColor == true) && (hasImage == true))
                                {
                                    tmpImg.ToBitonal();
                                }
                                if (tmpImg.SaveFile(tifFileName) == IGRStatus.Success)
                                {
                                    success = true;
                                }
                                else
                                {
                                    success = false;
                                }
                                SaveInColor = false;
                                hasImage = false;
                            }
                        }
                        else
                        {
                            if (tmpImg.selfcheck == 1)
                            {
                                MessageBox.Show("check fail");
                            }
                            tmpImg.LoadBitmapFromDIB(prmHBmp);
                            if (tmpImg.SaveFile(tifFileName) == IGRStatus.Success)
                            {
                                success = true;
                            }
                            else
                            {
                                success = false;
                            }
                        }
                        if (tmpImg.IsBlankPage() == IGRStatus.Success)
                        {
                            blankBol = false;
                        }
                        if (blankBol == false)
                        {
                            if (scanSeparatorType.ToString().Trim() == ihConstants.SCAN_SEPARATOR_BLACK)
                                exitPoly = GetBatchSplitter(tmpImg.GetBitmap());
                            else
                                exitPoly = !tmpImg.isSeparator(ihConstants.SCAN_SEPARATOR_COMPARE_STRING);

                            if (exitPoly == false)
                            {
                                blackBol = 1;
                                //if ((i - 1) != pageCount)
                                //{
                                //    //MessageBox.Show("Scanned page count mismatch with total page count entired at the time of inventry in", "Count mismatch", MessageBoxButtons.OK);
                                //    UpdatePageCountExceptionLog(pageCount, i);
                                //}

                                File.Delete(tifFileName);
                                SaveInColor = true;
                                scanDate = dbcon.GetCurrenctDTTM(1, sqlCon);
                                pPolicy = new CtrlPolicy(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), "1", ctrPolCurrent.PolicyNumber);
                                wPolicy = new wfePolicy(sqlCon, pPolicy);
                                crd.created_dttm = scanDate;
                                //wPolicy.UpdateStatus(eSTATES.POLICY_SCANNED, crd);

                                //insert into transaction log
                                wPolicy.UpdateTransactionLog(eSTATES.POLICY_SCANNED, crd);

                                //wPolicy.UpdateScanDetails(scanDate, ihConstants.SCAN_SUCCESS_FLAG);
                                pBox = new CtrlBox(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), "1");
                                wfeBox box = new wfeBox(sqlCon, pBox);
                                NovaNet.wfe.eSTATES[] state = new NovaNet.wfe.eSTATES[1];
                                state[0] = NovaNet.wfe.eSTATES.POLICY_CREATED;
                                //if (wPolicy.GetPolicyCount(state) == 0)
                                //{
                                //    box.UpdateStatus(eSTATES.BOX_SCANNED);
                                //}
                                policyChanged = true;
                                lstImageName.Items.Clear();

                                CtrlImage pImage = new CtrlImage(Convert.ToInt32(projKey),Convert.ToInt32(bundleKey), "1", ctrPolNext.PolicyNumber, string.Empty, string.Empty);
                                wfeImage wImage = new wfeImage(sqlCon, pImage);

                                int imageCount = wImage.GetImageCount();

                                i = imageCount;

                                j = j + 1;
                                //For showing the next and current policy
                                if ((policyList.Count > (j + 1)))
                                {
                                    ctrPolCurrent = (CtrlPolicy)policyList[j];
                                    ctrPolNext = (CtrlPolicy)policyList[j + 1];
                                }
                                else if (policyList.Count == (j + 1))
                                {
                                    ctrPolCurrent = (CtrlPolicy)policyList[j];
                                    ctrPolNext = (CtrlPolicy)policyList[j];
                                }
                                else
                                {
                                    
                                    //dg.Close();
                                    scanPic.Image = null;
                                    MessageBox.Show("No more case files are ready to be scanned....");
                                    
                                    if (prmHBmp != IntPtr.Zero)
                                    {
                                        Marshal.FreeHGlobal(prmHBmp);
                                        prmHBmp = IntPtr.Zero;
                                    }
                                    tw.CloseSrc();
                                    
                                    EndingScan();
                                    return;
                                }
                                lblCurrentPolicy.Text = ctrPolCurrent.PolicyNumber.ToString();
                                lblNextPolicy.Text = ctrPolNext.PolicyNumber.ToString();
                                lblCurrentPolicy.Refresh();
                                lblNextPolicy.Refresh();
                                //if (txtTotalpages.Text != "")
                                //{
                                //    txtRemainingPages.Text = (Convert.ToInt32(txtTotalpages.Text) - lstImageName.Items.Count).ToString();
                                //}
                                GetFileDetails(ctrPolCurrent.PolicyNumber.ToString());
                                //GetIndexDetails(ctrPolCurrent.PolicyNumber.ToString());
                                //PrevImages();
                                this.Text = "Bundle Scanning            " + " Current File- " + lblCurrentPolicy.Text + " Next File- " + lblNextPolicy.Text;
                            }
                        }
                        else
                        {
                            i = i - 1;
                            //bmpTif.Dispose();
                            File.Delete(tifFileName);
                        }
                        //File.Delete(bitmapFileName);
                        if (success == true)
                        {
                            if ((blankBol == false) && (exitPoly != false))
                            {
                                SetListboxValue(ctrPolCurrent.PolicyNumber + "_" + i.ToString().PadLeft(3, leftPad) + "_" + "A" + ".TIF", i);
                                tmpImg.LoadBitmapFromDIB(prmHBmp);

                                if (tmpImg.IsValid())
                                {
                                    scanPic.Image = null;
                                    scanPic.Image = tmpImg.GetBitmap();
                                    scanPic.Refresh();
                                    panel2.Height = this.ClientSize.Height - 25;
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("Error while saving files aborting....");
                            if (prmHBmp != IntPtr.Zero)
                            {
                                Marshal.FreeHGlobal(prmHBmp);
                                prmHBmp = IntPtr.Zero;
                            }
                            tw.CloseSrc();
                            
                            EndingScan();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("No more case files remain to be scanned....");
                    scanPic.Image = null;
                    //dg.Close();
                    tw.CloseSrc();
                    
                    EndingScan();
                }
            }
            else
            {
                blackBol = 0;
            }
            if (prmHBmp != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(prmHBmp);
                prmHBmp = IntPtr.Zero;
            }
            cmdScan.Focus();

        }

        public int GetPolicyPhotoStatus(string pID)
        {
            string sqlStr = null;

            DataSet policyDs = new DataSet();

            try
            {
                sqlStr = "select photo from metadata_entry where proj_code=" + projKey + " and bundle_key=" + bundleKey + " and patient_id='" + pID + "'";
                sqlAdap = new OdbcDataAdapter(sqlStr, sqlCon);
                sqlAdap.Fill(policyDs);
            }
            catch (Exception ex)
            {
                sqlAdap.Dispose();
                
            }

            return Convert.ToInt32(policyDs.Tables[0].Rows[0]["photo"].ToString());
        }
        /// <summary>
        /// Initialize box and all policy status, added on 28/07/2010------------------------------------
        /// </summary>
        private void InitializePolicyAndBox()
        {
            pBox = new CtrlBox(wBox.ctrlBox.ProjectCode, wBox.ctrlBox.BatchKey, wBox.ctrlBox.BoxNumber);
            wfeBox box = new wfeBox(sqlCon, pBox);
            if (box.UpdateStatus(eSTATES.BOX_CREATED) == true)
            {
                pPolicy = new CtrlPolicy(wBox.ctrlBox.ProjectCode, wBox.ctrlBox.BatchKey, wBox.ctrlBox.BoxNumber, "0");
                wfePolicy wPolicy = new wfePolicy(sqlCon, pPolicy);
                wPolicy.UpdateAllPolicyStatus(eSTATES.POLICY_CREATED, crd);
                wPolicy.DeleteTransLogEntry();
            }
        }
        /// <summary>
        /// This is used for detect black page
        /// </summary>
        /// <param name="bmp"></param>
        /// <returns>bool</returns>
        private bool GetBatchSplitter(Bitmap bmp)
        {
            bool bmpImg = true;

            int p = 0;

            for (int i = ImageHeaven.ihConstants._BATCHSPLITTER_STARTX; i < (ImageHeaven.ihConstants._BATCHSPLITTER_STARTX + ImageHeaven.ihConstants._BATCHSPLITTER_XLEN); i++)
            {
                for (int j = ImageHeaven.ihConstants._BATCHSPLITTER_STARTY; j < (ImageHeaven.ihConstants._BATCHSPLITTER_STARTY + ImageHeaven.ihConstants._BATCHSPLITTER_YLEN); j++)
                {
                    //string name = bmp.GetPixel(i, j).Name;
                    if (bmp.GetPixel(i, j).Name == "ff000000")
                    {
                        bmpImg = false;
                    }
                    else
                    {
                        if (p == 200)
                        {
                            bmpImg = true;
                            break;
                        }
                        p = p + 1;
                    }
                }
                if (bmpImg == true)
                    break;
            }
            bmp.Dispose();
            return bmpImg;
        }
        ///// <summary>
        ///// This is used for detect blank pages
        ///// </summary>
        ///// <param name="prmBmp"></param>
        ///// <returns>bool</returns>
        //private bool DetectBlankPage(Bitmap bmp)
        //{

        //    bool bmpImg = true;
        //    //int [,] SamplingMatrix = GetSamplingMatrix();
        //    DateTime dt = DateTime.Now;
        //    int p = 0;
        //    //SamplingMatrix[i,0],SamplingMatrix[i,1]

        //    for (int i = 200; i < 800; i++)
        //    {
        //        for (int j = 200; j < 800; j++)
        //        {
        //            if (bmp.GetPixel(i, j).Name == "ffffffff")
        //            {
        //                bmpImg = false;
        //            }
        //            else
        //            {
        //                if (p == 25)
        //                {
        //                    bmpImg = true;
        //                    break;
        //                }
        //                p = p + 1;
        //            }
        //        }
        //        if (bmpImg == true)
        //            break;
        //    }
        //   // bmp.Dispose();
        //    return bmpImg;
        //}
        /// <summary>
        /// Save image for group 4 compression
        /// </summary>
        /// <param name="prmSourceBmp"></param>
        /// <param name="prmDestFileName"></param>
        //private void SaveInTif(Bitmap prmSourceBmp, string prmDestFileName,EncoderValue prmEncoder)
        //{
        //    EncoderParameters ep = new EncoderParameters(1);

        //    //get ImageCodecInfo, generate tif format

        //    ImageCodecInfo info = null;
        //    foreach (ImageCodecInfo ice in ImageCodecInfo.GetImageEncoders())
        //    {
        //        if (ice.MimeType == "image/tiff")
        //        {
        //            info = ice;
        //            break;
        //        }
        //    }
        //    Image img = prmSourceBmp;
        //    ep.Param[0] = new EncoderParameter(Encoder.Compression, Convert.ToInt32(prmEncoder));
        //    img.Save(prmDestFileName, info, ep);
        //}
        /// <summary>
        /// Call for ending the scanning
        /// </summary>
        private void EndingScan()
        {
            if (msgfilter)
            {
                Application.RemoveMessageFilter(this);
                msgfilter = false;
                this.Enabled = true;
                this.Activate();
                cmdScan.Focus();
            }
        }
        //public void UpdatePageCountExceptionLog(int prmPageToBeScanned, int prmPageScanned)
        //{
        //    Credentials crd = new Credentials();

        //    pPolicy = new CtrlPolicy(wBox.ctrlBox.ProjectCode, wBox.ctrlBox.BatchKey, wBox.ctrlBox.BoxNumber, lblCurrentPolicy.Text);
        //    wfePolicy wPolicy = new wfePolicy(sqlCon, pPolicy);
        //    //crd.created_by = "ADMIN";
        //    crd.created_dttm = dbcon.GetCurrenctDTTM(1, sqlCon);
        //    wPolicy.SavePolicyPageCountException(crd, prmPageToBeScanned, prmPageScanned);
        //}
        private ArrayList GetPolicyList()
        {
            ArrayList arrPolicy = new ArrayList();
            wQuery pQuery = new ihwQuery(sqlCon, scanMode);
            eSTATES[] state = new eSTATES[1];
            state[0] = eSTATES.POLICY_CREATED;
            arrPolicy = GetItems(eITEMS.POLICY, state);
            return arrPolicy;
        }

        public ArrayList GetItems(eITEMS item, eSTATES[] state)
        {
            OdbcDataAdapter wAdap;
            OdbcTransaction trns = null;
            OdbcCommand oCom = new OdbcCommand();
            string strQuery = null;
            wItemControl wic = null;
            DataSet ds = new DataSet();
            ArrayList arrItem = new ArrayList();
            if (item == eITEMS.POLICY)
            {
                strQuery = "select proj_code,bundle_key,patient_id from metadata_entry where proj_code= '" +projKey + "' and bundle_key='" + bundleKey + "' and status ='1'";

                oCom.Connection = sqlCon;
                oCom.CommandText = strQuery;
                wAdap = new OdbcDataAdapter(oCom);
                wAdap.Fill(ds);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        string p = ds.Tables[0].Rows[i]["proj_code"].ToString();
                        string b = ds.Tables[0].Rows[i]["bundle_key"].ToString();

                        string pp = ds.Tables[0].Rows[i]["patient_id"].ToString();
                        wic = new CtrlPolicy(Convert.ToInt32(ds.Tables[0].Rows[i]["proj_code"]), Convert.ToInt32(ds.Tables[0].Rows[i]["bundle_key"]), "1", ds.Tables[0].Rows[i]["patient_id"].ToString());
                        arrItem.Add(wic);
                       
                    }
                }
            }
            return arrItem;
        }


        
        void SetListboxValue(string prmIamgeName, int prmSrlNo)
        {
            CtrlImage ctrlImg;
            //Credentials crd=new Credentials();
            long fileSize;
            System.IO.FileInfo info = new System.IO.FileInfo(scanFolder + "\\" + prmIamgeName);

            fileSize = info.Length;
            fileSize = fileSize / 1024;
            lblSize.Text = fileSize.ToString() + " KB";
            if (fileSize > 50)
            {
                lblSize.ForeColor = Color.Red;
            }
            else
            {
                lblSize.ForeColor = Color.Black;
            }
            wfeImage img;
            lstImageName.Items.Add(prmIamgeName);
            lstImageName.Refresh();
            ctrlImg = new CtrlImage(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), "1", lblCurrentPolicy.Text, prmIamgeName, string.Empty);
            img = new wfeImage(sqlCon, ctrlImg);
            img.Save(crd, eSTATES.PAGE_SCANNED, fileSize, ihConstants._NORMAL_PAGE, prmSrlNo, prmIamgeName);
        }
        private bool ShowPolicy()
        {
            CtrlPolicy ctrPolCurrent = null;
            CtrlPolicy ctrPolNext = null;

            policyList = GetPolicyList();
            if (policyList.Count > 0)
            {
                j = 0;
                if (policyList.Count > 1)
                {
                    ctrPolCurrent = (CtrlPolicy)policyList[0];
                    ctrPolNext = (CtrlPolicy)policyList[1];
                }
                else
                {
                    ctrPolCurrent = (CtrlPolicy)policyList[0];
                    ctrPolNext = (CtrlPolicy)policyList[0];
                }
                lblCurrentPolicy.Text = ctrPolCurrent.PolicyNumber.ToString();
                GetFileDetails(ctrPolCurrent.PolicyNumber.ToString());
                //GetIndexDetails(ctrPolCurrent.PolicyNumber.ToString());
                lblNextPolicy.Text = ctrPolNext.PolicyNumber.ToString();
                
                this.Text = "File Scanning            " + "  Current File- " + lblCurrentPolicy.Text + " Next File- " + lblNextPolicy.Text;
                cmdScan.Focus();
                PrevImages();
                return true;
            }
            else
            {
                MessageBox.Show("No more files remain to be scanned....");
                return false;
            }
        }

        public DataTable getFileDetails(string projKey, string bundleKey, string file_no)
        {
            DataTable dt = new DataTable();

            string sql = "select patient_name, gender,reg_number,guardian_name, dep_name,admission_date,death_date,discharge_date from metadata_entry where proj_code = '" + projKey + "' and bundle_key = '"+bundleKey+"' and patient_id = '"+file_no+"'";

            OdbcDataAdapter odap = new OdbcDataAdapter(sql, sqlCon);
            odap.Fill(dt);

            return dt;
        }

        private void GetFileDetails(string file_no)
        {
            DataTable dsVol = new DataTable();

            dsVol = getFileDetails(projKey,bundleKey,file_no);

            label12.Text = "Name : " + dsVol.Rows[0][0].ToString();
            label13.Text = "Gender : " + dsVol.Rows[0][1].ToString();
            Txtfirstparty.Text = dsVol.Rows[0][2].ToString();
            txtsecondparty.Text = dsVol.Rows[0][3].ToString();
            textBox4.Text = dsVol.Rows[0][4].ToString();
            textBox1.Text = dsVol.Rows[0][5].ToString();
            textBox2.Text = dsVol.Rows[0][6].ToString();
            textBox3.Text = dsVol.Rows[0][7].ToString();

        }

        private void GetDeedVolume1(string deed_no)
        {
            try
            {
                wQuery pQuery = new ihwQuery(sqlCon);
                DataSet dsVol = new DataSet();
                dsVol = pQuery.GetDeedVolume(wBox.ctrlBox.ProjectCode.ToString(),wBox.ctrlBox.BatchKey.ToString(),wBox.ctrlBox.BoxNumber.ToString(),lblCurrentPolicy.Text);
                //txtVol.Text = dsVol.Tables[0].Rows[0][0].ToString();
                //txtPgFrom.Text = dsVol.Tables[0].Rows[0][1].ToString();
                //txtPgTo.Text = dsVol.Tables[0].Rows[0][2].ToString();


                //if (txtPgFrom.Text != "" && txtPgTo.Text != "")
                //{
                //    txtTotalpages.Text = (Convert.ToInt32(txtPgTo.Text) - Convert.ToInt32(txtPgFrom.Text)).ToString();
                //}
            }
            catch (Exception ex)
            {

            }
        }
        private void GetIndexDetails(string deed_no)
        {
            wQuery pQuery = new ihwQuery(sqlCon);
            DataSet dsVol = new DataSet();
            string IMGName = deed_no;
            IMGName = IMGName.Split(new char[] { '[', ']' })[1];
            string deed_year = deed_no.Substring(deed_no.IndexOf("[") - 4, 4);
            dsVol = pQuery.GetIndexDetails(IMGName, deed_year);
            if (dsVol.Tables[0].Rows.Count >= 1)
            {
                lbldeedyear.Text = dsVol.Tables[0].Rows[0][0].ToString();
                lbldeedno.Text = dsVol.Tables[0].Rows[0][1].ToString();
                lblfirstparty.Text = dsVol.Tables[0].Rows[0][2].ToString();
                Txtfirstparty.Text = dsVol.Tables[0].Rows[0][3].ToString();
            }
                if (dsVol.Tables[0].Rows.Count >= 2)
                {
                    lblsecondParty.Text = dsVol.Tables[0].Rows[1][2].ToString();
                    txtsecondparty.Text = dsVol.Tables[0].Rows[1][3].ToString();
                }
                if (dsVol.Tables[0].Rows.Count > 1)
                {
                    //if (dg == null)
                    //{
                    //    dg = new frmdialog(sqlCon, crd, dsVol, PopupPoint.pnt);
                    //    dg.Show();
                    //}
                    //else { dg.Close();
                    //dg = new frmdialog(sqlCon, crd, dsVol, PopupPoint.pnt);
                    //dg.Show();
                    //}
                }
        }
        void PrevImages()
        {
            eSTATES[] prmPolicyState;

            ArrayList arrImage = new ArrayList();
            wQuery pQuery = new ihwQuery(sqlCon);
            CtrlPolicy ctrlPolicy = new CtrlPolicy(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), "1", lblCurrentPolicy.Text);
            wItem policy = new wfePolicy(sqlCon, ctrlPolicy);
            wBatch = new wfeBatch(sqlCon);
            batchPath = GetBundlePath(projKey,bundleKey).Tables[0].Rows[0][0].ToString();
            CtrlImage ctrlImage;
            
            prmPolicyState = new eSTATES[1];
            prmPolicyState[0] = eSTATES.POLICY_CREATED;
            arrImage = GetImagesItems(eITEMS.PAGE, prmPolicyState, lblCurrentPolicy.Text);

            lstImageName.Items.Clear();
            if (arrImage.Count > 0)
            {
                for (int l = 0; l < arrImage.Count; l++)
                {
                    ctrlImage = (CtrlImage)arrImage[l];
                    lstImageName.Items.Add(ctrlImage.ImageName);
                }

                scanFolder = batchPath + "\\" + lblCurrentPolicy.Text + "\\" + ihConstants._SCAN_FOLDER;
            }
            lstImageName.Refresh();
        }

        public ArrayList GetImagesItems(eITEMS item, eSTATES[] state, string case_file_no)
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
                strQuery = "select distinct A.proj_key,A.batch_key,A.box_number,A.policy_number,A.page_name,A.page_index_name,A.doc_type from image_master A,metadata_entry B where A.proj_key=B.proj_code and A.batch_key = B.bundle_key  and A.policy_number = B.patient_id and A.photo <> 1 and A.proj_key=" + projKey + " and A.batch_key=" + bundleKey + " and  A.policy_number='" + case_file_no + "'";

                oCom.Connection = sqlCon;
                oCom.CommandText = strQuery;
                wAdap = new OdbcDataAdapter(oCom);
                wAdap.Fill(ds);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {

                        wic = new CtrlImage(Convert.ToInt32(ds.Tables[0].Rows[i]["proj_key"].ToString()), Convert.ToInt32(ds.Tables[0].Rows[i]["batch_key"].ToString()),"1", ds.Tables[0].Rows[i]["policy_number"].ToString(), ds.Tables[0].Rows[i]["page_name"].ToString(), ds.Tables[0].Rows[i]["doc_type"].ToString());
                        arrItem.Add(wic);
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
                MessageBox.Show(ex.Message);
            }
            return bundleDs;
        }

        string scanColorMode = string.Empty;
        void CmdScanClick(object sender, EventArgs e)
        {
            FlatBedScan = false;
            if ((rdADF.Checked == true) || (rdFlatBed.Checked == true))
            {
                int imageCount = 0;

                if (!tw.Select()) { return; }
                SaveInColor = false;

                if (!msgfilter)
                {
                    //this.Enabled = false;
                    msgfilter = true;
                    Application.AddMessageFilter(this);
                }

                //DialogResult result = MessageBox.Show("Do you want to scan in color mode?", "Scan", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                /*
                if (scanMode == ihConstants._SCAN_WITH_PHOTO)
                {
                    colorMode = true;
                }
                else
                {
                    colorMode = false;
                }
                 */
                
                ImageConfig config = new ImageConfig(ihConstants.CONFIG_FILE_PATH);
                scanColorMode = config.GetValue(ihConstants._SCAN_SECTION, ihConstants._SCAN_IN_GRAYSCALE).Replace("\0", "").Trim();
                int dpi = Convert.ToInt32(config.GetValue(ihConstants._SCAN_SECTION, ihConstants._SCAN_DPI).Replace("\0", ""));
                scanDpi = (short)dpi;
                int greydpi = Convert.ToInt32(config.GetValue(ihConstants._SCAN_SECTION, ihConstants._SCAN_GREY_DPI).Replace("\0", ""));
                scangreyDpi = (short)greydpi;
                if (scanColorMode == "1")
                {
                    colorMode = false;
                }
                else
                {
                    colorMode = false;
                }
                //policyList = GetPolicyList();
                if (ShowPolicy() == true)
                {
                    j = 0;
                    wBatch = new wfeBatch(sqlCon);
                    batchPath = GetBundlePath(projKey, bundleKey).Tables[0].Rows[0][0].ToString();
                    CtrlImage pImage = new CtrlImage(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), "1", lblCurrentPolicy.Text, string.Empty, string.Empty);
                    wfeImage wImage = new wfeImage(sqlCon, pImage);
                    imageCount = wImage.GetMaxPageCount();
                    if (imageCount > 0)
                    {
                        hasImage = true;
                    }
                    i = imageCount;

                    if (rdADF.Checked == true)
                    {
                        //isOk = tw.Acquire(true, colorMode, scanDpi); //AcquireFixed(false, colorMode, 1, 0);
                        
                        DialogResult result;
                        result = MessageBox.Show("Do you want to scan in color mode?", "Scan", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                        if (result == DialogResult.Yes)
                        {
                            isOk = tw.Acquire(true); //duplex mode color scan
                        }
                        else
                        {
                            colorMode = false;
                            isOk = tw.Acquire(true, colorMode, scanDpi); //AcquireFixed(false, colorMode, 1, 0); // black and white duplex mode 
                        }

                    }
                    else
                    {
                        //isOk = tw.AcquireFixed(false, colorMode, 1, 0);
                        FlatBedScan = true;
                        ////isOk = tw.AcquireFixed(false, colorMode, 1, 0, scanDpi);
                        DialogResult result;
                        result = MessageBox.Show("Do you want to scan in color mode?", "Scan", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                        if (result == DialogResult.Yes)
                        {
                            colorMode = true;
                           // isOk = tw.AcquireFixed(false, colorMode, ihConstants.MAX_NO_SCAN_FQC, 0, scangreyDpi);
                            //////isOk = tw.Acquire(false, colorMode, scangreyDpi);
                            //isOk = tw.Acquire();

                            isOk = tw.Acquire(false, 0);   // color simplex
                           
                        }
                        else
                        {
                            colorMode = false;
                            isOk = tw.AcquireFixed(false, colorMode, ihConstants.MAX_NO_SCAN_FQC, 0, scanDpi);  // BW sipmlex
                        }

                        //isOk = tw.AcquireFixed(false, colorMode, ihConstants.MAX_NO_SCAN_FQC, 0,scanDpi);

                    }
                    if (isOk)
                    {
                        grpAction.Enabled = false;
                    }
                    else
                    {
                        MessageBox.Show("Error in starting scanner...");
                        int msg = tw.AcquireMsg;
                        if (acquiretimemessage == "1")
                        {
                            MessageBox.Show(msg.ToString());
                        }
                        EndingScan();
                        tw.CloseSrc();
                        
                        GC.Collect();
                        grpAction.Enabled = true;
                    }
                }
                else
                {
                    //MessageBox.Show("No more policies remain to be scanned....");
                    tw.CloseSrc();
                    
                    EndingScan();
                }
            }
            else
            {
                MessageBox.Show("Set the scanner settings from this software and from ScandAll....", "Settings", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void cmdCancelScan_Click(object sender, EventArgs e)
        {
            //tmpImg.LoadBitmapFromFile(@"I:\DATA\1902120018[106]140211\1\190212001[66666]\Scan\190212001[66666]_001_A.TIF");
            //scanPic.Image = null;
            //scanPic.Image = tmpImg.GetBitmap();
            //scanPic.Refresh();
            //panel2.Height = this.ClientSize.Height - 25;
            tw.GetCancelNote();
            tw.CloseSrc();
        }


        void AePolicyScanFormClosing(object sender, FormClosingEventArgs e)
        {
            if (tw != null)
            {
                tw.CloseSrc();
                tw.Finish();
                EndingScan();
                //tw.Destruct();
            }
        }

        private void lstImageName_SelectedIndexChanged(object sender, EventArgs e)
        {
            string imagename = null;
            string fileName = null;
            long fileSize;

            try
            {
                if (lstImageName.SelectedIndex >= 0)
                {
                    imagename = lstImageName.SelectedItem.ToString();
                    fileName = scanFolder + "\\" + imagename;
                    tmpImg.LoadBitmapFromFile(fileName);
                    scanPic.Image = tmpImg.GetBitmap();

                    System.IO.FileInfo info = new System.IO.FileInfo(fileName);

                    fileSize = info.Length;
                    fileSize = fileSize / 1024;
                    lblSize.Text = fileSize.ToString() + " KB";
                    if (fileSize > 50)
                    {
                        lblSize.ForeColor = Color.Red;
                    }
                    else
                    {
                        lblSize.ForeColor = Color.Black;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while showing the image.." + ex.Message);
                exMailLog.Log(ex);
            }
        }

        private void lstImageName_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void lstImageName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                string imagename = null;
                string fileName = null;

                try
                {
                    if (e.KeyCode == Keys.Delete)
                    {
                        imagename = lstImageName.SelectedItem.ToString();
                        fileName = scanFolder + "\\" + imagename;

                        pImage = new CtrlImage(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), "1", lblCurrentPolicy.Text, imagename, string.Empty);
                        wfeImage wImage = new wfeImage(sqlCon, pImage);
                        if (wImage.DeleteImage() == true)
                        {
                            //wImage.DeleteImage();
                            File.Delete(fileName);
                            lstImageName.Items.RemoveAt(lstImageName.SelectedIndex);
                            if (lstImageName.Items.Count > (lstImageName.SelectedIndex + 1))
                            {
                                lstImageName.SelectedIndex = lstImageName.SelectedIndex + 1;
                            }
                            lstImageName.Refresh();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error while showing the image.." + ex.Message);
                    exMailLog.Log(ex);
                }
            }
        }

        private void cmdDelete_Click(object sender, EventArgs e)
        {
            tw.CloseSrc();
            EndingScan();
            frmRescanPolicy fmRescan = new frmRescanPolicy(sqlCon, crd, scanMode);
            fmRescan.ShowDialog(this);
            ShowPolicy();
        }

        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
            Point pt = new Point();
            pt.X = e.X;
            pt.Y = e.Y;
            if (e.Button == MouseButtons.Right)
            {
                ihwQuery wQ = new ihwQuery(sqlCon);
                if (wQ.GetSysConfigValue(ihConstants.SCAN_TIME_HOLD_KEY) == ihConstants.SCAN_TIME_HOLD_VALUE)
                {
                    conHold.Show(panel1, pt);
                }
            }
        }

        private void conMarkHold_Click(object sender, EventArgs e)
        {
            string policyNo = lblCurrentPolicy.Text;
            try
            {
                DialogResult ds = MessageBox.Show("Are you sure, you want to hold this file?", "Hold Case File", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (ds == DialogResult.Yes)
                {
                    if (policyNo != string.Empty)
                    {
                        CtrlPolicy ctrlPolicy = new CtrlPolicy(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), "1", policyNo);
                        wfePolicy policy = new wfePolicy(sqlCon, ctrlPolicy);
                        if (policy.UpdateStatus(eSTATES.POLICY_ON_HOLD, crd))
                        {
                            pImage = new CtrlImage(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), "1", policyNo, string.Empty, string.Empty);
                            wfeImage wImage = new wfeImage(sqlCon, pImage);
                            wImage.TotalImageUpdateStatus(eSTATES.PAGE_ON_HOLD);
                            lstImageName.Items.Clear();
                            scanPic.Image = null;
                            if (policy.DeleteAllPage())
                            {
                                string policyFolder = batchPath + "\\" + policyNo;
                                //scanFolder = policyFolder + "\\" + ihConstants._SCAN_FOLDER;
                                if (Directory.Exists(policyFolder))
                                {
                                    Directory.Delete(policyFolder, true);
                                }
                            }
                        }
                    }
                    conHold.Hide();
                    ShowPolicy();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while updating Case File status......" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (rdADF.Checked == true)
            {
                FlatBedScan = false;
            }
            if (rdFlatBed.Checked == true)
            {
                FlatBedScan = true;
            }
            MessageBox.Show("Settings changed successfully, also change same settings from ScandAll....", "Settings", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void cmdUpdate_Click(object sender, EventArgs e)
        {
            string policyNo = lblCurrentPolicy.Text;
            CtrlPolicy ctrlPolicy = new CtrlPolicy(Convert.ToInt32(wBox.ctrlBox.ProjectCode), Convert.ToInt32(wBox.ctrlBox.BatchKey), wBox.ctrlBox.BoxNumber, policyNo);
            wfePolicy policy = new wfePolicy(sqlCon, ctrlPolicy);
            //if (policy.UpdatePageCount(Convert.ToInt32(lblPageCount.Text)))
            //{
            //    MessageBox.Show("Page count updated successfully..........");
            //}
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }
        public int GetFileCount(string projkey, string bundleKey)
        {
            string sqlStr = null;
            DataSet dsImage = new DataSet();
            OdbcDataAdapter sqlAdap = null;

            sqlStr = "select patient_id from metadata_entry " +
                    " where proj_code=" + projkey +
                " and bundle_key=" + bundleKey + " and status = '1'";


            try
            {
                sqlAdap = new OdbcDataAdapter(sqlStr, sqlCon);
                sqlAdap.Fill(dsImage);
            }
            catch (Exception ex)
            {
                sqlAdap.Dispose();
                
            }
            return dsImage.Tables[0].Rows.Count;
        }

        public int GetImageCount(string projkey, string bundleKey, string box, string policy)
        {
            string sqlStr = null;
            DataSet dsImage = new DataSet();
            OdbcDataAdapter sqlAdap = null;

            sqlStr = "select page_index_name from image_master " +
                    " where proj_key =" + projkey +
                " and batch_key=" + bundleKey + " and box_number = '"+box+"' and policy_number = '"+policy+"' and status = '24'";


            try
            {
                sqlAdap = new OdbcDataAdapter(sqlStr, sqlCon);
                sqlAdap.Fill(dsImage);
            }
            catch (Exception ex)
            {
                sqlAdap.Dispose();

            }
            return dsImage.Tables[0].Rows.Count;
        }

        public bool UpdateBundleStatus(int prmProjKey, int prmBatchKey)
        {
            string sqlStr = null;
            OdbcTransaction sqlTrans = null;
            bool commitBol = true;

            OdbcCommand sqlCmd = new OdbcCommand();

            sqlStr = @"update bundle_master" +
                " set status=" + 2 + " where " +
                " bundle_key=" + prmBatchKey + " and proj_code = '" + prmProjKey + "' and status = '1' and status<>" + 2;

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
                
            }
            return commitBol;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult dlg1 = MessageBox.Show(this, "Are you sure, this lot is ready for the next stage?", "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dlg1 == DialogResult.Yes)
            {
                pImage = new CtrlImage(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), "1", lblCurrentPolicy.Text.ToString(), string.Empty, string.Empty);
                        wfeImage wImage = new wfeImage(sqlCon, pImage);

                        NovaNet.wfe.eSTATES[] polState = new NovaNet.wfe.eSTATES[1];
                        CtrlPolicy ctrlPolicy = new CtrlPolicy(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), "1", lblCurrentPolicy.Text.ToString());
                        wfePolicy policy = new wfePolicy(sqlCon, ctrlPolicy);
                        if (GetImageCount(projKey, bundleKey, "1", lblCurrentPolicy.Text.ToString()) > 0)
                        {
                            policy.UpdateFileStatus(eSTATES.POLICY_SCANNED, crd);

                        }
                        else
                        {
                            MessageBox.Show(this,"No image is added for this case file...","",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                            return;
                        }
                        crd.created_dttm = dbcon.GetCurrenctDTTM(1, sqlCon);
                        ///insert into transaction log
                        policy.UpdateTransactionLog(eSTATES.POLICY_SCANNED, crd);

                        pBox = new CtrlBox(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), "1");
                        wBox = new wfeBox(sqlCon, pBox);
                        NovaNet.wfe.eSTATES[] state = new NovaNet.wfe.eSTATES[3];
                        state[0] = NovaNet.wfe.eSTATES.POLICY_CREATED;

                        if (policy.GetPolicyCount(state) == 0)
                        {
                            //wBox.UpdateStatus(eSTATES.BOX_SCANNED);
                        }

                        ///For checking the box status
                        NovaNet.wfe.eSTATES[] boxState = new NovaNet.wfe.eSTATES[3];
                        boxState[0] = NovaNet.wfe.eSTATES.BOX_CREATED;


                        wBatch = new wfeBatch(sqlCon);
                        //int cont = wBox.GetBoxCount(boxState);
                        if (GetFileCount(projKey,bundleKey) == 0)
                        {
                            ///Update the batch status
                            //wBatch.UpdateStatus(eSTATES.BATCH_SCANNED, wBox.ctrlBox.BatchKey);
                            UpdateBundleStatus(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey));
                        }
                    

                }

            if (policyList.Count-1 >0)
            {
                lstImageName.Items.Clear();
                scanPic.Image = null;
                ShowPolicy();
                cmdScan.Focus();
            }
            else
            {
                MessageBox.Show("No more case file remain to be scanned....");
                
                lstImageName.Items.Clear();
                scanPic.Image = null;
                return;
            }
                  
        }

        private void deButton1_Click(object sender, EventArgs e)
        {
            if (!tw.Select()) { return; }
            SaveInColor = true;
            if (!msgfilter)
            {
                this.Enabled = false;
                msgfilter = true;
                Application.AddMessageFilter(this);
            }
            //tw.Acquire();

            //MessageBox.Show("Done");

            //EndingScan();
            //tw.CloseSrc();
        }

        

    }  
}
