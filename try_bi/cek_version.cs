using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Data.SqlClient;
using System.Data;

namespace try_bi
{
    class cek_version
    {
        koneksi ckon = new koneksi();
        String message;
        String ver_apk = Properties.Settings.Default.mVersion;
        String ver_db;
        public string cek_ver()
        {
            string command;                        

            try
            {
                ckon.sqlCon().Open();
                command = "SELECT * FROM version_apk WHERE Type='POS'";
                CRUD sql = new CRUD();
                ckon.sqlDataRd = sql.ExecuteDataReader(command, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        ver_db = ckon.sqlDataRd["Version"].ToString();
                    }

                    if (ver_apk == ver_db)
                    {                        
                        message = "The Application Version Is up to date";
                    }
                    else
                    {                        
                        message = "Application Version Needs To Be Updated";
                    }
                }
                else
                {
                    message = "GetFirst";
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

            return message;
        }
    }
}
