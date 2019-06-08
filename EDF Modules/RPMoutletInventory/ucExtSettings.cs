using System;
using System.Collections.Generic;
using System.Linq;
using DevExpress.XtraEditors;
using WheelsScraper;
using System.Drawing;
using System.Windows.Forms;
using RPMoutletInventory.Enums;

namespace Databox.Libs.RPMoutletInventory
{
    public partial class ucExtSettings : XtraUserControl
    {
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
            layoutControl4.Dock = DockStyle.Fill;
        }
        private void ucExtSettings_Load(object sender, EventArgs e)
        {
            CheckEnabledParts();            
        }

        private void CheckEnabledParts()
        {
            if (!checkEditUsePremier.Checked)
                layoutControlGroupPremier.Enabled = false;
            else
                layoutControlGroupPremier.Enabled = true;

            if (!checkEditUseTurn14.Checked)
                layoutControlGroupTurn14.Enabled = false;
            else
                layoutControlGroupTurn14.Enabled = true;

            if (!checkEditUseMeyer.Checked)
                layoutControlGroupMeyer.Enabled = false;
            else
                layoutControlGroupMeyer.Enabled = true;

            CheckEbay();

            RefreshBindings();
        }

        private void CheckEbay()
        {
            if (!ExtSett.UseMeyer && !ExtSett.UsePremier && !ExtSett.UseTurn14)
            {
                ExtSett.UseEbay = false;
                ExtSett.UpdatePriceInSce = false;
                ExtSett.UseEbay = false;
                checkEditUseEbay.Enabled = false;
            }
            else
                checkEditUseEbay.Enabled = true;

            RefreshBindings();
        }

        protected void RefreshBindings()
        {
            bsSett.DataSource = ExtSett;
            bsSett.ResetBindings(false);
        }

        private void radioGroupTurn14Inventory_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedIndex = ((RadioGroup)sender).SelectedIndex;
            ExtSett.Turn14InventoryType = (Turn14InventoryType)selectedIndex;
        }

        private void checkEditUsePremier_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkEditUsePremier.Checked)
                layoutControlGroupPremier.Enabled = false;
            else          
                layoutControlGroupPremier.Enabled = true;

            CheckEbay();
        }

        private void checkEditUseTurn14_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkEditUseTurn14.Checked)
                layoutControlGroupTurn14.Enabled = false;
            else
                layoutControlGroupTurn14.Enabled = true;

            CheckEbay();
        }

        private void checkEditUseMeyer_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkEditUseMeyer.Checked)
                layoutControlGroupMeyer.Enabled = false;
            else
                layoutControlGroupMeyer.Enabled = true;

            CheckEbay();
        }

        private void buttonEditBrandsFilePath_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDiaog = new OpenFileDialog();
            fileDiaog.Filter = "Excel files (*.csv?) | *.csv?";
            if (fileDiaog.ShowDialog() == DialogResult.OK)
                buttonEditBrandsFilePath.Text = fileDiaog.FileName;
        }
    }
}
