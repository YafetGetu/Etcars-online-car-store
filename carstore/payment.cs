using System;
using System.Collections.Generic; 
using System.ComponentModel; 
using System.Data; 
using System.Drawing;
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 
using System.Windows.Forms;
using carstore; 

namespace carstore
{
    public partial class payment : Form
    {
        public payment()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(0, 40, 85); 
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {


            MessageBox.Show("Payment Successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

            this.DialogResult = DialogResult.OK;

            this.Close();
        }

        private void payment_FormClosing(object sender, FormClosingEventArgs e)
        {
            
            if (this.DialogResult != DialogResult.OK)
            {
                this.DialogResult = DialogResult.Cancel;
            }
        }


    }
}