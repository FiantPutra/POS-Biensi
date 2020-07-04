using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using try_bi.Class;

namespace try_bi
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            FileTransferScheduler.IntervalInMinutes(10, 30, 5,
                () =>
                {
                    UploadSyncFile uploadSyncFile = new UploadSyncFile();

                    uploadSyncFile.SyncUpload();
                });

            FileTransferScheduler.IntervalInMinutes(10, 30, 30,
                () => {
                    DownloadSyncFile downloadSync = new DownloadSyncFile();

                    downloadSync.SyncDownload();
                });

            Application.Run(new Form_Login());
        }
    }
}
