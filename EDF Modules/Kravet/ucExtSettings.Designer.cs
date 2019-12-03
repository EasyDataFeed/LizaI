namespace Databox.Libs.Kravet
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
            this.buttonEdit1 = new DevExpress.XtraEditors.ButtonEdit();
            this.bsSett = new System.Windows.Forms.BindingSource();
            this.checkEdit1 = new DevExpress.XtraEditors.CheckEdit();
            this.spinEdit1 = new DevExpress.XtraEditors.SpinEdit();
            this.spinEdit2 = new DevExpress.XtraEditors.SpinEdit();
            this.checkEdit2 = new DevExpress.XtraEditors.CheckEdit();
            this.textEdit1 = new DevExpress.XtraEditors.TextEdit();
            this.textEdit2 = new DevExpress.XtraEditors.TextEdit();
            this.textEdit3 = new DevExpress.XtraEditors.TextEdit();
            this.textEdit4 = new DevExpress.XtraEditors.TextEdit();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlGroup2 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem6 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem7 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem8 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem9 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlGroup3 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem5 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.buttonEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsSett)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinEdit2.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit2.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit2.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit3.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit4.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem9)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this.buttonEdit1);
            this.layoutControl1.Controls.Add(this.checkEdit1);
            this.layoutControl1.Controls.Add(this.spinEdit1);
            this.layoutControl1.Controls.Add(this.spinEdit2);
            this.layoutControl1.Controls.Add(this.checkEdit2);
            this.layoutControl1.Controls.Add(this.textEdit1);
            this.layoutControl1.Controls.Add(this.textEdit2);
            this.layoutControl1.Controls.Add(this.textEdit3);
            this.layoutControl1.Controls.Add(this.textEdit4);
            this.layoutControl1.Location = new System.Drawing.Point(3, 4);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.Root = this.layoutControlGroup1;
            this.layoutControl1.Size = new System.Drawing.Size(947, 656);
            this.layoutControl1.TabIndex = 0;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // buttonEdit1
            // 
            this.buttonEdit1.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.bsSett, "ExportFilePath", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.buttonEdit1.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bsSett, "ExportFilePath", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.buttonEdit1.Location = new System.Drawing.Point(127, 45);
            this.buttonEdit1.Name = "buttonEdit1";
            this.buttonEdit1.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.buttonEdit1.Size = new System.Drawing.Size(793, 20);
            this.buttonEdit1.StyleController = this.layoutControl1;
            this.buttonEdit1.TabIndex = 4;
            this.buttonEdit1.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.buttonEdit1_ButtonClick);
            // 
            // bsSett
            // 
            this.bsSett.DataSource = typeof(Databox.Libs.Kravet.ExtSettings);
            // 
            // checkEdit1
            // 
            this.checkEdit1.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.bsSett, "UseExistingExport", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkEdit1.Location = new System.Drawing.Point(127, 75);
            this.checkEdit1.Name = "checkEdit1";
            this.checkEdit1.Properties.Caption = "";
            this.checkEdit1.Size = new System.Drawing.Size(793, 19);
            this.checkEdit1.StyleController = this.layoutControl1;
            this.checkEdit1.TabIndex = 5;
            this.checkEdit1.CheckedChanged += new System.EventHandler(this.Export);
            // 
            // spinEdit1
            // 
            this.spinEdit1.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.bsSett, "WebPriceJober", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.spinEdit1.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.spinEdit1.Location = new System.Drawing.Point(127, 104);
            this.spinEdit1.Name = "spinEdit1";
            this.spinEdit1.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.spinEdit1.Size = new System.Drawing.Size(793, 20);
            this.spinEdit1.StyleController = this.layoutControl1;
            this.spinEdit1.TabIndex = 6;
            // 
            // spinEdit2
            // 
            this.spinEdit2.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.bsSett, "MSRP", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.spinEdit2.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.spinEdit2.Location = new System.Drawing.Point(127, 134);
            this.spinEdit2.Name = "spinEdit2";
            this.spinEdit2.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.spinEdit2.Size = new System.Drawing.Size(793, 20);
            this.spinEdit2.StyleController = this.layoutControl1;
            this.spinEdit2.TabIndex = 7;
            // 
            // checkEdit2
            // 
            this.checkEdit2.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.bsSett, "DoBatch", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkEdit2.Location = new System.Drawing.Point(127, 164);
            this.checkEdit2.Name = "checkEdit2";
            this.checkEdit2.Properties.Caption = "";
            this.checkEdit2.Size = new System.Drawing.Size(793, 19);
            this.checkEdit2.StyleController = this.layoutControl1;
            this.checkEdit2.TabIndex = 8;
            // 
            // textEdit1
            // 
            this.textEdit1.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.bsSett, "FTPLogin", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textEdit1.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bsSett, "FTPLogin", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textEdit1.Location = new System.Drawing.Point(127, 235);
            this.textEdit1.Name = "textEdit1";
            this.textEdit1.Size = new System.Drawing.Size(793, 20);
            this.textEdit1.StyleController = this.layoutControl1;
            this.textEdit1.TabIndex = 9;
            // 
            // textEdit2
            // 
            this.textEdit2.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.bsSett, "FTPPassword", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textEdit2.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bsSett, "FTPPassword", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textEdit2.Location = new System.Drawing.Point(127, 265);
            this.textEdit2.Name = "textEdit2";
            this.textEdit2.Size = new System.Drawing.Size(793, 20);
            this.textEdit2.StyleController = this.layoutControl1;
            this.textEdit2.TabIndex = 10;
            // 
            // textEdit3
            // 
            this.textEdit3.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.bsSett, "FTPHost", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textEdit3.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bsSett, "FTPHost", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textEdit3.Location = new System.Drawing.Point(127, 295);
            this.textEdit3.Name = "textEdit3";
            this.textEdit3.Size = new System.Drawing.Size(793, 20);
            this.textEdit3.StyleController = this.layoutControl1;
            this.textEdit3.TabIndex = 11;
            // 
            // textEdit4
            // 
            this.textEdit4.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.bsSett, "FtpFilePath", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textEdit4.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bsSett, "FtpFilePath", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textEdit4.Location = new System.Drawing.Point(127, 325);
            this.textEdit4.Name = "textEdit4";
            this.textEdit4.Size = new System.Drawing.Size(793, 20);
            this.textEdit4.StyleController = this.layoutControl1;
            this.textEdit4.TabIndex = 12;
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroup1.GroupBordersVisible = false;
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlGroup2,
            this.layoutControlGroup3});
            this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup1.Name = "layoutControlGroup1";
            this.layoutControlGroup1.Size = new System.Drawing.Size(947, 656);
            this.layoutControlGroup1.TextVisible = false;
            // 
            // layoutControlGroup2
            // 
            this.layoutControlGroup2.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem6,
            this.layoutControlItem7,
            this.layoutControlItem8,
            this.layoutControlItem9});
            this.layoutControlGroup2.Location = new System.Drawing.Point(0, 190);
            this.layoutControlGroup2.Name = "layoutControlGroup2";
            this.layoutControlGroup2.Size = new System.Drawing.Size(927, 446);
            this.layoutControlGroup2.Text = "FTP Credentials";
            // 
            // layoutControlItem6
            // 
            this.layoutControlItem6.Control = this.textEdit1;
            this.layoutControlItem6.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem6.Name = "layoutControlItem6";
            this.layoutControlItem6.Padding = new DevExpress.XtraLayout.Utils.Padding(5, 5, 5, 5);
            this.layoutControlItem6.Size = new System.Drawing.Size(903, 30);
            this.layoutControlItem6.Text = "FTP login:";
            this.layoutControlItem6.TextSize = new System.Drawing.Size(97, 13);
            // 
            // layoutControlItem7
            // 
            this.layoutControlItem7.Control = this.textEdit2;
            this.layoutControlItem7.Location = new System.Drawing.Point(0, 30);
            this.layoutControlItem7.Name = "layoutControlItem7";
            this.layoutControlItem7.Padding = new DevExpress.XtraLayout.Utils.Padding(5, 5, 5, 5);
            this.layoutControlItem7.Size = new System.Drawing.Size(903, 30);
            this.layoutControlItem7.Text = "FTP password:";
            this.layoutControlItem7.TextSize = new System.Drawing.Size(97, 13);
            // 
            // layoutControlItem8
            // 
            this.layoutControlItem8.Control = this.textEdit3;
            this.layoutControlItem8.Location = new System.Drawing.Point(0, 60);
            this.layoutControlItem8.Name = "layoutControlItem8";
            this.layoutControlItem8.Padding = new DevExpress.XtraLayout.Utils.Padding(5, 5, 5, 5);
            this.layoutControlItem8.Size = new System.Drawing.Size(903, 30);
            this.layoutControlItem8.Text = "FTP host:";
            this.layoutControlItem8.TextSize = new System.Drawing.Size(97, 13);
            // 
            // layoutControlItem9
            // 
            this.layoutControlItem9.Control = this.textEdit4;
            this.layoutControlItem9.Location = new System.Drawing.Point(0, 90);
            this.layoutControlItem9.Name = "layoutControlItem9";
            this.layoutControlItem9.Padding = new DevExpress.XtraLayout.Utils.Padding(5, 5, 5, 5);
            this.layoutControlItem9.Size = new System.Drawing.Size(903, 314);
            this.layoutControlItem9.Text = "FTP file path:";
            this.layoutControlItem9.TextSize = new System.Drawing.Size(97, 13);
            // 
            // layoutControlGroup3
            // 
            this.layoutControlGroup3.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem5,
            this.layoutControlItem4,
            this.layoutControlItem3,
            this.layoutControlItem2,
            this.layoutControlItem1});
            this.layoutControlGroup3.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup3.Name = "layoutControlGroup3";
            this.layoutControlGroup3.Size = new System.Drawing.Size(927, 190);
            this.layoutControlGroup3.Text = "Price Update";
            // 
            // layoutControlItem5
            // 
            this.layoutControlItem5.Control = this.checkEdit2;
            this.layoutControlItem5.Location = new System.Drawing.Point(0, 119);
            this.layoutControlItem5.Name = "layoutControlItem5";
            this.layoutControlItem5.Padding = new DevExpress.XtraLayout.Utils.Padding(5, 5, 5, 5);
            this.layoutControlItem5.Size = new System.Drawing.Size(903, 29);
            this.layoutControlItem5.Text = "Do Batch:";
            this.layoutControlItem5.TextSize = new System.Drawing.Size(97, 13);
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this.spinEdit2;
            this.layoutControlItem4.Location = new System.Drawing.Point(0, 89);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Padding = new DevExpress.XtraLayout.Utils.Padding(5, 5, 5, 5);
            this.layoutControlItem4.Size = new System.Drawing.Size(903, 30);
            this.layoutControlItem4.Text = "MSRP:";
            this.layoutControlItem4.TextSize = new System.Drawing.Size(97, 13);
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this.spinEdit1;
            this.layoutControlItem3.Location = new System.Drawing.Point(0, 59);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Padding = new DevExpress.XtraLayout.Utils.Padding(5, 5, 5, 5);
            this.layoutControlItem3.Size = new System.Drawing.Size(903, 30);
            this.layoutControlItem3.Text = "WebPrice / Jober:";
            this.layoutControlItem3.TextSize = new System.Drawing.Size(97, 13);
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.checkEdit1;
            this.layoutControlItem2.Location = new System.Drawing.Point(0, 30);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Padding = new DevExpress.XtraLayout.Utils.Padding(5, 5, 5, 5);
            this.layoutControlItem2.Size = new System.Drawing.Size(903, 29);
            this.layoutControlItem2.Text = "Use existing export:";
            this.layoutControlItem2.TextSize = new System.Drawing.Size(97, 13);
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.buttonEdit1;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Padding = new DevExpress.XtraLayout.Utils.Padding(5, 5, 5, 5);
            this.layoutControlItem1.Size = new System.Drawing.Size(903, 30);
            this.layoutControlItem1.Text = "Export file path:";
            this.layoutControlItem1.TextSize = new System.Drawing.Size(97, 13);
            // 
            // ucExtSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.layoutControl1);
            this.Name = "ucExtSettings";
            this.Size = new System.Drawing.Size(996, 672);
            this.Load += new System.EventHandler(this.ucExtSettings_Load);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.buttonEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsSett)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinEdit2.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit2.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit2.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit3.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit4.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem9)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.BindingSource bsSett;
        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraEditors.ButtonEdit buttonEdit1;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraEditors.CheckEdit checkEdit1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraEditors.SpinEdit spinEdit1;
        private DevExpress.XtraEditors.SpinEdit spinEdit2;
        private DevExpress.XtraEditors.CheckEdit checkEdit2;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem5;
        private DevExpress.XtraEditors.TextEdit textEdit1;
        private DevExpress.XtraEditors.TextEdit textEdit2;
        private DevExpress.XtraEditors.TextEdit textEdit3;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup2;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem6;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem7;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem8;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup3;
        private DevExpress.XtraEditors.TextEdit textEdit4;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem9;
    }
}
