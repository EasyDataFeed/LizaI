namespace Databox.Libs.Acumotors
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
            this.spinEdit1 = new DevExpress.XtraEditors.SpinEdit();
            this.spinEdit2 = new DevExpress.XtraEditors.SpinEdit();
            this.spinEdit3 = new DevExpress.XtraEditors.SpinEdit();
            this.checkEdit1 = new DevExpress.XtraEditors.CheckEdit();
            this.radioGroupPercentage = new DevExpress.XtraEditors.RadioGroup();
            this.textEdit1 = new DevExpress.XtraEditors.TextEdit();
            this.checkEdit2 = new DevExpress.XtraEditors.CheckEdit();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem2 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem5 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem6 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem7 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem8 = new DevExpress.XtraLayout.LayoutControlItem();
            this.buttonEdit2 = new DevExpress.XtraEditors.ButtonEdit();
            this.layoutControlItem9 = new DevExpress.XtraLayout.LayoutControlItem();
            this.bsSett = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.buttonEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinEdit2.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinEdit3.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radioGroupPercentage.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit2.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.buttonEdit2.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem9)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsSett)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this.buttonEdit1);
            this.layoutControl1.Controls.Add(this.spinEdit1);
            this.layoutControl1.Controls.Add(this.spinEdit2);
            this.layoutControl1.Controls.Add(this.spinEdit3);
            this.layoutControl1.Controls.Add(this.checkEdit1);
            this.layoutControl1.Controls.Add(this.radioGroupPercentage);
            this.layoutControl1.Controls.Add(this.textEdit1);
            this.layoutControl1.Controls.Add(this.checkEdit2);
            this.layoutControl1.Controls.Add(this.buttonEdit2);
            this.layoutControl1.Location = new System.Drawing.Point(0, 3);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.Root = this.layoutControlGroup1;
            this.layoutControl1.Size = new System.Drawing.Size(765, 483);
            this.layoutControl1.TabIndex = 0;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // buttonEdit1
            // 
            this.buttonEdit1.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.bsSett, "BrandsFilePath", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.buttonEdit1.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bsSett, "BrandsFilePath", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.buttonEdit1.Location = new System.Drawing.Point(141, 45);
            this.buttonEdit1.Name = "buttonEdit1";
            this.buttonEdit1.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.buttonEdit1.Size = new System.Drawing.Size(609, 20);
            this.buttonEdit1.StyleController = this.layoutControl1;
            this.buttonEdit1.TabIndex = 4;
            this.buttonEdit1.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.buttonEdit1_ButtonClick);
            // 
            // spinEdit1
            // 
            this.spinEdit1.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.bsSett, "PercentageForMSRP", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.spinEdit1.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.spinEdit1.Location = new System.Drawing.Point(141, 75);
            this.spinEdit1.Name = "spinEdit1";
            this.spinEdit1.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.spinEdit1.Size = new System.Drawing.Size(106, 20);
            this.spinEdit1.StyleController = this.layoutControl1;
            this.spinEdit1.TabIndex = 6;
            // 
            // spinEdit2
            // 
            this.spinEdit2.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.bsSett, "surchargePerLb", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.spinEdit2.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.spinEdit2.Location = new System.Drawing.Point(141, 105);
            this.spinEdit2.Name = "spinEdit2";
            this.spinEdit2.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.spinEdit2.Size = new System.Drawing.Size(106, 20);
            this.spinEdit2.StyleController = this.layoutControl1;
            this.spinEdit2.TabIndex = 7;
            // 
            // spinEdit3
            // 
            this.spinEdit3.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.bsSett, "minSurcharge", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.spinEdit3.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.spinEdit3.Location = new System.Drawing.Point(383, 105);
            this.spinEdit3.Name = "spinEdit3";
            this.spinEdit3.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.spinEdit3.Size = new System.Drawing.Size(107, 20);
            this.spinEdit3.StyleController = this.layoutControl1;
            this.spinEdit3.TabIndex = 8;
            // 
            // checkEdit1
            // 
            this.checkEdit1.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.bsSett, "doPricePerPound", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkEdit1.Location = new System.Drawing.Point(626, 105);
            this.checkEdit1.Name = "checkEdit1";
            this.checkEdit1.Properties.Caption = "";
            this.checkEdit1.Size = new System.Drawing.Size(124, 19);
            this.checkEdit1.StyleController = this.layoutControl1;
            this.checkEdit1.TabIndex = 9;
            // 
            // radioGroupPercentage
            // 
            this.radioGroupPercentage.Location = new System.Drawing.Point(141, 135);
            this.radioGroupPercentage.Name = "radioGroupPercentage";
            this.radioGroupPercentage.Properties.Columns = 2;
            this.radioGroupPercentage.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "Percentage for all brands"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "Take percentages from file")});
            this.radioGroupPercentage.Size = new System.Drawing.Size(609, 25);
            this.radioGroupPercentage.StyleController = this.layoutControl1;
            this.radioGroupPercentage.TabIndex = 10;
            this.radioGroupPercentage.SelectedIndexChanged += new System.EventHandler(this.radioGroupCompatibilityCheck_SelectedIndexChanged);
            // 
            // textEdit1
            // 
            this.textEdit1.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.bsSett, "GoogleSheetsLink", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textEdit1.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bsSett, "GoogleSheetsLink", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textEdit1.Location = new System.Drawing.Point(141, 170);
            this.textEdit1.Name = "textEdit1";
            this.textEdit1.Size = new System.Drawing.Size(349, 20);
            this.textEdit1.StyleController = this.layoutControl1;
            this.textEdit1.TabIndex = 11;
            // 
            // checkEdit2
            // 
            this.checkEdit2.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.bsSett, "UseGoogleSheets", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkEdit2.Location = new System.Drawing.Point(626, 170);
            this.checkEdit2.Name = "checkEdit2";
            this.checkEdit2.Properties.Caption = "";
            this.checkEdit2.Size = new System.Drawing.Size(124, 19);
            this.checkEdit2.StyleController = this.layoutControl1;
            this.checkEdit2.TabIndex = 12;
            this.checkEdit2.CheckedChanged += new System.EventHandler(this.Export);
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroup1.GroupBordersVisible = false;
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1,
            this.emptySpaceItem1,
            this.layoutControlItem3,
            this.emptySpaceItem2,
            this.layoutControlItem2,
            this.layoutControlItem4,
            this.layoutControlItem5,
            this.layoutControlItem6,
            this.layoutControlItem7,
            this.layoutControlItem8,
            this.layoutControlItem9});
            this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup1.Name = "layoutControlGroup1";
            this.layoutControlGroup1.Size = new System.Drawing.Size(765, 483);
            this.layoutControlGroup1.TextVisible = false;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.buttonEdit1;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 30);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Padding = new DevExpress.XtraLayout.Utils.Padding(5, 5, 5, 5);
            this.layoutControlItem1.Size = new System.Drawing.Size(745, 30);
            this.layoutControlItem1.Text = "Brand file path:";
            this.layoutControlItem1.TextSize = new System.Drawing.Size(123, 13);
            // 
            // emptySpaceItem1
            // 
            this.emptySpaceItem1.AllowHotTrack = false;
            this.emptySpaceItem1.Location = new System.Drawing.Point(0, 185);
            this.emptySpaceItem1.Name = "emptySpaceItem1";
            this.emptySpaceItem1.Size = new System.Drawing.Size(745, 278);
            this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this.spinEdit1;
            this.layoutControlItem3.Location = new System.Drawing.Point(0, 60);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Padding = new DevExpress.XtraLayout.Utils.Padding(5, 5, 5, 5);
            this.layoutControlItem3.Size = new System.Drawing.Size(242, 30);
            this.layoutControlItem3.Text = "Percentage for MSRP:";
            this.layoutControlItem3.TextSize = new System.Drawing.Size(123, 13);
            // 
            // emptySpaceItem2
            // 
            this.emptySpaceItem2.AllowHotTrack = false;
            this.emptySpaceItem2.Location = new System.Drawing.Point(242, 60);
            this.emptySpaceItem2.Name = "emptySpaceItem2";
            this.emptySpaceItem2.Size = new System.Drawing.Size(503, 30);
            this.emptySpaceItem2.TextSize = new System.Drawing.Size(0, 0);
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.spinEdit2;
            this.layoutControlItem2.Location = new System.Drawing.Point(0, 90);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Padding = new DevExpress.XtraLayout.Utils.Padding(5, 5, 5, 5);
            this.layoutControlItem2.Size = new System.Drawing.Size(242, 30);
            this.layoutControlItem2.Text = "Surcharge per lb ($):";
            this.layoutControlItem2.TextSize = new System.Drawing.Size(123, 13);
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this.spinEdit3;
            this.layoutControlItem4.Location = new System.Drawing.Point(242, 90);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Padding = new DevExpress.XtraLayout.Utils.Padding(5, 5, 5, 5);
            this.layoutControlItem4.Size = new System.Drawing.Size(243, 30);
            this.layoutControlItem4.Text = "Min Surcharge:";
            this.layoutControlItem4.TextSize = new System.Drawing.Size(123, 13);
            // 
            // layoutControlItem5
            // 
            this.layoutControlItem5.Control = this.checkEdit1;
            this.layoutControlItem5.Location = new System.Drawing.Point(485, 90);
            this.layoutControlItem5.Name = "layoutControlItem5";
            this.layoutControlItem5.Padding = new DevExpress.XtraLayout.Utils.Padding(5, 5, 5, 5);
            this.layoutControlItem5.Size = new System.Drawing.Size(260, 30);
            this.layoutControlItem5.Text = "Do Price Per Pound:";
            this.layoutControlItem5.TextSize = new System.Drawing.Size(123, 13);
            // 
            // layoutControlItem6
            // 
            this.layoutControlItem6.Control = this.radioGroupPercentage;
            this.layoutControlItem6.Location = new System.Drawing.Point(0, 120);
            this.layoutControlItem6.Name = "layoutControlItem6";
            this.layoutControlItem6.Padding = new DevExpress.XtraLayout.Utils.Padding(5, 5, 5, 5);
            this.layoutControlItem6.Size = new System.Drawing.Size(745, 35);
            this.layoutControlItem6.Text = "Rules for percentages:";
            this.layoutControlItem6.TextSize = new System.Drawing.Size(123, 13);
            // 
            // layoutControlItem7
            // 
            this.layoutControlItem7.Control = this.textEdit1;
            this.layoutControlItem7.Location = new System.Drawing.Point(0, 155);
            this.layoutControlItem7.Name = "layoutControlItem7";
            this.layoutControlItem7.Padding = new DevExpress.XtraLayout.Utils.Padding(5, 5, 5, 5);
            this.layoutControlItem7.Size = new System.Drawing.Size(485, 30);
            this.layoutControlItem7.Text = "Google spreadsheets link:";
            this.layoutControlItem7.TextSize = new System.Drawing.Size(123, 13);
            // 
            // layoutControlItem8
            // 
            this.layoutControlItem8.Control = this.checkEdit2;
            this.layoutControlItem8.Location = new System.Drawing.Point(485, 155);
            this.layoutControlItem8.Name = "layoutControlItem8";
            this.layoutControlItem8.Padding = new DevExpress.XtraLayout.Utils.Padding(5, 5, 5, 5);
            this.layoutControlItem8.Size = new System.Drawing.Size(260, 30);
            this.layoutControlItem8.Text = "Use Google Sheets:";
            this.layoutControlItem8.TextSize = new System.Drawing.Size(123, 13);
            // 
            // buttonEdit2
            // 
            this.buttonEdit2.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.bsSett, "SpecificationFilePath", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.buttonEdit2.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bsSett, "SpecificationFilePath", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.buttonEdit2.Location = new System.Drawing.Point(141, 15);
            this.buttonEdit2.Name = "buttonEdit2";
            this.buttonEdit2.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.buttonEdit2.Size = new System.Drawing.Size(609, 20);
            this.buttonEdit2.StyleController = this.layoutControl1;
            this.buttonEdit2.TabIndex = 13;
            this.buttonEdit2.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.buttonEdit2_ButtonClick);
            // 
            // layoutControlItem9
            // 
            this.layoutControlItem9.Control = this.buttonEdit2;
            this.layoutControlItem9.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem9.Name = "layoutControlItem9";
            this.layoutControlItem9.Padding = new DevExpress.XtraLayout.Utils.Padding(5, 5, 5, 5);
            this.layoutControlItem9.Size = new System.Drawing.Size(745, 30);
            this.layoutControlItem9.Text = "Specification file path:";
            this.layoutControlItem9.TextSize = new System.Drawing.Size(123, 13);
            // 
            // bsSett
            // 
            this.bsSett.DataSource = typeof(Databox.Libs.Acumotors.ExtSettings);
            // 
            // ucExtSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.layoutControl1);
            this.Name = "ucExtSettings";
            this.Size = new System.Drawing.Size(774, 493);
            this.Load += new System.EventHandler(this.ucExtSettings_Load);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.buttonEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinEdit2.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinEdit3.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radioGroupPercentage.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit2.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.buttonEdit2.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem9)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsSett)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.BindingSource bsSett;
        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraEditors.ButtonEdit buttonEdit1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem2;
        private DevExpress.XtraEditors.SpinEdit spinEdit1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
        private DevExpress.XtraEditors.SpinEdit spinEdit2;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraEditors.SpinEdit spinEdit3;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
        private DevExpress.XtraEditors.CheckEdit checkEdit1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem5;
        private DevExpress.XtraEditors.RadioGroup radioGroupPercentage;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem6;
        private DevExpress.XtraEditors.TextEdit textEdit1;
        private DevExpress.XtraEditors.CheckEdit checkEdit2;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem7;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem8;
        private DevExpress.XtraEditors.ButtonEdit buttonEdit2;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem9;
    }
}
