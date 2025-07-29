using BLL.Implementation;
using BLL.Objects;
using BLL.Utilities;

namespace AppUI.Components
{
    public partial class EmailServer : UserControl
    {
        private AppConfig config = new AppConfig();

        public EmailServer()
        {
            InitializeComponent();
        }

        private void EmailServer_Load(object sender, EventArgs e)
        {
            config = ConfigManager.LoadConfig();

            txtAddress.Text = config.MailServer.Address;
            txtPassword.Text = config.MailServer.Password;
            txtDisplayName.Text = config.MailServer.DisplayName;
            txtHost.Text = config.MailServer.Host;
            txtPort.Text = config.MailServer.Port.ToString();
            checkBoxSSL.Checked = config.MailServer.SSL;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            IEnumerable<TextBox> textBoxes = Controls.OfType<TextBox>().OrderBy(x => x.TabIndex);

            foreach (TextBox textBox in textBoxes)
            {
                if (string.IsNullOrWhiteSpace(textBox.Text))
                {
                    MessageBox.Show(textBox.Tag?.ToString(), "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    textBox.Focus();
                    return;
                }
            }

            if (!int.TryParse(txtPort.Text, out int port))
            {
                MessageBox.Show("The port number entered is invalid. Please enter a valid port number.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            config.MailServer.Address = txtAddress.Text;
            config.MailServer.Password = txtPassword.Text;
            config.MailServer.DisplayName = txtDisplayName.Text;
            config.MailServer.Host = txtHost.Text;
            config.MailServer.Port = port;
            config.MailServer.SSL = checkBoxSSL.Checked;

            ConfigManager.SaveConfig(config);

            MessageBox.Show("Settings have been saved successfully.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private async void btnTest_Click(object sender, EventArgs e)
        {
            EmailService emailService = new EmailService();

            try
            {
                if (await emailService.ConnectAsync())
                {
                    MessageBox.Show("Connection successfully.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
