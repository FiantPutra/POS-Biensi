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
namespace try_bi
{
    class API_FTPServer
    {
        String link_api;
        String storeCode = "";
        koneksi ckon = new koneksi();
        LinkApi link = new LinkApi();
        CRUD sql = new CRUD();

        public List<ftpServer> FTPServerRequest()
        {
            link_api = link.aLink;

            String response = "";
            var credentials = new NetworkCredential("username", "password");
            var handler = new HttpClientHandler { Credentials = credentials };
            ftpServerList ftpServerList = new ftpServerList();
            
            using (var client = new HttpClient(handler))
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    HttpResponseMessage message = client.GetAsync(link_api + "/homsg/ftpserver").Result;

                    if (message.IsSuccessStatusCode)
                    {
                        var serializer = new DataContractJsonSerializer(typeof(List<ftpServer>));
                        var result = message.Content.ReadAsStringAsync().Result;
                        byte[] byteArray = Encoding.UTF8.GetBytes(result);
                        MemoryStream stream = new MemoryStream(byteArray);
                        List<ftpServer> resultData = serializer.ReadObject(stream) as List<ftpServer>;
                        //String connectionString = ckon.msgSqlCon;

                        //using (SqlConnection mConnection = new SqlConnection(connectionString))
                        //{
                        //    mConnection.Open();
                        //    String cmd_insert = "IF NOT EXISTS (SELECT * FROM JobTabletoSynchDetailDownload WHERE SynchDetail = @PARM1) " +
                        //                            "BEGIN " +
                        //                            "INSERT INTO JobTabletoSynchDetailDownload(SynchDetail, JobID, StoreID, TableName, DownloadPath, Synchdate, CreateTable, RowFatch, SyncType, TablePrimarykey) " +
                        //                            "VALUES(@PARM1, @PARM2, @PARM3, @PARM4, @PARM5, @PARM6, @PARM7, @PARM8, @PARM9, @PARM10) " +
                        //                             "END";
                        //    SqlCommand cmd = new SqlCommand(cmd_insert);
                        //    cmd.Connection = mConnection;
                        //    cmd.CommandType = CommandType.Text;
                        //    cmd.Parameters.Add("@PARM1", DbType.Int64);
                        //    cmd.Parameters.Add("@PARM2", DbType.Int32);
                        //    cmd.Parameters.Add("@PARM3", DbType.String);
                        //    cmd.Parameters.Add("@PARM4", DbType.String);
                        //    cmd.Parameters.Add("@PARM5", DbType.String);
                        //    cmd.Parameters.Add("@PARM6", DbType.DateTime);
                        //    cmd.Parameters.Add("@PARM7", DbType.String);
                        //    cmd.Parameters.Add("@PARM8", DbType.Int32);
                        //    cmd.Parameters.Add("@PARM9", DbType.Int32);
                        //    cmd.Parameters.Add("@PARM10", DbType.String);

                        //    for (int i = 0; i < resultData.Count; i++)
                        //    {
                        //        cmd.Parameters[0].Value = Convert.ToInt64(resultData[i].IdDetail);
                        //        cmd.Parameters[1].Value = Convert.ToInt32(resultData[i].JobId);
                        //        cmd.Parameters[2].Value = resultData[i].StoreId;
                        //        cmd.Parameters[3].Value = resultData[i].TableName;
                        //        cmd.Parameters[4].Value = resultData[i].DownloadPath;
                        //        cmd.Parameters[5].Value = resultData[i].SyncDate;
                        //        cmd.Parameters[6].Value = resultData[i].createTable;
                        //        cmd.Parameters[7].Value = resultData[i].RowFatch;
                        //        cmd.Parameters[8].Value = resultData[i].syncType;
                        //        cmd.Parameters[9].Value = resultData[i].tablePrimaryKey;
                        //        cmd.ExecuteNonQuery();
                        //    }
                        //    mConnection.Close();
                        //}

                        ftpServerList.ftpServers = resultData;
                    }                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "No Connection", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                return ftpServerList.ftpServers;
            }            
        }
    }
}
