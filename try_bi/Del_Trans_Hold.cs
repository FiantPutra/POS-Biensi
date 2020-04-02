using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Data;

namespace try_bi
{
    class Del_Trans_Hold
    {
        String  id, ID_TRANS, id_substring, bulan_now, tahun_now;
        int id_substring2;
        koneksi ckon = new koneksi();
        koneksi2 ckon2 = new koneksi2();
        public void get_data(String id2)
        {
            id = id2;
        }

        //DELETE TRANSAKSI sesuai id shift yang dikirim
        public void del_trans()
        {
            string command;                        

            try
            {                
                command = "DELETE transaction_line FROM transaction_line INNER JOIN [transaction] "
                            + "ON [transaction].TRANSACTION_ID = transaction_line.TRANSACTION_ID "
                            + "WHERE [transaction].ID_SHIFT = '" + id + "' AND [transaction].STATUS = '0'";
                CRUD del_TransLine = new CRUD();
                del_TransLine.ExecuteNonQuery(command);

                command = "DELETE FROM [transaction] WHERE [transaction].ID_SHIFT = '" + id + "' AND [transaction].STATUS = '0'";
                CRUD del_Transaction = new CRUD();
                del_Transaction.ExecuteNonQuery(command);

                command = "DELETE inventory_line FROM inventory_line INNER JOIN [transaction] "
                            + "ON [transaction].TRANSACTION_ID = inventory_line.TRANS_REF_ID "
                            + "WHERE [transaction].ID_SHIFT = '" + id + "' AND [transaction].STATUS = '0'";
                CRUD del_InvLine = new CRUD();
                del_InvLine.ExecuteNonQuery(command);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }            
        }

        //MENDAPATKAN id transaksi terakhir berdasarkan id_shift dan status 1, ubah running number ke int, lalu update ke table auto_number
        public void update_runningNumber()
        {
            string command;                        

            try
            {
                get_year_month();

                ckon.sqlCon().Open();
                command = "SELECT TOP 1 SUBSTRING(TRANSACTION_ID, 13, LEN(TRANSACTION_ID)) AS inv FROM [transaction] ORDER BY TRANSACTION_ID DESC";
                CRUD sql = new CRUD();
                ckon.sqlDataRd = sql.ExecuteDataReader(command, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        id_substring = ckon.sqlDataRd["inv"].ToString();
                        id_substring2 = Convert.ToInt32(id_substring);
                    }

                    command = "UPDATE auto_number SET Number='" + id_substring2 + "' WHERE Type_Trans = '1' AND Year = '"+ tahun_now +"' AND Month = '"+ bulan_now +"'";
                    CRUD update = new CRUD();
                    update.ExecuteNonQuery(command);
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

        public void get_year_month()
        {
            DateTime mydate = DateTime.Now;            

            bulan_now = mydate.ToString("MM");
            tahun_now = mydate.ToString("yy");
        }

        //String a = "TR/AAB-1803-10104";
        //String b = a.Substring(12);
        //int c = Convert.ToInt32(b);
        //MessageBox.Show(b+",,,,"+c);
    }
}
