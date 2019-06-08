namespace Databox.Libs.EdgeInfo
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
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.buttonEditSupplierFileSkip = new DevExpress.XtraEditors.ButtonEdit();
            this.bsSett = new System.Windows.Forms.BindingSource();
            this.textEditFTPHost = new DevExpress.XtraEditors.TextEdit();
            this.textEditFTPPassword = new DevExpress.XtraEditors.TextEdit();
            this.textEditFTPLogin = new DevExpress.XtraEditors.TextEdit();
            this.buttonEditSupplierFilePath = new DevExpress.XtraEditors.ButtonEdit();
            this.checkEditDoBatch = new DevExpress.XtraEditors.CheckEdit();
            this.buttonEditSupplierZeroItems = new DevExpress.XtraEditors.ButtonEdit();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem5 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem6 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem7 = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.buttonEditSupplierFileSkip.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsSett)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditFTPHost.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditFTPPassword.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditFTPLogin.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.buttonEditSupplierFilePath.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEditDoBatch.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.buttonEditSupplierZeroItems.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem7)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this.buttonEditSupplierFileSkip);
            this.layoutControl1.Controls.Add(this.textEditFTPHost);
            this.layoutControl1.Controls.Add(this.textEditFTPPassword);
            this.layoutControl1.Controls.Add(this.textEditFTPLogin);
            this.layoutControl1.Controls.Add(this.buttonEditSupplierFilePath);
            this.layoutControl1.Controls.Add(this.checkEditDoBatch);
            this.layoutControl1.Controls.Add(this.buttonEditSupplierZeroItems);
            this.layoutControl1.Location = new System.Drawing.Point(10, 10);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.Root = this.layoutControlGroup1;
            this.layoutControl1.Size = new System.Drawing.Size(529, 315);
            this.layoutControl1.TabIndex = 0;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // buttonEditSupplierFileSkip
            // 
            this.buttonEditSupplierFileSkip.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.bsSett, "SupplierFileSkip", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.buttonEditSupplierFileSkip.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bsSett, "SupplierFileSkip", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.buttonEditSupplierFileSkip.Location = new System.Drawing.Point(111, 108);
            this.buttonEditSupplierFileSkip.Name = "buttonEditSupplierFileSkip";
            this.buttonEditSupplierFileSkip.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.buttonEditSupplierFileSkip.Size = new System.Drawing.Size(406, 20);
            this.buttonEditSupplierFileSkip.StyleController = this.layoutControl1;
            this.buttonEditSupplierFileSkip.TabIndex = 8;
            this.buttonEditSupplierFileSkip.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.buttonEditSupplierFileSkip_ButtonClick);
            // 
            // bsSett
            // 
            this.bsSett.DataSource = typeof(Databox.Libs.EdgeInfo.ExtSettings);
            // 
            // textEditFTPHost
            // 
            this.textEditFTPHost.Location = new System.Drawing.Point(111, 12);
            this.textEditFTPHost.Name = "textEditFTPHost";
            this.textEditFTPHost.Size = new System.Drawing.Size(406, 20);
            this.textEditFTPHost.StyleController = this.layoutControl1;
            this.textEditFTPHost.TabIndex = 6;
            this.textEditFTPHost.EditValueChanged += new System.EventHandler(this.textEditFTPHost_EditValueChanged);
            // 
            // textEditFTPPassword
            // 
            this.textEditFTPPassword.Location = new System.Drawing.Point(111, 60);
            this.textEditFTPPassword.Name = "textEditFTPPassword";
            this.textEditFTPPassword.Properties.PasswordChar = '*';
            this.textEditFTPPassword.Size = new System.Drawing.Size(406, 20);
            this.textEditFTPPassword.StyleController = this.layoutControl1;
            this.textEditFTPPassword.TabIndex = 5;
            this.textEditFTPPassword.EditValueChanged += new System.EventHandler(this.textEditFTPPassword_EditValueChanged);
            // 
            // textEditFTPLogin
            // 
            this.textEditFTPLogin.Location = new System.Drawing.Point(111, 36);
            this.textEditFTPLogin.Name = "textEditFTPLogin";
            this.textEditFTPLogin.Size = new System.Drawing.Size(406, 20);
            this.textEditFTPLogin.StyleController = this.layoutControl1;
            this.textEditFTPLogin.TabIndex = 4;
            this.textEditFTPLogin.EditValueChanged += new System.EventHandler(this.textEditFTPLogin_EditValueChanged);
            // 
            // buttonEditSupplierFilePath
            // 
            this.buttonEditSupplierFilePath.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.bsSett, "SupplierFilePath", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.buttonEditSupplierFilePath.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bsSett, "SupplierFilePath", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.buttonEditSupplierFilePath.Location = new System.Drawing.Point(111, 84);
            this.buttonEditSupplierFilePath.Name = "buttonEditSupplierFilePath";
            this.buttonEditSupplierFilePath.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.buttonEditSupplierFilePath.Size = new System.Drawing.Size(406, 20);
            this.buttonEditSupplierFilePath.StyleController = this.layoutControl1;
            this.buttonEditSupplierFilePath.TabIndex = 7;
            this.buttonEditSupplierFilePath.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.buttonEditSupplierFilePath_ButtonClick);
            // 
            // checkEditDoBatch
            // 
            this.checkEditDoBatch.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.bsSett, "DoBatch", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkEditDoBatch.Location = new System.Drawing.Point(111, 156);
            this.checkEditDoBatch.Name = "checkEditDoBatch";
            this.checkEditDoBatch.Properties.Caption = "";
            this.checkEditDoBatch.Size = new System.Drawing.Size(406, 19);
            this.checkEditDoBatch.StyleController = this.layoutControl1;
            this.checkEditDoBatch.TabIndex = 9;
            // 
            // buttonEditSupplierZeroItems
            // 
            this.buttonEditSupplierZeroItems.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.bsSett, "SupplierFileZeroSkip", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.buttonEditSupplierZeroItems.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bsSett, "SupplierFileZeroSkip", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.buttonEditSupplierZeroItems.Location = new System.Drawing.Point(111, 132);
            this.buttonEditSupplierZeroItems.Name = "buttonEditSupplierZeroItems";
            this.buttonEditSupplierZeroItems.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.buttonEditSupplierZeroItems.Size = new System.Drawing.Size(406, 20);
            this.buttonEditSupplierZeroItems.StyleController = this.layoutControl1;
            this.buttonEditSupplierZeroItems.TabIndex = 8;
            this.buttonEditSupplierZeroItems.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.buttonEditSupplierZeroItems_ButtonClick);
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroup1.GroupBordersVisible = false;
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1,
            this.emptySpaceItem1,
            this.layoutControlItem2,
            this.layoutControlItem3,
            this.layoutControlItem4,
            this.layoutControlItem5,
            this.layoutControlItem6,
            this.layoutControlItem7});
            this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup1.Name = "Root";
            this.layoutControlGroup1.Size = new System.Drawing.Size(529, 315);
            this.layoutControlGroup1.TextVisible = false;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.textEditFTPLogin;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 24);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(509, 24);
            this.layoutControlItem1.Text = "FTP Login";
            this.layoutControlItem1.TextSize = new System.Drawing.Size(96, 13);
            // 
            // emptySpaceItem1
            // 
            this.emptySpaceItem1.AllowHotTrack = false;
            this.emptySpaceItem1.Location = new System.Drawing.Point(0, 167);
            this.emptySpaceItem1.Name = "emptySpaceItem1";
            this.emptySpaceItem1.Size = new System.Drawing.Size(509, 128);
            this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.textEditFTPPassword;
            this.layoutControlItem2.Location = new System.Drawing.Point(0, 48);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(509, 24);
            this.layoutControlItem2.Text = "FTP Password";
            this.layoutControlItem2.TextSize = new System.Drawing.Size(96, 13);
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this.textEditFTPHost;
            this.layoutControlItem3.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Size = new System.Drawing.Size(509, 24);
            this.layoutControlItem3.Text = "FTP Host";
            this.layoutControlItem3.TextSize = new System.Drawing.Size(96, 13);
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this.buttonEditSupplierFilePath;
            this.layoutControlItem4.Location = new System.Drawing.Point(0, 72);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Size = new System.Drawing.Size(509, 24);
            this.layoutControlItem4.Text = "Supplier File Path";
            this.layoutControlItem4.TextSize = new System.Drawing.Size(96, 13);
            // 
            // layoutControlItem5
            // 
            this.layoutControlItem5.Control = this.buttonEditSupplierFileSkip;
            this.layoutControlItem5.Location = new System.Drawing.Point(0, 96);
            this.layoutControlItem5.Name = "layoutControlItem5";
            this.layoutControlItem5.Size = new System.Drawing.Size(509, 24);
            this.layoutControlItem5.Text = "File for Supplier Skip";
            this.layoutControlItem5.TextSize = new System.Drawing.Size(96, 13);
            // 
            // layoutControlItem6
            // 
            this.layoutControlItem6.Control = this.checkEditDoBatch;
            this.layoutControlItem6.Location = new System.Drawing.Point(0, 144);
            this.layoutControlItem6.Name = "layoutControlItem6";
            this.layoutControlItem6.Size = new System.Drawing.Size(509, 23);
            this.layoutControlItem6.Text = "Do Batch";
            this.layoutControlItem6.TextSize = new System.Drawing.Size(96, 13);
            // 
            // layoutControlItem7
            // 
            this.layoutControlItem7.Control = this.buttonEditSupplierZeroItems;
            this.layoutControlItem7.CustomizationFormText = "Supplier Zero Items";
            this.layoutControlItem7.Location = new System.Drawing.Point(0, 120);
            this.layoutControlItem7.Name = "layoutControlItem7";
            this.layoutControlItem7.Size = new System.Drawing.Size(509, 24);
            this.layoutControlItem7.Text = "Supplier Zero Items";
            this.layoutControlItem7.TextSize = new System.Drawing.Size(96, 13);
            // 
            // ucExtSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.layoutControl1);
            this.Name = "ucExtSettings";
            this.Size = new System.Drawing.Size(551, 338);
            this.Load += new System.EventHandler(this.ucExtSettings_Load);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.buttonEditSupplierFileSkip.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsSett)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditFTPHost.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditFTPPassword.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditFTPLogin.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.buttonEditSupplierFilePath.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEditDoBatch.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.buttonEditSupplierZeroItems.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem7)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.BindingSource bsSett;
        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraEditors.TextEdit textEditFTPHost;
        private DevExpress.XtraEditors.TextEdit textEditFTPPassword;
        private DevExpress.XtraEditors.TextEdit textEditFTPLogin;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
        private DevExpress.XtraEditors.ButtonEdit buttonEditSupplierFilePath;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
        private DevExpress.XtraEditors.ButtonEdit buttonEditSupplierFileSkip;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem5;
        private DevExpress.XtraEditors.CheckEdit checkEditDoBatch;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem6;
        private DevExpress.XtraEditors.ButtonEdit buttonEditSupplierZeroItems;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem7;
    }
}
