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

namespace try_bi
{
    class API_DeliveryOrderConfirm
    {
        CRUD sql = new CRUD();
        koneksi ckon = new koneksi();              
        int qtyReceive = 0, deliveryOrder = 0, deliveryOrderLines = 0;
        LinkApi link = new LinkApi();
        String link_api;

        public async Task deliveryOrderConfirm(String deliveyOrderId, String employeeId, String employeeName, String cust_id_store)
        {            
            try
            {
                link_api = link.aLink;
                ckon.sqlCon().Open();
                List<DeliveryOrderLine> dev_order_lines = new List<DeliveryOrderLine>();
                String cmd = "SELECT a._id as deliveryOrder, b._id as deliveryOrderLines, b.QTY_RECEIVE FROM deliveryorder a INNER JOIN deliveryorder_line b "
                                + "ON b.DELIVERY_ORDER_ID = a.DELIVERY_ORDER_ID "
                                + "WHERE a.DELIVERY_ORDER_ID = '"+ deliveyOrderId + "' AND a.STATUS_API = 0 AND (b.QTY_DISPUTE < 0 OR b.QTY_DISPUTE > 0)";

                ckon.sqlDataRd = sql.ExecuteDataReader(cmd, ckon.sqlCon());
                if (ckon.sqlDataRd.HasRows)
                {                                        
                    while (ckon.sqlDataRd.Read())
                    {
                        deliveryOrder = Convert.ToInt32(ckon.sqlDataRd["deliveryOrder"]);
                        deliveryOrderLines = Convert.ToInt32(ckon.sqlDataRd["deliveryOrderLines"]);                        
                        qtyReceive = Convert.ToInt32(ckon.sqlDataRd["QTY_RECEIVE"]);

                        DeliveryOrderLine do_line_list = new DeliveryOrderLine();
                        do_line_list.id = deliveryOrderLines;
                        do_line_list.qtyReceive = qtyReceive;                                                   
                        dev_order_lines.Add(do_line_list);
                    }

                    DeliveryOrder dev_order = new DeliveryOrder();
                    dev_order.deliveryOrderId = deliveyOrderId;
                    dev_order.deliveryOrderLines = dev_order_lines;
                    dev_order.id = deliveryOrder;
                    dev_order.employeeId = employeeId;
                    dev_order.employeeName = employeeName;
                    

                    var stringPayload = JsonConvert.SerializeObject(dev_order);
                    var credentials = new NetworkCredential("username", "password");
                    var handler = new HttpClientHandler { Credentials = credentials };
                    var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");
                    using (var client = new HttpClient(handler))
                    {
                        try
                        {
                            HttpResponseMessage message = client.PutAsync(link_api + "/api/DeliveryOrder", httpContent).Result;                            
                            if (message.IsSuccessStatusCode)
                            {
                                String cmd_update = "UPDATE deliveryorder SET STATUS='Confirmed', STATUS_API = 1,  CUST_ID_STORE='" + cust_id_store + "',EMPLOYEE_ID = '" + employeeId + "', EMPLOYEE_NAME='" + employeeName + "' WHERE DELIVERY_ORDER_ID='" + deliveyOrderId + "'";
                                CRUD update = new CRUD();
                                update.ExecuteNonQuery(cmd_update);

                                MessageBox.Show("Success!");
                            }
                            else
                            {
                                MessageBox.Show("Failed! Please try again.");
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString());
                        }
                    }
                }                               
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
            
        }
    }
}
