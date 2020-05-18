using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
using System.Data.SqlClient;

namespace try_bi
{
    class koneksi
    {
        LinkApi xmlCon = new LinkApi();
        private static string conString = "";
        public string config = "";
        public string msgSqlCon = "";
        public string conSQL = "";

        //public static String connectionString = "";
        public koneksi()
        {                       
            conString = "Server='" + xmlCon.host_db + "';Database='" + xmlCon.name_db + "';Uid='" + xmlCon.user_db + "';Pwd='" + xmlCon.pass_db + "';";
            config = "Server='" + xmlCon.host_db + "';Database='" + xmlCon.name_db + "';Uid='" + xmlCon.user_db + "';Pwd='" + xmlCon.pass_db + "';";
            conSQL = "Data Source='" + xmlCon.host_db + "';Initial Catalog='" + xmlCon.name_db + "';User ID='" + xmlCon.user_db + "';Password='" + xmlCon.pass_db + "'";
            msgSqlCon = "Data Source='" + xmlCon.host_db + "';Initial Catalog='" + xmlCon.msg_db + "';User ID='" + xmlCon.user_db + "';Password='" + xmlCon.pass_db + "'";
        }      
        
        public SqlConnection sqlCon()
        {
            LinkApi xmlCon = new LinkApi();
            String connectionString = "";
            connectionString = "Data Source='" + xmlCon.host_db + "';Initial Catalog='" + xmlCon.name_db + "';User ID='" + xmlCon.user_db + "';Password='" + xmlCon.pass_db + "'";
            SqlConnection sqlCon = new SqlConnection(connectionString);            

            return sqlCon;
        }

        public SqlConnection sqlConMsg()
        {
            LinkApi xmlCon = new LinkApi();
            String connectionString = "";
            connectionString = "Data Source='" + xmlCon.host_db + "';Initial Catalog='" + xmlCon.msg_db + "';User ID='" + xmlCon.user_db + "';Password='" + xmlCon.pass_db + "'";
            SqlConnection sqlConMsg = new SqlConnection(connectionString);

            return sqlConMsg;
        }
        
        public MySqlConnection con = new MySqlConnection(conString);
        public MySqlCommand cmd;
        public MySqlDataAdapter adapter;
        public DataTable dt = new DataTable();
        public DataSet ds = new DataSet();
        public MySqlDataReader myReader;

        //public sqlCon()nection sqlCon() = new sqlCon()nection();
        public SqlDataReader sqlDataRd = null;
        public SqlDataReader sqlDataRdHeader = null;
        public SqlDataReader sqlDataRdLine = null;
    }

    class DBConn
    {
        private static string conString = "";

        public DBConn()
        {
            conString = "Data Source=DESKTOP-J5OORT9\\SQLEXPRESS;Initial Catalog=Biensi;User ID=biensi_pos;Password=admin12345!";
        }
        public string sqlConString = conString;        
    }
    class koneksi2
    {
        LinkApi xmlCon = new LinkApi();
        private static string conString = "";
        public koneksi2()
        {
            conString = "Server='" + xmlCon.host_db + "';Database='" + xmlCon.name_db + "';Uid='" + xmlCon.user_db + "';Pwd='" + xmlCon.pass_db + "';";
        }
        //public static string conString2 = "Server='" + try_bi.Properties.Settings.Default.mServer + "';Database='" + try_bi.Properties.Settings.Default.mDBName + "';Uid='" + try_bi.Properties.Settings.Default.mUserDB + "';Pwd='" + try_bi.Properties.Settings.Default.mPassDB + "';";
        public MySqlConnection con2 = new MySqlConnection(conString);
        public MySqlCommand cmd2;
        public MySqlDataAdapter adapter2;
        public DataTable dt2 = new DataTable();
        public DataSet ds2 = new DataSet();
        public MySqlDataReader myReader2;
    }

    class koneksi3
    {
        LinkApi xmlCon = new LinkApi();
        private static string conString = "";
        public koneksi3()
        {
            conString = "Server='" + xmlCon.host_db + "';Database='" + xmlCon.name_db + "';Uid='" + xmlCon.user_db + "';Pwd='" + xmlCon.pass_db + "';";
        }
        //public static string conString3 = "Server='" + try_bi.Properties.Settings.Default.mServer + "';Database='" + try_bi.Properties.Settings.Default.mDBName + "';Uid='" + try_bi.Properties.Settings.Default.mUserDB + "';Pwd='" + try_bi.Properties.Settings.Default.mPassDB + "';";
        public MySqlConnection con3 = new MySqlConnection(conString);
        public MySqlCommand cmd3;
        public MySqlDataAdapter adapter3;
        public DataTable dt3 = new DataTable();
        public DataSet ds3 = new DataSet();
        public MySqlDataReader myReader3;
    }

    class koneksi4
    {
        LinkApi xmlCon = new LinkApi();
        private static string conString = "";
        public koneksi4()
        {
            conString = "Server='" + xmlCon.host_db + "';Database='" + xmlCon.name_db + "';Uid='" + xmlCon.user_db + "';Pwd='" + xmlCon.pass_db + "';";
        }
        //public static string conString4 = "Server='" + try_bi.Properties.Settings.Default.mServer + "';Database='" + try_bi.Properties.Settings.Default.mDBName + "';Uid='" + try_bi.Properties.Settings.Default.mUserDB + "';Pwd='" + try_bi.Properties.Settings.Default.mPassDB + "';";
        public MySqlConnection con4 = new MySqlConnection(conString);
        public MySqlCommand cmd4;
        public MySqlDataAdapter adapter4;
        public DataTable dt4 = new DataTable();
        public DataSet ds4 = new DataSet();
        public MySqlDataReader myReader4;
    }
}
