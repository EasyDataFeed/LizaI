namespace Databox.Libs.Insidefabric
{
    partial class ucExtSettings
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.brandsListBox = new System.Windows.Forms.CheckedListBox();
            this.bsSett = new System.Windows.Forms.BindingSource(this.components);
            this.checkAllButton = new System.Windows.Forms.Button();
            this.unchekAllButton = new System.Windows.Forms.Button();
            this.categoryCombobox = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.bsSett)).BeginInit();
            this.SuspendLayout();
            // 
            // brandsListBox
            // 
            this.brandsListBox.BackColor = System.Drawing.Color.WhiteSmoke;
            this.brandsListBox.CheckOnClick = true;
            this.brandsListBox.FormattingEnabled = true;
            this.brandsListBox.Location = new System.Drawing.Point(36, 85);
            this.brandsListBox.Name = "brandsListBox";
            this.brandsListBox.Size = new System.Drawing.Size(175, 356);
            this.brandsListBox.TabIndex = 0;
            // 
            // bsSett
            // 
            this.bsSett.DataSource = typeof(Databox.Libs.Insidefabric.ExtSettings);
            // 
            // checkAllButton
            // 
            this.checkAllButton.Location = new System.Drawing.Point(36, 50);
            this.checkAllButton.Name = "checkAllButton";
            this.checkAllButton.Size = new System.Drawing.Size(84, 23);
            this.checkAllButton.TabIndex = 2;
            this.checkAllButton.Text = "Check all";
            this.checkAllButton.UseVisualStyleBackColor = true;
            this.checkAllButton.Click += new System.EventHandler(this.checkAllButton_Click);
            // 
            // unchekAllButton
            // 
            this.unchekAllButton.Location = new System.Drawing.Point(127, 50);
            this.unchekAllButton.Name = "unchekAllButton";
            this.unchekAllButton.Size = new System.Drawing.Size(84, 23);
            this.unchekAllButton.TabIndex = 3;
            this.unchekAllButton.Text = "Uncheck all";
            this.unchekAllButton.UseVisualStyleBackColor = true;
            this.unchekAllButton.Click += new System.EventHandler(this.unchekAllButton_Click);
            // 
            // categoryCombobox
            // 
            this.categoryCombobox.FormattingEnabled = true;
            this.categoryCombobox.Items.AddRange(new object[] {
            "Insidefabric",
            "Insidewallpapers"});
            this.categoryCombobox.Location = new System.Drawing.Point(36, 18);
            this.categoryCombobox.Name = "categoryCombobox";
            this.categoryCombobox.Size = new System.Drawing.Size(175, 21);
            this.categoryCombobox.TabIndex = 4;
            this.categoryCombobox.SelectedIndexChanged += new System.EventHandler(this.categoryCombobox_SelectedIndexChanged);
            // 
            // ucExtSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.categoryCombobox);
            this.Controls.Add(this.unchekAllButton);
            this.Controls.Add(this.checkAllButton);
            this.Controls.Add(this.brandsListBox);
            this.Name = "ucExtSettings";
            this.Size = new System.Drawing.Size(500, 466);
            this.Load += new System.EventHandler(this.ucExtSettings_Load);
            ((System.ComponentModel.ISupportInitialize)(this.bsSett)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.BindingSource bsSett;
        private System.Windows.Forms.CheckedListBox brandsListBox;
        private System.Windows.Forms.Button checkAllButton;
        private System.Windows.Forms.Button unchekAllButton;
        private System.Windows.Forms.ComboBox categoryCombobox;
    }
}
