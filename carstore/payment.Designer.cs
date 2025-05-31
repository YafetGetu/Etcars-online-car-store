namespace carstore
{
    partial class payment
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            panel1 = new Panel();
            textBox5 = new TextBox();
            label6 = new Label();
            textBox4 = new TextBox();
            label5 = new Label();
            textBox3 = new TextBox();
            label4 = new Label();
            textBox2 = new TextBox();
            label3 = new Label();
            textBox1 = new TextBox();
            label2 = new Label();
            label1 = new Label();
            label7 = new Label();
            button1 = new Button();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = Color.Navy;
            panel1.Controls.Add(button1);
            panel1.Controls.Add(label7);
            panel1.Controls.Add(textBox5);
            panel1.Controls.Add(label6);
            panel1.Controls.Add(textBox4);
            panel1.Controls.Add(label5);
            panel1.Controls.Add(textBox3);
            panel1.Controls.Add(label4);
            panel1.Controls.Add(textBox2);
            panel1.Controls.Add(label3);
            panel1.Controls.Add(textBox1);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(label1);
            panel1.Location = new Point(50, 30);
            panel1.Name = "panel1";
            panel1.Size = new Size(1030, 647);
            panel1.TabIndex = 0;
            panel1.Paint += panel1_Paint;
            // 
            // textBox5
            // 
            textBox5.Location = new Point(664, 132);
            textBox5.Name = "textBox5";
            textBox5.Size = new Size(90, 27);
            textBox5.TabIndex = 10;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.ForeColor = Color.White;
            label6.Location = new Point(596, 139);
            label6.Name = "label6";
            label6.Size = new Size(36, 20);
            label6.TabIndex = 9;
            label6.Text = "CVV";
            // 
            // textBox4
            // 
            textBox4.Location = new Point(664, 80);
            textBox4.Name = "textBox4";
            textBox4.Size = new Size(288, 27);
            textBox4.TabIndex = 8;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.ForeColor = Color.White;
            label5.Location = new Point(543, 83);
            label5.Name = "label5";
            label5.Size = new Size(98, 20);
            label5.TabIndex = 7;
            label5.Text = "Card Number";
            // 
            // textBox3
            // 
            textBox3.Location = new Point(180, 191);
            textBox3.Name = "textBox3";
            textBox3.Size = new Size(250, 27);
            textBox3.TabIndex = 6;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.ForeColor = Color.White;
            label4.Location = new Point(59, 194);
            label4.Name = "label4";
            label4.Size = new Size(79, 20);
            label4.TabIndex = 5;
            label4.Text = "Last Name";
            // 
            // textBox2
            // 
            textBox2.Location = new Point(180, 138);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(250, 27);
            textBox2.TabIndex = 4;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.ForeColor = Color.White;
            label3.Location = new Point(59, 141);
            label3.Name = "label3";
            label3.Size = new Size(100, 20);
            label3.TabIndex = 3;
            label3.Text = "Middle Name";
            // 
            // textBox1
            // 
            textBox1.Location = new Point(180, 83);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(250, 27);
            textBox1.TabIndex = 2;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.ForeColor = Color.White;
            label2.Location = new Point(59, 86);
            label2.Name = "label2";
            label2.Size = new Size(80, 20);
            label2.TabIndex = 1;
            label2.Text = "First Name";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.ForeColor = Color.White;
            label1.Location = new Point(117, 33);
            label1.Name = "label1";
            label1.Size = new Size(122, 20);
            label1.TabIndex = 0;
            label1.Text = "Card information";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.ForeColor = Color.White;
            label7.Location = new Point(117, 260);
            label7.Name = "label7";
            label7.Size = new Size(160, 20);
            label7.TabIndex = 11;
            label7.Text = "Car Model information";
            // 
            // button1
            // 
            button1.Location = new Point(446, 563);
            button1.Name = "button1";
            button1.Size = new Size(120, 29);
            button1.TabIndex = 12;
            button1.Text = "pay now";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // payment
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1144, 707);
            Controls.Add(panel1);
            Name = "payment";
            Text = "payment";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);

        }

        #endregion

        private Panel panel1;
        private Label label2;
        private Label label1;
        private TextBox textBox5;
        private Label label6;
        private TextBox textBox4;
        private Label label5;
        private TextBox textBox3;
        private Label label4;
        private TextBox textBox2;
        private Label label3;
        private TextBox textBox1;
        private Button button1;
        private Label label7;
    }
}