﻿namespace Databox.Libs.Turn14_EDF
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
            this.bsSett = new System.Windows.Forms.BindingSource(this.components);
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.dateEditDateFrom = new DevExpress.XtraEditors.DateEdit();
            this.dateEditDateTo = new DevExpress.XtraEditors.DateEdit();
            this.checkEditDoBatch = new DevExpress.XtraEditors.CheckEdit();
            this.checkEditInventorySync = new DevExpress.XtraEditors.CheckEdit();
            this.checkEditPriceSync = new DevExpress.XtraEditors.CheckEdit();
            this.checkEditOrderSync = new DevExpress.XtraEditors.CheckEdit();
            this.buttonEditBrandFilePath = new DevExpress.XtraEditors.ButtonEdit();
            this.spinEditPersentageOfCost = new DevExpress.XtraEditors.SpinEdit();
            this.checkEditConsiderMAPPrice = new DevExpress.XtraEditors.CheckEdit();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlGroupOrderSync = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.layoutControlGroupInvAndPriceSync = new DevExpress.XtraLayout.LayoutControlGroup();
            this.emptySpaceItem2 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem8 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem9 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem5 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem6 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem7 = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.bsSett)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditDateFrom.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditDateFrom.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditDateTo.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditDateTo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEditDoBatch.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEditInventorySync.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEditPriceSync.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEditOrderSync.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.buttonEditBrandFilePath.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinEditPersentageOfCost.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEditConsiderMAPPrice.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroupOrderSync)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroupInvAndPriceSync)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem9)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem7)).BeginInit();
            this.SuspendLayout();
            // 
            // bsSett
            // 
            this.bsSett.DataSource = typeof(Databox.Libs.Turn14_EDF.ExtSettings);
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this.dateEditDateFrom);
            this.layoutControl1.Controls.Add(this.dateEditDateTo);
            this.layoutControl1.Controls.Add(this.checkEditDoBatch);
            this.layoutControl1.Controls.Add(this.checkEditInventorySync);
            this.layoutControl1.Controls.Add(this.checkEditPriceSync);
            this.layoutControl1.Controls.Add(this.checkEditOrderSync);
            this.layoutControl1.Controls.Add(this.buttonEditBrandFilePath);
            this.layoutControl1.Controls.Add(this.spinEditPersentageOfCost);
            this.layoutControl1.Controls.Add(this.checkEditConsiderMAPPrice);
            this.layoutControl1.Location = new System.Drawing.Point(4, 4);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.Root = this.layoutControlGroup1;
            this.layoutControl1.Size = new System.Drawing.Size(707, 406);
            this.layoutControl1.TabIndex = 0;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // dateEditDateFrom
            // 
            this.dateEditDateFrom.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.bsSett, "DateFrom", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.dateEditDateFrom.EditValue = null;
            this.dateEditDateFrom.Location = new System.Drawing.Point(136, 74);
            this.dateEditDateFrom.Name = "dateEditDateFrom";
            this.dateEditDateFrom.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateEditDateFrom.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateEditDateFrom.Size = new System.Drawing.Size(212, 20);
            this.dateEditDateFrom.StyleController = this.layoutControl1;
            this.dateEditDateFrom.TabIndex = 4;
            // 
            // dateEditDateTo
            // 
            this.dateEditDateTo.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.bsSett, "DateTo", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.dateEditDateTo.EditValue = null;
            this.dateEditDateTo.Location = new System.Drawing.Point(467, 74);
            this.dateEditDateTo.Name = "dateEditDateTo";
            this.dateEditDateTo.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateEditDateTo.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateEditDateTo.Size = new System.Drawing.Size(213, 20);
            this.dateEditDateTo.StyleController = this.layoutControl1;
            this.dateEditDateTo.TabIndex = 5;
            // 
            // checkEditDoBatch
            // 
            this.checkEditDoBatch.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.bsSett, "DoBatch", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkEditDoBatch.Location = new System.Drawing.Point(136, 248);
            this.checkEditDoBatch.Name = "checkEditDoBatch";
            this.checkEditDoBatch.Properties.Caption = "";
            this.checkEditDoBatch.Size = new System.Drawing.Size(544, 19);
            this.checkEditDoBatch.StyleController = this.layoutControl1;
            this.checkEditDoBatch.TabIndex = 6;
            // 
            // checkEditInventorySync
            // 
            this.checkEditInventorySync.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.bsSett, "InventorySync", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkEditInventorySync.Location = new System.Drawing.Point(300, 15);
            this.checkEditInventorySync.Name = "checkEditInventorySync";
            this.checkEditInventorySync.Properties.Caption = "";
            this.checkEditInventorySync.Size = new System.Drawing.Size(62, 19);
            this.checkEditInventorySync.StyleController = this.layoutControl1;
            this.checkEditInventorySync.TabIndex = 8;
            this.checkEditInventorySync.CheckedChanged += new System.EventHandler(this.checkEditInventorySync_CheckedChanged);
            // 
            // checkEditPriceSync
            // 
            this.checkEditPriceSync.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.bsSett, "PriceSync", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkEditPriceSync.Location = new System.Drawing.Point(481, 15);
            this.checkEditPriceSync.Name = "checkEditPriceSync";
            this.checkEditPriceSync.Properties.Caption = "";
            this.checkEditPriceSync.Size = new System.Drawing.Size(211, 19);
            this.checkEditPriceSync.StyleController = this.layoutControl1;
            this.checkEditPriceSync.TabIndex = 9;
            this.checkEditPriceSync.CheckedChanged += new System.EventHandler(this.checkEditPriceSync_CheckedChanged);
            // 
            // checkEditOrderSync
            // 
            this.checkEditOrderSync.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.bsSett, "OrdersSync", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkEditOrderSync.Location = new System.Drawing.Point(124, 15);
            this.checkEditOrderSync.Name = "checkEditOrderSync";
            this.checkEditOrderSync.Properties.Caption = "";
            this.checkEditOrderSync.Size = new System.Drawing.Size(57, 19);
            this.checkEditOrderSync.StyleController = this.layoutControl1;
            this.checkEditOrderSync.TabIndex = 10;
            this.checkEditOrderSync.CheckedChanged += new System.EventHandler(this.checkEditOrderSync_CheckedChanged);
            // 
            // buttonEditBrandFilePath
            // 
            this.buttonEditBrandFilePath.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.bsSett, "BrandFilePath", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.buttonEditBrandFilePath.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bsSett, "BrandFilePath", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.buttonEditBrandFilePath.Location = new System.Drawing.Point(136, 188);
            this.buttonEditBrandFilePath.Name = "buttonEditBrandFilePath";
            this.buttonEditBrandFilePath.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.buttonEditBrandFilePath.Size = new System.Drawing.Size(544, 20);
            this.buttonEditBrandFilePath.StyleController = this.layoutControl1;
            this.buttonEditBrandFilePath.TabIndex = 7;
            this.buttonEditBrandFilePath.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.buttonEditBrandFilePath_ButtonClick);
            // 
            // spinEditPersentageOfCost
            // 
            this.spinEditPersentageOfCost.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.bsSett, "PercentageOfCost", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.spinEditPersentageOfCost.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.spinEditPersentageOfCost.Location = new System.Drawing.Point(136, 218);
            this.spinEditPersentageOfCost.Name = "spinEditPersentageOfCost";
            this.spinEditPersentageOfCost.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.spinEditPersentageOfCost.Size = new System.Drawing.Size(212, 20);
            this.spinEditPersentageOfCost.StyleController = this.layoutControl1;
            this.spinEditPersentageOfCost.TabIndex = 11;
            // 
            // checkEditConsiderMAPPrice
            // 
            this.checkEditConsiderMAPPrice.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.bsSett, "ConsiderMAPPrice", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkEditConsiderMAPPrice.Location = new System.Drawing.Point(467, 218);
            this.checkEditConsiderMAPPrice.Name = "checkEditConsiderMAPPrice";
            this.checkEditConsiderMAPPrice.Properties.Caption = "";
            this.checkEditConsiderMAPPrice.Size = new System.Drawing.Size(213, 19);
            this.checkEditConsiderMAPPrice.StyleController = this.layoutControl1;
            this.checkEditConsiderMAPPrice.TabIndex = 12;
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroup1.GroupBordersVisible = false;
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlGroupOrderSync,
            this.layoutControlGroupInvAndPriceSync,
            this.layoutControlItem5,
            this.layoutControlItem6,
            this.layoutControlItem7});
            this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup1.Name = "layoutControlGroup1";
            this.layoutControlGroup1.Size = new System.Drawing.Size(707, 406);
            this.layoutControlGroup1.TextVisible = false;
            // 
            // layoutControlGroupOrderSync
            // 
            this.layoutControlGroupOrderSync.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1,
            this.layoutControlItem2,
            this.emptySpaceItem1});
            this.layoutControlGroupOrderSync.Location = new System.Drawing.Point(0, 29);
            this.layoutControlGroupOrderSync.Name = "layoutControlGroupOrderSync";
            this.layoutControlGroupOrderSync.Size = new System.Drawing.Size(687, 114);
            this.layoutControlGroupOrderSync.Text = "Order Sync:";
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.dateEditDateFrom;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Padding = new DevExpress.XtraLayout.Utils.Padding(5, 5, 5, 5);
            this.layoutControlItem1.Size = new System.Drawing.Size(331, 30);
            this.layoutControlItem1.Text = "Date From:";
            this.layoutControlItem1.TextSize = new System.Drawing.Size(106, 13);
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.dateEditDateTo;
            this.layoutControlItem2.Location = new System.Drawing.Point(331, 0);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Padding = new DevExpress.XtraLayout.Utils.Padding(5, 5, 5, 5);
            this.layoutControlItem2.Size = new System.Drawing.Size(332, 30);
            this.layoutControlItem2.Text = "Date To:";
            this.layoutControlItem2.TextSize = new System.Drawing.Size(106, 13);
            // 
            // emptySpaceItem1
            // 
            this.emptySpaceItem1.AllowHotTrack = false;
            this.emptySpaceItem1.Location = new System.Drawing.Point(0, 30);
            this.emptySpaceItem1.Name = "emptySpaceItem1";
            this.emptySpaceItem1.Size = new System.Drawing.Size(663, 42);
            this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
            // 
            // layoutControlGroupInvAndPriceSync
            // 
            this.layoutControlGroupInvAndPriceSync.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.emptySpaceItem2,
            this.layoutControlItem4,
            this.layoutControlItem3,
            this.layoutControlItem8,
            this.layoutControlItem9});
            this.layoutControlGroupInvAndPriceSync.Location = new System.Drawing.Point(0, 143);
            this.layoutControlGroupInvAndPriceSync.Name = "layoutControlGroupInvAndPriceSync";
            this.layoutControlGroupInvAndPriceSync.Size = new System.Drawing.Size(687, 243);
            this.layoutControlGroupInvAndPriceSync.Text = "Inventory and Price Sync:";
            // 
            // emptySpaceItem2
            // 
            this.emptySpaceItem2.AllowHotTrack = false;
            this.emptySpaceItem2.Location = new System.Drawing.Point(0, 89);
            this.emptySpaceItem2.Name = "emptySpaceItem2";
            this.emptySpaceItem2.Size = new System.Drawing.Size(663, 112);
            this.emptySpaceItem2.TextSize = new System.Drawing.Size(0, 0);
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this.buttonEditBrandFilePath;
            this.layoutControlItem4.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Padding = new DevExpress.XtraLayout.Utils.Padding(5, 5, 5, 5);
            this.layoutControlItem4.Size = new System.Drawing.Size(663, 30);
            this.layoutControlItem4.Text = "Brand File Path:";
            this.layoutControlItem4.TextSize = new System.Drawing.Size(106, 13);
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this.checkEditDoBatch;
            this.layoutControlItem3.Location = new System.Drawing.Point(0, 60);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Padding = new DevExpress.XtraLayout.Utils.Padding(5, 5, 5, 5);
            this.layoutControlItem3.Size = new System.Drawing.Size(663, 29);
            this.layoutControlItem3.Text = "Do Batch:";
            this.layoutControlItem3.TextSize = new System.Drawing.Size(106, 13);
            // 
            // layoutControlItem8
            // 
            this.layoutControlItem8.Control = this.spinEditPersentageOfCost;
            this.layoutControlItem8.Location = new System.Drawing.Point(0, 30);
            this.layoutControlItem8.Name = "layoutControlItem8";
            this.layoutControlItem8.Padding = new DevExpress.XtraLayout.Utils.Padding(5, 5, 5, 5);
            this.layoutControlItem8.Size = new System.Drawing.Size(331, 30);
            this.layoutControlItem8.Text = "Percentage for MSRP:";
            this.layoutControlItem8.TextSize = new System.Drawing.Size(106, 13);
            // 
            // layoutControlItem9
            // 
            this.layoutControlItem9.Control = this.checkEditConsiderMAPPrice;
            this.layoutControlItem9.Location = new System.Drawing.Point(331, 30);
            this.layoutControlItem9.Name = "layoutControlItem9";
            this.layoutControlItem9.Padding = new DevExpress.XtraLayout.Utils.Padding(5, 5, 5, 5);
            this.layoutControlItem9.Size = new System.Drawing.Size(332, 30);
            this.layoutControlItem9.Text = "Consider MAP price:";
            this.layoutControlItem9.TextSize = new System.Drawing.Size(106, 13);
            // 
            // layoutControlItem5
            // 
            this.layoutControlItem5.Control = this.checkEditInventorySync;
            this.layoutControlItem5.Location = new System.Drawing.Point(176, 0);
            this.layoutControlItem5.Name = "layoutControlItem5";
            this.layoutControlItem5.Padding = new DevExpress.XtraLayout.Utils.Padding(5, 5, 5, 5);
            this.layoutControlItem5.Size = new System.Drawing.Size(181, 29);
            this.layoutControlItem5.Text = "Inventory Sync:";
            this.layoutControlItem5.TextSize = new System.Drawing.Size(106, 13);
            // 
            // layoutControlItem6
            // 
            this.layoutControlItem6.Control = this.checkEditPriceSync;
            this.layoutControlItem6.Location = new System.Drawing.Point(357, 0);
            this.layoutControlItem6.Name = "layoutControlItem6";
            this.layoutControlItem6.Padding = new DevExpress.XtraLayout.Utils.Padding(5, 5, 5, 5);
            this.layoutControlItem6.Size = new System.Drawing.Size(330, 29);
            this.layoutControlItem6.Text = "Price Sync:";
            this.layoutControlItem6.TextSize = new System.Drawing.Size(106, 13);
            // 
            // layoutControlItem7
            // 
            this.layoutControlItem7.Control = this.checkEditOrderSync;
            this.layoutControlItem7.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem7.Name = "layoutControlItem7";
            this.layoutControlItem7.Padding = new DevExpress.XtraLayout.Utils.Padding(5, 5, 5, 5);
            this.layoutControlItem7.Size = new System.Drawing.Size(176, 29);
            this.layoutControlItem7.Text = "Order Sync:";
            this.layoutControlItem7.TextSize = new System.Drawing.Size(106, 13);
            // 
            // ucExtSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.layoutControl1);
            this.Name = "ucExtSettings";
            this.Size = new System.Drawing.Size(721, 419);
            this.Load += new System.EventHandler(this.ucExtSettings_Load);
            ((System.ComponentModel.ISupportInitialize)(this.bsSett)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dateEditDateFrom.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditDateFrom.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditDateTo.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditDateTo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEditDoBatch.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEditInventorySync.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEditPriceSync.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEditOrderSync.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.buttonEditBrandFilePath.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinEditPersentageOfCost.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEditConsiderMAPPrice.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroupOrderSync)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroupInvAndPriceSync)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem9)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem7)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.BindingSource bsSett;
        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraEditors.DateEdit dateEditDateFrom;
        private DevExpress.XtraEditors.DateEdit dateEditDateTo;
        private DevExpress.XtraEditors.CheckEdit checkEditDoBatch;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroupOrderSync;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroupInvAndPriceSync;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem2;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
        private DevExpress.XtraEditors.CheckEdit checkEditInventorySync;
        private DevExpress.XtraEditors.CheckEdit checkEditPriceSync;
        private DevExpress.XtraEditors.CheckEdit checkEditOrderSync;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem5;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem6;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem7;
        private DevExpress.XtraEditors.ButtonEdit buttonEditBrandFilePath;
        private DevExpress.XtraEditors.SpinEdit spinEditPersentageOfCost;
        private DevExpress.XtraEditors.CheckEdit checkEditConsiderMAPPrice;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem8;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem9;
    }
}