using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using System.IO.Compression;
using System.Diagnostics;
using System.Data.SqlClient;

namespace try_bi
{
    public partial class UC_SyncUploadFile : UserControl
    {
        public static Form1 f1;
        koneksi ckon = new koneksi();
        private static UC_SyncUploadFile _instance;
        LinkApi xmlCon = new LinkApi();
        String tableName = "", jobId = "", storeId = "", downloadPath = "", rowFatch = "", rowApplied = "", status = "", syncDate = "", newStatus = "";

        private void b_back2_Click(object sender, EventArgs e)
        {
            UC_SyncStore syncStore = new UC_SyncStore(f1);

            f1.p_kanan.Controls.Clear();
            if (!f1.p_kanan.Controls.Contains(UC_SyncStore.Instance))
            {
                f1.p_kanan.Controls.Add(UC_SyncStore.Instance);
                UC_SyncStore.Instance.Dock = DockStyle.Fill;
                UC_SyncDownloadFile.Instance.BringToFront();
            }
            else
            {
                UC_SyncDownloadFile.Instance.BringToFront();
            }
        }

        private void b_uploadFTP_Click(object sender, EventArgs e)
        {
            UploadSyncFile uploadSyncFile = new UploadSyncFile();

            uploadSyncFile.SyncUpload();
            retreive();
        }

        public static UC_SyncUploadFile Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new UC_SyncUploadFile(f1);
                return _instance;
            }
        }
        public UC_SyncUploadFile(Form1 form1)
        {
            f1 = form1;

            InitializeComponent();
        }

        public void retreive()
        {
            CRUD sql = new CRUD();
            dgv_UploadFile.Rows.Clear();

            try
            {
                ckon.sqlConMsg().Open();
                String cmd = "SELECT a.TableName, a.JobID, a.StoreID, a.RowFatch, b.RowApplied, b.Status, a.SynchDate FROM JobTabletoSynchDetailUpload a " +
                                "INNER JOIN JobSynchDetailUploadStatus b ON b.SynchDetail = a.SynchDetail";
                ckon.sqlDataRd = sql.ExecuteDataReader(cmd, ckon.sqlConMsg());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        tableName = Convert.ToString(ckon.sqlDataRd["TableName"]);
                        jobId = Convert.ToString(ckon.sqlDataRd["JobID"]);
                        storeId = Convert.ToString(ckon.sqlDataRd["StoreID"]);
                        rowFatch = Convert.ToString(ckon.sqlDataRd["RowFatch"]);
                        rowApplied = Convert.ToString(ckon.sqlDataRd["RowApplied"]);
                        status = Convert.ToString(ckon.sqlDataRd["Status"]);
                        syncDate = Convert.ToString(ckon.sqlDataRd["SynchDate"]);                        


                        if (status == "0")
                            newStatus = "Not Uploaded";
                        else
                            newStatus = "Uploaded";

                        int dgrows = dgv_UploadFile.Rows.Add();

                        dgv_UploadFile.Rows[dgrows].Cells[0].Value = tableName;
                        dgv_UploadFile.Rows[dgrows].Cells[1].Value = rowFatch;
                        dgv_UploadFile.Rows[dgrows].Cells[2].Value = rowApplied;
                        dgv_UploadFile.Rows[dgrows].Cells[3].Value = newStatus;
                        dgv_UploadFile.Rows[dgrows].Cells[4].Value = syncDate;                        
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (ckon.sqlDataRd != null)
                    ckon.sqlDataRd.Close();

                if (ckon.sqlCon().State == ConnectionState.Open)
                    ckon.sqlCon().Close();
            }
        }
    }
}
