using BLL.Objects;
using BLL.Utilities;

namespace AppUI.Components
{
    public partial class FtpServer : UserControl
    {
        private AppConfig config = new AppConfig();

        public FtpServer()
        {
            InitializeComponent();
        }

        private void FtpServer_Load(object sender, EventArgs e)
        {
            config = ConfigManager.LoadConfig();

            txtHost.Text = config.FtpServer.Host;
            txtUser.Text = config.FtpServer.User;
            txtPassword.Text = config.FtpServer.Password;
            txtPort.Text = config.FtpServer.Port.ToString();
            txtRootPath.Text = config.FtpServer.RootPath;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Dispose();
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

            config.FtpServer.Host = txtHost.Text;
            config.FtpServer.User = txtUser.Text;
            config.FtpServer.Password = txtPassword.Text;
            config.FtpServer.Port = port;
            config.FtpServer.RootPath = txtRootPath.Text;

            ConfigManager.SaveConfig(config);

            MessageBox.Show("Settings have been saved successfully.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private async void btnTest_Click(object sender, EventArgs e)
        {
            FtpService ftpService = new FtpService();

            try
            {
                if (await ftpService.Connect())
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
