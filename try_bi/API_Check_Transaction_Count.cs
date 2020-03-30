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
    class API_Check_Transaction_Count
    {
        LinkApi link = new LinkApi();
        String employee_id, link_api, transCount;

        public void setApi(String employeeId)
        {
            employee_id = employeeId;
            getApiCount().Wait();
        }

        public int getCount()
        {
            int a = Int32.Parse(transCount);
            return a;
        }

        public async Task getApiCount()
        {
            link_api = link.aLink;

            var credentials = new NetworkCredential("username", "password");
            var handler = new HttpClientHandler { Credentials = credentials };

            using (var client = new HttpClient(handler))
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {

                    HttpResponseMessage message = client.GetAsync(link_api + "/api/CalculateDiscountEmployee?employeeId=" + employee_id).Result;

                    if (message.IsSuccessStatusCode)
                    {
                        transCount = message.Content.ReadAsStringAsync().Result;
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
