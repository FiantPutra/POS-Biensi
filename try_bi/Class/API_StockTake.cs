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
using System.Web.Script.Serialization;
using Newtonsoft.Json;

namespace try_bi
{
    class API_StockTake
    {
        LinkApi link = new LinkApi();
        koneksi ckon  = new koneksi();
        koneksi2 ckon2 = new koneksi2();

        //========================VARIABLE FOR ARTICLE ======== =========================================
        String id_from_article2, articleName2, brand2, color2, department2, dept_type2, gender2, size2, unit2;
        int id_article2, price_article2;
        //=========================VARIABLE FOR STORE===================
        String cust_id_store, store_code;
        public String id_employe3, nm_employe3;
        //==================variable for stock take=======================
        int api_id, api_articleid, api_good, api_reject, api_whgood, api_whreject, api_status;
        String link_api;
        public void select_store()
        {
            ckon.con.Close();
            String sql = "SELECT * FROM store";
            ckon.cmd = new MySqlCommand(sql, ckon.con);
            try
            {
                ckon.con.Open();
                ckon.myReader = ckon.cmd.ExecuteReader();
                if (ckon.myReader.HasRows)
                {
                    while (ckon.myReader.Read())
                    {
                        store_code = ckon.myReader.GetString("CODE");
                        cust_id_store = ckon.myReader.GetString("CUST_ID_STORE");

                    }

                }
                ckon.con.Close();
            }
            catch
            { }
        }
        //================================================================================
        public async Task Post_stockTake()
        {
            link_api = link.aLink;

            StockTake stock2 = new StockTake();
            stock2.stockTakeLines = new List<StockTakeLine>();
            String sql = "SELECT * FROM stock_take";
            ckon.cmd = new MySqlCommand(sql, ckon.con);
            ckon.con.Open();
            ckon.myReader = ckon.cmd.ExecuteReader();
            if (ckon.myReader.HasRows)
            {
                while (ckon.myReader.Read())
                {
                    api_id = ckon.myReader.GetInt32("_id");
                    api_articleid = ckon.myReader.GetInt32("ARTICLE_ID");
                    api_good = ckon.myReader.GetInt32("GOOD_QTY");
                    api_reject = ckon.myReader.GetInt32("REJECT_QTY");
                    api_whgood = ckon.myReader.GetInt32("WH_GOOD_QTY");
                    api_whreject = ckon.myReader.GetInt32("WH_REJECT_QTY");
                    api_status = ckon.myReader.GetInt32("STATUS");
                    //============================================================
                    String sql3 = "SELECT * FROM article WHERE _id='" + api_articleid + "'";
                    ckon2.cmd2 = new MySqlCommand(sql3, ckon2.con2);
                    ckon2.con2.Open();
                    ckon2.myReader2 = ckon2.cmd2.ExecuteReader();
                    while (ckon2.myReader2.Read())
                    {
                        id_article2 = ckon2.myReader2.GetInt32("_id");
                        id_from_article2 = ckon2.myReader2.GetString("ARTICLE_ID");
                        articleName2 = ckon2.myReader2.GetString("ARTICLE_NAME");
                        brand2 = ckon2.myReader2.GetString("BRAND");
                        gender2 = ckon2.myReader2.GetString("GENDER");
                        department2 = ckon2.myReader2.GetString("DEPARTMENT");
                        dept_type2 = ckon2.myReader2.GetString("DEPARTMENT_TYPE");
                        size2 = ckon2.myReader2.GetString("SIZE");
                        color2 = ckon2.myReader2.GetString("COLOR");
                        unit2 = ckon2.myReader2.GetString("UNIT");
                        price_article2 = ckon2.myReader2.GetInt32("PRICE");

                       
                    }
                    //===============================END OF ARTICLE DATA============================

                    StockTakeLine stock_list = new StockTakeLine()
                    {
                        article = new Article
                        {
                            articleId = id_from_article2,
                            articleName = articleName2,
                            brand = brand2,
                            color = color2,
                            department = department2,
                            departmentType = dept_type2,
                            gender = gender2,
                            size = size2,
                            unit = unit2,
                            id = id_article2,
                            price = price_article2
                        },
                        id = api_id,
                        articleId = api_articleid,
                        goodQty = api_good,
                        rejectQty = api_reject,
                        whGoodQty = api_whgood,
                        whRejectQty = api_whreject,
                        status = api_status,
                    };

                    stock2.stockTakeLines.Add(stock_list);
                    ckon2.con2.Close();
                }

            }
            ckon.con.Close();
            StockTake stock_new = new StockTake()
            {
                employeeId = id_employe3,
                employeeName = nm_employe3,
                storeCode = store_code,
                stockTakeLines = stock2.stockTakeLines
            };
            var stringPayload = JsonConvert.SerializeObject(stock_new);
            String response = "";
            var credentials = new NetworkCredential("username", "password");
            var handler = new HttpClientHandler { Credentials = credentials };
            var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");
            using (var client = new HttpClient(handler))
            {
                //HttpResponseMessage message = client.PostAsync("http://retailbiensi.azurewebsites.net/api/StockTake", httpContent).Result;
                HttpResponseMessage message = client.PostAsync(link_api+"/api/StockTake", httpContent).Result;
            }
            
        }
        //==========================================================================================
    }
}
