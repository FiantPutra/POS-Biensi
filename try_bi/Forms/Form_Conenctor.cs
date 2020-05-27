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
using System.Xml;
using System.Net.NetworkInformation;
using Microsoft.Reporting.WinForms;
using System.Drawing.Printing;
using System.IO;
using System.Drawing.Imaging;

namespace try_bi
{
    public partial class Form_Conenctor : Form
    {
        String replace_pass, VarBackDate, StringBackDate, OnOrOff, MasterOrChild, DeviceCode = "", pDefault;
        String link = "", host = "", dbName="", userDB="", msgDbName = "", printerName = "";
        String ftpFilePath, storeId;
        public Form_Conenctor()
        {
            InitializeComponent();
        }        

        private void Form_Conenctor_Load(object sender, EventArgs e)
        {
            //radioOnline.Checked = true;

            //groupOnline.Visible = true;
            //groupOffline.Visible = false;

            dateTimePicker1.Visible = false;

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("C:/Program Files/Pos Biensi/xmlConn.xml");

            string xpath = "Table/Product";
            var nodes = xmlDoc.SelectNodes(xpath);

            foreach (XmlNode childrenNode in nodes)
            {
                txtApiUrl.Text = childrenNode.SelectSingleNode("link_api").InnerText;
                txtHostDB.Text = childrenNode.SelectSingleNode("host_db").InnerText;
                textUserDb.Text = childrenNode.SelectSingleNode("user_db").InnerText;
                txtPassDB.Text = childrenNode.SelectSingleNode("pass_db").InnerText;
                txtPOSNameDB.Text = childrenNode.SelectSingleNode("name_db").InnerText;
                txtMsgNameDB.Text = childrenNode.SelectSingleNode("msg_db").InnerText;
                txtFilePath.Text = childrenNode.SelectSingleNode("FilePath").InnerText;
                txt_storeId.Text = childrenNode.SelectSingleNode("storeId").InnerText;
                nm_printer.Text = childrenNode.SelectSingleNode("printer_name").InnerText;
            }

            //LabelYesNo.Text = Properties.Settings.Default.mBackDate;
            //txtPassDB.Text = Properties.Settings.Default.mPassDB;
            //nm_printer.Text = Properties.Settings.Default.mPrinter;
            ////txtOff.Text = Properties.Settings.Default.mServer;
            //textUserDb.Text = Properties.Settings.Default.mUserDB;
            //txtHostDB.Text = Properties.Settings.Default.mServer;
            //txtPOSNameDB.Text = Properties.Settings.Default.mDBName;
            //txtMsgNameDB.Text = Properties.Settings.Default.msgDBName;            
            //txtFilePath.Text = Properties.Settings.Default.FTPFilePath;    
            //txt_storeId.Text = Properties.Settings.Default.StoreId;
        }

        private void b_save_Click(object sender, EventArgs e)
        {
            //String link = "http://mpos.biensicore.co.id";
            //String link = "http://localhost:64994";

            //if (OnOrOff == "Offline")
            //{
            //    host = txtOff.Text;
            //    userDB = textUserDb.Text;
            //    //MethodGetDevId();
            //}
            
            if (checkboxBackDate.Checked == true)
            {
                LabelYesNo.Text = "YES";
            }
            else
            {
                LabelYesNo.Text = "NO";
            }

            if (defaultPrint.Checked)
            {
                pDefault = "1";
            } 
            else
            {
                pDefault = "0";
            }

            try
            {
                link = txtApiUrl.Text;
                host = txtHostDB.Text;
                dbName = txtPOSNameDB.Text;
                msgDbName = txtMsgNameDB.Text;                
                userDB = textUserDb.Text;                
                replace_pass = txtPassDB.Text.Replace(" ", "");                
                ftpFilePath = txtFilePath.Text;
                storeId = txt_storeId.Text;
                printerName = nm_printer.Text;

                //Properties.Settings.Default.mServer = host;
                //Properties.Settings.Default.mDBName = dbName;
                //Properties.Settings.Default.mUserDB = userDB;
                //Properties.Settings.Default.mPassDB = replace_pass;
                //Properties.Settings.Default.mPrinter = nm_printer.Text;
                //Properties.Settings.Default.mBackDate = LabelYesNo.Text;
                //Properties.Settings.Default.ValueBackDate = dateTimePicker1.Text;
                //Properties.Settings.Default.msgDBName = msgDbName;                
                //Properties.Settings.Default.FTPFilePath = ftpFilePath;                               
                //Properties.Settings.Default.OnnOrOff = OnOrOff;
                //Properties.Settings.Default.MstrOrChld = MasterOrChild;
                //Properties.Settings.Default.DevCode = DeviceCode;
                //Properties.Settings.Default.printerDefault = pDefault;
                //Properties.Settings.Default.StoreId = storeId;

                //save_app_config();
                //Properties.Settings.Default.Save();

                XmlDocument doc = new XmlDocument();
                doc.Load("C:/Program Files/Pos Biensi/xmlConn.xml");

                XmlNode node2 = doc.SelectSingleNode("Table/Product/link_api[1]");
                node2.InnerText = link;
                XmlNode node3 = doc.SelectSingleNode("Table/Product/code_pc[1]");
                node3.InnerText = DeviceCode;
                XmlNode host_db = doc.SelectSingleNode("Table/Product/host_db[1]");
                host_db.InnerText = host;
                XmlNode name_db = doc.SelectSingleNode("Table/Product/name_db[1]");
                name_db.InnerText = dbName;
                XmlNode msg_db = doc.SelectSingleNode("Table/Product/msg_db[1]");
                msg_db.InnerText = msgDbName;
                XmlNode user_db = doc.SelectSingleNode("Table/Product/user_db[1]");
                user_db.InnerText = userDB;
                XmlNode pass_db = doc.SelectSingleNode("Table/Product/pass_db[1]");
                pass_db.InnerText = replace_pass;                               
                XmlNode DownloadFilePath = doc.SelectSingleNode("Table/Product/FilePath[1]");
                DownloadFilePath.InnerText = ftpFilePath;
                XmlNode store_Id = doc.SelectSingleNode("Table/Product/storeId[1]");
                store_Id.InnerText = storeId;
                //XmlNode printer_dfl = doc.SelectSingleNode("Table/Product/printer_default[1]");
                //printer_dfl.InnerText = pDefault;
                XmlNode printer = doc.SelectSingleNode("Table/Product/printer_name[1]");
                printer.InnerText = printerName;
                doc.Save("C:/Program Files/Pos Biensi/xmlConn.xml");

                MessageBox.Show("Connection Successfully Saved. Application Will Be Closed, Please Re-Open");
                Application.Restart();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Please Open The Aplication With Right Click, Run As Administrator", "Run As Administratror", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }

        }

        public void save_app_config()
        {
            //String connectionString = string.Format("User Id={0};Password={1};Host={2};Database={3};Persist Security Info=True", userDB, replace_pass, host, dbName);
            String connectionString = string.Format("Data Source={0};Initial Catalog={1};Integrated Security=False;Persist Security Info=True;User ID={2};Password={3}", host, dbName, userDB, replace_pass);
            try
            {
                AppSetting setting = new AppSetting();
                setting.SaveConnectionString("BiensiDataContextConnectionString", connectionString);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }        

        private void LinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            LinkApi ls = new LinkApi();
            string printer_name = ls.printer_name;
            MessageBox.Show("{System: '" + nm_printer.Text + "' #Entry: '" + printer_name + "'}");

            if (printer_name != nm_printer.Text && printer_name != "")
            {
                MessageBox.Show("Masukkan nama printer dan klik save dibawah. Buka kembali aplikasi dan test print. {System: '"+nm_printer.Text+"' #Entry: '"+printer_name+"'}");
            }
            else if (printer_name == "")
            {
                MessageBox.Show("Masukkan nama printer dan klik save dibawah. Buka kembali aplikasi dan test print. {System: '" + nm_printer.Text + "' #Entry: '" + printer_name + "'}");
            }
            else
            {
                Cursor.Current = Cursors.WaitCursor;
                Coba_Print_Baru print = new Coba_Print_Baru();
                print.coba_print();
                Cursor.Current = Cursors.Default;
            }
        }

        private void linkLabel1_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            HO_Print m = new HO_Print();
            m.set_print("IO/HO-1909-00014");
        }

        private void checkboxBackDate_CheckedChanged(object sender, EventArgs e)
        {
            if(checkboxBackDate.Checked == true)
            {
                dateTimePicker1.Visible = true;
                LabelYesNo.Text = "YES";
            }
            else
            {
                dateTimePicker1.Visible = false;
                LabelYesNo.Text = "NO";

            }
            
        }

        private void b_MacAdrs_Click(object sender, EventArgs e)
        {
            string mac = "";
            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {

                if (nic.OperationalStatus == OperationalStatus.Up && (!nic.Description.Contains("Virtual") && !nic.Description.Contains("Pseudo")))
                {
                    if (nic.GetPhysicalAddress().ToString() != "")
                    {
                        mac = nic.GetPhysicalAddress().ToString();
                    }
                }
            }
            //MessageBox.Show(mac);

            API_DeviceCode code = new API_DeviceCode();
            code.cek_storeCode();
            DeviceCode = code.GetDeviceId(mac);
            //MessageBox.Show(DeviceCode);
        }

        ////=======================MEMUNCULKAN GROUP BOX ONLINE TRUE============================
        //private void radioOnline_CheckedChanged(object sender, EventArgs e)
        //{
        //    groupOnline.Visible = true;
        //    groupOffline.Visible = false;
        //    radioMaster.Checked = false;
        //    radioChild.Checked = false;
        //    OnOrOff = "Online";
        //    DeviceCode = "null";
        //}
        ////=====================MEMUNCULKAN GROUP BOX OFFLINE TRUE=============================
        //private void radioOffline_CheckedChanged(object sender, EventArgs e)
        //{
        //    groupOnline.Visible = true;
        //    groupOffline.Visible = true;
        //    radioMaster.Checked = true;
        //    OnOrOff = "Offline";
        //}
        //===============Set to PC Master=================================
        private void radioMaster_CheckedChanged(object sender, EventArgs e)
        {
            MasterOrChild = "Master";
        }
        //===============Set to PC Child==================================
        private void radioChild_CheckedChanged(object sender, EventArgs e)
        {
            MasterOrChild = "Child";
        }
        //================================================================
        public void MethodGetDevId()
        {
            string mac = "";
            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {

                if (nic.OperationalStatus == OperationalStatus.Up && (!nic.Description.Contains("Virtual") && !nic.Description.Contains("Pseudo")))
                {
                    if (nic.GetPhysicalAddress().ToString() != "")
                    {
                        mac = nic.GetPhysicalAddress().ToString();
                    }
                }
            }
            //MessageBox.Show(mac);

            API_DeviceCode code = new API_DeviceCode();
            //code.LinkGetCode(txtOff.Text);
            code.cek_storeCode();
            DeviceCode = code.GetDeviceId(mac);
            //MessageBox.Show(DeviceCode);
        }
    }
}
