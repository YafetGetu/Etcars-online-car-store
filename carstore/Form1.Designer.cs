using System.ComponentModel.Design;

namespace carstore
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            panel1 = new Panel();
            panel5 = new Panel();
            panel6 = new Panel();
            panel7 = new Panel();
            panel8 = new Panel();
            panel9 = new Panel();
            panel10 = new Panel();
            panel11 = new Panel();
            panel12 = new Panel();
            panel13 = new Panel();
            pictureBox1 = new PictureBox();
            panel4 = new Panel();
            panel2 = new Panel();
            panel3 = new Panel();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = Color.White;
            panel1.Dock = DockStyle.Left;
            panel1.Location = new Point(0, 0);
            panel1.Margin = new Padding(2);
            panel1.Name = "panel1";
            panel1.Size = new Size(219, 844);
            panel1.TabIndex = 0;
            panel1.Paint += panel1_Paint;
            // 
            // panel5
            // 
            panel5.Location = new Point(0, 0);
            panel5.Name = "panel5";
            panel5.Size = new Size(200, 100);
            panel5.TabIndex = 15;
            // 
            // panel6
            // 
            panel6.Location = new Point(0, 0);
            panel6.Name = "panel6";
            panel6.Size = new Size(200, 100);
            panel6.TabIndex = 14;
            // 
            // panel7
            // 
            panel7.Location = new Point(0, 0);
            panel7.Name = "panel7";
            panel7.Size = new Size(200, 100);
            panel7.TabIndex = 13;
            // 
            // panel8
            // 
            panel8.Location = new Point(0, 0);
            panel8.Name = "panel8";
            panel8.Size = new Size(200, 100);
            panel8.TabIndex = 12;
            panel8.Paint += panel8_Paint_1;
            // 
            // panel9
            // 
            panel9.BackColor = Color.Transparent;
            panel9.Location = new Point(1216, 538);
            panel9.Margin = new Padding(2);
            panel9.Name = "panel9";
            panel9.Size = new Size(320, 48);
            panel9.TabIndex = 7;
            panel9.Paint += panel9_Paint;
            // 
            // panel10
            // 
            panel10.BackColor = Color.Transparent;
            panel10.Location = new Point(557, 10);
            panel10.Margin = new Padding(2);
            panel10.Name = "panel10";
            panel10.Size = new Size(330, 226);
            panel10.TabIndex = 8;
            panel10.Paint += panel10_Paint;
            // 
            // panel11
            // 
            panel11.BackColor = Color.Transparent;
            panel11.Location = new Point(1038, 135);
            panel11.Margin = new Padding(2);
            panel11.Name = "panel11";
            panel11.Size = new Size(108, 100);
            panel11.TabIndex = 8;
            panel11.Paint += panel11_Paint_2;
            // 
            // panel12
            // 
            panel12.BackColor = Color.Transparent;
            panel12.Location = new Point(314, 135);
            panel12.Margin = new Padding(2);
            panel12.Name = "panel12";
            panel12.Size = new Size(105, 100);
            panel12.TabIndex = 9;
            panel12.Paint += panel12_Paint;
            // 
            // panel13
            // 
            panel13.BackColor = Color.Transparent;
            panel13.Location = new Point(645, 702);
            panel13.Margin = new Padding(2);
            panel13.Name = "panel13";
            panel13.Size = new Size(242, 48);
            panel13.TabIndex = 3;
            panel13.Paint += panel13_Paint_1;
            // 
            // pictureBox1
            // 
            pictureBox1.Location = new Point(334, 92);
            pictureBox1.Margin = new Padding(2);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(730, 578);
            pictureBox1.TabIndex = 1;
            pictureBox1.TabStop = false;
            pictureBox1.Click += pictureBox1_Click;
            // 
            // panel4
            // 
            panel4.BackColor = Color.Transparent;
            panel4.Location = new Point(1216, 187);
            panel4.Margin = new Padding(2);
            panel4.Name = "panel4";
            panel4.Size = new Size(320, 48);
            panel4.TabIndex = 3;
            panel4.Paint += panel4_Paint;
            // 
            // panel2
            // 
            panel2.BackColor = Color.Transparent;
            panel2.Location = new Point(1216, 119);
            panel2.Margin = new Padding(2);
            panel2.Name = "panel2";
            panel2.Size = new Size(320, 48);
            panel2.TabIndex = 10;
            panel2.Paint += panel2_Paint_2;
            // 
            // panel3
            // 
            panel3.BackColor = Color.Transparent;
            panel3.Location = new Point(1116, 52);
            panel3.Margin = new Padding(2);
            panel3.Name = "panel3";
            panel3.Size = new Size(320, 48);
            panel3.TabIndex = 11;
            panel3.Paint += panel3_Paint_1;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            AutoValidate = AutoValidate.EnableAllowFocusChange;
            ClientSize = new Size(1536, 844);
            Controls.Add(panel3);
            Controls.Add(panel2);
            Controls.Add(panel13);
            Controls.Add(panel12);
            Controls.Add(panel11);
            Controls.Add(panel10);
            Controls.Add(panel9);
            Controls.Add(panel8);
            Controls.Add(panel7);
            Controls.Add(panel6);
            Controls.Add(panel5);
            Controls.Add(panel4);
            Controls.Add(panel1);
            Controls.Add(pictureBox1);
            ForeColor = SystemColors.ActiveCaptionText;
            Margin = new Padding(2);
            Name = "Form1";
            StartPosition = FormStartPosition.Manual;
            Text = "Form1";
            WindowState = FormWindowState.Maximized;
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Panel panel5;
        private Panel panel6;
        private Panel panel7;
        private Panel panel8;
        private Panel panel9;
        private Panel panel10;
        private Panel panel11;
        private Panel panel12;
        private Panel panel13;
        private PictureBox pictureBox1;
        private Panel panel4;
        private Panel panel2;
        private Panel panel3;
    }
}