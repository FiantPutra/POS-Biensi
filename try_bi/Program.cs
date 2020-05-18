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

            FileTransferScheduler.IntervalInMinutes(13, 00, 5,
                () => {
                    UploadSyncFile uploadSyncFile = new UploadSyncFile();

                    uploadSyncFile.SyncUpload();
                });

            Application.Run(new Form_Login());
        }
    }
}
