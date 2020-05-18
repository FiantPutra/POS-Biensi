using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Data.SqlClient;

namespace try_bi
{
    public partial class Form_First_Opened : Form
    {
        koneksi ckon = new koneksi();
        koneksi2 ckon2 = new koneksi2();
        koneksi3 ckon3 = new koneksi3();
        koneksi4 ckon4 = new koneksi4();        
        String pesan;
        public Form_First_Opened()
        {
            InitializeComponent();
        }

        //button buka POS BIENSI
        private void b_biensi_Click(object sender, EventArgs e)
        {
            this.Hide();
            SplashScreen splash = new SplashScreen();
            splash.ShowDialog();
            this.Close();

            /*
            // Aslinya kodingan untuk buka POS
            String coba_a = "coba a"; String coba_b = "coba_b";
            
            API_CekVersion cek = new API_CekVersion();
            cek.GetVoucher().Wait();

            pesan = cek.cek_ver();
            var pesan2 = cek.cek_ver2(coba_a, coba_b);

            //mengecel apakah jenis aplikasi yang di instal sudah sesuai dengan versi terbaru dari backend, jika tidak maka tidak bisa dibuka
            if (pesan == "Same")
            {
                this.Hide();
                SplashScreen splash = new SplashScreen();
                splash.ShowDialog();
                this.Close();
            }
            else if (pesan == "NotSame")
            {
               DialogResult dialog =  MessageBox.Show("Application Version Needs To Be Updated, the Date of latest version is "+ pesan2.Item1 +".  Please download the latest version. Click OK to open link "+pesan2.Item2, "Version Sistem", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                //System.Diagnostics.Process.Start("www.google.com");
                try
                {
                    if (dialog == DialogResult.OK)
                    {
                        System.Diagnostics.Process.Start(pesan2.Item2);
                    }
                }
                catch
                {
                    MessageBox.Show("Please Connect To Internet", "Connect To Internet", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                
            }
            else
            {
                MessageBox.Show("Please Select First Store In POS Connector Application", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            */

        }

        private void b_connection_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form_Conenctor con = new Form_Conenctor();
            con.ShowDialog();
            this.Close();
        }
        public void tes_koneksi()
        {
            //string connectionString = "";
            //LinkApi xmlCon = new LinkApi();
            //connectionString = "Data Source='" + xmlCon.host_db + "';Initial Catalog='" + xmlCon.name_db + "';User ID='" + xmlCon.user_db + "';Password='" + xmlCon.pass_db + "'";
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

        public void test_sqlCon()
        {            
            try
            {
                ckon.sqlCon().Open();
                MessageBox.Show("Connection Open ! ");
                ckon.sqlCon().Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Can not open connection ! ");
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

        private void Form_First_Opened_Load(object sender, EventArgs e)
        {
            label_version.Text = try_bi.Properties.Settings.Default.mVersion;
            tes_koneksi();
            //test_sqlCon()();
        }

        //private void button1_Click(object sender, EventArgs e)
        //{
        //    datagridview DG = new datagridview();
        //    DG.ShowDialog();
        //}
    }
}
