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
    public partial class W_PettyCash_Confirm : Form
    {
        public W_PettyCash_Confirm()
        {
            InitializeComponent();
        }

        private void b_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        //button OK
        private void b_ok_Click(object sender, EventArgs e)
        {
            UC_Petty_Cash.Instance.method_confim_ok();
            this.Close();
        }
    }
}
