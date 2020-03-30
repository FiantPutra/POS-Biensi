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
    class API_Closing_Store
    {
        LinkApi link = new LinkApi();
        CRUD sql = new CRUD();

        koneksi ckon = new koneksi();
        String id_store, _id, store_id, shift, opening_time, closing_time, dev_name, total_qty, status, epy_id, epy_name, seq_number_substring, link_api;
        int open_trans_balance, closing_trans_balance, real_trans_balance, dispute_trans_balance, open_pety, real_pety, close_pety, dispute_pety, open_deposit, close_deposit, real_deposit, dispute_deposit;
        //==============METHOD FOR GET ID_CLOSING_SHIFT==============
        public void get_code(String id)
        {
            id_store = id;
        }

        //=====================METHOD FOR ASYNC TASK API===================
        public async Task Post_Closing_Store()
        {
            link_api = link.aLink;

            String cmd = "SELECT * FROM closing_store WHERE ID_C_STORE = '" + id_store + "'";
            ckon.sqlCon().Open();
            ckon.sqlDataRd = sql.ExecuteDataReader(cmd, ckon.sqlCon());
            
            while (ckon.sqlDataRd.Read())
            {
                _id = Convert.ToString(ckon.sqlDataRd["_id"]);
                id_store = Convert.ToString(ckon.sqlDataRd["ID_C_STORE"]);
                seq_number_substring = id_store.Substring(12);
                store_id = Convert.ToString(ckon.sqlDataRd["STORE_ID"]);
                //shift = ckon.myReader.GetString("SHIFT");
                opening_time = Convert.ToString(ckon.sqlDataRd["OPENING_TIME"]);
                closing_time = Convert.ToString(ckon.sqlDataRd["CLOSING_TIME"]);
                open_trans_balance = Convert.ToInt32(ckon.sqlDataRd["OPENING_TRANS_BALANCE"]);
                closing_trans_balance = Convert.ToInt32(ckon.sqlDataRd["CLOSING_TRANS_BALANCE"]);
                real_trans_balance = Convert.ToInt32(ckon.sqlDataRd["REAL_TRANS_BALANCE"]);
                dispute_trans_balance = Convert.ToInt32(ckon.sqlDataRd["DISPUTE_TRANS_BALANCE"]);
                open_pety = Convert.ToInt32(ckon.sqlDataRd["OPENING_PETTY_CASH"]);
                close_pety = Convert.ToInt32(ckon.sqlDataRd["CLOSING_PETTY_CASH"]);
                real_pety = Convert.ToInt32(ckon.sqlDataRd["REAL_PETTY_CASH"]);
                dispute_pety = Convert.ToInt32(ckon.sqlDataRd["DISPUTE_PETTY_CASH"]);
                open_deposit = Convert.ToInt32(ckon.sqlDataRd["OPENING_DEPOSIT"]);
                close_deposit = Convert.ToInt32(ckon.sqlDataRd["CLOSING_DEPOSIT"]);
                real_deposit = Convert.ToInt32(ckon.sqlDataRd["REAL_DEPOSIT"]);
                dispute_deposit = Convert.ToInt32(ckon.sqlDataRd["DISPUTE_DEPOSIT"]);
                dev_name = Convert.ToString(ckon.sqlDataRd["DEVICE_NAME"]);
                status = Convert.ToString(ckon.sqlDataRd["STATUS_CLOSE"]);
                epy_id = Convert.ToString(ckon.sqlDataRd["EMPLOYEE_ID"]);
                epy_name = Convert.ToString(ckon.sqlDataRd["EMPLOYEE_NAME"]);
            }
            ckon.sqlCon().Close();
            ClosingStore close = new ClosingStore()
            {
                closingStoreId = id_store,
                sequenceNumber = seq_number_substring,
                storeCode = store_id,
                //shiftCode = shift,
                openingTimestamp = opening_time,
                closingTimestamp = closing_time,
                openingTransBal = open_trans_balance,
                closingTransBal = closing_trans_balance,
                realTransBal = real_trans_balance,
                disputeTransBal = dispute_trans_balance,
                openingPettyCash = open_pety,
                closingPettyCash = close_pety,
                realPettyCash = real_pety,
                disputePettyCash = dispute_pety,
                openingDeposit = open_deposit,
                closingDeposit = close_deposit,
                realDeposit = real_deposit,
                disputeDeposit = dispute_deposit,
                deviceName = dev_name,
                statusClose = status,
                employeeId = epy_id,
                employeeName = epy_name
            };
            var stringPayload = JsonConvert.SerializeObject(close);
            String response = "";
            var credentials = new NetworkCredential("username", "password");
            var handler = new HttpClientHandler { Credentials = credentials };
            var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");
            using (var client = new HttpClient(handler))
            {
                //HttpResponseMessage message = client.PostAsync("http://retailbiensi.azurewebsites.net/api/ClosingStore", httpContent).Result;
                HttpResponseMessage message = client.PostAsync(link_api+"/api/ClosingStore", httpContent).Result;
            }

        }
        //================================================================
    }
}
