﻿namespace AppUI
{
    partial class Settings
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Settings));
            menuStrip = new MenuStrip();
            ftpServerToolStripMenuItem = new ToolStripMenuItem();
            emailServertoolStripMenuItem = new ToolStripMenuItem();
            panelContainer = new Panel();
            menuStrip.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip
            // 
            menuStrip.Items.AddRange(new ToolStripItem[] { ftpServerToolStripMenuItem, emailServertoolStripMenuItem });
            menuStrip.Location = new Point(0, 0);
            menuStrip.Name = "menuStrip";
            menuStrip.Size = new Size(484, 24);
            menuStrip.TabIndex = 0;
            menuStrip.Text = "menuStrip";
            // 
            // ftpServerToolStripMenuItem
            // 
            ftpServerToolStripMenuItem.Name = "ftpServerToolStripMenuItem";
            ftpServerToolStripMenuItem.Size = new Size(74, 20);
            ftpServerToolStripMenuItem.Text = "FTP Server";
            ftpServerToolStripMenuItem.Click += ftpServerToolStripMenuItem_Click;
            // 
            // emailServertoolStripMenuItem
            // 
            emailServertoolStripMenuItem.Name = "emailServertoolStripMenuItem";
            emailServertoolStripMenuItem.Size = new Size(83, 20);
            emailServertoolStripMenuItem.Text = "Email Server";
            emailServertoolStripMenuItem.Click += emailServertoolStripMenuItem_Click;
            // 
            // panelContainer
            // 
            panelContainer.Dock = DockStyle.Fill;
            panelContainer.Location = new Point(0, 24);
            panelContainer.Name = "panelContainer";
            panelContainer.Size = new Size(484, 387);
            panelContainer.TabIndex = 1;
            // 
            // Settings
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(484, 411);
            Controls.Add(panelContainer);
            Controls.Add(menuStrip);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = menuStrip;
            MaximizeBox = false;
            Name = "Settings";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Settings";
            Load += Settings_Load;
            menuStrip.ResumeLayout(false);
            menuStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip;
        private ToolStripMenuItem ftpServerToolStripMenuItem;
        private ToolStripMenuItem emailServertoolStripMenuItem;
        private Panel panelContainer;
    }
}