using AppUI.Components;
using AppUI.Utilities;

namespace AppUI
{
    public partial class Settings : Form
    {
        public Settings()
        {
            InitializeComponent();
        }

        private void Settings_Load(object sender, EventArgs e)
        {
            string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.json");

            if (!File.Exists(configPath))
                ConfigManager.CreateDefaultConfig(configPath);
        }

        private void ftpServerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFormOnPanel<FtpServer>();
        }

        private void emailServertoolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void openFormOnPanel<Component>() where Component : UserControl, new()
        {
            Component? userControl = panelContainer.Controls.OfType<Component>().FirstOrDefault();

            if (userControl == null)
            {
                userControl = new Component();
                panelContainer.SuspendLayout();
                userControl.Dock = DockStyle.Fill;
                panelContainer.Controls.Add(userControl);
                panelContainer.Tag = userControl;
                userControl.BringToFront();
                panelContainer.ResumeLayout();
            }
            else
                userControl.BringToFront();
        }
    }
}
