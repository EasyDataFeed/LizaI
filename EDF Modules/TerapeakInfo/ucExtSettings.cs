using System;
using System.Collections.Generic;
using System.Linq;
using DevExpress.XtraEditors;
using WheelsScraper;

namespace Databox.Libs.TerapeakInfo
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

                    checkEdit1.Enabled = true;
                    ExtSett.FullDescription = false;
                }
                else
                {
                    ExtSett.VehicleCompatibility = true;

                    checkEdit1.Enabled = false;
                    ExtSett.FullDescription = false;
                }
            }

            RefreshBindings();
        }
    }
}
