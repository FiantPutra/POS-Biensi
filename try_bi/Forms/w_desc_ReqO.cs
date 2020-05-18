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

namespace try_bi
{
    public partial class w_desc_ReqO : Form
    {
        koneksi ckon = new koneksi();
        String qty2, cust_id2, id_epy2, nm_epy2, id2, no_sj2;
        int total2;
        public w_desc_ReqO()
        {
            InitializeComponent();
        }
        //LOAD FORM
        private void w_desc_ReqO_Load(object sender, EventArgs e)
        {
            this.ActiveControl = t_desc;
            t_desc.Focus();
        }

        //menerima variabel dari halaman request order
        public void get_data(String qty, String cust_id, String id_epy, String nm_epy, String id, int total, String no_sj)
        {
            qty2 = qty;
            cust_id2 = cust_id;
            id_epy2 = id_epy;
            nm_epy2 = nm_epy;
            id2 = id;
            total2 = total;
            no_sj2 = no_sj;
        }

        //button ok
        private void b_ok_Click(object sender, EventArgs e)
        {
            try
            {
                String cmd_update = "UPDATE requestorder SET TOTAL_QTY='" + qty2 + "', STATUS='1', CUST_ID_STORE='" + cust_id2 + "', EMPLOYEE_ID='" + id_epy2 + "', EMPLOYEE_NAME='" + nm_epy2 + "', DESCRIPTION = '" + t_desc.Text + "', TOTAL_AMOUNT='"+ total2 +"', NO_SJ='"+ no_sj2 +"' WHERE REQUEST_ORDER_ID = '" + id2 + "'";
                CRUD update = new CRUD();
                update.ExecuteNonQuery(cmd_update);
                MessageBox.Show("data successfully added");
                //RunningNumber running = new RunningNumber();
                //running.get_data_before("2", "RO");
                UC_Req_order.Instance.reset();
                UC_Req_order.Instance.new_invoice();
                UC_Req_order.Instance.set_running_number();
                this.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                this.Close();
            }

        }
        private void b_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
