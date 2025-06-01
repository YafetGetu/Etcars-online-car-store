using System;
using System.Drawing;
using System.Windows.Forms;

namespace carstore
{
    public partial class about : Form
    {
        public about()
        {
            InitializeComponent();
            this.Text = "About EtCars - Our Story";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(1150, 1000);
            this.BackColor = Color.FromArgb(0, 40, 85); 

            SetupControls();
        }

        private void SetupControls()
        {
            panel1.BackColor = Color.FromArgb(0, 40, 85);
            panel1.Dock = DockStyle.Fill;
            panel1.AutoScroll = true;

            int currentYPosition = 30; 
            int sectionSpacing = 30;
            int titleContentSpacing = 15; 

            // Company logo
            PictureBox logo = new PictureBox();
            logo.Size = new Size(500, 400);
            logo.Location = new Point(350, currentYPosition);
            currentYPosition += logo.Height + sectionSpacing;
            logo.SizeMode = PictureBoxSizeMode.Zoom;
            logo.Image = Image.FromFile("asset\\bg\\logo.jpg");
            logo.BackColor = Color.Transparent;
            panel1.Controls.Add(logo);

            // Main title (white text)
            Label mainTitle = new Label();
            mainTitle.Text = "Welcome to EtCars Automotive";
            mainTitle.Font = new Font("Arial", 28, FontStyle.Bold);
            mainTitle.ForeColor = Color.White; // White text
            mainTitle.AutoSize = true;
            mainTitle.Location = new Point(200, currentYPosition - 30);
            currentYPosition += mainTitle.Height + titleContentSpacing;
            panel1.Controls.Add(mainTitle);

            // Subtitle (light gray text)
            Label subTitle = new Label();
            subTitle.Text = "Driving Excellence Since 2010";
            subTitle.Font = new Font("Arial", 16, FontStyle.Italic);
            subTitle.ForeColor = Color.LightGray; 
            subTitle.AutoSize = true;
            subTitle.Location = new Point(350, currentYPosition + 10);
            currentYPosition += subTitle.Height + sectionSpacing;
            panel1.Controls.Add(subTitle);

            // Section: Our Story
            currentYPosition = AddSection(panel1, "Our Story", currentYPosition,
                @"EtCars was founded in 2010 with a simple vision: to revolutionize the car buying experience. 
What began as a single dealership in Detroit has grown into a nationally recognized automotive retailer with 15 locations across the country.

Our journey has been fueled by a passion for automobiles and a commitment to customer satisfaction. Over the years, we've consistently ranked among the top dealership groups in customer service awards, a testament to our team's dedication.");

            // Section: Our Mission
            currentYPosition = AddSection(panel1, "Our Mission", currentYPosition,
                @"At EtCars, we exist to make car ownership simple, transparent, and enjoyable. We achieve this by:

• Offering a carefully curated selection of quality vehicles
• Providing honest, pressure-free consultations
• Delivering exceptional after-sales support
• Maintaining transparent pricing with no hidden fees
• Investing in continuous staff training and development

We believe that buying a car should be as exciting as driving one, and we're committed to making that a reality for every customer.");

            // Section: Our Values
            currentYPosition = AddSection(panel1, "Our Core Values", currentYPosition,
                @"Integrity: We do what's right, even when no one is watching.
            
Excellence: We strive to exceed expectations in everything we do.

Innovation: We continuously seek better ways to serve our customers.

Community: We're committed to giving back to the communities we serve.

Sustainability: We promote eco-friendly practices in our operations.");

            // Close button (styled to match theme)
            Button closeButton = new Button();
            closeButton.Text = "Close Window";
            closeButton.Font = new Font("Arial", 12, FontStyle.Bold);
            closeButton.Size = new Size(150, 45);
            closeButton.Location = new Point(450, currentYPosition + 20);
            closeButton.BackColor = Color.FromArgb(0, 120, 215); // Matching blue
            closeButton.ForeColor = Color.White;
            closeButton.FlatStyle = FlatStyle.Flat;
            closeButton.FlatAppearance.BorderSize = 0;
            closeButton.Click += (sender, e) => this.Close();
            panel1.Controls.Add(closeButton);
        }

        private int AddSection(Panel parent, string title, int yPos, string content)
        {
            int sectionSpacing = 15;

            // Section Title (white text)
            Label sectionTitle = new Label();
            sectionTitle.Text = title;
            sectionTitle.Font = new Font("Arial", 20, FontStyle.Bold | FontStyle.Underline);
            sectionTitle.ForeColor = Color.White; 
            sectionTitle.AutoSize = true;
            sectionTitle.Location = new Point(50, yPos);
            parent.Controls.Add(sectionTitle);

            // Section Content (light gray text)
            Label sectionContent = new Label();
            sectionContent.Text = content;
            sectionContent.Font = new Font("Arial", 12);
            sectionContent.ForeColor = Color.LightGray;
            sectionContent.AutoSize = true;
            sectionContent.Location = new Point(50, yPos + sectionTitle.Height + sectionSpacing);
            sectionContent.MaximumSize = new Size(900, 0);
            parent.Controls.Add(sectionContent);

            return yPos + sectionTitle.Height + sectionSpacing + CalculateTextHeight(content, sectionContent.Font, 900) + 30;
        }

        private int CalculateTextHeight(string text, Font font, int maxWidth)
        {
            using (Graphics g = CreateGraphics())
            {
                SizeF size = g.MeasureString(text, font, maxWidth);
                return (int)Math.Ceiling(size.Height);
            }
        }

        private void about_Load(object sender, EventArgs e)
        {
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
        }
    }
}