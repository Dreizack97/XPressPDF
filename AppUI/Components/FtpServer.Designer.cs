namespace AppUI.Components
{
    partial class FtpServer
    {
        /// <summary> 
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de componentes

        /// <summary> 
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            txtHost = new TextBox();
            txtUser = new TextBox();
            txtPassword = new TextBox();
            txtPort = new TextBox();
            txtRootPath = new TextBox();
            btnSave = new Button();
            btnTest = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(13, 20);
            label1.Name = "label1";
            label1.Size = new Size(32, 15);
            label1.TabIndex = 0;
            label1.Text = "Host";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(13, 75);
            label2.Name = "label2";
            label2.Size = new Size(30, 15);
            label2.TabIndex = 2;
            label2.Text = "User";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(253, 75);
            label3.Name = "label3";
            label3.Size = new Size(57, 15);
            label3.TabIndex = 3;
            label3.Text = "Password";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(13, 130);
            label4.Name = "label4";
            label4.Size = new Size(29, 15);
            label4.TabIndex = 6;
            label4.Text = "Port";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(253, 130);
            label5.Name = "label5";
            label5.Size = new Size(59, 15);
            label5.TabIndex = 7;
            label5.Text = "Root Path";
            // 
            // txtHost
            // 
            txtHost.Location = new Point(13, 38);
            txtHost.Name = "txtHost";
            txtHost.Size = new Size(200, 23);
            txtHost.TabIndex = 1;
            txtHost.Tag = "Please enter the FTP server host.";
            // 
            // txtUser
            // 
            txtUser.Location = new Point(13, 93);
            txtUser.Name = "txtUser";
            txtUser.Size = new Size(200, 23);
            txtUser.TabIndex = 4;
            txtUser.Tag = "Please enter the FTP username.";
            // 
            // txtPassword
            // 
            txtPassword.Location = new Point(253, 93);
            txtPassword.Name = "txtPassword";
            txtPassword.Size = new Size(200, 23);
            txtPassword.TabIndex = 5;
            txtPassword.Tag = "Please enter the FTP password.";
            txtPassword.UseSystemPasswordChar = true;
            // 
            // txtPort
            // 
            txtPort.Location = new Point(13, 148);
            txtPort.Name = "txtPort";
            txtPort.Size = new Size(200, 23);
            txtPort.TabIndex = 8;
            txtPort.Tag = "Please enter a valid FTP port number.";
            // 
            // txtRootPath
            // 
            txtRootPath.Location = new Point(253, 148);
            txtRootPath.Name = "txtRootPath";
            txtRootPath.Size = new Size(200, 23);
            txtRootPath.TabIndex = 9;
            txtRootPath.Tag = "Please enter the FTP root path.";
            // 
            // btnSave
            // 
            btnSave.AutoSize = true;
            btnSave.Location = new Point(13, 190);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(90, 25);
            btnSave.TabIndex = 10;
            btnSave.Text = "Save Changes";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // btnTest
            // 
            btnTest.AutoSize = true;
            btnTest.Location = new Point(146, 190);
            btnTest.Name = "btnTest";
            btnTest.Size = new Size(103, 25);
            btnTest.TabIndex = 11;
            btnTest.Text = "Test Connection";
            btnTest.UseVisualStyleBackColor = true;
            btnTest.Click += btnTest_Click;
            // 
            // FtpServer
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(btnTest);
            Controls.Add(btnSave);
            Controls.Add(txtRootPath);
            Controls.Add(txtPort);
            Controls.Add(txtPassword);
            Controls.Add(txtUser);
            Controls.Add(txtHost);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Name = "FtpServer";
            Size = new Size(484, 387);
            Load += FtpServer_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private TextBox txtHost;
        private TextBox txtUser;
        private TextBox txtPassword;
        private TextBox txtPort;
        private TextBox txtRootPath;
        private Button btnSave;
        private Button btnTest;
    }
}
