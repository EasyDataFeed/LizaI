#region using

using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.Script.Serialization;
using HtmlAgilityPack;
using Turn14_EDF;
using Databox.Libs.Turn14_EDF;
using Ionic.Zip;
using LumenWorks.Framework.IO.Csv;
using Turn14_EDF.DateItems;
using Turn14_EDF.Helpers;
using Turn14_EDF.SCEapi;
using Microsoft.VisualBasic.FileIO;

#endregion

namespace WheelsScraper
{
    public class Turn14_EDF : BaseScraper
    {
        private const string TempFileNameFTP = "Turn14AddedOrders.json";
        private List<Turn14Order> Turn14AddedOrders;

        public Turn14_EDF()
        {
            Name = "Turn14_EDF";
            Url = "https://www.turn14.com/";
            PageRetriever.Referer = Url;
            WareInfoList = new List<ExtWareInfo>();
            Wares.Clear();
            BrandItemType = 2;

            SpecialSettings = new ExtSettings();
            Complete += Turn14_EDF_Complete;
        }

        private void Turn14_EDF_Complete(object sender, EventArgs e)
        {
            if (Turn14AddedOrders != null && Turn14AddedOrders.Count > 0)
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                string filePath = Regex.Replace(Settings.Location, @"[\w-%&]*.edf", TempFileNameFTP);
                string ftpLogin = Settings.FtpUsername;
                string ftpPassword = Settings.FtpPassword;
                string ftpAddress = Settings.FtpAddress;

                string json = jss.Serialize(Turn14AddedOrders);
                File.WriteAllText(filePath, json);
                MessagePrinter.PrintMessage($"Uploading {TempFileNameFTP} to FTP. Please wait...");
                try
                {
                    FtpHelper.UploadFileToFtp(ftpAddress, ftpLogin, ftpPassword, TempFileNameFTP, filePath);
                    MessagePrinter.PrintMessage($"File {TempFileNameFTP} uploaded to FTP!");
                    File.Delete(filePath);
                }
                catch (Exception ex)
                {
                    MessagePrinter.PrintMessage(ex.Message);
                    MessagePrinter.PrintMessage($"File {filePath} saved, but not uploaded to FTP!");
                }
            }
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
            MessagePrinter.PrintMessage("Starting login...");
            var loginInfo = GetLoginInfo();
            if (loginInfo == null)
            {
                throw new Exception("No valid login found");
            }

            string loginURL = $"{this.Url}user/login";
            string login = HttpUtility.UrlEncode(loginInfo.Login);
            string password = HttpUtility.UrlEncode(loginInfo.Password);

            string postData = $"username={login}&password={password}";
            string html = PageRetriever.WriteToServer(loginURL, postData, true);

            if (html.Contains("LOGOUT"))
            {
                MessagePrinter.PrintMessage("Login done!");
                return true;
            }
            else
            {
                MessagePrinter.PrintMessage("Login failed!");
                return false;
            }
        }

        protected override void RealStartProcess()
        {
            if (extSett.InventorySync || extSett.PriceSync)
            {
                if (string.IsNullOrEmpty(extSett.BrandFilePath))
                {
                    MessagePrinter.PrintMessage("You must choice file with brands", ImportanceLevel.Critical);
                    return;
                }

                extSett.BrandsAlignmentsItems = new List<BrandsAlignment>();
                extSett.BrandsAlignmentsItems = FileHelper.ReadBrandsAlignments(extSett.BrandFilePath);

                MessagePrinter.PrintMessage($"Download SCE export.");

                string sceExportFilePath = SceApiHelper.LoadProductsExport(Settings, extSett);

                MessagePrinter.PrintMessage($"SCE export downloaded");
                extSett.SceExportItems = FileHelper.ReadSceExport(sceExportFilePath);
                extSett.SceExportItems = extSett.SceExportItems.Distinct(new SceExportItem()).ToList();

                foreach (SceExportItem sceItem in extSett.SceExportItems)
                {
                    foreach (BrandsAlignment brandsAlignment in extSett.BrandsAlignmentsItems)
                    {
                        if (sceItem.Brand.ToLower() == brandsAlignment.BrandInSce.ToLower())
                        {
                            extSett.TransferInfoItems.Add(new TransferInfoItem()
                            {
                                ProdIdSce = sceItem.ProdId,
                                ProdTypeSce = sceItem.ProductType,
                                BrandSce = sceItem.Brand,
                                PartNumberSce = sceItem.PartNumber,
                                ManufacturerPartNumberSce = sceItem.ManufacturerPartNumber,
                                BrandTurn14 = brandsAlignment.BrandInTurn14,
                            });
                        }
                    }
                }

                if (File.Exists(sceExportFilePath))
                    File.Delete(sceExportFilePath);

                Turn14Inventory();
            }

            if (extSett.OrdersSync)
            {
                var sceOrders = SceApiHelper.LoadSceOrders(Settings, extSett.DateFrom, extSett.DateTo);
                if (sceOrders.Length > 0)
                {
                    GetTurn14AddedOrders();
                    MessagePrinter.PrintMessage($"{sceOrders.Length} - Orders in SCE found!");
                    // 3523

                    foreach (var order in sceOrders)
                    {

                        //MessagePrinter.PrintMessage($"Check inventory for order {order.OrderID}");

                        //int counterItems = 0;

                        //foreach (OrderItem orderItem in order.OrderItems)
                        //{
                        //    bool flagFound = false;
                        //    string partInOrder = orderItem.PartNo;
                        //    foreach (TransferInfoItem transferInfoItem in extSett.TransferInfoItems)
                        //    {
                        //        if (orderItem.PartNo == transferInfoItem.PartNumberSce)
                        //        {
                        //            orderItem.PartNo = transferInfoItem.PartNumberTurn14;

                        //            if (orderItem.Qty > int.Parse(transferInfoItem.QuantityTurn14))
                        //            {
                        //                MessagePrinter.PrintMessage($"for this pn - {transferInfoItem.PartNumberSce} not enough stock on turn14", ImportanceLevel.High);
                        //                flagFound = true;
                        //                break;
                        //            }
                        //            else
                        //            {
                        //                counterItems++;
                        //                flagFound = true;
                        //                break;
                        //            }
                        //        }

                        //    }

                        //    if (!flagFound)
                        //    {
                        //        MessagePrinter.PrintMessage($"{partInOrder} this pn not exist in turn 14", ImportanceLevel.High);
                        //        continue;
                        //    }
                        //}

                        //if (counterItems != order.OrderItems.Length)
                        //{
                        //    MessagePrinter.PrintMessage($"for order - {order.OrderID} not enough stock on turn 14", ImportanceLevel.High);
                        //    continue;
                        //}
                        //else
                        //{
                        //    MessagePrinter.PrintMessage($"Processing order - {order.OrderID}");
                        //}

                        if (Turn14AddedOrders.Exists(x => x.OrderSceID == order.OrderID && !x.TrackingNoUpdated)) // if order SCE added to Turn14
                        {
                            // process tracking number
                            Turn14Order turn14Order = Turn14AddedOrders.Find(x => x.OrderSceID == order.OrderID);
                            lstProcessQueue.Add(new ProcessQueueItem { ItemType = 3, Item = turn14Order });
                        }
                        else if (!Turn14AddedOrders.Exists(x => x.OrderSceID == order.OrderID))
                        {
                            // add order SCE to Turn14
                            if (order.ShippingService.CarrierID == eShippingProvider.UPS || order.ShippingService.CarrierID == eShippingProvider.USPS)
                            {
                                string currentURL = string.Format("{0}ajax_scripts/cart_ajax.php", this.Url);
                                // process order SCE
                                lstProcessQueue.Add(new ProcessQueueItem { URL = currentURL, ItemType = 2, Item = order });
                            }
                            else
                            {
                                MessagePrinter.PrintMessage(string.Format("Carrier of Order #{0} is {1}. Turn14 uses only UPS and USPS Priority Mail.",
                                    order.OrderID, order.ShippingService.CarrierID.ToString()));
                            }
                        }
                    }
                }
                else
                {
                    MessagePrinter.PrintMessage("Orders SCE not found!");
                }
            }

            StartOrPushPropertiesThread();
        }


        protected void Turn14Inventory()
        {
            try
            {
                if (!Login())
                {
                    return;
                }

                MessagePrinter.PrintMessage("Downloading inventory from Turn14. Please wait...");
                bool turn14Processed = false;

                int counter = 0;
                string csvFile = string.Empty;

                while (counter < 3)
                {
                    csvFile = LoadCsvFile();
                    if (!string.IsNullOrEmpty(csvFile))
                    {
                        break;
                        Thread.Sleep(10000);
                        csvFile = LoadCsvFile();
                    }
                    counter++;
                    MessagePrinter.PrintMessage("Wait 3 minutes for next try...", ImportanceLevel.Mid);
                    Thread.Sleep(1000 * 60 * 3);
                }

                if (string.IsNullOrEmpty(csvFile))
                {
                    MessagePrinter.PrintMessage($"Turn 14 Inventory not downloaded!", ImportanceLevel.Critical);
                }
                else
                {
                    MessagePrinter.PrintMessage("Turn 14 Inventory downloaded!");
                    if (File.Exists(csvFile))
                    {
                        MessagePrinter.PrintMessage(string.Format("Reading all inventory items from the file {0}", csvFile));
                        using (var sr = File.OpenText(csvFile))
                        {
                            using (var csv = new CsvReader(sr, true, ','))
                            {
                                var headers = csv.GetFieldHeaders();

                                while (csv.ReadNextRecord())
                                {
                                    try
                                    {
                                        string brand = csv["PrimaryVendor"];
                                        string warehouseCentralStock = "";
                                        bool brandFound = false;

                                        var resultBrand = extSett.BrandsAlignmentsItems.Where(i => string.Equals(i.BrandInTurn14, brand, StringComparison.OrdinalIgnoreCase)).ToList();
                                        if (resultBrand.Count > 0)
                                        {
                                            brand = resultBrand[0].BrandInTurn14;
                                            brandFound = true;
                                        }

                                        if (brandFound)
                                        {
                                            if (headers.Contains($"CentralStock"))
                                                warehouseCentralStock = csv["CentralStock"];
                                            if (string.IsNullOrEmpty(warehouseCentralStock))
                                            {
                                                warehouseCentralStock = "";
                                            }

                                            string manufacturerPartNumber = csv["PartNumber"];

                                            //if (manufacturerPartNumber == "42132")
                                            //{

                                            //}

                                            string partNumber = csv["InternalPartNumber"];
                                            string mapPrice = csv["Map"];
                                            string webPrice = mapPrice;
                                            string costPrice = csv["Cost"];
                                            string msrp = csv["Retail"];
                                            //string jobber = csv["Jobber"];
                                            string jobber = mapPrice;
                                            string warehouseEastStock = csv["EastStock"];
                                            string warehouseWesStock = csv["WestStock"];

                                            string stock = string.Empty;
                                            //if (extSett.Turn14InventoryType == Turn14InventoryType.Stock)
                                            stock = csv["Stock"];
                                            //else if (extSett.Turn14InventoryType == Turn14InventoryType.ManufacturerStorck)
                                            //    stock = csv["MfrStock"];

                                            if (string.IsNullOrEmpty(stock))
                                                stock = "0";

                                            if (!string.IsNullOrEmpty(costPrice))
                                            {
                                                webPrice = GetPriceRules(double.Parse(costPrice)).ToString();

                                                if (extSett.ConsiderMAPPrice)
                                                {
                                                    if (!string.IsNullOrEmpty(mapPrice))
                                                    {
                                                        if (double.Parse(mapPrice) > double.Parse(webPrice))
                                                        {
                                                            msrp = mapPrice;
                                                            webPrice = mapPrice;
                                                        }
                                                        else
                                                        {
                                                            msrp = webPrice;
                                                        }
                                                    }
                                                    else
                                                        msrp = webPrice;
                                                }
                                                else
                                                {
                                                    msrp = webPrice;
                                                }

                                                //if (double.Parse(costPrice) > double.Parse(msrp))
                                                //    msrp = ((double.Parse(costPrice) + 11) + ((double.Parse(costPrice) + 11) * 0.08)).ToString();

                                                //if (double.Parse(costPrice) >= double.Parse(webPrice))
                                                //    webPrice = ((double.Parse(costPrice) + 11) + ((double.Parse(costPrice) + 11) * 0.08)).ToString();

                                                //if (double.Parse(msrp) < double.Parse(webPrice))
                                                //    msrp = webPrice;

                                                jobber = webPrice;
                                            }

                                            foreach (TransferInfoItem transferInfoItem in extSett.TransferInfoItems)
                                            {
                                                if (String.Equals(transferInfoItem.BrandTurn14, brand, StringComparison.CurrentCultureIgnoreCase) &&
                                                    String.Equals(transferInfoItem.ManufacturerPartNumberSce, manufacturerPartNumber, StringComparison.CurrentCultureIgnoreCase))
                                                {
                                                    turn14Processed = true;

                                                    transferInfoItem.PartNumberTurn14 = partNumber;
                                                    transferInfoItem.ManufacturerPartNumberTurn14 = manufacturerPartNumber;
                                                    transferInfoItem.Msrp = msrp.Replace(",", string.Empty);
                                                    transferInfoItem.Jobber = jobber.Replace(",", string.Empty);
                                                    transferInfoItem.WebPrice = webPrice.Replace(",", string.Empty);
                                                    transferInfoItem.CostPrice = costPrice.Replace(",", string.Empty);

                                                    transferInfoItem.Turn14WarehouseWesStock = warehouseWesStock;
                                                    transferInfoItem.Turn14WarehouseEastStock = warehouseEastStock;
                                                    transferInfoItem.Turn14WarehouseCentralStock = warehouseCentralStock;
                                                    transferInfoItem.QuantityTurn14 = stock;

                                                    // transferInfoItem.EbayPrice = webPrice;
                                                }
                                            }
                                        }


                                    }
                                    catch (Exception e)
                                    {

                                    }
                                }
                            }
                        }
                        File.Delete(csvFile);
                        MessagePrinter.PrintMessage("Turn14 Inventory processed.");
                    }
                    else
                    {
                        MessagePrinter.PrintMessage(string.Format("File {0} not found!", csvFile), ImportanceLevel.Critical);
                    }
                }

                if (extSett.InventorySync)
                    if (extSett.TransferInfoItems.Count > 0 && turn14Processed)
                        FileHelper.CreateTurn14File(this, extSett);

                if (extSett.PriceSync)
                    if (extSett.TransferInfoItems.Count > 0 && turn14Processed)
                        FileHelper.CreateTurn14BatchPriceFile(this, extSett);
            }
            catch (Exception ex)
            {
                MessagePrinter.PrintMessage($"{ex.Message} {ex.StackTrace}", ImportanceLevel.Critical);
            }
        }

        private double GetPriceRules(double price)
        {
            price = price + (price * extSett.PercentageOfCost / 100);

            return Math.Round(price, 2);
        }

        private string LoadCsvFile()
        {
            try
            {
                var Url = "https://www.turn14.com/";
                string curUrl = string.Format("{0}export.php", Url);
                string postData = "stockExport=items";
                string localPath = Settings.Location;
                string FilePath =
                    localPath.Substring(0, localPath.Length - (localPath.Length - localPath.LastIndexOf('\\')) + 1) +
                    "turnfourteen.zip";
                PageRetriever.SaveFromServerWithPost(curUrl, postData, FilePath, true);
                if (File.Exists(FilePath))
                {
                    MessagePrinter.PrintMessage(string.Format("File {0} downloaded!", FilePath));
                    string unzippedFile = FilePath.Replace(".zip", ".csv");
                    using (var zipArc = ZipFile.Read(FilePath))
                    {
                        zipArc[0].FileName = Path.GetFileName(unzippedFile);
                        zipArc[0].Extract(Path.GetDirectoryName(unzippedFile),
                            ExtractExistingFileAction.OverwriteSilently);
                        MessagePrinter.PrintMessage(string.Format("File {0} unzipped!", unzippedFile));
                    }
                    File.Delete(FilePath);
                    MessagePrinter.PrintMessage(string.Format("File {0} deleted!", FilePath));
                    return unzippedFile;
                }
                else
                {
                    MessagePrinter.PrintMessage(string.Format("File {0} not found!", FilePath),
                        ImportanceLevel.Critical);
                }
            }
            catch (Exception e)
            {
                MessagePrinter.PrintMessage(e.Message, ImportanceLevel.Critical);
            }
            return string.Empty;
        }

        private void GetTurn14AddedOrders()
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();
            string localPath = Regex.Replace(Settings.Location, @"[\w-%&]*.edf", string.Empty);
            string ftpLogin = Settings.FtpUsername;
            string ftpPassword = Settings.FtpPassword;
            string ftpAddress = Settings.FtpAddress;
            string json;

            if (File.Exists(localPath + TempFileNameFTP))
            {
                MessagePrinter.PrintMessage(string.Format("Reading local file {0}...", localPath + TempFileNameFTP));
                json = File.ReadAllText(localPath + TempFileNameFTP);
                Turn14AddedOrders = jss.Deserialize<List<Turn14Order>>(json);
                MessagePrinter.PrintMessage(string.Format("File {0} readed!", localPath + TempFileNameFTP));
            }
            else
            {
                bool tempFileExists = FtpHelper.CheckIfFileExists(string.Format("ftp://{0}/{1}", ftpAddress, TempFileNameFTP),
                    ftpLogin, ftpPassword);

                if (tempFileExists)
                {
                    MessagePrinter.PrintMessage($"Downloading {TempFileNameFTP} from FTP. Please wait...");
                    string ftpFile = RequestHelper.DownloadFTPFile(localPath, "ftp://" + ftpAddress, TempFileNameFTP, ftpLogin, ftpPassword);
                    if (!ftpFile.StartsWith("Error"))
                    {
                        json = File.ReadAllText(localPath + TempFileNameFTP);
                        Turn14AddedOrders = jss.Deserialize<List<Turn14Order>>(json);
                        MessagePrinter.PrintMessage($"{TempFileNameFTP} downloaded from FTP!");
                    }
                    else
                    {
                        MessagePrinter.PrintMessage($"{TempFileNameFTP} not downloaded from FTP! {ftpFile}", ImportanceLevel.Critical);
                        Turn14AddedOrders = new List<Turn14Order>();
                    }
                }
                else
                {
                    Turn14AddedOrders = new List<Turn14Order>();
                }
            }
        }


        protected void ProcessOrder(ProcessQueueItem pqi)
        {
            try
            {
                if (cancel)
                    return;

                Order order = pqi.Item as Order;

                if (order.Shipments != null)
                {
                    List<OrderItem> orderItems = new List<OrderItem>(order.OrderItems);
                    JavaScriptSerializer jss = new JavaScriptSerializer();
                    CartResult cartResult;
                    string currentURL = pqi.URL, json, postData, partNumber;
                    bool partsFound;
                    List<Shipment> shipments = new List<Shipment>();

                    // empty cart
                    PageRetriever.WriteToServer(currentURL, "action=emptyCart", true, true, false);
                    foreach (Shipment shipment in order.Shipments)
                    {
                        partsFound = false;
                        foreach (ShipmentBox box in shipment.Boxes)
                        {
                            partsFound = false;
                            foreach (ShipmentBoxItem item in box.Items)
                            {
                                partNumber = orderItems.Find(x => x.OrderItemID == item.OrderItemID).PartNo;
                                // add items to cart
                                MessagePrinter.PrintMessage(string.Format("Add Part #{0} to cart in Turn14. Please wait...", partNumber));
                                postData = string.Format("action=addItem&itemname={0}&quantity={1}",
                                    partNumber, item.Qty);
                                json = PageRetriever.WriteToServer(currentURL, postData, true);
                                // get cart result
                                cartResult = jss.Deserialize<CartResult>(json);
                                if (cartResult.Result == "success") // if item was added to cart successfully
                                {
                                    MessagePrinter.PrintMessage(string.Format("Success! Part #{0} added to cart!",
                                        item.OrderItemID));
                                    partsFound = true;
                                }
                                else // else print failed validation message
                                {
                                    MessagePrinter.PrintMessage(string.Format("Part #: {0}. {1}",
                                        partNumber, cartResult.FailedValidationMessage));
                                    partsFound = false;
                                    break;
                                }
                            }
                            if (!partsFound)
                            {
                                break;
                            }
                        }
                        // if parts found then add shipment for updating
                        if (partsFound)
                        {
                            shipments.Add(shipment);
                        }
                    }

                    if (shipments.Count > 0)
                    {
                        // try to add order on site Turn14
                        if (tryAddOrderToTurn14(order))
                        {
                            Turn14Order turn14Order = new Turn14Order(order.OrderID, DateTime.Today.ToString("MM/dd/yyyy"), shipments, order.ShippingService.Code, order.ShippingService.SceCode, order.ShippingService.CarrierID);
                            Turn14AddedOrders.Add(turn14Order);
                            try
                            {
                                var objsceApi = SceApiHelper.GetApiClient(Settings);
                                objsceApi.MarkOrderERPSynced(order.OrderID);
                            }
                            catch (Exception e)
                            {
                            }

                            MessagePrinter.PrintMessage(string.Format("Order SCE #{0} processed", order.OrderID));
                        }
                        else
                        {
                            MessagePrinter.PrintMessage(string.Format("Order SCE #{0} not added to Turn14!", order.OrderID));
                        }
                    }
                    else
                    {
                        // empty cart
                        PageRetriever.WriteToServer(currentURL, "action=emptyCart", true, true, false);
                        MessagePrinter.PrintMessage(string.Format("Some products not found for Order SCE #{0} in Turn14!", order.OrderID));
                    }
                }
                else
                {
                    MessagePrinter.PrintMessage(string.Format("Shipments not found in Order SCE #{0}", order.OrderID));
                }

                pqi.Processed = true;
                StartOrPushPropertiesThread();
            }
            catch (Exception ex)
            {
                MessagePrinter.PrintMessage($"{ex.Message} in Process Order");
            }
        }

        private bool tryAddOrderToTurn14(Order order)
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();
            string currentURL = string.Format("{0}ajax_scripts/cart_ajax.php", this.Url);
            string response, postData, currentTimeStamp = string.Empty;
            CartResult orderResult;
            HtmlDocument doc;

            // get address details from Order SCE
            string contactName = HttpUtility.UrlEncode(order.ShippingAddress.CustomerName).Replace(" ", "+");
            string company = HttpUtility.UrlEncode(order.ShippingAddress.Company).Replace(" ", "+");
            string address1 = HttpUtility.UrlEncode(order.ShippingAddress.Address1).Replace(" ", "+");
            string address2 = HttpUtility.UrlEncode(order.ShippingAddress.Address2).Replace(" ", "+");
            string zipCode = HttpUtility.UrlEncode(order.ShippingAddress.Zip).Replace(" ", "+");
            string city = HttpUtility.UrlEncode(order.ShippingAddress.City).Replace(" ", "+");
            string state = HttpUtility.UrlEncode(order.ShippingAddress.StateAbbr).Replace(" ", "+");
            string country = HttpUtility.UrlEncode(order.ShippingAddress.CountryCode).Replace(" ", "+");
            string phone = HttpUtility.UrlEncode(order.ShippingAddress.Phone).Replace(" ", "+");
            string email = HttpUtility.UrlEncode(order.Account.Contacts[0].Email).Replace(" ", "+");
            string shippingName;
            switch (order.ShippingService.SceCode)
            {
                case eOrderShipType.ThreeDay:
                    shippingName = "USPS Priority Mail";
                    break;
                case eOrderShipType.TwoDay:
                    shippingName = "UPS Second Day Air";
                    break;
                case eOrderShipType.NextDay:
                    shippingName = "UPS Next Day Air";
                    break;
                default:
                    shippingName = "UPS Ground";
                    break;
            }

            MessagePrinter.PrintMessage(string.Format("Add Order SCE #{0} to Turn14. Please wait...", order.OrderID));
            // query 1 - get cart page
            response = PageRetriever.ReadFromServer(string.Format("{0}cart/cart.php", this.Url), true);
            doc = CreateDoc(response);
            var currentTimeStampHidden = doc.DocumentNode.SelectSingleNode("//div[@class='current-time-stamp hidden']");
            if (currentTimeStampHidden != null)
            {
                currentTimeStamp = HttpUtility.UrlEncode(HttpUtility.HtmlDecode(currentTimeStampHidden.InnerText))
                    .Replace(" ", "+");
            }
            var tagQuoteId = doc.DocumentNode.SelectSingleNode("//input[@id='quoteID']");
            string quoteId = string.Empty;
            if (tagQuoteId != null)
            {
                quoteId = tagQuoteId.Attributes["value"].Value;
            }
            var tagOrigPickup = doc.DocumentNode.SelectSingleNode("//input[@id='orig_pickup']");
            string origPickup = string.Empty;
            if (tagOrigPickup != null)
            {
                origPickup = tagOrigPickup.Attributes["value"].Value;
            }
            var tagOrigZip = doc.DocumentNode.SelectSingleNode("//input[@id='orig_zip_code']");
            string origZip = string.Empty;
            if (tagOrigZip != null)
            {
                origZip = tagOrigZip.Attributes["value"].Value;
            }
            var tagOrigCountry = doc.DocumentNode.SelectSingleNode("//input[@id='orig_country']");
            string origCountry = string.Empty;
            if (tagOrigCountry != null)
            {
                origCountry = tagOrigCountry.Attributes["value"].Value;
            }

            // query 2 - Calculate Shipping
            //postData = string.Format("contact={0}&company={1}&address_one={2}&address_two={3}" +
            //  "&zip_code={4}&city={5}&state={6}&state-us={6}&state-ca=&country={7}&phone={8}" +
            //  "&recipient_email={9}&orig_zip_code={10}&orig_country={11}&orig_pickup={12}" +
            //  "&quote_id={13}&action=saveAddressGetSplits&current_time_stamp={14}",
            //  contactName, company, address1, address2, zipCode, city, state, country, phone,
            //  email, origZip, origCountry, origPickup, quoteId, currentTimeStamp);

            postData = string.Format("contact={0}&company={1}&address_one={2}&address_two={3}&zip_code={4}" +
                "&city={5}&state=&state-us={6}&state-ca=&country={7}&phone={8}&orig_zip_code=&orig_country=" +
                "&orig_pickup=N&quote_id={13}&action=saveAddressGetSplits&current_time_stamp={14}",
            contactName, company, address1, address2, zipCode, city, state, country, phone, email, origZip, origCountry, origPickup, quoteId, currentTimeStamp);
            response = PageRetriever.WriteToServer(currentURL, postData, true).Replace("last_update", "lastupdate");
            orderResult = jss.Deserialize<CartResult>(response);
            currentTimeStamp = HttpUtility.UrlEncode(orderResult.LastUpdate.ToString()).Replace(" ", "+");

            // query 3 - get shipping options
            response = PageRetriever.WriteToServer(string.Format("{0}cart/shipping.php", this.Url), postData, true);
            doc = CreateDoc(response);
            HtmlNode tagShippingOption, tagShippingName;
            string shippingNum = string.Empty;
            var shipmentOptions = doc.DocumentNode.SelectNodes("//div[@class='radio']");
            if (shipmentOptions != null)
            {
                foreach (var shipmentOption in shipmentOptions)
                {
                    tagShippingName = shipmentOption.ChildNodes[1].SelectSingleNode("strong");
                    if (tagShippingName != null)
                    {
                        if (shippingName == tagShippingName.InnerText)
                        {
                            tagShippingOption = shipmentOption.ChildNodes[1].SelectSingleNode("input");
                            if (tagShippingOption != null)
                            {
                                shippingNum = tagShippingOption.Attributes["value"].Value;
                                break;
                            }
                        }
                    }
                }
            }

            if (string.IsNullOrEmpty(shippingNum))
            {
                MessagePrinter.PrintMessage(string.Format("Shipping Service {0} not found in Turn14. Please inform the Project Manager.", shippingName), ImportanceLevel.Critical);
            }
            else
            {
                // query 4 - Submit Shipping
                postData = string.Format("current_time_stamp={0}&shipping_o_{1}={1}&action=saveShippingOptions", currentTimeStamp, shippingNum);
                response = PageRetriever.WriteToServer(currentURL, postData, true).Replace("last_update", "lastupdate");
                orderResult = jss.Deserialize<CartResult>(response);
                currentTimeStamp = HttpUtility.UrlEncode(orderResult.LastUpdate.ToString()).Replace(" ", "+");

                // query 5 - Submit Order
                postData = string.Format("action=submitOrder&notes=&po_number={0}&current_time_stamp={1}", order.OrderID, currentTimeStamp);
                response = PageRetriever.WriteToServer(currentURL, postData, true);
                orderResult = jss.Deserialize<CartResult>(response);

                if (orderResult.Result.Contains("success"))
                {
                    MessagePrinter.PrintMessage(string.Format("Order SCE #{0} added to Turn14!", order.OrderID));
                    return true;
                }
                else
                {
                    MessagePrinter.PrintMessage(string.Format("Order SCE #{0}. {1}", order.OrderID, orderResult.FailedValidationMessage));
                    PageRetriever.WriteToServer(currentURL, "action=emptyCart", true, true, false);
                }
            }

            return false;
        }

        protected void ProcessTrackingNumber(ProcessQueueItem pqi)
        {
            if (cancel)
                return;

            try
            {
                Turn14Order turn14Order = pqi.Item as Turn14Order;
                string trackingNumber = getTrackingNumber(turn14Order.OrderSceID, turn14Order.Date);

                if (string.IsNullOrEmpty(trackingNumber))
                {
                    MessagePrinter.PrintMessage(string.Format("Tracking number not found in Turn14 for Order SCE #{0}!", turn14Order.OrderSceID));
                }
                else
                {
                    MessagePrinter.PrintMessage(string.Format("Tracking number found in Turn14 for Order SCE #{0}!", turn14Order.OrderSceID));
                    foreach (Shipment shipment in turn14Order.Shipments)
                    {
                        shipment.TrackingNo = trackingNumber;
                        shipment.ShipDate = DateTime.Today;
                        shipment.Service.Code = turn14Order.Code;
                        shipment.Service.SceCode = turn14Order.SCECode;
                        shipment.Service.CarrierID = turn14Order.CarrierID;
                        // update shipment details
                        lock (this)
                        {
                            lstProcessQueue.Add(new ProcessQueueItem { ItemType = 4, Item = shipment });
                        }
                    }
                    turn14Order.TrackingNoUpdated = true;
                    turn14Order.Shipments.Clear();
                }
            }
            catch (Exception e)
            {
                MessagePrinter.PrintMessage($"{e.Message} in Process Tracking number");
            }

            pqi.Processed = true;
            StartOrPushPropertiesThread();
        }

        private string getTrackingNumber(int orderID, string date)
        {
            string trackingNumber = string.Empty;

            MessagePrinter.PrintMessage(string.Format("Search tracking number for Order #{0} in Turn14. Please wait...", orderID));
            string currentURL = string.Format("{0}transactions.php?transactionType=all_orders&sort=&sortType=" +
                "&rows_per_page=&start={1}&end={1}&search={2}&column=P.O+%23", this.Url, HttpUtility.UrlEncode(date), orderID);
            string html = PageRetriever.ReadFromServer(currentURL, true);
            var doc = CreateDoc(html);
            var tableOrders = doc.DocumentNode.SelectSingleNode("//table[@class='table  table-bordered table-striped']");

            if (tableOrders != null)
            {
                int numColumnTrackingNumber = -1;
                int numColumnPO = -1;
                var heads = tableOrders.SelectNodes("thead/tr/th");
                if (heads != null)
                {
                    for (int i = 0; i < heads.Count; i++)
                    {
                        if (heads[i].InnerText.Contains("P.O #"))
                        {
                            numColumnPO = i + 1;
                        }
                        else if (heads[i].InnerText.Contains("Tracking"))
                        {
                            numColumnTrackingNumber = i + 1;
                            break;
                        }
                    }
                }

                if (numColumnTrackingNumber != -1 && numColumnPO != -1)
                {
                    HtmlNode columnPO, columnTrackingNumber;
                    var tableRows = tableOrders.SelectNodes("tbody/tr");
                    foreach (var row in tableRows)
                    {
                        columnPO = row.SelectSingleNode(string.Format("td[{0}]", numColumnPO));
                        columnTrackingNumber = row.SelectSingleNode(string.Format("td[{0}]", numColumnTrackingNumber));
                        if (columnTrackingNumber != null && columnPO != null)
                        {
                            if (columnPO.InnerText == orderID.ToString())
                            {
                                var linkTrackingNumber = columnTrackingNumber.SelectSingleNode("a");
                                if (linkTrackingNumber != null)
                                {
                                    trackingNumber = linkTrackingNumber.InnerText.Trim(' ', '\n');
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            return trackingNumber;
        }

        protected void UpdateShipmentTrackingNumber(ProcessQueueItem pqi)
        {
            if (cancel)
                return;

            Shipment shipment = pqi.Item as Shipment;

            MessagePrinter.PrintMessage(string.Format("Updating tracking number in Shipment #{0}. Please wait...", shipment.ShipmentId));
            var objsceApi = SceApiHelper.GetApiClient(Settings);
            try
            {
                objsceApi.UpdateShipmentDetails(shipment);
                MessagePrinter.PrintMessage(string.Format("Tracking number updated in Shipment #{0}", shipment.ShipmentId));
            }
            catch (Exception ex)
            {
                MessagePrinter.PrintMessage(ex.Message);
            }

            pqi.Processed = true;
            StartOrPushPropertiesThread();
        }

        protected override Action<ProcessQueueItem> GetItemProcessor(ProcessQueueItem item)
        {
            Action<ProcessQueueItem> act;
            if (item.ItemType == 2)
            {
                act = ProcessOrder;
            }
            else if (item.ItemType == 3)
            {
                act = ProcessTrackingNumber;
            }
            else if (item.ItemType == 4)
            {
                act = UpdateShipmentTrackingNumber;
            }
            else
            {
                act = null;
            }

            return act;
        }
    }
}
