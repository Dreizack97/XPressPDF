using AppUI.Objetcs;
using BLL;

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

            int i = 0;
            btnConvert.Enabled = false;

            try
            {
                await Task.Run(async () =>
                {
                    XmlReader xmlReader = new XmlReader();

                    foreach (XmlFile xmlFile in xmlFiles)
                    {
                        await xmlReader.Read(xmlFile.Path);

                        dataGrid.Rows[i].Cells[4].Value = "Complete";
                        i++;
                    }
                });
            }
            catch (Exception ex)
            {
                dataGrid.Rows[i].Cells[4].Value = ex.Message;
            }
            finally
            {
                btnConvert.Enabled = true;
            }
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {

        }
    }
}
