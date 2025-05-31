using System;
using System.Drawing;
using System.Windows.Forms;

namespace carstore
{
    public partial class privacy : Form
    {
        public privacy()
        {
            InitializeComponent();
            this.Text = "EtCars - Privacy Policy";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(1150, 1000);
            this.BackColor = Color.FromArgb(0, 40, 85); // Dark blue background

            SetupControls();
        }

        private void SetupControls()
        {
            // Main panel with dark blue background
            panel1.BackColor = Color.FromArgb(0, 40, 85);
            panel1.Dock = DockStyle.Fill;
            panel1.AutoScroll = true;

            // Adjust spacing parameters
            int currentYPosition = 30; // Start position
            int sectionSpacing = 30; // Space between sections
            int titleContentSpacing = 15; // Space between title and content

            // Privacy Policy title
            Label mainTitle = new Label();
            mainTitle.Text = "EtCars Privacy Policy";
            mainTitle.Font = new Font("Arial", 28, FontStyle.Bold);
            mainTitle.ForeColor = Color.White;
            mainTitle.AutoSize = true;
            mainTitle.Location = new Point(350, currentYPosition);
            currentYPosition += mainTitle.Height + titleContentSpacing;
            panel1.Controls.Add(mainTitle);

            // Last updated date
            Label updateDate = new Label();
            updateDate.Text = "Last Updated: January 2024";
            updateDate.Font = new Font("Arial", 12, FontStyle.Italic);
            updateDate.ForeColor = Color.LightGray;
            updateDate.AutoSize = true;
            updateDate.Location = new Point(400, currentYPosition+20);
            currentYPosition += updateDate.Height + sectionSpacing;
            panel1.Controls.Add(updateDate);

            // Introduction section
            currentYPosition = AddSection(panel1, "Introduction", currentYPosition,
                @"At EtCars Automotive, we are committed to protecting your privacy. This Privacy Policy explains how we collect, use, disclose, and safeguard your information when you visit our dealerships, use our website, or interact with our services. Please read this policy carefully.");

            // Information Collection section
            currentYPosition = AddSection(panel1, "Information We Collect", currentYPosition,
                @"We may collect personal information that you voluntarily provide to us, including:

• Contact Information: Name, email address, phone number
• Demographic Information: Age, gender, location
• Financial Information: Credit score, income details (for financing)
• Vehicle Preferences: Makes, models, features of interest
• Identification Documents: Driver's license (for test drives)
• Transaction History: Purchases, services, warranties

We also automatically collect certain technical information when you visit our website, such as IP address, browser type, and browsing behavior.");

            // How We Use Information section
            currentYPosition = AddSection(panel1, "How We Use Your Information", currentYPosition,
                @"We use the information we collect for various purposes, including:

• To provide and maintain our services
• To process transactions and deliver vehicles
• To notify you about special offers and promotions
• To improve customer service and personalize your experience
• To conduct credit checks and process financing applications
• To comply with legal obligations and prevent fraud
• To analyze website usage and improve our online presence

We will never sell your personal information to third parties for marketing purposes.");

            // Data Sharing section
            currentYPosition = AddSection(panel1, "Data Sharing and Disclosure", currentYPosition,
                @"We may share your information in the following circumstances:

• With authorized dealership personnel to assist with your purchase
• With financial institutions for credit applications
• With vehicle manufacturers for warranty and recall purposes
• With service providers who assist with our operations (under confidentiality agreements)
• When required by law or to protect our legal rights

We require all third parties to respect the security of your personal data and to treat it in accordance with the law.");

            // Data Security section
            currentYPosition = AddSection(panel1, "Data Security", currentYPosition,
                @"We implement appropriate technical and organizational measures to protect your personal data, including:

• Secure servers with encryption
• Restricted access to personal information
• Regular security audits and updates
• Employee training on data protection

While we strive to protect your information, no electronic transmission or storage is 100% secure. We cannot guarantee absolute security.");

            // Your Rights section
            currentYPosition = AddSection(panel1, "Your Privacy Rights", currentYPosition,
                @"Depending on your location, you may have certain rights regarding your personal information:

• Right to access the personal data we hold about you
• Right to request correction of inaccurate data
• Right to request deletion of your personal data
• Right to object to or restrict certain processing
• Right to data portability (where applicable)
• Right to withdraw consent (where processing is based on consent)

To exercise these rights, please contact our Data Protection Officer at privacy@etcars.com.");

           

            // Policy Changes section
            currentYPosition = AddSection(panel1, "Changes to This Policy", currentYPosition,
                @"We may update this Privacy Policy periodically. The updated version will be indicated by a new 'Last Updated' date and will be effective immediately upon posting. We encourage you to review this policy regularly to stay informed about how we protect your information.");

            

            // Close button (styled to match theme)
            Button closeButton = new Button();
            closeButton.Text = "Close Window";
            closeButton.Font = new Font("Arial", 12, FontStyle.Bold);
            closeButton.Size = new Size(150, 45);
            closeButton.Location = new Point(450, currentYPosition + 10);
            closeButton.BackColor = Color.FromArgb(0, 120, 215);
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

        private void privacy_Load(object sender, EventArgs e)
        {
            // Any additional load operations can go here
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            // Custom painting if needed
        }
    }
}