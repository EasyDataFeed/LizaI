using System;
using System.Collections.Generic;
using DevExpress.XtraEditors;
using WheelsScraper;
using DevExpress.XtraSplashScreen;
using System.Windows.Forms;
using PremierConnector;
using PremierConnector.JsonTypes;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Columns;
using System.Linq;
using System.Text.RegularExpressions;

namespace Databox.Libs.PremierConnector
{
    public partial class ucExtSettings : XtraUserControl
    {
        public Func<bool, List<InventoryItem>> DownloadPremierInventory;
        public Action<List<InventoryItem>> UpdateSCEInventory;
        public Func<DateTime, DateTime, List<ExtSalesOrder>> LoadSCEOrders;
        public Action<List<ExtSalesOrder>> SyncOrders;
        public Action<List<ExtSalesOrder>, List<InventoryItem>> checkPremierInventoryForOrders;
        public Action<List<ExtSalesOrder>> SubmitOrders;
        public Func<List<ExtSalesOrder>, bool> UpdateTrackingNumbers;

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
            get
            {
                return _sett;
            }
            set
            {
                _sett = value;
                if (_sett != null)
                {
                    RefreshBindings();
                }
            }
        }

        public ucExtSettings()
        {
            InitializeComponent();
        }

        private void ExecuteActionWithWaitForm<T>(Action a, string msg = null)
        {
            SplashScreenManager.ShowForm(null, typeof(T), true, true, false, 1000);
            bool isError = false;
            try
            {
                a();
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                isError = true;
            }
            finally
            {
                SplashScreenManager.CloseForm(false);
                if (!string.IsNullOrEmpty(msg))
                {
                    if (isError)
                    {
                        MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        MessageBox.Show(msg, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }

        protected void RefreshBindings()
        {
            bsSett.DataSource = ExtSett;
            bsSett.ResetBindings(false);
        }

        private void APIKeyTextEdit_EditValueChanged(object sender, EventArgs e)
        {
            ExtSett.PremierAPIKey = APIKeyTextEdit.Text;
        }

        private void FtpAddressTextEdit_EditValueChanged(object sender, EventArgs e)
        {
            ExtSett.InvFtpAddress = FtpAddressTextEdit.Text;
        }

        private void FtpLoginTextEdit_EditValueChanged(object sender, EventArgs e)
        {
            ExtSett.InvFtpLogin = FtpLoginTextEdit.Text;
        }

        private void FtpPasswordTextEdit_EditValueChanged(object sender, EventArgs e)
        {
            ExtSett.InvFtpPassword = FtpPasswordTextEdit.Text;
        }

        private void ShowPasswordCheckEdit_CheckedChanged(object sender, EventArgs e)
        {
            FtpPasswordTextEdit.Properties.PasswordChar = (ShowPasswordCheckEdit.Checked) ? '\0' : '*';
        }

        private void UpdateInventoryStartCheckEdit_CheckedChanged(object sender, EventArgs e)
        {
            ExtSett.UpdateInventory = UpdateInventoryStartCheckEdit.Checked;
        }

        private void UpdatePricesStartCheckEdit_CheckedChanged(object sender, EventArgs e)
        {
            ExtSett.UpdatePrices = UpdatePricesStartCheckEdit.Checked;
        }

        private void SubmitOrdersStartCheckEdit_CheckedChanged(object sender, EventArgs e)
        {
            ExtSett.SubmitOrders = SubmitOrdersStartCheckEdit.Checked;
        }

        private void UpdateTrackingNumbersCheckEdit_CheckedChanged(object sender, EventArgs e)
        {
            ExtSett.UpdateTrackingNumbers = UpdateTrackingNumbersCheckEdit.Checked;
        }

        private void OrdersGridView_MasterRowGetRelationCount(object sender, MasterRowGetRelationCountEventArgs e)
        {
            e.RelationCount = 2;
        }

        private void OrdersGridView_MasterRowGetRelationName(object sender, MasterRowGetRelationNameEventArgs e)
        {
            if (e.RelationIndex == 1)
            {
                e.RelationName = "Shipping Address";
            }
        }

        private void OrdersGridView_MasterRowGetChildList(object sender, MasterRowGetChildListEventArgs e)
        {
            if (e.RelationIndex == 1)
            {
                List<ShipToAddress> list = new List<ShipToAddress>();
                list.Add(((ExtSalesOrder)OrdersGridView.GetRow(e.RowHandle)).ShipToAddress);
                e.ChildList = list;
            }
        }

        private void OrdersGridView_MasterRowExpanded(object sender, CustomMasterRowEventArgs e)
        {
            if (e.RelationIndex == 0)
            {
                // disable editing on detail view
                GridView detailView = OrdersGridView.GetDetailView(e.RowHandle, 0) as GridView;
                foreach (GridColumn column in detailView.Columns)
                {
                    column.OptionsColumn.AllowEdit = false;
                }
            }
            else if (e.RelationIndex == 1)
            {
                // disable editing and change captions on detail view
                GridView detailView = OrdersGridView.GetDetailView(e.RowHandle, 1) as GridView;
                foreach (GridColumn column in detailView.Columns)
                {
                    column.OptionsColumn.AllowEdit = false;
                    switch (column.FieldName)
                    {
                        case "Name":
                            column.Caption = "Customer name";
                            break;
                        case "AddressLine1":
                            column.Caption = "Street Address 1";
                            break;
                        case "AddressLine2":
                            column.Caption = "Street Address 2";
                            break;
                        case "RegionCode":
                            column.Caption = "State";
                            break;
                        case "Phone":
                            column.Caption = "Phone Number";
                            break;
                    }
                }
            }
        }

        private void LoadPremierInventorySimpleButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ExtSett.InvFtpAddress))
            {
                MessageBox.Show("Please enter FTP Address", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (string.IsNullOrEmpty(ExtSett.InvFtpLogin))
            {
                MessageBox.Show("Please enter FTP Login", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (string.IsNullOrEmpty(ExtSett.InvFtpPassword))
            {
                MessageBox.Show("Please enter FTP Password", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                ExecuteActionWithWaitForm<PremierWaitForm>(loadPremierInventory);
                if (ExtSett.InventoryItems.Count > 0)
                {
                    UpdateSCEInventorySimpleButton.Enabled = true;
                    inventoryItemsBindingSource.ResetBindings(false);
                }
            }
        }

        private void loadPremierInventory()
        {
            ExtSett.InventoryItems.AddRange(DownloadPremierInventory(true));
        }

        private void UpdateSCEInventorySimpleButton_Click(object sender, EventArgs e)
        {
            ExecuteActionWithWaitForm<PremierWaitForm>(updateSCEInventory, "SCE Inventory updated!");
            UpdateSCEInventorySimpleButton.Enabled = false;
            ExtSett.InventoryItems.Clear();
            inventoryItemsBindingSource.ResetBindings(false);
        }

        private void updateSCEInventory()
        {
            UpdateSCEInventory(ExtSett.InventoryItems);
        }

        private void LoadSCEOrdersSimpleButton_Click(object sender, EventArgs e)
        {
            ExecuteActionWithWaitForm<PremierWaitForm>(loadSCEOrders);
            salesOrdersBindingSource.ResetBindings(false);
        }

        private void loadSCEOrders()
        {
            ExtSett.SalesOrders.Clear();
            ExtSett.SalesOrders.AddRange(LoadSCEOrders(ExtSett.DateFrom, ExtSett.DateTo));
            if (ExtSett.SalesOrders.Count > 0)
            {
                SyncSimpleButton.Enabled = true;
                SubmitOrdersSimpleButton.Enabled = false;
                UpdateTrackingNumbersSimpleButton.Enabled = false;
                ExtSett.TrackingStartDate = ExtSett.DateFrom;
            }
        }

        private void SyncSimpleButton_Click(object sender, EventArgs e)
        {
            ExecuteActionWithWaitForm<PremierWaitForm>(syncOrders);
            salesOrdersBindingSource.ResetBindings(false);
        }

        private void syncOrders()
        {
            if (ExtSett.InventoryItems == null || ExtSett.InventoryItems.Count == 0)
                loadPremierInventory();

            checkPremierInventoryForOrders(ExtSett.SalesOrders, ExtSett.InventoryItems);

            SyncOrders(ExtSett.SalesOrders);
            
            int submitCount = ExtSett.SalesOrders.Count(x => x.Submit);
            int foundInPremierCount = ExtSett.SalesOrders.Count(x => x.Message == "Found in Premier.");
            if (submitCount > 0)
            {
                SubmitOrdersSimpleButton.Enabled = true;
            }
            else
            {
                SubmitOrdersSimpleButton.Enabled = false;
            }
            if (foundInPremierCount > 0)
            {
                UpdateTrackingNumbersSimpleButton.Enabled = true;
            }
            else
            {
                UpdateTrackingNumbersSimpleButton.Enabled = false;
            }
        }

        private void SubmitOrdersSimpleButton_Click(object sender, EventArgs e)
        {
            ExecuteActionWithWaitForm<PremierWaitForm>(submitOrders, "Orders submitted!");
            salesOrdersBindingSource.ResetBindings(false);
        }

        private void submitOrders()
        {
            SubmitOrders(ExtSett.SalesOrders);
            loadSCEOrders();
            syncOrders();
        }

        private void UpdateTrackingNumbersSimpleButton_Click(object sender, EventArgs e)
        {
            ExecuteActionWithWaitForm<PremierWaitForm>(updateTrackingNumbers, (updatedTrackingNumbers) ? "Tracking numbers have been updated successfully." : "There are no new tracking numbers to update. Please try again later.");
            salesOrdersBindingSource.ResetBindings(false);
        }

        private bool updatedTrackingNumbers;
        private void updateTrackingNumbers()
        {
            updatedTrackingNumbers = UpdateTrackingNumbers(ExtSett.SalesOrders);
            loadSCEOrders();
            syncOrders();
        }

        private void EmailNotificationTextEdit_EditValueChanged(object sender, EventArgs e)
        {
            ExtSett.Emails.Clear();
            string[] emails = ((TextEdit)sender).Text.Split(';');
            string email;
            string regexPattern = @"[\w-]+@[\w-]+\.[\w-]+\.?[\w-]+";
            foreach (string emailEl in emails)
            {
                email = emailEl.Trim(' ');
                if (Regex.IsMatch(email, regexPattern) && Regex.Match(email, regexPattern).Value == email)
                {
                    ExtSett.Emails.Add(email);
                }
            }
        }

        private void rdTypeConnection_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rdTypeConnection.SelectedIndex == 0)
                ExtSett.PremierAPIURL = "http://api-test.premierwd.com/api/v5/";
            else
                ExtSett.PremierAPIURL = "https://api.premierwd.com/api/v5/";
        }

        private void ucExtSettings_Load(object sender, EventArgs e)
        {
            rdTypeConnection.SelectedIndex = 1;
            ExtSett.PremierAPIURL = "https://api.premierwd.com/api/v5/";
        }
    }
}
