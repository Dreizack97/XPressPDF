namespace AppUI.Components
{
    partial class EmailServer
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
            btnTest = new Button();
            btnSave = new Button();
            txtPort = new TextBox();
            txtHost = new TextBox();
            txtDisplayName = new TextBox();
            txtAddress = new TextBox();
            label4 = new Label();
            label3 = new Label();
            label2 = new Label();
            label1 = new Label();
            txtPassword = new TextBox();
            label6 = new Label();
            checkBoxSSL = new CheckBox();
            SuspendLayout();
            // 
            // btnTest
            // 
            btnTest.AutoSize = true;
            btnTest.Location = new Point(146, 190);
            btnTest.Name = "btnTest";
            btnTest.Size = new Size(103, 25);
            btnTest.TabIndex = 12;
            btnTest.Text = "Test Connection";
            btnTest.UseVisualStyleBackColor = true;
            btnTest.Click += btnTest_Click;
            // 
            // btnSave
            // 
            btnSave.AutoSize = true;
            btnSave.Location = new Point(13, 190);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(90, 25);
            btnSave.TabIndex = 11;
            btnSave.Text = "Save Changes";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // txtPort
            // 
            txtPort.Location = new Point(13, 148);
            txtPort.Name = "txtPort";
            txtPort.Size = new Size(200, 23);
            txtPort.TabIndex = 9;
            txtPort.Tag = "Please enter a valid port number.";
            // 
            // txtHost
            // 
            txtHost.Location = new Point(253, 93);
            txtHost.Name = "txtHost";
            txtHost.Size = new Size(200, 23);
            txtHost.TabIndex = 7;
            txtHost.Tag = "Please enter the email server host.";
            // 
            // txtDisplayName
            // 
            txtDisplayName.Location = new Point(13, 93);
            txtDisplayName.Name = "txtDisplayName";
            txtDisplayName.Size = new Size(200, 23);
            txtDisplayName.TabIndex = 6;
            txtDisplayName.Tag = "Please enter the display name.";
            // 
            // txtAddress
            // 
            txtAddress.Location = new Point(13, 38);
            txtAddress.Name = "txtAddress";
            txtAddress.Size = new Size(200, 23);
            txtAddress.TabIndex = 2;
            txtAddress.Tag = "Please enter the address.";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(13, 130);
            label4.Name = "label4";
            label4.Size = new Size(29, 15);
            label4.TabIndex = 8;
            label4.Text = "Port";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(253, 75);
            label3.Name = "label3";
            label3.Size = new Size(32, 15);
            label3.TabIndex = 5;
            label3.Text = "Host";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(13, 75);
            label2.Name = "label2";
            label2.Size = new Size(80, 15);
            label2.TabIndex = 4;
            label2.Text = "Display Name";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(13, 20);
            label1.Name = "label1";
            label1.Size = new Size(49, 15);
            label1.TabIndex = 0;
            label1.Text = "Address";
            // 
            // txtPassword
            // 
            txtPassword.Location = new Point(253, 38);
            txtPassword.Name = "txtPassword";
            txtPassword.Size = new Size(200, 23);
            txtPassword.TabIndex = 3;
            txtPassword.Tag = "Please enter the password.";
            txtPassword.UseSystemPasswordChar = true;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(253, 20);
            label6.Name = "label6";
            label6.Size = new Size(57, 15);
            label6.TabIndex = 1;
            label6.Text = "Password";
            // 
            // checkBoxSSL
            // 
            checkBoxSSL.AutoSize = true;
            checkBoxSSL.Location = new Point(253, 150);
            checkBoxSSL.Name = "checkBoxSSL";
            checkBoxSSL.Size = new Size(66, 19);
            checkBoxSSL.TabIndex = 10;
            checkBoxSSL.Text = "Use SSL";
            checkBoxSSL.UseVisualStyleBackColor = true;
            // 
            // EmailServer
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(checkBoxSSL);
            Controls.Add(txtPassword);
            Controls.Add(label6);
            Controls.Add(btnTest);
            Controls.Add(btnSave);
            Controls.Add(txtPort);
            Controls.Add(txtHost);
            Controls.Add(txtDisplayName);
            Controls.Add(txtAddress);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Name = "EmailServer";
            Size = new Size(484, 387);
            Load += EmailServer_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnTest;
        private Button btnSave;
        private TextBox txtPort;
        private TextBox txtHost;
        private TextBox txtDisplayName;
        private TextBox txtAddress;
        private Label label4;
        private Label label3;
        private Label label2;
        private Label label1;
        private TextBox txtPassword;
        private Label label6;
        private CheckBox checkBoxSSL;
    }
}
