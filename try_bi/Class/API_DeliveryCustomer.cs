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
    class API_DeliveryCustomer
    {
        CRUD sql = new CRUD();
        koneksi ckon = new koneksi();
        LinkApi link = new LinkApi();
        string date, time, total_qty, empl_Id, empl_Name, transactionId, articleId, courier, qty, address, type, noResi, storeFrom, storeTo;  
        String link_api;

        public async Task<Boolean> deliveryCustPost(string _deliveryCustId)
        {
            bool isSuccess = false;

            try
            {
                List<DeliveryCustomerLines> deliveryCustLines = new List<DeliveryCustomerLines>();
                link_api = link.aLink;
                ckon.sqlCon().Open();

                String cmd = "SELECT a.DATE, a.TIME, a.TOTAL_QTY, a.EMPLOYEE_ID, a.EMPLOYEE_NAME, a.TRANSACTION_ID, " 
                                + "b.ARTICLE_ID, b.COURIER, b.QTY, b.DELIVERYADDRESS, b.DELIVERYTYPE, b.NO_RESI, b.QTY, b.STORE_FROM, b.STORE_TO FROM deliverycustomer a "
                                + "join deliverycustomer_line b on b.DELIVERY_CUST_ID = a.DELIVERY_CUST_ID "
                                + "where a.DELIVERY_CUST_ID = '"+ _deliveryCustId + "'";

                ckon.sqlDataRd = sql.ExecuteDataReader(cmd, ckon.sqlCon());
                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {                        
                        date = Convert.ToString(ckon.sqlDataRd["DATE"]);
                        time = Convert.ToString(ckon.sqlDataRd["TIME"]);
                        total_qty = Convert.ToString(ckon.sqlDataRd["TOTAL_QTY"]);
                        empl_Id = Convert.ToString(ckon.sqlDataRd["EMPLOYEE_ID"]);
                        empl_Name = Convert.ToString(ckon.sqlDataRd["EMPLOYEE_NAME"]);
                        transactionId = Convert.ToString(ckon.sqlDataRd["TRANSACTION_ID"]);
                        articleId = Convert.ToString(ckon.sqlDataRd["ARTICLE_ID"]);
                        courier = Convert.ToString(ckon.sqlDataRd["COURIER"]);
                        qty = Convert.ToString(ckon.sqlDataRd["QTY"]);
                        address = Convert.ToString(ckon.sqlDataRd["DELIVERYADDRESS"]);
                        type = Convert.ToString(ckon.sqlDataRd["DELIVERYTYPE"]);
                        noResi = Convert.ToString(ckon.sqlDataRd["NO_RESI"]);
                        storeFrom = Convert.ToString(ckon.sqlDataRd["STORE_FROM"]);
                        storeTo = Convert.ToString(ckon.sqlDataRd["STORE_TO"]);
                        
                        DeliveryCustomerLines deliveryCustomerLines = new DeliveryCustomerLines();
                        deliveryCustomerLines.DeliveryCustId = _deliveryCustId;
                        deliveryCustomerLines.ArticleId = articleId;
                        deliveryCustomerLines.Qty = Convert.ToInt32(qty);
                        deliveryCustomerLines.StoreFrom = storeFrom;
                        deliveryCustomerLines.StoreTo = storeTo;
                        deliveryCustomerLines.DeliveryAddress = address;
                        deliveryCustomerLines.Courier = courier;
                        deliveryCustomerLines.NoResi = noResi;
                        deliveryCustomerLines.DeliveryType = type;
                        deliveryCustLines.Add(deliveryCustomerLines);
                    }

                    DeliveryCustomer deliveryCustomer = new DeliveryCustomer();
                    deliveryCustomer.DeliveryCustId = _deliveryCustId;
                    deliveryCustomer.Date = date;
                    deliveryCustomer.Time = time;
                    deliveryCustomer.TotalQty = total_qty;
                    deliveryCustomer.TransactionId = transactionId;
                    deliveryCustomer.EmployeeId = empl_Id;
                    deliveryCustomer.EmployeeName = empl_Name;
                    deliveryCustomer.deliveryCustLines = deliveryCustLines;

                    var mutasiOrder = JsonConvert.SerializeObject(deliveryCustomer);
                    var credentials = new NetworkCredential("username", "password");
                    var handler = new HttpClientHandler { Credentials = credentials };
                    var httpContent = new StringContent(mutasiOrder, Encoding.UTF8, "application/json");
                    using (var client = new HttpClient(handler))
                    {
                        try
                        {
                            HttpResponseMessage message = client.PostAsync(link_api + "/api/DeliveryCust", httpContent).Result;
                            if (message.IsSuccessStatusCode)
                            {
                                String cmd_update = "UPDATE deliverycustomer SET STATUS_API = '1', STATUS = '1' WHERE DELIVERY_CUST_ID='" + _deliveryCustId + "'";
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

        public async Task getDeliveryCust(string storeCode)
        {
            link_api = link.aLink;

            String response = "";
            var credentials = new NetworkCredential("username", "password");
            var handler = new HttpClientHandler { Credentials = credentials };


            using (var client = new HttpClient(handler))
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    HttpResponseMessage message = client.GetAsync(link_api + "/api/DeliveryCust?storeCode=" + storeCode).Result;

                    if (message.IsSuccessStatusCode)
                    {
                        var serializer = new DataContractJsonSerializer(typeof(List<DeliveryCustList>));
                        var result = message.Content.ReadAsStringAsync().Result;
                        byte[] byteArray = Encoding.UTF8.GetBytes(result);
                        MemoryStream stream = new MemoryStream(byteArray);
                        List<DeliveryCustList> resultData = serializer.ReadObject(stream) as List<DeliveryCustList>;
                        String connectionString = ckon.conSQL;

                        using (SqlConnection mConnection = new SqlConnection(connectionString))
                        {
                            mConnection.Open();
                            String cmd_insert = "IF NOT EXISTS (SELECT * FROM deliverycustomer WHERE DELIVERY_CUST_ID = @PARM1) " +
                                                    "BEGIN " +
                                                    "INSERT INTO deliverycustomer(DELIVERY_CUST_ID, TOTAL_QTY, STATUS, DATE, TIME, STATUS_API, EMPLOYEE_ID, EMPLOYEE_NAME, TRANSACTION_ID) " +
                                                    "VALUES(@PARM1, '0', '0', @PARM2, @PARM3, 0, '', '', '') " +
                                                    "INSERT INTO deliverycustomer_line(DELIVERY_CUST_ID, ARTICLE_ID, QTY, STORE_FROM, STORE_TO, NO_RESI, COURIER, DELIVERYADDRESS, DELIVERYTYPE) " +
                                                    "VALUES(@PARM1, @PARM4, @PARM5, @PARM6, @PARM7, '', @PARM8, @PARM9, @PARM10) " +
                                                     "END";
                            SqlCommand cmd = new SqlCommand(cmd_insert);
                            cmd.Connection = mConnection;
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.Add("@PARM1", DbType.String);
                            cmd.Parameters.Add("@PARM2", DbType.String);
                            cmd.Parameters.Add("@PARM3", DbType.String);
                            cmd.Parameters.Add("@PARM4", DbType.String);
                            cmd.Parameters.Add("@PARM5", DbType.String);
                            cmd.Parameters.Add("@PARM6", DbType.Int32);
                            cmd.Parameters.Add("@PARM7", DbType.String);
                            cmd.Parameters.Add("@PARM8", DbType.String);
                            cmd.Parameters.Add("@PARM9", DbType.String);
                            cmd.Parameters.Add("@PARM10", DbType.String);                            

                            for (int i = 0; i < resultData.Count; i++)
                            {
                                cmd.Parameters[0].Value = resultData[i].deliveryCustId;
                                cmd.Parameters[1].Value = resultData[i].date;
                                cmd.Parameters[2].Value = resultData[i].time;
                                cmd.Parameters[3].Value = resultData[i].articleId;
                                cmd.Parameters[4].Value = resultData[i].qty;
                                cmd.Parameters[5].Value = resultData[i].storeFrom;
                                cmd.Parameters[6].Value = resultData[i].storeTo;
                                cmd.Parameters[7].Value = resultData[i].courier;
                                cmd.Parameters[8].Value = resultData[i].deliveryAddress;
                                cmd.Parameters[9].Value = resultData[i].deliveryType;                                
                                cmd.ExecuteNonQuery();
                            }
                            mConnection.Close();
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
