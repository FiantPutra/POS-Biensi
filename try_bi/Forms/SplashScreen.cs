using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace try_bi
{
    public partial class SplashScreen : Form
    {
        public SplashScreen()
        {
            InitializeComponent();
        }

        private void SplashScreen_Load(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                rectangleShape2.Width += 55;
                if (rectangleShape2.Width >= 997)
                {
                    timer1.Stop();
                    this.Hide();
                    Form_Login fm_lgn = new Form_Login();
                    fm_lgn.ShowDialog();
                    this.Close();
                }
            }
            catch
            {
                return;
            }
        }
    }
}
