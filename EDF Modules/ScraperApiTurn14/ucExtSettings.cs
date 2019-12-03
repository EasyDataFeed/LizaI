using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraSplashScreen;
using ScraperApiTurn14.DataItems.Turn14;
using ScraperApiTurn14.Resources;
using Turn14ApiScraper.Views;
using WheelsScraper;

namespace Databox.Libs.ScraperApiTurn14
{
    public partial class ucExtSettings : XtraUserControl
    {
        private const string FileDialogFilter = "Excel files (*.csv?) | *.csv?";
        public Func<BrandsJson> LoadTurn14Brands { get; set; }

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
            InitializeImages();
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

        private void DoLoadTurn14Brands()
        {
            var brands = LoadTurn14Brands();
            ClearControls();

            listBoxControlTurn14Brands.Items.AddRange(brands.data.ToArray());
            ExtSett.Turn14Brands = brands;
            RefreshBindings();
        }

        private void ClearControls()
        {
            listBoxControlTurn14Brands.Items.Clear();
            listBoxControlBrandToScraping.Items.Clear();
            ExtSett.BrandsForScraping = new BrandsJson();
            ExtSett.Turn14Brands = new BrandsJson();
        }

        private void ExecuteActionWithWaitFormOfType(Action a, Type type)
        {
            SplashScreenManager.ShowForm(null, type, true, true, false, 1000);
            try
            {
                a();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ex.StackTrace);
                throw new Exception(ex.Message + ex.StackTrace);
            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }
        }

        private void simpleButtonLoadBrands_Click(object sender, EventArgs e)
        {
            ExecuteActionWithWaitFormOfType(DoLoadTurn14Brands, typeof(OrderWaitForm));
        }

        private void simpleButtonMoveBrand_Click(object sender, EventArgs e)
        {
            if (listBoxControlTurn14Brands.SelectedIndex != -1)
            {
                var selectedItem = listBoxControlTurn14Brands.Items[listBoxControlTurn14Brands.SelectedIndex];
                listBoxControlBrandToScraping.Items.Add(selectedItem);
                ExtSett.BrandsForScraping.data.Add((dataBrands)selectedItem);
                listBoxControlTurn14Brands.Items.Remove(selectedItem);
                ExtSett.Turn14Brands.data.Remove((dataBrands)selectedItem);
            }
        }

        private void simpleButtonMoveBrandFromScrap_Click(object sender, EventArgs e)
        {
            if (listBoxControlBrandToScraping.SelectedIndex != -1)
            {
                var selectedItem = listBoxControlBrandToScraping.Items[listBoxControlBrandToScraping.SelectedIndex];
                listBoxControlTurn14Brands.Items.Add(selectedItem);
                ExtSett.Turn14Brands.data.Add((dataBrands)selectedItem);
                listBoxControlBrandToScraping.Items.Remove(selectedItem);
                ExtSett.BrandsForScraping.data.Remove((dataBrands)selectedItem);
            }
        }

        private void buttonEditVehicleInfoForFitments_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            string filePath = GetFilePath();

            if (!string.IsNullOrEmpty(filePath))
                buttonEditVehicleInfoForFitments.Text = filePath;
            else
                throw new Exception("You must select file");
        }
    }
}
