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
    class API_MutasiOrder
    {
        CRUD sql = new CRUD();
        koneksi ckon = new koneksi();       
        LinkApi link = new LinkApi();
        String link_api;
        //========================VARIABLE FOR ARTICLE ======== =========================================
        String id_from_article2, articleName2, unit2, art_id_alias, store_code;
        int id_article2, price_article2, countData;
        //========================VARIABLE FOR MUTASI ORDER LINE=========================================
        int id_MO_Line2, qty2, id_article_Fk2;        
        //========================VARIABLE FOR MUTASI ORDER HEADER=======================================
        String store_code2, date2, mut_from_war2, mut_to_war2, id_m_o2, req_dev_date2, time2, timestamp2, epy_id, epy_name, seq_number_substring, no_sj;
        int id_Mo2, status2, totalqty2;
        //===============================================================================================        

        public async Task<Boolean> mutasiOrder()
        {
            bool isSuccess = false;

            try
            {
                MutasiOrder mo_new2 = new MutasiOrder();
                link_api = link.aLink;
                ckon.sqlCon().Open();                

                String cmd = "SELECT a.MUTASI_ORDER_ID, a.STORE_CODE, a.DATE, a._id as MoId, a.MUTASI_FROM_WAREHOUSE, a.MUTASI_TO_WAREHOUSE, "
                                + "a.REQUEST_DELIVERY_DATE, a.STATUS, a.TIME, a.TIME_STAMP, a.TOTAL_QTY, a.EMPLOYEE_ID, a.EMPLOYEE_NAME, a.NO_SJ, "
                                + "c._id as ArtId, c.ARTICLE_ID, c.ARTICLE_NAME, c.UNIT, c.PRICE, c.ARTICLE_ID_ALIAS, b._id as MoLineId, b.QUANTITY FROM mutasiorder a INNER JOIN mutasiorder_line b "
                                + "ON a.MUTASI_ORDER_ID = b.MUTASI_ORDER_ID INNER JOIN article c "
                                + "ON c.ARTICLE_ID = b.ARTICLE_ID WHERE a.STATUS_API = 0 AND a.STATUS = 1";

                ckon.sqlDataRd = sql.ExecuteDataReader(cmd, ckon.sqlCon());
                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        id_m_o2 = Convert.ToString(ckon.sqlDataRd["MUTASI_ORDER_ID"]);
                        seq_number_substring = id_m_o2.Substring(12);
                        store_code2 = Convert.ToString(ckon.sqlDataRd["STORE_CODE"]);
                        date2 = Convert.ToString(ckon.sqlDataRd["DATE"]);
                        id_Mo2 = Convert.ToInt32(ckon.sqlDataRd["MoId"]);
                        mut_from_war2 = Convert.ToString(ckon.sqlDataRd["MUTASI_FROM_WAREHOUSE"]);
                        mut_to_war2 = Convert.ToString(ckon.sqlDataRd["MUTASI_TO_WAREHOUSE"]);
                        req_dev_date2 = Convert.ToString(ckon.sqlDataRd["REQUEST_DELIVERY_DATE"]);
                        status2 = Convert.ToInt32(ckon.sqlDataRd["STATUS"]);
                        time2 = Convert.ToString(ckon.sqlDataRd["TIME"]);
                        timestamp2 = Convert.ToString(ckon.sqlDataRd["TIME_STAMP"]);
                        totalqty2 = Convert.ToInt32(ckon.sqlDataRd["TOTAL_QTY"]);
                        epy_id = Convert.ToString(ckon.sqlDataRd["EMPLOYEE_ID"]);
                        epy_name = Convert.ToString(ckon.sqlDataRd["EMPLOYEE_NAME"]);
                        no_sj = Convert.ToString(ckon.sqlDataRd["NO_SJ"]);

                        mo_new2.mutasiOrderLines = new List<MutasiOrderLine>();

                        id_article2 = Convert.ToInt32(ckon.sqlDataRd["ArtId"]);
                        id_from_article2 = Convert.ToString(ckon.sqlDataRd["ARTICLE_ID"]);
                        articleName2 = Convert.ToString(ckon.sqlDataRd["ARTICLE_NAME"]);
                        unit2 = Convert.ToString(ckon.sqlDataRd["UNIT"]);
                        price_article2 = Convert.ToInt32(ckon.sqlDataRd["PRICE"]);
                        art_id_alias = Convert.ToString(ckon.sqlDataRd["ARTICLE_ID_ALIAS"]);

                        id_article_Fk2 = id_article2;
                        id_MO_Line2 = Convert.ToInt32(ckon.sqlDataRd["MoLineId"]);
                        qty2 = Convert.ToInt32(ckon.sqlDataRd["QUANTITY"]);

                        MutasiOrderLine mo_line = new MutasiOrderLine()
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
                            id = id_MO_Line2,
                            mutasiOrderId = id_m_o2,
                            mutasiOrderIdFk = id_Mo2,
                            quantity = qty2,
                            unit = unit2
                        };
                        mo_new2.mutasiOrderLines.Add(mo_line);
                    }

                    MutasiOrder mo_new = new MutasiOrder()
                    {
                        storeCode = store_code2,
                        sequenceNumber = seq_number_substring,
                        date = date2,
                        id = id_Mo2,
                        mutasiFromWarehouse = mut_from_war2,
                        mutasiToWarehouse = mut_to_war2,
                        mutasiOrderId = id_m_o2,
                        mutasiOrderLines = mo_new2.mutasiOrderLines,
                        requestDeliveryDate = req_dev_date2,
                        status = status2,
                        time = time2,
                        timeStamp = timestamp2,
                        totalQty = totalqty2,
                        employeeId = epy_id,
                        employeeName = epy_name,
                        oldSJ = no_sj
                    };

                    var mutasiOrder = JsonConvert.SerializeObject(mo_new);
                    var credentials = new NetworkCredential("username", "password");
                    var handler = new HttpClientHandler { Credentials = credentials };
                    var httpContent = new StringContent(mutasiOrder, Encoding.UTF8, "application/json");
                    using (var client = new HttpClient(handler))
                    {
                        try
                        {
                            HttpResponseMessage message = client.PostAsync(link_api + "/api/MutasiOrder", httpContent).Result;
                            if (message.IsSuccessStatusCode)
                            {
                                String cmd_update = "UPDATE mutasiorder SET STATUS_API = '1' WHERE MUTASI_ORDER_ID='" + id_m_o2 + "'";
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
