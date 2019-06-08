namespace Databox.Libs.XmlCombine
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
            this.buttonEditInputDirectory = new DevExpress.XtraEditors.ButtonEdit();
            this.bsSett = new System.Windows.Forms.BindingSource(this.components);
            this.buttonEditOutputDirectory = new DevExpress.XtraEditors.ButtonEdit();
            this.checkEdit1 = new DevExpress.XtraEditors.CheckEdit();
            this.buttonEditFilterFile = new DevExpress.XtraEditors.ButtonEdit();
            this.spinEdit1 = new DevExpress.XtraEditors.SpinEdit();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItemVehicleNameWordPosition = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.buttonEditInputDirectory.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsSett)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.buttonEditOutputDirectory.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.buttonEditFilterFile.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemVehicleNameWordPosition)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this.buttonEditInputDirectory);
            this.layoutControl1.Controls.Add(this.buttonEditOutputDirectory);
            this.layoutControl1.Controls.Add(this.checkEdit1);
            this.layoutControl1.Controls.Add(this.buttonEditFilterFile);
            this.layoutControl1.Controls.Add(this.spinEdit1);
            this.layoutControl1.Location = new System.Drawing.Point(4, 4);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.Root = this.layoutControlGroup1;
            this.layoutControl1.Size = new System.Drawing.Size(482, 326);
            this.layoutControl1.TabIndex = 0;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // buttonEditInputDirectory
            // 
            this.buttonEditInputDirectory.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.bsSett, "InputDirectory", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.buttonEditInputDirectory.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bsSett, "InputDirectory", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.buttonEditInputDirectory.Location = new System.Drawing.Point(177, 16);
            this.buttonEditInputDirectory.Name = "buttonEditInputDirectory";
            this.buttonEditInputDirectory.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.buttonEditInputDirectory.Size = new System.Drawing.Size(289, 20);
            this.buttonEditInputDirectory.StyleController = this.layoutControl1;
            this.buttonEditInputDirectory.TabIndex = 5;
            this.buttonEditInputDirectory.Click += new System.EventHandler(this.buttonEditInputDirectory_Click);
            // 
            // bsSett
            // 
            this.bsSett.DataSource = typeof(Databox.Libs.XmlCombine.ExtSettings);
            // 
            // buttonEditOutputDirectory
            // 
            this.buttonEditOutputDirectory.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.bsSett, "OutputDirectory", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.buttonEditOutputDirectory.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bsSett, "OutputDirectory", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.buttonEditOutputDirectory.Location = new System.Drawing.Point(177, 48);
            this.buttonEditOutputDirectory.Name = "buttonEditOutputDirectory";
            this.buttonEditOutputDirectory.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.buttonEditOutputDirectory.Size = new System.Drawing.Size(289, 20);
            this.buttonEditOutputDirectory.StyleController = this.layoutControl1;
            this.buttonEditOutputDirectory.TabIndex = 6;
            this.buttonEditOutputDirectory.Click += new System.EventHandler(this.buttonEditOutputDirectory_Click);
            // 
            // checkEdit1
            // 
            this.checkEdit1.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.bsSett, "UseFilterFile", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkEdit1.Location = new System.Drawing.Point(408, 80);
            this.checkEdit1.Name = "checkEdit1";
            this.checkEdit1.Properties.Caption = "";
            this.checkEdit1.Size = new System.Drawing.Size(58, 19);
            this.checkEdit1.StyleController = this.layoutControl1;
            this.checkEdit1.TabIndex = 7;
            // 
            // buttonEditFilterFile
            // 
            this.buttonEditFilterFile.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.bsSett, "FilterFileFilePath", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.buttonEditFilterFile.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bsSett, "FilterFileFilePath", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.buttonEditFilterFile.Location = new System.Drawing.Point(177, 112);
            this.buttonEditFilterFile.Name = "buttonEditFilterFile";
            this.buttonEditFilterFile.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.buttonEditFilterFile.Size = new System.Drawing.Size(289, 20);
            this.buttonEditFilterFile.StyleController = this.layoutControl1;
            this.buttonEditFilterFile.TabIndex = 8;
            this.buttonEditFilterFile.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.buttonEditFilterFile_ButtonClick);
            // 
            // spinEdit1
            // 
            this.spinEdit1.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.bsSett, "VehicleNameWordPosition", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.spinEdit1.EditValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.spinEdit1.Location = new System.Drawing.Point(177, 80);
            this.spinEdit1.Name = "spinEdit1";
            this.spinEdit1.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.spinEdit1.Properties.MaxValue = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.spinEdit1.Properties.MinValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.spinEdit1.Size = new System.Drawing.Size(58, 20);
            this.spinEdit1.StyleController = this.layoutControl1;
            this.spinEdit1.TabIndex = 9;
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroup1.GroupBordersVisible = false;
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem2,
            this.layoutControlItem1,
            this.layoutControlItem4,
            this.layoutControlItemVehicleNameWordPosition,
            this.layoutControlItem3});
            this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup1.Name = "layoutControlGroup1";
            this.layoutControlGroup1.Size = new System.Drawing.Size(482, 326);
            this.layoutControlGroup1.TextVisible = false;
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 9.25F);
            this.layoutControlItem2.AppearanceItemCaption.Options.UseFont = true;
            this.layoutControlItem2.Control = this.buttonEditInputDirectory;
            this.layoutControlItem2.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Padding = new DevExpress.XtraLayout.Utils.Padding(6, 6, 6, 6);
            this.layoutControlItem2.Size = new System.Drawing.Size(462, 32);
            this.layoutControlItem2.Text = "Input Directory:";
            this.layoutControlItem2.TextSize = new System.Drawing.Size(158, 14);
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 9.25F);
            this.layoutControlItem1.AppearanceItemCaption.Options.UseFont = true;
            this.layoutControlItem1.Control = this.buttonEditOutputDirectory;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 32);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Padding = new DevExpress.XtraLayout.Utils.Padding(6, 6, 6, 6);
            this.layoutControlItem1.Size = new System.Drawing.Size(462, 32);
            this.layoutControlItem1.Text = "Output Directory:";
            this.layoutControlItem1.TextSize = new System.Drawing.Size(158, 14);
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 9.25F);
            this.layoutControlItem4.AppearanceItemCaption.Options.UseFont = true;
            this.layoutControlItem4.Control = this.buttonEditFilterFile;
            this.layoutControlItem4.Location = new System.Drawing.Point(0, 96);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Padding = new DevExpress.XtraLayout.Utils.Padding(6, 6, 6, 6);
            this.layoutControlItem4.Size = new System.Drawing.Size(462, 210);
            this.layoutControlItem4.Text = "Filter File Path:";
            this.layoutControlItem4.TextSize = new System.Drawing.Size(158, 14);
            // 
            // layoutControlItemVehicleNameWordPosition
            // 
            this.layoutControlItemVehicleNameWordPosition.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 9.25F);
            this.layoutControlItemVehicleNameWordPosition.AppearanceItemCaption.Options.UseFont = true;
            this.layoutControlItemVehicleNameWordPosition.Control = this.spinEdit1;
            this.layoutControlItemVehicleNameWordPosition.Location = new System.Drawing.Point(0, 64);
            this.layoutControlItemVehicleNameWordPosition.Name = "layoutControlItemVehicleNameWordPosition";
            this.layoutControlItemVehicleNameWordPosition.Padding = new DevExpress.XtraLayout.Utils.Padding(6, 6, 6, 6);
            this.layoutControlItemVehicleNameWordPosition.Size = new System.Drawing.Size(231, 32);
            this.layoutControlItemVehicleNameWordPosition.Text = "Vehicle Name Word Position:";
            this.layoutControlItemVehicleNameWordPosition.TextSize = new System.Drawing.Size(158, 14);
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 9.25F);
            this.layoutControlItem3.AppearanceItemCaption.Options.UseFont = true;
            this.layoutControlItem3.Control = this.checkEdit1;
            this.layoutControlItem3.Location = new System.Drawing.Point(231, 64);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Padding = new DevExpress.XtraLayout.Utils.Padding(6, 6, 6, 6);
            this.layoutControlItem3.Size = new System.Drawing.Size(231, 32);
            this.layoutControlItem3.Text = "Use Filter File:";
            this.layoutControlItem3.TextSize = new System.Drawing.Size(158, 14);
            // 
            // ucExtSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.layoutControl1);
            this.Name = "ucExtSettings";
            this.Size = new System.Drawing.Size(490, 337);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.buttonEditInputDirectory.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsSett)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.buttonEditOutputDirectory.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.buttonEditFilterFile.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemVehicleNameWordPosition)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.BindingSource bsSett;
        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraEditors.ButtonEdit buttonEditInputDirectory;
        private DevExpress.XtraEditors.ButtonEdit buttonEditOutputDirectory;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraEditors.CheckEdit checkEdit1;
        private DevExpress.XtraEditors.ButtonEdit buttonEditFilterFile;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
        private DevExpress.XtraEditors.SpinEdit spinEdit1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItemVehicleNameWordPosition;
    }
}
