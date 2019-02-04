namespace LogAnalyzer
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.openFileButton = new System.Windows.Forms.Button();
            this.getErrorsButton = new System.Windows.Forms.Button();
            this.sfDataGrid = new Syncfusion.WinForms.DataGrid.SfDataGrid();
            this.refreshButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sfDataGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.refreshButton);
            this.splitContainer1.Panel1.Controls.Add(this.getErrorsButton);
            this.splitContainer1.Panel1.Controls.Add(this.openFileButton);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.sfDataGrid);
            this.splitContainer1.Size = new System.Drawing.Size(990, 654);
            this.splitContainer1.SplitterDistance = 65;
            this.splitContainer1.TabIndex = 1;
            // 
            // openFileButton
            // 
            this.openFileButton.Location = new System.Drawing.Point(12, 12);
            this.openFileButton.Name = "openFileButton";
            this.openFileButton.Size = new System.Drawing.Size(75, 23);
            this.openFileButton.TabIndex = 0;
            this.openFileButton.Text = "Open file";
            this.openFileButton.UseVisualStyleBackColor = true;
            this.openFileButton.Click += new System.EventHandler(this.openFileButton_Click);
            // 
            // getErrorsButton
            // 
            this.getErrorsButton.Enabled = false;
            this.getErrorsButton.Location = new System.Drawing.Point(112, 12);
            this.getErrorsButton.Name = "getErrorsButton";
            this.getErrorsButton.Size = new System.Drawing.Size(75, 23);
            this.getErrorsButton.TabIndex = 4;
            this.getErrorsButton.Text = "Get errors";
            this.getErrorsButton.UseVisualStyleBackColor = true;
            this.getErrorsButton.Click += new System.EventHandler(this.getErrorsButton_Click);
            // 
            // sfDataGrid
            // 
            this.sfDataGrid.AccessibleName = "Table";
            this.sfDataGrid.AllowDraggingColumns = true;
            this.sfDataGrid.AllowFiltering = true;
            this.sfDataGrid.AllowResizingColumns = true;
            this.sfDataGrid.AllowResizingHiddenColumns = true;
            this.sfDataGrid.AllowSorting = false;
            this.sfDataGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sfDataGrid.FilterPopupMode = Syncfusion.WinForms.GridCommon.FilterPopupMode.AdvancedFilter;
            this.sfDataGrid.Location = new System.Drawing.Point(0, 0);
            this.sfDataGrid.Name = "sfDataGrid";
            this.sfDataGrid.Size = new System.Drawing.Size(990, 585);
            this.sfDataGrid.TabIndex = 0;
            this.sfDataGrid.Text = "sfDataGrid1";
            // 
            // refreshButton
            // 
            this.refreshButton.Enabled = false;
            this.refreshButton.Location = new System.Drawing.Point(210, 12);
            this.refreshButton.Name = "refreshButton";
            this.refreshButton.Size = new System.Drawing.Size(75, 23);
            this.refreshButton.TabIndex = 5;
            this.refreshButton.Text = "Refresh";
            this.refreshButton.UseVisualStyleBackColor = true;
            this.refreshButton.Click += new System.EventHandler(this.refreshButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(990, 654);
            this.Controls.Add(this.splitContainer1);
            this.Name = "Form1";
            this.Text = "LogAnalyzer";
            this.SizeChanged += new System.EventHandler(this.Form1_SizeChanged);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.sfDataGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button openFileButton;
        private System.Windows.Forms.Button getErrorsButton;
        private Syncfusion.WinForms.DataGrid.SfDataGrid sfDataGrid;
        private System.Windows.Forms.Button refreshButton;
    }
}

