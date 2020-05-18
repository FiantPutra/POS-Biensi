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
    public partial class W_StockTake_Confirm : Form
    {
        public W_StockTake_Confirm()
        {
            InitializeComponent();
        }

        private void b_ok_Click(object sender, EventArgs e)
        {
            UC_Stock_Take.Instance.method_confirm();
            this.Close();
        }

        private void b_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
