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
using System.Data.SqlClient;

namespace try_bi
{
    public partial class Form_Login : Form
    {
        koneksi ckon = new koneksi();
        DBConn dbConn = new DBConn();
        String nm_employe, id_employee, store_code;
        public Form_Login()
        {
            InitializeComponent();
        }

        private void Form_Login_Load(object sender, EventArgs e)
        {
            tes_koneksi();
            this.ActiveControl = t_username;
            t_username.Focus();
            t_pass.isPassword = true;
            //Form1 f1 = new Form1();
            //f1.Close();
        }

        private void b_login_Click(object sender, EventArgs e)
        {            
            loginSQL();
        }
        //===============================================================================
        private void b_close_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        //===LOGIN MENGGUNAKAN ENTER=============
        private void t_username_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == (char)Keys.Enter)
            {
                b_login_Click(null, null);
            }
        }
        //===LOGIN MENGGUNAKAN ENTER=============
        private void t_pass_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                b_login_Click(null, null);
            }
        }

        //===================LINK GANTI PASSWORD=======================================        
        private void ChangePassBtn_Click(object sender, EventArgs e)
        {
            W_ChangePass change = new W_ChangePass();
            change.ShowDialog();
        }

        private void configBtn_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form_Conenctor con = new Form_Conenctor();
            con.ShowDialog();
            this.Close();
        }

        //==========================LOGIN================================================        
        public void loginSQL()
        {
            string command;            
            int count0 = 0;

            try
            {
                ckon.sqlCon().Open();
                command = "SELECT TOP 1 * FROM employee WHERE EMPLOYEE_ID = '" + t_username.Text + "' and CONVERT(nvarchar, Pass) = '" + t_pass.Text + "'";
                CRUD sql = new CRUD();
                ckon.sqlDataRd = sql.ExecuteDataReader(command, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        count0 = count0 + 1;

                        nm_employe = ckon.sqlDataRd["NAME"].ToString();
                        id_employee = ckon.sqlDataRd["EMPLOYEE_ID"].ToString();
                        store_code = ckon.sqlDataRd["STORE_CODE"].ToString();
                    }

                    if (count0 != 0)
                    {
                        this.Hide();
                        Form1 fm1 = new Form1();
                        fm1.nama_employee = nm_employe;
                        fm1.id_employee = id_employee;
                        fm1.store_code = store_code;
                        fm1.setHo();
                        fm1.ShowDialog();
                        this.Close();
                    }
                }
                else
                {
                    MessageBox.Show("Username or Password Not Valid");
                }
            }
            catch(Exception e)
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

        public void tes_koneksi()
        {           
            try
            {
                ckon.sqlCon().Open();
                checkConnectionString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Connection refused! Please configure a password on the connection.");
                var appLog = new System.Diagnostics.EventLog("Pos Biensi");
                appLog.Source = "Connection";
            }
            finally
            {
                if (ckon.sqlCon().State == ConnectionState.Open)
                    ckon.sqlCon().Close();
            }
        }

        private void checkConnectionString()
        {
            LinkApi xmlCon = new LinkApi();
            AppSetting setting = new AppSetting();
            String exist_con = setting.GetConnectionString("BiensiPosDbDataContextConnectionString");
            String new_con = string.Format("User Id={0};Password={1};Host={2};Database={3};Persist Security Info=True", xmlCon.user_db, xmlCon.pass_db, xmlCon.host_db, xmlCon.name_db);

            if (exist_con != new_con)
            {
                //save connection
                setting.SaveConnectionString("BiensiPosDbDataContextConnectionString", new_con);
                Application.Restart();
            }
        }
    }
}
