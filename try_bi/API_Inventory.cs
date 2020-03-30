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
    class API_Inventory
    {
        LinkApi link = new LinkApi();

        koneksi ckon = new koneksi();
        String store_code, link_api;
        public void SetInvHitLocal()
        {
            delete();
            get_NameStore();
            GetInv().Wait();
        }
        //======================================================================================================
        public void get_NameStore()
        {            
            String sql = "SELECT * FROM store";
            ckon.cmd = new MySqlCommand(sql, ckon.con);
            ckon.con.Open();
            ckon.myReader = ckon.cmd.ExecuteReader();
            while (ckon.myReader.Read())
            {
                store_code = ckon.myReader.GetString("CODE");
            }
            ckon.con.Close();
        }
        //======================================DELETE DATA BEFORE GET FROM API==================================
        public void delete()
        {
            ckon.con.Close();
            String sql = "DELETE FROM inventory";
            CRUD delete = new CRUD();
            delete.ExecuteNonQuery(sql);

        }
        //==============================================================================
        public async Task GetInv()
        {
            link_api = link.aLink;

            String response = "";
            var credentials = new NetworkCredential("username", "password");
            var handler = new HttpClientHandler { Credentials = credentials }; // for validation
                                                                               //    handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };// allow domain checker
            using (var client = new HttpClient(handler))
            {
                // Make your request...
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    //HttpResponseMessage message = client.GetAsync("http://retailbiensi.azurewebsites.net/api/Inventory?warehouseId=" + store_code).Result;
                    //HttpResponseMessage message = client.GetAsync("http://mpos.biensicore.co.id/api/Inventory?warehouseId=" + store_code).Result;
                    HttpResponseMessage message = client.GetAsync(link_api+"/api/Inventory?warehouseId=" + store_code).Result;

                    string ConnectionString = ckon.config;// "Server='" + try_bi.Properties.Settings.Default.mServer + "';Database='" + try_bi.Properties.Settings.Default.mDBName + "';Uid='" + try_bi.Properties.Settings.Default.mUserDB + "';Pwd='" + try_bi.Properties.Settings.Default.mPassDB + "';";
                    StringBuilder sCommand = new StringBuilder("INSERT INTO inventory (_id ,ARTICLE_ID, GOOD_QTY, REJECT_QTY, WH_GOOD_QTY, WH_REJECT_QTY, STATUS) VALUES");

                    if (message.IsSuccessStatusCode)
                    {
                        var serializer = new DataContractJsonSerializer(typeof(List<Inventory>));
                        var result = message.Content.ReadAsStringAsync().Result;
                        byte[] byteArray = Encoding.UTF8.GetBytes(result);
                        MemoryStream stream = new MemoryStream(byteArray);
                        List<Inventory> resultData = serializer.ReadObject(stream) as List<Inventory>;

                        
                        using (MySqlConnection mConnection = new MySqlConnection(ConnectionString))
                        {

                            List<string> Rows = new List<string>();
                            for (int i = 0; i < resultData.Count; i++)
                            {
                                Rows.Add(string.Format("('{0}','{1}','{2}','{3}','{4}','{5}','{6}')", MySqlHelper.EscapeString(resultData[i].id.ToString()), MySqlHelper.EscapeString(resultData[i].articleId), MySqlHelper.EscapeString(resultData[i].goodQty), MySqlHelper.EscapeString(resultData[i].rejectQty), MySqlHelper.EscapeString(resultData[i].whGoodQty), MySqlHelper.EscapeString(resultData[i].whRejectQty), MySqlHelper.EscapeString(resultData[i].status)));
                            }
                            sCommand.Append(string.Join(",", Rows));
                            sCommand.Append(";");
                            mConnection.Open();
                            using (MySqlCommand myCmd = new MySqlCommand(sCommand.ToString(), mConnection))
                            {
                                myCmd.CommandType = CommandType.Text;
                                myCmd.ExecuteNonQuery();

                                //String query = "UPDATE log_msg SET Status='Success' WHERE Data = 'Inventory' ";
                                //CRUD update = new CRUD();
                                //update.ExecuteNonQuery(query);
                                //MessageBox.Show("Successful Update Data Inventory", "Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
