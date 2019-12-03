using System;
using System.Collections.Generic;
using DevExpress.XtraEditors;
using Insidefabric;
using WheelsScraper;

namespace Databox.Libs.Insidefabric
{
    public partial class ucExtSettings : XtraUserControl
    {
        public Func<string, List<Brand>> GetBrandsList { get; set; }

        List<Brand> BrandsList { get; set; }
        List<Brand> SelectedBrands { get; set; }

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
            set { _sett = value; /*if (_sett != null) RefreshBindings();*/ }
        }

        public ucExtSettings()
        {
            InitializeComponent();
        }

        public List<Brand> GetSelectedBrands()
        {
            List<Brand> selectedBrands = new List<Brand>();
            var selectedItems = brandsListBox.CheckedItems;

            foreach (var item in selectedItems)
            {
                var brand = BrandsList.Find(x => x.Name == item.ToString());
                if (brand != null)
                    selectedBrands.Add(brand);
            }
            return selectedBrands;
        }

        protected void RefreshBindings()
        {

        }

        private void FillBrands(string category)
        {
            SelectedBrands = new List<Brand>();
            BrandsList = GetBrandsList(category);
            brandsListBox.Items.Clear();

            foreach (Brand brand in BrandsList)
                brandsListBox.Items.Add(brand.Name);
        }

        private void ucExtSettings_Load(object sender, EventArgs e)
        {

        }

        private void checkAllButton_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < brandsListBox.Items.Count; i++)
            {
                brandsListBox.SetItemChecked(i, true);
            }
        }

        private void unchekAllButton_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < brandsListBox.Items.Count; i++)
            {
                brandsListBox.SetItemChecked(i, false);
            }
        }

        private void categoryCombobox_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillBrands(categoryCombobox.SelectedItem.ToString());
        }
    }
}
