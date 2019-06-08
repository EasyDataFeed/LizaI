using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using WheelsScraper;

namespace Databox.Libs.Turn14_EDF
{
    public partial class ucExtSettings : XtraUserControl
    {
        private const string FileDialogFilter = "Excel files (*.csv?) | *.csv?";

        public ExtSettings ExtSett
        {
            get
            {
                return (ExtSettings)Sett.SpecialSettings;
            }
        }

        ScraperSettings _sett;
        public ScraperSettings Sett
        {
            get { return _sett; }
            set { _sett = value; if (_sett != null) RefreshBindings(); }
        }
        public ucExtSettings()
        {
            InitializeComponent();
        }

        protected void RefreshBindings()
        {
            bsSett.DataSource = ExtSett;
            bsSett.ResetBindings(false);
        }

        private void buttonEditBrandFilePath_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            string filePath = GetFilePath();

            if (!string.IsNullOrEmpty(filePath))
                buttonEditBrandFilePath.Text = filePath;
            else
                throw new Exception("You must select file");

        }

        private string GetFilePath()
        {
            OpenFileDialog fileDiaog = new OpenFileDialog();
            fileDiaog.Filter = FileDialogFilter;
            if (fileDiaog.ShowDialog() == DialogResult.OK)
                return fileDiaog.FileName;
            else
                return null;
        }

        private void ucExtSettings_Load(object sender, EventArgs e)
        {
            CheckOrderSyncBoxes();
            CheckInventoryAndPriceBoxes();
        }

        private void checkEditOrderSync_CheckedChanged(object sender, EventArgs e)
        {
            CheckOrderSyncBoxes();
        }

        private void checkEditInventorySync_CheckedChanged(object sender, EventArgs e)
        {
            CheckInventoryAndPriceBoxes();
        }

        private void checkEditPriceSync_CheckedChanged(object sender, EventArgs e)
        {
            CheckInventoryAndPriceBoxes();
        }

        private void CheckOrderSyncBoxes()
        {
            layoutControlGroupOrderSync.Enabled = checkEditOrderSync.Checked;
        }

        private void CheckInventoryAndPriceBoxes()
        {
            if (checkEditPriceSync.Checked || checkEditInventorySync.Checked)
                layoutControlGroupInvAndPriceSync.Enabled = true;
            else if(!checkEditPriceSync.Checked && !checkEditInventorySync.Checked)
                layoutControlGroupInvAndPriceSync.Enabled = false;
        }
    }
}
