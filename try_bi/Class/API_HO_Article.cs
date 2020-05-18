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
    class API_HO_Article
    {
        String link_api;
        koneksi ckon = new koneksi();
        LinkApi link = new LinkApi();

        public void execArticle()
        {
            delete();
            getArticle().Wait();
        }
        //======================================DELETE DATA BEFORE GET FROM API==================================
        public void delete()
        {
            ckon.con.Close();
            String sql = "DELETE FROM article_ho";
            CRUD exec = new CRUD();
            exec.ExecuteNonQuery(sql);
        }
        //=======================================================================================================
        public async Task getArticle()
        {
            link_api = link.aLink;

            var credentials = new NetworkCredential("username", "password");
            var handler = new HttpClientHandler { Credentials = credentials };
            
            using (var client = new HttpClient(handler))
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    HttpResponseMessage message = client.GetAsync(link_api + "/api/HOArticle").Result;
                    string ConnectionString = ckon.config; //"Server='" + try_bi.Properties.Settings.Default.mServer + "';Database='" + try_bi.Properties.Settings.Default.mDBName + "';Uid='" + try_bi.Properties.Settings.Default.mUserDB + "';Pwd='" + try_bi.Properties.Settings.Default.mPassDB + "';";
                    StringBuilder sCommand = new StringBuilder("INSERT INTO article_ho (_id ,ARTICLE_ID, ARTICLE_NAME, BRAND, GENDER, DEPARTMENT, DEPARTMENT_TYPE, SIZE, COLOR, UNIT, PRICE, ARTICLE_ID_ALIAS, IS_SERVICE) VALUES");

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
                                try
                                {
                                    myCmd.CommandType = CommandType.Text;
                                    myCmd.ExecuteNonQuery();
                                    MessageBox.Show("Success");
                                } 
                                catch (Exception ep)
                                {
                                    MessageBox.Show("Failed! Try again!");
                                }
                            }

                        }
                    } else
                    {
                        MessageBox.Show("Failed! Try again!");
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
