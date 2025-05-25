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
            this.BackColor = Color.White;

            SetupControls();
        }

        private void SetupControls()
        {
            // Main panel
            panel1.BackColor = Color.WhiteSmoke;
            panel1.Dock = DockStyle.Fill;
            panel1.AutoScroll = true;
            
            

            // Adjust spacing parameters
            int currentYPosition = 30; // Start position
            int sectionSpacing = 30; // Space between sections
            int titleContentSpacing = 15; // Space between title and content

            // Company logo
            PictureBox logo = new PictureBox();
            logo.Size = new Size(500, 400);
            logo.Location = new Point(350, currentYPosition);
            currentYPosition += logo.Height + sectionSpacing;
            logo.SizeMode = PictureBoxSizeMode.Zoom;
            logo.Image = Image.FromFile("asset\\bg\\logo.jpg");
            logo.BackColor = Color.Transparent;
            panel1.Controls.Add(logo);

            // Main title
            Label mainTitle = new Label();
            mainTitle.Text = "Welcome to EtCars Automotive";
            mainTitle.Font = new Font("Arial", 28, FontStyle.Bold);
            mainTitle.ForeColor = Color.FromArgb(0, 51, 102);
            mainTitle.AutoSize = true;
            mainTitle.Location = new Point(200, currentYPosition-30);
            currentYPosition += mainTitle.Height + titleContentSpacing;
            panel1.Controls.Add(mainTitle);

            // Subtitle
            Label subTitle = new Label();
            subTitle.Text = "Driving Excellence Since 2010";
            subTitle.Font = new Font("Arial", 16, FontStyle.Italic);
            subTitle.ForeColor = Color.FromArgb(102, 102, 102);
            subTitle.AutoSize = true;
            subTitle.Location = new Point(350, currentYPosition+10);
            currentYPosition += subTitle.Height + sectionSpacing;
            panel1.Controls.Add(subTitle);

            // Section: Our Story
            currentYPosition = AddSection(panel1, "Our Story", currentYPosition,
                @"EtCars was founded in 2010 with a simple vision: to revolutionize the car buying experience. 
What began as a single dealership in Detroit has grown into a nationally recognized automotive retailer with 15 locations across the country.

Our journey has been fueled by a passion for automobiles and a commitment to customer satisfaction. Over the years, we've consistently ranked among the top dealership groups in customer service awards, a testament to our team's dedication.");

            // Section: Our Mission (positioned higher)
            currentYPosition = AddSection(panel1, "Our Mission", currentYPosition,
                @"At EtCars, we exist to make car ownership simple, transparent, and enjoyable. We achieve this by:

• Offering a carefully curated selection of quality vehicles
• Providing honest, pressure-free consultations
• Delivering exceptional after-sales support
• Maintaining transparent pricing with no hidden fees
• Investing in continuous staff training and development

We believe that buying a car should be as exciting as driving one, and we're committed to making that a reality for every customer.");

            // Section: Our Values (positioned higher)
            currentYPosition = AddSection(panel1, "Our Core Values", currentYPosition,
                @"Integrity: We do what's right, even when no one is watching.
            
Excellence: We strive to exceed expectations in everything we do.

Innovation: We continuously seek better ways to serve our customers.

Community: We're committed to giving back to the communities we serve.

Sustainability: We promote eco-friendly practices in our operations.");

            // Close button (positioned with proper spacing)
            Button closeButton = new Button();
            closeButton.Text = "Close Window";
            closeButton.Font = new Font("Arial", 12, FontStyle.Bold);
            closeButton.Size = new Size(150, 45);
            closeButton.Location = new Point(450, currentYPosition + 20);
            closeButton.BackColor = Color.FromArgb(0, 51, 102);
            closeButton.ForeColor = Color.White;
            closeButton.Click += (sender, e) => this.Close();
            panel1.Controls.Add(closeButton);
        }

        private int AddSection(Panel parent, string title, int yPos, string content)
        {
            int sectionSpacing = 15;

            // Section Title
            Label sectionTitle = new Label();
            sectionTitle.Text = title;
            sectionTitle.Font = new Font("Arial", 20, FontStyle.Bold | FontStyle.Underline);
            sectionTitle.ForeColor = Color.FromArgb(0, 51, 102);
            sectionTitle.AutoSize = true;
            sectionTitle.Location = new Point(50, yPos);
            parent.Controls.Add(sectionTitle);

            // Section Content
            Label sectionContent = new Label();
            sectionContent.Text = content;
            sectionContent.Font = new Font("Arial", 12);
            sectionContent.ForeColor = Color.DarkSlateGray;
            sectionContent.AutoSize = true;
            sectionContent.Location = new Point(50, yPos + sectionTitle.Height + sectionSpacing);
            sectionContent.MaximumSize = new Size(900, 0);
            parent.Controls.Add(sectionContent);

            // Return the new Y position after this section
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
            // Any additional load operations can go here
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            // Custom painting if needed
        }
    }
}