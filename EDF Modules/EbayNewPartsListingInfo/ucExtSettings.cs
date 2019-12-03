using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using WheelsScraper;

namespace Databox.Libs.EbayNewPartsListingInfo
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
        }

        protected void RefreshBindings()
        {
            bsSett.DataSource = ExtSett;
            bsSett.ResetBindings(false);
        }

        protected string PickFile(string filter)
        {
            var ofd = new OpenFileDialog();
            ofd.Filter = filter;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                return ofd.FileName;
            }

            return null;
        }

        private void buttonEditEbayItemsFilepath_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            var file = PickFile("Excel files (*.csv?) | *.csv?");
            if (file != null)
                ((DevExpress.XtraEditors.ButtonEdit)(sender)).Text = file;
        }
        private void ucExtSettings_Load(object sender, EventArgs e)
        {
            radioGroupCompatibilityCheck.SelectedIndex = 1;
        }

        private void radioGroupCompatibilityCheck_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (radioGroupCompatibilityCheck.SelectedIndex != -1)
            {
                if (radioGroupCompatibilityCheck.Properties.Items[radioGroupCompatibilityCheck.SelectedIndex].Description == "Without Compatibility")
                {
                    ExtSett.VehicleCompatibility = false;
                }
                else
                {
                    ExtSett.VehicleCompatibility = true;
                }
            }
        }


    }
}
