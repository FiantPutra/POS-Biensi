using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Runtime.Serialization.Json;
using System.Data.SqlClient;
using Newtonsoft.Json;

namespace try_bi
{
    class API_UpdateSyncDetail
    {
        String link_api;        
        koneksi ckon = new koneksi();
        LinkApi link = new LinkApi();
        CRUD sql = new CRUD();

        public void updateSync()
        {
            updateSyncDetailReq().Wait();
        }

        public async Task updateSyncDetailReq()
        {
            String syncDetailId = "";
            String rowFatch = "";
            String rowApplied = "";
            String status = "";
            String downloadSessionId = "";

            link_api = link.aLink;
            bracketSyncDetail updateSyncs = new bracketSyncDetail();
            List<updateSyncDetailDownload> updateSyncDetails = new List<updateSyncDetailDownload>();
            updateSyncs.syncDetailDownload = new List<updateSyncDetailDownload>();
            try
            {
                ckon.sqlConMsg().Open();
                String cmd = "SELECT * FROM JobSynchDetailDownloadStatus WHERE RowApplied != 0 AND status = 1 ORDER BY SynchDetail ASC";                
                ckon.sqlDataRd = sql.ExecuteDataReader(cmd, ckon.sqlConMsg());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        syncDetailId = Convert.ToString(ckon.sqlDataRd["SynchDetail"]);
                        rowApplied = Convert.ToString(ckon.sqlDataRd["RowApplied"]);
                        rowFatch = Convert.ToString(ckon.sqlDataRd["RowFatch"]);
                        status = Convert.ToString(ckon.sqlDataRd["Status"]);
                        downloadSessionId = Convert.ToString(ckon.sqlDataRd["Downloadsessionid"]);

                        updateSyncDetailDownload tmp = new updateSyncDetailDownload();
                        tmp.syncDetailsId = syncDetailId;
                        tmp.rowApplied = rowApplied;
                        tmp.rowFatch = rowFatch;
                        tmp.status = status;
                        tmp.downloadSessionId = downloadSessionId;                        
                        updateSyncs.syncDetailDownload.Add(tmp);
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

            var syncData = JsonConvert.SerializeObject(updateSyncs);
            String response = "";
            var credentials = new NetworkCredential("username", "password");
            var handler = new HttpClientHandler { Credentials = credentials };
            var httpContent = new StringContent(syncData, Encoding.UTF8, "application/json");
            using (var client = new HttpClient(handler))
            {                
                HttpResponseMessage message = client.PostAsync(link_api + "/homsg/updateSyncDownload", httpContent).Result;
            }
        }            
    }
}
