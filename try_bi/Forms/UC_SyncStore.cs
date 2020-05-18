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
    public partial class UC_SyncStore : UserControl
    {
        public static Form1 f1;
        koneksi ckon = new koneksi();
        private static UC_SyncStore _instance;
        LinkApi xmlCon = new LinkApi();

        public static UC_SyncStore Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new UC_SyncStore(f1);
                return _instance;
            }
        }
        public UC_SyncStore(Form1 form1)
        {
            f1 = form1;

            InitializeComponent();
        }

        private void b_DownloadFiles_Click(object sender, EventArgs e)
        {
            UC_SyncDownloadFile downloadFile = new UC_SyncDownloadFile(f1);
            
            f1.p_kanan.Controls.Clear();
            if (!f1.p_kanan.Controls.Contains(UC_SyncDownloadFile.Instance))
            {
                f1.p_kanan.Controls.Add(UC_SyncDownloadFile.Instance);
                UC_SyncDownloadFile.Instance.Dock = DockStyle.Fill;
                UC_SyncDownloadFile.Instance.retreive();
                UC_SyncDownloadFile.Instance.BringToFront();
            }
            else
            {
                UC_SyncDownloadFile.Instance.BringToFront();
            }
        }

        private void b_UploadFile_Click(object sender, EventArgs e)
        {
            UC_SyncUploadFile uploadFile = new UC_SyncUploadFile(f1);
            
            f1.p_kanan.Controls.Clear();
            if (!f1.p_kanan.Controls.Contains(UC_SyncUploadFile.Instance))
            {
                f1.p_kanan.Controls.Add(UC_SyncUploadFile.Instance);
                UC_SyncUploadFile.Instance.Dock = DockStyle.Fill;
                UC_SyncUploadFile.Instance.retreive();
                UC_SyncUploadFile.Instance.BringToFront();
            }
            else
            {
                UC_SyncUploadFile.Instance.BringToFront();
            }
        }
    }
}
