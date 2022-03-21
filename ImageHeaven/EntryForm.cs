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
using nControls;

namespace ImageHeaven
{
    public partial class EntryForm : Form
    {

        public static int index;

        Credentials crd = new Credentials();

        string name = frmMain.name;
        OdbcConnection sqlCon = null;
        public static bool _modeBool;

        public static DataLayerDefs.Mode _mode = DataLayerDefs.Mode._Edit;

        public static string projKey;
        public static string bundleKey;

        public static string caseFileNo;

        public EntryForm()
        {
            InitializeComponent();
        }

        public EntryForm(OdbcConnection pCon,string projkey, string batchkey, DataLayerDefs.Mode mode, string casefileno)
        {
            InitializeComponent();
            sqlCon = pCon;

            projKey = projkey;
            bundleKey = batchkey;
            caseFileNo = casefileno;

            _mode = mode;
        }

        public EntryForm(OdbcConnection pCon, DataLayerDefs.Mode mode,string casefileno)
        {
            InitializeComponent();
            sqlCon = pCon;

            if (mode == DataLayerDefs.Mode._Add)
            {
                this.Text = "Record Management - Entry Details (Add)";
                projKey = Files.projKey;
                bundleKey = Files.bundleKey;

                caseFileNo = casefileno;

                _mode = mode;
            }

            if (mode == DataLayerDefs.Mode._Edit)
            {
                this.Text = "Record Management - Entry Details (Add)";
                projKey = Files.projKey;
                bundleKey = Files.bundleKey;

                caseFileNo = casefileno;

                _mode = mode;
            }
        }

        public DataTable _GetBundleDetails(string proj, string bundle)
        {
            DataTable dt = new DataTable();
            string sql = "select distinct proj_code, bundle_Key,establishment as Establishment, bundle_name as 'Bundle Name', Bundle_no as 'Bundle Number', date_format(handover_date,'%Y-%m-%d') as 'Handover Date' from bundle_master where proj_code = '" + proj + "' and bundle_key = '" + bundle + "' ";
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(dt);
            return dt;
        }

        public DataTable _GetFileCaseDetails(string proj, string bundle, string casefileno)
        {
            DataTable dt = new DataTable();
            string sql = "select distinct proj_code, bundle_Key,item_no,case_file_no,case_status, case_nature, case_type, case_year from case_file_master where proj_code = '" + proj + "' and bundle_key = '" + bundle + "' and case_file_no = '"+casefileno+"'";
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(dt);
            return dt;
        }

        public DataTable _GetMetaCount(string proj, string bundle, string casefileno)
        {
            DataTable dt = new DataTable();
            string sql = "select distinct proj_code, bundle_Key,item_no,case_file_no from metadata_entry where proj_code = '" + proj + "' and bundle_key = '" + bundle + "' and case_file_no = '" + casefileno + "'";
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(dt);
            return dt;
        }

        private void disablegroup1()
        {
            deTextBox1.Enabled = false;
            deTextBox2.Enabled = false;
            deTextBox3.Enabled = false;
            deTextBox4.Enabled = false;
        }

        private void disablegroup2()
        {
            deTextBox8.Enabled = false;
            deComboBox1.Enabled = false;
            deComboBox2.Enabled = false;
            deTextBox5.Enabled = false;
            deComboBox12.Enabled = false;
        }

        private void disableDateDisposal()
        {
            deTextBox6.Enabled = false;
            deTextBox7.Enabled = false;
            deTextBox9.Enabled = false;
        }

        private void enableDateDisposal()
        {
            deTextBox6.Enabled = true;
            deTextBox7.Enabled = true;
            deTextBox9.Enabled = true;
        }

        private void populateCasestatus()
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            string sql = "select case_status_id, case_status from case_status_master  ";

            OdbcDataAdapter odap = new OdbcDataAdapter(sql, sqlCon);
            odap.Fill(dt);


            if (dt.Rows.Count > 0)
            {
                deComboBox1.DataSource = dt;
                deComboBox1.DisplayMember = "case_status";
                deComboBox1.ValueMember = "case_status_id";
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
                deComboBox12.DataSource = dt;
                deComboBox12.DisplayMember = "case_nature";
                deComboBox12.ValueMember = "case_nature_id";
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
                deComboBox2.DataSource = dt;
                deComboBox2.DisplayMember = "case_type_code";
                deComboBox2.ValueMember = "case_type_id";
            }
        }

        private void populateConnAppCaseType()
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            string sql = "select case_type_id, case_type_code from case_type_master  ";

            OdbcDataAdapter odap = new OdbcDataAdapter(sql, sqlCon);
            odap.Fill(dt);


            if (dt.Rows.Count > 0)
            {
                deComboBox7.DataSource = dt;
                deComboBox7.DisplayMember = "case_type_code";
                deComboBox7.ValueMember = "case_type_id";

                
            }
        }

        private void populateConnMainCaseType()
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            string sql = "select case_type_id, case_type_code from case_type_master  ";

            OdbcDataAdapter odap = new OdbcDataAdapter(sql, sqlCon);
            odap.Fill(dt);


            if (dt.Rows.Count > 0)
            {
                deComboBox9.DataSource = dt;
                deComboBox9.DisplayMember = "case_type_code";
                deComboBox9.ValueMember = "case_type_id";

            }
        }

        private void analogousCaseType()
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            string sql = "select case_type_id, case_type_code from case_type_master  ";

            OdbcDataAdapter odap = new OdbcDataAdapter(sql, sqlCon);
            odap.Fill(dt);


            if (dt.Rows.Count > 0)
            {
                deComboBox10.DataSource = dt;
                deComboBox10.DisplayMember = "case_type_code";
                deComboBox10.ValueMember = "case_type_id";
            }
        }

        private void oldCaseType()
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            string sql = "select case_type_id, case_type_code from case_type_master  ";

            OdbcDataAdapter odap = new OdbcDataAdapter(sql, sqlCon);
            odap.Fill(dt);


            if (dt.Rows.Count > 0)
            {
                deComboBox11.DataSource = dt;
                deComboBox11.DisplayMember = "case_type_code";
                deComboBox11.ValueMember = "case_type_id";
            }
        }

        private void populateLcCaseType()
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            string sql = "select lc_case_type_id, lc_case_type_code from lc_case_type_master  ";

            OdbcDataAdapter odap = new OdbcDataAdapter(sql, sqlCon);
            odap.Fill(dt);


            if (dt.Rows.Count > 0)
            {
                deComboBox5.DataSource = dt;
                deComboBox5.DisplayMember = "lc_case_type_code";
                deComboBox5.ValueMember = "lc_case_type_id";

               
            }
        }

        private void populateJudge()
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            string sql = "select judge_designation, judge_name from judge_master  ";

            OdbcDataAdapter odap = new OdbcDataAdapter(sql, sqlCon);
            odap.Fill(dt);


            if (dt.Rows.Count > 0)
            {
                deComboBox3.DataSource = dt;
                deComboBox3.DisplayMember = "judge_name";
                deComboBox3.ValueMember = "judge_designation";

            }
        }

        private void populateLCJudge()
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            string sql = "select judge_designation, judge_name from judge_master  ";

            OdbcDataAdapter odap = new OdbcDataAdapter(sql, sqlCon);
            odap.Fill(dt);


            if (dt.Rows.Count > 0)
            {
               
                deComboBox6.DataSource = dt;
                deComboBox6.DisplayMember = "judge_name";
                deComboBox6.ValueMember = "judge_designation";
            }
        }
        private void populateDistrict()
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            string sql = "select district_code, district_name from district  where district_code <> '00' and district_code <> '99' order by district_name";

            OdbcDataAdapter odap = new OdbcDataAdapter(sql, sqlCon);
            odap.Fill(dt);


            if (dt.Rows.Count > 0)
            {
                deComboBox4.DataSource = dt;
                deComboBox4.DisplayMember = "district_name";
                deComboBox4.ValueMember = "district_code";


            }
        }

        private void populateDisposalType()
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            string sql = "select disposal_type_id, disposal_type_name from disposal_type_master";

            OdbcDataAdapter odap = new OdbcDataAdapter(sql, sqlCon);
            odap.Fill(dt);


            if (dt.Rows.Count > 0)
            {
                deComboBox8.DataSource = dt;
                deComboBox8.DisplayMember = "disposal_type_name";
                deComboBox8.ValueMember = "disposal_type_id";


            }
        }

        private DataTable searchJudge(string judge_name)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            string sql = "select judge_designation, judge_name from judge_master where judge_name = '" + judge_name + "' ";

            OdbcDataAdapter odap = new OdbcDataAdapter(sql, sqlCon);
            odap.Fill(dt);

            return dt;
            
        }

        private DataTable searchLCCaseType(string lc_case_type_code)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            string sql = "select lc_case_type_id, lc_case_type_code from lc_case_type_master where lc_case_type_code = '" + lc_case_type_code + "' ";

            OdbcDataAdapter odap = new OdbcDataAdapter(sql, sqlCon);
            odap.Fill(dt);

            return dt;

        }

        private DataTable searchCaseType(string case_type_code)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            string sql = "select case_type_id, case_type_code from case_type_master where case_type_code = '" + case_type_code + "' ";

            OdbcDataAdapter odap = new OdbcDataAdapter(sql, sqlCon);
            odap.Fill(dt);

            return dt;

        }

        private void EntryForm_Load(object sender, EventArgs e)
        {
            if (_mode == DataLayerDefs.Mode._Add)
            {
                populateCasestatus();
                populateCaseType();
                populateCaseNature();
                populateJudge();
                populateDistrict();
                populateLcCaseType();
                populateLCJudge();
                populateConnAppCaseType();
                populateDisposalType();
                populateConnMainCaseType();
                analogousCaseType();
                oldCaseType();

                //bundle details
                deTextBox1.Text = _GetBundleDetails(projKey, bundleKey).Rows[0][4].ToString();
                deTextBox2.Text = _GetBundleDetails(projKey, bundleKey).Rows[0][3].ToString();
                deTextBox3.Text = _GetBundleDetails(projKey, bundleKey).Rows[0][2].ToString();
                deTextBox4.Text = _GetBundleDetails(projKey, bundleKey).Rows[0][5].ToString();
                disablegroup1();

                //file details
                deTextBox8.Text = _GetFileCaseDetails(projKey, bundleKey, caseFileNo).Rows[0][3].ToString();
                deComboBox1.Text = _GetFileCaseDetails(projKey, bundleKey, caseFileNo).Rows[0][4].ToString();
                deComboBox2.Text = _GetFileCaseDetails(projKey, bundleKey, caseFileNo).Rows[0][6].ToString();
                deTextBox5.Text = _GetFileCaseDetails(projKey, bundleKey, caseFileNo).Rows[0][7].ToString();
                deComboBox12.Text = _GetFileCaseDetails(projKey, bundleKey, caseFileNo).Rows[0][5].ToString();
                disablegroup2();
                if (deComboBox1.Text == "Disposed")
                {
                    enableDateDisposal();
                    deTextBox9.Focus();
                }
                else
                {
                    disableDateDisposal();
                }
                deComboBox3.Text = string.Empty;
                deComboBox4.Text = string.Empty;
                deComboBox5.Text = string.Empty;
                deComboBox6.Text = string.Empty;
                deComboBox7.Text = string.Empty;
                deComboBox8.Text = string.Empty;
                deComboBox9.Text = string.Empty;
                deComboBox10.Text = string.Empty;
                deComboBox11.Text = string.Empty;
               
            }

            if (_mode == DataLayerDefs.Mode._Edit)
            {
                populateCasestatus();
                populateCaseType();
                populateCaseNature();
                populateJudge();
                populateDistrict();
                populateLcCaseType();
                populateLCJudge();
                populateConnAppCaseType();
                populateDisposalType();
                populateConnMainCaseType();
                analogousCaseType();
                oldCaseType();

                //bundle details
                deTextBox1.Text = _GetBundleDetails(projKey, bundleKey).Rows[0][4].ToString();
                deTextBox2.Text = _GetBundleDetails(projKey, bundleKey).Rows[0][3].ToString();
                deTextBox3.Text = _GetBundleDetails(projKey, bundleKey).Rows[0][2].ToString();
                deTextBox4.Text = _GetBundleDetails(projKey, bundleKey).Rows[0][5].ToString();
                disablegroup1();

                //file details
                deTextBox8.Text = _GetFileCaseDetails(projKey, bundleKey, caseFileNo).Rows[0][3].ToString();
                deComboBox1.Text = _GetFileCaseDetails(projKey, bundleKey, caseFileNo).Rows[0][4].ToString();
                deComboBox2.Text = _GetFileCaseDetails(projKey, bundleKey, caseFileNo).Rows[0][6].ToString();
                deTextBox5.Text = _GetFileCaseDetails(projKey, bundleKey, caseFileNo).Rows[0][7].ToString();
                deComboBox12.Text = _GetFileCaseDetails(projKey, bundleKey, caseFileNo).Rows[0][5].ToString();
                disablegroup2();
                if (deComboBox1.Text == "Disposed")
                {
                    enableDateDisposal();
                    deTextBox9.Focus();
                }
                else
                {
                    disableDateDisposal();
                }

                string item_no = _GetFileCaseDetails(projKey, bundleKey, caseFileNo).Rows[0][2].ToString();

                string disposal_date = metadata_details(projKey, bundleKey, caseFileNo, item_no).Rows[0][8].ToString();
                if(disposal_date == "" || disposal_date == null)
                {
                    deTextBox6.Text = string.Empty;
                    deTextBox7.Text = string.Empty;
                    deTextBox9.Text = string.Empty;
                }
                else
                {
                    deTextBox6.Text = disposal_date.Substring(0, 4);
                    deTextBox7.Text = disposal_date.Substring(5, 2);
                    deTextBox9.Text = disposal_date.Substring(8, 2);
                }
                string judge_name = metadata_details(projKey, bundleKey, caseFileNo, item_no).Rows[0][9].ToString();
                if(judge_name == "" || judge_name == null)
                {
                    deComboBox3.Text = string.Empty;
                    listView1.Items.Clear();
                }
                else
                {
                    deComboBox3.Text = string.Empty;
                    string[] split = judge_name.Split(';');
                    
                    foreach (string judge in split)
                    {
                        Console.WriteLine(judge);
                        if (judge == null || judge == "")
                        {
                        }
                        else
                        {
                            listView1.Items.Add(judge);
                        }
                    }
                }

                string district = metadata_details(projKey, bundleKey, caseFileNo, item_no).Rows[0][10].ToString();
                if(district == null || district == "")
                {
                    deComboBox4.Text = string.Empty;
                }
                else
                {
                    deComboBox4.Text = district;
                }

                string petitioner_name = metadata_details(projKey, bundleKey, caseFileNo, item_no).Rows[0][11].ToString();
                if(petitioner_name == null || petitioner_name == "")
                {
                    deTextBox11.Text = string.Empty;
                    listView2.Items.Clear();
                }
                else
                {
                    deTextBox11.Text = string.Empty;
                    string[] split = petitioner_name.Split(';');

                    foreach (string petitioner in split)
                    {
                        Console.WriteLine(petitioner);
                        if (petitioner == null || petitioner == "")
                        {
                        }
                        else
                        {
                            listView2.Items.Add(petitioner);
                        }
                    }
                }

                string petitioner_counsel_name = metadata_details(projKey, bundleKey, caseFileNo, item_no).Rows[0][12].ToString();
                if(petitioner_counsel_name == null || petitioner_counsel_name == "")
                {
                    deTextBox14.Text = string.Empty;
                    listView7.Items.Clear();
                }
                else
                {
                    deTextBox14.Text = string.Empty;
                    string[] split = petitioner_counsel_name.Split(';');

                    foreach (string petitionercounsel in split)
                    {
                        Console.WriteLine(petitionercounsel);
                        if (petitionercounsel == null || petitionercounsel == "")
                        {
                        }
                        else
                        {
                            listView7.Items.Add(petitionercounsel);
                        }
                    }
                }

                string respondant_name = metadata_details(projKey, bundleKey, caseFileNo, item_no).Rows[0][13].ToString();
                if(respondant_name == null || respondant_name == "")
                {
                    deTextBox18.Text = string.Empty;
                    listView3.Items.Clear();
                }
                else
                {
                    deTextBox18.Text = string.Empty;
                    string[] split = respondant_name.Split(';');

                    foreach (string respondant in split)
                    {
                        Console.WriteLine(respondant);
                        if (respondant == null || respondant == "")
                        {
                        }
                        else
                        {
                            listView3.Items.Add(respondant);
                        }
                    }
                }

                string respondant_counsel = metadata_details(projKey, bundleKey, caseFileNo, item_no).Rows[0][14].ToString();
                if(respondant_counsel == null || respondant_counsel == "")
                {
                    deTextBox16.Text = string.Empty;
                    listView8.Items.Clear();
                }
                else
                {
                    deTextBox16.Text = string.Empty;
                    string[] split = respondant_counsel.Split(';');

                    foreach (string respondantcounsel in split)
                    {
                        Console.WriteLine(respondantcounsel);
                        if (respondantcounsel == null || respondantcounsel == "")
                        {
                        }
                        else
                        {
                            listView8.Items.Add(respondantcounsel);
                        }
                    }
                }

                string case_filling_date = metadata_details(projKey, bundleKey, caseFileNo, item_no).Rows[0][15].ToString();
                if(case_filling_date == null || case_filling_date == "")
                {
                    deTextBox21.Text = string.Empty;
                    deTextBox20.Text = string.Empty;
                    deTextBox19.Text = string.Empty;
                }
                else
                {
                    deTextBox21.Text = case_filling_date.Substring(0, 4);
                    deTextBox20.Text = case_filling_date.Substring(5, 2);
                    deTextBox19.Text = case_filling_date.Substring(8, 2);
                }

                string ps = metadata_details(projKey, bundleKey, caseFileNo, item_no).Rows[0][16].ToString();
                if(ps == null || ps == "")
                {
                    deTextBox22.Text = string.Empty;
                }
                else
                {
                    deTextBox22.Text = ps;
                }

                string ps_case_no = metadata_details(projKey, bundleKey, caseFileNo, item_no).Rows[0][17].ToString();
                if(ps_case_no == null || ps_case_no == "")
                {
                    deTextBox23.Text = string.Empty;
                }
                else
                {
                    deTextBox23.Text = ps_case_no;
                }

                string lc_case_no = metadata_details(projKey, bundleKey, caseFileNo, item_no).Rows[0][18].ToString();
                if(lc_case_no == null || lc_case_no == "")
                {
                    deComboBox5.Text = string.Empty;
                    deTextBox24.Text = string.Empty;
                    deTextBox25.Text = string.Empty;
                    listView4.Items.Clear();
                }
                else
                {
                    deComboBox5.Text = string.Empty;
                    deTextBox24.Text = string.Empty;
                    deTextBox25.Text = string.Empty;

                    string[] split = lc_case_no.Split(';');

                    foreach (string lccaseno in split)
                    {
                        Console.WriteLine(lccaseno);
                        if (lccaseno == null || lccaseno == "")
                        {
                        }
                        else
                        {
                            listView4.Items.Add(lccaseno);
                        }
                    }
                }

                string lc_order_date = metadata_details(projKey, bundleKey, caseFileNo, item_no).Rows[0][19].ToString();
                if(lc_order_date == null || lc_order_date == "")
                {
                    deTextBox29.Text = string.Empty;
                    deTextBox28.Text = string.Empty;
                    deTextBox27.Text = string.Empty;
                }
                else
                {
                    deTextBox29.Text = lc_order_date.Substring(0, 4);
                    deTextBox28.Text = lc_order_date.Substring(5, 2);
                    deTextBox27.Text = lc_order_date.Substring(8, 2);
                }

                string lc_judge_name = metadata_details(projKey, bundleKey, caseFileNo, item_no).Rows[0][20].ToString();
                if(lc_judge_name == null || lc_judge_name == "")
                {
                    deComboBox6.Text = string.Empty;
                    listView9.Items.Clear();
                }
                else
                {
                    deComboBox6.Text = string.Empty;
                    string[] split = lc_judge_name.Split(';');

                    foreach (string lcjudge in split)
                    {
                        Console.WriteLine(lcjudge);
                        if (lcjudge == null || lcjudge == "")
                        {
                        }
                        else
                        {
                            listView9.Items.Add(lcjudge);
                        }
                    }
                }

                string conn_app_case_no = metadata_details(projKey, bundleKey, caseFileNo, item_no).Rows[0][21].ToString();
                if(conn_app_case_no == null || conn_app_case_no == "")
                {
                    deComboBox7.Text = string.Empty;
                    deTextBox31.Text = string.Empty;
                    deTextBox32.Text = string.Empty;
                    listView5.Items.Clear();
                }
                else
                {
                    deComboBox7.Text = string.Empty;
                    deTextBox31.Text = string.Empty;
                    deTextBox32.Text = string.Empty;
                    string[] split = conn_app_case_no.Split(';');

                    foreach (string connappcaseno in split)
                    {
                        Console.WriteLine(connappcaseno);
                        if (connappcaseno == null || connappcaseno == "")
                        {
                        }
                        else
                        {
                            listView5.Items.Add(connappcaseno);
                        }
                    }
                }

                string conn_disposal_type = metadata_details(projKey, bundleKey, caseFileNo, item_no).Rows[0][22].ToString();
                if(conn_disposal_type == null || conn_disposal_type == "")
                {
                    deComboBox8.Text = string.Empty;
                }
                else
                {
                    deComboBox8.Text = conn_disposal_type;
                }

                string conn_main_case_no = metadata_details(projKey, bundleKey, caseFileNo, item_no).Rows[0][23].ToString();
                if(conn_main_case_no == null || conn_main_case_no == "")
                {
                    deComboBox9.Text = string.Empty;
                    deTextBox35.Text = string.Empty;
                    deTextBox36.Text = string.Empty;
                    listView10.Items.Clear();
                }
                else
                {
                    deComboBox9.Text = string.Empty;
                    deTextBox35.Text = string.Empty;
                    deTextBox36.Text = string.Empty;
                    string[] split = conn_main_case_no.Split(';');

                    foreach (string connmaincaseno in split)
                    {
                        Console.WriteLine(connmaincaseno);
                        if (connmaincaseno == null || connmaincaseno == "")
                        {
                        }
                        else
                        {
                            listView10.Items.Add(connmaincaseno);
                        }
                    }
                }

                string analogous_case_no = metadata_details(projKey, bundleKey, caseFileNo, item_no).Rows[0][24].ToString();
                if(analogous_case_no == null || analogous_case_no == "")
                {
                    deComboBox10.Text = string.Empty;
                    deTextBox38.Text = string.Empty;
                    deTextBox39.Text = string.Empty;
                    listView6.Items.Clear();
                }
                else
                {
                    deComboBox10.Text = string.Empty;
                    deTextBox38.Text = string.Empty;
                    deTextBox39.Text = string.Empty;
                    string[] split = analogous_case_no.Split(';');

                    foreach (string analogouscaseno in split)
                    {
                        Console.WriteLine(analogouscaseno);
                        if (analogouscaseno == null || analogouscaseno == "")
                        {
                        }
                        else
                        {
                            listView6.Items.Add(analogouscaseno);
                        }
                    }
                }

                string old_case_type = metadata_details(projKey, bundleKey, caseFileNo, item_no).Rows[0][25].ToString();
                if (old_case_type == null || old_case_type == "")
                {
                    deComboBox11.Text = string.Empty;
                }
                else
                {
                    deComboBox11.Text = old_case_type;
                }

                string old_case_no = metadata_details(projKey, bundleKey, caseFileNo, item_no).Rows[0][26].ToString();
                if (old_case_no == null || old_case_no == "")
                {
                    deTextBox40.Text = string.Empty;
                }
                else
                {
                    deTextBox40.Text = old_case_no;
                }

                string old_case_year = metadata_details(projKey, bundleKey, caseFileNo, item_no).Rows[0][27].ToString();
                if (old_case_year == null || old_case_year == "")
                {
                    deTextBox41.Text = string.Empty;
                }
                else
                {
                    deTextBox41.Text = old_case_year;
                }

                string file_move_history = metadata_details(projKey, bundleKey, caseFileNo, item_no).Rows[0][28].ToString();
                if (file_move_history == null || file_move_history == "")
                {
                    deTextBox42.Text = string.Empty;
                }
                else
                {
                    deTextBox42.Text = file_move_history;
                }

                string dept_remark = metadata_details(projKey, bundleKey, caseFileNo, item_no).Rows[0][29].ToString();
                if (dept_remark == null || dept_remark == "")
                {
                    deTextBox43.Text = string.Empty;
                }
                else
                {
                    deTextBox43.Text = dept_remark;
                }
            }
        }

        private DataTable metadata_details(string projkey, string bundlekey, string casefileno, string item_no)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            string sql = "select proj_code, bundle_key, item_no,case_file_no, case_status, case_type, case_nature, case_year, disposal_date, judge_name, district,petitioner_name,petitioner_counsel_name,respondant_name, respondant_counsel_name, case_filling_date, ps_name, ps_case_no, lc_case_no,lc_order_date,lc_judge_name, conn_app_case_no, conn_disposal_type,conn_main_case_no, analogous_case_no, old_case_type,old_case_no,old_case_year,file_move_history,dept_remark from metadata_entry where proj_code = '"+projkey+"' and bundle_key = '"+bundlekey+"' and case_file_no = '"+casefileno+"' and item_no = '"+item_no+"' ";

            OdbcDataAdapter odap = new OdbcDataAdapter(sql, sqlCon);
            odap.Fill(dt);

            return dt;

        }

        private void EntryForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                if (_mode == DataLayerDefs.Mode._Add)
                {
                    DialogResult dr = MessageBox.Show(this, "Do you want to Exit ? ", "Record Management ", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (dr == DialogResult.Yes)
                    {
                        this.Hide();
                        Files fm = new Files(sqlCon, DataLayerDefs.Mode._Add);
                        fm.ShowDialog(this);
                    }
                    else
                    {
                        return;
                    }
                }

                if (_mode == DataLayerDefs.Mode._Edit)
                {
                    DialogResult dr = MessageBox.Show(this, "Do you want to Exit ? ", "Record Management ", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (dr == DialogResult.Yes)
                    {
                        this.Hide();
                        //Files fm = new Files(sqlCon, DataLayerDefs.Mode._Edit);
                        //fm.ShowDialog(this);
                    }
                    else
                    {
                        return;
                    }
                }
            }

            if (e.Control == true && e.KeyCode == Keys.S)
            {
                deButton20_Click(sender, e);
            }

            if (e.Control == true && e.KeyCode == Keys.D)
            {
                deComboBox4.Focus();
                return;
            }

            if (e.Control == true && e.KeyCode == Keys.J)
            {
                deComboBox3.Focus();
                return;
            }

            if (e.Control == true && e.KeyCode == Keys.P)
            {
                deTextBox11.Focus();
                return;
            }

            if (e.Control == true && e.KeyCode == Keys.R)
            {
                deTextBox18.Focus();
                return;
            }

            if (e.Control == true && e.KeyCode == Keys.L)
            {
                deComboBox5.Focus();
                return;
            }

            if (e.Control == true && e.KeyCode == Keys.C)
            {
                deComboBox7.Focus();
                return;
            }

            if (e.Control == true && e.KeyCode == Keys.M)
            {
                deComboBox9.Focus();
                return;
            }

            if (e.Control == true && e.KeyCode == Keys.A)
            {
                deComboBox10.Focus();
                return;
            }

            if (e.Control == true && e.KeyCode == Keys.O)
            {
                deComboBox11.Focus();
                return;
            }

            if (e.Control == true && e.KeyCode == Keys.F)
            {
                deTextBox42.Focus();
                return;
            }

            if (e.Control == true && e.KeyCode == Keys.N)
            {
                deTextBox43.Focus();
                return;
            }
        }

        private void cmdnew_Click(object sender, EventArgs e)
        {

            if ((deComboBox3.Text != "" || deComboBox3.Text != null || String.IsNullOrEmpty(deComboBox3.Text) || String.IsNullOrWhiteSpace(deComboBox3.Text)) && searchJudge(deComboBox3.Text.Trim()).Rows.Count > 0)
            {
                string judge_name = "HON`BLE " + deComboBox3.SelectedValue.ToString() + " " + deComboBox3.Text;

                for (int i = 0; i < listView1.Items.Count; i++)
                {
                    if (listView1.Items[i].SubItems[0].Text == judge_name)
                    {
                        MessageBox.Show("This Judge name is already added...");
                        deComboBox3.Focus();
                        return;
                    }
                    else
                    {
                        continue;
                    }
                }

                string[] row = { judge_name };
                var listItem = new ListViewItem(row);
                listView1.Items.Add(listItem);
                deComboBox3.Text = string.Empty;
                deComboBox3.Focus();

            }
            else
            {
                MessageBox.Show("No Such Judge name is selected...");
                deComboBox3.Focus();
                return;
            }
        }

        private void deButton1_Click(object sender, EventArgs e)
        {
            if(listView1.Items.Count > 0)
            {
                for (int i = 0; i < listView1.Items.Count; i++)
                {
                    if (listView1.Items[i].Selected == true)
                    {
                        if (listView1.SelectedItems[0].Selected == true)
                        {
                            listView1.SelectedItems[0].Remove();
                            deComboBox3.Text = string.Empty;
                            deComboBox3.Focus();
                        }
                    }
                }
            }
        }

        private void deButton3_Click(object sender, EventArgs e)
        {
            if (deTextBox11.Text == "" || deTextBox11.Text == null || deTextBox11.Text == string.Empty || String.IsNullOrEmpty(deTextBox11.Text) || String.IsNullOrWhiteSpace(deTextBox11.Text))
            {
                deTextBox11.Focus();
                return;
            }
            if (deTextBox11.Text != "" || deTextBox11.Text != string.Empty)
            {
                for (int i = 0; i < listView2.Items.Count; i++)
                {
                    if (listView2.Items[i].SubItems[0].Text == deTextBox11.Text)
                    {
                        MessageBox.Show("This Petinioer name is already added...");
                        deTextBox11.Focus();
                        return;
                    }
                    else
                    {
                        continue;
                    }
                }

                string[] row = { deTextBox11.Text };
                var listItem = new ListViewItem(row);
                listView2.Items.Add(listItem);
                deTextBox11.Text = string.Empty;
                deTextBox11.Focus();
            }
            
        }

        private void deButton2_Click(object sender, EventArgs e)
        {
            if (listView2.Items.Count > 0)
            {
                for (int i = 0; i < listView2.Items.Count; i++)
                {
                    if(listView2.Items[i].Selected == true)
                    {
                        if (listView2.SelectedItems[0].Selected == true)
                        {
                            listView2.SelectedItems[0].Remove();
                            deTextBox11.Text = string.Empty;
                            deTextBox11.Focus();
                        }
                    }
                }
                   
            }
        }

        private void deButton10_Click(object sender, EventArgs e)
        {

            if (deTextBox18.Text == "" || deTextBox18.Text == null || String.IsNullOrEmpty(deTextBox18.Text) || String.IsNullOrWhiteSpace(deTextBox18.Text))
            {
                deTextBox18.Focus();
                return;
            }
            if (deTextBox18.Text != "" || deTextBox18.Text != null)
            {
                for (int i = 0; i < listView3.Items.Count; i++)
                {
                    if (listView3.Items[i].SubItems[0].Text == deTextBox18.Text)
                    {
                        MessageBox.Show("This Respondant name is already added...");
                        deTextBox18.Focus();
                        return;
                    }
                    else
                    {
                        continue;
                    }
                }
                string[] row = { deTextBox18.Text };
                var listItem = new ListViewItem(row);
                listView3.Items.Add(listItem);
                deTextBox18.Text = string.Empty;
                deTextBox18.Focus();
            }
           
        }

        private void deButton11_Click(object sender, EventArgs e)
        {
            if (listView3.Items.Count > 0)
            {
                for (int i = 0; i < listView3.Items.Count; i++)
                {
                    if (listView3.Items[i].Selected == true)
                    {
                        if (listView3.SelectedItems[0].Selected == true)
                        {
                            listView3.SelectedItems[0].Remove();
                            deTextBox18.Text = string.Empty;
                            deTextBox18.Focus();
                        }
                    }
                }
            }
        }

        private void deButton5_Click(object sender, EventArgs e)
        {

            if (deTextBox14.Text == "" || deTextBox14.Text == null || String.IsNullOrEmpty(deTextBox14.Text) || String.IsNullOrWhiteSpace(deTextBox14.Text))
            {
                deTextBox14.Focus();
                return;
            }
            if (deTextBox14.Text != "" || deTextBox14.Text != null)
            {
                for (int i = 0; i < listView7.Items.Count; i++)
                {
                    if (listView7.Items[i].SubItems[0].Text == deTextBox14.Text)
                    {
                        MessageBox.Show("This Petinioner Counsel name is already added...");
                        deTextBox14.Focus();
                        return;
                    }
                    else
                    {
                        continue;
                    }
                }
                string[] row = { deTextBox14.Text };
                var listItem = new ListViewItem(row);
                listView7.Items.Add(listItem);
                deTextBox14.Text = string.Empty;
                deTextBox14.Focus();
            }
           
        }

        private void deButton4_Click(object sender, EventArgs e)
        {
            if (listView7.Items.Count > 0)
            {
                for (int i = 0; i < listView7.Items.Count; i++)
                {
                    if (listView7.Items[i].Selected == true)
                    {
                        if (listView7.SelectedItems[0].Selected == true)
                        {
                            listView7.SelectedItems[0].Remove();
                            deTextBox14.Text = string.Empty;
                            deTextBox14.Focus();
                        }
                    }
                }
            }
        }

        private void deButton6_Click(object sender, EventArgs e)
        {
            if (deTextBox16.Text == "" || deTextBox16.Text == null || String.IsNullOrEmpty(deTextBox16.Text) || String.IsNullOrWhiteSpace(deTextBox16.Text))
            {
                deTextBox16.Focus();
                return;
            }
            if (deTextBox16.Text != "" || deTextBox16.Text != null)
            {
                for (int i = 0; i < listView8.Items.Count; i++)
                {
                    if (listView8.Items[i].SubItems[0].Text == deTextBox16.Text)
                    {
                        MessageBox.Show("This Respondant Counsel name is already added...");
                        deTextBox16.Focus();
                        return;
                    }
                    else
                    {
                        continue;
                    }
                }
                string[] row = { deTextBox16.Text };
                var listItem = new ListViewItem(row);
                listView8.Items.Add(listItem);
                deTextBox16.Text = string.Empty;
                deTextBox16.Focus();
            }
            
        }

        private void deButton7_Click(object sender, EventArgs e)
        {
            if (listView8.Items.Count > 0)
            {
                for (int i = 0; i < listView8.Items.Count; i++)
                {
                    if (listView8.Items[i].Selected == true)
                    {
                        if (listView8.SelectedItems[0].Selected == true)
                        {
                            listView8.SelectedItems[0].Remove();
                            deTextBox16.Text = string.Empty;
                            deTextBox16.Focus();
                        }
                    }
                }
            }
        }

        private void deButton9_Click(object sender, EventArgs e)
        {
            
            if ((deComboBox5.Text == "" || deComboBox5.Text == null || String.IsNullOrEmpty(deComboBox5.Text) || String.IsNullOrWhiteSpace(deComboBox5.Text)) || (deTextBox24.Text == "" || deTextBox24.Text == null || String.IsNullOrEmpty(deTextBox24.Text) || String.IsNullOrWhiteSpace(deTextBox24.Text)) || (deTextBox25.Text == "" || deTextBox25.Text == string.Empty || deTextBox25.Text == null || String.IsNullOrEmpty(deTextBox25.Text) || String.IsNullOrWhiteSpace(deTextBox25.Text)))
            {
                MessageBox.Show("Please Fill All the fields ...");
                deComboBox5.Focus();
                return;
            }

            if ((deComboBox5.Text != "" || deComboBox5.Text != null) || (deTextBox24.Text != "" || deTextBox24.Text != null) || (deTextBox25.Text != "" || deTextBox25.Text != null))
            {
                if (searchLCCaseType(deComboBox5.Text.Trim()).Rows.Count > 0)
                {
                    string lc_case_number = deComboBox5.Text + "/" + deTextBox24.Text + "/" + deTextBox25.Text;

                    for (int i = 0; i < listView4.Items.Count; i++)
                    {
                        if (listView4.Items[i].SubItems[0].Text == lc_case_number)
                        {
                            MessageBox.Show("This Lower court case number is already added...");
                            deComboBox5.Focus();
                            return;
                        }
                        else
                        {
                            continue;
                        }
                    }

                    string[] row = { lc_case_number };
                    var listItem = new ListViewItem(row);
                    listView4.Items.Add(listItem);
                    deComboBox5.Text = string.Empty;
                    deTextBox24.Text = string.Empty;
                    deTextBox25.Text = string.Empty;
                    deComboBox5.Focus();
                }
                else
                {
                    MessageBox.Show("Select proper LC Case type...");
                    deComboBox5.Text = string.Empty;
                    deTextBox24.Text = string.Empty;
                    deTextBox25.Text = string.Empty;
                    deComboBox5.Focus();
                    return;
                }

            }
            
        }

        private void deButton8_Click(object sender, EventArgs e)
        {
            if (listView4.Items.Count > 0)
            {
                for (int i = 0; i < listView4.Items.Count; i++)
                {
                    if (listView4.Items[i].Selected == true)
                    {
                        if (listView4.SelectedItems[0].Selected == true)
                        {
                            listView4.SelectedItems[0].Remove();
                            deComboBox5.Text = string.Empty;
                            deTextBox24.Text = string.Empty;
                            deTextBox25.Text = string.Empty;
                            deComboBox5.Focus();
                        }
                    }
                }
            }
        }

        private void deButton13_Click(object sender, EventArgs e)
        {
            if ((deComboBox6.Text == "" || deComboBox6.Text == null || String.IsNullOrEmpty(deComboBox6.Text)) || String.IsNullOrWhiteSpace(deComboBox6.Text))
            {
                MessageBox.Show("No Such LC Judge name is selected...");
                deComboBox6.Focus();
                return;
            }
            if ((deComboBox6.Text != "" || deComboBox6.Text != null) && searchJudge(deComboBox6.Text.Trim()).Rows.Count > 0)
            {
                string judge_name = "HON`BLE " + deComboBox6.SelectedValue.ToString() + " " + deComboBox6.Text;

                for (int i = 0; i < listView9.Items.Count; i++)
                {
                    if (listView9.Items[i].SubItems[0].Text == judge_name)
                    {
                        MessageBox.Show("This LC Judge name is already added...");
                        deComboBox6.Focus();
                        return;
                    }
                    else
                    {
                        continue;
                    }
                }

                string[] row = { judge_name };
                var listItem = new ListViewItem(row);
                listView9.Items.Add(listItem);
                deComboBox6.Text = string.Empty;
                deComboBox6.Focus();

            }
            
        }

        private void deButton12_Click(object sender, EventArgs e)
        {
            if (listView9.Items.Count > 0)
            {
                for (int i = 0; i < listView9.Items.Count; i++)
                {
                    if (listView9.Items[i].Selected == true)
                    {
                        if (listView9.SelectedItems[0].Selected == true)
                        {
                            listView9.SelectedItems[0].Remove();
                            deComboBox6.Text = string.Empty;
                            deComboBox6.Focus();
                        }
                    }
                }
            }
        }

        private void deButton14_Click(object sender, EventArgs e)
        {
            if (listView5.Items.Count > 0)
            {
                for (int i = 0; i < listView5.Items.Count; i++)
                {
                    if (listView5.Items[i].Selected == true)
                    {
                        if (listView5.SelectedItems[0].Selected == true)
                        {
                            listView5.SelectedItems[0].Remove();
                            deComboBox7.Text = string.Empty;
                            deTextBox31.Text = string.Empty;
                            deTextBox32.Text = string.Empty;
                            deComboBox7.Focus();
                        }
                    }
                }
            }
        }

        private void deButton15_Click(object sender, EventArgs e)
        {
            
            if ((deComboBox7.Text == "" || deComboBox7.Text == null || String.IsNullOrEmpty(deComboBox7.Text) || String.IsNullOrWhiteSpace(deComboBox7.Text)) || (deTextBox31.Text == "" || deTextBox31.Text == null || String.IsNullOrEmpty(deTextBox31.Text) || String.IsNullOrWhiteSpace(deTextBox31.Text)) || (deTextBox32.Text == "" || deTextBox32.Text == null || String.IsNullOrEmpty(deTextBox32.Text) || String.IsNullOrWhiteSpace(deTextBox32.Text)))
            {
                MessageBox.Show("Please Fill All the fields ...");
                deComboBox7.Focus();
                return;
            }
            if ((deComboBox7.Text != "" || deComboBox7.Text != null) && (deTextBox31.Text != "" || deTextBox31.Text != null) && (deTextBox32.Text != "" || deTextBox32.Text != null))
            {
                if (searchCaseType(deComboBox7.Text.Trim()).Rows.Count > 0)
                {
                    string con_case_number = deComboBox7.Text + "/" + deTextBox31.Text + "/" + deTextBox32.Text;

                    for (int i = 0; i < listView5.Items.Count; i++)
                    {
                        if (listView5.Items[i].SubItems[0].Text == con_case_number)
                        {
                            MessageBox.Show("This Connected application case number is already added...");
                            deComboBox7.Focus();
                            return;
                        }
                        else
                        {
                            continue;
                        }
                    }

                    string[] row = { con_case_number };
                    var listItem = new ListViewItem(row);
                    listView5.Items.Add(listItem);
                    deComboBox7.Text = string.Empty;
                    deTextBox31.Text = string.Empty;
                    deTextBox32.Text = string.Empty;
                    deComboBox7.Focus();
                }
                else
                {
                    MessageBox.Show("Select proper Case type...");
                    deComboBox7.Focus();
                    return;
                }

            }
            
        }

        private void deButton17_Click(object sender, EventArgs e)
        {
           
            if ((deComboBox9.Text == "" || deComboBox9.Text == null || String.IsNullOrEmpty(deComboBox9.Text) || String.IsNullOrWhiteSpace(deComboBox9.Text)) || (deTextBox35.Text == "" || deTextBox35.Text == null || String.IsNullOrEmpty(deTextBox35.Text) || String.IsNullOrWhiteSpace(deTextBox35.Text)) || (deTextBox36.Text == "" || deTextBox36.Text == null || String.IsNullOrEmpty(deTextBox36.Text) || String.IsNullOrWhiteSpace(deTextBox36.Text)))
            {
                MessageBox.Show("Please Fill All the fields ...");
                deComboBox9.Focus();
                return;
            }
            if ((deComboBox9.Text != "" || deComboBox9.Text != null) && (deTextBox35.Text != "" || deTextBox35.Text != null) && (deTextBox36.Text != "" || deTextBox36.Text != null))
            {
                if (searchCaseType(deComboBox9.Text.Trim()).Rows.Count > 0)
                {
                    string con_main_case_number = deComboBox9.Text + "/" + deTextBox35.Text + "/" + deTextBox36.Text;

                    for (int i = 0; i < listView10.Items.Count; i++)
                    {
                        if (listView10.Items[i].SubItems[0].Text == con_main_case_number)
                        {
                            MessageBox.Show("This Connected Main case number is already added...");
                            deComboBox9.Focus();
                            return;
                        }
                        else
                        {
                            continue;
                        }
                    }

                    string[] row = { con_main_case_number };
                    var listItem = new ListViewItem(row);
                    listView10.Items.Add(listItem);
                    deComboBox9.Text = string.Empty;
                    deTextBox35.Text = string.Empty;
                    deTextBox36.Text = string.Empty;
                    deComboBox9.Focus();
                }
                else
                {
                    MessageBox.Show("Select proper Case type...");
                    deComboBox9.Focus();
                    return;
                }

            }
           
        }

        private void deButton16_Click(object sender, EventArgs e)
        {
            if (listView10.Items.Count > 0)
            {
                for (int i = 0; i < listView10.Items.Count; i++)
                {
                    if (listView10.Items[i].Selected == true)
                    {
                        if (listView10.SelectedItems[0].Selected == true)
                        {
                            listView10.SelectedItems[0].Remove();
                            deComboBox9.Text = string.Empty;
                            deTextBox35.Text = string.Empty;
                            deTextBox36.Text = string.Empty;
                            deComboBox9.Focus();
                        }
                    }
                }
            }
        }

        private void deButton19_Click(object sender, EventArgs e)
        {
            
            if ((deComboBox10.Text == "" || deComboBox10.Text == null || String.IsNullOrEmpty(deComboBox10.Text) || String.IsNullOrWhiteSpace(deComboBox10.Text)) || (deTextBox38.Text == "" || deTextBox38.Text == null || String.IsNullOrEmpty(deTextBox38.Text) || String.IsNullOrWhiteSpace(deTextBox38.Text)) || (deTextBox39.Text == "" || deTextBox39.Text == null || String.IsNullOrEmpty(deTextBox39.Text) || String.IsNullOrWhiteSpace(deTextBox39.Text)))
            {
                MessageBox.Show("Please Fill All the fields ...");
                deComboBox10.Focus();
                return;
            }
            if ((deComboBox10.Text != "" || deComboBox10.Text != null) && (deTextBox38.Text != "" || deTextBox38.Text != null) && (deTextBox39.Text != "" || deTextBox39.Text != null))
            {
                if (searchCaseType(deComboBox10.Text.Trim()).Rows.Count > 0)
                {
                    string analogous_case_number = deComboBox10.Text + "/" + deTextBox38.Text + "/" + deTextBox39.Text;

                    for (int i = 0; i < listView6.Items.Count; i++)
                    {
                        if (listView6.Items[i].SubItems[0].Text == analogous_case_number)
                        {
                            MessageBox.Show("This Analogous case number is already added...");
                            deComboBox10.Focus();
                            return;
                        }
                        else
                        {
                            continue;
                        }
                    }

                    string[] row = { analogous_case_number };
                    var listItem = new ListViewItem(row);
                    listView6.Items.Add(listItem);
                    deComboBox10.Text = string.Empty;
                    deTextBox38.Text = string.Empty;
                    deTextBox39.Text = string.Empty;
                    deComboBox10.Focus();
                }
                else
                {
                    MessageBox.Show("Select proper Case type...");
                    deComboBox10.Focus();
                    return;
                }

            }
            
        }

        private void deButton18_Click(object sender, EventArgs e)
        {
            if (listView6.Items.Count > 0)
            {
                for (int i = 0; i < listView6.Items.Count; i++)
                {
                    if (listView6.Items[i].Selected == true)
                    {
                        if (listView6.SelectedItems[0].Selected == true)
                        {
                            listView6.SelectedItems[0].Remove();
                            deComboBox10.Text = string.Empty;
                            deTextBox38.Text = string.Empty;
                            deTextBox39.Text = string.Empty;
                            deComboBox10.Focus();
                        }
                    }
                }
            }
        }

        private void deButton21_Click(object sender, EventArgs e)
        {
            if (_mode == DataLayerDefs.Mode._Add)
            {
                DialogResult dr = MessageBox.Show(this, "Do you want to Exit ? ", "Record Management ", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dr == DialogResult.Yes)
                {
                    this.Hide();
                    Files fm = new Files(sqlCon, DataLayerDefs.Mode._Add);
                    fm.ShowDialog(this);
                }
                else
                {
                    return;
                }
            }
            if (_mode == DataLayerDefs.Mode._Edit)
            {
                DialogResult dr = MessageBox.Show(this, "Do you want to Exit ? ", "Record Management ", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dr == DialogResult.Yes)
                {
                    this.Hide();
                    //Files fm = new Files(sqlCon, DataLayerDefs.Mode._Edit);
                    //fm.ShowDialog(this);
                }
                else
                {
                    return;
                }
            }
        }

        

        public bool validate()
        {
            bool retval = false;

            string currDate = DateTime.Now.ToString("yyyy-MM-dd");
            string curYear = DateTime.Now.ToString("yyyy");
            int curIntYear = Convert.ToInt32(curYear);

            if (deTextBox6.Text != "" || deTextBox7.Text != "" || deTextBox9.Text != "")
            {
                if (deTextBox6.Text != "")
                {

                    bool res = System.Text.RegularExpressions.Regex.IsMatch(deTextBox6.Text, "[^0-9]");
                    if (res != true && Convert.ToInt32(deTextBox6.Text) <= curIntYear && deTextBox6.Text.Length == 4 && deTextBox6.Text.Substring(0, 1) != "0")
                    {
                        retval = true;
                    }
                    else
                    {
                        retval = false;
                        MessageBox.Show("Please input Valid Year...");
                        deTextBox6.Focus();
                        return retval;
                    }
                }
                
                if (deTextBox7.Text != "")
                {

                    bool res1 = System.Text.RegularExpressions.Regex.IsMatch(deTextBox7.Text, "[^0-9]");

                    if (res1 != true && deTextBox7.Text.Length == 2 && Convert.ToInt32(deTextBox7.Text) <= 12 && Convert.ToInt32(deTextBox7.Text) != 0 && deTextBox7.Text != "00")
                    {
                        retval = true;

                    }
                    else
                    {
                        retval = false;
                        MessageBox.Show("Please input Valid Month...");
                        deTextBox7.Focus();
                        return retval;
                    }
                }
                else
                {
                    retval = false;
                    MessageBox.Show("Please input Valid Month...");
                    deTextBox7.Focus();
                    return retval;
                }
                if (deTextBox9.Text != "")
                {

                    bool res2 = System.Text.RegularExpressions.Regex.IsMatch(deTextBox9.Text, "[^0-9]");
                    if (res2 != true && deTextBox9.Text.Length == 2 && Convert.ToInt32(deTextBox9.Text) <= 31 && Convert.ToInt32(deTextBox9.Text) != 0 && deTextBox9.Text != "00")
                    {
                        retval = true;

                    }
                    else
                    {
                        retval = false;
                        MessageBox.Show("Please input Valid Date...");
                        deTextBox9.Focus();
                        return retval;
                    }
                }
                else
                {
                    retval = false;
                    MessageBox.Show("Please input Valid Date...");
                    deTextBox9.Focus();
                    return retval;
                }

                DateTime temp;
                string isDate = deTextBox6.Text + "-" + deTextBox7.Text + "-" + deTextBox9.Text;
                if (DateTime.TryParse(isDate, out temp) && DateTime.Parse(isDate) <= DateTime.Parse(currDate))
                {
                    retval = true;
                }
                else
                {
                    retval = false;
                    MessageBox.Show("Please select a valid date", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    deTextBox6.Select();
                    return retval;

                }
            }
            else
            {
                retval = true;
            }

            if (deTextBox21.Text != "" || deTextBox20.Text != "" || deTextBox19.Text != "")
            {
                if (deTextBox21.Text != "")
                {

                    bool res = System.Text.RegularExpressions.Regex.IsMatch(deTextBox21.Text, "[^0-9]");
                    if (res != true && Convert.ToInt32(deTextBox21.Text) <= curIntYear && deTextBox21.Text.Length == 4 && deTextBox21.Text.Substring(0, 1) != "0")
                    {
                        //retval = true;
                        retval = true;
                    }
                    else
                    {
                        retval = false;
                        MessageBox.Show("Please input Valid Year...");
                        deTextBox21.Focus();
                        return retval;
                    }
                }

                if (deTextBox20.Text != "")
                {

                    bool res1 = System.Text.RegularExpressions.Regex.IsMatch(deTextBox20.Text, "[^0-9]");

                    if (res1 != true && deTextBox20.Text.Length == 2 && Convert.ToInt32(deTextBox20.Text) <= 12 && Convert.ToInt32(deTextBox20.Text) != 0 && deTextBox20.Text != "00")
                    {
                        //retval = true;
                        retval = true;
                    }
                    else
                    {
                        retval = false;
                        MessageBox.Show("Please input Valid Month...");
                        deTextBox20.Focus();
                        return retval;
                    }
                }
                else
                {
                    retval = false;
                    MessageBox.Show("Please input Valid Month...");
                    deTextBox20.Focus();
                    return retval;
                }

                if (deTextBox19.Text != "")
                {

                    bool res2 = System.Text.RegularExpressions.Regex.IsMatch(deTextBox19.Text, "[^0-9]");
                    if (res2 != true && deTextBox19.Text.Length == 2 && Convert.ToInt32(deTextBox19.Text) <= 31 && Convert.ToInt32(deTextBox19.Text) != 0 && deTextBox19.Text != "00")
                    {
                        //retval = true;
                        retval = true;
                    }
                    else
                    {
                        retval = false;
                        MessageBox.Show("Please input Valid Date...");
                        deTextBox19.Focus();
                        return retval;
                    }
                }
                else
                {
                    retval = false;
                    MessageBox.Show("Please input Valid Date...");
                    deTextBox19.Focus();
                    return retval;
                }

                DateTime temp;
                string isDate = deTextBox21.Text + "-" + deTextBox20.Text + "-" + deTextBox19.Text;
                if (DateTime.TryParse(isDate, out temp) && DateTime.Parse(isDate) <= DateTime.Parse(currDate))
                {
                    retval = true;
                }
                else
                {
                    retval = false;
                    MessageBox.Show("Please select a valid date", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    deTextBox21.Select();
                    return retval;

                }
            }
            else
            {
                retval = true;
            }

            if (deTextBox29.Text != "" || deTextBox28.Text != "" || deTextBox27.Text != "")
            {
                if (deTextBox29.Text != "")
                {

                    bool res = System.Text.RegularExpressions.Regex.IsMatch(deTextBox29.Text, "[^0-9]");
                    if (res != true && Convert.ToInt32(deTextBox29.Text) <= curIntYear && deTextBox29.Text.Length == 4 && deTextBox29.Text.Substring(0, 1) != "0")
                    {
                        //retval = true;
                        retval = true;
                    }
                    else
                    {
                        retval = false;
                        MessageBox.Show("Please input Valid Year...");
                        deTextBox29.Focus();
                        return retval;
                    }
                }
                if (deTextBox28.Text != "")
                {

                    bool res1 = System.Text.RegularExpressions.Regex.IsMatch(deTextBox28.Text, "[^0-9]");

                    if (res1 != true && deTextBox28.Text.Length == 2 && Convert.ToInt32(deTextBox28.Text) <= 12 && Convert.ToInt32(deTextBox28.Text) != 0 && deTextBox28.Text != "00")
                    {
                        //retval = true;
                        retval = true;
                    }
                    else
                    {
                        retval = false;
                        MessageBox.Show("Please input Valid Month...");
                        deTextBox28.Focus();
                        return retval;
                    }
                }
                else
                {
                    retval = false;
                    MessageBox.Show("Please input Valid Month...");
                    deTextBox28.Focus();
                    return retval;
                }
                if (deTextBox27.Text != "")
                {

                    bool res2 = System.Text.RegularExpressions.Regex.IsMatch(deTextBox27.Text, "[^0-9]");
                    if (res2 != true && deTextBox27.Text.Length == 2 && Convert.ToInt32(deTextBox27.Text) <= 31 && Convert.ToInt32(deTextBox27.Text) != 0 && deTextBox27.Text != "00")
                    {
                        //retval = true;
                        retval = true;
                    }
                    else
                    {
                        retval = false;
                        MessageBox.Show("Please input Valid Date...");
                        deTextBox27.Focus();
                        return retval;
                    }
                }
                else
                {
                    retval = false;
                    MessageBox.Show("Please input Valid Date...");
                    deTextBox27.Focus();
                    return retval;
                }

                DateTime temp;
                string isDate = deTextBox29.Text + "-" + deTextBox28.Text + "-" + deTextBox27.Text;
                if (DateTime.TryParse(isDate, out temp) && DateTime.Parse(isDate) <= DateTime.Parse(currDate))
                {
                    retval = true;
                }
                else
                {
                    retval = false;
                    MessageBox.Show("Please select a valid date", "Please select a valid date", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    deTextBox29.Select();
                    return retval;

                }
            }
            else
            {
                retval = true;
            }

            

            return retval;
        }
        private bool insertIntoDB(string item_no)
        {
            bool commitBol = true;

            string sqlStr = string.Empty;

            OdbcCommand sqlCmd = new OdbcCommand();

            string judge_name = "";
            string petitionoer_name = "";
            string petitionoer_counsel = "";
            string respondant_name = "";
            string respondant_counsel = "";
            string lc_case_no = "";
            string lc_judge_name = "";
            string conn_app_case_no = "";
            string conn_main_case_no = "";
            string analogous_case_no = "";

            string case_file_no = deTextBox8.Text;
            string case_status = deComboBox1.Text;
            string case_type = deComboBox2.Text;
            string case_nature = deComboBox12.Text;
            string case_year = deTextBox5.Text;
            string disposal_date = "";

            if (deTextBox6.Text != "" && deTextBox7.Text != "" && deTextBox9.Text != "")
            {
                disposal_date = deTextBox6.Text+"-"+deTextBox7.Text+"-"+deTextBox9.Text;
            }
            else
            {
                disposal_date = "";
            }
            
            if (listView1.Items.Count > 0)
            {
                if (listView1.Items.Count == 1)
                {
                    judge_name = listView1.Items[0].SubItems[0].Text;
                }
                else
                {
                    for (int i = 0; i < listView1.Items.Count ;i++)
                    {
                        judge_name = judge_name + listView1.Items[i].SubItems[0].Text + ";";
                    }
                }
            }
            else
            {
                judge_name = string.Empty;
            }

            string district = deComboBox4.Text;

            if (listView2.Items.Count > 0)
            {
                if (listView2.Items.Count == 1)
                {
                    petitionoer_name = listView2.Items[0].SubItems[0].Text;
                }
                else
                {
                    for (int i = 0; i < listView2.Items.Count; i++)
                    {
                        petitionoer_name = petitionoer_name + listView2.Items[i].SubItems[0].Text + ";";
                    }
                }
            }
            else
            {
                petitionoer_name = string.Empty;
            }

            if (listView7.Items.Count > 0)
            {
                if (listView7.Items.Count == 1)
                {
                    petitionoer_counsel = listView7.Items[0].SubItems[0].Text;
                }
                else
                {
                    for (int i = 0; i < listView7.Items.Count; i++)
                    {
                        petitionoer_counsel = petitionoer_counsel + listView7.Items[i].SubItems[0].Text + ";";
                    }
                }
            }
            else
            {
                petitionoer_counsel = string.Empty;
            }

            if (listView3.Items.Count > 0)
            {
                if (listView3.Items.Count == 1)
                {
                    respondant_name = listView3.Items[0].SubItems[0].Text;
                }
                else
                {
                    for (int i = 0; i < listView3.Items.Count; i++)
                    {
                        respondant_name = respondant_name + listView3.Items[i].SubItems[0].Text + ";";
                    }
                }
            }
            else
            {
                respondant_name = string.Empty;
            }

            if (listView8.Items.Count > 0)
            {
                if (listView8.Items.Count == 1)
                {
                    respondant_counsel = listView8.Items[0].SubItems[0].Text;
                }
                else
                {
                    for (int i = 0; i < listView8.Items.Count; i++)
                    {
                        respondant_counsel = respondant_counsel + listView8.Items[i].SubItems[0].Text + ";";
                    }
                }
            }
            else
            {
                respondant_counsel = string.Empty;
            }

            string case_filling_date = "";
            if (deTextBox21.Text != "" && deTextBox20.Text != "" && deTextBox19.Text != "")
            {
                case_filling_date = deTextBox21.Text + "-" + deTextBox20.Text + "-" + deTextBox19.Text;
            }
            else
            {
                case_filling_date = "";
            }
            
            string ps = deTextBox22.Text.Trim();
            string ps_caseno = deTextBox23.Text.Trim();

            if (listView4.Items.Count > 0)
            {
                if (listView4.Items.Count == 1)
                {
                    lc_case_no = listView4.Items[0].SubItems[0].Text;
                }
                else
                {
                    for (int i = 0; i < listView4.Items.Count; i++)
                    {
                        lc_case_no = lc_case_no + listView4.Items[i].SubItems[0].Text + ";";
                    }
                }
            }
            else
            {
                lc_case_no = string.Empty;
            }

            string lc_order_date = "";
            if (deTextBox29.Text != "" && deTextBox28.Text != "" && deTextBox27.Text != "")
            {
                lc_order_date = deTextBox29.Text + "-" + deTextBox28.Text + "-" + deTextBox27.Text;
            }
            else
            {
                lc_order_date = "";
            }

            if (listView9.Items.Count > 0)
            {
                if (listView9.Items.Count == 1)
                {
                    lc_judge_name = listView9.Items[0].SubItems[0].Text;
                }
                else
                {
                    for (int i = 0; i < listView9.Items.Count; i++)
                    {
                        lc_judge_name = lc_judge_name + listView9.Items[i].SubItems[0].Text + ";";
                    }
                }
            }
            else
            {
                lc_judge_name = string.Empty;
            }

            if (listView5.Items.Count > 0)
            {
                if (listView5.Items.Count == 1)
                {
                    conn_app_case_no = listView5.Items[0].SubItems[0].Text;
                }
                else
                {
                    for (int i = 0; i < listView5.Items.Count; i++)
                    {
                        conn_app_case_no = conn_app_case_no + listView5.Items[i].SubItems[0].Text + ";";
                    }
                }
            }
            else
            {
                conn_app_case_no = string.Empty;
            }

            string conn_app_disposal_type = deComboBox8.Text;

            if (listView10.Items.Count > 0)
            {
                if (listView10.Items.Count == 1)
                {
                    conn_main_case_no = listView10.Items[0].SubItems[0].Text;
                }
                else
                {
                    for (int i = 0; i < listView10.Items.Count; i++)
                    {
                        conn_main_case_no = conn_main_case_no + listView10.Items[i].SubItems[0].Text + ";";
                    }
                }
            }
            else
            {
                conn_main_case_no = string.Empty;
            }

            if (listView6.Items.Count > 0)
            {
                if (listView6.Items.Count == 1)
                {
                    analogous_case_no = listView6.Items[0].SubItems[0].Text;
                }
                else
                {
                    for (int i = 0; i < listView6.Items.Count; i++)
                    {
                        analogous_case_no = analogous_case_no + listView6.Items[i].SubItems[0].Text + ";";
                    }
                }
            }
            else
            {
                analogous_case_no = string.Empty;
            }

            string old_case_type = deComboBox11.Text;
            string old_case_no = deTextBox40.Text;
            string old_case_year = deTextBox41.Text;

            string file_move_history = deTextBox42.Text;
            string dept_note = deTextBox43.Text;


            sqlStr = @"insert into metadata_entry(proj_code,bundle_key,item_no,case_file_no,case_status,case_type,case_nature,case_year,disposal_date,judge_name,district,petitioner_name,petitioner_counsel_name,respondant_name,respondant_counsel_name,case_filling_date,ps_name,ps_case_no,lc_case_no,lc_order_date,lc_judge_name,conn_app_case_no,conn_disposal_type,conn_main_case_no,analogous_case_no,old_case_type,old_case_no,old_case_year,file_move_history,dept_remark,created_by,created_dttm) values('" +
                        projKey + "','" + bundleKey + "','"+item_no+"','" + case_file_no +
                        "','" + case_status + "','" + case_type + "','" + case_nature + "','" + case_year + "','" + disposal_date + "','" + judge_name + "','" + district + "','" + petitionoer_name + "','" + petitionoer_counsel + "','" + respondant_name + "','" + respondant_counsel + "','" + case_filling_date + "','" + ps + "','" + ps_caseno + "','" + lc_case_no + "','" + lc_order_date + "','" + lc_judge_name + "','"+conn_app_case_no+"','"+conn_app_disposal_type+"','"+conn_main_case_no+"','"+analogous_case_no+"','"+old_case_type+"','"+old_case_no+"','"+old_case_year+"','"+file_move_history+"','"+dept_note+"','" + frmMain.name + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
            sqlCmd.Connection = sqlCon;
            //sqlCmd.Transaction = trans;
            sqlCmd.CommandText = sqlStr;
            int j = sqlCmd.ExecuteNonQuery();
            if (j > 0)
            {
                commitBol = true;
            }
            else
            {
                commitBol = false;
            }

            return commitBol;
        }

        private bool updateDB(string item_no)
        {
            bool commitBol = true;

            string sqlStr = string.Empty;

            OdbcCommand sqlCmd = new OdbcCommand();

            string judge_name = "";
            string petitionoer_name = "";
            string petitionoer_counsel = "";
            string respondant_name = "";
            string respondant_counsel = "";
            string lc_case_no = "";
            string lc_judge_name = "";
            string conn_app_case_no = "";
            string conn_main_case_no = "";
            string analogous_case_no = "";

            string case_file_no = deTextBox8.Text;
            string case_status = deComboBox1.Text;
            string case_type = deComboBox2.Text;
            string case_nature = deComboBox12.Text;
            string case_year = deTextBox5.Text;
            string disposal_date = "";

            if (deTextBox6.Text != "" && deTextBox7.Text != "" && deTextBox9.Text != "")
            {
                disposal_date = deTextBox6.Text + "-" + deTextBox7.Text + "-" + deTextBox9.Text;
            }
            else
            {
                disposal_date = "";
            }

            if (listView1.Items.Count > 0)
            {
                if (listView1.Items.Count == 1)
                {
                    judge_name = listView1.Items[0].SubItems[0].Text;
                }
                else
                {
                    for (int i = 0; i < listView1.Items.Count; i++)
                    {
                        judge_name = judge_name + listView1.Items[i].SubItems[0].Text + ";";
                    }
                }
            }
            else
            {
                judge_name = string.Empty;
            }

            string district = deComboBox4.Text;

            if (listView2.Items.Count > 0)
            {
                if (listView2.Items.Count == 1)
                {
                    petitionoer_name = listView2.Items[0].SubItems[0].Text;
                }
                else
                {
                    for (int i = 0; i < listView2.Items.Count; i++)
                    {
                        petitionoer_name = petitionoer_name + listView2.Items[i].SubItems[0].Text + ";";
                    }
                }
            }
            else
            {
                petitionoer_name = string.Empty;
            }

            if (listView7.Items.Count > 0)
            {
                if (listView7.Items.Count == 1)
                {
                    petitionoer_counsel = listView7.Items[0].SubItems[0].Text;
                }
                else
                {
                    for (int i = 0; i < listView7.Items.Count; i++)
                    {
                        petitionoer_counsel = petitionoer_counsel + listView7.Items[i].SubItems[0].Text + ";";
                    }
                }
            }
            else
            {
                petitionoer_counsel = string.Empty;
            }

            if (listView3.Items.Count > 0)
            {
                if (listView3.Items.Count == 1)
                {
                    respondant_name = listView3.Items[0].SubItems[0].Text;
                }
                else
                {
                    for (int i = 0; i < listView3.Items.Count; i++)
                    {
                        respondant_name = respondant_name + listView3.Items[i].SubItems[0].Text + ";";
                    }
                }
            }
            else
            {
                respondant_name = string.Empty;
            }

            if (listView8.Items.Count > 0)
            {
                if (listView8.Items.Count == 1)
                {
                    respondant_counsel = listView8.Items[0].SubItems[0].Text;
                }
                else
                {
                    for (int i = 0; i < listView8.Items.Count; i++)
                    {
                        respondant_counsel = respondant_counsel + listView8.Items[i].SubItems[0].Text + ";";
                    }
                }
            }
            else
            {
                respondant_counsel = string.Empty;
            }

            string case_filling_date = "";
            if (deTextBox21.Text != "" && deTextBox20.Text != "" && deTextBox19.Text != "")
            {
                case_filling_date = deTextBox21.Text + "-" + deTextBox20.Text + "-" + deTextBox19.Text;
            }
            else
            {
                case_filling_date = "";
            }

            string ps = deTextBox22.Text.Trim();
            string ps_caseno = deTextBox23.Text.Trim();

            if (listView4.Items.Count > 0)
            {
                if (listView4.Items.Count == 1)
                {
                    lc_case_no = listView4.Items[0].SubItems[0].Text;
                }
                else
                {
                    for (int i = 0; i < listView4.Items.Count; i++)
                    {
                        lc_case_no = lc_case_no + listView4.Items[i].SubItems[0].Text + ";";
                    }
                }
            }
            else
            {
                lc_case_no = string.Empty;
            }

            string lc_order_date = "";
            if (deTextBox29.Text != "" && deTextBox28.Text != "" && deTextBox27.Text != "")
            {
                lc_order_date = deTextBox29.Text + "-" + deTextBox28.Text + "-" + deTextBox27.Text;
            }
            else
            {
                lc_order_date = "";
            }

            if (listView9.Items.Count > 0)
            {
                if (listView9.Items.Count == 1)
                {
                    lc_judge_name = listView9.Items[0].SubItems[0].Text;
                }
                else
                {
                    for (int i = 0; i < listView9.Items.Count; i++)
                    {
                        lc_judge_name = lc_judge_name + listView9.Items[i].SubItems[0].Text + ";";
                    }
                }
            }
            else
            {
                lc_judge_name = string.Empty;
            }

            if (listView5.Items.Count > 0)
            {
                if (listView5.Items.Count == 1)
                {
                    conn_app_case_no = listView5.Items[0].SubItems[0].Text;
                }
                else
                {
                    for (int i = 0; i < listView5.Items.Count; i++)
                    {
                        conn_app_case_no = conn_app_case_no + listView5.Items[i].SubItems[0].Text + ";";
                    }
                }
            }
            else
            {
                conn_app_case_no = string.Empty;
            }

            string conn_app_disposal_type = deComboBox8.Text;

            if (listView10.Items.Count > 0)
            {
                if (listView10.Items.Count == 1)
                {
                    conn_main_case_no = listView10.Items[0].SubItems[0].Text;
                }
                else
                {
                    for (int i = 0; i < listView10.Items.Count; i++)
                    {
                        conn_main_case_no = conn_main_case_no + listView10.Items[i].SubItems[0].Text + ";";
                    }
                }
            }
            else
            {
                conn_main_case_no = string.Empty;
            }

            if (listView6.Items.Count > 0)
            {
                if (listView6.Items.Count == 1)
                {
                    analogous_case_no = listView6.Items[0].SubItems[0].Text;
                }
                else
                {
                    for (int i = 0; i < listView6.Items.Count; i++)
                    {
                        analogous_case_no = analogous_case_no + listView6.Items[i].SubItems[0].Text + ";";
                    }
                }
            }
            else
            {
                analogous_case_no = string.Empty;
            }

            string old_case_type = deComboBox11.Text;
            string old_case_no = deTextBox40.Text;
            string old_case_year = deTextBox41.Text;

            string file_move_history = deTextBox42.Text;
            string dept_note = deTextBox43.Text;


            sqlStr = @"update metadata_entry set case_status = '" + case_status + "',case_type ='" + case_type + "',case_nature='" + case_nature + "',case_year ='" + case_year + "',disposal_date='" + disposal_date + "',judge_name ='" + judge_name + "',district='" + district + "',petitioner_name ='" + petitionoer_name + "',petitioner_counsel_name='" + petitionoer_counsel + "',respondant_name='" + respondant_name + "',respondant_counsel_name='" + respondant_counsel + "',case_filling_date='" + case_filling_date + "',ps_name='" + ps + "',ps_case_no='" + ps_caseno + "',lc_case_no='" + lc_case_no + "',lc_order_date= '" + lc_order_date + "',lc_judge_name='" + lc_judge_name + "',conn_app_case_no='" + conn_app_case_no + "',conn_disposal_type='" + conn_app_disposal_type + "',conn_main_case_no='" + conn_main_case_no + "',analogous_case_no='" + analogous_case_no + "',old_case_type='" + old_case_type + "',old_case_no='" + old_case_no + "',old_case_year='" + old_case_year + "',file_move_history = '" + file_move_history + "',dept_remark = '" + dept_note + "',modified_by ='" + frmMain.name + "',modified_dttm = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where proj_code ='" + projKey + "' and bundle_key = '" + bundleKey + "' and case_file_no ='" + caseFileNo + "' and item_no ='" + item_no + "' ";   
            sqlCmd.Connection = sqlCon;
            //sqlCmd.Transaction = trans;
            sqlCmd.CommandText = sqlStr;
            int j = sqlCmd.ExecuteNonQuery();
            if (j > 0)
            {
                commitBol = true;
            }
            else
            {
                commitBol = false;
            }

            return commitBol;
        }


        private void deButton20_Click(object sender, EventArgs e)
        {
            if (deComboBox4.Text == "" || deComboBox4.Text == null || String.IsNullOrEmpty(deComboBox4.Text) || String.IsNullOrWhiteSpace(deComboBox4.Text))
            {
                MessageBox.Show("You cannot leave this field blank...", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                deComboBox4.Focus();
                return;
            }
            if (deComboBox4.Text != "" || deComboBox4.Text != null) 
            {
                if (listView2.Items.Count > 0)
                {
                    if (listView3.Items.Count > 0)
                    {
                        if(validate()== true)
                        {
                            if (_mode == DataLayerDefs.Mode._Add)
                            {
                                if (_GetMetaCount(projKey, bundleKey, caseFileNo).Rows.Count == 0)
                                {
                                    bool insertmeta = insertIntoDB(_GetFileCaseDetails(projKey, bundleKey, caseFileNo).Rows[0][2].ToString());

                                    if (insertmeta == true)
                                    {
                                        MessageBox.Show(this, "Record Saved Successfully...", "Record Management", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        this.Hide();
                                        Files fm = new Files(sqlCon, DataLayerDefs.Mode._Add);
                                        fm.ShowDialog(this);
                                    }
                                    else
                                    {
                                        MessageBox.Show(this, "Ooops!!! There is an Error - Record not Saved...", "Record Management", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        this.Hide();
                                        Files fm = new Files(sqlCon, DataLayerDefs.Mode._Add);
                                        fm.ShowDialog(this);
                                    }
                                }
                                else
                                {
                                    MessageBox.Show(this, "Record Exists...", "Record Management ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    this.Hide();
                                    Files fm = new Files(sqlCon, DataLayerDefs.Mode._Add);
                                    fm.ShowDialog(this);
                                }
                                
                            }
                            else if (_mode == DataLayerDefs.Mode._Edit)
                            {
                                bool updatemeta = updateDB(_GetFileCaseDetails(projKey, bundleKey, caseFileNo).Rows[0][2].ToString());

                                if (updatemeta == true)
                                {
                                    MessageBox.Show(this, "Record Saved Successfully...", "Record Management", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    this.Hide();
                                    //Files fm = new Files(sqlCon, DataLayerDefs.Mode._Edit);
                                    //fm.ShowDialog(this);
                                }
                                else
                                {
                                    MessageBox.Show(this, "Ooops!!! There is an Error - Record not Saved...", "Record Management", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    this.Hide();
                                    //Files fm = new Files(sqlCon, DataLayerDefs.Mode._Edit);
                                    //fm.ShowDialog(this);
                                }
                            }
                            else
                            {
                                MessageBox.Show(this, "Ooops!!! There is an Error - Record not Saved...", "Record Management", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                this.Hide();
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("You have to enter minimum one respondant","", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        deTextBox18.Focus();
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("You have to enter minimum one petitioner","", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    deTextBox11.Focus();
                    return;
                }
            }
            else
            {
                MessageBox.Show("You cannot leave this field blank...","",MessageBoxButtons.OK,MessageBoxIcon.Error);
                deComboBox4.Focus();
                return;
            }
        }

        private void deTextBox25_Leave(object sender, EventArgs e)
        {
            string currDate = DateTime.Now.ToString("yyyy-MM-dd");
            string curYear = DateTime.Now.ToString("yyyy");
            int curIntYear = Convert.ToInt32(curYear);

            if (deTextBox25.Text != "")
            {

                bool res = System.Text.RegularExpressions.Regex.IsMatch(deTextBox25.Text, "[^0-9]");
                if (res != true && Convert.ToInt32(deTextBox25.Text) <= curIntYear && deTextBox25.Text.Length == 4 && deTextBox25.Text.Substring(0, 1) != "0")
                {
                   
                }
                else
                {
                   
                    MessageBox.Show("Please input Valid Lower Court Case Year...");
                    deTextBox25.Focus();
                    return;
                }
            }
            
        }

        private void deTextBox32_Leave(object sender, EventArgs e)
        {
            string currDate = DateTime.Now.ToString("yyyy-MM-dd");
            string curYear = DateTime.Now.ToString("yyyy");
            int curIntYear = Convert.ToInt32(curYear);
            if (deTextBox32.Text != "")
            {

                bool res = System.Text.RegularExpressions.Regex.IsMatch(deTextBox32.Text, "[^0-9]");
                if (res != true && Convert.ToInt32(deTextBox32.Text) <= curIntYear && deTextBox32.Text.Length == 4 && deTextBox32.Text.Substring(0, 1) != "0")
                {
                   
                }
                else
                {
                   
                    MessageBox.Show("Please input Valid Connected Application Case Year...");
                    deTextBox32.Focus();
                    return;
                }
            }
           
        }

        private void deTextBox36_Leave(object sender, EventArgs e)
        {
            string currDate = DateTime.Now.ToString("yyyy-MM-dd");
            string curYear = DateTime.Now.ToString("yyyy");
            int curIntYear = Convert.ToInt32(curYear);

            if (deTextBox36.Text != "")
            {

                bool res = System.Text.RegularExpressions.Regex.IsMatch(deTextBox36.Text, "[^0-9]");
                if (res != true && Convert.ToInt32(deTextBox36.Text) <= curIntYear && deTextBox36.Text.Length == 4 && deTextBox36.Text.Substring(0, 1) != "0")
                {
                  
                }
                else
                {
                   
                    MessageBox.Show("Please input Valid Connected Main Case Year...");
                    deTextBox36.Focus();
                    return;
                }
            }
            
        }

        private void deTextBox39_Leave(object sender, EventArgs e)
        {
            string currDate = DateTime.Now.ToString("yyyy-MM-dd");
            string curYear = DateTime.Now.ToString("yyyy");
            int curIntYear = Convert.ToInt32(curYear);

            if (deTextBox39.Text != "")
            {

                bool res = System.Text.RegularExpressions.Regex.IsMatch(deTextBox39.Text, "[^0-9]");
                if (res != true && Convert.ToInt32(deTextBox39.Text) <= curIntYear && deTextBox39.Text.Length == 4 && deTextBox39.Text.Substring(0, 1) != "0")
                {
                    
                }
                else
                {
                    
                    MessageBox.Show("Please input Valid Analogous Case Year...");
                    deTextBox39.Focus();
                    return;
                }
            }
            
        }

        private void deTextBox41_Leave(object sender, EventArgs e)
        {
            string currDate = DateTime.Now.ToString("yyyy-MM-dd");
            string curYear = DateTime.Now.ToString("yyyy");
            int curIntYear = Convert.ToInt32(curYear);

            if (deTextBox41.Text != "")
            {

                bool res = System.Text.RegularExpressions.Regex.IsMatch(deTextBox41.Text, "[^0-9]");
                if (res != true && Convert.ToInt32(deTextBox41.Text) <= curIntYear && deTextBox41.Text.Length == 4 && deTextBox41.Text.Substring(0, 1) != "0")
                {
                    
                }
                else
                {
                    
                    MessageBox.Show("Please input Valid Connected Main Case Year...");
                    deTextBox41.Focus();
                    return;
                }
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            listView1.Select();
        }

        private void listView1_KeyUp(object sender, KeyEventArgs e)
        {
            if (listView1.Items.Count > 0)
            {
                if (listView1.SelectedItems.Count > 0)
                {
                    if (e.KeyCode == Keys.F2 || e.KeyCode == Keys.Enter)
                    {
                        listView1_DoubleClick(sender, e);
                    }
                    if(e.KeyCode == Keys.Delete)
                    {
                        deButton1_Click(sender, e);
                    }
                }
                else
                {
                    //listView1.Items[0].Selected = true;
                    //listView1.Items[0].Focused = true;
                    listView1.Select();
                }
            }
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            
            listView1.Select();
           
            //listView1.SelectedItems[0].Selected = true;
           
            string name1="";
            
            try
            {
                
                string[] split = listView1.SelectedItems[0].SubItems[0].Text.Split(' ');
                
                foreach (string judge in split)
                {
                    Console.WriteLine(judge);
                    if (judge == null || judge == "" || judge == "HON`BLE" || judge == "JUSTICE" || judge == "CHIEF" || judge == "ACTING")
                    {
                    }
                    else
                    {
                        if (name1 == "")
                        {
                            name1 = judge;
                        }
                        else
                        {
                            name1 = name1 + " " + judge;
                        }
                        
                    }
                }
                if(searchJudge(name1).Rows.Count > 0)
                {
                    deComboBox3.Text = name1;
                    deComboBox3.Select();
                }   
                else
                {
                    deComboBox3.Text = "";
                    return;
                }
                
            }
            catch (Exception ex)
            {
                return;
                // MessageBox.Show(ex.Message);
            }
        }

        private void cmdnew_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2)
            {
                if (deComboBox3.Text != "" && searchJudge(deComboBox3.Text.Trim()).Rows.Count > 0)
                {
                    string judge_name = "HON`BLE " + deComboBox3.SelectedValue.ToString() + " " + deComboBox3.Text;

                    for (int i = 0; i < listView1.Items.Count; i++)
                    {
                        if (listView1.Items[i].SubItems[0].Text == judge_name)
                        {
                            MessageBox.Show("This Judge name is already added...");
                            deComboBox3.Focus();
                            return;
                        }
                        else
                        {
                            continue;
                        }
                    }
                    if (listView1.SelectedItems[0].Selected == true)
                    {
                        listView1.SelectedItems[0].SubItems[0].Text = judge_name;
                        listView1.Select();
                        deComboBox3.Text = "";
                    }
                }
            }
        }

        private void listView2_SelectedIndexChanged(object sender, EventArgs e)
        {
            listView2.Select();
        }

        private void listView2_DoubleClick(object sender, EventArgs e)
        {
            listView2.Select();

            string name1 = "";

            try
            {
                string[] split = listView2.SelectedItems[0].SubItems[0].Text.Split(' ');

                foreach (string petitioner in split)
                {

                    if (petitioner == null || petitioner == "")
                    {
                    }
                    else
                    {
                        if (name1 == "")
                        {
                            name1 = petitioner;
                        }
                        else
                        {
                            name1 = name1 + " " + petitioner;
                        }

                    }
                }

                deTextBox11.Text = name1;
                deTextBox11.Select();
            }
            catch (Exception ex)
            {
                return;
                // MessageBox.Show(ex.Message);
            }
            
            
        }

        private void listView2_KeyUp(object sender, KeyEventArgs e)
        {
            if (listView2.Items.Count > 0)
            {
                if (listView2.SelectedItems.Count > 0)
                {
                    if (e.KeyCode == Keys.F2 || e.KeyCode == Keys.Enter)
                    {
                        listView2_DoubleClick(sender, e);
                    }
                    if (e.KeyCode == Keys.Delete)
                    {
                        deButton2_Click(sender, e);
                    }
                }
                else
                {
                    //listView2.Items[0].Selected = true;
                    //listView2.Items[0].Focused = true;
                    listView2.Select();
                }

            }
        }

        private void deButton3_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.F2)
            {
                if(deTextBox11.Text != "")
                {
                    for (int i = 0; i < listView2.Items.Count; i++)
                    {
                        if (listView2.Items[i].SubItems[0].Text == deTextBox11.Text)
                        {
                            MessageBox.Show("This Petinioer name is already added...");
                            deTextBox11.Focus();
                            return;
                        }
                        else
                        {
                            continue;
                        }
                    }

                    if (listView2.SelectedItems[0].Selected == true)
                    {
                        listView2.SelectedItems[0].SubItems[0].Text = deTextBox11.Text;
                        listView2.Select();
                        deTextBox11.Text = "";
                    }
                }
            }
        }

        private void listView3_SelectedIndexChanged(object sender, EventArgs e)
        {
            listView3.Select();
        }

        private void listView3_KeyUp(object sender, KeyEventArgs e)
        {
            if (listView3.Items.Count > 0)
            {
                if (listView3.SelectedItems.Count > 0)
                {
                    if (e.KeyCode == Keys.F2 || e.KeyCode == Keys.Enter)
                    {
                        listView3_DoubleClick(sender, e);
                    }
                    if (e.KeyCode == Keys.Delete)
                    {
                        deButton11_Click(sender, e);
                    }
                }
                else
                {
                    //listView2.Items[0].Selected = true;
                    //listView2.Items[0].Focused = true;
                    listView3.Select();
                }

            }
        }

        private void listView3_DoubleClick(object sender, EventArgs e)
        {
            listView3.Select();

            string name1 = "";

            try
            {
                string[] split = listView3.SelectedItems[0].SubItems[0].Text.Split(' ');

                foreach (string respondant in split)
                {

                    if (respondant == null || respondant == "")
                    {
                    }
                    else
                    {
                        if (name1 == "")
                        {
                            name1 = respondant;
                        }
                        else
                        {
                            name1 = name1 + " " + respondant;
                        }

                    }
                }

                deTextBox18.Text = name1;
                deTextBox18.Select();
            }
            catch (Exception ex)
            {
                return;
                // MessageBox.Show(ex.Message);
            }
        }

        private void deButton10_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2)
            {
                if (deTextBox18.Text != "")
                {
                    for (int i = 0; i < listView3.Items.Count; i++)
                    {
                        if (listView3.Items[i].SubItems[0].Text == deTextBox18.Text)
                        {
                            MessageBox.Show("This Respondant name is already added...");
                            deTextBox18.Focus();
                            return;
                        }
                        else
                        {
                            continue;
                        }
                    }

                    if (listView3.SelectedItems[0].Selected == true)
                    {
                        listView3.SelectedItems[0].SubItems[0].Text = deTextBox18.Text;
                        listView3.Select();
                        deTextBox18.Text = "";
                    }
                }
            }
        }

        private void listView7_SelectedIndexChanged(object sender, EventArgs e)
        {
            listView7.Select();
        }

        private void listView7_KeyUp(object sender, KeyEventArgs e)
        {
            if (listView7.Items.Count > 0)
            {
                if (listView7.SelectedItems.Count > 0)
                {
                    if (e.KeyCode == Keys.F2 || e.KeyCode == Keys.Enter)
                    {
                        listView7_DoubleClick(sender, e);
                    }
                    if (e.KeyCode == Keys.Delete)
                    {
                        deButton4_Click(sender, e);
                    }
                }
                else
                {
                    //listView2.Items[0].Selected = true;
                    //listView2.Items[0].Focused = true;
                    listView7.Select();
                }

            }
        }

        private void listView7_DoubleClick(object sender, EventArgs e)
        {
            listView7.Select();

            string name1 = "";

            try
            {
                string[] split = listView7.SelectedItems[0].SubItems[0].Text.Split(' ');

                foreach (string petcounsel in split)
                {

                    if (petcounsel == null || petcounsel == "")
                    {
                    }
                    else
                    {
                        if (name1 == "")
                        {
                            name1 = petcounsel;
                        }
                        else
                        {
                            name1 = name1 + " " + petcounsel;
                        }

                    }
                }

                deTextBox14.Text = name1;
                deTextBox14.Select();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
               
            }
        }

        private void deButton5_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2)
            {
                if (deTextBox14.Text != "")
                {
                    for (int i = 0; i < listView7.Items.Count; i++)
                    {
                        if (listView7.Items[i].SubItems[0].Text == deTextBox14.Text)
                        {
                            MessageBox.Show("This Petitioner Counsel name is already added...");
                            deTextBox14.Focus();
                            return;
                        }
                        else
                        {
                            continue;
                        }
                    }

                    if (listView7.SelectedItems[0].Selected == true)
                    {
                        listView7.SelectedItems[0].SubItems[0].Text = deTextBox14.Text;
                        listView7.Select();
                        deTextBox14.Text = "";
                    }
                }
            }
        }

        private void listView8_SelectedIndexChanged(object sender, EventArgs e)
        {
            listView8.Select();
        }

        private void listView8_KeyUp(object sender, KeyEventArgs e)
        {
            if (listView8.Items.Count > 0)
            {
                if (listView8.SelectedItems.Count > 0)
                {
                    if (e.KeyCode == Keys.F2 || e.KeyCode == Keys.Enter)
                    {
                        listView8_DoubleClick(sender, e);
                    }
                    if (e.KeyCode == Keys.Delete)
                    {
                        deButton7_Click(sender, e);
                    }
                }
                else
                {
                    //listView2.Items[0].Selected = true;
                    //listView2.Items[0].Focused = true;
                    listView8.Select();
                }

            }
        }

        private void listView8_DoubleClick(object sender, EventArgs e)
        {
            listView8.Select();

            string name1 = "";

            try
            {
                string[] split = listView8.SelectedItems[0].SubItems[0].Text.Split(' ');

                foreach (string rescounsel in split)
                {

                    if (rescounsel == null || rescounsel == "")
                    {
                    }
                    else
                    {
                        if (name1 == "")
                        {
                            name1 = rescounsel;
                        }
                        else
                        {
                            name1 = name1 + " " + rescounsel;
                        }

                    }
                }

                deTextBox16.Text = name1;
                deTextBox16.Select();
            }
            catch (Exception ex)
            {
                return;
                // MessageBox.Show(ex.Message);
            }
        }

        private void deButton6_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2)
            {
                if (deTextBox16.Text != "")
                {
                    for (int i = 0; i < listView8.Items.Count; i++)
                    {
                        if (listView8.Items[i].SubItems[0].Text == deTextBox16.Text)
                        {
                            MessageBox.Show("This Respondant Counsel name is already added...");
                            deTextBox16.Focus();
                            return;
                        }
                        else
                        {
                            continue;
                        }
                    }

                    if (listView8.SelectedItems[0].Selected == true)
                    {
                        listView8.SelectedItems[0].SubItems[0].Text = deTextBox16.Text;
                        listView8.Select();
                        deTextBox16.Text = "";
                    }
                }
            }
        }

        private void listView4_SelectedIndexChanged(object sender, EventArgs e)
        {
            listView4.Select();
        }

        private void listView4_KeyUp(object sender, KeyEventArgs e)
        {
            if (listView4.Items.Count > 0)
            {
                if (listView4.SelectedItems.Count > 0)
                {
                    if (e.KeyCode == Keys.F2 || e.KeyCode == Keys.Enter)
                    {
                        listView4_DoubleClick(sender, e);
                    }
                    if (e.KeyCode == Keys.Delete)
                    {
                        deButton8_Click(sender, e);
                    }
                }
                else
                {
                    //listView2.Items[0].Selected = true;
                    //listView2.Items[0].Focused = true;
                    listView4.Select();
                }

            }
        }

        private void listView4_DoubleClick(object sender, EventArgs e)
        {
            listView4.Select();

            
            try
            {
                string[] split = listView4.SelectedItems[0].SubItems[0].Text.Split('/');

                for (int i = 0; i < split.Length; i++)
                {
                    if (split[i] == "")
                    {
                        deComboBox5.Text = "";
                        deTextBox24.Text = "";
                        deTextBox25.Text = "";
                        return;
                    }
                    else
                    {
                        if (i == 0 && split[0] != "" && searchLCCaseType(split[0]).Rows.Count >0)
                        {
                            deComboBox5.Text = split[0];
                            deComboBox5.Select();
                        }
                        else if (i== 1 && split[1] != "")
                        {
                            deTextBox24.Text = split[1];
                        }
                        else if (i== 2 && split[2] != "")
                        {
                            deTextBox25.Text = split[2];
                        }
                        else
                        {
                            deComboBox5.Text = "";
                            deTextBox24.Text = "";
                            deTextBox25.Text = "";
                            return;
                        }
                    }
                }

                
            }
            catch (Exception ex)
            {
                return;
                // MessageBox.Show(ex.Message);
            }

            

            }

        private void deButton9_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2)
            {
                if(deComboBox5.Text != "" && searchLCCaseType(deComboBox5.Text).Rows.Count >0)
                {
                    if(deTextBox24.Text != "" && deTextBox25.Text != "")
                    {
                        string lc_case_number = deComboBox5.Text + "/" + deTextBox24.Text + "/" + deTextBox25.Text;

                        for (int i = 0; i < listView4.Items.Count; i++)
                        {
                            if (listView4.Items[i].SubItems[0].Text == lc_case_number)
                            {
                                MessageBox.Show("This Lower court case number is already added...");
                                deComboBox5.Focus();
                                return;
                            }
                            else
                            {
                                continue;
                            }
                        }

                        if (listView4.SelectedItems[0].Selected == true)
                        {
                            listView4.SelectedItems[0].SubItems[0].Text = lc_case_number;
                            listView4.Select();
                            deComboBox5.Text = "";
                            deTextBox24.Text = "";
                            deTextBox25.Text = "";
                        }
                    }
                }
            }
        }

        private void listView9_SelectedIndexChanged(object sender, EventArgs e)
        {
            listView9.Select();
        }

        private void listView9_KeyUp(object sender, KeyEventArgs e)
        {
            if (listView9.Items.Count > 0)
            {
                if (listView9.SelectedItems.Count > 0)
                {
                    if (e.KeyCode == Keys.F2 || e.KeyCode == Keys.Enter)
                    {
                        listView9_DoubleClick(sender, e);
                    }
                    if (e.KeyCode == Keys.Delete)
                    {
                        deButton12_Click(sender, e);
                    }
                }
                else
                {
                    //listView2.Items[0].Selected = true;
                    //listView2.Items[0].Focused = true;
                    listView9.Select();
                }

            }
        }

        private void listView9_DoubleClick(object sender, EventArgs e)
        {
            listView9.Select();

           
            string name1 = "";

            try
            {

                string[] split = listView9.SelectedItems[0].SubItems[0].Text.Split(' ');

                foreach (string lcjudge in split)
                {
                    Console.WriteLine(lcjudge);
                    if (lcjudge == null || lcjudge == "" || lcjudge == "HON`BLE" || lcjudge == "JUSTICE" || lcjudge == "CHIEF" || lcjudge == "ACTING")
                    {
                    }
                    else
                    {
                        if (name1 == "")
                        {
                            name1 = lcjudge;
                        }
                        else
                        {
                            name1 = name1 + " " + lcjudge;
                        }

                    }
                }
                if (searchJudge(name1).Rows.Count > 0)
                {
                    deComboBox6.Text = name1;
                    deComboBox6.Select();
                }
                else
                {
                    deComboBox6.Text = "";
                    return;
                }

            }
            catch (Exception ex)
            {
                return;
                // MessageBox.Show(ex.Message);
            }
        }

        private void deButton13_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2)
            {
                if (deComboBox6.Text != "" && searchJudge(deComboBox6.Text.Trim()).Rows.Count > 0)
                {
                    string lc_judge_name = "HON`BLE " + deComboBox6.SelectedValue.ToString() + " " + deComboBox6.Text;

                    for (int i = 0; i < listView9.Items.Count; i++)
                    {
                        if (listView9.Items[i].SubItems[0].Text == lc_judge_name)
                        {
                            MessageBox.Show("This Lower Court Judge name is already added...");
                            deComboBox6.Focus();
                            return;
                        }
                        else
                        {
                            continue;
                        }
                    }
                    if (listView9.SelectedItems[0].Selected == true)
                    {
                        listView9.SelectedItems[0].SubItems[0].Text = lc_judge_name;
                        listView9.Select();
                        deComboBox6.Text = "";
                    }
                }
            }
        }

        private void listView5_SelectedIndexChanged(object sender, EventArgs e)
        {
            listView5.Select();
        }

        private void listView5_KeyUp(object sender, KeyEventArgs e)
        {
            if (listView5.Items.Count > 0)
            {
                if (listView5.SelectedItems.Count > 0)
                {
                    if (e.KeyCode == Keys.F2 || e.KeyCode == Keys.Enter)
                    {
                        listView5_DoubleClick(sender, e);
                    }
                    if (e.KeyCode == Keys.Delete)
                    {
                        deButton14_Click(sender, e);
                    }
                }
                else
                {
                    //listView2.Items[0].Selected = true;
                    //listView2.Items[0].Focused = true;
                    listView5.Select();
                }

            }
        }

        private void listView5_DoubleClick(object sender, EventArgs e)
        {
            listView5.Select();


            try
            {
                string[] split = listView5.SelectedItems[0].SubItems[0].Text.Split('/');

                for (int i = 0; i < split.Length; i++)
                {
                    if (split[i] == "")
                    {
                        deComboBox7.Text = "";
                        deTextBox31.Text = "";
                        deTextBox32.Text = "";
                        return;
                    }
                    else
                    {
                        if (i == 0 && split[0] != "" && searchCaseType(split[0]).Rows.Count > 0)
                        {
                            deComboBox7.Text = split[0];
                            deComboBox7.Select();
                        }
                        else if (i == 1 && split[1] != "")
                        {
                            deTextBox31.Text = split[1];
                        }
                        else if (i == 2 && split[2] != "")
                        {
                            deTextBox32.Text = split[2];
                        }
                        else
                        {
                            deComboBox7.Text = "";
                            deTextBox31.Text = "";
                            deTextBox32.Text = "";
                            return;
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                return;
                // MessageBox.Show(ex.Message);
            }
        }

        private void deButton15_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2)
            {
                if (deComboBox7.Text != "" && searchCaseType(deComboBox7.Text).Rows.Count > 0)
                {
                    if (deTextBox31.Text != "" && deTextBox32.Text != "")
                    {
                        string conn_app_case_number = deComboBox7.Text + "/" + deTextBox31.Text + "/" + deTextBox32.Text;

                        for (int i = 0; i < listView5.Items.Count; i++)
                        {
                            if (listView5.Items[i].SubItems[0].Text == conn_app_case_number)
                            {
                                MessageBox.Show("This Connected Application case number is already added...");
                                deComboBox7.Focus();
                                return;
                            }
                            else
                            {
                                continue;
                            }
                        }

                        if (listView5.SelectedItems[0].Selected == true)
                        {
                            listView5.SelectedItems[0].SubItems[0].Text = conn_app_case_number;
                            listView5.Select();
                            deComboBox7.Text = "";
                            deTextBox31.Text = "";
                            deTextBox32.Text = "";
                        }
                    }
                }
            }
        }

        private void listView10_SelectedIndexChanged(object sender, EventArgs e)
        {
            listView10.Select();
        }

        private void listView10_KeyUp(object sender, KeyEventArgs e)
        {
            if (listView10.Items.Count > 0)
            {
                if (listView10.SelectedItems.Count > 0)
                {
                    if (e.KeyCode == Keys.F2 || e.KeyCode == Keys.Enter)
                    {
                        listView10_DoubleClick(sender, e);
                    }
                    if (e.KeyCode == Keys.Delete)
                    {
                        deButton16_Click(sender, e);
                    }
                }
                else
                {
                    //listView2.Items[0].Selected = true;
                    //listView2.Items[0].Focused = true;
                    listView10.Select();
                }

            }
        }

        private void listView10_DoubleClick(object sender, EventArgs e)
        {
            listView10.Select();


            try
            {
                string[] split = listView10.SelectedItems[0].SubItems[0].Text.Split('/');

                for (int i = 0; i < split.Length; i++)
                {
                    if (split[i] == "")
                    {
                        deComboBox9.Text = "";
                        deTextBox35.Text = "";
                        deTextBox36.Text = "";
                        return;
                    }
                    else
                    {
                        if (i == 0 && split[0] != "" && searchCaseType(split[0]).Rows.Count > 0)
                        {
                            deComboBox9.Text = split[0];
                            deComboBox9.Select();
                        }
                        else if (i == 1 && split[1] != "")
                        {
                            deTextBox35.Text = split[1];
                        }
                        else if (i == 2 && split[2] != "")
                        {
                            deTextBox36.Text = split[2];
                        }
                        else
                        {
                            deComboBox9.Text = "";
                            deTextBox35.Text = "";
                            deTextBox36.Text = "";
                            return;
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                return;
                // MessageBox.Show(ex.Message);
            }
        }

        private void deButton17_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2)
            {
                if (deComboBox9.Text != "" && searchCaseType(deComboBox9.Text).Rows.Count > 0)
                {
                    if (deTextBox35.Text != "" && deTextBox36.Text != "")
                    {
                        string conn_main_case_number = deComboBox9.Text + "/" + deTextBox35.Text + "/" + deTextBox36.Text;

                        for (int i = 0; i < listView10.Items.Count; i++)
                        {
                            if (listView10.Items[i].SubItems[0].Text == conn_main_case_number)
                            {
                                MessageBox.Show("This Connected Main case number is already added...");
                                deComboBox9.Focus();
                                return;
                            }
                            else
                            {
                                continue;
                            }
                        }

                        if (listView10.SelectedItems[0].Selected == true)
                        {
                            listView10.SelectedItems[0].SubItems[0].Text = conn_main_case_number;
                            listView10.Select();
                            deComboBox9.Text = "";
                            deTextBox35.Text = "";
                            deTextBox36.Text = "";
                        }
                    }
                }
            }
        }

        private void listView6_SelectedIndexChanged(object sender, EventArgs e)
        {
            listView6.Select();
        }

        private void listView6_KeyUp(object sender, KeyEventArgs e)
        {
            if (listView6.Items.Count > 0)
            {
                if (listView6.SelectedItems.Count > 0)
                {
                    if (e.KeyCode == Keys.F2 || e.KeyCode == Keys.Enter)
                    {
                        listView6_DoubleClick(sender, e);
                    }
                    if (e.KeyCode == Keys.Delete)
                    {
                        deButton18_Click(sender, e);
                    }
                }
                else
                {
                    //listView2.Items[0].Selected = true;
                    //listView2.Items[0].Focused = true;
                    listView6.Select();
                }

            }
        }

        private void listView6_DoubleClick(object sender, EventArgs e)
        {
            listView6.Select();


            try
            {
                string[] split = listView6.SelectedItems[0].SubItems[0].Text.Split('/');

                for (int i = 0; i < split.Length; i++)
                {
                    if (split[i] == "")
                    {
                        deComboBox10.Text = "";
                        deTextBox38.Text = "";
                        deTextBox39.Text = "";
                        return;
                    }
                    else
                    {
                        if (i == 0 && split[0] != "" && searchCaseType(split[0]).Rows.Count > 0)
                        {
                            deComboBox10.Text = split[0];
                            deComboBox10.Select();
                        }
                        else if (i == 1 && split[1] != "")
                        {
                            deTextBox38.Text = split[1];
                        }
                        else if (i == 2 && split[2] != "")
                        {
                            deTextBox39.Text = split[2];
                        }
                        else
                        {
                            deComboBox10.Text = "";
                            deTextBox38.Text = "";
                            deTextBox39.Text = "";
                            return;
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                return;
                // MessageBox.Show(ex.Message);
            }
        }

        private void deButton19_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2)
            {
                if (deComboBox10.Text != "" && searchCaseType(deComboBox10.Text).Rows.Count > 0)
                {
                    if (deTextBox38.Text != "" && deTextBox39.Text != "")
                    {
                        string analogous_case_number = deComboBox10.Text + "/" + deTextBox38.Text + "/" + deTextBox39.Text;

                        for (int i = 0; i < listView6.Items.Count; i++)
                        {
                            if (listView6.Items[i].SubItems[0].Text == analogous_case_number)
                            {
                                MessageBox.Show("This Analogous case number is already added...");
                                deComboBox10.Focus();
                                return;
                            }
                            else
                            {
                                continue;
                            }
                        }

                        if (listView6.SelectedItems[0].Selected == true)
                        {
                            listView6.SelectedItems[0].SubItems[0].Text = analogous_case_number;
                            listView6.Select();
                            deComboBox10.Text = "";
                            deTextBox38.Text = "";
                            deTextBox39.Text = "";
                        }
                    }
                }
            }
        }

     }
}
