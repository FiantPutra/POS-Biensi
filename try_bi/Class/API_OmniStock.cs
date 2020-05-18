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
    class API_OmniStock
    {
        String link_api;
        String storeCode = "", regional = "", city = "";
        koneksi ckon = new koneksi();
        LinkApi link = new LinkApi();
        CRUD sql = new CRUD();

        public List<OmniStock> getOmniStock(string itemId)
        {
            getStore();

            return OmniStock(itemId).Result;
        }

        public void getStore()
        {
            try
            {
                ckon.sqlCon().Open();
                String cmd = "SELECT * FROM store";
                ckon.sqlDataRd = sql.ExecuteDataReader(cmd, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        storeCode = Convert.ToString(ckon.sqlDataRd["CODE"]);
                        regional = Convert.ToString(ckon.sqlDataRd["REGIONAL"]);
                        city = Convert.ToString(ckon.sqlDataRd["CITY"]);
                    }
                }                
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (ckon.sqlDataRd != null)
                    ckon.sqlDataRd.Close();

                if (ckon.sqlCon().State == ConnectionState.Open)
                    ckon.sqlCon().Close();
            }
        }

        public async Task<List<OmniStock>> OmniStock(string itemId)
        {
            link_api = link.aLink;
            List<OmniStock> resultData = null;
            String response = "";
            var credentials = new NetworkCredential("username", "password");
            var handler = new HttpClientHandler { Credentials = credentials };


            using (var client = new HttpClient(handler))
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    HttpResponseMessage message = client.GetAsync(link_api + "/api/OmniStock?itemId=" + itemId + "&regional=" + regional + "&city=" + city + "&storeCode=" + storeCode).Result;

                    if (message.IsSuccessStatusCode)
                    {
                        var serializer = new DataContractJsonSerializer(typeof(List<OmniStock>));
                        var result = message.Content.ReadAsStringAsync().Result;
                        byte[] byteArray = Encoding.UTF8.GetBytes(result);
                        MemoryStream stream = new MemoryStream(byteArray);
                        resultData = serializer.ReadObject(stream) as List<OmniStock>;                        
                    }                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "No Connection", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                return resultData;
            }
        }
    }
}
