using System;
using System.Drawing;
using System.Windows.Forms;
using System.Data;
using System.Data.Odbc;
using NovaNet.Utils;
using NovaNet.wfe;
using LItems;
using System.Collections;
using System.Drawing.Imaging;
using System.IO;
using DataLayerDefs;
using System.Drawing.Drawing2D;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Linq;
//using AcroPDFLib;
using AxAcroPDFLib;


namespace ImageHeaven
{
    public partial class frmPDFView : Form
    {
        MemoryStream stateLog;
        byte[] tmpWrite;
        OdbcConnection sqlCon = null;
        NovaNet.Utils.dbCon dbcon = null;
        CtrlPolicy pPolicy = null;
        private CtrlImage pImage = null;
        wfePolicy wPolicy = null;
        wfeImage wImage = null;

        public string projKey;
        public string bundleKey;

        public string patientID;

        public string bundlepath = null;

        public string bundlecode;

        public string filename;

        public frmPDFView()
        {
            InitializeComponent();
        }

        public frmPDFView(OdbcConnection prmCon, Credentials prmCrd)
        {
            this.Name = "NRS PDF Viewer. . .";
            InitializeComponent();
            sqlCon = prmCon;
           
        }

        public DataTable getBundle(string proj, string bundle)
        {
            DataTable dt = new DataTable();

            string sql = "select bundle_path,bundle_code from bundle_master where proj_code = '"+proj+"' and bundle_key = '"+bundle+"' ";
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(dt);

            return dt;
        }

        private void frmPDFView_Load(object sender, EventArgs e)
        {
            this.Name = "NRS PDF Viewer. . .";

            projKey = frmSearch.projKey;
            bundleKey = frmSearch.bundleKey;
            patientID = frmSearch.patientID;

            bundlepath = getBundle(projKey, bundleKey).Rows[0][0].ToString();



            if (bundlepath != null || bundlepath != "")
            {
                bundlecode = getBundle(projKey, bundleKey).Rows[0][1].ToString();
                if(bundlecode != null || bundlecode != "")
                {
                    string exportfolder = string.Empty;

                    exportfolder = bundlepath + "\\" + "Export\\" + bundlecode;

                    if (exportfolder != null || exportfolder != "")
                    {
                        filename = exportfolder + "\\" + patientID + ".pdf";
                        string name = patientID + ".pdf";
                        axAcroPDF1.src = filename;
                        axAcroPDF1.Name = name;
                        axAcroPDF1.LoadFile(filename);
                    }
                }
                
            }
        }
    }
}
