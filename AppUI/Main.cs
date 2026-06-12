using AppUI.Objects;
using BLL.Implementation;
using BLL.Objects;
using BLL.Utilities;
using System.Text;

namespace AppUI
{
    public partial class Main : Form
    {
        private List<XmlFile> xmlFiles = new List<XmlFile>();

        public Main()
        {
            InitializeComponent();
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    dataGrid.DataSource = null;
                    xmlFiles.Clear();

                    foreach (string file in openFileDialog.FileNames)
                    {
                        FileInfo fileInfo = new FileInfo(file);

                        xmlFiles.Add(new XmlFile
                        {
                            Name = fileInfo.Name,
                            Path = fileInfo.FullName,
                            Size = (fileInfo.Length / 1024) + " KB",
                            ModificationDate = fileInfo.LastWriteTime
                        });
                    }

                    dataGrid.DataSource = xmlFiles;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private async void btnConvert_Click(object sender, EventArgs e)
        {
            if (xmlFiles.Count == 0)
            {
                MessageBox.Show("You have not selected any files to convert. Please select at least one file to continue.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            btnConvert.Enabled = false;
            Cursor = Cursors.WaitCursor;

            try
            {
                XmlDeserializer xmlDeserializer = new XmlDeserializer();
                XmlReaderService xmlReader = new XmlReaderService(new ComplementService(xmlDeserializer), xmlDeserializer);

                for (int i = 0; i < xmlFiles.Count; i++)
                {
                    string filePath = xmlFiles[i].Path;

                    try
                    {
                        await Task.Run(() => xmlReader.Read(filePath));
                        dataGrid.Rows[i].Cells[4].Value = "Complete";
                    }
                    catch (Exception ex)
                    {
                        dataGrid.Rows[i].Cells[4].Value = ex.Message;
                    }
                }
            }
            finally
            {
                btnConvert.Enabled = true;
                Cursor = Cursors.Default;
            }
        }

        private async void btnUpload_Click(object sender, EventArgs e)
        {
            btnUpload.Enabled = false;
            Cursor = Cursors.WaitCursor;

            string? directoryPath = Path.GetDirectoryName(openFileDialog.FileName);

            if (string.IsNullOrWhiteSpace(directoryPath))
                throw new DirectoryNotFoundException("Directory not found.");

            string uploadPath = Path.Combine(DateTime.Now.Year.ToString(), "S", "1");
            string[] pdfFiles = Directory.GetFiles(directoryPath, "*.pdf");

            try
            {
                FtpService ftpService = new FtpService(new ConfigManager(), new LogManager());
                List<FileUploadResult> results = await ftpService.UploadAsync(pdfFiles, uploadPath);

                int successCount = results.Count(r => r.Success);
                int failedCount = results.Count(r => !r.Success);

                string logFileName = $"Log-{DateTime.Now:yyyyMMdd}.txt";
                string fullLogFilePath = Path.Combine(AppContext.BaseDirectory, "Logs", logFileName);

                StringBuilder messageBuilder = new StringBuilder();
                messageBuilder.AppendLine("FTP Upload Results:");
                messageBuilder.AppendLine($"✅ Files Uploaded: {successCount}");
                messageBuilder.AppendLine($"❌ Files Failed: {failedCount}");
                messageBuilder.AppendLine($"Check the log file for more details: {fullLogFilePath}");

                MessageBox.Show(messageBuilder.ToString(), "FTP Upload Summary", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnUpload.Enabled = true;
                Cursor = Cursors.Default;
            }
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            Settings settings = new Settings();
            settings.ShowDialog();
        }
    }
}
