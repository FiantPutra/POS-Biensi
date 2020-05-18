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
    class API_UploadSync
    {
        String link_api;
        koneksi ckon = new koneksi();
        LinkApi link = new LinkApi();
        CRUD sql = new CRUD();

        public void uploadSync(String SyncDetail)
        {
            uploadSyncDetailReq(SyncDetail).Wait();
        }

        public async Task uploadSyncDetailReq(String SyncDetail)
        {
            int syncDetailId = 0;
            int jobId = 0;
            String storeId = "";
            String tableName = "";
            String uploadPath = "";
            DateTime syncDate = DateTime.MinValue;
            String createTable = "";
            int rowFatch = 0;
            int minId = 0;
            int maxId = 0;
            String tablePrimaryKey = "";
            String identityColumn = "";

            link_api = link.aLink;
            bracketSyncUploadDetail uploadSyncs = new bracketSyncUploadDetail();
            List<syncUploadDetail> uploadSyncDetails = new List<syncUploadDetail>();
            uploadSyncs.uploadDetails = new List<syncUploadDetail>();
            try
            {
                ckon.sqlConMsg().Open();
                String cmd = "SELECT SynchDetail, JobID, StoreID, TableName, UploadPath, Synchdate, CreateTable, RowFatch, MinId, MaxId, TablePrimaryKey, identityColumn FROM JobTabletoSynchDetailUpload "
                                + "WHERE SynchDetail = "+ SyncDetail + "";
                ckon.sqlDataRd = sql.ExecuteDataReader(cmd, ckon.sqlConMsg());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        syncDetailId = Convert.ToInt32(ckon.sqlDataRd["SynchDetail"]);
                        jobId = Convert.ToInt32(ckon.sqlDataRd["JobID"]);
                        storeId = Convert.ToString(ckon.sqlDataRd["StoreID"]);
                        tableName = Convert.ToString(ckon.sqlDataRd["TableName"]);
                        uploadPath = Convert.ToString(ckon.sqlDataRd["UploadPath"]);
                        syncDate = Convert.ToDateTime(ckon.sqlDataRd["Synchdate"]);
                        createTable = Convert.ToString(ckon.sqlDataRd["CreateTable"]);
                        rowFatch = Convert.ToInt32(ckon.sqlDataRd["rowFatch"]);
                        minId = Convert.ToInt32(ckon.sqlDataRd["MinId"]);
                        maxId = Convert.ToInt32(ckon.sqlDataRd["MaxId"]);
                        tablePrimaryKey = Convert.ToString(ckon.sqlDataRd["TablePrimaryKey"]);
                        identityColumn = Convert.ToString(ckon.sqlDataRd["identityColumn"]);

                        syncUploadDetail tmp = new syncUploadDetail();
                        tmp.syncDetailsId = syncDetailId;
                        tmp.JobId = jobId;
                        tmp.StoreId = storeId;
                        tmp.TableName = tableName;
                        tmp.UploadPath = uploadPath;
                        tmp.Synchdate = syncDate;
                        tmp.CreateTable = createTable;
                        tmp.RowFatch = rowFatch;
                        tmp.minId = minId;
                        tmp.maxId = maxId;
                        tmp.TablePrimaryKey = tablePrimaryKey;
                        tmp.identityColumn = identityColumn;
                        uploadSyncs.uploadDetails.Add(tmp);
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

            var syncData = JsonConvert.SerializeObject(uploadSyncs);
            String response = "";
            var credentials = new NetworkCredential("username", "password");
            var handler = new HttpClientHandler { Credentials = credentials };
            var httpContent = new StringContent(syncData, Encoding.UTF8, "application/json");
            using (var client = new HttpClient(handler))
            {
                HttpResponseMessage message = client.PostAsync(link_api + "/homsg/Upload", httpContent).Result;
            }
        }
    }
}
