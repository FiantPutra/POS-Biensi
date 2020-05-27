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

namespace try_bi.Class
{
    class API_ReturnOrder
    {
        CRUD sql = new CRUD();
        koneksi ckon = new koneksi();
        LinkApi link = new LinkApi();
        String link_api;        

        //========================VARIABLE FOR ARTICLE ======== =========================================
        String id_from_article2, articleName2, unit2, art_id_alias;
        int id_article2, price_article2;
        //========================VARIABLE FOR RETURN ORDER LINE=========================================
        int id_RO_Line2, qty2, Ro_id_Fk2, id_article_Fk2;
        String Ro_id2, unit_ro;
        //Double id_article_Fk2;
        //========================VARIABLE FOR RETURN ORDER HEADER=======================================
        String store_code2, date2, remark2, id_r_o2, time2, timestamp2, warehouseid2, seq_number_substring, epy_id, epy_name, no_sj;
        int id_Ro2, status2, totalqty2;
        //===============================================================================================
        String real_article_id;

        public async Task<Boolean> returnOrder()
        {
            bool isSuccess = false;

            try
            {
                List<ReturnOrderLine> ro_LineList = new List<ReturnOrderLine>();
                link_api = link.aLink;
                ckon.sqlCon().Open();

                String cmd = "SELECT a.RETURN_ORDER_ID, a._id as RoId, a.STORE_CODE, a.DATE, a.REMARK, a.STATUS, a.TIME, a.TIME_STAMP, a.TOTAL_QTY, "
                                + "a.WAREHOUSE_ID,  a.EMPLOYEE_ID, a.EMPLOYEE_NAME, a.NO_SJ, c._id as ArtId, c.ARTICLE_ID, c.ARTICLE_NAME, c.ARTICLE_ID_ALIAS, c.PRICE, c.UNIT, "
                                + "b._id as RoLineId, b.QUANTITY FROM returnorder a INNER JOIN returnorder_line b "
                                + "ON a.RETURN_ORDER_ID = b.RETURN_ORDER_ID INNER JOIN article c "
                                + "ON b.ARTICLE_ID = c.ARTICLE_ID WHERE a.STATUS_API = 0";

                ckon.sqlDataRd = sql.ExecuteDataReader(cmd, ckon.sqlCon());
                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        id_r_o2 = Convert.ToString(ckon.sqlDataRd["RETURN_ORDER_ID"]);
                        seq_number_substring = id_r_o2.Substring(12);
                        store_code2 = Convert.ToString(ckon.sqlDataRd["STORE_CODE"]);
                        date2 = Convert.ToString(ckon.sqlDataRd["DATE"]);
                        id_Ro2 = Convert.ToInt32(ckon.sqlDataRd["RoId"]);
                        remark2 = Convert.ToString(ckon.sqlDataRd["REMARK"]);
                        status2 = Convert.ToInt32(ckon.sqlDataRd["STATUS"]);
                        time2 = Convert.ToString(ckon.sqlDataRd["TIME"]);
                        timestamp2 = Convert.ToString(ckon.sqlDataRd["TIME_STAMP"]);
                        totalqty2 = Convert.ToInt32(ckon.sqlDataRd["TOTAL_QTY"]);
                        warehouseid2 = Convert.ToString(ckon.sqlDataRd["WAREHOUSE_ID"]);
                        epy_id = Convert.ToString(ckon.sqlDataRd["EMPLOYEE_ID"]);
                        epy_name = Convert.ToString(ckon.sqlDataRd["EMPLOYEE_NAME"]);
                        no_sj = Convert.ToString(ckon.sqlDataRd["NO_SJ"]);

                        id_article2 = Convert.ToInt32(ckon.sqlDataRd["ArtId"]);
                        id_from_article2 = Convert.ToString(ckon.sqlDataRd["ARTICLE_ID"]);
                        articleName2 = Convert.ToString(ckon.sqlDataRd["ARTICLE_NAME"]);
                        unit2 = Convert.ToString(ckon.sqlDataRd["UNIT"]);
                        price_article2 = Convert.ToInt32(ckon.sqlDataRd["PRICE"]);
                        art_id_alias = Convert.ToString(ckon.sqlDataRd["ARTICLE_ID_ALIAS"]);

                        id_article_Fk2 = id_article2;
                        id_RO_Line2 = Convert.ToInt32(ckon.sqlDataRd["RoLineId"]);
                        qty2 = Convert.ToInt32(ckon.sqlDataRd["QUANTITY"]);

                        ReturnOrderLine roLine = new ReturnOrderLine()
                        {
                            article = new Article
                            {
                                articleId = id_from_article2,
                                articleName = articleName2,
                                unit = unit2,
                                id = id_article2,
                                price = price_article2,
                                articleIdAlias = art_id_alias
                            },
                            articleIdFk = id_article_Fk2,
                            id = id_RO_Line2,
                            quantity = qty2,
                            returnOrderId = Ro_id2,
                            returnOrderIdFk = id_Ro2,
                            unit = unit_ro
                        };
                        ro_LineList.Add(roLine);
                    }

                    RetrunOrder ro_new = new RetrunOrder()
                    {
                        storeCode = store_code2,
                        sequenceNumber = seq_number_substring,
                        date = date2,
                        id = id_Ro2,
                        remark = remark2,
                        returnOrderId = id_r_o2,
                        returnOrderLines = ro_LineList,
                        status = status2,
                        time = time2,
                        timeStamp = timestamp2,
                        totalQty = totalqty2,
                        warehouseId = warehouseid2,
                        oldSJ = no_sj

                    };
                        
                    var returnOrder = JsonConvert.SerializeObject(ro_new);
                    var credentials = new NetworkCredential("username", "password");
                    var handler = new HttpClientHandler { Credentials = credentials };
                    var httpContent = new StringContent(returnOrder, Encoding.UTF8, "application/json");
                    using (var client = new HttpClient(handler))
                    {
                        try
                        {
                            HttpResponseMessage message = client.PostAsync(link_api + "/api/ReturnOrder", httpContent).Result;
                            if (message.IsSuccessStatusCode)
                            {
                                String cmd_update = "UPDATE returnorder SET STATUS_API='1' WHERE RETURN_ORDER_ID='" + id_r_o2 + "'";
                                CRUD update = new CRUD();
                                update.ExecuteNonQuery(cmd_update);

                                isSuccess = true;
                            }
                            else
                            {
                                isSuccess = false;
                                MessageBox.Show("Failed! Please try again.");
                            }
                        }
                        catch (Exception ex)
                        {
                            isSuccess = false;
                            MessageBox.Show(ex.ToString());
                        }
                    }                    
                }
                return isSuccess;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());

                return isSuccess;
            }
        }
    }
}
