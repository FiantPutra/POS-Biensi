using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Runtime.Serialization.Json;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using System.Windows;

namespace try_bi
{
    class API_Post_Inout
    {
        String link_api;
        koneksi ckon = new koneksi();
        koneksi ckon1 = new koneksi();
        koneksi ckon2 = new koneksi();
        koneksi ckon3 = new koneksi();

        LinkApi link = new LinkApi();

        //========================VARIABLE FOR ARTICLE ======== =========================================
        String id_from_article2, articleName2, brand2, color2, department2, dept_type2, gender2, size2, unit2, art_id_alias, store_code;
        int id_article2, price_article2, countData;
        //========================VARIABLE FOR LINE=========================================
        int id_MO_Line2, qty2, Mo_id_Fk2, id_article_Fk2;
        String Mo_id2, unit_Mo;
        //========================VARIABLE FOR HEADER=======================================
        String store_code2, date2, mut_from_war2, mut_to_war2, id_m_o2, req_dev_date2, time2, timestamp2, epy_id, epy_name, seq_number_substring, no_sj;
        int id_Mo2, status2, totalqty2, totAmount, Ho_TransTypeId;
        //===============================================================================================
        String real_article_id, do_id, Ho_TransTypeCode;

        public void execMovement(String code)
        {
            do_id = code;
            postInout().Wait();
        }

        public async Task postInout()
        {
            link_api = link.aLink;

            hoTransactionHeader head_new = new hoTransactionHeader();

            ckon.con.Close();
            String sql = "SELECT * FROM ho_header WHERE STATUS_API=0 AND MUTASI_ORDER_ID='" + do_id + "'";
            ckon.cmd = new MySqlCommand(sql, ckon.con);
            ckon.con.Open();
            ckon.myReader = ckon.cmd.ExecuteReader();
            while (ckon.myReader.Read())
            {
                //AMBIL NILAI DARI TRANSACTION HEADER DISINI

                //==============GET DATA FROM RET_ORDER HEADER========
                id_m_o2 = ckon.myReader.GetString("MUTASI_ORDER_ID");
                seq_number_substring = id_m_o2.Substring(11);
                Mo_id_Fk2 = ckon.myReader.GetInt32("_id");
                store_code2 = ckon.myReader.GetString("STORE_CODE");
                date2 = ckon.myReader.GetString("DATE");
                id_Mo2 = ckon.myReader.GetInt32("_id");
                mut_from_war2 = ckon.myReader.GetString("MUTASI_FROM_WAREHOUSE");
                mut_to_war2 = ckon.myReader.GetString("MUTASI_TO_WAREHOUSE");
                req_dev_date2 = ckon.myReader.GetString("REQUEST_DELIVERY_DATE");
                status2 = ckon.myReader.GetInt32("STATUS");
                time2 = ckon.myReader.GetString("TIME");
                timestamp2 = ckon.myReader.GetString("TIME_STAMP");
                totalqty2 = ckon.myReader.GetInt32("TOTAL_QTY");
                epy_id = ckon.myReader.GetString("EMPLOYEE_ID");
                epy_name = ckon.myReader.GetString("EMPLOYEE_NAME");
                no_sj = ckon.myReader.GetString("NO_SJ");
                totAmount = ckon.myReader.GetInt32("TOTAL_AMOUNT");
                Ho_TransTypeId = ckon.myReader.GetInt32("TRANS_TYPE_ID");
                Ho_TransTypeCode = ckon.myReader.GetString("TRANS_TYPE_CODE");
                //
                //==============SEARCH BY MUTASI ORDER ID=============
                String sql2 = "SELECT * FROM ho_line WHERE MUTASI_ORDER_ID= '" + do_id + "'";
                head_new.hoTransactionLines = new List<hoTransactionLine>();
                ckon2.cmd = new MySqlCommand(sql2, ckon2.con);
                ckon2.con.Open();
                ckon2.myReader = ckon2.cmd.ExecuteReader();
                while (ckon2.myReader.Read())
                {
                    //===============GET ARTICLE ID FROM MUT_ORDER LINE===============================
                    real_article_id = ckon2.myReader.GetString("ARTICLE_ID");
                    //=====================SEARCH ARTICLE BY ARTICLE ID===============================
                    String sql3 = "SELECT * FROM article_ho WHERE ARTICLE_ID='" + real_article_id + "'";
                    ckon3.cmd = new MySqlCommand(sql3, ckon3.con);

                    ckon3.con.Open();
                    ckon3.myReader = ckon3.cmd.ExecuteReader();
                    while (ckon3.myReader.Read())
                    {
                        id_article2 = ckon3.myReader.GetInt32("_id");
                        id_from_article2 = ckon3.myReader.GetString("ARTICLE_ID");
                        articleName2 = ckon3.myReader.GetString("ARTICLE_NAME");
                        brand2 = ckon3.myReader.GetString("BRAND");
                        gender2 = ckon3.myReader.GetString("GENDER");
                        department2 = ckon3.myReader.GetString("DEPARTMENT");
                        dept_type2 = ckon3.myReader.GetString("DEPARTMENT_TYPE");
                        size2 = ckon3.myReader.GetString("SIZE");
                        color2 = ckon3.myReader.GetString("COLOR");
                        unit2 = ckon3.myReader.GetString("UNIT");
                        price_article2 = ckon3.myReader.GetInt32("PRICE");
                        art_id_alias = ckon3.myReader.GetString("ARTICLE_ID_ALIAS");
                    }
                    ckon3.con.Close();
                    //===============================END OF ARTICLE DATA==============================
                    id_article_Fk2 = id_article2;
                    id_MO_Line2 = ckon2.myReader.GetInt32("_id");
                    Mo_id2 = ckon2.myReader.GetString("MUTASI_ORDER_ID");
                    qty2 = ckon2.myReader.GetInt32("QUANTITY");
                    unit_Mo = ckon2.myReader.GetString("UNIT");
                    //===============================GET VARIABLE FOR API MUT ORDER LINE====================
                    hoTransactionLine mo_line = new hoTransactionLine()
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
                            price = price_article2,
                            articleIdAlias = art_id_alias
                        },
                        articleIdFk = id_article_Fk2,
                        id = id_MO_Line2,
                        mutasiOrderId = Mo_id2,
                        mutasiOrderIdFk = Mo_id_Fk2,
                        quantity = qty2,
                        unit = unit_Mo
                    };
                    head_new.hoTransactionLines.Add(mo_line);
                    //===============================END GET VARIABLE API FOR API MUT ORDER LINE============
                }
                ckon2.con.Close();
                //============================END OF WHILE IN RMUT_ORDER LINE===========================

                //================================GET API FOR MUT ORDER HEADER=============================
                hoTransactionHeader ho_new = new hoTransactionHeader()
                {
                    storeCode = store_code2,
                    sequenceNumber = seq_number_substring,
                    date = date2,
                    id = id_Mo2,
                    mutasiFromWarehouse = mut_from_war2,
                    mutasiToWarehouse = mut_to_war2,
                    mutasiOrderId = id_m_o2,
                    hoTransactionLines = head_new.hoTransactionLines,
                    requestDeliveryDate = req_dev_date2,
                    status = status2,
                    time = time2,
                    timeStamp = timestamp2,
                    totalQty = totalqty2,
                    employeeId = epy_id,
                    employeeName = epy_name,
                    oldSJ = no_sj,
                    totalAmount = totAmount,
                    hoTransTypeId = Ho_TransTypeId,
                    hoTransTypeCode = Ho_TransTypeCode
                };

                String response = "";
                var stringPayload = JsonConvert.SerializeObject(ho_new);
                var credentials = new NetworkCredential("username", "password");
                var handler = new HttpClientHandler { Credentials = credentials };
                var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");
                using (var client = new HttpClient(handler))
                {
                    try
                    {
                        HttpResponseMessage message = client.PostAsync(link_api + "/api/HOTransaction", httpContent).Result;
                        if (message.IsSuccessStatusCode)
                        {
                            var jsonBody = await message.Content.ReadAsStringAsync();
                            dynamic objRes = JsonConvert.DeserializeObject<dynamic>(jsonBody);

                            String query = "UPDATE ho_header SET STATUS_API='1', STATUS='1', RIDN='"+ objRes.id +"' WHERE MUTASI_ORDER_ID='" + do_id + "'";
                            CRUD input = new CRUD();
                            input.ExecuteNonQuery(query);

                            //HO_Print hp = new HO_Print();
                            //hp.set_print(do_id);
                        }

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                }
            }
            ckon.con.Close();
        }
    }
}
