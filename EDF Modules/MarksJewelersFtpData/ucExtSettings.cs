using System;
using System.Collections.Generic;
using System.Linq;
using DevExpress.XtraEditors;
using DevExpress.XtraLayout.Utils;
using WheelsScraper;

namespace Databox.Libs.MarksJewelersFtpData
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
            layoutControlItem8.Visibility = LayoutVisibility.Never;
            layoutControlItem9.Visibility = LayoutVisibility.Never;
            layoutControlItem11.Visibility = LayoutVisibility.Never;
        }

        private void checkEditRosyBlue_CheckedChanged(object sender, EventArgs e)
        {
            if (checkEditRosyBlue.Checked == true)
            {
                ExtSett.DownloadRosyBlue = true;
            }
            else
            {
                ExtSett.DownloadRosyBlue = false;
            }
        }

        private void UploadInventorycheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (UploadInventorycheckBox.Checked == true)
            {
                ExtSett.UploadInventory = true;
            }
            else
            {
                ExtSett.UploadInventory = false;
            }
        }

       
    }
}
