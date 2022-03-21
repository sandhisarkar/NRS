using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.ComponentModel;
using WeifenLuo.WinFormsUI.Docking;
using System.Runtime.InteropServices;
using DockSample.Customization;
using System.IO;
using DockSample;
using NovaNet.Utils;
using NovaNet.wfe;
using System.Data;
using System.Data.Odbc;
using System.Collections;
using LItems;
//using AForge.Imaging;
//using AForge;
//using AForge.Imaging.Filters;
using TwainLib;
using GdiPlusLib;
using System.Threading;

namespace ImageHeaven
{
    public partial class aeFQC : Form, IMessageFilter 
    {
        public delegate void NextClickedHandler(object sender, EventArgs e);
        public delegate void PreviousClickedHandler(object sender, EventArgs e);
        public delegate void PolicyChangeHandler(object sender, EventArgs e);
        public delegate void ImageChangeHandler(object sender, EventArgs e);
        public delegate void BoxDetailsLoaded(object sender, EventArgs e);
        public delegate void LstDelImageKeyPress(object sender, KeyEventArgs e);
        public delegate void LstImageIndexKeyPress(object sender, KeyPressEventArgs e);
        public delegate void BoxDetailsMouseClick(object sender, MouseEventArgs e);
        public delegate void LstImageClick(object sender, EventArgs e);
        public delegate void LstDelImageClick(object sender, EventArgs e);
        public delegate void LstNextKeyPress(object sender, KeyEventArgs e);
        public delegate void ComboValueChanged(object sender, EventArgs e);

        private int indexCount = 0;
        private bool indexingOn = false;
        //private DummyToolbox m_toolbox = new DummyToolbox();
        private OdbcConnection sqlCon = null;
        private bool m_bSaveLayout = true;
        private DeserializeDockContent m_deserializeDockContent;
        private FloatToolbox m_toolbox = new FloatToolbox();
        //private MagickNet.Image imgQc;
        private string imgFileName = null;
        //private ImageQC objQc=new ImageQC();
        NovaNet.Utils.ImageManupulation delImage;
        private wfeBox wBox = null;
        private CtrlPolicy pPolicy = null;
        private CtrlImage pImage = null;
        private static string docType;
        private CtrlBox pBox = null;
        private string indexFilePath = null;
        NovaNet.Utils.dbCon dbcon = new NovaNet.Utils.dbCon();
        long fileSize;
        Credentials crd = new Credentials();
        int firstImage = 0;
        int lastImage = 0;
        ///For key config
        string cropKey;
        string zoomInKey;
        string zoomOutKey;
        string autoCropKey;
        string rotateRKey;
        string rotateLKey;
        string skewRKey;
        string skewLKey;
        string noiseRemovalLKey;
        string cleanKey;
        string deleteKey;
        private const int THUMBNAIL_DATA = 0x501B;
        /// <summary>
        /// For drawing rectangle
        /// </summary>
        private int cropX;
        private int cropY;
        private int cropWidth;
        private int cropHeight;
        private double constRotateAngle;
        private double skewXAngle;
        private double skewYAngle;
        bool hasPhotoBol;
        private int OperationInProgress;
		string[] imageName;
        //private Bitmap cropBitmap;
        delegate void ch();
        private Pen cropPen;
        private int cropPenSize = 1;
        private System.Drawing.Color cropPenColor = System.Drawing.Color.Blue;

        private Label lblImageSize = null;
        private int zoomWidth;
        private int zoomHeight;
        private Size zoomSize = new Size();
        private int keyPressed = 1;
        private ImageConfig config = null;
        private string policyPath = string.Empty;
        private ListBox delImgList = null;
        private ComboBox cmbBox = null;
        //GD objects
        //private GdPicture.GdPictureImaging imgQc = new GdPicture.GdPictureImaging();
        //private int ino;
        MemoryStream stateLog;
        byte[] tmpWrite;
        private ListBox policyLst = null;
        private ListBox imageLst = null;
        private ListBox imageDelLst = null;
        private CtrlPolicy ctrlPolicy = null;
        private wfePolicy policy = null;
        private udtPolicy policyData = null;
        private FileorFolder fileMove = null;
        private string sourceFilePath = null;
        private string indexFolderName = null;
        private string qcFolderName = null;
        private bool getPhotoOperation = false;
        private string scanFolder = null;
        private string qcFolder = null;
        private string error;
        private bool insertFlag = false;
        private int lvwIndex;
        //Scanning
        private Twain twScan;
        private bool colorMode;
        private bool msgfilter;
        private int scanWhat = 0;
        private Label lblBatch;
        //private long Page2=0;
        
        private Imagery img;
        private Imagery imgAll;
        private Imagery imgBond;
        public static NovaNet.Utils.exLog.Logger exMailLog = new NovaNet.Utils.exLog.emailLogger("./errLog.log", NovaNet.Utils.exLog.LogLevel.Dev, Constants._MAIL_TO, Constants._MAIL_FROM, Constants._SMTP);
        public static NovaNet.Utils.exLog.Logger exTxtLog = new NovaNet.Utils.exLog.txtLogger("./errLog.log", NovaNet.Utils.exLog.LogLevel.Dev);
        private string selDocType = string.Empty;
        private int currntPg = 0;
        private bool firstDoc = true;
        private string prevDoc;
        private int policyLen = 0;
        //System.Windows.Forms.Button prmButtonCrop = new Button();
        //System.Windows.Forms.Button prmButtonAutoCrp = new Button();
        //System.Windows.Forms.Button prmButtonRotateRight = new Button();
        //System.Windows.Forms.Button prmButtonRotateLeft = new Button();
        //System.Windows.Forms.Button prmButtonZoomIn = new Button();
        //System.Windows.Forms.Button prmButtonZoomOut = new Button();
        //System.Windows.Forms.Button prmButtonSkewRight = new Button();
        System.Windows.Forms.Button prmButtonSkewLeft = new Button();
        //System.Windows.Forms.Button prmButtonNoiseRemove = new Button();
        //System.Windows.Forms.Button prmButtonCleanImg = new Button();
        //System.Windows.Forms.Button prmButtonCopyImage = new Button();
        //System.Windows.Forms.Button prmButtonDelImage = new Button();
        //System.Windows.Forms.Button prmButtonRescan = new Button();
        //System.Windows.Forms.Button prmButtonScan = new Button();
        System.Windows.Forms.Button prmButtonCopyTo = new Button();
        //System.Windows.Forms.Button prmButtonImportImage = new Button();
        System.Windows.Forms.Button prmButtonMoveTo = new Button();
        System.Windows.Forms.Button prmButtonCopyImageTo = new Button();
        System.Windows.Forms.Button prmButtonCopyProposalForm = new Button();
        System.Windows.Forms.Button prmButtonCopyProposalReviewSlip = new Button();
        System.Windows.Forms.Button prmNext = null;
        System.Windows.Forms.Button prmPrevious = null;

        bool pageDelInsrt;
        private bool photoCropOperation = false;
        public static string projKey = null;
        public static string bundleKey = null;
        public static string boxNumber = null;
        OdbcDataAdapter sqlAdap;

        public static DataLayerDefs.Mode _mode = DataLayerDefs.Mode._Edit;

        public static DataLayerDefs.Mode mode;

        private bool delinsrtBol = false;

        public aeFQC()
        {
           InitializeComponent();

			m_deserializeDockContent = new DeserializeDockContent(GetContentFromPersistString);
			this.Text="Final QC";
            exMailLog.SetNextLogger(exTxtLog);
        }

        public aeFQC(OdbcConnection prmCon, Credentials prmCrd)
        {
            //
            // The InitializeComponent() call is required for Windows Forms designer support.
            //
            System.Diagnostics.Process.GetCurrentProcess().PriorityClass = System.Diagnostics.ProcessPriorityClass.RealTime;
            sqlCon = prmCon;
            //wBox = prmBox;
            crd = prmCrd;
            InitializeComponent();

            img = IgrFactory.GetImagery(Constants.IGR_CLEARIMAGE);
            imgAll = IgrFactory.GetImagery(Constants.IGR_CLEARIMAGE);
            imgBond = IgrFactory.GetImagery(Constants.IGR_CLEARIMAGE);
            //img = IgrFactory.GetImagery(Constants.IGR_GDPICTURE);
            //imgAll = IgrFactory.GetImagery(Constants.IGR_GDPICTURE);
            m_deserializeDockContent = new DeserializeDockContent(GetContentFromPersistString);
            this.Text = "Final QC";
            exMailLog.SetNextLogger(exTxtLog);
            //
            // TODO: Add constructor code after the InitializeComponent() call.
            //

            _mode = mode;
        }

        private IDockContent GetContentFromPersistString(string persistString)
		{
				return m_toolbox;
		}

        //MemoryStream StateData.StateLog()
        //{
        //    return stateLog;
        //}

        /// <summary>
        /// It's a message filter interface
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        bool IMessageFilter.PreFilterMessage(ref Message m)
        {
            TwainCommand cmd = twScan.PassMessage(ref m);
            if (cmd == TwainCommand.Not)
            {
                this.Refresh();
                return false;
            }
            switch (cmd)
            {
                case TwainCommand.CloseRequest:
                    {
                        EndingScan();
                        twScan.CloseSrc();
                        break;
                    }
                case TwainCommand.CloseOk:
                    {
                        EndingScan();
                        twScan.CloseSrc();
                        break;
                    }
                case TwainCommand.DeviceEvent:
                    {
                        break;
                    }
                case TwainCommand.TransferReady:
                    {
                        Twain.ImageNotification delMan;
                        int pics;
                        if (scanWhat == ihConstants.SCAN_RE_FQC)
                        {
                            delMan = new Twain.ImageNotification(GetImage);
                            pics = twScan.TransferPicturesFixed(GetImage, this);
                        }
                        if (scanWhat == ihConstants.SCAN_NEW_FQC)
                        {
                            delMan = new Twain.ImageNotification(GetImageNew);
                            pics = twScan.TransferPicturesFixed(GetImageNew, this);
                        }
                        twScan.CloseSrc();
                        EndingScan();
                        break;
                    }
            }

            return true;
        }
        private void ChangeSize()
        {
            try
            {
                if (img.IsValid() == true)
                {
                    //panel1.Width = (panel2.Left - BoxDtls.Right);
                    //panel1.Height = this.ClientSize.Height - 20;
                    if (!System.IO.File.Exists(imgFileName)) return;
                    Image newImage = img.GetBitmap();
                    if (newImage.PixelFormat == PixelFormat.Format1bppIndexed)
                    {
                        pictureControl.Image = null;
                        pictureControl.Width = panel1.Width - 10;
                        pictureControl.Height = tabControl2.Height - 30;
                        double scaleX = (double)pictureControl.Width / (double)newImage.Width;
                        double scaleY = (double)pictureControl.Height / (double)newImage.Height;
                        double Scale = Math.Min(scaleX, scaleY);
                        int w = (int)(newImage.Width * Scale);
                        int h = (int)(newImage.Height * Scale);
                        pictureControl.Width = w;
                        pictureControl.Height = h;
                        //pictureControl.Image = CreateThumbnail(imgFileName, w, h); //newImage.GetThumbnailImage(w, h, new System.Drawing.Image.GetThumbnailImageAbort(GetThumbnailImageAbort), IntPtr.Zero);
                        pictureControl.Image = CreateThumbnail(newImage, w, h); //newImage.GetThumbnailImage(w, h, new System.Drawing.Image.GetThumbnailImageAbort(GetThumbnailImageAbort), IntPtr.Zero);
                        newImage.Dispose();
                        //////////////
                        //pictureControl.Refresh();
                        if (Convert.ToDouble(fileSize) < 60)
                        {
                            lblImgSize.ForeColor = Color.Black;
                        }
                        else
                        {
                            lblImgSize.ForeColor = Color.Red;
                        }
                    }
                    else
                    {
                        //pictureControl.Width =panel1.Width - 2;
                        //pictureControl.Height =panel1.Height-2;
                        pictureControl.Width = panel1.Width - 50;
                        pictureControl.Height = tabControl2.Height - 30;
                        img.LoadBitmapFromFile(imgFileName);
                        pictureControl.Image = img.GetBitmap();
                        pictureControl.SizeMode = PictureBoxSizeMode.StretchImage;
                        pictureControl.Refresh();
                        if (Convert.ToDouble(fileSize) < 20)
                        {
                            lblImgSize.ForeColor = Color.Black;
                        }
                        else
                        {
                            lblImgSize.ForeColor = Color.Red;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
                MessageBox.Show("Error while cropping the image" + ex.Message, "Crop error");
            }
        }
        public void GetImageNew(System.IntPtr prmHBmp)
        {
            char leftPad = Convert.ToChar("0");

            string sourcePath = null;
            string desPath = null;
            string qcPath = null;
            string scanPath = null;
            int imgCount = 0;
            int pageCount = 0;
            string flName;
            string scanDate;
            bool success = false;
            try
            {
                policyLst = lstPolicy;
                pImage = new CtrlImage(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), boxNumber, policyLst.SelectedItem.ToString(), string.Empty, string.Empty);
                wfeImage wImage = new wfeImage(sqlCon, pImage);

                imgCount = wImage.GetImageCount();
                pageCount = wImage.GetMaxPageCount();

                if (imgCount >= 0)
                {
                    imgCount = imgCount + 1;
                    pageCount = pageCount + 1;
                    flName = policyLst.SelectedItem.ToString() + "_" + pageCount.ToString().PadLeft(3, '0') + "_A.TIF";
                    //Build string for supposed to be file in indexing folder
                    //sourcePath = sourceFilePath + "\\" + flName;
                    //Build string for file in FQC folder
                    desPath = indexFolderName + "\\" + flName;
                    //Build string for file in QC folder
                    scanPath = scanFolder + "\\" + flName;
                    //Build string for file in scan folder
                    //qcPath = qcFolder + "\\" + flName;
                    //ino = gdScanImg.CreateGdPictureImageFromDIB(prmHBmp);
                    pPolicy = new CtrlPolicy(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), boxNumber, policyLst.SelectedItem.ToString());
                    wfePolicy wPolicy = new wfePolicy(sqlCon, pPolicy);
                    scanDate = dbcon.GetCurrenctDTTM(1, sqlCon);
                    wPolicy.UpdateScanDetails(scanDate, ihConstants.SCAN_SUCCESS_FLAG);

                    img.LoadBitmapFromDIB(prmHBmp);
                    if (Directory.Exists(indexFolderName))
                    {
                        if (img.SaveFile(desPath) == IGRStatus.Success)
                        {
                            if (Directory.Exists(scanFolder))
                            {
                                if (img.SaveFile(scanPath) == IGRStatus.Success)
                                {
                                    success = true;
                                }
                            }
                        }
                    }
                    //if (Directory.Exists(sourceFilePath))
                    //{
                    //    img.SaveFile(sourcePath);
                    //}
                    //if (Directory.Exists(qcFolder))
                    //{
                    //    img.SaveFile(qcPath);
                    //}
                    if (success == true)
                    {
                        //pictureControl.Image = img.GetBitmap();
                        ChangeSize();
                        SetListboxValue(flName, imgCount);
                        if (prmHBmp != IntPtr.Zero)
                        {
                            Marshal.FreeHGlobal(prmHBmp);
                            prmHBmp = IntPtr.Zero;
                        }
                    }
                    else
                    {
                        twScan.CloseSrc();
                        MessageBox.Show("Error while saving....");
                        if (prmHBmp != IntPtr.Zero)
                        {
                            Marshal.FreeHGlobal(prmHBmp);
                            prmHBmp = IntPtr.Zero;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                twScan.CloseSrc();
                MessageBox.Show("Error - " + ex.Message);
                //exMailLog.Log(ex);
                if (prmHBmp != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(prmHBmp);
                    prmHBmp = IntPtr.Zero;
                }
            }
        }
        void SetListboxValue(string prmIamgeName, int prmSrlNo)
        {
            CtrlImage ctrlImg;

            long fileSize;
            System.IO.FileInfo info = new System.IO.FileInfo(indexFolderName + "\\" + prmIamgeName);

            fileSize = info.Length;
            fileSize = fileSize / 1024;

            wfeImage img;
            imageLst = lstImage;
            policyLst = lstPolicy;
            imageLst.Items.Add(prmIamgeName);
            ctrlImg = new CtrlImage(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), boxNumber, policyLst.SelectedItem.ToString(), prmIamgeName, string.Empty);

            img = new wfeImage(sqlCon, ctrlImg);
            img.Save(crd, eSTATES.PAGE_RESCANNED_NOT_INDEXED, fileSize, ihConstants._NORMAL_PAGE, prmSrlNo, prmIamgeName);

            ctrlPolicy = new CtrlPolicy(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), boxNumber, policyLst.SelectedItem.ToString());
            policy = new wfePolicy(sqlCon, ctrlPolicy);
            UpdateStatus(eSTATES.POLICY_NOT_INDEXED, crd);
        }  
        public void GetImage(System.IntPtr prmHBmp)
        {
            char leftPad = Convert.ToChar("0");
            string sourcePath = null;
            string desPath = null;
            int pos = 0;
            string fileName;
            try
            {
                imageLst = lstImage;
                if (imageLst.SelectedItem.ToString() != string.Empty)
                {
                    //Build string for supposed to be file in indexing folder
                    sourcePath = sourceFilePath + "\\" + imageLst.SelectedItem.ToString();
                    //Build string for file in FQC folder
                    desPath = indexFolderName + "\\" + imageLst.SelectedItem.ToString();
                }
                //Wild card search: To save in the indexing folder
                fileName = imageLst.SelectedItem.ToString();
                pos = fileName.IndexOf("-");
                if (pos > 0)
                {
                    string originalImage = fileName.Substring(0, pos - 4) + "*" + ".TIF";
                    //string[] searchFileName = Directory.GetFiles(sourceFilePath, originalImage);
                    ////For the file in index folder
                    //if (searchFileName.Length >= 0)
                    //    sourcePath = searchFileName[0];
                    //For the file in FQC folder
                    string[] searchFileName = Directory.GetFiles(indexFolderName, originalImage);
                    if (searchFileName.Length >= 0)
                        desPath = searchFileName[0];
                }
                //End: Wild card search
                img.LoadBitmapFromDIB(prmHBmp);
                img.SaveFile(desPath);
                //img.SaveFile(sourcePath);
                ChangeSize(desPath);
                if (prmHBmp != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(prmHBmp);
                    prmHBmp = IntPtr.Zero;
                }
            }
            catch (Exception ex)
            {
                twScan.CloseSrc();
                MessageBox.Show("Error - " + ex.Message);
                exMailLog.Log(ex);
            }
        }
        private bool GetThumbnailImageAbort()
        {
            return false;
        }

        private void ChangeSize(string fName)
        {
            Image imgTot = null;
            try
            {
                if (img.IsValid() == true)
                {
                    pictureControl.Width = panel1.Width - 5;
                    pictureControl.Height = panel1.Height - 5;
                    if (!System.IO.File.Exists(fName)) return;
                    Image newImage;
                    imgAll.LoadBitmapFromFile(fName);
                    if (imgAll.GetBitmap().PixelFormat == PixelFormat.Format24bppRgb)
                    {
                        imgAll.GetLZW("tmp1.TIF");
                        imgTot = Image.FromFile("tmp1.TIF");
                        newImage = imgTot;
                        //File.Delete("tmp1.TIF");
                    }
                    else
                    {
                        newImage = System.Drawing.Image.FromFile(fName);
                    }
                    double scaleX = (double)pictureControl.Width / (double)newImage.Width;
                    double scaleY = (double)pictureControl.Height / (double)newImage.Height;
                    double Scale = Math.Min(scaleX, scaleY);
                    int w = (int)(newImage.Width * Scale);
                    int h = (int)(newImage.Height * Scale);
                    pictureControl.Width = w - 5;
                    pictureControl.Height = h - 5;
                    pictureControl.Image = CreateThumbnail(newImage, w, h); //newImage.GetThumbnailImage(w, h, new System.Drawing.Image.GetThumbnailImageAbort(GetThumbnailImageAbort), IntPtr.Zero);
                    newImage.Dispose();
                    pictureControl.Refresh();
                    if (imgTot != null)
                    {
                        imgTot.Dispose();
                        imgTot = null;
                        if (File.Exists("tmp1.tif"))
                            File.Delete("tmp1.TIF");
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
                error = ex.Message;
                MessageBox.Show("Error while cropping the image" + ex.Message, "Crop error");
            }
        }
        public Image CreateThumbnail(Image pImage, int lnWidth, int lnHeight)
        {

            Bitmap bmp = new Bitmap(lnWidth, lnHeight);
            try
            {

                DateTime stdt = DateTime.Now;

                //create a new Bitmap the size of the new image

                //create a new graphic from the Bitmap
                Graphics graphic = Graphics.FromImage((Image)bmp);
                graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
                //draw the newly resized image
                graphic.DrawImage(pImage, 0, 0, lnWidth, lnHeight);
                //dispose and free up the resources
                graphic.Dispose();
                DateTime dt = DateTime.Now;
                TimeSpan tp = dt - stdt;
                //MessageBox.Show(tp.Milliseconds.ToString());
                //return the image

            }
            catch
            {
                return null;
            }
            return (Image)bmp;
        }
        void Button1Click(object sender, EventArgs e)
        {
           // m_toolbox.Show(dockPanel);
        }
        private void EndingScan()
        {
            if (msgfilter)
            {
                Application.RemoveMessageFilter(this);
                msgfilter = false;
                this.Enabled = true;
                this.Activate();
            }
        }
        void ReadConfigKey()
        {
            config = new ImageConfig(ihConstants.CONFIG_FILE_PATH);
            cropKey = config.GetValue(ihConstants.IMAGE_SHORTCUT_KEY_SECTION, ihConstants.CROP_KEY).Remove(1, 1).Trim();
            zoomInKey = config.GetValue(ihConstants.IMAGE_SHORTCUT_KEY_SECTION, ihConstants.ZOOM_IN_KEY).Remove(1, 1).Trim();
            zoomOutKey = config.GetValue(ihConstants.IMAGE_SHORTCUT_KEY_SECTION, ihConstants.ZOOM_OUT_KEY).Remove(1, 1).Trim();
            autoCropKey = config.GetValue(ihConstants.IMAGE_SHORTCUT_KEY_SECTION, ihConstants.AUTO_CROP_KEY).Remove(1, 1).Trim();
            rotateRKey = config.GetValue(ihConstants.IMAGE_SHORTCUT_KEY_SECTION, ihConstants.ROTATE_RIGHT_KEY).Remove(1, 1).Trim();
            rotateLKey = config.GetValue(ihConstants.IMAGE_SHORTCUT_KEY_SECTION, ihConstants.ROTATE_LEFT_KEY).Remove(1, 1).Trim();
            skewRKey = config.GetValue(ihConstants.IMAGE_SHORTCUT_KEY_SECTION, ihConstants.SKEW_RIGHT_KEY).Remove(1, 1).Trim();
            skewLKey = config.GetValue(ihConstants.IMAGE_SHORTCUT_KEY_SECTION, ihConstants.SKEW_LEFT_KEY).Remove(1, 1).Trim();
            noiseRemovalLKey = config.GetValue(ihConstants.IMAGE_SHORTCUT_KEY_SECTION, ihConstants.NOISE_REMOVE_KEY).Remove(1, 1).Trim();
            cleanKey = config.GetValue(ihConstants.IMAGE_SHORTCUT_KEY_SECTION, ihConstants.CLEAN_KEY).Remove(1, 1).Trim();
            deleteKey = config.GetValue(ihConstants.IMAGE_SHORTCUT_KEY_SECTION, ihConstants.DELETE_KEY).Remove(1, 1).Trim();
        }
        void DisplayValues()
        {
            projKey = frmMain.projKey;
            bundleKey = frmMain.bundleKey;
            boxNumber = "1";
            lblProject.Text = "Project: " + GetProjectName(Convert.ToInt32(projKey));
            lblBatch.Text = "Bundle: " + GetBundleName(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey));
            lblBox.Text = "Box: " + boxNumber;
            lblProject.ForeColor = Color.RoyalBlue;
            lblBatch.ForeColor = Color.RoyalBlue;
            lblBox.ForeColor = Color.RoyalBlue;
            lblCount.ForeColor = Color.RoyalBlue;

        }
        public string GetProjectName(int prmProjectKey)
        {
            string sqlStr = null;
            string projName = null;

            DataSet projDs = new DataSet();

            try
            {
                sqlStr = "select proj_key,proj_code from project_master where proj_key=" + prmProjectKey;
                sqlAdap = new OdbcDataAdapter(sqlStr, sqlCon);
                sqlAdap.Fill(projDs);
            }
            catch (Exception ex)
            {
                sqlAdap.Dispose();
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes(sqlStr + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex);
            }
            if (projDs.Tables[0].Rows.Count > 0)
            {
                projName = projDs.Tables[0].Rows[0]["proj_code"].ToString();
            }
            else
                projName = string.Empty;
            return projName;
        }
        public string GetBundleName(int prmProjectKey, int prmBundleKey)
        {
            string sqlStr = null;
            string projName = null;

            DataSet projDs = new DataSet();

            try
            {
                sqlStr = "select bundle_code,bundle_name from bundle_master where proj_code= '" + prmProjectKey + "' and bundle_key = '" + prmBundleKey + "' ";
                sqlAdap = new OdbcDataAdapter(sqlStr, sqlCon);
                sqlAdap.Fill(projDs);
            }
            catch (Exception ex)
            {
                sqlAdap.Dispose();
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes(sqlStr + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex);
            }
            if (projDs.Tables[0].Rows.Count > 0)
            {
                projName = projDs.Tables[0].Rows[0]["bundle_code"].ToString();
            }
            else
                projName = string.Empty;
            return projName;
        }
        private void aeFQC_Load(object sender, EventArgs e)
        {
            System.Diagnostics.Process.GetCurrentProcess().PriorityClass = System.Diagnostics.ProcessPriorityClass.RealTime;
            //wfePolicy wPolicy = new wfePolicy(sqlCon);
            //int count = wPolicy.GetTransactionLogCount(wBox.ctrlBox.BatchKey.ToString(), dbcon.GetCurrenctDTTM(2, sqlCon), crd.created_by, eSTATES.POLICY_FQC);
            this.Text = this.Text;// +"                                       Today you have done " + count + " ";

            System.Windows.Forms.ToolTip bttnToolTip = new System.Windows.Forms.ToolTip();
            ReadConfigKey();
            DisplayValues();
            cmbDesValue.SelectedIndex = 0;
            ArrayList arrPolicy = new ArrayList();

            twScan = new Twain();
            twScan.Init(this.Handle);
            TwainLib.Twain.TwainEventNotification delTEvn = new TwainLib.Twain.TwainEventNotification(GetNotification);
            twScan.SetNotification(delTEvn);

            wQuery pQuery = new ihwQuery(sqlCon);

            delImage = new NovaNet.Utils.ImageManupulation(CropRegister);
            prmButtonCrop.Text = "Crop";
            bttnToolTip.SetToolTip(prmButtonCrop, "Shortcut Key-" + cropKey);
            //m_toolbox.AddButton(prmButtonCrop, delImage);

            delImage = new NovaNet.Utils.ImageManupulation(AutoCrop);
            prmButtonAutoCrp.Text = "Auto-Crop";
            bttnToolTip.SetToolTip(prmButtonAutoCrp, "Shortcut Key-" + autoCropKey);
            //m_toolbox.AddButton(prmButtonAutoCrp, delImage);

            config = new ImageConfig(ihConstants.CONFIG_FILE_PATH);
            constRotateAngle = Convert.ToDouble(config.GetValue(ihConstants.IMAGE_RELATED_VALUE_SECTION, ihConstants.ROTATE_ANGLE_KEY).Replace("\0", ""));
            delImage = new NovaNet.Utils.ImageManupulation(RotateRight);
            bttnToolTip.SetToolTip(prmButtonRotateRight, "Shortcut Key-" + rotateRKey);
            prmButtonRotateRight.Text = "Rotate Right";
            //m_toolbox.AddButton(prmButtonRotateRight, delImage);

            //delImage = ZoomOut;
            delImage = new NovaNet.Utils.ImageManupulation(RotateLeft);
            prmButtonRotateLeft.Text = "Rotate Left";
            bttnToolTip.SetToolTip(prmButtonRotateLeft, "Shortcut Key-" + rotateLKey);
            //m_toolbox.AddButton(prmButtonRotateLeft, delImage);

            delImage = new NovaNet.Utils.ImageManupulation(ZoomIn);
            prmButtonZoomIn.Text = "Zoom In";
            bttnToolTip.SetToolTip(prmButtonZoomIn, "Shortcut Key-" + zoomInKey);
            //m_toolbox.AddButton(prmButtonZoomIn, delImage);

            //delImage = ZoomOut;
            delImage = new NovaNet.Utils.ImageManupulation(ZoomOut);
            prmButtonZoomOut.Text = "Zoom Out";
            bttnToolTip.SetToolTip(prmButtonZoomOut, "Shortcut Key-" + zoomOutKey);
            //m_toolbox.AddButton(prmButtonZoomOut, delImage);

            config = new ImageConfig(ihConstants.CONFIG_FILE_PATH);
            //skewXAngle =Convert.ToDouble(config.GetValue(ihConstants.IMAGE_RELATED_VALUE_SECTION, ihConstants.SKEW_X_KEY).Replace("\0", ""));
            //skewYAngle=Convert.ToDouble(config.GetValue(ihConstants.IMAGE_RELATED_VALUE_SECTION, ihConstants.SKEW_Y_KEY).Replace("\0", ""));
            delImage = new NovaNet.Utils.ImageManupulation(SkewRight);
            prmButtonSkewRight.Text = "DeSkew";
            bttnToolTip.SetToolTip(prmButtonSkewRight, "Shortcut Key-" + skewRKey);
            //m_toolbox.AddButton(prmButtonSkewRight, delImage);

            delImage = new NovaNet.Utils.ImageManupulation(NoiseRemove);
            prmButtonNoiseRemove.Text = "Despacle";
            bttnToolTip.SetToolTip(prmButtonNoiseRemove, "Shortcut Key-" + noiseRemovalLKey);
            prmButtonNoiseRemove.AutoSize = true;
            //m_toolbox.AddButton(prmButtonNoiseRemove, delImage);

            delImage = new NovaNet.Utils.ImageManupulation(CleanImageRegister);
            prmButtonCleanImg.Text = "Clean";
            bttnToolTip.SetToolTip(prmButtonCleanImg, "Shortcut Key-" + cleanKey);
            prmButtonCleanImg.AutoSize = true;
            //m_toolbox.AddButton(prmButtonCleanImg, delImage);

            delImage = new NovaNet.Utils.ImageManupulation(ImageCopy);
            bttnToolTip.SetToolTip(prmButtonCopyImage, "Shortcut Key-(Control+z)");
            prmButtonCopyImage.Text = "Copy Original";
            prmButtonCopyImage.AutoSize = true;
            //m_toolbox.AddButton(prmButtonCopyImage, delImage);

            delImage = new NovaNet.Utils.ImageManupulation(ImageDelete);
            bttnToolTip.SetToolTip(prmButtonDelImage, "Shortcut Key-" + deleteKey);
            prmButtonDelImage.Text = "Delete";
            prmButtonDelImage.AutoSize = true;
            //m_toolbox.AddButton(prmButtonDelImage, delImage);

            delImage = new NovaNet.Utils.ImageManupulation(RescanImage);
            prmButtonRescan.Text = "Rescan";
            prmButtonRescan.AutoSize = true;
            //m_toolbox.AddButton(prmButtonRescan, delImage);

            delImage = new NovaNet.Utils.ImageManupulation(ScanImage);
            prmButtonScan.Text = "Scan";
            prmButtonScan.AutoSize = true;
            //m_toolbox.AddButton(prmButtonScan, delImage);

            delImage = new NovaNet.Utils.ImageManupulation(ImportImage);
            prmButtonCopyTo.Text = "Import Bond";
            prmButtonCopyTo.AutoSize = true;
            //prmButtonCopyTo.Enabled = false;
            //m_toolbox.AddButton(prmButtonCopyTo, delImage);
            //prmButtonCopyTo.Visible = false;

            delImage = new NovaNet.Utils.ImageManupulation(ImportImageFromDir);
            prmButtonImportImage.Text = "Import Image";
            prmButtonImportImage.AutoSize = true;
            //m_toolbox.AddButton(prmButtonImportImage, delImage);
            prmButtonImportImage.Visible = true;

            //delImage = new NovaNet.Utils.ImageManupulation(MoveImage);
            prmButtonMoveTo.Text = "Move To";
            prmButtonMoveTo.AutoSize = true;
            //m_toolbox.AddButton(prmButtonMoveTo, delImage);
            //prmButtonMoveTo.Visible = false;
            ShowPolicyDetails();
            this.WindowState = FormWindowState.Maximized;
            policyLst = lstPolicy;
            if (lstPolicy.Items.Count > 0)
            {
                lstPolicy.SelectedIndex = 0;
            }

            System.Windows.Forms.ToolTip toolTip = new System.Windows.Forms.ToolTip();
            toolTip.SetToolTip(lstImageDel, "Press (Insert) key to insert this deleted image");

            imageLst = lstImage;
            imageDelLst = lstImageDel;
            policyLst = lstPolicy;
            Label delLabel = label3;
            lblSearch.Visible = true;
            txtSearch.Visible = true;

            PopulateListView();
            imageLst.Enabled = true;
            policyLst.Enabled = true;

            ShowAllException();
            this.WindowState = FormWindowState.Maximized;
            PopulateDelList(policyLst.SelectedItem.ToString());
            ///changed in version 1.0.0.1
            DisplayDockTypes();
            DisplayDocTypeCount();

            deLabel2.Text = "Total Scanned Image : " + lstImage.Items.Count.ToString();
        }
        private void GetNotification(string pNotification)
        {
            groupBox3.Enabled = true;
        }
        void ShowAllException()
        {
            policyLst = lstPolicy;
            DataSet expDs = new DataSet();
            pPolicy = new CtrlPolicy(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), boxNumber, policyLst.SelectedItem.ToString());
            wfePolicy policy = new wfePolicy(sqlCon, pPolicy);

            txtExceptionType.Text = string.Empty;
            expDs = policy.GetAllException();
            if (expDs.Tables[0].Rows.Count > 0)
            {

                if (Convert.ToInt32(expDs.Tables[0].Rows[0]["missing_img_exp"].ToString()) == 1)
                {
                    txtExceptionType.Text = "Missing image";
                }
                if (Convert.ToInt32(expDs.Tables[0].Rows[0]["crop_clean_exp"].ToString()) == 1)
                {
                    txtExceptionType.Text = txtExceptionType.Text + "\r\n" + "Crop clean exception";
                }

                if (Convert.ToInt32(expDs.Tables[0].Rows[0]["poor_scan_exp"].ToString()) == 1)
                {
                    txtExceptionType.Text = txtExceptionType.Text + "\r\n" + "Poor quality of scan";
                }

                if (Convert.ToInt32(expDs.Tables[0].Rows[0]["wrong_indexing_exp"].ToString()) == 1)
                {
                    txtExceptionType.Text = txtExceptionType.Text + "\r\n" + "Wrong indexing";
                }

                if (Convert.ToInt32(expDs.Tables[0].Rows[0]["linked_policy_exp"].ToString()) == 1)
                {
                    txtExceptionType.Text = txtExceptionType.Text + "\r\n" + "Linked policy exception";
                }

                if (Convert.ToInt32(expDs.Tables[0].Rows[0]["decision_misd_exp"].ToString()) == 1)
                {
                    txtExceptionType.Text = txtExceptionType.Text + "\r\n" + "Decision misd Exception";
                }

                if (Convert.ToInt32(expDs.Tables[0].Rows[0]["extra_page_exp"].ToString()) == 1)
                {
                    txtExceptionType.Text = txtExceptionType.Text + "\r\n" + "Extra page exception";
                }

                if (Convert.ToInt32(expDs.Tables[0].Rows[0]["rearrange_exp"].ToString()) == 1)
                {
                    txtExceptionType.Text = txtExceptionType.Text + "\r\n" + "Rearrange exception";
                }

                if (Convert.ToInt32(expDs.Tables[0].Rows[0]["other_exp"].ToString()) == 1)
                {
                    txtExceptionType.Text = txtExceptionType.Text + "\r\n" + "Other exception";
                }

                if (Convert.ToInt32(expDs.Tables[0].Rows[0]["move_to_respective_policy_exp"].ToString()) == 1)
                {
                    txtExceptionType.Text = txtExceptionType.Text + "\r\n" + "Move to respective policy exception";
                }

                txtComments.Text = expDs.Tables[0].Rows[0]["comments"].ToString();
                //MessageBox.Show ("This policy has some exception, details are given at right side","Policy Exception",MessageBoxButtons.OK);
            }
            else
            {
                txtExceptionType.Text = string.Empty;
                txtComments.Text = string.Empty;
            }
        }
        private void PopulateListView()
        {
            DataSet ds = new DataSet();
            string imageName = string.Empty;
            policyLst = lstPolicy;
            imageLst = lstImage;

            /* Changed by Rahul: 29 May, 09
             * To avoid Null Reference in imageLst.SelectedItem during swapping items within the list
             * Occurs when user requests rearrangement of items
             * */
            //int pos = imageLst.SelectedItem.ToString().IndexOf("-");
            int pos = imageLst.Text.ToString().IndexOf("-");
            if (pos > 0)
            {
                //imageName=imageLst.SelectedItem.Substring(0,pos);
                imageName = imageLst.Text.Substring(0, pos);
            }
            else
            {
                //imageName = imageLst.SelectedItem.ToString();
                imageName = imageLst.Text.ToString();
            }
            pImage = new CtrlImage(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), boxNumber, policyLst.SelectedItem.ToString(), imageName, string.Empty);
            wfeImage wImage = new wfeImage(sqlCon, pImage);

            ds = wImage.GetCustomException(ihConstants._NOT_RESOLVED);
            if (ds.Tables[0].Rows.Count > 0)
            {
                string custExcp = string.Empty;
                listView1.Items.Clear();
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    ListViewItem lvwItem = listView1.Items.Add(ds.Tables[0].Rows[i]["problem_type"].ToString());
                    //listView1.Items.Add(ds.Tables[0].Rows[i]["problem_type"].ToString());
                    lvwItem.SubItems.Add(ds.Tables[0].Rows[i]["Remarks"].ToString());
                }
                //txtRemarks.Text = ds.Tables[0].Rows[0]["Remarks"].ToString();
            }
            else
            {
                listView1.Items.Clear();
                //txtRemarks.Text = string.Empty;
            }
        }

        private string GetPolicyPath(string policyNo)
        {
            policyLst = lstPolicy;
            wfeBatch wBatch = new wfeBatch(sqlCon);
            string batchPath = GetPath(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey));
            return batchPath + "\\" + policyNo;
        }
        private void ShowPolicyDetails()
        {
            lstPolicy.Items.Clear();
            ArrayList arrPolicy = new ArrayList();
            wQuery pQuery = new ihwQuery(sqlCon, crd);
            CtrlPolicy ctrPol;

            arrPolicy = GetItems(eITEMS.POLICY);
            for (int i = 0; i < arrPolicy.Count; i++)
            {
                ctrPol = (CtrlPolicy)arrPolicy[i];
                lstPolicy.Items.Add(ctrPol.PolicyNumber);
            }
            if (arrPolicy.Count > 0)
            {
                lblCount.Text = "Total File: " + arrPolicy.Count;
            }
            else
            {
                lblCount.Text = "Total File: 0";
                lstImageDel.Items.Clear();
            }
        }
        public ArrayList GetItems(eITEMS item)
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
                strQuery = "select proj_code,bundle_key,patient_id from metadata_entry where proj_code= '" + projKey + "' and bundle_key='" + bundleKey + "' and (status ='3' or status ='4' or status ='5' or status ='6' or status ='7' or status ='8' or status ='30' or status ='31' or status = '37' or status ='40' or status = '77')";

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
        private int ImportImageFromDir()
        {
            string imageName = null;
            wfePolicy wPolicy = null;
            DataSet ds = new DataSet();
            NovaNet.wfe.eSTATES[] state = new NovaNet.wfe.eSTATES[4];
            wfeImage wImage = null;
            string newImageName = string.Empty;
            string origImageName = string.Empty;
            try
            {
                imageLst = lstImage;
                policyLst = lstPolicy;
                imageName = imageLst.SelectedItem.ToString();
                ///get the file name to copy
                fileDlg.Filter = "TIF File|*.TIF";
                fileDlg.FileName = string.Empty;
                fileDlg.Title = "CHC - TIF Files";
                fileDlg.ShowDialog();
                imageName = fileDlg.FileName.ToString();
                //status = Convert.ToInt32(cmbPolicy.SelectedValue.ToString());
                if ((imageName != null) && ((imageName != string.Empty)))
                {

                    //copy to
                    pPolicy = new CtrlPolicy(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), boxNumber, policyLst.SelectedItem.ToString());
                    wPolicy = new wfePolicy(sqlCon, pPolicy);
                    string newPolicyPath = GetPolicyPath(policyLst.SelectedItem.ToString()); //wPolicy.GetPolicyPath();

                    pImage = new CtrlImage(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), boxNumber, policyLst.SelectedItem.ToString(), string.Empty, string.Empty);
                    wImage = new wfeImage(sqlCon, pImage);
                    int imgCount = wImage.GetImageCount();
                    imgCount++;
                    int pageCount = wImage.GetMaxPageCount();
                    pageCount++;
                    newImageName = policyLst.SelectedItem.ToString() + "_" + pageCount.ToString().PadLeft(3, '0') + "_A.TIF";
                    if ((GetPolicyStatus() == (int)eSTATES.POLICY_FQC) || (GetPolicyStatus() == (int)eSTATES.POLICY_CHECKED) || (GetPolicyStatus() == (int)eSTATES.POLICY_EXPORTED))
                    {
                        if (Directory.Exists(newPolicyPath + "\\" + ihConstants._FQC_FOLDER))
                        {
                            File.Copy(imageName, newPolicyPath + "\\" + ihConstants._FQC_FOLDER + "\\" + newImageName, true);
                            File.Copy(imageName, newPolicyPath + "\\" + ihConstants._SCAN_FOLDER + "\\" + newImageName, true);
                        }
                        //else
                        //{
                        //    if (Directory.Exists(newPolicyPath + "\\" + ihConstants._INDEXING_FOLDER))
                        //    {
                        //        File.Copy(imageName, newPolicyPath + "\\" + ihConstants._INDEXING_FOLDER + "\\" + newImageName, true);
                        //    }
                        //    else
                        //    {
                        //        Directory.CreateDirectory(newPolicyPath + "\\" + ihConstants._INDEXING_FOLDER);
                        //        File.Copy(imageName, newPolicyPath + "\\" + ihConstants._INDEXING_FOLDER + "\\" + newImageName, true);
                        //    }
                        //}
                    }
                    else
                    {
                        if (Directory.Exists(newPolicyPath + "\\" + ihConstants._FQC_FOLDER))
                        {
                            File.Copy(imageName, newPolicyPath + "\\" + ihConstants._FQC_FOLDER + "\\" + newImageName, true);
                            File.Copy(imageName, newPolicyPath + "\\" + ihConstants._SCAN_FOLDER + "\\" + newImageName, true);
                        }
                        //if (Directory.Exists(newPolicyPath + "\\" + ihConstants._INDEXING_FOLDER))
                        //{
                        //    File.Copy(imageName, newPolicyPath + "\\" + ihConstants._INDEXING_FOLDER + "\\" + newImageName, true);
                        //}
                        //else
                        //{
                        //    Directory.CreateDirectory(newPolicyPath + "\\" + ihConstants._INDEXING_FOLDER);
                        //    File.Copy(imageName, newPolicyPath + "\\" + ihConstants._INDEXING_FOLDER + "\\" + newImageName, true);
                        //}
                    }
                    //MessageBox.Show(ds.Tables[0].Rows[i]["status"].ToString());
                    System.IO.FileInfo info = new System.IO.FileInfo(fileDlg.FileName.ToString());
                    fileSize = info.Length;
                    fileSize = fileSize / 1024;
                    //crd.created_by = "ADMIN";
                    crd.created_dttm = dbcon.GetCurrenctDTTM(1, sqlCon);
                    pImage = new CtrlImage(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), boxNumber, policyLst.SelectedItem.ToString(), newImageName, string.Empty);
                    wImage = new wfeImage(sqlCon, pImage);
                    //wImage.Save(crd, eSTATES.PAGE_FQC, fileSize, ihConstants._NORMAL_PAGE, imgCount,string.Empty);
                    if (GetPolicyStatus() == 6)
                    {
                        wImage.Save(crd, eSTATES.PAGE_EXPORTED, fileSize, ihConstants._NORMAL_PAGE, imgCount, newImageName);
                    }
                    else
                    {
                        if (wImage.Save(crd, eSTATES.PAGE_NOT_INDEXED, fileSize, ihConstants._NORMAL_PAGE, imgCount, newImageName) == true)
                        {
                            UpdateStatus(eSTATES.POLICY_NOT_INDEXED, crd);
                        }
                    }
                    lstImage.Items.Add(newImageName);
                }
                //if (policyLst.Items.Count != 1)
                //{
                //    if ((policyLst.SelectedIndex) != (policyLst.Items.Count - 1))
                //    {
                //        policyLst.SelectedIndex = policyLst.SelectedIndex + 1;
                //        policyLst.SelectedIndex = policyLst.SelectedIndex - 1;
                //    }
                //    else
                //    {
                //        policyLst.SelectedIndex = 0;
                //        policyLst.SelectedIndex = policyLst.SelectedIndex + 1;
                //    }
                //}
                //else
                //{
                //    RefreshNotify();
                //}
                MessageBox.Show("Image has been imported successfully.....");
                deLabel2.Text = "Total Scanned Image : " + lstImage.Items.Count.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while importing the selected image" + ex.Message);
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes("Project-" + wBox.ctrlBox.ProjectCode + " ,Batch-" + wBox.ctrlBox.BatchKey + " ,Box-" + wBox.ctrlBox.BoxNumber + "Image name-" + newImageName + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex);
            }
            return 1;
        }
        public void RefreshNotify()
        {
            ShowPolicyDetails();
            if (lstPolicy.Items.Count > 0)
            {
                lstPolicy.SelectedIndex = 0;
            }
            //PopulateImageList((int)lstPolicy.SelectedItem);
        }
        private int ImportImage()
        {
            string imageName = null;
            wfePolicy wPolicy = null;
            DataSet ds = new DataSet();
            NovaNet.wfe.eSTATES[] state = new NovaNet.wfe.eSTATES[5];
            wfeImage wImage = null;
            string newImageName = string.Empty;
            string origImageName = string.Empty;
            string policyNo = string.Empty;
            string batchName = string.Empty;
            string boxNumber = string.Empty;
            string tmpBondPath = Path.GetDirectoryName(Application.ExecutablePath);
            try
            {
                ihwQuery wQ = new ihwQuery(sqlCon);

                wfeBatch pBatch = new wfeBatch(sqlCon);
                policyLst = lstPolicy;
                imageLst = lstImage;

                state[0] = NovaNet.wfe.eSTATES.POLICY_INDEXED;
                state[1] = NovaNet.wfe.eSTATES.POLICY_FQC;
                state[2] = NovaNet.wfe.eSTATES.POLICY_CHECKED;
                state[3] = NovaNet.wfe.eSTATES.POLICY_EXPORTED;
                state[4] = NovaNet.wfe.eSTATES.POLICY_NOT_INDEXED;

                fileDlg.Filter = "TIF File|*.TIF";
                fileDlg.FileName = string.Empty;
                fileDlg.Title = "B'Zer - TIF Files";
                fileDlg.ShowDialog();
                imageName = fileDlg.FileName.ToString();

                if ((imageName != null) && ((imageName != string.Empty)))
                {
                    int j = 0;
                    for (int i = 0; i < policyLst.Items.Count; i++)
                    {
                        //MessageBox.Show(cmbPolicy.DisplayMember);
                        //copy to
                        policyNo = policyLst.Items[i].ToString();
                        batchName = pBatch.GetBatchName(wBox.ctrlBox.ProjectCode, wBox.ctrlBox.BatchKey);
                        boxNumber = wBox.ctrlBox.BoxNumber.ToString();

                        if (wQ.GetSysConfigValue(ihConstants.SPECIALBOND_KEY) == ihConstants.SPECIALBOND_VALUE)
                        {
                            ///This is for auto text add
                            Image image = Image.FromFile(fileDlg.FileName.ToString());
                            Bitmap bmp = new Bitmap(image);

                            bmp.SetResolution(200, 200);
                            Graphics g = Graphics.FromImage(bmp);
                            g.DrawString(batchName, new Font("Calibri", 11), Brushes.Black, new PointF(348, 395));
                            g.DrawString(boxNumber, new Font("Calibri", 11), Brushes.Black, new PointF(781, 395));
                            g.DrawString(policyNo, new Font("Calibri", 11), Brushes.Black, new PointF(1099, 395));
                            bmp.Save(tmpBondPath + "\\tempBond.tif", ImageFormat.Tiff);
                            imgBond.LoadBitmapFromFile(tmpBondPath + "\\tempBond.TIF");
                            imgBond.ConvertTo1Bpp(tmpBondPath + "\\tempBond.TIF", tmpBondPath + "\\tempBond.TIF");
                            imageName = tmpBondPath + "\\tempBond.TIF";
                        }
                        pPolicy = new CtrlPolicy(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), boxNumber, policyNo);
                        wPolicy = new wfePolicy(sqlCon, pPolicy);
                        string newPolicyPath = GetPolicyPath(policyNo); //wPolicy.GetPolicyPath();

                        pImage = new CtrlImage(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), boxNumber, policyNo, string.Empty, string.Empty);
                        wImage = new wfeImage(sqlCon, pImage);
                        int imgCount = wImage.GetImageCount();
                        int pageCount = wImage.GetMaxPageCount();
                        pageCount++;
                        imgCount++;
                        newImageName = policyNo + "_" + pageCount.ToString().PadLeft(3, '0') + "_A-Policy_bond.TIF";
                        origImageName = policyNo + "_" + pageCount.ToString().PadLeft(3, '0') + "_A.TIF";
                        string status = GetPolicyStatus().ToString();
                        if ((Convert.ToInt32(status) != (int)eSTATES.POLICY_ON_HOLD))
                        {
                            if ((Convert.ToInt32(status) == (int)eSTATES.POLICY_FQC) || ((Convert.ToInt32(status) == (int)eSTATES.POLICY_CHECKED)) || (Convert.ToInt32(status) == (int)eSTATES.POLICY_EXPORTED))
                            {
                                if (Directory.Exists(newPolicyPath + "\\" + ihConstants._FQC_FOLDER))
                                {
                                    File.Copy(imageName, newPolicyPath + "\\" + ihConstants._FQC_FOLDER + "\\" + newImageName, true);
                                    File.Copy(imageName, newPolicyPath + "\\" + ihConstants._SCAN_FOLDER + "\\" + origImageName, true);
                                }
                                else
                                {
                                    if (Directory.Exists(newPolicyPath + "\\" + ihConstants._INDEXING_FOLDER))
                                    {
                                        File.Copy(imageName, newPolicyPath + "\\" + ihConstants._INDEXING_FOLDER + "\\" + newImageName, true);
                                        File.Copy(imageName, newPolicyPath + "\\" + ihConstants._SCAN_FOLDER + "\\" + origImageName, true);
                                    }
                                }
                            }
                            else
                            {
                                if (Directory.Exists(newPolicyPath + "\\" + ihConstants._INDEXING_FOLDER))
                                {
                                    File.Copy(imageName, newPolicyPath + "\\" + ihConstants._INDEXING_FOLDER + "\\" + newImageName, true);
                                    File.Copy(imageName, newPolicyPath + "\\" + ihConstants._SCAN_FOLDER + "\\" + origImageName, true);
                                }
                            }
                            //MessageBox.Show(ds.Tables[0].Rows[i]["status"].ToString());
                            System.IO.FileInfo info = new System.IO.FileInfo(fileDlg.FileName.ToString());
                            fileSize = info.Length;
                            fileSize = fileSize / 1024;
                            //crd.created_by = "ADMIN";
                            crd.created_dttm = dbcon.GetCurrenctDTTM(1, sqlCon);
                            pImage = new CtrlImage(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), boxNumber,policyNo, origImageName, string.Empty);
                            wImage = new wfeImage(sqlCon, pImage);
                            //wImage.Save(crd, eSTATES.PAGE_FQC, fileSize, ihConstants._NORMAL_PAGE, imgCount,string.Empty);
                            if (Convert.ToInt32(status) == (int)eSTATES.POLICY_EXPORTED)
                            {
                                wImage.Save(crd, eSTATES.PAGE_EXPORTED, ihConstants.POLICYBOND_FILE, newImageName, fileSize, imgCount);
                            }
                            else
                            {
                                wImage.Save(crd, eSTATES.PAGE_FQC, ihConstants.POLICYBOND_FILE, newImageName, fileSize, imgCount);
                            }
                            //wPolicy.UpdateStatus(eSTATES.POLICY_INDEXED,crd);
                            j = j + 1;
                        }
                        //Refresh the image list
                        if (File.Exists(tmpBondPath + "\\tempBond.TIF"))
                        {
                            File.Delete(tmpBondPath + "\\tempBond.TIF");
                        }
                    }
                    if (j > 0)
                    {
                        if (policyLst.Items.Count != 1)
                        {
                            if ((policyLst.SelectedIndex) != (policyLst.Items.Count + 1))
                            {
                                policyLst.SelectedIndex = policyLst.SelectedIndex + 1;
                                policyLst.SelectedIndex = policyLst.SelectedIndex - 1;
                            }
                            else
                            {
                                policyLst.SelectedIndex = policyLst.SelectedIndex - 1;
                                policyLst.SelectedIndex = policyLst.SelectedIndex + 1;
                            }
                        }
                        else
                        {
                            imageLst.Items.Add(origImageName + "-" + ihConstants.POLICYBOND_FILE);
                        }
                        MessageBox.Show("Image has been imported successfully in " + j + " policies.....");
                        prmButtonCopyTo.Enabled = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while importing the selected image" + ex.Message);
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes("Project-" + wBox.ctrlBox.ProjectCode + " ,Batch-" + wBox.ctrlBox.BatchKey + " ,Box-" + wBox.ctrlBox.BoxNumber + "Image name-" + newImageName + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex);
            }
            return 1;
        }
        private string GetPolicyPath()
        {
            policyLst = lstPolicy;
            wfeBatch wBatch = new wfeBatch(sqlCon);
            string batchPath = GetPath(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey));
            return batchPath + "\\" + policyLst.SelectedItem.ToString();
        }
        public string GetPath(int prmProjKey, int prmBatchKey)
        {
            string sqlStr = null;
            DataSet projDs = new DataSet();
            string Path;

            try
            {
                sqlStr = @"select bundle_path from bundle_master where proj_code=" + prmProjKey + " and bundle_key=" + prmBatchKey;
                sqlAdap = new OdbcDataAdapter(sqlStr, sqlCon);
                sqlAdap.Fill(projDs);
            }
            catch (Exception ex)
            {
                sqlAdap.Dispose();
                //err = ex.Message;
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes(sqlStr + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex);
            }
            if (projDs.Tables[0].Rows.Count > 0)
            {
                Path = projDs.Tables[0].Rows[0]["bundle_path"].ToString();
            }
            else
                Path = string.Empty;

            return Path;
        }
        private int ScanImage()
        {
            DialogResult result;
            bool isOk;
            if (twScan.Select() == false)
                return 0;

            if (!msgfilter)
            {
                //this.Enabled = false;
                msgfilter = true;
                Application.AddMessageFilter(this);
            }
            result = MessageBox.Show("Do you want to scan in color mode?", "Scan", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (result == DialogResult.Yes)
            {
                colorMode = true;
                result = MessageBox.Show("Do you want to scan in duplex mode?", "Scan mode", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (result == DialogResult.Yes)
                {
                    // isOk = twScan.AcquireFixed(false, colorMode, ihConstants.MAX_NO_SCAN_FQC, 1);

                    isOk = isOk = twScan.Acquire(true);  // color duplex
                }
                else
                {
                   // isOk = twScan.AcquireFixed(false, colorMode, ihConstants.MAX_NO_SCAN_FQC, 0);

                    isOk = twScan.Acquire(false, 0);   // color simplex
                }
            }
            else
            {
                colorMode = false;
                result = MessageBox.Show("Do you want to scan in duplex mode?", "Scan mode", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (result == DialogResult.Yes)
                {
                    // isOk = twScan.AcquireFixed(false, colorMode, ihConstants.MAX_NO_SCAN_FQC, 1);

                    //isOk = twScan.Acquire(true);
                    isOk = twScan.Acquire(true, colorMode, 200); //bw duplex 200 dpi
                }
                else
                {
                    //isOk = twScan.AcquireFixed(false, colorMode, ihConstants.MAX_NO_SCAN_FQC, 0);
                    isOk = twScan.AcquireFixed(false, colorMode, ihConstants.MAX_NO_SCAN_FQC, 0, 200);// bw simplex 200 dpi
                }
            }


            scanWhat = ihConstants.SCAN_NEW_FQC;

            
            if (!isOk)
            {
                MessageBox.Show("Error in acquiring from scanner", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                EndingScan();
            }
            deLabel2.Text = "Total Scanned Image : " + lstImage.Items.Count.ToString();
            return 1;
        }
        private int RescanImage()
        {
            if (twScan.Select() == false)
                return 0;
            if (!msgfilter)
            {
                //this.Enabled = false;
                msgfilter = true;
                Application.AddMessageFilter(this);
            }
            DialogResult result = MessageBox.Show("Do you want to scan in color mode?", "Scan", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (result == DialogResult.Yes)
            {
                colorMode = true;
                bool isOk = twScan.Acquire(false, 0);  //color simplex
                if (!isOk)
                {
                    MessageBox.Show("Error in acquiring from scanner", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    EndingScan();
                }
            }
            else
            {
                colorMode = false;
                bool isOk = twScan.AcquireFixed(false, colorMode, ihConstants.MAX_NO_SCAN_FQC, 0, 200);
                if (!isOk)
                {
                    MessageBox.Show("Error in acquiring from scanner", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    EndingScan();
                }
            }
            scanWhat = ihConstants.SCAN_RE_FQC;
            //bool isOk = twScan.AcquireFixed(false, colorMode, 1, 0);
            
            return 1;
        }
        bool UpdateState(eSTATES prmPageSate, string prmPageName)
        {
            double fileSize;
            bool success = false;
            NovaNet.Utils.dbCon dbcon = new NovaNet.Utils.dbCon();
            try
            {
                pImage = new CtrlImage(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), boxNumber, policyLst.SelectedItem.ToString(), prmPageName, string.Empty);
                wfeImage wImage = new wfeImage(sqlCon, pImage);
                if (wImage.UpdateStatus(prmPageSate, crd))
                {
                    success = true;
                }
                else
                {
                    success = false;
                }
                imageLst = lstImage;
                if ((prmPageSate != eSTATES.PAGE_DELETED) && (insertFlag != true))
                {
                    System.IO.FileInfo info = new System.IO.FileInfo(indexFilePath);

                    fileSize = info.Length;
                    fileSize = fileSize / 1024;

                    wImage.UpdateImageSize(crd, eSTATES.PAGE_FQC, fileSize);
                }
                pPolicy = new CtrlPolicy(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), boxNumber, policyLst.SelectedItem.ToString());
                wfePolicy wPolicy = new wfePolicy(sqlCon, pPolicy);
                if ((wImage.GetImageCount(eSTATES.PAGE_NOT_INDEXED) == false) && (wImage.GetImageCount(eSTATES.PAGE_ON_HOLD) == false) && (wImage.GetImageCount(eSTATES.PAGE_RESCANNED_NOT_INDEXED) == false) && (wImage.GetImageCount(eSTATES.PAGE_EXPORTED) == false) && (policy.GetPolicyStatus() != (int)eSTATES.POLICY_ON_HOLD))
                {
                    crd.created_dttm = dbcon.GetCurrenctDTTM(1, sqlCon);
                    if (policy.GetLICLogStatus() == ihConstants._LIC_QA_POLICY_CHECKED)
                    {
                        UpdateStatus(eSTATES.POLICY_CHECKED, crd);
                    }
                    else if ((policy.GetLICLogStatus() == ihConstants._LIC_QA_POLICY_EXCEPTION) || (policy.GetLICLogStatus() == ihConstants._LIC_QA_POLICY_EXCEPTION_SLOVED))
                    {
                        UpdateStatus(eSTATES.POLICY_EXCEPTION, crd);
                    }
                    else
                    {
                        UpdateStatus(eSTATES.POLICY_FQC, crd);
                    }
                    ///update into transaction log
                    wPolicy.UpdateTransactionLog(eSTATES.POLICY_FQC, crd);
                }
                pBox = new CtrlBox(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), boxNumber);
                wfeBox box = new wfeBox(sqlCon, pBox);
                NovaNet.wfe.eSTATES[] state = new NovaNet.wfe.eSTATES[3];
                state[0] = NovaNet.wfe.eSTATES.POLICY_INDEXED;
                state[1] = NovaNet.wfe.eSTATES.POLICY_QC;
                state[2] = NovaNet.wfe.eSTATES.POLICY_SCANNED;

                if (wPolicy.GetPolicyCount(state) == 0)
                {
                    //box.UpdateStatus(eSTATES.BOX_FQC);
                }

                if (GetFileCount(projKey, bundleKey) == 0)
                {
                    ///Update the batch status
                    //wBatch.UpdateStatus(eSTATES.BATCH_SCANNED, wBox.ctrlBox.BatchKey);
                    UpdateBundleStatus(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while updaing the status " + ex.Message);
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes("Project-" + projKey + " ,Bundle-" + bundleKey + " ,Box-" + boxNumber + "Policy-" + policyLst.SelectedItem.ToString() + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex);
                success = false;
            }
            insertFlag = false;
            return success;
        }
        public bool UpdateStatus(eSTATES state, Credentials prmCrd, bool pLock)
        {
            string sqlStr = null;
            OdbcTransaction sqlTrans = null;
            bool commitBol = true;
            OdbcCommand sqlCmd = new OdbcCommand();

            sqlStr = @"update metadata_entry" +
                " set status=" + (int)state + ",modified_by='" + prmCrd.created_by + "',modified_dttm='" + prmCrd.created_dttm + "' where proj_code =" + ctrlPolicy.ProjectKey +
                " and bundle_key=" + ctrlPolicy.BatchKey +
                " and patient_id='" + ctrlPolicy.PolicyNumber + "' and status<>" + (int)eSTATES.POLICY_EXPORTED;

            try
            {

                sqlTrans = sqlCon.BeginTransaction();
                sqlCmd.Connection = sqlCon;
                sqlCmd.Transaction = sqlTrans;
                sqlCmd.CommandText = sqlStr;
                int i = sqlCmd.ExecuteNonQuery();
                sqlTrans.Commit();
                if (i > 0)
                {
                    commitBol = true;
                }
                else
                {
                    commitBol = false;
                }
            }
            catch (Exception ex)
            {
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
        void UpdateAllStatus()
        {
            string imageName;
            int pos = 0;
            wfeImage wImage = null;
            eSTATES pageState;
            DateTime stDt = DateTime.Now;
            imageLst = lstImage;
            policyLst = lstPolicy;
            ctrlPolicy = new CtrlPolicy(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), boxNumber, policyLst.SelectedItem.ToString());
            policy = new wfePolicy(sqlCon, ctrlPolicy);

            pImage = new CtrlImage(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), boxNumber, policyLst.SelectedItem.ToString(), string.Empty, string.Empty);
            wImage = new wfeImage(sqlCon, pImage);

            if ((wImage.GetImageCount(eSTATES.PAGE_NOT_INDEXED) == false) && (wImage.GetImageCount(eSTATES.PAGE_ON_HOLD) == false) && (wImage.GetImageCount(eSTATES.PAGE_RESCANNED_NOT_INDEXED) == false) && (GetPolicyStatus() != 6) && (GetPolicyStatus() != (int)eSTATES.POLICY_ON_HOLD))
            {
                if (policy.GetLICLogStatus() == ihConstants._LIC_QA_POLICY_CHECKED)
                {
                    UpdateStatus(eSTATES.POLICY_CHECKED, crd,true);
                    pageState = eSTATES.PAGE_CHECKED;
                }
                else if ((policy.GetLICLogStatus() == ihConstants._LIC_QA_POLICY_EXCEPTION) || (policy.GetLICLogStatus() == ihConstants._LIC_QA_POLICY_EXCEPTION_SLOVED))
                {
                    UpdateStatus(eSTATES.POLICY_EXCEPTION, crd, true);
                    pageState = eSTATES.PAGE_EXCEPTION;
                }
                else
                {
                    UpdateStatus(eSTATES.POLICY_FQC, crd, true);
                    pageState = eSTATES.PAGE_FQC;
                }
                if (crd.role != ihConstants._ADMINISTRATOR_ROLE)
                {
                    policy.UpdateTransactionLog(eSTATES.POLICY_FQC, crd);
                }
                //wfePolicy wPolicy = new wfePolicy(sqlCon);
                //int count =  wPolicy.GetTransactionLogCount(wBox.ctrlBox.BatchKey.ToString(), dbcon.GetCurrenctDTTM(2, sqlCon), crd.created_by, eSTATES.POLICY_FQC);
                this.Text = "Final QC";
                this.Text = this.Text;// +"                       Today you have done " + count + " ";
                for (int i = 0; i < imageLst.Items.Count; i++)
                {
                    imageName = imageLst.Items[i].ToString();
                    pos = imageName.IndexOf("-");
                    if (pos > 0)
                    {
                        imageName = imageName.Substring(0, pos);
                    }
                    //To get the index file name from del list selected file name.
                    pImage = new CtrlImage(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), boxNumber, policyLst.SelectedItem.ToString(), imageName, string.Empty);
                    wImage = new wfeImage(sqlCon, pImage);
                    wImage.UpdateStatus(pageState, crd);
                }
                if (GetFileCount(projKey, bundleKey) == 0)
                {
                    ///Update the batch status
                    //wBatch.UpdateStatus(eSTATES.BATCH_SCANNED, wBox.ctrlBox.BatchKey);
                    UpdateBundleStatus(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey));
                }
            }
            DateTime endDt = DateTime.Now;
            TimeSpan tpm = endDt - stDt;
            //MessageBox.Show(tpm.Milliseconds.ToString());
        }
        bool UpdateState(eSTATES prmPageSate, string prmPageName, string prmPolicyNumber)
        {
            bool delBol = false;
            double fileSize;
            NovaNet.Utils.dbCon dbcon = new NovaNet.Utils.dbCon();

            pImage = new CtrlImage(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), boxNumber, prmPolicyNumber, prmPageName, string.Empty);
            wfeImage wImage = new wfeImage(sqlCon, pImage);
            if (wImage.UpdateStatus(prmPageSate, crd) == true)
            {
                delBol = true;
            }

            imageLst = lstImage;
            if (pageDelInsrt == false)
            {
                System.IO.FileInfo info = new System.IO.FileInfo(indexFilePath);

                fileSize = info.Length;
                fileSize = fileSize / 1024;
                wImage.UpdateImageSize(crd, eSTATES.PAGE_INDEXED, fileSize);

            }

            return delBol;
        }
        bool UpdateState(eSTATES prmPageSate, string prmPageName, string prmDocType, string prmIndexImageName)
        {
            double fileSize;
            bool success = false;
            NovaNet.Utils.dbCon dbcon = new NovaNet.Utils.dbCon();
            try
            {
                policyLst = lstPolicy;
                ctrlPolicy = new CtrlPolicy(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), boxNumber, policyLst.SelectedItem.ToString());
                policy = new wfePolicy(sqlCon, ctrlPolicy);

                pImage = new CtrlImage(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), boxNumber, policyLst.SelectedItem.ToString(), prmPageName, string.Empty);
                wfeImage wImage = new wfeImage(sqlCon, pImage);
                if (wImage.UpdateStatusAndDockType(prmPageSate, prmDocType, prmIndexImageName, crd))
                {
                    if (prmPageSate == eSTATES.PAGE_DELETED)
                    {
                        pImage = new CtrlImage(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), boxNumber, policyLst.SelectedItem.ToString(), string.Empty, string.Empty);
                        wImage = new wfeImage(sqlCon, pImage);

                        if ((wImage.GetImageCount(eSTATES.PAGE_NOT_INDEXED) == false) && (wImage.GetImageCount(eSTATES.PAGE_ON_HOLD) == false) && (wImage.GetImageCount(eSTATES.PAGE_RESCANNED_NOT_INDEXED) == false) && (GetPolicyStatus() != 6) && (GetPolicyStatus() != (int)eSTATES.POLICY_ON_HOLD))
                        {
                            if (policy.GetLICLogStatus() == ihConstants._LIC_QA_POLICY_CHECKED)
                            {
                                UpdateStatus(eSTATES.POLICY_CHECKED, crd);
                            }
                            else if ((policy.GetLICLogStatus() == ihConstants._LIC_QA_POLICY_EXCEPTION) || (policy.GetLICLogStatus() == ihConstants._LIC_QA_POLICY_EXCEPTION_SLOVED))
                            {
                                UpdateStatus(eSTATES.POLICY_EXCEPTION, crd);
                            }
                            else
                            {
                                UpdateStatus(eSTATES.POLICY_FQC, crd);
                            }
                            policy.UpdateTransactionLog(eSTATES.POLICY_FQC, crd);
                        }
                        success = true;
                    }
                }
                else
                {
                    success = false;
                }
                imageLst = lstImage;
                if ((prmPageSate != eSTATES.PAGE_DELETED) && (insertFlag != true))
                {
                    if ((indexFilePath != null) && (indexingOn == false))
                    {
                        System.IO.FileInfo info = new System.IO.FileInfo(indexFilePath);

                        fileSize = info.Length;
                        fileSize = fileSize / 1024;

                        wImage.UpdateImageSize(crd, eSTATES.PAGE_FQC, fileSize);
                    }
                    pPolicy = new CtrlPolicy(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), boxNumber, policyLst.SelectedItem.ToString());
                    wfePolicy wPolicy = new wfePolicy(sqlCon, pPolicy);
                    //                
                    UpdateAllStatus();
                    pBox = new CtrlBox(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), boxNumber);
                    wfeBox box = new wfeBox(sqlCon, pBox);

                    NovaNet.wfe.eSTATES[] state = new NovaNet.wfe.eSTATES[3];
                    state[0] = NovaNet.wfe.eSTATES.POLICY_INDEXED;
                    state[1] = NovaNet.wfe.eSTATES.POLICY_QC;
                    state[2] = NovaNet.wfe.eSTATES.POLICY_SCANNED;

                    //if (wPolicy.GetPolicyCount(state) == 0)
                    //{
                    //    //box.UpdateStatus(eSTATES.BOX_FQC);
                    //}
                    if (GetFileCount(projKey, bundleKey) == 0)
                    {
                        ///Update the batch status
                        //wBatch.UpdateStatus(eSTATES.BATCH_SCANNED, wBox.ctrlBox.BatchKey);
                        UpdateBundleStatus(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while updaing the status " + ex.Message);
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes("Project-" + wBox.ctrlBox.ProjectCode + " ,Batch-" + wBox.ctrlBox.BatchKey + " ,Box-" + wBox.ctrlBox.BoxNumber + "Policy-" + policyLst.SelectedItem.ToString() + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex);
                success = false;
            }

            insertFlag = false;
            return success;
        }

        public int GetFileCount(string projkey, string bundleKey)
        {
            string sqlStr = null;
            DataSet dsImage = new DataSet();
            OdbcDataAdapter sqlAdap = null;

            sqlStr = "select patient_id from metadata_entry " +
                    " where proj_code=" + projkey +
                " and bundle_key=" + bundleKey + " and (status = '3' or status = '2' or status = '4')";


            try
            {
                sqlAdap = new OdbcDataAdapter(sqlStr, sqlCon);
                sqlAdap.Fill(dsImage);
            }
            catch (Exception ex)
            {
                sqlAdap.Dispose();
                MessageBox.Show(ex.Message);
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
                " set status=" + 5 + " where " +
                " bundle_key=" + prmBatchKey + " and proj_code = '" + prmProjKey + "' and (status = '2' or status = '3' or status = '4') and status<>" + 5;

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
        public bool UpdateStatus(eSTATES state, Credentials prmCrd)
        {
            string sqlStr = null;
            OdbcTransaction sqlTrans = null;
            bool commitBol = true;
            OdbcCommand sqlCmd = new OdbcCommand();

           

            sqlStr = @"update metadata_entry" +
                " set status=" + (int)state + ",modified_by='" + prmCrd.created_by + "',modified_dttm='" + prmCrd.created_dttm + "' where proj_code =" + ctrlPolicy.ProjectKey +
                " and bundle_key=" + ctrlPolicy.BatchKey +
                " and patient_id='" + ctrlPolicy.PolicyNumber + "' and status<>" + (int)eSTATES.POLICY_EXPORTED;

            try
            {

                sqlTrans = sqlCon.BeginTransaction();
                sqlCmd.Connection = sqlCon;
                sqlCmd.Transaction = sqlTrans;
                sqlCmd.CommandText = sqlStr;
                int i = sqlCmd.ExecuteNonQuery();
                sqlTrans.Commit();
                if (i > 0)
                {
                    commitBol = true;
                }
                else
                {
                    commitBol = false;
                }
            }
            catch (Exception ex)
            {
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

        int ImageDelete()
        {
            string qcDelPath = null;
            string sourceFileName = null;
            string destFileName = null;
            string qcFilePath = null;
            string originalFile = null;
            string scanPath = null;
            int pos;
            wfeImage wImage;

            try
            {
                qcDelPath = policyPath + "\\" + ihConstants._SCAN_FOLDER + "\\" + ihConstants._DELETE_FOLDER;

                imageLst = lstImage;
                delImgList = lstImageDel;
                pageDelInsrt = true;
                pos = imageLst.SelectedItem.ToString().IndexOf("-");
                if (pos <= 0)
                {
                    destFileName = imageLst.SelectedItem.ToString();
                    originalFile = destFileName;
                }
                else
                {
                    pos = imageLst.SelectedItem.ToString().IndexOf(".TIF");
                    destFileName = imageLst.SelectedItem.ToString().Substring(0, pos) + imageLst.SelectedItem.ToString().Substring(pos + 4) + ".TIF";
                    originalFile = imageLst.SelectedItem.ToString().Substring(0, pos) + ".TIF";
                }
                scanPath = policyPath + "\\" + ihConstants._SCAN_FOLDER + "\\" + originalFile;
                pImage = new CtrlImage(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), boxNumber, policyLst.SelectedItem.ToString(), imageLst.SelectedItem.ToString(), string.Empty);
                if (imageLst.SelectedItem.ToString().IndexOf("Signature_page") <= 0)
                {
                    if (UpdateState(eSTATES.PAGE_DELETED, originalFile, policyLst.SelectedItem.ToString()) == true)
                    {

                        sourceFileName = indexFolderName + "\\" + destFileName;
                        destFileName = qcDelPath + "\\" + originalFile;
                        qcFilePath = sourceFilePath + "\\" + originalFile;
                        if (FileorFolder.CreateFolder(qcDelPath) == true)
                        {
                            if (File.Exists(destFileName) == false)
                            {
                                File.Move(sourceFileName, destFileName);

                                File.Delete(scanPath);
                            }
                        }



                    }
                }
                else
                {
                    pImage = new CtrlImage(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), boxNumber, policyLst.SelectedItem.ToString(), originalFile, string.Empty);
                    wImage = new wfeImage(sqlCon, pImage);
                    if (wImage.DeleteImage() == true)
                    {
                        sourceFileName = indexFolderName + "\\" + destFileName;
                        destFileName = qcDelPath + "\\" + originalFile;
                        qcFilePath = sourceFilePath + "\\" + originalFile;
                        if (FileorFolder.CreateFolder(qcDelPath) == true)
                        {
                            if (File.Exists(destFileName) == false)
                            {
                                File.Delete(sourceFileName);
                                File.Delete(scanPath);
                            }
                        }
                    }
                }
                DeleteNotify(imageLst.SelectedIndex);
                DisplayDocTypeCount();
                ShowImage(true);
                deLabel2.Text = "Total Scanned Image : " + lstImage.Items.Count.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while doing the operation..." + ex.Message);
            }
            return 0;
        }

        private void DisplayDockTypes()
        {
            config = new ImageConfig(ihConstants.CONFIG_FILE_PATH);
            char PROPOSALFORM = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.PROPOSALFORM_KEY).Remove(1, 1).Trim());
            char PHOTOADDENDUM = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.PHOTOADDENDUM_KEY).Remove(1, 1).Trim());
            char PROPOSALENCLOSERS = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.PROPOSALENCLOSERS_KEY).Remove(1, 1).Trim());
            char SIGNATUREPAGE = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.SIGNATUREPAGE_KEY).Remove(1, 1).Trim());
            char MEDICALREPORT = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.MEDICALREPORT_KEY).Remove(1, 1).Trim());
            char PROPOSALREVIEWSLIP = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.PROPOSALREVIEWSLIP_KEY).Remove(1, 1).Trim());
            char POLICYBOND = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.POLICYBOND_KEY).Remove(1, 1).Trim());
            char NOMINATION = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.NOMINATION_KEY).Remove(1, 1).Trim());
            char ASSIGNMENT = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.ASSIGNMENT_KEY).Remove(1, 1).Trim());
            char ALTERATION = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.ALTERATION_KEY).Remove(1, 1).Trim());
            char REVIVALS = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.REVIVALS_KEY).Remove(1, 1).Trim());
            char POLICYLOANS = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.POLICYLOANS_KEY).Remove(1, 1).Trim());
            char SURRENDER = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.SURRENDER_KEY).Remove(1, 1).Trim());
            char CLAIMS = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.CLAIMS_KEY).Remove(1, 1).Trim());
            char CORRESPONDENCE = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.CORRESPONDENCE_KEY).Remove(1, 1).Trim());
            char OTHERS = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.OTHERS_KEY).Remove(1, 1).Trim());
            char KYCDOCUMENT = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.KYCDOCUMENT_KEY).Remove(1, 1).Trim());
            //char MAINPETITION = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.MAINPETITION_KEY).Remove(1, 1).Trim());
            //char MAINPETITIONANNEXURES = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.MAINPETITIONANNEXTURES_KEY).Remove(1, 1).Trim());
            //char AFFIDAVITSWITHANNEXURES = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.AFFIDAVITSWITHANNEXTURES_KEY).Remove(1, 1).Trim());
            //char WRITTENSTATEMENTOBJECTION = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.WRITTENSTATEMENTOBJECTION_KEY).Remove(1, 1).Trim());
            //char CONNECTEDAPPLICATIONS = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.CONNECTEDAPPLICATIONS_KEY).Remove(1, 1).Trim());
            //char ANALOGOUSANDCONNECTEDCASE = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.ANALOGOUSANDCONNECTEDCASE_KEY).Remove(1, 1).Trim());
            //char VAKALATNAMAANDWARRENT = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.VAKALATNAMAANDWARRENT_KEY).Remove(1, 1).Trim());
            //char SUMMONS = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.SUMMONS_KEY).Remove(1, 1).Trim());
            //char NOTICE = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.NOTICE_KEY).Remove(1, 1).Trim());
            //char WITNESSACTIONDEPOSITION = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.WITNESSACTIONDEPOSITION_KEY).Remove(1, 1).Trim());
            //char ISSUES = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.ISSUES_KEY).Remove(1, 1).Trim());
            //char EXHIBITS = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.EXHIBITS_KEY).Remove(1, 1).Trim());
            //char DRAFTPAPERS = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.DRAFTPAPERS_KEY).Remove(1, 1).Trim());
            //char NOTICEOFARGUMENT = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.NOTICEOFARGUMENT_KEY).Remove(1, 1).Trim());
            //char ENGROSSEDPRELIMINARY = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.ENGROSSEDPRELIMINARY_KEY).Remove(1, 1).Trim());
            //char ORDERSMAINCASE = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.ORDERSMAINCASE_KEY).Remove(1, 1).Trim());
            //char ORDERSAPPLICATIONS = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.ORDERSAPPLICATIONS_KEY).Remove(1, 1).Trim());
            //char FINALJUDGEMENTORDER = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.FINALJUDGEMENTORDER_KEY).Remove(1, 1).Trim());
            //char LOWERCOURTRECORDS = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.LOWERCOURTRECORDS_KEY).Remove(1, 1).Trim());
            //char IMPUGNEDORDER = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.IMPUGNEDORDER_KEY).Remove(1, 1).Trim());
            //char PAPERBOOK = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.PAPERBOOK_KEY).Remove(1, 1).Trim());
            char DELETE = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.DELETE_KEY).Remove(1, 1).Trim());
            char PATIENTIDENTIFICATION = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.PATIENTIDENTIFICATION_KEY).Remove(1, 1).Trim());
            char COMPLAINS = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.COMPLAINS_KEY).Remove(1, 1).Trim());
            char ADMISSIONSHEET = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.ADMISSIONSHEET_KEY).Remove(1, 1).Trim());
            char PHYSICIANORDERSHEET = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.PHYSICIANORDERSHEET_KEY).Remove(1, 1).Trim());
            char ADMISSIONASSESSMENT = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.ADMISSIONASSESSMENT_KEY).Remove(1, 1).Trim());
            char FAMILYHISTORY = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.FAMILYHISTORY_KEY).Remove(1, 1).Trim());
            char CONSENTFORTREATMENT = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.CONSENTFORTREATMENT_KEY).Remove(1, 1).Trim());
            char PHYSICALEXAMINATION = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.PHYSICALEXAMINATION_KEY).Remove(1, 1).Trim());
            char MEDICALHISTORY = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.MEDICALHISTORY_KEY).Remove(1, 1).Trim());
            char TENTATIVEHISTORY = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.TENTATIVEHISTORY_KEY).Remove(1, 1).Trim());
            char MEDICALDIAGNOSIS = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.MEDICALDIAGNOSIS_KEY).Remove(1, 1).Trim());
            char CAREGIVEN = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.CAREGIVEN_KEY).Remove(1, 1).Trim());
            char TRAETMENTGIVEN = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.TRAETMENTGIVEN_KEY).Remove(1, 1).Trim());
            char THERATPEUTICORDER = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.THERATPEUTICORDER_KEY).Remove(1, 1).Trim());
            char PROGRESSNOTES = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.PROGRESSNOTES_KEY).Remove(1, 1).Trim());
            char OPERATIVEPROCEDURE = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.OPERATIVEPROCEDURE_KEY).Remove(1, 1).Trim());
            char DISCHARGEREPORT = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.DISCHARGEREPORT_KEY).Remove(1, 1).Trim());
            char DEATHREPORT = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.DEATHREPORT_KEY).Remove(1, 1).Trim());
            lvwDockTypes.Items.Clear();
            /*ListViewItem lvwItem = lvwDockTypes.Items.Add("PROPOSAL FORM");
            lvwItem.SubItems.Add(PROPOSALFORM.ToString());
            lvwItem.SubItems.Add("0");

            lvwItem = lvwDockTypes.Items.Add("PHOTO ADDENDUM");
            lvwItem.SubItems.Add(PHOTOADDENDUM.ToString());
            lvwItem.SubItems.Add("0");

            lvwItem = lvwDockTypes.Items.Add("PROPOSAL ENCLOSERS");
            lvwItem.SubItems.Add(PROPOSALENCLOSERS.ToString());
            lvwItem.SubItems.Add("0");

            lvwItem = lvwDockTypes.Items.Add("SIGNATURE PAGE");
            lvwItem.SubItems.Add(SIGNATUREPAGE.ToString());
            lvwItem.SubItems.Add("0");

            lvwItem = lvwDockTypes.Items.Add("MEDICAL REPORT");
            lvwItem.SubItems.Add(MEDICALREPORT.ToString());
            lvwItem.SubItems.Add("0");

            lvwItem = lvwDockTypes.Items.Add("PROPOSAL REVIEW SLIP");
            lvwItem.SubItems.Add(PROPOSALREVIEWSLIP.ToString());
            lvwItem.SubItems.Add("0");

            lvwItem = lvwDockTypes.Items.Add("POLICY BOND");
            lvwItem.SubItems.Add(POLICYBOND.ToString());
            lvwItem.SubItems.Add("0");

            lvwItem = lvwDockTypes.Items.Add("NOMINATION");
            lvwItem.SubItems.Add(NOMINATION.ToString());
            lvwItem.SubItems.Add("0");

            lvwItem = lvwDockTypes.Items.Add("ASSIGNMENT");
            lvwItem.SubItems.Add(ASSIGNMENT.ToString());
            lvwItem.SubItems.Add("0");

            lvwItem = lvwDockTypes.Items.Add("ALTERATION");
            lvwItem.SubItems.Add(ALTERATION.ToString());
            lvwItem.SubItems.Add("0");

            lvwItem = lvwDockTypes.Items.Add("REVIVALS");
            lvwItem.SubItems.Add(REVIVALS.ToString());
            lvwItem.SubItems.Add("0");

            lvwItem = lvwDockTypes.Items.Add("POLICY LOANS");
            lvwItem.SubItems.Add(POLICYLOANS.ToString());
            lvwItem.SubItems.Add("0");

            lvwItem = lvwDockTypes.Items.Add("SURRENDER");
            lvwItem.SubItems.Add(SURRENDER.ToString());
            lvwItem.SubItems.Add("0");

            lvwItem = lvwDockTypes.Items.Add("CLAIMS");
            lvwItem.SubItems.Add(CLAIMS.ToString());
            lvwItem.SubItems.Add("0");

            lvwItem = lvwDockTypes.Items.Add("CORRESPONDENCE");
            lvwItem.SubItems.Add(CORRESPONDENCE.ToString());
            lvwItem.SubItems.Add("0");

            

            lvwItem = lvwDockTypes.Items.Add("KYC DOCUMENT");
            lvwItem.SubItems.Add(KYCDOCUMENT.ToString());
            lvwItem.SubItems.Add("0");*/

            //ListViewItem lvwItem = lvwDockTypes.Items.Add("MAIN PETITION");
            //lvwItem.SubItems.Add(MAINPETITION.ToString());
            //lvwItem.SubItems.Add("0");

            //lvwItem = lvwDockTypes.Items.Add("MAIN PETITION ANNEXURES");
            //lvwItem.SubItems.Add(MAINPETITIONANNEXURES.ToString());
            //lvwItem.SubItems.Add("0");

            //lvwItem = lvwDockTypes.Items.Add("AFFIDAVITS WITH ANNEXURES");
            //lvwItem.SubItems.Add(AFFIDAVITSWITHANNEXURES.ToString());
            //lvwItem.SubItems.Add("0");

            //lvwItem = lvwDockTypes.Items.Add("WRITTEN STATEMENT OBJECTION");
            //lvwItem.SubItems.Add(WRITTENSTATEMENTOBJECTION.ToString());
            //lvwItem.SubItems.Add("0");

            //lvwItem = lvwDockTypes.Items.Add("CONNECTED APPLICATIONS");
            //lvwItem.SubItems.Add(CONNECTEDAPPLICATIONS.ToString());
            //lvwItem.SubItems.Add("0");

            //lvwItem = lvwDockTypes.Items.Add("ANALOGOUS AND CONNECTED CASE");
            //lvwItem.SubItems.Add(ANALOGOUSANDCONNECTEDCASE.ToString());
            //lvwItem.SubItems.Add("0");

            //lvwItem = lvwDockTypes.Items.Add("VAKALATNAMA AND WARRENT");
            //lvwItem.SubItems.Add(VAKALATNAMAANDWARRENT.ToString());
            //lvwItem.SubItems.Add("0");

            //lvwItem = lvwDockTypes.Items.Add("SUMMONS");
            //lvwItem.SubItems.Add(SUMMONS.ToString());
            //lvwItem.SubItems.Add("0");

            //lvwItem = lvwDockTypes.Items.Add("NOTICE");
            //lvwItem.SubItems.Add(NOTICE.ToString());
            //lvwItem.SubItems.Add("0");

            //lvwItem = lvwDockTypes.Items.Add("WITNESS ACTION DEPOSITION");
            //lvwItem.SubItems.Add(WITNESSACTIONDEPOSITION.ToString());
            //lvwItem.SubItems.Add("0");

            //lvwItem = lvwDockTypes.Items.Add("ISSUES");
            //lvwItem.SubItems.Add(ISSUES.ToString());
            //lvwItem.SubItems.Add("0");

            //lvwItem = lvwDockTypes.Items.Add("EXHIBITS");
            //lvwItem.SubItems.Add(EXHIBITS.ToString());
            //lvwItem.SubItems.Add("0");

            //lvwItem = lvwDockTypes.Items.Add("DRAFT PAPERS");
            //lvwItem.SubItems.Add(DRAFTPAPERS.ToString());
            //lvwItem.SubItems.Add("0");

            //lvwItem = lvwDockTypes.Items.Add("NOTICE OF ARGUMENT");
            //lvwItem.SubItems.Add(NOTICEOFARGUMENT.ToString());
            //lvwItem.SubItems.Add("0");

            //lvwItem = lvwDockTypes.Items.Add("ENGROSSED PRELIMINARY");
            //lvwItem.SubItems.Add(ENGROSSEDPRELIMINARY.ToString());
            //lvwItem.SubItems.Add("0");

            //lvwItem = lvwDockTypes.Items.Add("ORDERS MAIN CASE");
            //lvwItem.SubItems.Add(ORDERSMAINCASE.ToString());
            //lvwItem.SubItems.Add("0");

            //lvwItem = lvwDockTypes.Items.Add("ORDERS CONNECTED APPLICATIONS");
            //lvwItem.SubItems.Add(ORDERSAPPLICATIONS.ToString());
            //lvwItem.SubItems.Add("0");

            //lvwItem = lvwDockTypes.Items.Add("FINAL JUDGEMENT ORDER");
            //lvwItem.SubItems.Add(FINALJUDGEMENTORDER.ToString());
            //lvwItem.SubItems.Add("0");

            //lvwItem = lvwDockTypes.Items.Add("LOWER COURT RECORDS");
            //lvwItem.SubItems.Add(LOWERCOURTRECORDS.ToString());
            //lvwItem.SubItems.Add("0");

            //lvwItem = lvwDockTypes.Items.Add("IMPUGNED ORDER");
            //lvwItem.SubItems.Add(IMPUGNEDORDER.ToString());
            //lvwItem.SubItems.Add("0");

            //lvwItem = lvwDockTypes.Items.Add("PAPER BOOK");
            //lvwItem.SubItems.Add(PAPERBOOK.ToString());
            //lvwItem.SubItems.Add("0");



            ListViewItem lvwItem = lvwDockTypes.Items.Add("PATIENT IDENTIFICATION");
            lvwItem.SubItems.Add(PATIENTIDENTIFICATION.ToString());
            lvwItem.SubItems.Add("0");

            lvwItem = lvwDockTypes.Items.Add("COMPLAINTS");
            lvwItem.SubItems.Add(COMPLAINS.ToString());
            lvwItem.SubItems.Add("0");

            lvwItem = lvwDockTypes.Items.Add("ADMISSION SHEET");
            lvwItem.SubItems.Add(ADMISSIONSHEET.ToString());
            lvwItem.SubItems.Add("0");

            lvwItem = lvwDockTypes.Items.Add("PHYSICIAN ORDER SHEET");
            lvwItem.SubItems.Add(PHYSICIANORDERSHEET.ToString());
            lvwItem.SubItems.Add("0");

            lvwItem = lvwDockTypes.Items.Add("ADMISSION ASSESSMENT");
            lvwItem.SubItems.Add(ADMISSIONASSESSMENT.ToString());
            lvwItem.SubItems.Add("0");

            lvwItem = lvwDockTypes.Items.Add("FAMILY HISTORY");
            lvwItem.SubItems.Add(FAMILYHISTORY.ToString());
            lvwItem.SubItems.Add("0");

            lvwItem = lvwDockTypes.Items.Add("CONSENT FOR TREATMENT");
            lvwItem.SubItems.Add(CONSENTFORTREATMENT.ToString());
            lvwItem.SubItems.Add("0");

            lvwItem = lvwDockTypes.Items.Add("PHYSICAL EXAMINATION");
            lvwItem.SubItems.Add(PHYSICALEXAMINATION.ToString());
            lvwItem.SubItems.Add("0");

            lvwItem = lvwDockTypes.Items.Add("MEDICAL HISTORY");
            lvwItem.SubItems.Add(MEDICALHISTORY.ToString());
            lvwItem.SubItems.Add("0");

            lvwItem = lvwDockTypes.Items.Add("TENTATIVE HISTORY");
            lvwItem.SubItems.Add(TENTATIVEHISTORY.ToString());
            lvwItem.SubItems.Add("0");

            lvwItem = lvwDockTypes.Items.Add("MEDICAL DIAGNOSIS");
            lvwItem.SubItems.Add(MEDICALDIAGNOSIS.ToString());
            lvwItem.SubItems.Add("0");

            lvwItem = lvwDockTypes.Items.Add("CARE GIVEN");
            lvwItem.SubItems.Add(CAREGIVEN.ToString());
            lvwItem.SubItems.Add("0");

            lvwItem = lvwDockTypes.Items.Add("TRAETMENT GIVEN");
            lvwItem.SubItems.Add(TRAETMENTGIVEN.ToString());
            lvwItem.SubItems.Add("0");

            lvwItem = lvwDockTypes.Items.Add("THERATPEUTIC ORDER");
            lvwItem.SubItems.Add(THERATPEUTICORDER.ToString());
            lvwItem.SubItems.Add("0");

            lvwItem = lvwDockTypes.Items.Add("PROGRESS NOTES");
            lvwItem.SubItems.Add(PROGRESSNOTES.ToString());
            lvwItem.SubItems.Add("0");

            lvwItem = lvwDockTypes.Items.Add("OPERATIVE PROCEDURE");
            lvwItem.SubItems.Add(OPERATIVEPROCEDURE.ToString());
            lvwItem.SubItems.Add("0");

            lvwItem = lvwDockTypes.Items.Add("DISCHARGE REPORT");
            lvwItem.SubItems.Add(DISCHARGEREPORT.ToString());
            lvwItem.SubItems.Add("0");

            lvwItem = lvwDockTypes.Items.Add("DEATH REPORT");
            lvwItem.SubItems.Add(DEATHREPORT.ToString());
            lvwItem.SubItems.Add("0");

            lvwItem = lvwDockTypes.Items.Add("MEDICAL REPORT");
            lvwItem.SubItems.Add(MEDICALREPORT.ToString());
            lvwItem.SubItems.Add("0");

            lvwItem = lvwDockTypes.Items.Add("OTHERS");
            lvwItem.SubItems.Add(OTHERS.ToString());
            lvwItem.SubItems.Add("0");

            //		lvwItem=lvwDockTypes.Items.Add("DELETE");
            //		lvwItem.SubItems.Add(DELETE.ToString());
            //		lvwItem.SubItems.Add("0");
        }
        private void DisplayDocTypeCount()
        {

            int pos;

            DisplayDockTypes();
            for (int i = 0; i < lstImage.Items.Count; i++)
            {
                pos = lstImage.Items[i].ToString().IndexOf("-");
                docType = lstImage.Items[i].ToString().Substring(pos + 1);
                /*if (docType == ihConstants.PROPOSALFORM_FILE)
                {
                    lvwDockTypes.Items[0].SubItems[2].Text = Convert.ToString((Convert.ToInt32(lvwDockTypes.Items[0].SubItems[2].Text) + 1));
                }
                if (docType == ihConstants.PHOTOADDENDUM_FILE)
                {
                    lvwDockTypes.Items[1].SubItems[2].Text = Convert.ToString((Convert.ToInt32(lvwDockTypes.Items[1].SubItems[2].Text) + 1));
                }
                if (docType == ihConstants.PROPOSALENCLOSERS_FILE)
                {
                    lvwDockTypes.Items[2].SubItems[2].Text = Convert.ToString((Convert.ToInt32(lvwDockTypes.Items[2].SubItems[2].Text) + 1));
                }
                if (docType == ihConstants.SIGNATUREPAGE_FILE)
                {
                    lvwDockTypes.Items[3].SubItems[2].Text = Convert.ToString((Convert.ToInt32(lvwDockTypes.Items[3].SubItems[2].Text) + 1));
                }
                if (docType == ihConstants.MEDICALREPORT_FILE)
                {
                    lvwDockTypes.Items[4].SubItems[2].Text = Convert.ToString((Convert.ToInt32(lvwDockTypes.Items[4].SubItems[2].Text) + 1));
                }
                if (docType == ihConstants.PROPOSALREVIEWSLIP_FILE)
                {
                    lvwDockTypes.Items[5].SubItems[2].Text = Convert.ToString((Convert.ToInt32(lvwDockTypes.Items[5].SubItems[2].Text) + 1));
                }
                if (docType == ihConstants.POLICYBOND_FILE)
                {
                    lvwDockTypes.Items[6].SubItems[2].Text = Convert.ToString((Convert.ToInt32(lvwDockTypes.Items[6].SubItems[2].Text) + 1));
                }
                if (docType == ihConstants.NOMINATION_FILE)
                {
                    lvwDockTypes.Items[7].SubItems[2].Text = Convert.ToString((Convert.ToInt32(lvwDockTypes.Items[7].SubItems[2].Text) + 1));
                }
                if (docType == ihConstants.ASSIGNMENT_FILE)
                {
                    lvwDockTypes.Items[8].SubItems[2].Text = Convert.ToString((Convert.ToInt32(lvwDockTypes.Items[8].SubItems[2].Text) + 1));
                } if (docType == ihConstants.ALTERATION_FILE)
                {
                    lvwDockTypes.Items[9].SubItems[2].Text = Convert.ToString((Convert.ToInt32(lvwDockTypes.Items[9].SubItems[2].Text) + 1));
                } if (docType == ihConstants.REVIVALS_FILE)
                {
                    lvwDockTypes.Items[10].SubItems[2].Text = Convert.ToString((Convert.ToInt32(lvwDockTypes.Items[10].SubItems[2].Text) + 1));
                } if (docType == ihConstants.POLICYLOANS_FILE)
                {
                    lvwDockTypes.Items[11].SubItems[2].Text = Convert.ToString((Convert.ToInt32(lvwDockTypes.Items[11].SubItems[2].Text) + 1));
                } if (docType == ihConstants.SURRENDER_FILE)
                {
                    lvwDockTypes.Items[12].SubItems[2].Text = Convert.ToString((Convert.ToInt32(lvwDockTypes.Items[12].SubItems[2].Text) + 1));
                } if (docType == ihConstants.CLAIMS_FILE)
                {
                    lvwDockTypes.Items[13].SubItems[2].Text = Convert.ToString((Convert.ToInt32(lvwDockTypes.Items[13].SubItems[2].Text) + 1));
                } if (docType == ihConstants.CORRESPONDENCE_FILE)
                {
                    lvwDockTypes.Items[14].SubItems[2].Text = Convert.ToString((Convert.ToInt32(lvwDockTypes.Items[14].SubItems[2].Text) + 1));
                } 
                if (docType == ihConstants.KYCDOCUMENT_FILE)
                {
                    lvwDockTypes.Items[16].SubItems[2].Text = Convert.ToString((Convert.ToInt32(lvwDockTypes.Items[16].SubItems[2].Text) + 1));
                }*/
                if (docType == ihConstants.PATIENTIDENTIFICATION_FILE)
                {
                    lvwDockTypes.Items[0].SubItems[2].Text = Convert.ToString((Convert.ToInt32(lvwDockTypes.Items[0].SubItems[2].Text) + 1));
                }
                //if (docType == ihConstants.MAINPETITION_FILE)
                //{
                //    lvwDockTypes.Items[0].SubItems[2].Text = Convert.ToString((Convert.ToInt32(lvwDockTypes.Items[0].SubItems[2].Text) + 1));
                //}
                if (docType == ihConstants.COMPLAINS_FILE)
                {
                    lvwDockTypes.Items[1].SubItems[2].Text = Convert.ToString((Convert.ToInt32(lvwDockTypes.Items[1].SubItems[2].Text) + 1));
                }
                //if (docType == ihConstants.MAINPETITIONANNEXTURES_FILE)
                //{
                //    lvwDockTypes.Items[1].SubItems[2].Text = Convert.ToString((Convert.ToInt32(lvwDockTypes.Items[1].SubItems[2].Text) + 1));
                //}
                if (docType == ihConstants.ADMISSIONSHEET_FILE)
                {
                    lvwDockTypes.Items[2].SubItems[2].Text = Convert.ToString((Convert.ToInt32(lvwDockTypes.Items[2].SubItems[2].Text) + 1));
                }
                //if (docType == ihConstants.AFFIDAVITSWITHANNEXTURES_FILE)
                //{
                //    lvwDockTypes.Items[2].SubItems[2].Text = Convert.ToString((Convert.ToInt32(lvwDockTypes.Items[2].SubItems[2].Text) + 1));
                //}
                //if (docType == ihConstants.WRITTENSTATEMENTOBJECTION_FILE)
                //{
                //    lvwDockTypes.Items[3].SubItems[2].Text = Convert.ToString((Convert.ToInt32(lvwDockTypes.Items[3].SubItems[2].Text) + 1));
                //}
                //if (docType == ihConstants.CONNECTEDAPPLICATIONS_FILE)
                //{
                //    lvwDockTypes.Items[4].SubItems[2].Text = Convert.ToString((Convert.ToInt32(lvwDockTypes.Items[4].SubItems[2].Text) + 1));
                //}
                //if (docType == ihConstants.ANALOGOUSANDCONNECTEDCASE_FILE)
                //{
                //    lvwDockTypes.Items[5].SubItems[2].Text = Convert.ToString((Convert.ToInt32(lvwDockTypes.Items[5].SubItems[2].Text) + 1));
                //}
                //if (docType == ihConstants.VAKALATNAMAANDWARRENT_FILE)
                //{
                //    lvwDockTypes.Items[6].SubItems[2].Text = Convert.ToString((Convert.ToInt32(lvwDockTypes.Items[6].SubItems[2].Text) + 1));
                //}
                //if (docType == ihConstants.SUMMONS_FILE)
                //{
                //    lvwDockTypes.Items[7].SubItems[2].Text = Convert.ToString((Convert.ToInt32(lvwDockTypes.Items[7].SubItems[2].Text) + 1));
                //}
                //if (docType == ihConstants.NOTICE_FILE)
                //{
                //    lvwDockTypes.Items[8].SubItems[2].Text = Convert.ToString((Convert.ToInt32(lvwDockTypes.Items[8].SubItems[2].Text) + 1));
                //}
                //if (docType == ihConstants.WITNESSACTIONDEPOSITION_FILE)
                //{
                //    lvwDockTypes.Items[9].SubItems[2].Text = Convert.ToString((Convert.ToInt32(lvwDockTypes.Items[9].SubItems[2].Text) + 1));
                //}
                //if (docType == ihConstants.ISSUES_FILE)
                //{
                //    lvwDockTypes.Items[10].SubItems[2].Text = Convert.ToString((Convert.ToInt32(lvwDockTypes.Items[10].SubItems[2].Text) + 1));
                //}
                //if (docType == ihConstants.EXHIBITS_FILE)
                //{
                //    lvwDockTypes.Items[11].SubItems[2].Text = Convert.ToString((Convert.ToInt32(lvwDockTypes.Items[11].SubItems[2].Text) + 1));
                //}
                //if (docType == ihConstants.DRAFTPAPERS_FILE)
                //{
                //    lvwDockTypes.Items[12].SubItems[2].Text = Convert.ToString((Convert.ToInt32(lvwDockTypes.Items[12].SubItems[2].Text) + 1));
                //}
                //if (docType == ihConstants.NOTICEOFARGUMENT_FILE)
                //{
                //    lvwDockTypes.Items[13].SubItems[2].Text = Convert.ToString((Convert.ToInt32(lvwDockTypes.Items[13].SubItems[2].Text) + 1));
                //}
                //if (docType == ihConstants.ENGROSSEDPRELIMINARY_FILE)
                //{
                //    lvwDockTypes.Items[14].SubItems[2].Text = Convert.ToString((Convert.ToInt32(lvwDockTypes.Items[14].SubItems[2].Text) + 1));
                //}
                //if (docType == ihConstants.ORDERSMAINCASE_FILE)
                //{
                //    lvwDockTypes.Items[15].SubItems[2].Text = Convert.ToString((Convert.ToInt32(lvwDockTypes.Items[15].SubItems[2].Text) + 1));
                //}
                //if (docType == ihConstants.ORDERSAPPLICATIONS_FILE)
                //{
                //    lvwDockTypes.Items[16].SubItems[2].Text = Convert.ToString((Convert.ToInt32(lvwDockTypes.Items[16].SubItems[2].Text) + 1));
                //}
                //if (docType == ihConstants.FINALJUDGEMENTORDER_FILE)
                //{
                //    lvwDockTypes.Items[17].SubItems[2].Text = Convert.ToString((Convert.ToInt32(lvwDockTypes.Items[17].SubItems[2].Text) + 1));
                //}
                //if (docType == ihConstants.LOWERCOURTRECORDS_FILE)
                //{
                //    lvwDockTypes.Items[18].SubItems[2].Text = Convert.ToString((Convert.ToInt32(lvwDockTypes.Items[18].SubItems[2].Text) + 1));
                //}
                //if (docType == ihConstants.IMPUGNEDORDER_FILE)
                //{
                //    lvwDockTypes.Items[19].SubItems[2].Text = Convert.ToString((Convert.ToInt32(lvwDockTypes.Items[19].SubItems[2].Text) + 1));
                //}
                //if (docType == ihConstants.PAPERBOOK_FILE)
                //{
                //    lvwDockTypes.Items[20].SubItems[2].Text = Convert.ToString((Convert.ToInt32(lvwDockTypes.Items[20].SubItems[2].Text) + 1));
                //}
                if (docType == ihConstants.PHYSICIANORDERSHEET_FILE)
                {
                    lvwDockTypes.Items[3].SubItems[2].Text = Convert.ToString((Convert.ToInt32(lvwDockTypes.Items[3].SubItems[2].Text) + 1));
                }
                if (docType == ihConstants.ADMISSIONASSESSMENT_FILE)
                {
                    lvwDockTypes.Items[4].SubItems[2].Text = Convert.ToString((Convert.ToInt32(lvwDockTypes.Items[4].SubItems[2].Text) + 1));
                }
                if (docType == ihConstants.FAMILYHISTORY_FILE)
                {
                    lvwDockTypes.Items[5].SubItems[2].Text = Convert.ToString((Convert.ToInt32(lvwDockTypes.Items[5].SubItems[2].Text) + 1));
                }
                if (docType == ihConstants.CONSENTFORTREATMENT_FILE)
                {
                    lvwDockTypes.Items[6].SubItems[2].Text = Convert.ToString((Convert.ToInt32(lvwDockTypes.Items[6].SubItems[2].Text) + 1));
                }
                if (docType == ihConstants.PHYSICALEXAMINATION_FILE)
                {
                    lvwDockTypes.Items[7].SubItems[2].Text = Convert.ToString((Convert.ToInt32(lvwDockTypes.Items[7].SubItems[2].Text) + 1));
                }
                if (docType == ihConstants.MEDICALHISTORY_FILE)
                {
                    lvwDockTypes.Items[8].SubItems[2].Text = Convert.ToString((Convert.ToInt32(lvwDockTypes.Items[8].SubItems[2].Text) + 1));
                }
                if (docType == ihConstants.TENTATIVEHISTORY_FILE)
                {
                    lvwDockTypes.Items[9].SubItems[2].Text = Convert.ToString((Convert.ToInt32(lvwDockTypes.Items[9].SubItems[2].Text) + 1));
                }
                if (docType == ihConstants.MEDICALDIAGNOSIS_FILE)
                {
                    lvwDockTypes.Items[10].SubItems[2].Text = Convert.ToString((Convert.ToInt32(lvwDockTypes.Items[10].SubItems[2].Text) + 1));
                }
                if (docType == ihConstants.CAREGIVEN_FILE)
                {
                    lvwDockTypes.Items[11].SubItems[2].Text = Convert.ToString((Convert.ToInt32(lvwDockTypes.Items[11].SubItems[2].Text) + 1));
                }
                if (docType == ihConstants.TRAETMENTGIVEN_FILE)
                {
                    lvwDockTypes.Items[12].SubItems[2].Text = Convert.ToString((Convert.ToInt32(lvwDockTypes.Items[12].SubItems[2].Text) + 1));
                }
                if (docType == ihConstants.THERATPEUTICORDER_FILE)
                {
                    lvwDockTypes.Items[13].SubItems[2].Text = Convert.ToString((Convert.ToInt32(lvwDockTypes.Items[13].SubItems[2].Text) + 1));
                }
                if (docType == ihConstants.PROGRESSNOTES_FILE)
                {
                    lvwDockTypes.Items[14].SubItems[2].Text = Convert.ToString((Convert.ToInt32(lvwDockTypes.Items[14].SubItems[2].Text) + 1));
                }
                if (docType == ihConstants.OPERATIVEPROCEDURE_FILE)
                {
                    lvwDockTypes.Items[15].SubItems[2].Text = Convert.ToString((Convert.ToInt32(lvwDockTypes.Items[15].SubItems[2].Text) + 1));
                }
                if (docType == ihConstants.DISCHARGEREPORT_FILE)
                {
                    lvwDockTypes.Items[16].SubItems[2].Text = Convert.ToString((Convert.ToInt32(lvwDockTypes.Items[16].SubItems[2].Text) + 1));
                }
                if (docType == ihConstants.DEATHREPORT_FILE)
                {
                    lvwDockTypes.Items[17].SubItems[2].Text = Convert.ToString((Convert.ToInt32(lvwDockTypes.Items[17].SubItems[2].Text) + 1));
                }
                if (docType == ihConstants.MEDICALREPORT_FILE)
                {
                    lvwDockTypes.Items[18].SubItems[2].Text = Convert.ToString((Convert.ToInt32(lvwDockTypes.Items[18].SubItems[2].Text) + 1));
                }
                if (docType == ihConstants.OTHERS_FILE)
                {
                    lvwDockTypes.Items[19].SubItems[2].Text = Convert.ToString((Convert.ToInt32(lvwDockTypes.Items[19].SubItems[2].Text) + 1));
                }
            }
            //imageDelLst = (ListBox)BoxDtls.Controls["lstImageDel"];
            //lvwDockTypes.Items[15].SubItems[2].Text =Convert.ToString(imageDelLst.Items.Count);
        }
        public int GetPolicyPhotoStatus()
        {
            string sqlStr = null;

            DataSet policyDs = new DataSet();

            try
            {
                sqlStr = "select photo from metadata_entry where proj_code=" + ctrlPolicy.ProjectKey + " and bundle_key=" + ctrlPolicy.BatchKey + "  and patient_id='" + ctrlPolicy.PolicyNumber + "'";
                sqlAdap = new OdbcDataAdapter(sqlStr, sqlCon);
                sqlAdap.Fill(policyDs);
            }
            catch (Exception ex)
            {
                sqlAdap.Dispose();
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes(sqlStr + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex);
            }

            return Convert.ToInt32(policyDs.Tables[0].Rows[0]["photo"].ToString());
        }
        void ShowImage(bool prmOverWrite)
        {

            string policyName;
            string changedImageName = string.Empty;
            wfeImage wImage = null;
            string photoImageName = null;

            int pos;
            //((ListBox)BoxDtls.Controls["lstPolicy"]).GetItemText();
            try
            {
                policyLst = lstPolicy;
                imageLst = lstImage;
                policyName = policyLst.SelectedItem.ToString();
                if (imageLst.SelectedIndex >= 0)
                {
                    changedImageName = imageLst.SelectedItem.ToString();

                    pos = imageLst.SelectedItem.ToString().IndexOf("-");

                    if (pos > 0)
                    {
                        changedImageName = imageLst.SelectedItem.ToString().Substring(0, pos);
                        pImage = new CtrlImage(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), boxNumber, policyLst.SelectedItem.ToString(), changedImageName, string.Empty);
                        wImage = new wfeImage(sqlCon, pImage);
                        changedImageName = wImage.GetIndexedImageName();
                    }
                    ctrlPolicy = new CtrlPolicy(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), boxNumber, policyLst.SelectedItem.ToString());
                    policy = new wfePolicy(sqlCon, ctrlPolicy);
                    if (GetPolicyPhotoStatus() == ihConstants._POLICY_CONTAINS_PHOTO)
                    {
                        hasPhotoBol = true;
                    }
                    else
                    {
                        hasPhotoBol = false;
                    }
                    //policyData=(udtPolicy)policy.LoadValuesFromDB();
                    policyPath = GetPolicyPath(); //policyData.policy_path;
                    fileMove = new FileorFolder();
                    string sourcePath = policyPath + "\\" + ihConstants._INDEXING_FOLDER;
                    string destPath = policyPath + "\\" + ihConstants._FQC_FOLDER;
                    sourceFilePath = sourcePath;
                    indexFolderName = destPath;
                    if (Directory.Exists(destPath) == false)
                    {
                        //Directory.CreateDirectory(destPath);
                        //FileorFolder.MoveFiles(sourcePath, destPath);
                        FileorFolder.RenameFolder(sourcePath, destPath);
                    }
                    if (pos <= 0)
                    {
                        //fileMove.MoveFile(sourcePath,destPath,changedImageName,prmOverWrite);
                    }
                    //prmButtonRescan.Enabled = true;
                    //prmButtonSkewRight.Enabled = true;
                    imgFileName = destPath + "\\" + changedImageName;
                    if (hasPhotoBol == true)
                    {
                        if ((changedImageName.Substring(policyLen, 6) == "_000_A") && (pos <= 0))
                        {
                            //Open the source file
                            img.LoadBitmapFromFile(imgFileName);
                            //Show the image back in picture box
                            //pictureControl.Image = img.GetBitmap();
                            prmButtonRescan.Enabled = false;
                            prmButtonSkewRight.Enabled = false;
                        }
                        else if ((changedImageName.Substring(policyLen, 6) == "_000_A") && (pos > 0))
                        {
                            photoImageName = imageLst.SelectedItem.ToString().Substring(0, pos);
                            pImage = new CtrlImage(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), boxNumber, policyLst.SelectedItem.ToString(), photoImageName , string.Empty);
                            wImage = new wfeImage(sqlCon, pImage);
                            changedImageName = wImage.GetIndexedImageName();
                            if (pos > 0)
                            {
                                //fileMove.MoveFile(sourcePath,destPath,changedImageName,prmOverWrite);
                            }
                            imgFileName = destPath + "\\" + changedImageName;
                            //Open the source file
                            img.LoadBitmapFromFile(imgFileName);
                            //Show the image back in picture box
                            //pictureControl.Image = img.GetBitmap();
                            prmButtonRescan.Enabled = false;
                            prmButtonSkewRight.Enabled = false;
                        }
                        else
                        {
                            if (pos > 0)
                            {
                                photoImageName = imageLst.SelectedItem.ToString().Substring(0, pos);
                                pImage = new CtrlImage(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), boxNumber, policyLst.SelectedItem.ToString(), photoImageName, string.Empty);
                                wImage = new wfeImage(sqlCon, pImage);
                                changedImageName = wImage.GetIndexedImageName();
                                if (pos > 0)
                                {
                                    //fileMove.MoveFile(sourcePath,destPath,changedImageName,prmOverWrite);
                                }
                                imgFileName = destPath + "\\" + changedImageName;
                            }
                            //Open the source file
                            img.LoadBitmapFromFile(imgFileName);
                            //Show the image back in picture box
                            //pictureControl.Image = img.GetBitmap();
                        }
                    }
                    else
                    {
                        if (pos > 0)
                        {
                            photoImageName = imageLst.SelectedItem.ToString().Substring(0, pos);
                            //fileMove.MoveFile(sourcePath,destPath,changedImageName,prmOverWrite);
                            pImage = new CtrlImage(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), boxNumber, policyLst.SelectedItem.ToString(), photoImageName, string.Empty);
                            wImage = new wfeImage(sqlCon, pImage);
                            changedImageName = wImage.GetIndexedImageName();
                        }
                        imgFileName = destPath + "\\" + changedImageName;
                        //Open the source file
                        img.LoadBitmapFromFile(imgFileName);
                        //Show the image back in picture box
                        //pictureControl.Image = img.GetBitmap();
                    }
                    pictureControl.Refresh();
                    System.IO.FileInfo info = new System.IO.FileInfo(imgFileName);
                    long fileSize = info.Length;
                    fileSize = fileSize / 1024;
                    //lblImageSize = (Label)BoxDtls.Controls["lblImageSize"];
                    lblImgSize.Text = fileSize.ToString() + " KB";
                }
                ChangeSize();
            }

            catch (Exception ex)
            {

                MessageBox.Show("Error while showing the image", "Image error");
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes("Project-" + wBox.ctrlBox.ProjectCode + " ,Batch-" + wBox.ctrlBox.BatchKey + " ,Box-" + wBox.ctrlBox.BoxNumber + "Image name-" + imgFileName + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex);
            }
        }
        public bool DeleteNotify(int currIndex)
        {
            lstImage.Items.RemoveAt(currIndex);
            //PopulateImageList((int)lstPolicy.SelectedItem);
            if (lstImage.Items.Count > 0)
            {
                if (currIndex != lstImage.Items.Count)
                {
                    lstImage.SelectedIndex = currIndex;
                    indexCount = lstImage.SelectedIndex;
                }
                else
                {
                    if ((lstPolicy.SelectedIndex) != (lstPolicy.Items.Count - 1))
                    {
                        lstPolicy.SelectedIndex = lstPolicy.SelectedIndex + 1;
                    }
                    else
                    {
                        lstImage.SelectedIndex = currIndex - 1;
                    }
                }
            }
            else
            {
                if ((lstPolicy.SelectedIndex) != (lstPolicy.Items.Count - 1))
                {
                    lstPolicy.SelectedIndex = lstPolicy.SelectedIndex + 1;
                }
            }
            PopulateDelList(lstPolicy.SelectedItem.ToString());
            //			//MoveNext();
            //			lstImage.Refresh();
            //			lstImageDel.Refresh();
            return true;
        }
        public void PopulateDelList(string prmPolicyNo)
        {
            lstImageDel.Items.Clear();
            CtrlPolicy ctrlPolicy = new CtrlPolicy(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), "1", prmPolicyNo);
            wItem policy = new wfePolicy(sqlCon, ctrlPolicy);
            //ListViewItem lstView;
            ArrayList arrImage = new ArrayList();
            wfeImage wImage = new wfeImage(sqlCon);
            //eSTATES[] policyState=new eSTATES[1];
            eSTATES[] imageState = new eSTATES[1];
            //            policyState[0]=currState;
            imageState[0] = eSTATES.PAGE_DELETED;
            //state[1]=eSTATES.POLICY_SCANNED;
            CtrlImage ctrlImage;
            arrImage = wImage.GetDeletedPageList3(imageState, policy);
            for (int i = 0; i < arrImage.Count; i++)
            {
                ctrlImage = (CtrlImage)arrImage[i];
                //lstView=lstImage.Items.Add(ctrlImage.ImageName);
                lstImageDel.Items.Add(ctrlImage.ImageName);
            }
        }
        int ImageCopy1()
        {
            DialogResult dlg;
            string path = string.Empty;
            string changedImageName = string.Empty;
            string scanImageName = string.Empty;
            wfeImage wImage = null;
            int pos;
            string fPath = string.Empty;

            policyLst = lstPolicy;
            imageLst = lstImage;

            if (imageLst.SelectedIndex >= 0)
            {
                changedImageName = imageLst.SelectedItem.ToString();
                scanImageName = changedImageName;
                pos = imageLst.SelectedItem.ToString().IndexOf("-");

                if (pos > 0)
                {
                    changedImageName = imageLst.SelectedItem.ToString().Substring(0, pos);
                    scanImageName = changedImageName;
                    pImage = new CtrlImage(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), boxNumber, policyLst.SelectedItem.ToString(), changedImageName, string.Empty);
                    wImage = new wfeImage(sqlCon, pImage);
                    changedImageName = wImage.GetIndexedImageName();
                }
            }
            dlg = DialogResult.Yes; //MessageBox.Show(this, "Do you want to copy it from scan folder?", "Copy", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dlg == DialogResult.Yes)
            {
                path = policyPath + "\\" + ihConstants._SCAN_FOLDER;
                fPath = path + "\\" + scanImageName;
            }
            //else
            //{
            //    path = policyPath + "\\" + ihConstants._INDEXING_FOLDER;
            //    fPath = path + "\\" + changedImageName;
            //}

            if (File.Exists(fPath))
            {
                ShowImage(true, path);
                //File.Copy(fPath, imgFileName);
                ChangeSize(fPath);
            }
            else
            {
                MessageBox.Show("This image is not present in the folder......");
            }
            return 0;
        }
        int ImageCopy()
        {
            string path;
            string changedImageName = string.Empty;
            string scanImageName = string.Empty;
            wfeImage wImage = null;
            int pos;
            string sPath;
            string iPath;
            policyLst = lstPolicy;
            imageLst = lstImage;

            if (imageLst.SelectedIndex >= 0)
            {
                changedImageName = imageLst.SelectedItem.ToString();
                scanImageName = changedImageName;
                pos = imageLst.SelectedItem.ToString().IndexOf("-");

                if (pos > 0)
                {
                    changedImageName = imageLst.SelectedItem.ToString().Substring(0, pos);
                    scanImageName = changedImageName;
                    pImage = new CtrlImage(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), boxNumber, policyLst.SelectedItem.ToString(), changedImageName, string.Empty);
                    wImage = new wfeImage(sqlCon, pImage);
                    changedImageName = wImage.GetIndexedImageName();
                }
            }

            path = policyPath + "\\" + ihConstants._SCAN_FOLDER;
            sPath = path + "\\" + scanImageName;

            path = policyPath + "\\" + ihConstants._INDEXING_FOLDER;
            iPath = path + "\\" + changedImageName;
            if (File.Exists(sPath))
            {
                File.Copy(sPath, iPath, true);
                img.LoadBitmapFromFile(iPath);
                ChangeSize(iPath);
            }
            else
            {
                MessageBox.Show("This image is not present in the folder......");
            }
            return 0;
        }
        void ShowImage(bool prmOverWrite, string pFPath)
        {

            string policyName;
            string changedImageName = string.Empty;
            wfeImage wImage = null;
            string photoImageName = null;

            int pos;
            //((ListBox)BoxDtls.Controls["lstPolicy"]).GetItemText();
            try
            {
                policyLst = lstPolicy;
                imageLst = lstImage;
                policyName = policyLst.SelectedItem.ToString();
                if (imageLst.SelectedIndex >= 0)
                {
                    changedImageName = imageLst.SelectedItem.ToString();

                    pos = imageLst.SelectedItem.ToString().IndexOf("-");

                    if (pos > 0)
                    {
                        changedImageName = imageLst.SelectedItem.ToString().Substring(0, pos);
                        pImage = new CtrlImage(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), boxNumber, policyLst.SelectedItem.ToString(), changedImageName, string.Empty);
                        wImage = new wfeImage(sqlCon, pImage);
                        changedImageName = wImage.GetIndexedImageName();
                    }
                    ctrlPolicy = new CtrlPolicy(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), boxNumber, policyLst.SelectedItem.ToString());
                    policy = new wfePolicy(sqlCon, ctrlPolicy);
                    if (GetPolicyPhotoStatus() == ihConstants._POLICY_CONTAINS_PHOTO)
                    {
                        hasPhotoBol = true;
                    }
                    else
                    {
                        hasPhotoBol = false;
                    }
                    //policyData = (udtPolicy)policy.LoadValuesFromDB();
                    policyPath = GetPolicyPath(); //policyData.policy_path;
                    fileMove = new FileorFolder();
                    string sourcePath = pFPath;
                    string destPath = policyPath + "\\" + ihConstants._FQC_FOLDER;
                    sourceFilePath = sourcePath;
                    indexFolderName = destPath;
                    //if (Directory.Exists(destPath) == false)
                    //{
                    //    Directory.CreateDirectory(destPath);
                    //    FileorFolder.MoveFiles(sourcePath, destPath);
                    //}

                    //if (pos <= 0)
                    //{
                    //    fileMove.MoveFile(sourcePath, destPath, changedImageName, prmOverWrite);
                    //}
                    //prmButtonRescan.Enabled = true;
                    //prmButtonSkewRight.Enabled = true;
                    if (Directory.Exists(destPath) == false)
                    {
                        //Directory.CreateDirectory(destPath);
                        //FileorFolder.MoveFiles(sourcePath, destPath);
                        FileorFolder.RenameFolder(sourcePath, destPath);
                    }
                    imgFileName = destPath + "\\" + changedImageName;
                    if (hasPhotoBol == true)
                    {
                        if ((changedImageName.Substring(policyLen, 6) == "_000_A") && (pos <= 0))
                        {
                            //Open the source file
                            img.LoadBitmapFromFile(imgFileName);
                            //Show the image back in picture box
                            //pictureControl.Image = img.GetBitmap();
                            prmButtonRescan.Enabled = false;
                            prmButtonSkewRight.Enabled = false;
                        }
                        else if ((changedImageName.Substring(policyLen, 6) == "_000_A") && (pos > 0))
                        {
                            photoImageName = imageLst.SelectedItem.ToString().Substring(0, pos);
                            pImage = new CtrlImage(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey),boxNumber, policyLst.SelectedItem.ToString(), photoImageName, string.Empty);
                            wImage = new wfeImage(sqlCon, pImage);
                            changedImageName = wImage.GetIndexedImageName();
                            if (pos > 0)
                            {
                                //fileMove.MoveFile(sourcePath, destPath, changedImageName, prmOverWrite);
                            }
                            imgFileName = destPath + "\\" + changedImageName;
                            //Open the source file
                            img.LoadBitmapFromFile(imgFileName);
                            //Show the image back in picture box
                            //pictureControl.Image = img.GetBitmap();
                            prmButtonRescan.Enabled = false;
                            prmButtonSkewRight.Enabled = false;
                        }
                        else
                        {
                            if (pos > 0)
                            {
                                photoImageName = imageLst.SelectedItem.ToString().Substring(0, pos);
                                pImage = new CtrlImage(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), boxNumber, policyLst.SelectedItem.ToString(), photoImageName, string.Empty);
                                wImage = new wfeImage(sqlCon, pImage);
                                changedImageName = wImage.GetIndexedImageName();
                                if (pos > 0)
                                {
                                    //fileMove.MoveFile(sourcePath, destPath, changedImageName, prmOverWrite);
                                }
                                imgFileName = destPath + "\\" + changedImageName;
                            }
                            //Open the source file
                            img.LoadBitmapFromFile(imgFileName);
                            //Show the image back in picture box
                            //pictureControl.Image = img.GetBitmap();
                        }
                    }
                    else
                    {
                        if (pos > 0)
                        {
                            photoImageName = imageLst.SelectedItem.ToString().Substring(0, pos);
                            fileMove.MoveFile(sourcePath, destPath, changedImageName, prmOverWrite);
                            pImage = new CtrlImage(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), boxNumber, policyLst.SelectedItem.ToString(), photoImageName, string.Empty);
                            wImage = new wfeImage(sqlCon, pImage);
                            changedImageName = wImage.GetIndexedImageName();
                        }
                        imgFileName = destPath + "\\" + changedImageName;
                        //Open the source file
                        img.LoadBitmapFromFile(imgFileName);
                        //Show the image back in picture box
                        //pictureControl.Image = img.GetBitmap();
                    }
                    pictureControl.Refresh();
                    System.IO.FileInfo info = new System.IO.FileInfo(imgFileName);
                    long fileSize = info.Length;
                    fileSize = fileSize / 1024;
                    //lblImageSize = (Label)BoxDtls.Controls["lblImageSize"];
                    lblImgSize.Text = fileSize.ToString() + " KB";
                }
                ChangeSize();
            }

            catch (Exception ex)
            {

                MessageBox.Show("Error while showing the image", "Image error");
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes("Project-" + wBox.ctrlBox.ProjectCode + " ,Batch-" + wBox.ctrlBox.BatchKey + " ,Box-" + wBox.ctrlBox.BoxNumber + "Image name-" + imgFileName + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex);
            }
        }
        int AutoCrop()
        {
            try
            {
                if (img.IsValid() == true)
                {
                    //Auto Crop
                    img.AutoCrop();
                    //Call the save routine
                    img.SaveFile(imgFileName);
                    //Show the image back in picture box
                    img.LoadBitmapFromFile(imgFileName);
                    ChangeSize();
                }
                //ChangeSize();
                System.IO.FileInfo info = new System.IO.FileInfo(imgFileName);
                fileSize = info.Length;
                fileSize = fileSize / 1024;
                //lblImageSize = (Label)BoxDtls.Controls["lblImageSize"];
                lblImgSize.Text = fileSize.ToString() + " KB";
                UpdateImageSize(fileSize);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while auto cropping the image", "Auto Crop Error");
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes("Project-" + projKey + " ,Bundle-" + bundleKey + " ,Box-" + boxNumber + "Image name-" + imgFileName + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex);
                error = ex.Message;
            }
            return 0;
        }
        int RotateLeft()
        {
            long fileSize;

            OperationInProgress = ihConstants._OTHER_OPERATION;

            try
            {
                if (img.IsValid() == true)
                {
                    //Rotate right -90
                    img.RotateLeft();
                    //Call the save routine
                    img.SaveFile(imgFileName);
                    //Show the image back in picture box
                    img.LoadBitmapFromFile(imgFileName);
                    ChangeSize();
                    //delinsrtBol = false;
                    System.IO.FileInfo info = new System.IO.FileInfo(imgFileName);
                    fileSize = info.Length;
                    fileSize = fileSize / 1024;
                    UpdateImageSize(fileSize);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while rotate the image" + ex.Message, "Rotation Error");
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes("Project-" + projKey + " ,Batch-" +bundleKey + " ,Box-" + boxNumber + "Image name-" + imgFileName + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex);
            }
            //imgQc.Write(imgFileName);

            return 0;

        }	
        int CropRegister()
        {
            OperationInProgress = ihConstants._CROP;
            return 0;
        }
        int CleanImageRegister()
        {
            OperationInProgress = ihConstants._CLEAN;
            //ChangeSize();
            return 0;
        }
        int ZoomIn()
        {
            try
            {
                if (img.IsValid() == true)
                {
                    pictureControl.Dock = DockStyle.None;
                    //OperationInProgress = ihConstants._OTHER_OPERATION;
                    keyPressed = keyPressed + 1;
                    zoomHeight = Convert.ToInt32(img.GetBitmap().Height * (1.2));
                    zoomWidth = Convert.ToInt32(img.GetBitmap().Width * (1.2));
                    zoomSize.Height = zoomHeight;
                    zoomSize.Width = zoomWidth;

                    pictureControl.Width = Convert.ToInt32(Convert.ToDouble(pictureControl.Width) * 1.2);
                    pictureControl.Height = Convert.ToInt32(Convert.ToDouble(pictureControl.Height) * 1.2);
                    pictureControl.Refresh();
                    ChangeZoomSize();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while auto cropping " + ex.Message, "Auto Crop Error");

                error = ex.Message;
            }
            return 0;
        }
        private void ChangeZoomSize()
        {
            if (!System.IO.File.Exists(imgFileName)) return;
            Image newImage = Image.FromFile(imgFileName);
            double scaleX = (double)pictureControl.Width / (double)newImage.Width;
            double scaleY = (double)pictureControl.Height / (double)newImage.Height;
            double Scale = Math.Min(scaleX, scaleY);
            int w = (int)(newImage.Width * Scale);
            int h = (int)(newImage.Height * Scale);
            //pictureControl.Width = w;
            //pictureControl.Height = h;
            pictureControl.Image = newImage.GetThumbnailImage(w, h, new System.Drawing.Image.GetThumbnailImageAbort(GetThumbnailImageAbort), IntPtr.Zero);
            newImage.Dispose();
        }
        int ZoomOut()
        {
            try
            {
                if (keyPressed > 0)
                {
                    pictureControl.Dock = DockStyle.None;
                    //OperationInProgress = ihConstants._OTHER_OPERATION;
                    keyPressed = keyPressed + 1;
                    zoomHeight = Convert.ToInt32(img.GetBitmap().Height / (1.2));
                    zoomWidth = Convert.ToInt32(img.GetBitmap().Width / (1.2));
                    zoomSize.Height = zoomHeight;
                    zoomSize.Width = zoomWidth;

                    pictureControl.Width = Convert.ToInt32(Convert.ToDouble(pictureControl.Width) / 1.2);
                    pictureControl.Height = Convert.ToInt32(Convert.ToDouble(pictureControl.Height) / 1.2);
                    pictureControl.Refresh();
                    ChangeZoomSize();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while auto cropping " + ex.Message, "Auto Crop Error");
                error = ex.Message;
            }
            return 0;
        }

        int RotateRight()
        {
            long fileSize;

            OperationInProgress = ihConstants._OTHER_OPERATION;

            try
            {
                if (img.IsValid() == true)
                {
                    //Rotate right +90
                    img.RotateRight();
                    //Call the save routine
                    img.SaveFile(imgFileName);
                    //Show the image back in picture box
                    img.LoadBitmapFromFile(imgFileName);
                    ChangeSize();

                    System.IO.FileInfo info = new System.IO.FileInfo(imgFileName);
                    fileSize = info.Length;
                    fileSize = fileSize / 1024;
                    //delinsrtBol = false;
                    UpdateImageSize(fileSize);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while rotate the image" + ex.Message, "Rotation Error");
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes("Project-" + projKey + " ,BUndle-" + bundleKey + " ,Box-" + boxNumber + "Image name-" + imgFileName + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex);
            }
            return 0;
        }
        void UpdateImageSize(long prmSize)
        {
            //string photoName;
            wfeImage img;
            //long fileSize;

            policyLst = lstPolicy;
            imageLst = lstImage;

            pImage = new CtrlImage(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), boxNumber, policyLst.SelectedItem.ToString(), imageLst.SelectedItem.ToString(), string.Empty);
            img = new wfeImage(sqlCon, pImage);
            img.UpdateImageSize(crd, eSTATES.PAGE_QC, prmSize);
        }
        int SkewRight()
        {
            long fileSize;
            OperationInProgress = ihConstants._OTHER_OPERATION;
            try
            {
                if (img.IsValid() == true)
                {
                    //Auto Deskew
                    img.AutoDeSkew();
                    //Call the save routine
                    img.SaveFile(imgFileName);
                    //Show the image back in picture box
                    img.LoadBitmapFromFile(imgFileName);
                    ChangeSize();
                    System.IO.FileInfo info = new System.IO.FileInfo(imgFileName);
                    fileSize = info.Length;
                    fileSize = fileSize / 1024;
                    //lblImageSize = (Label)BoxDtls.Controls["lblImageSize"];
                    lblImgSize.Text = fileSize.ToString() + " KB";
                    UpdateImageSize(fileSize);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while rotate the image" + ex.Message, "Rotation Error");
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes("Project-" + projKey + " ,Bundle-" + bundleKey + " ,Box-" + boxNumber + "Image name-" + imgFileName + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex);
            }
            return 0;
        }

        int SkewLeft()
        {
            OperationInProgress = ihConstants._OTHER_OPERATION;

            //            //rotateAngle = rotateAngle + constRotateAngle;
            //            imgQc = objQc.Skew(imgQc,(-skewXAngle),(-skewYAngle));
            //            pictureControl.Image = MagickNet.Image.ToBitmap(imgQc);
            //            pictureControl.Refresh();
            //imgQc.Write(imgFileName);

            return 0;
        }

        int NoiseRemove()
        {
            long fileSize;
            ComboBox noiseVal = cmbDesValue;

            try
            {
                if (img.IsValid() == true)
                {
                    //Auto Deskew
                    //img.Despeckle();
                    img.Despeckle(Convert.ToInt32(Convert.ToInt32(noiseVal.Text)));
                    //Call the save routine
                    img.SaveFile(imgFileName);
                    //Show the image back in picture box
                    img.LoadBitmapFromFile(imgFileName);
                    ChangeSize();
                    System.IO.FileInfo info = new System.IO.FileInfo(imgFileName);
                    fileSize = info.Length;
                    fileSize = fileSize / 1024;
                    //lblImageSize = lblImgSize;
                    lblImgSize.Text = fileSize.ToString() + " KB";
                    UpdateImageSize(fileSize);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while rotating the image" + ex.Message, "Rotation Error");
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes("Project-" + projKey + " ,Bundle-" + bundleKey + " ,Box-" + boxNumber + "Image name-" + imgFileName + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex);
            }
            return 0;
        }

        private void lstPolicy_SelectedValueChanged(object sender, EventArgs e)
        {

        }
        void EnableDisbleControls(bool prmControl)
        {
            prmButtonCrop.Enabled = prmControl;
            prmButtonAutoCrp.Enabled = prmControl;
            prmButtonRotateRight.Enabled = prmControl;
            prmButtonRotateLeft.Enabled = prmControl;
            prmButtonZoomIn.Enabled = prmControl;
            prmButtonZoomOut.Enabled = prmControl;
            prmButtonSkewRight.Enabled = prmControl;
            //prmButtonSkewLeft.Enabled = prmControl;
            prmButtonNoiseRemove.Enabled = prmControl;
            prmButtonCleanImg.Enabled = prmControl;
            prmButtonCopyImage.Enabled = prmControl;
            prmButtonDelImage.Enabled = prmControl;
            prmButtonRescan.Enabled = prmControl;
            prmButtonScan.Enabled = prmControl;
            //prmButtonCopyTo.Enabled = prmControl;
            prmButtonMoveTo.Enabled = prmControl;
            prmButtonCopyImageTo.Enabled = prmControl;
        }

        private void ShowPolicyDetails(string file_no)
        {
            DataTable dsVol = new DataTable();

            dsVol = getFileDetails(projKey, bundleKey, file_no);
            if (dsVol.Rows.Count > 0)
            {
                label12.Text = "Name : " + dsVol.Rows[0][0].ToString();
                label13.Text = "Gender : " + dsVol.Rows[0][1].ToString();
                Txtfirstparty.Text = dsVol.Rows[0][2].ToString();
                txtsecondparty.Text = dsVol.Rows[0][3].ToString();
                textBox4.Text = dsVol.Rows[0][4].ToString();
                textBox1.Text = dsVol.Rows[0][5].ToString();
                textBox2.Text = dsVol.Rows[0][6].ToString();
                textBox3.Text = dsVol.Rows[0][7].ToString();
            }
        }
        public DataTable getFileDetails(string projKey, string bundleKey, string file_no)
        {
            DataTable dt = new DataTable();

            string sql = "select patient_name, gender,reg_number,guardian_name, dep_name,admission_date,death_date,discharge_date from metadata_entry where proj_code = '" + projKey + "' and bundle_key = '" + bundleKey + "' and patient_id = '" + file_no + "'";

            OdbcDataAdapter odap = new OdbcDataAdapter(sql, sqlCon);
            odap.Fill(dt);

            return dt;
        }
        void CreateAllFolders()
        {
            policyLst = lstPolicy;
            ctrlPolicy = new CtrlPolicy(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), boxNumber, policyLst.SelectedItem.ToString());
            policy = new wfePolicy(sqlCon, ctrlPolicy);
            if (GetPolicyStatus() != (int)eSTATES.POLICY_ON_HOLD)
            {
                //policyData = (udtPolicy)policy.LoadValuesFromDB();
                policyPath = GetPolicyPath();
                indexFolderName = policyPath + "\\" + ihConstants._FQC_FOLDER;
                sourceFilePath = policyPath + "\\" + ihConstants._QC_FOLDER;
                scanFolder = policyPath + "\\" + ihConstants._SCAN_FOLDER;
                qcFolder = policyPath + "\\" + ihConstants._QC_FOLDER;
                if (Directory.Exists(policyPath) == false)
                {
                    Directory.CreateDirectory(policyPath);
                    if (!Directory.Exists(scanFolder))
                    {
                        Directory.CreateDirectory(scanFolder);
                    }
                    if (Directory.Exists(indexFolderName) == false)
                    {
                        Directory.CreateDirectory(indexFolderName);
                    }
                }
                else
                {
                    if (!Directory.Exists(scanFolder))
                    {
                        Directory.CreateDirectory(scanFolder);
                    }
                    if (Directory.Exists(indexFolderName) == false)
                    {
                        Directory.CreateDirectory(indexFolderName);
                    }
                }
            }
        }
        public int GetPolicyStatus()
        {
            string sqlStr = null;
            DataSet dsImage = new DataSet();
            OdbcDataAdapter sqlAdap = null;

            sqlStr = "select status from metadata_entry " +
                    " where proj_code=" + ctrlPolicy.ProjectKey +
                " and bundle_key=" + ctrlPolicy.BatchKey + " and patient_id='" + ctrlPolicy.PolicyNumber + "'";

            try
            {
                sqlAdap = new OdbcDataAdapter(sqlStr, sqlCon);
                sqlAdap.Fill(dsImage);
            }
            catch (Exception ex)
            {
                sqlAdap.Dispose();
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes(sqlStr + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex);
            }
            return Convert.ToInt32(dsImage.Tables[0].Rows[0]["status"]);
        }
        private void PopulateImageList(string prmPolicyNo)
        {
            lstImage.Items.Clear();
            CtrlPolicy ctrlPolicy = new CtrlPolicy(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), "1", prmPolicyNo);
            wItem policy = new wfePolicy(sqlCon, ctrlPolicy);
            //ListViewItem lstView;
            ArrayList arrImage = new ArrayList();
            wQuery pQuery = new ihwQuery(sqlCon);
            CtrlImage ctrlImage;
            arrImage = GetImagesItems(eITEMS.PAGE, prmPolicyNo);
            for (int i = 0; i < arrImage.Count; i++)
            {
                ctrlImage = (CtrlImage)arrImage[i];
                //lstView=lstImage.Items.Add(ctrlImage.ImageName);

                if (ctrlImage.DocType != string.Empty)
                {
                    //lstImage.Items.Add(ctrlImage.ImageName);
                    lstImage.Items.Add(ctrlImage.ImageName + "-" + ctrlImage.DocType);
                }
                else
                    lstImage.Items.Add(ctrlImage.ImageName);
            }
        }

        public ArrayList GetImagesItems(eITEMS item, string case_file_no)
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
                strQuery = "select distinct A.proj_key,A.batch_key,A.box_number,A.policy_number,A.page_name,A.page_index_name,A.doc_type from image_master A,metadata_entry B where A.proj_key=B.proj_code and A.batch_key = B.bundle_key  and A.policy_number = B.patient_id and A.photo <> 1 and A.proj_key=" + projKey + " and A.batch_key=" + bundleKey + " and  A.policy_number='" + case_file_no + "' and a.status <> 29 and (b.status = 3 or b.status = 4 or b.status = 5 or b.status = 6 or B.status ='7' or B.status = '8' or b.status = '30' or b.status = '31' or b.status = '37' or b.status = '40' or b.status = '77') order by a.serial_no";

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

            return arrItem;
        }

        [Category("Action")]
        [Description("Fires when the Policy is changed.")]
        public event PolicyChangeHandler PolicyChanged;

        private void lstPolicy_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            NovaNet.wfe.eSTATES[] state = new NovaNet.wfe.eSTATES[1];
            DateTime stdt = new DateTime();
            DateTime enddt = new DateTime();
            stdt = DateTime.Now;
            PopulateImageList(lstPolicy.SelectedItem.ToString());
            wQuery pQuery = new ihwQuery(sqlCon);
            DataSet dsVol = new DataSet();
            CtrlPolicy ctrPol;
            if (lstImage.Items.Count > 0)
            {
                lstImage.SelectedIndex = 0;
            }
            if (PolicyChanged != null)
            {
                PolicyChanged(this, e);
                ShowPolicyDetails(lstPolicy.SelectedItem.ToString());
                //GetIndexDetails(lstPolicy.SelectedItem.ToString());
            }
            else
            {
                ShowPolicyDetails(lstPolicy.SelectedItem.ToString());
            }

            if (policyLst.SelectedItem != null)
            {
                ShowAllException();
                if (tabControl1.SelectedIndex == 1)
                {
                    PopulateListView();
                }
                string policy_no = policyLst.SelectedItem.ToString();
                //string deed_no = policy_no.Split(new char[] { '[', ']' })[1];
                //txtOldDeedno.Text = deed_no;
                lnkPage1.Visible = true;
                lnkPage2.Visible = true;
                lnkPage3.Visible = true;
                EnableDisbleControls(true);
                try
                {
                    ctrlPolicy = new CtrlPolicy(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), boxNumber, policyLst.SelectedItem.ToString());
                    policy = new wfePolicy(sqlCon, ctrlPolicy);
                    if (GetPolicyStatus() != (int)eSTATES.POLICY_ON_HOLD)
                    {
                        EnableDisbleControls(true);
                        //policyData = (udtPolicy)policy.LoadValuesFromDB();

                        indexFolderName = GetPolicyPath() + "\\" + ihConstants._FQC_FOLDER;
                        sourceFilePath = GetPolicyPath() + "\\" + ihConstants._QC_FOLDER;
                        scanFolder = GetPolicyPath() + "\\" + ihConstants._SCAN_FOLDER;
                        qcFolder = GetPolicyPath() + "\\" + ihConstants._QC_FOLDER;
                        imageLst = lstImage;
                        if (imageLst.Items.Count == 0)
                        {
                            pictureControl.Image = null;
                        }
                        ShowPolicyDetails(policyLst.SelectedItem.ToString());

                        //DataSet ds = policy.GetPolicyDetails();
                        //Label lblName = lblName;
                        //if (ds.Tables[0].Rows.Count > 0)
                        lblName.Text = "Name: " + txtName.Text; //ds.Tables[0].Rows[0]["name_of_policyholder"].ToString();              
                        policyLen = policyLst.SelectedItem.ToString().Length;
                        UpdateAllStatus();
                        policy.UpdateTransactionLog(eSTATES.POLICY_FQC, crd);
                        //DataSet pDs = policy.GetPolicyLog();
                        //if (pDs.Tables.Count > 0)
                        //{
                        //    if (pDs.Tables[0].Rows.Count > 0)
                        //    {
                        //        this.Text = this.Text + " QC User: " + pDs.Tables[0].Rows[0]["qc_user"].ToString();
                        //        this.Text = this.Text + " Index User: " + pDs.Tables[0].Rows[0]["index_user"].ToString();
                        //        this.Text = this.Text + " FQC User: " + pDs.Tables[0].Rows[0]["fqc_user"].ToString();
                        //    }
                        //}
                    }
                    else
                    {
                        Image img = null;
                        pictureControl.Image = img;
                        EnableDisbleControls(false);
                    }
                    //}
                    if (imageLst.Items.Count > 0)
                    {
                        policyLst = lstPolicy;
                        PopulateDelList(policyLst.SelectedItem.ToString());
                        DisplayDocTypeCount();
                        ShowImage(false);
                        //GetIndexDetails(policyLst.SelectedItem.ToString());
                        //ChangeSize();
                    }
                    CreateAllFolders();
                    if (tabControl2.SelectedIndex == 1)
                    {
                        lvwDockTypes.Items[lvwIndex].Selected = true;
                        lvwDockTypes.Focus();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error while copying all images from Index folder " + ex.Message);
                    stateLog = new MemoryStream();
                    tmpWrite = new System.Text.ASCIIEncoding().GetBytes(wBox.ctrlBox.ProjectCode.ToString());
                    stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                    exMailLog.Log(ex);
                }
                enddt = DateTime.Now;
                TimeSpan ts = enddt - stdt;
                //PopulateListView();
                //MessageBox.Show("Total time for execution - " + ts.ToString());
            }
        }

        [Category("Action")]
        [Description("Fires when the Image is changed.")]
        public event ImageChangeHandler ImageChanged;

        private void lstImage_SelectedIndexChanged(object sender, EventArgs e)
        {
            DateTime st = DateTime.Now;
            indexCount = lstImage.SelectedIndex;
            ShowImage(false);
            if ((ImageChanged != null) && (indexCount >= 0))
            {
                ImageChanged(this, e);
            }
            if (tabControl1.SelectedIndex == 1)
                PopulateListView();
            DateTime end = DateTime.Now;
            TimeSpan duration = end - st;
        }

        public event LstImageClick LstImgClick;
        private void lstImage_Click(object sender, EventArgs e)
        {
            DateTime stdt = DateTime.Now;
            tabControl2.SelectedIndex = 0;
            ShowImage(false);
            DateTime enddt = DateTime.Now;
            TimeSpan tp = enddt - stdt;
        }
        private bool ChangeAndMoveFile(string prmSourceFileName, string prmDocType)
        {
            string indexFileName = null;
            string sourceFile = null;
            CtrlImage ctrlImg;
            wfeImage wimg;
            int pos;
            int tifPos;
            string origDoctype = string.Empty;


            //newFileName=GetFileName(prmSourceFileName,prmDocType);

            //indexFileName=indexFolderName + "\\" + prmSourceFileName;
            try
            {
                pos = prmSourceFileName.ToString().IndexOf("-");
                tifPos = prmSourceFileName.ToString().IndexOf("-") + 1;
                if (tifPos > 0)
                {
                    origDoctype = prmSourceFileName.Substring(tifPos);
                }
                if (pos <= 0)
                {
                    pos = prmSourceFileName.ToString().IndexOf("TIF") - 1;
                    if (prmDocType != ihConstants.SIGNATUREPAGE_FILE)
                    {
                        indexFileName = indexFolderName + "\\" + prmSourceFileName.Substring(0, pos) + "-" + prmDocType + ".TIF";
                        sourceFile = indexFolderName + "\\" + prmSourceFileName;
                        indexFilePath = indexFileName;
                    }
                    else
                    {
                        indexFileName = indexFolderName + "\\" + prmSourceFileName.Substring(0, pos) + "-" + ihConstants.PROPOSALFORM_FILE + ".TIF";
                        sourceFile = indexFolderName + "\\" + prmSourceFileName;
                        indexFilePath = indexFileName;
                    }
                }
                else
                {
                    if (prmDocType != ihConstants.SIGNATUREPAGE_FILE)
                    {
                        prmSourceFileName = imageLst.SelectedItem.ToString().Substring(0, pos);
                        pImage = new CtrlImage(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), boxNumber, policyLst.SelectedItem.ToString(), prmSourceFileName, string.Empty);
                        wfeImage wImage = new wfeImage(sqlCon, pImage);
                        prmSourceFileName = wImage.GetIndexedImageName();
                        sourceFile = indexFolderName + "\\" + prmSourceFileName;
                        indexFileName = indexFolderName + "\\" + prmSourceFileName.Substring(0, pos - 3) + prmDocType + ".TIF";
                        indexFilePath = indexFileName;
                    }
                    else
                    {
                        if (origDoctype != ihConstants.SIGNATUREPAGE_FILE)
                        {
                            prmSourceFileName = imageLst.SelectedItem.ToString().Substring(0, pos);
                            pImage = new CtrlImage(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), boxNumber, policyLst.SelectedItem.ToString(), prmSourceFileName, string.Empty);
                            wfeImage wImage = new wfeImage(sqlCon, pImage);
                            prmSourceFileName = wImage.GetIndexedImageName();
                            sourceFile = indexFolderName + "\\" + prmSourceFileName;
                            indexFileName = indexFolderName + "\\" + prmSourceFileName.Substring(0, pos - 3) + ihConstants.PROPOSALFORM_FILE + ".TIF";
                            indexFilePath = indexFileName;
                        }
                    }
                }
                string fileCount = imageLst.SelectedItem.ToString().Substring(policyLen, 4);
                string propFileName = indexFolderName + "\\" + policyLst.SelectedItem.ToString() + fileCount + "_B-" + ihConstants.SIGNATUREPAGE_FILE + ".TIF";
                if ((File.Exists(propFileName) == false) || (File.Exists(indexFileName) == false))
                {
                    if (prmDocType != ihConstants.SIGNATUREPAGE_FILE)
                    {
                        if (origDoctype != prmDocType)
                        {
                            File.Copy(sourceFile, indexFileName, true);
                            File.Delete(sourceFile);
                        }
                    }
                    else
                    {
                        if (origDoctype != ihConstants.SIGNATUREPAGE_FILE)
                        {
                            if (File.Exists(indexFileName) == false)
                            {
                                File.Copy(sourceFile, indexFileName, true);
                                File.Delete(sourceFile);
                            }
                            policyLst = lstPolicy;
                            fileCount = imageLst.SelectedItem.ToString().Substring(policyLen, 4);
                            propFileName = indexFolderName + "\\" + policyLst.SelectedItem.ToString() + fileCount + "_B-" + ihConstants.SIGNATUREPAGE_FILE + ".TIF";
                            if (File.Exists(propFileName) == false)
                            {
                                ctrlImg = new CtrlImage(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), boxNumber, policyLst.SelectedItem.ToString(), policyLst.SelectedItem.ToString() + fileCount + "_B" + ".TIF", string.Empty);
                                wimg = new wfeImage(sqlCon, ctrlImg);
                                int imgCount = wimg.GetImageCount();
                                imgCount++;
                                System.IO.FileInfo info = new System.IO.FileInfo(indexFileName);
                                double fileSize = info.Length;
                                fileSize = fileSize / 1024;

                                if (wimg.Save(crd, eSTATES.PAGE_INDEXED, ihConstants.SIGNATUREPAGE_FILE, policyLst.SelectedItem.ToString() + fileCount + "_B-" + ihConstants.SIGNATUREPAGE_FILE + ".TIF", fileSize, imgCount) == true)
                                {
                                    File.Copy(indexFileName, propFileName, true);
                                    //File.Copy(indexFileName, sourceFilePath + "\\" + policyLst.SelectedItem.ToString() + fileCount + "_B" + ".TIF", true);
                                    imageLst.Items.Insert(imageLst.SelectedIndex + 1, policyLst.SelectedItem.ToString() + fileCount + "_B.TIF-" + ihConstants.SIGNATUREPAGE_FILE);
                                }
                            }
                            indexFileName = propFileName;
                        }
                    }
                }
                //img.LoadBitmapFromFile(indexFileName);
                return true;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while changing the index name......" + ex.Message);
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes("Project-" + projKey + " ,Batch-" + bundleKey + " ,Box-" + boxNumber + "Image name-" + sourceFile + "Doc type-" + prmDocType + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex);
                return false;
            }
        }

        private string GetFileName(string prmSelFileName, string prmDocType)
        {
            string modFileName = null;
            int pos;

            pos = prmSelFileName.IndexOf("-");
            if (pos <= 0)
            {
                modFileName = prmSelFileName;
            }
            else
            {
                modFileName = prmSelFileName.Substring(0, pos);
            }
            return modFileName;
        }


        void ShowIndexedImage()
        {
            string imageName;
            int pos = 0;
            string docType = string.Empty;

            imageLst = lstImage;
            try
            {
                imageName = imageLst.SelectedItem.ToString();
                pos = imageName.IndexOf("-");
                if (pos > 0)
                {
                    docType = imageName.Substring(pos);
                    pos = imageName.IndexOf(".TIF");
                    imageName = imageName.Substring(0, pos) + docType + ".TIF";
                    imgFileName = indexFolderName + "\\" + imageName;
                }
                else
                {
                    imgFileName = indexFolderName + "\\" + imageLst.SelectedItem.ToString();
                }

                //Open the source file
                img.LoadBitmapFromFile(imgFileName);
                //Show the image back in picture box
                //pictureControl.Image = img.GetBitmap();
                ChangeSize();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while showing image..." + ex.Message);
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes("Project-" + projKey + " ,Batch-" + bundleKey + " ,Box-" + boxNumber + "Image name-" + imgFileName + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex);
            }
        }


        [Category("Action")]
        [Description("Fires when the key Pressed for indexing")]
        public event LstImageIndexKeyPress LstImageIndex;
        private void lstImage_KeyPress(object sender, KeyPressEventArgs e)
        {
            DialogResult rlst;
            config = new ImageConfig(ihConstants.CONFIG_FILE_PATH);
            char PROPOSALFORM = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.PROPOSALFORM_KEY).Remove(1, 1).Trim());
            char PHOTOADDENDUM = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.PHOTOADDENDUM_KEY).Remove(1, 1).Trim());
            char PROPOSALENCLOSERS = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.PROPOSALENCLOSERS_KEY).Remove(1, 1).Trim());
            char SIGNATUREPAGE = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.SIGNATUREPAGE_KEY).Remove(1, 1).Trim());
            char MEDICALREPORT = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.MEDICALREPORT_KEY).Remove(1, 1).Trim());
            char PROPOSALREVIEWSLIP = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.PROPOSALREVIEWSLIP_KEY).Remove(1, 1).Trim());
            char POLICYBOND = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.POLICYBOND_KEY).Remove(1, 1).Trim());
            char NOMINATION = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.NOMINATION_KEY).Remove(1, 1).Trim());
            char ASSIGNMENT = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.ASSIGNMENT_KEY).Remove(1, 1).Trim());
            char ALTERATION = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.ALTERATION_KEY).Remove(1, 1).Trim());
            char REVIVALS = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.REVIVALS_KEY).Remove(1, 1).Trim());
            char POLICYLOANS = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.POLICYLOANS_KEY).Remove(1, 1).Trim());
            char SURRENDER = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.SURRENDER_KEY).Remove(1, 1).Trim());
            char CLAIMS = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.CLAIMS_KEY).Remove(1, 1).Trim());
            char CORRESPONDENCE = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.CORRESPONDENCE_KEY).Remove(1, 1).Trim());
            char OTHERS = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.OTHERS_KEY).Remove(1, 1).Trim());
            char KYCDOCUMENT = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.KYCDOCUMENT_KEY).Remove(1, 1).Trim());
            //char MAINPETITION = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.MAINPETITION_KEY).Remove(1, 1).Trim());
            //char MAINPETITIONANNEXURES = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.MAINPETITIONANNEXTURES_KEY).Remove(1, 1).Trim());
            //char AFFIDAVITSWITHANNEXURES = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.AFFIDAVITSWITHANNEXTURES_KEY).Remove(1, 1).Trim());
            //char WRITTENSTATEMENTOBJECTION = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.WRITTENSTATEMENTOBJECTION_KEY).Remove(1, 1).Trim());
            //char CONNECTEDAPPLICATIONS = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.CONNECTEDAPPLICATIONS_KEY).Remove(1, 1).Trim());
            //char ANALOGOUSANDCONNECTEDCASE = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.ANALOGOUSANDCONNECTEDCASE_KEY).Remove(1, 1).Trim());
            //char VAKALATNAMAANDWARRENT = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.VAKALATNAMAANDWARRENT_KEY).Remove(1, 1).Trim());
            //char SUMMONS = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.SUMMONS_KEY).Remove(1, 1).Trim());
            //char NOTICE = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.NOTICE_KEY).Remove(1, 1).Trim());
            //char WITNESSACTIONDEPOSITION = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.WITNESSACTIONDEPOSITION_KEY).Remove(1, 1).Trim());
            //char ISSUES = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.ISSUES_KEY).Remove(1, 1).Trim());
            //char EXHIBITS = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.EXHIBITS_KEY).Remove(1, 1).Trim());
            //char DRAFTPAPERS = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.DRAFTPAPERS_KEY).Remove(1, 1).Trim());
            //char NOTICEOFARGUMENT = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.NOTICEOFARGUMENT_KEY).Remove(1, 1).Trim());
            //char ENGROSSEDPRELIMINARY = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.ENGROSSEDPRELIMINARY_KEY).Remove(1, 1).Trim());
            //char ORDERSMAINCASE = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.ORDERSMAINCASE_KEY).Remove(1, 1).Trim());
            //char ORDERSAPPLICATIONS = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.ORDERSAPPLICATIONS_KEY).Remove(1, 1).Trim());
            //char FINALJUDGEMENTORDER = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.FINALJUDGEMENTORDER_KEY).Remove(1, 1).Trim());
            //char LOWERCOURTRECORDS = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.LOWERCOURTRECORDS_KEY).Remove(1, 1).Trim());
            //char IMPUGNEDORDER = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.IMPUGNEDORDER_KEY).Remove(1, 1).Trim());
            //char PAPERBOOK = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.PAPERBOOK_KEY).Remove(1, 1).Trim());
            char DELETE = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.DELETE_KEY).Remove(1, 1).Trim());
            char PATIENTIDENTIFICATION = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.PATIENTIDENTIFICATION_KEY).Remove(1, 1).Trim());
            char COMPLAINS = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.COMPLAINS_KEY).Remove(1, 1).Trim());
            char ADMISSIONSHEET = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.ADMISSIONSHEET_KEY).Remove(1, 1).Trim());
            char PHYSICIANORDERSHEET = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.PHYSICIANORDERSHEET_KEY).Remove(1, 1).Trim());
            char ADMISSIONASSESSMENT = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.ADMISSIONASSESSMENT_KEY).Remove(1, 1).Trim());
            char FAMILYHISTORY = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.FAMILYHISTORY_KEY).Remove(1, 1).Trim());
            char CONSENTFORTREATMENT = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.CONSENTFORTREATMENT_KEY).Remove(1, 1).Trim());
            char PHYSICALEXAMINATION = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.PHYSICALEXAMINATION_KEY).Remove(1, 1).Trim());
            char MEDICALHISTORY = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.MEDICALHISTORY_KEY).Remove(1, 1).Trim());
            char TENTATIVEHISTORY = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.TENTATIVEHISTORY_KEY).Remove(1, 1).Trim());
            char MEDICALDIAGNOSIS = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.MEDICALDIAGNOSIS_KEY).Remove(1, 1).Trim());
            char CAREGIVEN = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.CAREGIVEN_KEY).Remove(1, 1).Trim());
            char TRAETMENTGIVEN = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.TRAETMENTGIVEN_KEY).Remove(1, 1).Trim());
            char THERATPEUTICORDER = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.THERATPEUTICORDER_KEY).Remove(1, 1).Trim());
            char PROGRESSNOTES = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.PROGRESSNOTES_KEY).Remove(1, 1).Trim());
            char OPERATIVEPROCEDURE = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.OPERATIVEPROCEDURE_KEY).Remove(1, 1).Trim());
            char DISCHARGEREPORT = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.DISCHARGEREPORT_KEY).Remove(1, 1).Trim());
            char DEATHREPORT = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.DEATHREPORT_KEY).Remove(1, 1).Trim());
            int pos = 0;
            int index;
            bool bolKey = false;
            string selImageName = null;
            string originalFileName = null;
            string indexFileName = null;
            string docType = null;
            bool indexedBol = false;
            bool sigBol = false;

            policyLst = lstPolicy;

            string policyNo = policyLst.SelectedItem.ToString();
            string origDoctype = string.Empty;
            int tifPos = imageLst.SelectedItem.ToString().ToString().IndexOf("-") + 1;
            if (tifPos > 0)
            {
                origDoctype = imageLst.SelectedItem.ToString().Substring(tifPos);
            }
            if (Convert.ToChar(e.KeyChar.ToString().ToUpper()) == DELETE)
            {
                ImageDelete();
                return;
            }
            if (origDoctype == "Signature_page" && (Convert.ToChar(e.KeyChar.ToString().ToUpper()) != DELETE))
            {
                MessageBox.Show("You can not change signature page. You can only delete it...");
                return;
            }
            if (Convert.ToChar(e.KeyChar.ToString().ToUpper()) == PROPOSALFORM)
            {
                indexedBol = ChangeAndMoveFile(imageLst.SelectedItem.ToString(), ihConstants.PROPOSALFORM_FILE);
                docType = ihConstants.PROPOSALFORM_FILE;
                bolKey = true;
            }
            //if (Convert.ToChar(e.KeyChar.ToString().ToUpper()) == MAINPETITION)
            //{
            //    indexedBol = ChangeAndMoveFile(imageLst.SelectedItem.ToString(), ihConstants.MAINPETITION_FILE);
            //    docType = ihConstants.MAINPETITION_FILE;
            //    bolKey = true;
            //}
            //if (Convert.ToChar(e.KeyChar.ToString().ToUpper()) == MAINPETITIONANNEXURES)
            //{
            //    indexedBol = ChangeAndMoveFile(imageLst.SelectedItem.ToString(), ihConstants.MAINPETITIONANNEXTURES_FILE);
            //    docType = ihConstants.MAINPETITIONANNEXTURES_FILE;
            //    bolKey = true;
            //}
            //if (Convert.ToChar(e.KeyChar.ToString().ToUpper()) == AFFIDAVITSWITHANNEXURES)
            //{
            //    indexedBol = ChangeAndMoveFile(imageLst.SelectedItem.ToString(), ihConstants.AFFIDAVITSWITHANNEXTURES_FILE);
            //    docType = ihConstants.AFFIDAVITSWITHANNEXTURES_FILE;
            //    bolKey = true;
            //}
            //if (Convert.ToChar(e.KeyChar.ToString().ToUpper()) == WRITTENSTATEMENTOBJECTION)
            //{
            //    indexedBol = ChangeAndMoveFile(imageLst.SelectedItem.ToString(), ihConstants.WRITTENSTATEMENTOBJECTION_FILE);
            //    docType = ihConstants.WRITTENSTATEMENTOBJECTION_FILE;
            //    bolKey = true;
            //}
            //if (Convert.ToChar(e.KeyChar.ToString().ToUpper()) == CONNECTEDAPPLICATIONS)
            //{
            //    indexedBol = ChangeAndMoveFile(imageLst.SelectedItem.ToString(), ihConstants.CONNECTEDAPPLICATIONS_FILE);
            //    docType = ihConstants.CONNECTEDAPPLICATIONS_FILE;
            //    bolKey = true;
            //}
            //if (Convert.ToChar(e.KeyChar.ToString().ToUpper()) == ANALOGOUSANDCONNECTEDCASE)
            //{
            //    indexedBol = ChangeAndMoveFile(imageLst.SelectedItem.ToString(), ihConstants.ANALOGOUSANDCONNECTEDCASE_FILE);
            //    docType = ihConstants.ANALOGOUSANDCONNECTEDCASE_FILE;
            //    bolKey = true;
            //}
            //if (Convert.ToChar(e.KeyChar.ToString().ToUpper()) == VAKALATNAMAANDWARRENT)
            //{
            //    indexedBol = ChangeAndMoveFile(imageLst.SelectedItem.ToString(), ihConstants.VAKALATNAMAANDWARRENT_FILE);
            //    docType = ihConstants.VAKALATNAMAANDWARRENT_FILE;
            //    bolKey = true;
            //}
            //if (Convert.ToChar(e.KeyChar.ToString().ToUpper()) == SUMMONS)
            //{
            //    indexedBol = ChangeAndMoveFile(imageLst.SelectedItem.ToString(), ihConstants.SUMMONS_FILE);
            //    docType = ihConstants.SUMMONS_FILE;
            //    bolKey = true;
            //}
            //if (Convert.ToChar(e.KeyChar.ToString().ToUpper()) == NOTICE)
            //{
            //    indexedBol = ChangeAndMoveFile(imageLst.SelectedItem.ToString(), ihConstants.NOTICE_FILE);
            //    docType = ihConstants.NOTICE_FILE;
            //    bolKey = true;
            //}
            //if (Convert.ToChar(e.KeyChar.ToString().ToUpper()) == WITNESSACTIONDEPOSITION)
            //{
            //    indexedBol = ChangeAndMoveFile(imageLst.SelectedItem.ToString(), ihConstants.WITNESSACTIONDEPOSITION_FILE);
            //    docType = ihConstants.WITNESSACTIONDEPOSITION_FILE;
            //    bolKey = true;
            //}
            //if (Convert.ToChar(e.KeyChar.ToString().ToUpper()) == ISSUES)
            //{
            //    indexedBol = ChangeAndMoveFile(imageLst.SelectedItem.ToString(), ihConstants.ISSUES_FILE);
            //    docType = ihConstants.ISSUES_FILE;
            //    bolKey = true;
            //}
            //if (Convert.ToChar(e.KeyChar.ToString().ToUpper()) == EXHIBITS)
            //{
            //    indexedBol = ChangeAndMoveFile(imageLst.SelectedItem.ToString(), ihConstants.EXHIBITS_FILE);
            //    docType = ihConstants.EXHIBITS_FILE;
            //    bolKey = true;
            //}
            //if (Convert.ToChar(e.KeyChar.ToString().ToUpper()) == DRAFTPAPERS)
            //{
            //    indexedBol = ChangeAndMoveFile(imageLst.SelectedItem.ToString(), ihConstants.DRAFTPAPERS_FILE);
            //    docType = ihConstants.DRAFTPAPERS_FILE;
            //    bolKey = true;
            //}
            //if (Convert.ToChar(e.KeyChar.ToString().ToUpper()) == NOTICEOFARGUMENT)
            //{
            //    indexedBol = ChangeAndMoveFile(imageLst.SelectedItem.ToString(), ihConstants.NOTICEOFARGUMENT_FILE);
            //    docType = ihConstants.NOTICEOFARGUMENT_FILE;
            //    bolKey = true;
            //}
            //if (Convert.ToChar(e.KeyChar.ToString().ToUpper()) == ENGROSSEDPRELIMINARY)
            //{
            //    indexedBol = ChangeAndMoveFile(imageLst.SelectedItem.ToString(), ihConstants.ENGROSSEDPRELIMINARY_FILE);
            //    docType = ihConstants.ENGROSSEDPRELIMINARY_FILE;
            //    bolKey = true;
            //}
            //if (Convert.ToChar(e.KeyChar.ToString().ToUpper()) == ORDERSMAINCASE)
            //{
            //    indexedBol = ChangeAndMoveFile(imageLst.SelectedItem.ToString(), ihConstants.ORDERSMAINCASE_FILE);
            //    docType = ihConstants.ORDERSMAINCASE_FILE;
            //    bolKey = true;
            //}
            //if (Convert.ToChar(e.KeyChar.ToString().ToUpper()) == ORDERSAPPLICATIONS)
            //{
            //    indexedBol = ChangeAndMoveFile(imageLst.SelectedItem.ToString(), ihConstants.ORDERSAPPLICATIONS_FILE);
            //    docType = ihConstants.ORDERSAPPLICATIONS_FILE;
            //    bolKey = true;
            //}
            //if (Convert.ToChar(e.KeyChar.ToString().ToUpper()) == FINALJUDGEMENTORDER)
            //{
            //    indexedBol = ChangeAndMoveFile(imageLst.SelectedItem.ToString(), ihConstants.FINALJUDGEMENTORDER_FILE);
            //    docType = ihConstants.FINALJUDGEMENTORDER_FILE;
            //    bolKey = true;
            //}
            //if (Convert.ToChar(e.KeyChar.ToString().ToUpper()) == LOWERCOURTRECORDS)
            //{
            //    indexedBol = ChangeAndMoveFile(imageLst.SelectedItem.ToString(), ihConstants.LOWERCOURTRECORDS_FILE);
            //    docType = ihConstants.LOWERCOURTRECORDS_FILE;
            //    bolKey = true;
            //}
            //if (Convert.ToChar(e.KeyChar.ToString().ToUpper()) == IMPUGNEDORDER)
            //{
            //    indexedBol = ChangeAndMoveFile(imageLst.SelectedItem.ToString(), ihConstants.IMPUGNEDORDER_FILE);
            //    docType = ihConstants.IMPUGNEDORDER_FILE;
            //    bolKey = true;
            //}
            //if (Convert.ToChar(e.KeyChar.ToString().ToUpper()) == PAPERBOOK)
            //{
            //    indexedBol = ChangeAndMoveFile(imageLst.SelectedItem.ToString(), ihConstants.PAPERBOOK_FILE);
            //    docType = ihConstants.PAPERBOOK_FILE;
            //    bolKey = true;
            //}
            if (Convert.ToChar(e.KeyChar.ToString().ToUpper()) == PHOTOADDENDUM)
            {
                indexedBol = ChangeAndMoveFile(imageLst.SelectedItem.ToString(), ihConstants.PHOTOADDENDUM_FILE);
                docType = ihConstants.PHOTOADDENDUM_FILE;
                bolKey = true;
            }
            if (Convert.ToChar(e.KeyChar.ToString().ToUpper()) == PROPOSALENCLOSERS)
            {
                indexedBol = ChangeAndMoveFile(imageLst.SelectedItem.ToString(), ihConstants.PROPOSALENCLOSERS_FILE);
                docType = ihConstants.PROPOSALENCLOSERS_FILE;
                bolKey = true;
            }
            if (Convert.ToChar(e.KeyChar.ToString().ToUpper()) == SIGNATUREPAGE)
            {
                if (origDoctype != ihConstants.SIGNATUREPAGE_FILE)
                {
                    indexedBol = ChangeAndMoveFile(imageLst.SelectedItem.ToString(), ihConstants.SIGNATUREPAGE_FILE);
                    docType = ihConstants.PROPOSALFORM_FILE;
                    bolKey = true;
                    sigBol = true;
                }
            }
            if (Convert.ToChar(e.KeyChar.ToString().ToUpper()) == MEDICALREPORT)
            {
                indexedBol = ChangeAndMoveFile(imageLst.SelectedItem.ToString(), ihConstants.MEDICALREPORT_FILE);
                docType = ihConstants.MEDICALREPORT_FILE;
                bolKey = true;
            }
            if (Convert.ToChar(e.KeyChar.ToString().ToUpper()) == PROPOSALREVIEWSLIP)
            {
                indexedBol = ChangeAndMoveFile(imageLst.SelectedItem.ToString(), ihConstants.PROPOSALREVIEWSLIP_FILE);
                docType = ihConstants.PROPOSALREVIEWSLIP_FILE;
                bolKey = true;
            }
            if (Convert.ToChar(e.KeyChar.ToString().ToUpper()) == POLICYBOND)
            {
                indexedBol = ChangeAndMoveFile(imageLst.SelectedItem.ToString(), ihConstants.POLICYBOND_FILE);
                docType = ihConstants.POLICYBOND_FILE;
                bolKey = true;
            }
            if (Convert.ToChar(e.KeyChar.ToString().ToUpper()) == NOMINATION)
            {
                indexedBol = ChangeAndMoveFile(imageLst.SelectedItem.ToString(), ihConstants.NOMINATION_FILE);
                docType = ihConstants.NOMINATION_FILE;
                bolKey = true;
            }
            if (Convert.ToChar(e.KeyChar.ToString().ToUpper()) == ASSIGNMENT)
            {
                indexedBol = ChangeAndMoveFile(imageLst.SelectedItem.ToString(), ihConstants.ASSIGNMENT_FILE);
                docType = ihConstants.ASSIGNMENT_FILE;
                bolKey = true;
            }
            if (Convert.ToChar(e.KeyChar.ToString().ToUpper()) == ALTERATION)
            {
                indexedBol = ChangeAndMoveFile(imageLst.SelectedItem.ToString(), ihConstants.ALTERATION_FILE);
                docType = ihConstants.ALTERATION_FILE;
                bolKey = true;
            }
            if (Convert.ToChar(e.KeyChar.ToString().ToUpper()) == REVIVALS)
            {
                indexedBol = ChangeAndMoveFile(imageLst.SelectedItem.ToString(), ihConstants.REVIVALS_FILE);
                docType = ihConstants.REVIVALS_FILE;
                bolKey = true;
            }
            if (Convert.ToChar(e.KeyChar.ToString().ToUpper()) == POLICYLOANS)
            {
                indexedBol = ChangeAndMoveFile(imageLst.SelectedItem.ToString(), ihConstants.POLICYLOANS_FILE);
                docType = ihConstants.POLICYLOANS_FILE;
                bolKey = true;
            }
            if (Convert.ToChar(e.KeyChar.ToString().ToUpper()) == SURRENDER)
            {
                indexedBol = ChangeAndMoveFile(imageLst.SelectedItem.ToString(), ihConstants.SURRENDER_FILE);
                docType = ihConstants.SURRENDER_FILE;
                bolKey = true;
            }
            if (Convert.ToChar(e.KeyChar.ToString().ToUpper()) == CLAIMS)
            {
                indexedBol = ChangeAndMoveFile(imageLst.SelectedItem.ToString(), ihConstants.CLAIMS_FILE);
                docType = ihConstants.CLAIMS_FILE;
                bolKey = true;
            }
            if (Convert.ToChar(e.KeyChar.ToString().ToUpper()) == CORRESPONDENCE)
            {
                indexedBol = ChangeAndMoveFile(imageLst.SelectedItem.ToString(), ihConstants.CORRESPONDENCE_FILE);
                docType = ihConstants.CORRESPONDENCE_FILE;
                bolKey = true;
            }
            if (Convert.ToChar(e.KeyChar.ToString().ToUpper()) == OTHERS)
            {
                indexedBol = ChangeAndMoveFile(imageLst.SelectedItem.ToString(), ihConstants.OTHERS_FILE);
                docType = ihConstants.OTHERS_FILE;
                bolKey = true;
            }
            if (Convert.ToChar(e.KeyChar.ToString().ToUpper()) == KYCDOCUMENT)
            {
                indexedBol = ChangeAndMoveFile(imageLst.SelectedItem.ToString(), ihConstants.KYCDOCUMENT_FILE);
                docType = ihConstants.KYCDOCUMENT_FILE;
                bolKey = true;
            }
            if (Convert.ToChar(e.KeyChar.ToString().ToUpper()) == PATIENTIDENTIFICATION)
            {
                indexedBol = ChangeAndMoveFile(imageLst.SelectedItem.ToString(), ihConstants.PATIENTIDENTIFICATION_FILE);
                docType = ihConstants.PATIENTIDENTIFICATION_FILE;
                bolKey = true;
            }
            if (Convert.ToChar(e.KeyChar.ToString().ToUpper()) == COMPLAINS)
            {
                indexedBol = ChangeAndMoveFile(imageLst.SelectedItem.ToString(), ihConstants.COMPLAINS_FILE);
                docType = ihConstants.COMPLAINS_FILE;
                bolKey = true;
            }
            if (Convert.ToChar(e.KeyChar.ToString().ToUpper()) == ADMISSIONSHEET)
            {
                indexedBol = ChangeAndMoveFile(imageLst.SelectedItem.ToString(), ihConstants.ADMISSIONSHEET_FILE);
                docType = ihConstants.ADMISSIONSHEET_FILE;
                bolKey = true;
            }
            if (Convert.ToChar(e.KeyChar.ToString().ToUpper()) == PHYSICIANORDERSHEET)
            {
                indexedBol = ChangeAndMoveFile(imageLst.SelectedItem.ToString(), ihConstants.PHYSICIANORDERSHEET_FILE);
                docType = ihConstants.PHYSICIANORDERSHEET_FILE;
                bolKey = true;
            }
            if (Convert.ToChar(e.KeyChar.ToString().ToUpper()) == ADMISSIONASSESSMENT)
            {
                indexedBol = ChangeAndMoveFile(imageLst.SelectedItem.ToString(), ihConstants.ADMISSIONASSESSMENT_FILE);
                docType = ihConstants.ADMISSIONASSESSMENT_FILE;
                bolKey = true;
            }
            if (Convert.ToChar(e.KeyChar.ToString().ToUpper()) == FAMILYHISTORY)
            {
                indexedBol = ChangeAndMoveFile(imageLst.SelectedItem.ToString(), ihConstants.FAMILYHISTORY_FILE);
                docType = ihConstants.FAMILYHISTORY_FILE;
                bolKey = true;
            }
            if (Convert.ToChar(e.KeyChar.ToString().ToUpper()) == CONSENTFORTREATMENT)
            {
                indexedBol = ChangeAndMoveFile(imageLst.SelectedItem.ToString(), ihConstants.CONSENTFORTREATMENT_FILE);
                docType = ihConstants.CONSENTFORTREATMENT_FILE;
                bolKey = true;
            }
            if (Convert.ToChar(e.KeyChar.ToString().ToUpper()) == PHYSICALEXAMINATION)
            {
                indexedBol = ChangeAndMoveFile(imageLst.SelectedItem.ToString(), ihConstants.PHYSICALEXAMINATION_FILE);
                docType = ihConstants.PHYSICALEXAMINATION_FILE;
                bolKey = true;
            }
            if (Convert.ToChar(e.KeyChar.ToString().ToUpper()) == MEDICALHISTORY)
            {
                indexedBol = ChangeAndMoveFile(imageLst.SelectedItem.ToString(), ihConstants.MEDICALHISTORY_FILE);
                docType = ihConstants.MEDICALHISTORY_FILE;
                bolKey = true;
            }
            if (Convert.ToChar(e.KeyChar.ToString().ToUpper()) == TENTATIVEHISTORY)
            {
                indexedBol = ChangeAndMoveFile(imageLst.SelectedItem.ToString(), ihConstants.TENTATIVEHISTORY_FILE);
                docType = ihConstants.TENTATIVEHISTORY_FILE;
                bolKey = true;
            }
            if (Convert.ToChar(e.KeyChar.ToString().ToUpper()) == MEDICALDIAGNOSIS)
            {
                indexedBol = ChangeAndMoveFile(imageLst.SelectedItem.ToString(), ihConstants.MEDICALDIAGNOSIS_FILE);
                docType = ihConstants.MEDICALDIAGNOSIS_FILE;
                bolKey = true;
            }
            if (Convert.ToChar(e.KeyChar.ToString().ToUpper()) == CAREGIVEN)
            {
                indexedBol = ChangeAndMoveFile(imageLst.SelectedItem.ToString(), ihConstants.CAREGIVEN_FILE);
                docType = ihConstants.CAREGIVEN_FILE;
                bolKey = true;
            }
            if (Convert.ToChar(e.KeyChar.ToString().ToUpper()) == TRAETMENTGIVEN)
            {
                indexedBol = ChangeAndMoveFile(imageLst.SelectedItem.ToString(), ihConstants.TRAETMENTGIVEN_FILE);
                docType = ihConstants.TRAETMENTGIVEN_FILE;
                bolKey = true;
            }
            if (Convert.ToChar(e.KeyChar.ToString().ToUpper()) == THERATPEUTICORDER)
            {
                indexedBol = ChangeAndMoveFile(imageLst.SelectedItem.ToString(), ihConstants.THERATPEUTICORDER_FILE);
                docType = ihConstants.THERATPEUTICORDER_FILE;
                bolKey = true;
            }
            if (Convert.ToChar(e.KeyChar.ToString().ToUpper()) == PROGRESSNOTES)
            {
                indexedBol = ChangeAndMoveFile(imageLst.SelectedItem.ToString(), ihConstants.PROGRESSNOTES_FILE);
                docType = ihConstants.PROGRESSNOTES_FILE;
                bolKey = true;
            }
            if (Convert.ToChar(e.KeyChar.ToString().ToUpper()) == OPERATIVEPROCEDURE)
            {
                indexedBol = ChangeAndMoveFile(imageLst.SelectedItem.ToString(), ihConstants.OPERATIVEPROCEDURE_FILE);
                docType = ihConstants.OPERATIVEPROCEDURE_FILE;
                bolKey = true;
            }
            if (Convert.ToChar(e.KeyChar.ToString().ToUpper()) == DISCHARGEREPORT)
            {
                indexedBol = ChangeAndMoveFile(imageLst.SelectedItem.ToString(), ihConstants.DISCHARGEREPORT_FILE);
                docType = ihConstants.DISCHARGEREPORT_FILE;
                bolKey = true;
            }
            if (Convert.ToChar(e.KeyChar.ToString().ToUpper()) == DEATHREPORT)
            {
                indexedBol = ChangeAndMoveFile(imageLst.SelectedItem.ToString(), ihConstants.DEATHREPORT_FILE);
                docType = ihConstants.DEATHREPORT_FILE;
                bolKey = true;
            }
            if (bolKey == true)
            {
                pos = imageLst.SelectedItem.ToString().IndexOf("-");
                if (pos <= 0)
                {
                    selImageName = imageLst.SelectedItem.ToString() + "-";
                    pos = imageLst.SelectedItem.ToString().IndexOf(".TIF") + 5;
                    selImageName = selImageName.Insert(pos, docType);
                }
                else
                {
                    selImageName = imageLst.SelectedItem.ToString().Substring(0, (pos + 1));
                    selImageName = selImageName + docType;
                }
                if (indexedBol == true)
                {
                    originalFileName = GetFileName(imageLst.SelectedItem.ToString(), docType);
                    pos = originalFileName.ToString().IndexOf(".TIF");
                    indexFileName = selImageName.Substring(0, pos) + "-" + docType + ".TIF";
                    ctrlPolicy = new CtrlPolicy(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), boxNumber, policyNo);
                    policy = new wfePolicy(sqlCon, ctrlPolicy);
                    indexingOn = true;
                    if ((GetPolicyStatus() == 5) || (GetPolicyStatus() == 4) || (GetPolicyStatus() == 3) || (GetPolicyStatus() == (int)eSTATES.POLICY_NOT_INDEXED) || (GetPolicyStatus() == (int)eSTATES.POLICY_EXCEPTION) || (GetPolicyStatus() == (int)eSTATES.POLICY_CHECKED))
                    {
                        UpdateState(eSTATES.PAGE_FQC, originalFileName, docType, indexFileName);
                    }
                    else if (GetPolicyStatus() == (int)eSTATES.POLICY_ON_HOLD)
                    {
                        UpdateState(eSTATES.PAGE_ON_HOLD, originalFileName, docType, indexFileName);
                    }
                    else if (GetPolicyStatus() == 6 || GetPolicyStatus() == 7 || GetPolicyStatus() == 8)
                    {
                        UpdateState(eSTATES.PAGE_EXPORTED, originalFileName, docType, indexFileName);
                    }
                    indexingOn = false;
                }
                index = imageLst.SelectedIndex;
                //imageLst.Text.Replace(imageLst.SelectedItem.ToString(), selImageName);
                imageLst.Items[index] = selImageName;
                imageLst.Refresh();
                //imageLst.Items.RemoveAt(index);
                //imageLst.Text = selImageName;
                //selImageName = selImageName + " - " + docType;
                //imageLst.Items.Insert(index,selImageName );


                if ((index + 1) != imageLst.Items.Count)
                {
                    if (docType != ihConstants.SIGNATUREPAGE_FILE)
                    {
                        imageLst.SelectedIndex = index + 1;
                        ShowIndexedImage();
                    }
                    if (sigBol == true)
                    {
                        if ((index + 2) != imageLst.Items.Count)
                        {
                            imageLst.SelectedIndex = index + 2;
                            ShowIndexedImage();
                        }
                        else
                        {
                            if ((policyLst.SelectedIndex + 1) != policyLst.Items.Count)
                            {
                                policyLst.SelectedIndex = policyLst.SelectedIndex + 1;
                            }
                        }
                    }
                }
                else
                {
                    if ((policyLst.SelectedIndex + 1) != (policyLst.Items.Count))
                    {
                        policyLst.SelectedIndex = policyLst.SelectedIndex + 1;
                    }
                }
                if (GetFileCount(projKey, bundleKey) == 0)
                {
                    ///Update the batch status
                    //wBatch.UpdateStatus(eSTATES.BATCH_SCANNED, wBox.ctrlBox.BatchKey);
                    UpdateBundleStatus(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey));
                }
            }
            DisplayDocTypeCount();
        }

        
        private void lstImageDel_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        public event LstDelImageClick LstDelImgClick;
        private void lstImageDel_Click(object sender, EventArgs e)
        {
            string delFileName;

            delImgList = lstImageDel;
            if (lstImageDel.Items.Count > 0)
            {
                //statusStrip1.Visible = true;
                toolStripStatusLabel1.Text = "";
                toolStripStatusLabel1.Text = "Press 'Insert' to remove image from deleted list";
            }
            else
            {
                statusStrip1.Visible = false;
                toolStripStatusLabel1.Text = "";
            }
            if (delImgList.Items.Count > 0)
            {
                if (delImgList.SelectedIndex >= 0)
                {
                    EnableDisbleControls(false);
                    if (File.Exists(policyPath + "\\" + ihConstants._SCAN_FOLDER + "\\" + ihConstants._DELETE_FOLDER + "\\" + delImgList.SelectedItem.ToString()))
                    {
                        delFileName = policyPath + "\\" + ihConstants._SCAN_FOLDER + "\\" + ihConstants._DELETE_FOLDER;
                    }
                    else
                    {
                        delFileName = sourceFilePath + "\\" + ihConstants._DELETE_FOLDER;
                    }

                    string[] searchFileName = Directory.GetFiles(delFileName, delImgList.SelectedItem.ToString());
                    //For searching deleted file in deleted folder.
                    if (searchFileName.Length >= 0)
                    {
                        delFileName = searchFileName[0];

                        img.LoadBitmapFromFile(delFileName);
                        ChangeSize(delFileName);
                        //Show the image back in picture box
                        //pictureControl.Image = img.GetBitmap();
                    }
                }
            }
            if (LstDelImgClick != null)
            {
                LstDelImgClick(this, e);
            }
        }

        [Category("Action")]
        [Description("Fires when the image from deleted list inserted.")]
        public event LstDelImageKeyPress LstDelIamgeInsert;	
        private void lstImageDel_KeyDown(object sender, KeyEventArgs e)
        {
            string delPath = null;
            string sourceFileName = null;
            string qcFilePath = null;
            string sourcePath = null;
            string[] searchFileName;
            try
            {
                if (e.KeyCode == Keys.Insert)
                {

                    delPath = policyPath + "\\" + ihConstants._SCAN_FOLDER + "\\" + ihConstants._DELETE_FOLDER;
                    imageDelLst = lstImageDel;
                    policyLst = lstPolicy;
                    imageLst = lstImage;
                    if (imageDelLst.Items.Count > 0)
                    {
                        imageLst = lstImage;
                        ctrlPolicy = new CtrlPolicy(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), boxNumber, policyLst.SelectedItem.ToString());
                        policy = new wfePolicy(sqlCon, ctrlPolicy);

                        //policyData = (udtPolicy)policy.LoadValuesFromDB();
                        policyPath = GetPolicyPath(); //policyData.policy_path;
                        //To get the index file name from del list selected file name.
                        pImage = new CtrlImage(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), boxNumber, policyLst.SelectedItem.ToString(), imageDelLst.SelectedItem.ToString(), string.Empty);
                        wfeImage wImage = new wfeImage(sqlCon, pImage);
                        string changedImageName = wImage.GetIndexedImageName();

                        if (changedImageName == string.Empty)
                        {
                            sourceFileName = sourceFilePath + "\\" + imageDelLst.SelectedItem.ToString();
                            qcFilePath = indexFolderName + "\\" + imageDelLst.SelectedItem.ToString();
                        }
                        else
                        {
                            sourceFileName = sourceFilePath + "\\" + changedImageName;
                            qcFilePath = indexFolderName + "\\" + changedImageName;
                        }
                        int pos = imageDelLst.SelectedItem.ToString().IndexOf(".TIF");
                        searchFileName = Directory.GetFiles(delPath, imageDelLst.SelectedItem.ToString().Substring(0, pos) + "*.TIF", SearchOption.AllDirectories);
                        //For searching deleted file in deleted folder.
                        if (searchFileName.Length <= 0)
                        {
                            //delPath = policyPath + "\\" + ihConstants._QC_FOLDER;
                            //searchFileName = Directory.GetFiles(delPath, imageDelLst.SelectedItem.ToString().Substring(0, pos) + "*.TIF", SearchOption.AllDirectories);
                            //if (searchFileName.Length <= 0)
                            //{
                            delPath = policyPath + "\\" + ihConstants._SCAN_FOLDER;
                            searchFileName = Directory.GetFiles(delPath, imageDelLst.SelectedItem.ToString().Substring(0, pos) + "*.TIF", SearchOption.AllDirectories);
                            if (searchFileName.Length > 0)
                            {
                                sourcePath = searchFileName[0];
                            }
                            //}
                            //else
                            //{
                            //    sourcePath = searchFileName[0];
                            //}
                        }
                        else
                        {
                            sourcePath = searchFileName[0];
                        }
                        string scanFilePath = policyPath + "\\" + ihConstants._SCAN_FOLDER + "\\" + imageDelLst.SelectedItem.ToString();
                        if (sourcePath != string.Empty)
                        {
                            if (File.Exists(sourcePath) == true)
                            {
                                File.Move(sourcePath, qcFilePath);
                                if (File.Exists(scanFilePath) == false)
                                    File.Copy(qcFilePath, scanFilePath);
                            }
                            //pImage = new CtrlImage(Convert.ToInt32(wBox.ctrlBox.ProjectCode),Convert.ToInt32(wBox.ctrlBox.BatchKey.ToString()),wBox.ctrlBox.BoxNumber.ToString().ToString(), policyLst.SelectedItem.ToString(),imageDelLst.SelectedItem.ToString(),string.Empty);
                            //		    wfeImage wImage  = new wfeImage(sqlCon, pImage);
                            //		    wImage.UpdateStatus(eSTATES.PAGE_DELETED);
                            insertFlag = true;
                            int dashedPos = changedImageName.IndexOf("-");
                            if (dashedPos > 0)
                            {
                                string docType = changedImageName.Substring(dashedPos + 1);
                                int tifPos = docType.IndexOf(".TIF");
                                if (tifPos > 0)
                                {
                                    docType = docType.Substring(0, tifPos);
                                    UpdateState(eSTATES.PAGE_FQC, imageDelLst.SelectedItem.ToString(), docType, changedImageName);
                                }
                            }
                            else
                            {
                                pPolicy = new CtrlPolicy(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), boxNumber, policyLst.SelectedItem.ToString());
                                wfePolicy wPolicy = new wfePolicy(sqlCon, pPolicy);
                                wPolicy.UpdateStatus(eSTATES.POLICY_NOT_INDEXED, crd);
                                UpdateState(eSTATES.PAGE_NOT_INDEXED, imageDelLst.SelectedItem.ToString());
                            }
                            InsertNotify(imageLst.SelectedIndex);
                        }
                    }
                    deLabel2.Text = "Total Scanned Image : " + lstImage.Items.Count.ToString();
                }
                DisplayDocTypeCount();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while doing the operation.... " + ex.Message);
            }
        }
        public bool InsertNotify(int currIndex)
        {
            //lstImage.Items.Remove(delValue);
            PopulateImageList(lstPolicy.SelectedItem.ToString());
            if (lstImage.Items.Count > 0)
            {
                if (currIndex != lstImage.Items.Count)
                {
                    lstImage.SelectedIndex = currIndex;
                    indexCount = lstImage.SelectedIndex;
                }
                else
                    lstPolicy.SelectedIndex = lstPolicy.SelectedIndex + 1;
            }
            else
            {
                if ((lstPolicy.SelectedIndex) != (lstPolicy.Items.Count - 1))
                {
                    lstPolicy.SelectedIndex = lstPolicy.SelectedIndex + 1;
                }
            }
            PopulateDelList(lstPolicy.SelectedItem.ToString());
            //			//MoveNext();
            //			lstImage.Refresh();
            //			lstImageDel.Refresh();
            return true;
        }

        private void cmdPrevious_Click(object sender, EventArgs e)
        {
            policyLst = lstPolicy;

            string policyNo = policyLst.SelectedItem.ToString();

            ctrlPolicy = new CtrlPolicy(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), boxNumber, policyNo);
            policy = new wfePolicy(sqlCon, ctrlPolicy);
            if (GetPolicyStatus() != (int)eSTATES.POLICY_ON_HOLD)
            {
                EnableDisbleControls(true);
            }
            else
            {
                EnableDisbleControls(false);
            }
            //BoxDtls.SetCurrentSelection(imageLst.SelectedIndex);
            //ClearSelection();
            MovePrevious();
            ShowImage(false);
        }
        public bool MovePrevious()
        {
            if ((indexCount > 0) && (lstImage.Items.Count > 0))
            {
                //ClearSelection();
                indexCount = indexCount - 1;
                lstImage.SelectedIndex = indexCount;
            }
            if ((indexCount == 0) && (lstImage.Items.Count > 0) && (lstPolicy.SelectedIndex != 0))
            {
                //ClearSelection();
                indexCount = 0;
                lstPolicy.SelectedIndex = lstPolicy.SelectedIndex - 1;
            }
            return true;
        }
        private void cmdNext_Click(object sender, EventArgs e)
        {
            policyLst = lstPolicy;

            string policyNo = policyLst.SelectedItem.ToString();

            ctrlPolicy = new CtrlPolicy(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), boxNumber, policyNo);
            policy = new wfePolicy(sqlCon, ctrlPolicy);
            if (GetPolicyStatus() != (int)eSTATES.POLICY_ON_HOLD)
            {
                EnableDisbleControls(true);
            }
            else
            {
                EnableDisbleControls(false);
            }

            //ClearPicBox();
            //BoxDtls.SetCurrentSelection(imageLst.SelectedIndex);
            //ClearSelection();
            MoveNext();
            //GetIndexDetails(policyNo);
            ShowImage(false);
            //ChangeSize();
            //DisplayDocTypeCount();
        }
        public bool MoveNext()
        {
            if (lstImage.Items.Count > 0)
            {
                indexCount = indexCount + 1;
                if ((lstImage.Items.Count - 1) < indexCount)
                {
                    indexCount = 0;
                    if ((lstPolicy.Items.Count - 1) > (lstPolicy.SelectedIndex))
                    {
                        lstPolicy.SelectedIndex = lstPolicy.SelectedIndex + 1;
                    }
                    if (lstImage.Items.Count > 0)
                    {
                        //ClearSelection();
                        lstImage.SelectedIndex = indexCount;
                    }
                    //lstImage.EnsureVisible(indexCount);
                }
                else
                {
                    //					lstImage.Items[indexCount].Selected=true;
                    //					lstImage.EnsureVisible(indexCount);
                    //ClearSelection();
                    lstImage.SelectedIndex = indexCount;
                }
            }
            else
            {
                if ((lstPolicy.Items.Count - 1) > (lstPolicy.SelectedIndex))
                {
                    lstPolicy.SelectedIndex = lstPolicy.SelectedIndex + 1;
                }
            }
            return true;
        }

        private void pictureControl_MouseUp(object sender, MouseEventArgs e)
        {
            cropWidth = Math.Abs(e.X - cropX);
            cropHeight = Math.Abs(e.Y - cropY);


            Cursor = Cursors.Default;
            if (OperationInProgress == ihConstants._CROP || (OperationInProgress == ihConstants._CLEAN))
            {
                //Create the rectangle on which to operate
                if ((cropWidth > 1) && (e.Button == MouseButtons.Left))
                {
                    //Works both ways
                    Rectangle rect = new Rectangle(((cropX > e.X) ? e.X : cropX), ((cropY > e.Y) ? e.Y : cropY), cropWidth, cropHeight);
                    if (OperationInProgress == ihConstants._CROP)
                    {
                        Crop(rect);
                    }
                    if (OperationInProgress == ihConstants._CLEAN)
                    {
                        Clean(rect);
                    }
                }
                else
                    Cursor = Cursors.Default;
            }

            if (e.Button == MouseButtons.Right)
            {
                Cursor = Cursors.Default;
                OperationInProgress = ihConstants._NO_OPERATION;
            }

        }
        int Crop(Rectangle rect)
        {
            Bitmap bmpImage = new Bitmap(pictureControl.Image);
            double htRatio = 0;
            double wdRatio = 0;

            try
            {
                if (img.IsValid() == true)
                {
                    bmpImage = img.GetBitmap();

                    htRatio = Convert.ToDouble(bmpImage.Size.Height) / Convert.ToDouble(pictureControl.Height);
                    rect.Height = Convert.ToInt32(Convert.ToDouble(rect.Height) * htRatio);
                    wdRatio = (Convert.ToDouble(bmpImage.Size.Width) / Convert.ToDouble(pictureControl.Width));
                    rect.Width = Convert.ToInt32(Convert.ToDouble(rect.Width) * wdRatio);

                    rect.X = Convert.ToInt32(rect.X * wdRatio);
                    rect.Y = Convert.ToInt32(rect.Y * htRatio);

                    img.Crop(rect);
                    img.SaveFile(imgFileName);
                    img.LoadBitmapFromFile(imgFileName);
                }
                ChangeSize();

                System.IO.FileInfo info = new System.IO.FileInfo(imgFileName);
                fileSize = info.Length;
                fileSize = fileSize / 1024;
                //lblImageSize = (Label)BoxDtls.Controls["lblImageSize"];
                lblImgSize.Text = fileSize.ToString() + " KB";
                UpdateImageSize(fileSize);
            }

            catch (Exception ex)
            {
                MessageBox.Show("Error while cropping the image", "Crop Error");
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes("Project-" + wBox.ctrlBox.ProjectCode + " ,Batch-" + wBox.ctrlBox.BatchKey + " ,Box-" + wBox.ctrlBox.BoxNumber.ToString() + "Image name-" + imageName + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex);
            }
            return 0;
        }
        int Clean(Rectangle rect)
        {
            //Rectangle rect  = new Rectangle(e.X,e.Y, 6,6);

            double htRatio = 0;
            double wdRatio = 0;
            long fileSize;
            try
            {
                if (img.IsValid() == true)
                {
                    Bitmap bmpImage = img.GetBitmap();

                    htRatio = Convert.ToDouble(bmpImage.Size.Height) / Convert.ToDouble(pictureControl.Height);
                    rect.Height = Convert.ToInt32(Convert.ToDouble(rect.Height) * htRatio);
                    wdRatio = (Convert.ToDouble(bmpImage.Size.Width) / Convert.ToDouble(pictureControl.Width));
                    rect.Width = Convert.ToInt32(Convert.ToDouble(rect.Width) * wdRatio);

                    rect.X = Convert.ToInt32(rect.X * wdRatio);
                    rect.Y = Convert.ToInt32(rect.Y * htRatio);
                    if (rect.X <= 0)
                    {
                        rect.Width = rect.Width + rect.X;
                        rect.X = 0;
                    }
                    if (rect.X + rect.Width >= rect.Width)
                    {
                        rect.Width = rect.Width;
                    }
                    if (rect.Y <= 0)
                    {
                        rect.Height = rect.Height + rect.Y;
                        rect.Y = 0;
                    }
                    if (rect.Y + rect.Height >= rect.Height)
                    {
                        rect.Height = rect.Height;
                    }
                    //MessageBox.Show("Before filling " + gdi.GetBitDepth(imageNo).ToString());
                    img.Clean(rect);
                    img.SaveFile(imgFileName);
                    img.LoadBitmapFromFile(imgFileName);
                }
                //Change the size of the image in relation to the canvas
                ChangeSize();

                System.IO.FileInfo info = new System.IO.FileInfo(imgFileName);
                fileSize = info.Length;
                fileSize = fileSize / 1024;
                //lblImageSize = (Label)BoxDtls.Controls["lblImageSize"];
                lblImgSize.Text = fileSize.ToString() + " KB";
                UpdateImageSize(fileSize);
                //bmpCrop.Dispose();
            }
            catch (Exception ex)
            {
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes("Project-" + wBox.ctrlBox.ProjectCode + " ,Batch-" + wBox.ctrlBox.BatchKey + " ,Box-" + wBox.ctrlBox.BoxNumber.ToString() + "Image name-" + imgFileName + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex);
                MessageBox.Show("Error while cleaning the image" + ex.Message, "Crop error");
                return 0;
            }
            return 1;
        }	
        private void pictureControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (OperationInProgress == ihConstants._CROP)
            {
                Cursor = Cursors.Cross;
                if ((pictureControl.Image != null) && (cropPen != null))
                {
                    if (e.Button == MouseButtons.Left)
                    {
                        pictureControl.Refresh();
                        cropWidth = Math.Abs(e.X - cropX);
                        cropHeight = Math.Abs(e.Y - cropY);
                        pictureControl.CreateGraphics().DrawRectangle(cropPen, ((cropX > e.X) ? e.X : cropX), ((cropY > e.Y) ? e.Y : cropY), cropWidth, cropHeight);
                    }
                }
            }
            if (OperationInProgress == ihConstants._CLEAN)
            {
                if ((pictureControl.Image != null))
                {
                    if (e.Button == MouseButtons.Left)
                    {
                        Cursor = Cursors.Cross;
                        pictureControl.Refresh();
                        cropWidth = Math.Abs(e.X - cropX);
                        cropHeight = Math.Abs(e.Y - cropY);
                        pictureControl.CreateGraphics().DrawRectangle(cropPen, ((cropX > e.X) ? e.X : cropX), ((cropY > e.Y) ? e.Y : cropY), cropWidth, cropHeight);
                    }
                }
            }
            if (e.Button == MouseButtons.Right)
            {
                Cursor = Cursors.Default;
                OperationInProgress = ihConstants._NO_OPERATION;
            }
        }

        private void pictureControl_MouseDown(object sender, MouseEventArgs e)
        {
            if (OperationInProgress == ihConstants._CROP)
            {
                if (e.Button == MouseButtons.Left)
                {

                    cropX = e.X;
                    cropY = e.Y;

                    cropPen = new Pen(cropPenColor, cropPenSize);
                    cropPen.DashStyle = DashStyle.Solid;
                    Cursor = Cursors.Cross;
                    pictureControl.Refresh();
                }
            }
            if (OperationInProgress == ihConstants._CLEAN)
            {
                if (e.Button == MouseButtons.Left)
                {
                    cropX = e.X;
                    cropY = e.Y;

                    //MessageBox.Show("X-" + cropX + "Y-" + cropY);
                    cropPen = new Pen(cropPenColor, cropPenSize);
                    cropPen.DashStyle = DashStyle.Solid;
                    Cursor = Cursors.Cross;
                    pictureControl.Refresh();
                }
            }
            if (e.Button == MouseButtons.Right)
            {
                OperationInProgress = ihConstants._NO_OPERATION;
                Cursor = Cursors.No;
                Cursor = Cursors.Default;
                pictureControl.Cursor = Cursors.Default;
            }
        }
        private int GetDocTypePos()
        {
            string currntDoc;
            int index = 0;
            string srchStr;
            imageLst = lstImage;
            for (int i = 0; i < lvwDockTypes.Items.Count; i++)
            {
                if (lvwDockTypes.Items[i].Selected == true)
                {
                    currntDoc = lvwDockTypes.Items[i].SubItems[0].Text;
                    for (int j = 0; j < imageLst.Items.Count; j++)
                    {
                        srchStr = imageLst.Items[j].ToString();
                        if (srchStr.IndexOf(currntDoc) > 0)
                        {
                            index = j;
                            break;
                        }
                    }
                    break;
                }
            }
            return index;
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
           
        }

        private void pictureBox1_DoubleClick(object sender, EventArgs e)
        {
            //Bitmap bmp;
            //picBig.Image = null;
            imageLst = lstImage;
            string imageName = imageLst.SelectedItem.ToString();
            if (imageName != null)
            {
                if (imageName.Length >= 1)
                {
                    //ThumbnailChangeSize(pictureBox1.Tag.ToString());

                    int lstIndex;
                    lstIndex = (currntPg * 6) + 0 + GetDocTypePos();

                    if (lstIndex < imageLst.Items.Count)
                    {
                        imageLst.SelectedIndex = lstIndex;
                        ShowImage(true);
                    }
                    tabControl2.SelectedIndex = 0;
                    lvwDockTypes.Focus();
                    //picBig.Visible = true;
                    //panelBig.Visible = true;
                }
            }
        }

        private void pictureBox2_DoubleClick(object sender, EventArgs e)
        {
            imageLst = lstImage;
            string imageName = imageLst.SelectedItem.ToString();
            if (imageName != null)
            {
                if (imageName.Length >= 2)
                {

                    //ThumbnailChangeSize(pictureBox2.Tag.ToString());
                    int lstIndex;
                    lstIndex = (currntPg * 6) + 1 + GetDocTypePos();
                    if (lstIndex < imageLst.Items.Count)
                    {
                        imageLst.SelectedIndex = lstIndex;
                        ShowImage(true);
                    }
                    tabControl2.SelectedIndex = 0;
                    lvwDockTypes.Focus();
                    //picBig.Visible = true;
                    //panelBig.Visible = true;
                }
            }
        }

        private void pictureBox3_DoubleClick(object sender, EventArgs e)
        {
            imageLst = lstImage;
            string imageName = imageLst.SelectedItem.ToString();
            if (imageName != null)
            {
                if (imageName.Length >= 3)
                {

                    //ThumbnailChangeSize(pictureBox3.Tag.ToString());
                    int lstIndex;
                    lstIndex = (currntPg * 6) + 2 + GetDocTypePos();
                    if (lstIndex < imageLst.Items.Count)
                    {
                        imageLst.SelectedIndex = lstIndex;
                        ShowImage(true);
                    }
                    tabControl2.SelectedIndex = 0;
                    lvwDockTypes.Focus();
                    //picBig.Visible = true;
                    //panelBig.Visible = true;
                }
            }
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            
        }

        private void pictureBox4_DoubleClick(object sender, EventArgs e)
        {
            //Bitmap bmp;
            //picBig.Image = null;
            imageLst = lstImage;
            string imageName = imageLst.SelectedItem.ToString();
            if (imageName != null)
            {
                if (imageName.Length >= 4)
                {

                    //ThumbnailChangeSize(pictureBox4.Tag.ToString());
                    int lstIndex;
                    lstIndex = (currntPg * 6) + 3 + GetDocTypePos();
                    if (lstIndex < imageLst.Items.Count)
                    {
                        imageLst.SelectedIndex = lstIndex;
                        ShowImage(true);
                    }
                    tabControl2.SelectedIndex = 0;
                    lvwDockTypes.Focus();
                    //picBig.Visible = true;
                    //panelBig.Visible = true;
                }
            }
        }

        private void pictureBox5_DoubleClick(object sender, EventArgs e)
        {
            imageLst = lstImage;
            string imageName = imageLst.SelectedItem.ToString();
            if (imageName != null)
            {
                if (imageName.Length >= 5)
                {

                    //ThumbnailChangeSize(pictureBox5.Tag.ToString());
                    int lstIndex;
                    lstIndex = (currntPg * 6) + 4 + GetDocTypePos();
                    if (lstIndex < imageLst.Items.Count)
                    {
                        imageLst.SelectedIndex = lstIndex;
                        ShowImage(true);
                    }
                    tabControl2.SelectedIndex = 0;
                    lvwDockTypes.Focus();
                    //picBig.Visible = true;
                    //panelBig.Visible = true;
                }
            }
        }

        private void pictureBox6_DoubleClick(object sender, EventArgs e)
        {
            //Bitmap bmp;
            //picBig.Image = null;
            imageLst = lstImage;
            string imageName = imageLst.SelectedItem.ToString();
            if (imageName != null)
            {
                if (imageName.Length >= 6)
                {

                    //ThumbnailChangeSize(pictureBox6.Tag.ToString());
                    int lstIndex;
                    lstIndex = (currntPg * 6) + 5 + GetDocTypePos();
                    if (lstIndex < imageLst.Items.Count)
                    {
                        imageLst.SelectedIndex = lstIndex;
                        ShowImage(true);
                    }
                    tabControl2.SelectedIndex = 0;
                    lvwDockTypes.Focus();
                    //picBig.Visible = true;
                    //panelBig.Visible = true;
                }
            }
        }

        private void lnkPage1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string imageFileName;
            Image imgNew = null;
            tabControl2.SelectedIndex = 1;

            System.Drawing.Image imgThumbNail = null;
            ClearPicBox();
            imageLst = lstImage;
            imageName = new String[lstImage.Items.Count];
            for (int i = 0; i < imageName.Length; i++)
            {
                imageName[i] = lstImage.Items[i].ToString();
                imageFileName = policyPath+"\\"+ihConstants._QC_FOLDER+"\\"+ imageName[i].Remove(imageName[i].IndexOf("."),4)+".TIF";
                if (!System.IO.File.Exists(imageFileName)) return;
                imgAll.LoadBitmapFromFile(imageFileName);

                if (imgAll.GetBitmap().PixelFormat == PixelFormat.Format24bppRgb)
                {
                    try
                    {
                        imgAll.GetLZW("tmp.TIF");
                        imgNew = Image.FromFile("tmp.TIF");
                        imgThumbNail = imgNew;
                    }
                    catch (Exception ex)
                    {
                        string err = ex.Message;
                    }
                }
                else
                {
                    imgThumbNail = System.Drawing.Image.FromFile(imageFileName);
                }
                double scaleX = (double)pictureBox1.Width / (double)imgThumbNail.Width;
                double scaleY = (double)pictureBox1.Height / (double)imgThumbNail.Height;
                double Scale = Math.Min(scaleX, scaleY);
                int w = (int)(imgThumbNail.Width * Scale);
                int h = (int)(imgThumbNail.Height * Scale);
                w = w - 5;
                imgThumbNail = CreateThumbnail(imgThumbNail, w, h); //imgThumbNail.GetThumbnailImage(w, h, new System.Drawing.Image.GetThumbnailImageAbort(GetThumbnailImageAbort), IntPtr.Zero);
                currntPg = 0;
                if (i == 0)
                {
                    pictureBox1.Image = imgThumbNail;
                    pictureBox1.Tag = imageFileName;
                }
                if (i == 1)
                {
                    pictureBox2.Image = imgThumbNail;
                    pictureBox2.Tag = imageFileName;
                }
                if (i == 2)
                {
                    pictureBox3.Image = imgThumbNail;
                    pictureBox3.Tag = imageFileName;
                }
                if (i == 3)
                {
                    pictureBox4.Image = imgThumbNail;
                    pictureBox4.Tag = imageFileName;
                }
                if (i == 4)
                {
                    pictureBox5.Image = imgThumbNail;
                    pictureBox5.Tag = imageFileName;
                }
                if (i == 5)
                {
                    pictureBox6.Image = imgThumbNail;
                    pictureBox6.Tag = imageFileName;
                }
                if (imgNew != null)
                {
                    imgNew.Dispose();
                    imgNew = null;
                    if (File.Exists("tmp.tif"))
                        File.Delete("tmp.TIF");
                }
            }
        }

        private void lnkPage2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string imageFileName;
            Image imgNew = null;
            tabControl2.SelectedIndex = 1;

            System.Drawing.Image imgThumbNail = null;
            ClearPicBox();
            imageLst = lstImage;
            imageName = new String[lstImage.Items.Count];
            for (int i = 6; i < imageName.Length; i++)
            {
                imageName[i] = lstImage.Items[i].ToString();
                imageFileName = policyPath + "\\" + ihConstants._QC_FOLDER + "\\" + imageName[i].Remove(imageName[i].IndexOf("."), 4) + ".TIF";
                //imageFileName = imageName[i];
                if (!System.IO.File.Exists(imageFileName)) return;
                imgAll.LoadBitmapFromFile(imageFileName);
                currntPg = 1;
                if (imgAll.GetBitmap().PixelFormat == PixelFormat.Format24bppRgb)
                {
                    try
                    {
                        imgAll.GetLZW("tmp.TIF");
                        imgNew = Image.FromFile("tmp.TIF");
                        imgThumbNail = imgNew;
                    }
                    catch (Exception ex)
                    {
                        string err = ex.Message;
                    }
                }
                else
                {
                    imgThumbNail = System.Drawing.Image.FromFile(imageFileName);
                }
                double scaleX = (double)pictureBox1.Width / (double)imgThumbNail.Width;
                double scaleY = (double)pictureBox1.Height / (double)imgThumbNail.Height;
                double Scale = Math.Min(scaleX, scaleY);
                int w = (int)(imgThumbNail.Width * Scale);
                int h = (int)(imgThumbNail.Height * Scale);
                w = w - 5;
                imgThumbNail = CreateThumbnail(imgThumbNail, w, h); //imgThumbNail.GetThumbnailImage(w, h, new System.Drawing.Image.GetThumbnailImageAbort(GetThumbnailImageAbort), IntPtr.Zero);

                if (i == 6)
                {
                    pictureBox1.Image = imgThumbNail;
                    pictureBox1.Tag = imageFileName;
                }
                if (i == 7)
                {
                    pictureBox2.Image = imgThumbNail;
                    pictureBox2.Tag = imageFileName;
                }
                if (i == 8)
                {
                    pictureBox3.Image = imgThumbNail;
                    pictureBox3.Tag = imageFileName;
                }
                if (i == 9)
                {
                    pictureBox4.Image = imgThumbNail;
                    pictureBox4.Tag = imageFileName;
                }
                if (i == 10)
                {
                    pictureBox5.Image = imgThumbNail;
                    pictureBox5.Tag = imageFileName;
                }
                if (i == 11)
                {
                    pictureBox6.Image = imgThumbNail;
                    pictureBox6.Tag = imageFileName;
                }
                if (imgNew != null)
                {
                    imgNew.Dispose();
                    imgNew = null;
                    if (File.Exists("tmp.tif"))
                        File.Delete("tmp.TIF");
                }
            }
        }

        private void lnkPage3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string imageFileName;
            Image imgNew = null;
            tabControl2.SelectedIndex = 1;

            System.Drawing.Image imgThumbNail = null;
            ClearPicBox();
            imageLst = lstImage;
            imageName = new String[lstImage.Items.Count];
            for (int i = 0; i < imageName.Length; i++)
            {
                imageName[i] = lstImage.Items[i].ToString();
                imageFileName = policyPath + "\\" + ihConstants._QC_FOLDER + "\\" + imageName[i].Remove(imageName[i].IndexOf("."), 4) + ".TIF";
                //imageFileName = imageName[i];
                if (!System.IO.File.Exists(imageFileName)) return;
                imgAll.LoadBitmapFromFile(imageFileName);
                currntPg = 2;
                if (imgAll.GetBitmap().PixelFormat == PixelFormat.Format24bppRgb)
                {
                    try
                    {
                        imgAll.GetLZW("tmp.TIF");
                        imgNew = Image.FromFile("tmp.TIF");
                        imgThumbNail = imgNew;
                    }
                    catch (Exception ex)
                    {
                        string err = ex.Message;
                    }
                }
                else
                {
                    imgThumbNail = System.Drawing.Image.FromFile(imageFileName);
                }
                double scaleX = (double)pictureBox1.Width / (double)imgThumbNail.Width;
                double scaleY = (double)pictureBox1.Height / (double)imgThumbNail.Height;
                double Scale = Math.Min(scaleX, scaleY);
                int w = (int)(imgThumbNail.Width * Scale);
                int h = (int)(imgThumbNail.Height * Scale);
                w = w - 5;
                imgThumbNail = CreateThumbnail(imgThumbNail, w, h); //imgThumbNail.GetThumbnailImage(w, h, new System.Drawing.Image.GetThumbnailImageAbort(GetThumbnailImageAbort), IntPtr.Zero);

                if (i == 12)
                {
                    pictureBox1.Image = imgThumbNail;
                    pictureBox1.Tag = imageFileName;
                }
                if (i == 13)
                {
                    pictureBox2.Image = imgThumbNail;
                    pictureBox2.Tag = imageFileName;
                }
                if (i == 14)
                {
                    pictureBox3.Image = imgThumbNail;
                    pictureBox3.Tag = imageFileName;
                }
                if (imgNew != null)
                {
                    imgNew.Dispose();
                    imgNew = null;
                    if (File.Exists("tmp.tif"))
                        File.Delete("tmp.TIF");
                }
            }
        }

        void ClearPicBox()
        {
            pictureBox1.Image = null;
            pictureBox2.Image = null;
            pictureBox3.Image = null;
            pictureBox4.Image = null;
            pictureBox5.Image = null;
            pictureBox6.Image = null;
        }

        private int GetKeyVal(string key)
        {
            int cropKeyVal = 0;

            foreach (char c in key)
            {
                cropKeyVal = (int)c;
            }
            return cropKeyVal;
        }

        private void aeFQC_KeyUp(object sender, KeyEventArgs e)
        {
            int cropKeyVal = GetKeyVal(cropKey.ToUpper());
            int zoomInKeyVal = GetKeyVal(zoomInKey.ToUpper());
            int zoomOutKeyVal = GetKeyVal(zoomOutKey.ToUpper());
            int autoCropKeyVal = GetKeyVal(autoCropKey.ToUpper());
            int rotateRKeyVal = GetKeyVal(rotateRKey.ToUpper());
            int rotateLKeyVal = GetKeyVal(rotateLKey.ToUpper());
            int skewRKeyVal = GetKeyVal(skewRKey.ToUpper());
            int skewLKeyVal = GetKeyVal(skewLKey.ToUpper());
            int noiseRemovalLKeyVal = GetKeyVal(noiseRemovalLKey.ToUpper());
            int cleanKeyVal = GetKeyVal(cleanKey.ToUpper());
            imageLst = lstImage;
            policyLst = lstPolicy;
            if ((int)e.KeyData == cropKeyVal)
            {
                OperationInProgress = ihConstants._CROP;
            }
            if ((int)e.KeyData == (zoomInKeyVal + 64))
            {
                ZoomIn();
            }
            if ((int)e.KeyData == (zoomOutKeyVal))
            {
                ZoomOut();
            }

            if ((int)e.KeyData == (autoCropKeyVal))
            {
                AutoCrop();
            }

            if ((int)e.KeyData == (rotateRKeyVal))
            {
                RotateRight();
            }
            if ((int)e.KeyData == (rotateLKeyVal))
            {
                RotateLeft();
            }

            if ((int)e.KeyData == (skewRKeyVal))
            {
                SkewRight();
            }
            if ((int)e.KeyData == (skewLKeyVal))
            {
                SkewLeft();
            }

            if ((int)e.KeyData == (noiseRemovalLKeyVal))
            {
                NoiseRemove();
            }
            if ((int)e.KeyData == (cleanKeyVal))
            {
                OperationInProgress = ihConstants._CLEAN;
            }
            if (e.KeyCode == Keys.Escape)
            {
                OperationInProgress = ihConstants._NO_OPERATION;
                Cursor = Cursors.No;
                Cursor = Cursors.Default;
                pictureControl.Cursor = Cursors.Default;
            }
            if (e.KeyCode == Keys.F11)
            {
                MoveUp();
            }
            if (e.KeyCode == Keys.F12)
            {
                MoveDown();
            }
            if (e.KeyCode == Keys.Right)
            {
                MoveNext();
                ShowImage(false);
                //ChangeSize();
                DisplayDocTypeCount();
            }
            if (e.KeyCode == Keys.Left)
            {
                MovePrevious();
                ShowImage(false);
                //ChangeSize();
            }
            if (e.KeyCode == Keys.Up)
            {
                //BoxDtls.MovePrevious();
                ShowImage(false);
                ChangeSize();
            }
            if (e.KeyCode == Keys.Down)
            {
                //BoxDtls.MoveNext();
                ShowImage(false);
                ChangeSize();
                //DisplayDocTypeCount();
            }
            if (e.KeyCode == Keys.F8)
            {
                Background();
            }
        }
        public bool MoveUp()
        {
            ArrayList arr = new ArrayList();
            CtrlPolicy ctPolicy = new CtrlPolicy(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), boxNumber, lstPolicy.SelectedItem.ToString());
            wfePolicy wp = new wfePolicy(sqlCon, ctPolicy);
            //Can only move up when there are records in the list box of images
            //and the currently selected element is greater than 0
            if (lstImage.Items.Count > 0 && lstImage.SelectedIndex > 0)
            {
                indexCount = indexCount - 1;
                object swp = lstImage.Items[lstImage.SelectedIndex];
                lstImage.Items[lstImage.SelectedIndex] = lstImage.Items[lstImage.SelectedIndex - 1]; ;
                lstImage.Items[lstImage.SelectedIndex - 1] = swp;
                lstImage.SelectedIndex = lstImage.SelectedIndex - 1;
                for (int i = 0; i < lstImage.Items.Count; i++)
                {
                    arr.Add(lstImage.Items[i]);
                }
                wp.UpdateSrl(arr);
            }

            return true;
        }
        int Background()
        {
            try
            {
                if (img.IsValid() == true)
                {
                    //Auto Crop
                    img.BackGround();
                    //Call the save routine
                    img.SaveFile(imgFileName);
                    //Show the image back in picture box
                    img.LoadBitmapFromFile(imgFileName);
                    ChangeSize();
                }
                ChangeSize();
                System.IO.FileInfo info = new System.IO.FileInfo(imgFileName);
                fileSize = info.Length;
                fileSize = fileSize / 1024;
                //lblImageSize = (Label)BoxDtls.Controls["lblImageSize"];
                lblImgSize.Text = fileSize.ToString() + " KB";
                UpdateImageSize(fileSize);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while auto cropping the image", "Auto Crop Error");
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes("Project-" + wBox.ctrlBox.ProjectCode + " ,Batch-" + wBox.ctrlBox.BatchKey + " ,Box-" + wBox.ctrlBox.BoxNumber.ToString() + "Image name-" + imgFileName + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex);
                error = ex.Message;
            }
            return 0;
        }
        public bool MoveDown()
        {
            ArrayList arr = new ArrayList();
            CtrlPolicy ctPolicy = new CtrlPolicy(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), boxNumber, lstPolicy.SelectedItem.ToString());
            wfePolicy wp = new wfePolicy(sqlCon, ctPolicy);
            //Can only move up when there are records in the list box of images
            //and the currently selected element is less than count of items-1
            if (lstImage.Items.Count > 0 && lstImage.SelectedIndex < (lstImage.Items.Count - 1))
            {
                string swp = lstImage.SelectedItem.ToString();
                lstImage.Items[lstImage.SelectedIndex] = lstImage.Items[lstImage.SelectedIndex + 1];
                lstImage.Items[lstImage.SelectedIndex + 1] = swp;
                lstImage.SelectedIndex = lstImage.SelectedIndex + 1;
                indexCount = indexCount + 1;
                for (int i = 0; i < lstImage.Items.Count; i++)
                {
                    arr.Add(lstImage.Items[i]);
                }
                wp.UpdateSrl(arr);

            }
            return true;
        }

        private void aeFQC_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                //if (dg != null)
                //{
                //    dg.Close();
                //}
            }
            //    if (picBig.Visible == true)
            //    {
            //        picBig.Visible = false;
            //        panelBig.Visible = false;
            //        picBig.Image = null;
            //    }
            //}
            ///For checking todays production count
            if ((e.KeyCode == Keys.F9))
            {
                wfePolicy wPolicy = new wfePolicy(sqlCon);
                int count = wPolicy.GetTransactionLogCount(bundleKey, dbcon.GetCurrenctDTTM(2, sqlCon), crd.created_by, eSTATES.POLICY_FQC);
                frmProductionCount frmProd = new frmProductionCount(count);
                frmProd.ShowDialog(this);
            }
            if ((e.KeyCode == Keys.L) && (e.Control))
            {
                ctrlPolicy = new CtrlPolicy(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), boxNumber, policyLst.SelectedItem.ToString());
                policy = new wfePolicy(sqlCon, ctrlPolicy);
                DataSet pDs = policy.GetPolicyLog();
                if (pDs.Tables.Count > 0)
                {
                    if (pDs.Tables[0].Rows.Count > 0)
                    {
                        frmPolicyLog log = new frmPolicyLog(pDs, ctrlPolicy.PolicyNumber.ToString());
                        log.Show();
                    }
                }
                else
                {
                    MessageBox.Show("No log information available for this policy....");
                }
            }
            if ((e.KeyCode == Keys.C) && (e.Control))
            {
                string imageName;
                policyLst = lstPolicy;
                imageLst = lstImage;
                int pos = imageLst.SelectedItem.ToString().IndexOf("-");
                if (pos > 0)
                {
                    imageName = imageLst.SelectedItem.ToString().Substring(0, pos);
                }
                else
                {
                    imageName = imageLst.SelectedItem.ToString();
                }
                pImage = new CtrlImage(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), boxNumber, policyLst.SelectedItem.ToString(), imageName, string.Empty);
                wfeImage wImage = new wfeImage(sqlCon, pImage);
                aeCustomExp frmExp = new aeCustomExp(wImage, sqlCon, crd);
                frmExp.ShowDialog(this);
            }
            if ((e.KeyCode == Keys.Z) && (e.Control))
            {
                ImageCopy();
            }
        }

        private void cmdDeedDetails_Click(object sender, EventArgs e)
        {
            string patientID = lstPolicy.SelectedItem.ToString();

            //EntryForm frm = new EntryForm(sqlCon, projKey, bundleKey, _mode, patientID);
            //frm.ShowDialog(this);

            fmEntry fm = new fmEntry(sqlCon, projKey, bundleKey, _mode, patientID);
            fm.ShowDialog(this);

        }

        private void prmButtonCrop_Click(object sender, EventArgs e)
        {
            CropRegister();
        }

        private void prmButtonImportImage_Click(object sender, EventArgs e)
        {
            ImportImageFromDir();
        }

        private void prmButtonAutoCrp_Click(object sender, EventArgs e)
        {
            AutoCrop();
        }

        private void prmButtonRotateRight_Click(object sender, EventArgs e)
        {
            RotateRight();
        }

        private void prmButtonRotateLeft_Click(object sender, EventArgs e)
        {
            RotateLeft();
        }

        private void prmButtonZoomIn_Click(object sender, EventArgs e)
        {
            ZoomIn();
        }

        private void prmButtonZoomOut_Click(object sender, EventArgs e)
        {
            ZoomOut();
        }

        private void prmButtonSkewRight_Click(object sender, EventArgs e)
        {
            SkewRight();
        }

        private void prmButtonNoiseRemove_Click(object sender, EventArgs e)
        {
            NoiseRemove();
        }

        private void prmButtonCleanImg_Click(object sender, EventArgs e)
        {
            CleanImageRegister();
        }

        private void prmButtonCopyImage_Click(object sender, EventArgs e)
        {
            ImageCopy();
        }

        private void prmButtonDelImage_Click(object sender, EventArgs e)
        {
            ImageDelete();
        }

        private void prmButtonRescan_Click(object sender, EventArgs e)
        {
            RescanImage();
        }

        private void prmButtonScan_Click(object sender, EventArgs e)
        {
            ScanImage();
        }

        private void prmPhotoCrop_Click(object sender, EventArgs e)
        {
            PhotoCropRegister();
        }

        private void prmEndPhotoCrop_Click(object sender, EventArgs e)
        {
            EndPhotoCrop();
        }

        private void prmGetPhoto_Click(object sender, EventArgs e)
        {
            GetPhoto();
        }

        int PhotoCropRegister()
        {
            delinsrtBol = false;
            imageLst = lstImage;
            if ((hasPhotoBol == true) && ((imageLst.SelectedIndex + 1) == ihConstants._PHOTO_PAGE_POSITION))
            {
                if (File.Exists(qcFolderName + "\\" + "1_" + imageLst.SelectedItem.ToString()) == false)
                {
                    File.Copy(sourceFilePath + "\\" + imageLst.SelectedItem.ToString(), qcFolderName + "\\" + "1_" + imageLst.SelectedItem.ToString());
                }
                imgFileName = qcFolderName + "\\" + "1_" + imageLst.SelectedItem.ToString();

                img.LoadBitmapFromFile(imgFileName);
                //pictureControl.Image = img.GetBitmap();

                ChangeSize();
                OperationInProgress = ihConstants._PHOTO_CROP;
                prmButtonCrop.Enabled = false;
                prmButtonAutoCrp.Enabled = false;
                prmButtonRotateRight.Enabled = false;
                prmButtonRotateLeft.Enabled = false;
                prmButtonZoomIn.Enabled = false;
                prmButtonZoomOut.Enabled = false;
                prmButtonSkewRight.Enabled = false;
                prmButtonSkewLeft.Enabled = false;
                prmButtonNoiseRemove.Enabled = false;
                prmButtonCleanImg.Enabled = false;
                prmButtonCopyImage.Enabled = false;
                prmButtonDelImage.Enabled = false;
                prmButtonRescan.Enabled = false;
                prmGetPhoto.Enabled = false;
                //prmButtonSkewRight.Enabled = true;
            }
            //ChangeSize();
            return 0;
        }
        int EndPhotoCrop()
        {
            string photoName;
            wfeImage wImg;
            long fileSize;

            try
            {
                System.IO.FileInfo info = null;
                imgFileName = qcFolderName + "\\" + imageLst.SelectedItem.ToString().Substring(0, policyLen) + "_000_A.TIF";
                if (File.Exists(imgFileName))
                {
                    if (File.Exists(qcFolderName + "\\" + "1_" + imageLst.SelectedItem.ToString()))
                    {
                        File.Delete(qcFolderName + "\\" + "1_" + imageLst.SelectedItem.ToString());
                    }
                    OperationInProgress = ihConstants._NO_OPERATION;
                    if (photoCropOperation == true)
                    {
                        info = new System.IO.FileInfo(imgFileName);
                        fileSize = info.Length;
                        fileSize = fileSize / 1024;

                        policyLst = lstPolicy;
                        imageLst = lstImage;
                        photoName = imageLst.SelectedItem.ToString().Substring(0, policyLen) + "_000_A.TIF";

                        pImage = new CtrlImage(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), boxNumber, policyLst.SelectedItem.ToString(), photoName, string.Empty);

                        wImg = new wfeImage(sqlCon, pImage);
                        if (wImg.DeletePage() == true)
                        {
                            wImg.Save(crd, eSTATES.PAGE_QC, fileSize, ihConstants._PHOTO_PAGE, 0, photoName);
                        }
                        //Save the colour photograph into 000
                        //img.SaveAsTiff(imgFileName,IGRComressionTIFF.JPEG);
                    }
                    //Now load the black and white image	
                }
                imgFileName = qcFolderName + "\\" + imageLst.SelectedItem.ToString();

                ChangeSize(imgFileName);
                if (File.Exists(qcFolderName + "\\" + "1_" + imageLst.SelectedItem.ToString()))
                {
                    File.Delete(qcFolderName + "\\" + "1_" + imageLst.SelectedItem.ToString());
                }
                if (File.Exists(qcFolderName + "\\" + "0_" + imageLst.SelectedItem.ToString()))
                {
                    File.Delete(qcFolderName + "\\" + "0_" + imageLst.SelectedItem.ToString());
                }
                prmButtonCrop.Enabled = true;
                prmButtonAutoCrp.Enabled = true;
                prmButtonRotateRight.Enabled = true;
                prmButtonRotateLeft.Enabled = true;
                prmButtonZoomIn.Enabled = true;
                prmButtonZoomOut.Enabled = true;
                prmButtonSkewRight.Enabled = true;
                prmButtonSkewLeft.Enabled = true;
                prmButtonNoiseRemove.Enabled = true;
                prmButtonCleanImg.Enabled = true;
                prmButtonCopyImage.Enabled = true;
                prmButtonDelImage.Enabled = true;
                prmButtonRescan.Enabled = true;
                photoCropOperation = false;
                getPhotoOperation = false;
                prmPhotoCrop.Enabled = true;
                prmGetPhoto.Enabled = true;
                prmButtonSkewRight.Enabled = true;
                prmNext = cmdNext;
                prmPrevious = cmdPrevious;
                prmNext.Enabled = true;
                prmPrevious.Enabled = true;
                delinsrtBol = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            return 0;
        }
        int GetPhoto()
        {

            try
            {
                prmButtonRescan.Enabled = false;
                prmButtonCopyImage.Enabled = false;
                prmPhotoCrop.Enabled = false;
                //imageLst = (ListBox)BoxDtls.Controls["lstImage"];

                //img.UpdateImageSize(crd, eSTATES.PAGE_QC,fileSize);
                imgFileName = qcFolderName + "\\" + imageLst.SelectedItem.ToString().Substring(0, policyLen) + "_000_A.TIF";
                if (File.Exists(imgFileName))
                {
                    /*
                    if (ino > 0)
                        imgQc.ReleaseGdPictureImage(ino);
	    		
                    ino=imgQc.CreateGdPictureImageFromFile(imgFileName);
                    imgQc.SaveAsTIFF(ino,imgFileName,GdPicture.TiffCompression.TiffCompressionJPEG);
                    Bitmap newBmp = new Bitmap(pictureControl.Width,pictureControl.Height);
                    newBmp = imgQc.GetBitmapFromGdPictureImage(ino);                           
                    pictureControl.Image = newBmp;//imgQc.GetBitmapFromGdPictureImage(ino);
                    */
                    //img.LoadBitmapFromFile(imgFileName);
                    //img.SaveAsTiff(imgFileName, IGRComressionTIFF.JPEG);
                    ChangeSize(imgFileName);
                    //pictureControl.Image=img.GetBitmap();
                    getPhotoOperation = true;
                    prmNext = cmdNext;
                    prmPrevious = cmdPrevious;
                    prmNext.Enabled = false;
                    prmPrevious.Enabled = false;
                    //prmButtonSkewRight.Enabled = false;
                    prmButtonDelImage.Enabled = false;
                    delinsrtBol = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            return 0;
        }

        private void txtSearch_Enter(object sender, EventArgs e)
        {
            txtSearch.SelectAll();
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void txtSearch_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!string.IsNullOrEmpty(txtSearch.Text.ToUpper().Trim()))
                {
                    for (int i = 0; i < lstPolicy.Items.Count; i++)
                    {
                        if (lstPolicy.Items[i].ToString().Contains(txtSearch.Text.ToUpper().Trim()))
                        {
                            //lstPolicy.SelectedIndex = i;
                            lstPolicy.SetSelected(i, true);
                        }
                    }
                }
            }
        }
    }
}
