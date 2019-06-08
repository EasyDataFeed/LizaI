using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Dalessuperstore;
using DevExpress.XtraEditors;
using WheelsScraper;

namespace Databox.Libs.Dalessuperstore
{
    public partial class ucExtSettings : XtraUserControl
    {
        public Func<List<Filter>> GetListBrands { get; set; }

        private List<Filter> ListBrands { get; set; }

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
            layoutControl1.Dock = DockStyle.Fill;
        }

        public List<Filter> GetSelectBrands()
        {
            List<Filter> selectBrands = new List<Filter>();
            var chekedItems = checkedListBoxControlBrands.CheckedItems;

            foreach (var item in chekedItems)
            {
                var brand = ListBrands.Find(x => x.Name == item.ToString());
                if (brand != null)
                    selectBrands.Add(brand);
            }

            return selectBrands;
        }

        protected void RefreshBindings()
        {
        }

        private void ucExtSettings_Load(object sender, EventArgs e)
        {
            ListBrands = GetListBrands();

            checkedListBoxControlBrands.Items.Clear();
            foreach (var item in ListBrands)
                checkedListBoxControlBrands.Items.Add(item.Name);
        }
    }
}
