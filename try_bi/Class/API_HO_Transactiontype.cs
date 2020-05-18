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

namespace try_bi
{
    class API_HO_Transactiontype
    {
        String link_api;
        koneksi ckon = new koneksi();
        LinkApi link = new LinkApi();

        public void execData()
        {
            delete();
            getData().Wait();
        }
        //======================================DELETE DATA BEFORE GET FROM API==================================
        public void delete()
        {
            ckon.con.Close();
            String sql = "DELETE FROM ho_transaction_type";
            CRUD exec = new CRUD();
            exec.ExecuteNonQuery(sql);
        }
        //=======================================================================================================
        public async Task getData()
        {
            link_api = link.aLink;

            var credentials = new NetworkCredential("username", "password");
            var handler = new HttpClientHandler { Credentials = credentials };

            using (var client = new HttpClient(handler))
            {
                // Make your request...
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    HttpResponseMessage message = client.GetAsync(link_api + "/api/HOTransactionType").Result;
                    string ConnectionString = ckon.config;// "Server='" + try_bi.Properties.Settings.Default.mServer + "';Database='" + try_bi.Properties.Settings.Default.mDBName + "';Uid='" + try_bi.Properties.Settings.Default.mUserDB + "';Pwd='" + try_bi.Properties.Settings.Default.mPassDB + "';";
                    StringBuilder sCommand = new StringBuilder("INSERT INTO ho_transaction_type (ID,CODE,DESCRIPTION,WAREHOUSE_FROM,WAREHOUSE_TO) VALUES");

                    if (message.IsSuccessStatusCode)
                    {
                        var serializer = new DataContractJsonSerializer(typeof(List<HoTransactionType>));
                        var result = message.Content.ReadAsStringAsync().Result;
                        byte[] byteArray = Encoding.UTF8.GetBytes(result);
                        MemoryStream stream = new MemoryStream(byteArray);
                        List<HoTransactionType> resultData = serializer.ReadObject(stream) as List<HoTransactionType>;
                        //====================================================================================
                        using (MySqlConnection mConnection = new MySqlConnection(ConnectionString))
                        {

                            List<string> Rows = new List<string>();
                            for (int i = 0; i < resultData.Count; i++)
                            {
                                Rows.Add(string.Format("('{0}','{1}','{2}','{3}','{4}')", MySqlHelper.EscapeString(resultData[i].Id.ToString()), MySqlHelper.EscapeString(resultData[i].Code), MySqlHelper.EscapeString(resultData[i].Description), MySqlHelper.EscapeString(resultData[i].WarehouseFrom), MySqlHelper.EscapeString(resultData[i].WarehouseTo)));
                            }
                            sCommand.Append(string.Join(",", Rows));
                            sCommand.Append(";");
                            mConnection.Open();
                            using (MySqlCommand myCmd = new MySqlCommand(sCommand.ToString(), mConnection))
                            {
                                myCmd.CommandType = CommandType.Text;
                                myCmd.ExecuteNonQuery();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "No Connection", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

    }
}
