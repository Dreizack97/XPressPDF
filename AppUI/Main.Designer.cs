namespace AppUI
{
    partial class Main
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            btnSelect = new Button();
            dataGrid = new DataGridView();
            btnConvert = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGrid).BeginInit();
            SuspendLayout();
            // 
            // btnSelect
            // 
            btnSelect.AutoSize = true;
            btnSelect.Location = new Point(12, 12);
            btnSelect.Name = "btnSelect";
            btnSelect.Size = new Size(99, 25);
            btnSelect.TabIndex = 0;
            btnSelect.Text = "Select XML files";
            btnSelect.UseVisualStyleBackColor = true;
            btnSelect.Click += btnSelect_Click;
            // 
            // dataGrid
            // 
            dataGrid.AllowUserToAddRows = false;
            dataGrid.AllowUserToDeleteRows = false;
            dataGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGrid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGrid.BorderStyle = BorderStyle.None;
            dataGrid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dataGrid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dataGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGrid.Location = new Point(12, 49);
            dataGrid.Name = "dataGrid";
            dataGrid.ReadOnly = true;
            dataGrid.RowHeadersVisible = false;
            dataGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGrid.Size = new Size(630, 450);
            dataGrid.TabIndex = 2;
            // 
            // btnConvert
            // 
            btnConvert.AutoSize = true;
            btnConvert.Location = new Point(130, 12);
            btnConvert.Name = "btnConvert";
            btnConvert.Size = new Size(99, 25);
            btnConvert.TabIndex = 1;
            btnConvert.Text = "Convert";
            btnConvert.UseVisualStyleBackColor = true;
            btnConvert.Click += btnConvert_Click;
            // 
            // Main
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(654, 511);
            Controls.Add(btnConvert);
            Controls.Add(dataGrid);
            Controls.Add(btnSelect);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "Main";
            Text = "XPressPDF";
            ((System.ComponentModel.ISupportInitialize)dataGrid).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnSelect;
        private DataGridView dataGrid;
        private Button btnConvert;
    }
}
