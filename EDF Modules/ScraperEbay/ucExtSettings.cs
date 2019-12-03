using System;
using System.Collections.Generic;
using System.Linq;
using DevExpress.XtraEditors;
using WheelsScraper;

namespace Databox.Libs.ScraperEbay
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

        private void textBoxStoreIds_TextChanged(object sender, EventArgs e)
        {
            List<string> listKeywords = new List<string>();
            foreach (var item in textBoxStoreIds.Lines)
            {
                if (!listKeywords.Contains(item.Trim(), StringComparer.OrdinalIgnoreCase) && (item.Trim() != string.Empty))
                {
                    listKeywords.Add(item.Trim());
                }
            }
            ExtSett.SotreIdsToScrap = listKeywords;
        }
    }
}
