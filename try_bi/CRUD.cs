using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace try_bi
{
    class CRUD
    {
        koneksi ckon = new koneksi();

        //==============METHOD INPUT, DELETE, Dan Edit============================================
        public void ExecuteNonQuery(String query)
        {
            string command;
            SqlCommand sqlCmd = null;
            SqlDataReader sqlDataRd = null;
            SqlConnection sqlCon;
            
            sqlCon = ckon.sqlCon();
            try
            {                
                if (sqlCon.State == ConnectionState.Closed)
                    sqlCon.Open();

                sqlCmd = new SqlCommand(query);
                sqlCmd.Connection = sqlCon;
                sqlCmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {     
                if (sqlCon.State == ConnectionState.Open)
                    sqlCon.Close();
            }
        }
        //=========================================================================================

        public void ExecuteNonQueryMsg(String query)
        {
            string command;
            SqlCommand sqlCmd = null;
            SqlDataReader sqlDataRd = null;
            SqlConnection sqlCon;
            
            sqlCon = ckon.sqlConMsg();
            try
            {
                if (sqlCon.State == ConnectionState.Closed)
                    sqlCon.Open();

                sqlCmd = new SqlCommand(query);
                sqlCmd.Connection = sqlCon;
                sqlCmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (sqlCon.State == ConnectionState.Open)
                    sqlCon.Close();
            }
        }

        public SqlDataReader ExecuteDataReader(String query, SqlConnection sqlCon)
         {
            sqlCon.Open();
            SqlCommand sqlCmd = null;
            SqlDataReader sqlDataRd = null;

            sqlCmd = new SqlCommand(query);
            sqlCmd.Connection = sqlCon;
            sqlDataRd = sqlCmd.ExecuteReader();

            return sqlDataRd;
        }

        public DataTable ExecuteDataTable(String query, SqlConnection sqlCon)
        {
            sqlCon.Open();
            SqlCommand sqlCmd = null;
            SqlDataAdapter sqlDataAdp = null;
            DataTable dt = new DataTable();

            sqlCmd = new SqlCommand(query);
            sqlCmd.Connection = sqlCon;
            sqlDataAdp = new SqlDataAdapter(sqlCmd);            
            sqlDataAdp.Fill(dt);

            return dt;
        }
    }
}
