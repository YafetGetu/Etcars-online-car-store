using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace carstore
{
    public partial class payment : Form
    {
        public payment()
        {
            InitializeComponent();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            panel1.BackColor = Color.FromArgb(0, 40, 85);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //button1.BackColor = Color.Green;
            MessageBox.Show("Payment Successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

    }
}
