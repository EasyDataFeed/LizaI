#region using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraLayout.Utils;
using DevExpress.XtraSplashScreen;
using Turn14Connector.DataItems;
using Turn14Connector.DataItems.Turn14;
using Turn14Connector.Resources;
using Turn14Connector.Views;
using WheelsScraper;

#endregion

namespace Databox.Libs.Turn14Connector
{
    public partial class ucExtSettings : XtraUserControl
    {

        #region Constants

        private const string FileDialogFilter = "Excel files (*.csv?) | *.csv?";

        #endregion

        #region Actions and Funcs

        public Func<List<OrderSync>> LoadOrders { get; set; }
        public Func<BrandsJson> LoadTurn14Brands { get; set; }
        public Action<DateTime, DateTime> SyncOrders { get; set; }
        public Action<DateTime, DateTime, List<OrderSync>> ImportOrders { get; set; }
        #endregion

        #region Standart Props and Methods

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
            //автоматически масштабируемый интерфейс
            layoutControlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            //var pg = tabbedControlGroup1.TabPages.FirstOrDefault(p => p.Text == "Scraper Setting");

            DevExpress.Data.CurrencyDataController.DisableThreadingProblemsDetection = true;
        }

        private void HideConnectorSettings()
        {
            tabbedControlGroup1.TabPages[0].Visibility = LayoutVisibility.Never; // Connector
            ExtSett.OrdersSync = false;
            ExtSett.InventorySync = false;
            ExtSett.PriceSync = false;
        }

        private void HideScraperSettings()
        {
            tabbedControlGroup1.TabPages[1].Visibility = LayoutVisibility.Never; // Scraper Setting
        }

        protected void RefreshBindings()
        {
            //обязательные 2 строчки для того чтоб на интерфейсе работали датабайнинги
            bsSett.DataSource = ExtSett;
            bsSett.ResetBindings(false);
        }

        #endregion        

        #region Connector Settings

        private void simpleButtonLoadOrders_Click(object sender, EventArgs e)
        {
            ExecuteActionWithWaitFormOfType(DoLoadOrders, typeof(OrderWaitForm));
        }

        private void simpleButtonSyncOrders_Click(object sender, EventArgs e)
        {
            ExecuteActionWithWaitFormOfType(DoSyncOrders, typeof(OrderWaitForm));
            ExecuteActionWithWaitFormOfType(DoImportOrders, typeof(OrderWaitForm));
        }

        public void AutoSyncOrders()
        {
            DoLoadOrders();
            DoSyncOrders();
            DoImportOrders();
        }

        private void DoLoadOrders()
        {
            List<OrderSync> orders = LoadOrders();
            ExtSett.OrdersToSync.Clear();
            ExtSett.OrdersToSync.AddRange(orders);
            RefreshBindings();
        }

        private void DoSyncOrders()
        {
            SyncOrders(ExtSett.DateTo.AddDays(-3), ExtSett.DateTo);
        }

        private void DoImportOrders()
        {
            ImportOrders(ExtSett.DateFrom, ExtSett.DateTo, ExtSett.OrdersToSync);

            List<OrderSync> impotedOrders = new List<OrderSync>();
            foreach (OrderSync orderSync in ExtSett.OrdersToSync)
            {
                if (orderSync.Imported)
                    impotedOrders.Add(orderSync);
            }

            foreach (OrderSync importedOrder in impotedOrders)
            {
                ExtSett.OrdersToSync.Remove(importedOrder);
            }

            RefreshBindings();
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

        #endregion

        #region On Load

        private void ucExtSettings_Load(object sender, EventArgs e)
        {
            InitializePropertiesForControls();
            InitializeImages();
            CheckOrderSyncBoxes();
            CheckInventoryAndPriceBoxes();

            HideScraperSettings();
            //HideConnectorSettings();
        }

        private void InitializePropertiesForControls()
        {
            listBoxControlBrandToScraping.SortOrder = SortOrder.Ascending;
            listBoxControlTurn14Brands.SortOrder = SortOrder.Ascending;
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

        private void CheckInventoryAndPriceBoxes()
        {
            if (checkEditPriceSync.Checked || checkEditInventorySync.Checked)
                layoutControlGroup2.Enabled = true;
            else if (!checkEditPriceSync.Checked && !checkEditInventorySync.Checked)
                layoutControlGroup2.Enabled = false;
        }

        private void CheckOrderSyncBoxes()
        {
            layoutControlGroup3.Enabled = checkEditOrderSync.Checked;
        }

        #endregion

        #region Wait Form

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

        #endregion

        #region Scraper Setting

        private void simpleButtonLoadBrands_Click(object sender, EventArgs e)
        {
            ExecuteActionWithWaitFormOfType(DoLoadTurn14Brands, typeof(OrderWaitForm));
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

        #endregion
    }
}
