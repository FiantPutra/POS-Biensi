using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace try_bi
{
    public partial class w_logout : Form
    {
        public static Form1 f1;
        public w_logout(Form1 form1)
        {
            f1 = form1;
            InitializeComponent();
        }

        private void b_back_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void b_logout_Click(object sender, EventArgs e)
        {

            f1.Hide();
            this.Hide();
           
            //f1.Hide();
            //f1.Hide();
            Form_Login login = new Form_Login();
            login.ShowDialog();

            f1.Close();
            this.Close();
            //this.Hide();
            //Form_Login login = new Form_Login();
            //Form1 f1 = new Form1();

            //login.ShowDialog();
            //this.Close();
            //f1.Close();
            
            //Application.Exit();
        }
    }
}
