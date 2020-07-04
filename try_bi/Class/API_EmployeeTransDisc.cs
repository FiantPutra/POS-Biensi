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

namespace try_bi.Class
{
    class API_EmployeeTransDisc
    {
        String link_api;        
        koneksi ckon = new koneksi();
        LinkApi link = new LinkApi();
        CRUD sql = new CRUD();

        public int qtyEmployeeTrans(string employeeId)
        {
            link_api = link.aLink;

            int qty = 0;

            var credentials = new NetworkCredential("username", "password");
            var handler = new HttpClientHandler { Credentials = credentials };            

            using (var client = new HttpClient(handler))
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    HttpResponseMessage message = client.GetAsync(link_api + "/api/EmployeeTransDisc?employeeId="+ employeeId + "").Result;

                    if (message.IsSuccessStatusCode)
                    {
                        var serializer = new DataContractJsonSerializer(typeof(List<employeeTransDisc>));
                        var result = message.Content.ReadAsStringAsync().Result;
                        byte[] byteArray = Encoding.UTF8.GetBytes(result);
                        MemoryStream stream = new MemoryStream(byteArray);
                        List<employeeTransDisc> resultData = serializer.ReadObject(stream) as List<employeeTransDisc>;

                        for (int i = 0; i < resultData.Count; i++)
                        {
                            qty = resultData[i].qty;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "No Connection", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                return qty;
            }
        }
    }
}
