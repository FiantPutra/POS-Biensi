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
using System.Data.SqlClient;
using System.Runtime.Serialization.Json;
using Microsoft.ApplicationBlocks.Data;

namespace try_bi
{
    class API_Article
    {
        String cust_id_store, link_api;
        koneksi ckon = new koneksi();
        LinkApi link = new LinkApi();

        public void execArticle()
        {
            delete();
            get_cust_id();
            getArticle().Wait();
        }
        //======================================================================================================
        public void get_cust_id()
        {
            CRUD sql = new CRUD();
            
            try
            {
                ckon.sqlCon().Open();
                String cmd = "SELECT * FROM store";
                ckon.sqlDataRd = sql.ExecuteDataReader(cmd, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        cust_id_store = ckon.sqlDataRd["CUST_ID_STORE"].ToString();
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

        //======================================DELETE DATA BEFORE GET FROM API==================================
        public void delete()
        {            
            String cmd_delete = "DELETE FROM article";
            CRUD delete = new CRUD();
            delete.ExecuteNonQuery(cmd_delete);
        }
        //=======================================================================================================
        public async Task getArticle()
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
                    HttpResponseMessage message = client.GetAsync(link_api + "/api/Article?customerCode=" + cust_id_store).Result;

                    string ConnectionString = "Server='" + try_bi.Properties.Settings.Default.mServer + "';Database='" + try_bi.Properties.Settings.Default.mDBName + "';Uid='" + try_bi.Properties.Settings.Default.mUserDB + "';Pwd='" + try_bi.Properties.Settings.Default.mPassDB + "';";
                    StringBuilder sCommand = new StringBuilder("INSERT INTO article (_id ,ARTICLE_ID, ARTICLE_NAME, BRAND, GENDER, DEPARTMENT, DEPARTMENT_TYPE, SIZE, COLOR, UNIT, PRICE, ARTICLE_ID_ALIAS, IS_SERVICE) VALUES");

                    if (message.IsSuccessStatusCode)
                    {
                        var serializer = new DataContractJsonSerializer(typeof(List<Article>));
                        var result = message.Content.ReadAsStringAsync().Result;
                        byte[] byteArray = Encoding.UTF8.GetBytes(result);
                        MemoryStream stream = new MemoryStream(byteArray);
                        List<Article> resultData = serializer.ReadObject(stream) as List<Article>;
                        //====================================================================================
                        using (MySqlConnection mConnection = new MySqlConnection(ConnectionString))
                        {                            
                            List<string> Rows = new List<string>();
                            for (int i = 0; i < resultData.Count; i++)
                            {
                                int isService = resultData[i].isService ? 1 : 0;
                                Rows.Add(string.Format("('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}')", MySqlHelper.EscapeString(resultData[i].id.ToString()), MySqlHelper.EscapeString(resultData[i].articleId), MySqlHelper.EscapeString(resultData[i].articleName), MySqlHelper.EscapeString(resultData[i].brand), MySqlHelper.EscapeString(resultData[i].gender), MySqlHelper.EscapeString(resultData[i].department), MySqlHelper.EscapeString(resultData[i].departmentType), MySqlHelper.EscapeString(resultData[i].size), MySqlHelper.EscapeString(resultData[i].color), MySqlHelper.EscapeString(resultData[i].unit), MySqlHelper.EscapeString(resultData[i].price.ToString()), MySqlHelper.EscapeString(resultData[i].articleIdAlias), isService));
                            }
                            sCommand.Append(string.Join(",", Rows));
                            sCommand.Append(";");
                            mConnection.Open();
                            using (MySqlCommand myCmd = new MySqlCommand(sCommand.ToString(), mConnection))
                            {
                                myCmd.CommandType = CommandType.Text;
                                myCmd.ExecuteNonQuery();

                                //String query = "UPDATE log_msg SET Status='Success' WHERE Data = 'Article' ";
                                //Crud update = new Crud();
                                //update.NonReturn2(query);
                                //MessageBox.Show("Successful Update Data Article", "Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
