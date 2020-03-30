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
    class API_Article_Single
    {
        koneksi ckon = new koneksi();
        LinkApi link = new LinkApi();
        //=======================================================================================================
        public async Task getData(String articleID)
        {
            var credentials = new NetworkCredential("username", "password");
            var handler = new HttpClientHandler { Credentials = credentials };

            using (var client = new HttpClient(handler))
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    HttpResponseMessage message = client.GetAsync(link.aLink + "/api/HOArticle?articleId=" + articleID).Result;

                    if (message.IsSuccessStatusCode)
                    {
                        var serializer = new DataContractJsonSerializer(typeof(List<Article>));
                        var result = message.Content.ReadAsStringAsync().Result;
                        byte[] byteArray = Encoding.UTF8.GetBytes(result);
                        MemoryStream stream = new MemoryStream(byteArray);
                        List<Article> resultData = serializer.ReadObject(stream) as List<Article>;
                        //====================================================================================
                        ckon.con.Close();
                        String sql = "SELECT distinct ARTICLE_ID FROM article_ho WHERE ARTICLE_ID = '" + articleID + "'";
                        ckon.cmd = new MySqlCommand(sql, ckon.con);
                        ckon.con.Open();
                        ckon.myReader = ckon.cmd.ExecuteReader();
                        int count = 0;
                        string sql_statement = "";
                        if (ckon.myReader.HasRows)
                        {
                            count = count + 1;
                        }
                        ckon.con.Close();
                        //
                        List<string> Rows = new List<string>();
                        for (int i = 0; i < resultData.Count; i++)
                        {
                            int isService = resultData[i].isService ? 1 : 0;

                            if (count > 0)
                            {
                                //update
                                sql_statement = "UPDATE article_ho SET " +
                                    "_id='"+ MySqlHelper.EscapeString(resultData[i].id.ToString()) + "'," + 
                                    "ARTICLE_NAME='" + MySqlHelper.EscapeString(resultData[i].articleName) + "'," +
                                    "BRAND='"+ MySqlHelper.EscapeString(resultData[i].brand) + "', " +
                                    "GENDER='"+ MySqlHelper.EscapeString(resultData[i].gender) + "', " +
                                    "DEPARTMENT='"+ MySqlHelper.EscapeString(resultData[i].department) + "', " +
                                    "DEPARTMENT_TYPE='"+ MySqlHelper.EscapeString(resultData[i].departmentType)+ "', " +
                                    "SIZE='"+ MySqlHelper.EscapeString(resultData[i].size) + "', " +
                                    "COLOR='"+ MySqlHelper.EscapeString(resultData[i].color) + "', " +
                                    "UNIT='"+ MySqlHelper.EscapeString(resultData[i].unit) + "', " +
                                    "PRICE='"+ MySqlHelper.EscapeString(resultData[i].price.ToString()) + "', " +
                                    "ARTICLE_ID_ALIAS='"+ MySqlHelper.EscapeString(resultData[i].articleIdAlias) + "', " +
                                    "IS_SERVICE='"+ isService +"' " +
                                    "WHERE ARTICLE_ID='"+articleID+"'";
                            } else
                            {
                                //input
                                sql_statement = "INSERT INTO article_ho (_id ,ARTICLE_ID, ARTICLE_NAME, BRAND, GENDER, DEPARTMENT, DEPARTMENT_TYPE, SIZE, COLOR, UNIT, PRICE, ARTICLE_ID_ALIAS, IS_SERVICE) VALUES";
                                sql_statement += string.Format("('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}')", MySqlHelper.EscapeString(resultData[i].id.ToString()), MySqlHelper.EscapeString(resultData[i].articleId), MySqlHelper.EscapeString(resultData[i].articleName), MySqlHelper.EscapeString(resultData[i].brand), MySqlHelper.EscapeString(resultData[i].gender), MySqlHelper.EscapeString(resultData[i].department), MySqlHelper.EscapeString(resultData[i].departmentType), MySqlHelper.EscapeString(resultData[i].size), MySqlHelper.EscapeString(resultData[i].color), MySqlHelper.EscapeString(resultData[i].unit), MySqlHelper.EscapeString(resultData[i].price.ToString()), MySqlHelper.EscapeString(resultData[i].articleIdAlias), isService);
                            }
                        }

                        try
                        {
                            CRUD input = new CRUD();
                            input.ExecuteNonQuery(sql_statement);
                            MessageBox.Show("Success");
                        }
                        catch
                        {
                            MessageBox.Show("Failed!");
                        }
                        ckon.con.Close();
                    }
                    else
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
        //end
    }
}
