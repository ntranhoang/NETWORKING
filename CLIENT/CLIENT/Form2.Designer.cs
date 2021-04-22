
namespace CLIENT
{
    partial class Form2
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
            this.listView = new System.Windows.Forms.ListView();
            this.headerId = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.headerName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.headerAuthor = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.headerCategory = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnSearch = new System.Windows.Forms.Button();
            this.typeSearchBox = new System.Windows.Forms.ComboBox();
            this.insertSearch = new System.Windows.Forms.TextBox();
            this.btnDownload = new System.Windows.Forms.Button();
            this.btnRead = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listView
            // 
            this.listView.CheckBoxes = true;
            this.listView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.headerId,
            this.headerName,
            this.headerAuthor,
            this.headerCategory});
            this.listView.FullRowSelect = true;
            this.listView.GridLines = true;
            this.listView.HideSelection = false;
            this.listView.Location = new System.Drawing.Point(12, 128);
            this.listView.MultiSelect = false;
            this.listView.Name = "listView";
            this.listView.Size = new System.Drawing.Size(757, 294);
            this.listView.TabIndex = 0;
            this.listView.UseCompatibleStateImageBehavior = false;
            this.listView.View = System.Windows.Forms.View.Details;
            // 
            // headerId
            // 
            this.headerId.Text = "ID";
            this.headerId.Width = 127;
            // 
            // headerName
            // 
            this.headerName.Text = "NAME";
            this.headerName.Width = 123;
            // 
            // headerAuthor
            // 
            this.headerAuthor.Text = "AUTHOR";
            this.headerAuthor.Width = 190;
            // 
            // headerCategory
            // 
            this.headerCategory.Text = "CATEGORY";
            this.headerCategory.Width = 142;
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(650, 12);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(119, 23);
            this.btnSearch.TabIndex = 2;
            this.btnSearch.Text = "SEARCH";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // typeSearchBox
            // 
            this.typeSearchBox.FormattingEnabled = true;
            this.typeSearchBox.Items.AddRange(new object[] {
            "all",
            "name",
            "id",
            "author",
            "category"});
            this.typeSearchBox.Location = new System.Drawing.Point(507, 12);
            this.typeSearchBox.Name = "typeSearchBox";
            this.typeSearchBox.Size = new System.Drawing.Size(121, 24);
            this.typeSearchBox.TabIndex = 1;
            this.typeSearchBox.SelectedIndexChanged += new System.EventHandler(this.typeSearchBox_SelectedIndexChanged);
            // 
            // insertSearch
            // 
            this.insertSearch.Location = new System.Drawing.Point(367, 12);
            this.insertSearch.Name = "insertSearch";
            this.insertSearch.Size = new System.Drawing.Size(100, 22);
            this.insertSearch.TabIndex = 0;
            this.insertSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.insertSearch_KeyDown);
            // 
            // btnDownload
            // 
            this.btnDownload.Location = new System.Drawing.Point(149, 99);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(144, 23);
            this.btnDownload.TabIndex = 4;
            this.btnDownload.Text = "DOWNLOAD";
            this.btnDownload.UseVisualStyleBackColor = true;
            this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click);
            // 
            // btnRead
            // 
            this.btnRead.Location = new System.Drawing.Point(11, 99);
            this.btnRead.Name = "btnRead";
            this.btnRead.Size = new System.Drawing.Size(121, 23);
            this.btnRead.TabIndex = 3;
            this.btnRead.Text = "READ";
            this.btnRead.UseVisualStyleBackColor = true;
            this.btnRead.Click += new System.EventHandler(this.btnRead_Click);
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnRead);
            this.Controls.Add(this.btnDownload);
            this.Controls.Add(this.insertSearch);
            this.Controls.Add(this.typeSearchBox);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.listView);
            this.Name = "Form2";
            this.Text = "USER";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView listView;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.ComboBox typeSearchBox;
        private System.Windows.Forms.TextBox insertSearch;
        private System.Windows.Forms.Button btnDownload;
        private System.Windows.Forms.ColumnHeader headerId;
        private System.Windows.Forms.ColumnHeader headerName;
        private System.Windows.Forms.ColumnHeader headerAuthor;
        private System.Windows.Forms.ColumnHeader headerCategory;
        private System.Windows.Forms.Button btnRead;
    }
}