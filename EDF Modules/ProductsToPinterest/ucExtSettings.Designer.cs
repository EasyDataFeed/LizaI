namespace Databox.Libs.ProductsToPinterest
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
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.buttonEdit1 = new DevExpress.XtraEditors.ButtonEdit();
            this.buttonEdit2 = new DevExpress.XtraEditors.ButtonEdit();
            this.radioGroupDestCategory = new DevExpress.XtraEditors.RadioGroup();
            this.checkEdit2 = new DevExpress.XtraEditors.CheckEdit();
            this.checkEdit3 = new DevExpress.XtraEditors.CheckEdit();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem5 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem6 = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.bsSett = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.buttonEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.buttonEdit2.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radioGroupDestCategory.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit2.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit3.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsSett)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this.buttonEdit1);
            this.layoutControl1.Controls.Add(this.buttonEdit2);
            this.layoutControl1.Controls.Add(this.radioGroupDestCategory);
            this.layoutControl1.Controls.Add(this.checkEdit2);
            this.layoutControl1.Controls.Add(this.checkEdit3);
            this.layoutControl1.Location = new System.Drawing.Point(6, 4);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.Root = this.layoutControlGroup1;
            this.layoutControl1.Size = new System.Drawing.Size(887, 592);
            this.layoutControl1.TabIndex = 0;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // buttonEdit1
            // 
            this.buttonEdit1.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.bsSett, "ExportFilePath", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.buttonEdit1.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bsSett, "ExportFilePath", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.buttonEdit1.Location = new System.Drawing.Point(136, 15);
            this.buttonEdit1.Name = "buttonEdit1";
            this.buttonEdit1.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.buttonEdit1.Size = new System.Drawing.Size(736, 20);
            this.buttonEdit1.StyleController = this.layoutControl1;
            this.buttonEdit1.TabIndex = 4;
            this.buttonEdit1.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.buttonEdit1_ButtonClick);
            // 
            // buttonEdit2
            // 
            this.buttonEdit2.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.bsSett, "Brand", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.buttonEdit2.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bsSett, "Brand", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.buttonEdit2.Location = new System.Drawing.Point(136, 45);
            this.buttonEdit2.Name = "buttonEdit2";
            this.buttonEdit2.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.buttonEdit2.Size = new System.Drawing.Size(736, 20);
            this.buttonEdit2.StyleController = this.layoutControl1;
            this.buttonEdit2.TabIndex = 5;
            this.buttonEdit2.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.buttonEdit2_ButtonClick);
            // 
            // radioGroupDestCategory
            // 
            this.radioGroupDestCategory.Location = new System.Drawing.Point(136, 75);
            this.radioGroupDestCategory.Name = "radioGroupDestCategory";
            this.radioGroupDestCategory.Properties.Columns = 3;
            this.radioGroupDestCategory.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "Brand"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "Main Category"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "Sub Category")});
            this.radioGroupDestCategory.Size = new System.Drawing.Size(736, 27);
            this.radioGroupDestCategory.StyleController = this.layoutControl1;
            this.radioGroupDestCategory.TabIndex = 6;
            this.radioGroupDestCategory.SelectedIndexChanged += new System.EventHandler(this.radioGroupDestCategory_SelectedIndexChanged);
            // 
            // checkEdit2
            // 
            this.checkEdit2.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.bsSett, "UseBusinessesAccount", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkEdit2.Location = new System.Drawing.Point(136, 112);
            this.checkEdit2.Name = "checkEdit2";
            this.checkEdit2.Properties.Caption = "";
            this.checkEdit2.Size = new System.Drawing.Size(736, 19);
            this.checkEdit2.StyleController = this.layoutControl1;
            this.checkEdit2.TabIndex = 8;
            // 
            // checkEdit3
            // 
            this.checkEdit3.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.bsSett, "ChangeWebsite", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkEdit3.Location = new System.Drawing.Point(136, 141);
            this.checkEdit3.Name = "checkEdit3";
            this.checkEdit3.Properties.Caption = "";
            this.checkEdit3.Size = new System.Drawing.Size(736, 19);
            this.checkEdit3.StyleController = this.layoutControl1;
            this.checkEdit3.TabIndex = 9;
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroup1.GroupBordersVisible = false;
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1,
            this.layoutControlItem2,
            this.layoutControlItem3,
            this.layoutControlItem5,
            this.layoutControlItem6,
            this.emptySpaceItem1});
            this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup1.Name = "layoutControlGroup1";
            this.layoutControlGroup1.Size = new System.Drawing.Size(887, 592);
            this.layoutControlGroup1.TextVisible = false;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.buttonEdit1;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Padding = new DevExpress.XtraLayout.Utils.Padding(5, 5, 5, 5);
            this.layoutControlItem1.Size = new System.Drawing.Size(867, 30);
            this.layoutControlItem1.Text = "Export file path:";
            this.layoutControlItem1.TextSize = new System.Drawing.Size(118, 13);
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.buttonEdit2;
            this.layoutControlItem2.Location = new System.Drawing.Point(0, 30);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Padding = new DevExpress.XtraLayout.Utils.Padding(5, 5, 5, 5);
            this.layoutControlItem2.Size = new System.Drawing.Size(867, 30);
            this.layoutControlItem2.Text = "Condition file path:";
            this.layoutControlItem2.TextSize = new System.Drawing.Size(118, 13);
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this.radioGroupDestCategory;
            this.layoutControlItem3.Location = new System.Drawing.Point(0, 60);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Padding = new DevExpress.XtraLayout.Utils.Padding(5, 5, 5, 5);
            this.layoutControlItem3.Size = new System.Drawing.Size(867, 37);
            this.layoutControlItem3.Text = "Title for Board:";
            this.layoutControlItem3.TextSize = new System.Drawing.Size(118, 13);
            // 
            // layoutControlItem5
            // 
            this.layoutControlItem5.Control = this.checkEdit2;
            this.layoutControlItem5.Location = new System.Drawing.Point(0, 97);
            this.layoutControlItem5.Name = "layoutControlItem5";
            this.layoutControlItem5.Padding = new DevExpress.XtraLayout.Utils.Padding(5, 5, 5, 5);
            this.layoutControlItem5.Size = new System.Drawing.Size(867, 29);
            this.layoutControlItem5.Text = "Use businesses account:";
            this.layoutControlItem5.TextSize = new System.Drawing.Size(118, 13);
            // 
            // layoutControlItem6
            // 
            this.layoutControlItem6.Control = this.checkEdit3;
            this.layoutControlItem6.Location = new System.Drawing.Point(0, 126);
            this.layoutControlItem6.Name = "layoutControlItem6";
            this.layoutControlItem6.Padding = new DevExpress.XtraLayout.Utils.Padding(5, 5, 5, 5);
            this.layoutControlItem6.Size = new System.Drawing.Size(867, 29);
            this.layoutControlItem6.Text = "Change \'website\':";
            this.layoutControlItem6.TextSize = new System.Drawing.Size(118, 13);
            // 
            // emptySpaceItem1
            // 
            this.emptySpaceItem1.AllowHotTrack = false;
            this.emptySpaceItem1.Location = new System.Drawing.Point(0, 155);
            this.emptySpaceItem1.Name = "emptySpaceItem1";
            this.emptySpaceItem1.Size = new System.Drawing.Size(867, 417);
            this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
            // 
            // bsSett
            // 
            this.bsSett.DataSource = typeof(Databox.Libs.ProductsToPinterest.ExtSettings);
            // 
            // ucExtSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.layoutControl1);
            this.Name = "ucExtSettings";
            this.Size = new System.Drawing.Size(938, 603);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.buttonEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.buttonEdit2.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radioGroupDestCategory.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit2.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit3.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsSett)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.BindingSource bsSett;
        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraEditors.ButtonEdit buttonEdit1;
        private DevExpress.XtraEditors.ButtonEdit buttonEdit2;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraEditors.RadioGroup radioGroupDestCategory;
        private DevExpress.XtraEditors.CheckEdit checkEdit2;
        private DevExpress.XtraEditors.CheckEdit checkEdit3;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem5;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem6;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
    }
}
