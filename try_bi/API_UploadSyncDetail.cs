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
    class API_UploadSyncDetail
    {
        String link_api;
        koneksi ckon = new koneksi();
        LinkApi link = new LinkApi();
        CRUD sql = new CRUD();

        public async Task updateSyncDetailReq()
        {
            int syncDetailId = 0;
            int rowFatch = 0;                                    
            int jobId = 0;
            String storeId = "";
            String uploadPath = "";
            DateTime synchDate = DateTime.MinValue;
            String createTable = "";
            String tableName = "";
            int minId = 0;
            int MaxId = 0;


            link_api = link.aLink;
            bracketSyncUploadDetail uploadSyncs = new bracketSyncUploadDetail();            
            uploadSyncs.uploadDetails = new List<syncUploadDetail>();
            try
            {
                ckon.sqlConMsg().Open();
                String cmd = "SELECT JobID, StoreID, UploadPath, SynchDetail, RowFatch, SynchDate, CreateTable, TableName, MinId, MaxId FROM JobTabletoSynchDetailUpload " +
                                "WHERE SynchDetail NOT IN(SELECT SynchDetail FROM JobSynchDetailUploadStatus)";
                ckon.sqlDataRd = sql.ExecuteDataReader(cmd, ckon.sqlConMsg());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        syncDetailId = Convert.ToInt32(ckon.sqlDataRd["SynchDetail"]);
                        jobId = Convert.ToInt32(ckon.sqlDataRd["JobID"]);
                        rowFatch = Convert.ToInt32(ckon.sqlDataRd["RowFatch"]);
                        storeId = Convert.ToString(ckon.sqlDataRd["StoreId"]);
                        uploadPath = Convert.ToString(ckon.sqlDataRd["UploadPath"]);
                        synchDate = Convert.ToDateTime(ckon.sqlDataRd["SynchDate"]);
                        createTable = Convert.ToString(ckon.sqlDataRd["CreateTable"]);
                        minId = Convert.ToInt32(ckon.sqlDataRd["MinId"]);
                        MaxId = Convert.ToInt32(ckon.sqlDataRd["MaxId"]);

                        syncUploadDetail tmp = new syncUploadDetail();
                        tmp.SynchDetail = syncDetailId;
                        tmp.JobId = jobId;
                        tmp.StoreId = storeId;
                        tmp.UploadPath = uploadPath;
                        tmp.Synchdate = synchDate;
                        tmp.CreateTable = createTable;
                        tmp.minId = minId;
                        tmp.maxId = MaxId;
                        uploadSyncs.uploadDetails.Add(tmp);
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("No connection to database", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (ckon.sqlDataRd != null)
                    ckon.sqlDataRd.Close();

                if (ckon.sqlCon().State == ConnectionState.Open)
                    ckon.sqlCon().Close();
            }

            var syncData = JsonConvert.SerializeObject(uploadSyncs);
            String response = "";
            var credentials = new NetworkCredential("username", "password");
            var handler = new HttpClientHandler { Credentials = credentials };
            var httpContent = new StringContent(syncData, Encoding.UTF8, "application/json");
            using (var client = new HttpClient(handler))
            {
                HttpResponseMessage message = client.PostAsync(link_api + "/homsg/upload", httpContent).Result;
            }
        }
    }
}
