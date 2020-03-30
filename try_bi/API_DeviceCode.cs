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
    class API_DeviceCode
    {
        //LinkApi link = new LinkApi();

        String store_code, link;

        koneksi ckon = new koneksi();

        public void LinkGetCode(String a)
        {
            link = a;
        }
        public void cek_storeCode()
        {
            ckon.con.Close();
            String sql_storeCode = "Select * from store";
            ckon.cmd = new MySqlCommand(sql_storeCode, ckon.con);
            ckon.con.Open();
            ckon.myReader = ckon.cmd.ExecuteReader();
            while (ckon.myReader.Read())
            {
                store_code = ckon.myReader.GetString("CODE");
            }
            ckon.con.Close();
        }

        public String GetDeviceId(String device_code)
        {
            String response = "";
            var credentials = new NetworkCredential("username", "password");
            var handler = new HttpClientHandler { Credentials = credentials };
            //var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");
            using (var client = new HttpClient(handler))
            {
                //try
                //{
                HttpResponseMessage message = client.GetAsync("http://mpos.biensicore.co.id/api/GetStoreData?storeCode=" + store_code+"&deviceId="+ device_code).Result;
                if (message.IsSuccessStatusCode)
                {

                    // GET RETURN VALUE FROM POST API
                    var serializer = new DataContractJsonSerializer(typeof(StoreMaster_respone));
                        var responseContent = message.Content.ReadAsStringAsync().Result;
                        byte[] byteArray = Encoding.UTF8.GetBytes(responseContent);
                        MemoryStream stream = new MemoryStream(byteArray);
                        StoreMaster_respone resultData = serializer.ReadObject(stream) as StoreMaster_respone;

                        device_code = resultData.deviceCode;
                        //MessageBox.Show("Successfully Retrieving Employee Data", "Successfully", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return device_code;
                }
                else
                {
                    MessageBox.Show("Error API Device Code", "Error API", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return device_code = "null";
                }

                //}
                //catch
                //{
                //    MessageBox.Show("Make Sure You Are Connected To The Internet", "No Connection", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //}

            }
        }
    }
}
