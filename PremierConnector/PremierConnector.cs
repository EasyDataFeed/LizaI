using System;
using System.Collections.Generic;
using System.Linq;
using PremierConnector;
using Databox.Libs.PremierConnector;
using System.Web.Script.Serialization;
using System.Net;
using System.Data;
using PremierConnector.sceAPI;
using PremierConnector.JsonTypes;
using System.Threading;
using System.IO;
using System.Text.RegularExpressions;
using Ionic.Zip;
using LumenWorks.Framework.IO.Csv;
using System.Net.Mail;
using PremierConnector.DataItems;
using PremierConnector.Helpers;

namespace WheelsScraper
{
    public class PremierConnector : BaseScraper
    {
        private string sessionToken;
        private string csvFilePath;

        private List<TransferInfoItem> TransferInfoItems { get; set; }
        public PremierConnector()
        {
            Name = "PremierConnector";
            Url = "https://premierwd.com";
            PageRetriever.Referer = Url;
            WareInfoList = new List<ExtWareInfo>();
            Wares.Clear();

            SpecialSettings = new ExtSettings();
        }

        private void setAuthorizationHeader(HttpWebRequest request, CookieContainer cookie)
        {
            string headerValue = string.Format("Bearer {0}", this.sessionToken);
            request.Headers.Add(HttpRequestHeader.Authorization, headerValue);
            request.Accept = "application/json";
            request.ContentType = "application/json";
        }

        private ExtSettings extSett
        {
            get
            {
                return (ExtSettings)Settings.SpecialSettings;
            }
        }

        public override Type[] GetTypesForXmlSerialization()
        {
            return new Type[] { typeof(ExtSettings) };
        }

        public override System.Windows.Forms.Control SettingsTab
        {
            get
            {
                var frm = new ucExtSettings();
                frm.Sett = Settings;
                frm.DownloadPremierInventory = downloadPremierInventory;
                frm.UpdateSCEInventory = updateSCEInventory;
                frm.LoadSCEOrders = loadSCEOrders;
                frm.SyncOrders = syncOrders;
                frm.SubmitOrders = submitSalesOrdersToPremier;
                frm.UpdateTrackingNumbers = updateTrackingNumbers;
                frm.checkPremierInventoryForOrders = checkPremierInventoryForOrders;
                return frm;
            }
        }

        public override WareInfo WareInfoType
        {
            get
            {
                return new ExtWareInfo();
            }
        }

        protected override bool Login()
        {
            string authenticateURL, json;
            JavaScriptSerializer jss;
            Dictionary<string, string> dict;

            if (string.IsNullOrEmpty(extSett.PremierAPIKey))
            {
                throw new Exception("Enter API Key on tab Special settings!");
            }
            else
            {
                // get session token
                authenticateURL = string.Format("{0}authenticate?apiKey={1}", extSett.PremierAPIURL, extSett.PremierAPIKey);
                json = PageRetriever.ReadFromServer(authenticateURL);
                jss = new JavaScriptSerializer();
                dict = jss.Deserialize<Dictionary<string, string>>(json);
                this.sessionToken = dict["sessionToken"];
                // add event to set header of authorization
                PageRetriever.AfterSetHeaders += setAuthorizationHeader;
                MessagePrinter.PrintMessage("Authentication Premier API done!");
                return true;
            }
        }

        protected override void RealStartProcess()
        {
            TransferInfoItems = new List<TransferInfoItem>();
            List<InventoryItem> premierInventory = null;

            if (extSett.UpdateInventory || extSett.UpdatePrices)
            {
                if (string.IsNullOrEmpty(extSett.InvFtpAddress))
                {
                    MessagePrinter.PrintMessage("Please enter FTP Address on tab \"Special settings\"", ImportanceLevel.High);
                    return;
                }
                else if (string.IsNullOrEmpty(extSett.InvFtpLogin))
                {
                    MessagePrinter.PrintMessage("Please enter FTP Login on tab \"Special settings\"", ImportanceLevel.High);
                    return;
                }
                else if (string.IsNullOrEmpty(extSett.InvFtpPassword))
                {
                    MessagePrinter.PrintMessage("Please enter FTP Password on tab \"Special settings\"", ImportanceLevel.High);
                    return;
                }
                else
                {
                    bool deleteFile = !extSett.UpdatePrices; // if update prices then don't delete inventory file
                    // update inventory
                    if (extSett.UpdateInventory)
                    {
                        premierInventory = downloadPremierInventory(deleteFile);
                        updateSCEInventory(premierInventory);
                    }
                    // update prices
                    if (extSett.UpdatePrices)
                    {
                        lstProcessQueue.Add(new ProcessQueueItem { ItemType = 1 });
                    }
                }
            }

            if (extSett.SubmitOrders || extSett.UpdateTrackingNumbers)
            {
                // load orders from SCE
                List<ExtSalesOrder> SCEOrders = loadSCEOrders(extSett.DateFrom, extSett.DateTo);
                if (SCEOrders.Count > 0) // if there are orders
                {
                    // sync orders
                    syncOrders(SCEOrders);
                    //try
                    //{
                    //    var a = PageRetriever.ReadFromServer("https://api.premierwd.com/api/v5/invoices/0005136577/lines");

                    //    var tracking = PageRetriever.ReadFromServer("https://api.premierwd.com/api/v5/tracking?invoiceNumber=0005136577");
                    //}
                    //catch (Exception e)
                    //{

                    //}


                    if (extSett.SubmitOrders)
                    {
                        if (premierInventory == null) premierInventory = downloadPremierInventory(true);

                        checkPremierInventoryForOrders(SCEOrders, premierInventory);

                        // submit orders to Premier
                        submitSalesOrdersToPremier(SCEOrders);
                    }
                    if (extSett.UpdateTrackingNumbers)
                    {
                        // update tracking numbers
                        updateTrackingNumbers(SCEOrders);
                    }
                }
            }

            StartOrPushPropertiesThread();
        }

        private List<ProductsPartNumber> getAllPartNumbersFromSCE()
        {
            MessagePrinter.PrintMessage("Loading part numbers from SCE. Please wait...");
            List<ProductsPartNumber> partNumbers = new List<ProductsPartNumber>();
            IEnumerable<DataRow> rows;
            int productType;
            var objsceApi = GetApi();
            DataSet data = objsceApi.GetProductsPartNumbers(0);
            for (int i = 0; i < data.Tables.Count; i++)
            {
                switch (data.Tables[i].TableName)
                {
                    case "Product":
                        productType = 1;
                        break;
                    case "Option":
                        productType = 2;
                        break;
                    case "Car":
                        productType = 3;
                        break;
                    case "Wheel":
                        productType = 6;
                        break;
                    case "Tire":
                        productType = 7;
                        break;
                    default:
                        productType = 1;
                        break;
                }
                rows = data.Tables[i].Select().Where(x => x["partNo"] != null);
                partNumbers.AddRange(rows.Select(x => new ProductsPartNumber()
                {
                    PartNumber = x["partNo"].ToString().ToUpper().Trim(),
                    ProductType = productType,
                    Prodid = int.Parse(x["prodID"].ToString())
                }
                ).Where(x => !string.IsNullOrEmpty(x.PartNumber)).ToArray());
            }
            MessagePrinter.PrintMessage("Part numbers loaded from SCE");
            return partNumbers.ToList();
        }

        private bool downloadFTPFile(string filePath, string ftpAddress, string user, string password)
        {
            try
            {
                FtpWebRequest requestFileDownload = (FtpWebRequest)WebRequest.Create(ftpAddress);
                requestFileDownload.Credentials = new NetworkCredential(user, password);
                requestFileDownload.Method = WebRequestMethods.Ftp.DownloadFile;

                FtpWebResponse responseFileDownload = (FtpWebResponse)requestFileDownload.GetResponse();

                Stream responseStream = responseFileDownload.GetResponseStream();
                FileStream writeStream = new FileStream(filePath, FileMode.Create);

                int Length = 2048;
                byte[] buffer = new byte[Length];
                int bytesRead = responseStream.Read(buffer, 0, Length);

                while (bytesRead > 0)
                {
                    writeStream.Write(buffer, 0, bytesRead);
                    bytesRead = responseStream.Read(buffer, 0, Length);
                }

                responseStream.Close();
                writeStream.Close();

                requestFileDownload = null;
                responseFileDownload = null;

                return true;
            }
            catch (Exception e)
            {
                MessagePrinter.PrintMessage(e.Message, ImportanceLevel.Critical);
            }

            return false;
        }
        public static string GetSettingsPath(string fileName)
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
        }

        private void downloadCsvFile()
        {
            try
            {
                // download zip file from FTP
                string zipFilePath = Regex.Replace(Settings.Location, @"[\w-%&]*.edf", "PremierItemExport.zip");
                string csvFileName = "StandardExport.csv";
                downloadFTPFile(zipFilePath, extSett.InvFtpAddress, extSett.InvFtpLogin, extSett.InvFtpPassword);
                if (File.Exists(zipFilePath))
                {
                    if (File.Exists(GetSettingsPath("StandardExport.csv.tmp")))
                        File.Delete(GetSettingsPath("StandardExport.csv.tmp"));

                    // extract zip
                    using (var zipArc = ZipFile.Read(zipFilePath))
                    {
                        zipArc[csvFileName].Extract(Path.GetDirectoryName(zipFilePath), ExtractExistingFileAction.OverwriteSilently);
                    }
                    // delete zip file
                    File.Delete(zipFilePath);
                    csvFilePath = string.Format("{0}\\{1}", Path.GetDirectoryName(zipFilePath), csvFileName);
                }
                else
                {
                    csvFilePath = string.Empty;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new Exception(e.ToString());
            }

        }

        private List<InventoryItem> downloadPremierInventory(bool deleteFile)
        {
            List<string> partNumbers = getAllPartNumbersFromSCE().Select(x => x.PartNumber).ToList();
            List<InventoryItem> inventoryItems = new List<InventoryItem>();
            InventoryItem inventoryItem;
            string[] warehouses;

            MessagePrinter.PrintMessage("Downloading inventory from FTP. Please wait...");
            // download and read csv file
            downloadCsvFile();
            if (!string.IsNullOrEmpty(csvFilePath))
            {
                using (var sr = File.OpenText(csvFilePath))
                {
                    using (var csv = new CsvReader(sr, true, '|'))
                    {
                        while (csv.ReadNextRecord())
                        {
                            if (partNumbers.Contains(csv["Part Number"].Trim().ToUpper()))
                            {
                                string partNumber = csv["Part Number"].Trim();
                                string brand = csv["Manufacturer Name"];
                                string inventoryType = csv["Inventory Type"];
                                bool discontinued = false || string.Equals(inventoryType, "Discontinued", StringComparison.OrdinalIgnoreCase);

                                // get inventory item
                                inventoryItem = new InventoryItem()
                                {
                                    PartNumber = partNumber,
                                    Quantity = 0,
                                    Discontinued = discontinued
                                };
                                warehouses = csv["Warehouse Availability"].TrimEnd(';').Split(';'); // get all warehouse lines
                                // sum values of warehouse lines to get quantity
                                foreach (string warehouseLine in warehouses)
                                {
                                    inventoryItem.Quantity += int.Parse(warehouseLine.Split(':')[1].Trim());
                                }
                                inventoryItems.Add(inventoryItem);

                                TransferInfoItem transferInfoItem = new TransferInfoItem();

                                transferInfoItem.PartNumberPremier = partNumber;
                                transferInfoItem.BrandPremier = brand;
                                int quantity = 0;
                                // sum values of warehouse lines to get quantity
                                foreach (string warehouseLine in warehouses)
                                {
                                    if (warehouseLine.Contains("Utah Warehouse"))
                                        transferInfoItem.PremierUtahWarehouse = warehouseLine.Split(':')[1].Trim();
                                    else if (warehouseLine.Contains("Kentucky Warehouse"))
                                        transferInfoItem.PremierKentuckyWarehouse = warehouseLine.Split(':')[1].Trim();
                                    else if (warehouseLine.Contains("Texas Warehouse"))
                                        transferInfoItem.PremierTexasWarehouse = warehouseLine.Split(':')[1].Trim();
                                    else if (warehouseLine.Contains("California Warehouse"))
                                        transferInfoItem.PremierCaliforniaWarehouse = warehouseLine.Split(':')[1].Trim();
                                    else if (warehouseLine.Contains("Calgary AB Warehouse"))
                                        transferInfoItem.PremierCalgaryABWarehouse = warehouseLine.Split(':')[1].Trim();
                                    else if (warehouseLine.Contains("Washington Warehouse"))
                                        transferInfoItem.PremierWashingtonWarehouse = warehouseLine.Split(':')[1].Trim();
                                    else if (warehouseLine.Contains("Ponoka AB Warehouse"))
                                        transferInfoItem.PremierPonokaABWarehouse = warehouseLine.Split(':')[1].Trim();
                                    else if (warehouseLine.Contains("Colorado Warehouse"))
                                        transferInfoItem.PremierColoradoWarehouse = warehouseLine.Split(':')[1].Trim();

                                    quantity += int.Parse(warehouseLine.Split(':')[1].Trim());
                                }

                                transferInfoItem.QuantityPremier = quantity.ToString();

                                TransferInfoItems.Add(transferInfoItem);
                            }
                        }
                    }
                }
                if (deleteFile)
                {
                    File.Delete(csvFilePath);
                    csvFilePath = string.Empty;
                }
                MessagePrinter.PrintMessage("Inventory downloaded!");
            }
            else
            {
                MessagePrinter.PrintMessage("Inventory not downloaded!", ImportanceLevel.Critical);
            }

            return inventoryItems;
        }

        private void updateSCEInventory(List<InventoryItem> inventoryItems)
        {
            List<StockItem> stockItems = new List<StockItem>();
            StockItem stockItem;

            foreach (InventoryItem inventoryItem in inventoryItems)
            {
                stockItem = new StockItem()
                {
                    PartNo = inventoryItem.PartNumber,
                    Qty = inventoryItem.Quantity,
                    SupplierName = "Premier"
                };
                stockItems.Add(stockItem);
            }

            MessagePrinter.PrintMessage("Updating SCE Inventory. Please wait...");

            ScraperHelper.CreatePremierInventoryFile(this, TransferInfoItems);

            #region Old Inventory

            //var objsceApi = GetApi();
            //StockItem[] results = objsceApi.UpdateInventory(stockItems.ToArray(), eStockImportType.OverwriteAll, false);
            //if (results != null)
            //{
            //    foreach (StockItem result in results)
            //    {
            //        if (result.Status == "Item Conflict")
            //        {
            //            MessagePrinter.PrintMessage(string.Format("Part Number #{0} has errors. Please check this product in SCE and try again.", result.PartNo), ImportanceLevel.Mid);
            //        }
            //    }
            //}

            #endregion

            MessagePrinter.PrintMessage("SCE Inventory updated!");
        }

        private List<ExtSalesOrder> loadSCEOrders(DateTime dateFrom, DateTime dateTo)
        {
            MessagePrinter.PrintMessage("Loading orders from SCE. Please wait...");
            List<ExtSalesOrder> salesOrders = new List<ExtSalesOrder>();
            ExtSalesOrder salesOrder;
            SalesOrderLine salesOrderLine;
            // get orders from SCE
            var objsceApi = GetApi();

            Order[] sceOrders = objsceApi.OrderSearch(dateFrom, dateTo, -1, -1, string.Empty, string.Empty, string.Empty, string.Empty,
               string.Empty, eOrderFilterShipping.NotShipped, eOrderFilterPayment.FullyCharged, string.Empty, string.Empty, string.Empty, string.Empty,
               string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, false, false, false);

            // prepare SCE orders to submit their to Premier
            foreach (Order sceOrder in sceOrders)
            {
                salesOrder = new ExtSalesOrder()
                {
                    Submit = false,
                    SceOrderID = sceOrder.OrderID,
                    Date = sceOrder.OrderDate,
                    SceCode = sceOrder.ShippingService.SceCode,
                    ShipCarrierTag = sceOrder.ShippingService.CarrierID,
                    ShippingCode = sceOrder.ShippingService.Code,
                    ShipToAddress = new ShipToAddress()
                    {
                        AddressLine1 = sceOrder.ShippingAddress.Address1,
                        AddressLine2 = sceOrder.ShippingAddress.Address2,
                        City = sceOrder.ShippingAddress.City,
                        CountryCode = sceOrder.ShippingAddress.CountryCode,
                        Name = (!String.IsNullOrEmpty(sceOrder.ShippingAddress.Company) ? sceOrder.ShippingAddress.Company + " " : "") + sceOrder.ShippingAddress.CustomerName,
                        Phone = string.IsNullOrEmpty(sceOrder.ShippingAddress.Phone) ? extSett.DefaultPhoneNumber : sceOrder.ShippingAddress.Phone,
                        PostalCode = sceOrder.ShippingAddress.Zip,
                        RegionCode = sceOrder.ShippingAddress.StateAbbr,
                    }
                };
                if (sceOrder.ShippingService.CarrierID == eShippingProvider.UPS)
                {
                    switch (sceOrder.ShippingService.SceCode)
                    {
                        case eOrderShipType.Ground:
                            salesOrder.ShipMethod = "UPS Ground";
                            break;
                        case eOrderShipType.NextDay:
                            salesOrder.ShipMethod = "UPS Next Day";
                            break;
                        case eOrderShipType.TwoDay:
                            salesOrder.ShipMethod = "UPS 2nd Day";
                            break;
                        case eOrderShipType.ThreeDay:
                            salesOrder.ShipMethod = "UPS 3 Day Select";
                            break;
                        case eOrderShipType.InternationalExp:
                            salesOrder.ShipMethod = "UPS Express International";
                            break;
                        default:
                            salesOrder.Message = string.Format("Used UPS {0} shipping service doesn't exist.", sceOrder.ShippingService.SceCode);
                            break;
                    }
                }
                else
                {
                    salesOrder.Message = "Incorrect shipping service! UPS carrier allowed only.";
                }
                salesOrder.SalesOrderLines = new List<SalesOrderLine>();
                foreach (OrderItem orderItem in sceOrder.OrderItems)
                {
                    if (orderItem.FiledWarehouse != "Premier Performance")
                    {
                        MessagePrinter.PrintMessage($"order - {sceOrder.OrderID} has {orderItem.PartNo} with {orderItem.FiledWarehouse} warehouse",ImportanceLevel.High);
                        continue;
                    }

                    salesOrderLine = new SalesOrderLine()
                    {
                        ItemNumber = orderItem.PartNo,
                        Note = orderItem.Notes,
                        Quantity = orderItem.Qty,
                        WarehouseCode = orderItem.FiledWarehouse
                    };
                    salesOrder.SalesOrderLines.Add(salesOrderLine);
                }

                if (sceOrder.OrderItems.Count() == salesOrder.SalesOrderLines.Count)
                    salesOrders.Add(salesOrder);
            }

            MessagePrinter.PrintMessage("Orders loaded from SCE!");
            return salesOrders;
        }

        private void checkPremierInventoryForOrders(List<ExtSalesOrder> orders, List<InventoryItem> inventoryItems)
        {
            if (orders == null || inventoryItems == null || inventoryItems.Count == 0)
                throw new Exception("Inventory cannot be empty!");

            foreach (var order in orders)
            {
                foreach (var item in order.SalesOrderLines)
                {
                    var invItem = inventoryItems.Find(x => x.PartNumber == item.ItemNumber);
                    if (invItem != null)
                    {
                        if (invItem.Discontinued)
                        {
                            order.Message = "Part Number Discontinued: " + item.ItemNumber;
                            order.Submit = false;
                            break;
                        }

                        if (item.Quantity <= invItem.Quantity || !extSett.CheckPremierExistingInventory) continue;

                        order.Message = "No inventory on Premier for Part Number: " + item.ItemNumber;
                        order.Submit = false;
                        break;
                    }
                    else
                    {
                        order.Message = "Not found product on Premier for Part Number: " + item.ItemNumber;
                        order.Submit = false;
                        break;
                    }
                }
            }
        }

        private void syncOrders(List<ExtSalesOrder> salesOrders)
        {
            MessagePrinter.PrintMessage("Synchronization...");
            if (string.IsNullOrEmpty(this.sessionToken))
            {
                Login();
            }

            string trackingURL, response;
            JavaScriptSerializer jss = new JavaScriptSerializer();
            //https://api.premierwd.com/api/v5/tracking/date?startDate=0001-01-01
            trackingURL = string.Format("{0}tracking/date?startDate={1}", extSett.PremierAPIURL, extSett.TrackingStartDate.ToString("yyyy-MM-dd"));
            //trackingURL = string.Format("{0}tracking/date?startDate={1}", extSett.PremierAPIURL, extSett.DateFrom.ToString("yyyy-MM-dd"));
            // get tracking
            response = PageRetriever.ReadFromServer(trackingURL);
            List<TrackingInfo> premierOrders = jss.Deserialize<List<TrackingInfo>>(response);
            // find matches
            TrackingInfo trackingInfo;
            foreach (ExtSalesOrder salesOrder in salesOrders)
            {
                trackingInfo = premierOrders.Find(x => x.CustomerPurchaseOrderNumber == string.Format("SCE-{0}", salesOrder.SceOrderID));
                if (trackingInfo == null)
                {
                    if (string.IsNullOrEmpty(salesOrder.Message))
                    {
                        salesOrder.Submit = true;
                        salesOrder.CustomerPurchaseOrderNumber = string.Format("SCE-{0}", salesOrder.SceOrderID);
                    }
                    else
                    {
                        salesOrder.Submit = false;
                    }
                }
                else
                {
                    salesOrder.Submit = false;
                    salesOrder.CustomerPurchaseOrderNumber = trackingInfo.CustomerPurchaseOrderNumber;
                    salesOrder.Message = "Found in Premier.";
                }
            }
        }

        private void submitSalesOrdersToPremier(List<ExtSalesOrder> salesOrders)
        {
            MessagePrinter.PrintMessage("Submitting orders to Premier. Please wait...");
            string postData, salesOrderURL, response;
            salesOrderURL = string.Format("{0}sales-orders/", extSett.PremierAPIURL);
            int submittedCount = 0;
            JavaScriptSerializer jss = new JavaScriptSerializer();
            // Email Subject
            string subject = "The Premier Order(s)";
            // Email Body
            string body = "The Shopping Cart Elite order(s)";
            foreach (ExtSalesOrder salesOrder in salesOrders)
            {
                try
                {
                    if (salesOrder.Submit && string.IsNullOrEmpty(salesOrder.Message))
                    {
                        postData = jss.Serialize(salesOrder).Replace(":null", ":\"\"");
                        postData = Regex.Replace(postData, "\"WarehouseCode\":\"\"", string.Empty).Replace(",,", ",");
                        response = PageRetriever.WriteToServer(salesOrderURL, postData, false, true, true);
                        if (response.Contains(salesOrder.CustomerPurchaseOrderNumber))
                        {
                            MessagePrinter.PrintMessage(string.Format("Order #{0} was successfully submitted to Premier.", salesOrder.SceOrderID));
                            subject += string.Format(" #{0},", salesOrder.SceOrderID);
                            body += string.Format(" #{0},", salesOrder.SceOrderID);
                            submittedCount++;
                        }
                        else
                        {
                            MessagePrinter.PrintMessage(string.Format("There's an issue with Order #{0} and it wasn't submitted to Priemier." +
                                " Please check this order in Shopping Cart Elite and try again.", salesOrder.SceOrderID), ImportanceLevel.Mid);
                        }
                        Thread.Sleep(1000); // delay
                    }
                    else if (salesOrder.Message.Contains("No inventory on Premier") || salesOrder.Message.Contains("Not found product on Premier"))
                    {
                        MessagePrinter.PrintMessage(String.Format("Order #{0} not send to Premier. " + salesOrder.Message, salesOrder.SceOrderID), ImportanceLevel.Mid);
                    }
                }
                catch (WebException ex)
                {
                    var webResponse = ex.Response as HttpWebResponse;
                    if (webResponse != null)
                    {
                        using (var sr = new StreamReader(webResponse.GetResponseStream()))
                        {
                            MessagePrinter.PrintMessage("Order #" + salesOrder.SceOrderID + " not send to Premier. Response: " + sr.ReadToEnd(), ImportanceLevel.High);
                        }
                    }
                    else
                    {
                        throw new Exception(ex.Message, ex);
                    }
                }
            }
            if (submittedCount > 0 && extSett.Emails.Count > 0)
            {
                subject = subject.TrimEnd(',');
                body = body.TrimEnd(',') + " was successfully submitted to Premier supplier.";
                // send email notification
                sendEmail(extSett.Emails, subject, body);
            }
            MessagePrinter.PrintMessage("Orders processed");
        }

        private void sendEmail(List<string> recipients, string subject, string body)
        {
            // create mail message
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("sales@anythingtrucks.com");
            foreach (string address in recipients)
            {
                mailMessage.To.Add(address);
            }
            mailMessage.Subject = subject;
            mailMessage.Body = body;
            // initialize client
            SmtpClient smtp = new SmtpClient
            {
                Host = "smtp.shoppingcartelite.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(mailMessage.From.Address, "admin~letmein22")
            };
            // try to send message
            try
            {
                smtp.Send(mailMessage);
            }
            catch (Exception ex)
            {
                MessagePrinter.PrintMessage(string.Format("EDF failed to send email notification.\nException message: {0}", ex.Message),
                    ImportanceLevel.Mid);
            }
            finally
            {
                smtp.Dispose();
                mailMessage.Dispose();
            }
        }

        private bool updateTrackingNumbers(List<ExtSalesOrder> sceOrders)
        {
            MessagePrinter.PrintMessage("Updating tracking numbers in SCE. Please wait...");
            if (string.IsNullOrEmpty(this.sessionToken))
            {
                Login();
            }

            ExtSalesOrder sceOrder;
            OrderItemShipping orderItemShipping;
            string trackingURL, response, trackingNumbers;
            bool updatedTrackingNumbers = false;
            JavaScriptSerializer jss = new JavaScriptSerializer();
            var objsceApi = GetApi();
            trackingURL = string.Format("{0}tracking/date?startDate={1}", extSett.PremierAPIURL, extSett.TrackingStartDate.ToString("yyyy-MM-dd"));
            // get tracking
            response = PageRetriever.ReadFromServer(trackingURL);
            List<TrackingInfo> premierOrders = jss.Deserialize<List<TrackingInfo>>(response);
            // find matches and update tracking numbers
            foreach (TrackingInfo premierOrder in premierOrders)
            {
                sceOrder = sceOrders.Find(x => x.CustomerPurchaseOrderNumber == premierOrder.CustomerPurchaseOrderNumber);
                if (sceOrder != null)
                {
                    trackingNumbers = string.Empty;
                    foreach (Tracking tracking in premierOrder.Tracking)
                    {
                        if (!string.IsNullOrEmpty(tracking.TrackingNumber))
                        {
                            trackingNumbers += string.Format("{0}, ", tracking.TrackingNumber);
                        }
                    }
                    trackingNumbers = trackingNumbers.Trim().TrimEnd(',');
                    if (!string.IsNullOrEmpty(trackingNumbers))
                    {
                        orderItemShipping = new OrderItemShipping()
                        {
                            orderID = sceOrder.SceOrderID,
                            ShipDt = DateTime.Today,
                            SceCode = sceOrder.SceCode,
                            ShipCarrierTag = sceOrder.ShipCarrierTag,
                            ShippingCode = sceOrder.ShippingCode,
                            TrackingNo = trackingNumbers
                        };
                        try
                        {
                            objsceApi.UpdateTrackingNo(orderItemShipping, false);
                            updatedTrackingNumbers = true;
                        }
                        catch (Exception ex)
                        {
                            string message = string.Format("Premier Sales Order Number #{0}\nTracking numbers update is failed.\n{1}",
                                premierOrder.SalesOrderNumber, ex.Message);
                            throw new Exception(message);
                        }
                    }
                }
            }
            if (updatedTrackingNumbers)
            {
                MessagePrinter.PrintMessage("Tracking numbers have been updated successfully.");
            }
            else
            {
                MessagePrinter.PrintMessage("There are no new tracking numbers to update. Please try again later.", ImportanceLevel.Mid);
            }
            return updatedTrackingNumbers;
        }

        protected void ProcessUpdatePrices(ProcessQueueItem pqi)
        {
            if (cancel)
                return;

            if (string.IsNullOrEmpty(csvFilePath))
            {
                // download inventory file
                downloadCsvFile();
            }

            if (!string.IsNullOrEmpty(csvFilePath) && File.Exists(csvFilePath)) // if inventory file exists
            {
                List<ProductsPartNumber> scePartNumbers = getAllPartNumbersFromSCE();
                // get prices from CSV file
                MessagePrinter.PrintMessage(string.Format("Reading prices from {0} Please wait...", csvFilePath));
                List<ProductsPartNumber> matchedPartNumbers;
                ExtWareInfo wi;
                using (var sr = File.OpenText(csvFilePath))
                {
                    using (var csv = new CsvReader(sr, true, '|'))
                    {
                        while (csv.ReadNextRecord())
                        {
                            matchedPartNumbers = scePartNumbers.FindAll(x => x.PartNumber == csv["Part Number"].ToUpper().Trim());
                            if (matchedPartNumbers.Count > 0)
                            {
                                foreach (ProductsPartNumber productsPartNumber in matchedPartNumbers)
                                {
                                    wi = new ExtWareInfo()
                                    {
                                        PartNumber = "@" + productsPartNumber.PartNumber,
                                        Action = "update",
                                        Prodid = productsPartNumber.Prodid,
                                        ProductType = productsPartNumber.ProductType,
                                        Jobber = csv["JobberPrice"].Trim(),
                                        MSRP = double.Parse(csv["MSRP"].Trim()),
                                        Cost = double.Parse(csv["Map"].Trim()),
                                        WebPrice = double.Parse(csv["Your Price"].Trim())
                                    };
                                    AddWareInfo(wi);
                                    OnItemLoaded(wi);
                                }
                            }
                        }
                    }
                }
                // delete inventory file
                File.Delete(csvFilePath);
                csvFilePath = string.Empty;
                MessagePrinter.PrintMessage("Prices processed");
            }
            else
            {
                MessagePrinter.PrintMessage("Prices will not be updated in SCE. Inventory file not found...", ImportanceLevel.Critical);
            }

            pqi.Processed = true;
            StartOrPushPropertiesThread();
        }

        protected override Action<ProcessQueueItem> GetItemProcessor(ProcessQueueItem item)
        {
            Action<ProcessQueueItem> act;
            if (item.ItemType == 1)
            {
                act = ProcessUpdatePrices;
            }
            else
            {
                act = null;
            }

            return act;
        }

        protected sceApi GetApi()
        {
            string Access_key = Settings.SCEAccessKey;
            string Api_key = Settings.SCEAPIKey;
            string Secret_key = Settings.SCEAPISecret;

            AuthHeaderAPI objAuthHeader;
            sceApi objsceApi;

            objAuthHeader = new AuthHeaderAPI();
            objsceApi = new sceApi();
            objsceApi.AuthHeaderAPIValue = objAuthHeader;
            objAuthHeader.MustUnderstand = true;

            objsceApi.Timeout = 100 * 60 * 1000;
            objsceApi.UseDefaultCredentials = false;
            objAuthHeader.ApiAccessKey = Access_key;
            objAuthHeader.ApiKey = Api_key;
            objAuthHeader.ApiSecretKey = Secret_key;
            objAuthHeader.StoreID = "";

            return objsceApi;
        }
    }
}
