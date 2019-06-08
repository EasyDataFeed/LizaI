using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraLayout.Utils;
using TerapeakVehicleMatching.Models;
using TerapeakVehicleMatching.Resources;
using WheelsScraper;

namespace Databox.Libs.TerapeakVehicleMatching
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
            layoutControlItem2.Visibility = LayoutVisibility.Never;
            layoutControlItem3.Visibility = LayoutVisibility.Never;
            layoutControlItem4.Visibility = LayoutVisibility.Never;
            layoutControlItem5.Visibility = LayoutVisibility.Never;

            InitializePropertiesForControls();
            InitializeImages();
            DoLoadBrands();
        }

        private void DoLoadBrands()
        {
            var brands = EbayCategories.Categories;
            ClearControls();

            listBoxControlPredifinedBrands.Items.AddRange(brands.ToArray());
            ExtSett.Brands = brands;
            RefreshBindings();
        }

        private void ClearControls()
        {
            listBoxControlPredifinedBrands.Items.Clear();
            listBoxControlBrandToScraping.Items.Clear();
            ExtSett.BrandsForScraping = new List<string>();
            ExtSett.Brands = new List<string>();
        }

        private void InitializePropertiesForControls()
        {
            listBoxControlPredifinedBrands.SortOrder = SortOrder.Ascending;
            listBoxControlBrandToScraping.SortOrder = SortOrder.Ascending;
        }

        private void InitializeImages()
        {
            simpleButtonMoveBrandToScrap.Image = Resources.right_arrow;
            simpleButtonMoveBrandToScrap.Text = String.Empty;
            simpleButtonMoveBrandToScrap.ImageLocation = ImageLocation.MiddleCenter;

            simpleButtonMoveBrandFromScrap.Image = Resources.left_arrow;
            simpleButtonMoveBrandFromScrap.Text = String.Empty;
            simpleButtonMoveBrandFromScrap.ImageLocation = ImageLocation.MiddleCenter;
        }

        private void simpleButtonMoveBrand_Click(object sender, EventArgs e)
        {
            if (listBoxControlPredifinedBrands.SelectedIndex != -1)
            {
                var selectedItem = listBoxControlPredifinedBrands.Items[listBoxControlPredifinedBrands.SelectedIndex];
                listBoxControlBrandToScraping.Items.Add(selectedItem);
                ExtSett.BrandsForScraping.Add((string)selectedItem);
                listBoxControlPredifinedBrands.Items.Remove(selectedItem);
                ExtSett.Brands.Remove((string)selectedItem);
            }
        }

        private void simpleButtonMoveBrandFromScrap_Click(object sender, EventArgs e)
        {
            if (listBoxControlBrandToScraping.SelectedIndex != -1)
            {
                var selectedItem = listBoxControlBrandToScraping.Items[listBoxControlBrandToScraping.SelectedIndex];
                listBoxControlPredifinedBrands.Items.Add(selectedItem);
                ExtSett.Brands.Add((string)selectedItem);
                listBoxControlBrandToScraping.Items.Remove(selectedItem);
                ExtSett.BrandsForScraping.Remove((string)selectedItem);
            }
        }
    }
}
