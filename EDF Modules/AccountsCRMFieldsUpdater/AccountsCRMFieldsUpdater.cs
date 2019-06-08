using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using Scraper.Shared;
using System.Web;
using HtmlAgilityPack;
using AccountsCRMFieldsUpdater;
using Databox.Libs.AccountsCRMFieldsUpdater;
using AccountsCRMFieldsUpdater.DataItems;
using AccountsCRMFieldsUpdater.Helpers;
using AccountsCRMFieldsUpdater.SCEapi;
using System.Threading;

namespace WheelsScraper
{
    public class AccountsCRMFieldsUpdater : BaseScraper
    {
        public AccountsCRMFieldsUpdater()
        {
            Name = "AccountsCRMFieldsUpdater";
            Url = "https://www.AccountsCRMFieldsUpdater.com/";
            PageRetriever.Referer = Url;
            WareInfoList = new List<ExtWareInfo>();
            Wares.Clear();
            BrandItemType = 2;

            SpecialSettings = new ExtSettings();
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
            return true;
        }

        public bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);

                if (!addr.Host.Contains("."))
                {
                    return false;
                }

                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public bool IsValidName(string name)
        {
            try
            {
                if (name.ToLower() == "null")
                {
                    return false;
                }
                else if (name.Length < 2)
                {
                    return false;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        protected override void RealStartProcess()
        {
            //lstProcessQueue.Add(new ProcessQueueItem { URL = Url, ItemType = 1 });

            //string filePath = @"D:\Leprizoriy\Work SCE\EDF\EDF Modules\UniversalFileReader\UniversalFileReader\bin\Debug\CustomersInfo.csv";
            List<CustomerInfo> customers = FileHelper.ReadCustomersFile(extSett.FilePath, out List<string> headers);

            MessagePrinter.PrintMessage($"{customers.Count} - customers readed");

            var sceCRMCustomFields = SceApiHelper.LoadSceCustomFields(Settings);

            int rowCounter = 1;

            List<CustomerInfo> customerInfos = new List<CustomerInfo>();

            foreach (CustomerInfo customer in customers)
            {
                try
                {
                    rowCounter++;
                    if (string.IsNullOrEmpty(customer.Email))
                    {
                        Console.WriteLine($"empty 'email' on [{rowCounter}] row");
                        continue;
                    }
                    else
                    {
                        //customer.Email = "\"cbass@rsrgroup.com\"";
                        //customer.Email = "EricHillman@europasports.com";
                        //customer.Email = "sales@novatechwholesale";
                        customer.Email = customer.Email.Replace("\"", "");
                        if (IsValidEmail(customer.Email) == false)
                        {
                            Console.WriteLine($"'email' {customer.Email} - is not valid");
                            customerInfos.Add(customer);

                            continue;
                        }
                    }

                    //if empty field we not need to update it probably add someValue(like 'null') to remove info

                    //get customer
                    //customer.Email = "jeff@broncograveyard.com";
                    MessagePrinter.PrintMessage($"Processing - {customer.Email}");

                    var client = SceApiHelper.GetApiClient(Settings);
                    var acc = client.AccountSearch(0, customer.Email, null, null, null, true);

                    if (extSett.ActionType == ActionType.Update)
                    {
                        if (acc.Count() > 0)
                        {
                            bool accUpdated = false;

                            if (!string.IsNullOrEmpty(customer.BillingCompany))
                            {
                                acc[0].Company = customer.BillingCompany;
                                accUpdated = true;
                            }

                            if (!string.IsNullOrEmpty(customer.Website))
                            {
                                acc[0].Website = customer.Website;
                                accUpdated = true;
                            }

                            if (!string.IsNullOrEmpty(customer.Phone1))
                            {
                                acc[0].AccountPhone1 = customer.Phone1;
                                accUpdated = true;
                            }

                            bool flag = true;

                            foreach (var contact in acc[0].Contacts)
                            {
                                if (contact.ID == acc[0].PrimaryContactID)
                                {
                                    if (!string.IsNullOrEmpty(customer.FirstName))
                                    {
                                        if (IsValidName(customer.FirstName) == true)
                                        {
                                            contact.FirstName = customer.FirstName;
                                            contact.Status = eStatus.IsUpdated;
                                            accUpdated = true;
                                        }
                                        else
                                        {
                                            flag = false;
                                            customerInfos.Add(customer);
                                            break;
                                        }
                                    }

                                    if (!string.IsNullOrEmpty(customer.LastName))
                                    {
                                        if (IsValidName(customer.LastName) == true)
                                        {
                                            contact.LastName = customer.LastName;
                                            contact.Status = eStatus.IsUpdated;
                                            accUpdated = true;
                                        }
                                        else
                                        {
                                            flag = false;
                                            customerInfos.Add(customer);
                                            break;
                                        }
                                    }

                                    if (!string.IsNullOrEmpty(customer.Phone1))
                                    {
                                        contact.Phone1 = customer.Phone1;
                                        contact.Status = eStatus.IsUpdated;
                                        accUpdated = true;
                                    }
                                }
                            }

                            if (flag == false)
                            {
                                continue;
                            }

                            if (acc[0].Addresses.Count() > 0)
                            {
                                bool found = false;
                                foreach (var address in acc[0].Addresses)
                                {
                                    if (address.City == customer.City &&
                                        address.StateAbbr == customer.State &&
                                        address.Zip == customer.Zip &&
                                        address.CountryCode == customer.Country &&
                                        address.Address1 == customer.BillingAddress)
                                    {
                                        found = true;
                                    }
                                }

                                if (!found)
                                {
                                    AccountAddress accountAddress = new AccountAddress();
                                    bool addressUpdated = false;

                                    if (!string.IsNullOrEmpty(customer.City))
                                    {
                                        accountAddress.City = customer.City;
                                        accountAddress.Status = eStatus.IsNew;
                                        accUpdated = true;
                                        addressUpdated = true;
                                    }

                                    if (!string.IsNullOrEmpty(customer.State))
                                    {
                                        accountAddress.StateAbbr = customer.State;
                                        accountAddress.Status = eStatus.IsNew;
                                        accUpdated = true;
                                        addressUpdated = true;
                                    }

                                    if (!string.IsNullOrEmpty(customer.Zip))
                                    {
                                        accountAddress.Zip = customer.Zip;
                                        accountAddress.Status = eStatus.IsNew;
                                        accUpdated = true;
                                        addressUpdated = true;
                                    }

                                    if (!string.IsNullOrEmpty(customer.Country))
                                    {
                                        accountAddress.CountryCode = customer.Country;
                                        accountAddress.Status = eStatus.IsNew;
                                        accUpdated = true;
                                        addressUpdated = true;
                                    }

                                    if (!string.IsNullOrEmpty(customer.BillingAddress))
                                    {
                                        accountAddress.Address1 = customer.BillingAddress;
                                        accountAddress.Status = eStatus.IsNew;
                                        accUpdated = true;
                                        addressUpdated = true;
                                    }

                                    if (addressUpdated)
                                    {
                                        accountAddress.ID = -1;
                                        var addr = acc[0].Addresses.ToList();
                                        addr.Add(accountAddress);
                                        acc[0].Addresses = addr.ToArray();
                                    }
                                }
                            }
                            else
                            {
                                AccountAddress accountAddress = new AccountAddress();
                                bool addressUpdated = false;

                                if (!string.IsNullOrEmpty(customer.City))
                                {
                                    accountAddress.City = customer.City;
                                    accountAddress.Status = eStatus.IsNew;
                                    accUpdated = true;
                                    addressUpdated = true;
                                }

                                if (!string.IsNullOrEmpty(customer.State))
                                {
                                    accountAddress.StateAbbr = customer.State;
                                    accountAddress.Status = eStatus.IsNew;
                                    accUpdated = true;
                                    addressUpdated = true;
                                }

                                if (!string.IsNullOrEmpty(customer.Zip))
                                {
                                    accountAddress.Zip = customer.Zip;
                                    accountAddress.Status = eStatus.IsNew;
                                    accUpdated = true;
                                    addressUpdated = true;
                                }

                                if (!string.IsNullOrEmpty(customer.Country))
                                {
                                    accountAddress.CountryCode = customer.Country;
                                    accountAddress.Status = eStatus.IsNew;
                                    accUpdated = true;
                                    addressUpdated = true;
                                }

                                if (!string.IsNullOrEmpty(customer.BillingAddress))
                                {
                                    accountAddress.Address1 = customer.BillingAddress;
                                    accountAddress.Status = eStatus.IsNew;
                                    accUpdated = true;
                                    addressUpdated = true;
                                }

                                if (addressUpdated)
                                {
                                    accountAddress.ID = -1;
                                    var addr = acc[0].Addresses.ToList();
                                    addr.Add(accountAddress);
                                    acc[0].Addresses = addr.ToArray();
                                }
                            }

                            if (accUpdated)
                            {
                                acc[0].Status = eStatus.IsUpdated;
                                Thread.Sleep(1000);
                                client.SaveAccount(acc[0]);
                            }


                            List<CustomField> accountsCustomFields = new List<CustomField>();
                            bool accountsCustomFieldsUpdated = false;
                            foreach (var accountField in customer.AccountCustomFields)
                            {
                                string fieldName = accountField.Split('^')[0];
                                string fieldValue = accountField.Split('^')[1];

                                if (!string.IsNullOrEmpty(fieldValue))
                                {
                                    if (fieldValue == "null")
                                    {
                                        fieldValue = "";
                                    }

                                    foreach (var sceCRMfield in sceCRMCustomFields)
                                    {
                                        if (string.IsNullOrEmpty(sceCRMfield.Name))
                                            continue;

                                        if (sceCRMfield.Name.Trim() == fieldName)
                                        {
                                            CustomField customField = new CustomField();
                                            customField.Value = fieldValue;
                                            customField.ID = sceCRMfield.ID;

                                            accountsCustomFields.Add(customField);
                                            accountsCustomFieldsUpdated = true;
                                        }
                                    }
                                }
                            }

                            if (accountsCustomFieldsUpdated)
                            {
                                Thread.Sleep(1000);
                                client.SaveCustomFields(acc[0].ID, null, null, null, accountsCustomFields.ToArray());
                            }

                            List<CustomField> contactCustomFields = new List<CustomField>();
                            bool contactCustomFieldsUpdated = false;
                            foreach (var contactField in customer.ContactCustomFields)
                            {
                                string fieldName = contactField.Split('^')[0];
                                string fieldValue = contactField.Split('^')[1];

                                if (!string.IsNullOrEmpty(fieldValue))
                                {
                                    if (fieldValue == "null")
                                    {
                                        fieldValue = "";
                                    }

                                    foreach (var sceCRMfield in sceCRMCustomFields)
                                    {
                                        if (string.IsNullOrEmpty(sceCRMfield.Name))
                                            continue;

                                        if (sceCRMfield.Name.Trim() == fieldName)
                                        {
                                            CustomField contactCustomField = new CustomField();
                                            contactCustomField.Value = fieldValue;
                                            contactCustomField.ID = sceCRMfield.ID;

                                            contactCustomFields.Add(contactCustomField);
                                            contactCustomFieldsUpdated = true;
                                        }
                                    }
                                }
                            }

                            if (contactCustomFieldsUpdated)
                            {
                                Thread.Sleep(1000);
                                client.SaveCustomFields(0, new int[] { acc[0].PrimaryContactID }, null, null, contactCustomFields.ToArray());
                            }

                            MessagePrinter.PrintMessage($"{customer.Email} - processed");
                        }
                        else
                        {
                            MessagePrinter.PrintMessage($"{customer.Email} - not found in SCE", ImportanceLevel.High);
                        }
                    }
                    else if (extSett.ActionType == ActionType.Add)
                    {
                        List<Contact> contacts = new List<Contact>();
                        var accNew = new Account();
                        var contact = new Contact();

                        if (acc.Count() < 1)
                        {
                            if (!string.IsNullOrEmpty(customer.BillingCompany))
                            {
                                accNew.Company = customer.BillingCompany;
                            }

                            if (!string.IsNullOrEmpty(customer.Website))
                            {
                                accNew.Website = customer.Website;
                            }

                            if (!string.IsNullOrEmpty(customer.Phone1))
                            {
                                accNew.AccountPhone1 = customer.Phone1;
                            }

                            bool flag = true;

                            if (!string.IsNullOrEmpty(customer.FirstName))
                            {
                                if (IsValidName(customer.FirstName) == true)
                                {
                                    contact.FirstName = customer.FirstName;
                                }
                                else
                                {
                                    flag = false;
                                }
                            }
                            else
                            {
                                flag = false;
                            }

                            if (!string.IsNullOrEmpty(customer.LastName))
                            {
                                if (IsValidName(customer.LastName) == true)
                                {
                                    contact.LastName = customer.LastName;
                                }
                                else
                                {
                                    flag = false;
                                }
                            }
                            else
                            {
                                flag = false;
                            }

                            if (!string.IsNullOrEmpty(customer.Email))
                            {
                                if (IsValidEmail(customer.Email) == true)
                                {
                                    contact.Email = customer.Email;
                                }
                                else
                                {
                                    flag = false;
                                }
                            }
                            else
                            {
                                flag = false;
                            }

                            if (!string.IsNullOrEmpty(customer.Phone1))
                            {
                                contact.Phone1 = customer.Phone1;
                                contact.Status = eStatus.IsNew;
                                contact.ID = -1;
                            }

                            AccountAddress accountAddress = new AccountAddress();
                            bool addressUpdated = false;

                            if (!string.IsNullOrEmpty(customer.City))
                            {
                                accountAddress.City = customer.City;
                                addressUpdated = true;
                            }

                            if (!string.IsNullOrEmpty(customer.State))
                            {
                                accountAddress.StateAbbr = customer.State;
                                addressUpdated = true;
                            }

                            if (!string.IsNullOrEmpty(customer.Zip))
                            {
                                accountAddress.Zip = customer.Zip;
                                addressUpdated = true;
                            }

                            if (!string.IsNullOrEmpty(customer.Country))
                            {
                                accountAddress.CountryCode = customer.Country;
                                addressUpdated = true;
                            }

                            if (!string.IsNullOrEmpty(customer.BillingAddress))
                            {
                                accountAddress.Address1 = customer.BillingAddress;
                                addressUpdated = true;
                            }

                            if (addressUpdated)
                            {
                                accountAddress.Status = eStatus.IsNew;
                                accountAddress.ID = -1;
                                var addr = accNew.Addresses.ToList();
                                addr.Add(accountAddress);
                                accNew.Addresses = addr.ToArray();
                            }

                            if (flag == true)
                            {
                                contact.Status = eStatus.IsNew;
                                contact.ID = -1;
                                contacts.Add(contact);
                                accNew.Contacts = contacts.ToArray();
                                accNew.PrimaryContactID = -1;
                                Thread.Sleep(1000);
                                var a = client.SaveAccount(accNew);

                                List<CustomField> accountsCustomFields = new List<CustomField>();
                                bool accountsCustomFieldsUpdated = false;
                                foreach (var accountField in customer.AccountCustomFields)
                                {
                                    string fieldName = accountField.Split('^')[0];
                                    string fieldValue = accountField.Split('^')[1];

                                    if (!string.IsNullOrEmpty(fieldValue))
                                    {
                                        if (fieldValue == "null")
                                        {
                                            fieldValue = "";
                                        }

                                        foreach (var sceCRMfield in sceCRMCustomFields)
                                        {
                                            if (string.IsNullOrEmpty(sceCRMfield.Name))
                                                continue;

                                            if (sceCRMfield.Name.Trim() == fieldName)
                                            {
                                                CustomField customField = new CustomField();
                                                customField.Value = fieldValue;
                                                customField.ID = sceCRMfield.ID;

                                                accountsCustomFields.Add(customField);
                                                accountsCustomFieldsUpdated = true;
                                            }
                                        }
                                    }
                                }

                                if (accountsCustomFieldsUpdated)
                                {
                                    Thread.Sleep(1000);
                                    client.SaveCustomFields(acc[0].ID, null, null, null, accountsCustomFields.ToArray());
                                }

                                List<CustomField> contactCustomFields = new List<CustomField>();
                                bool contactCustomFieldsUpdated = false;
                                foreach (var contactField in customer.ContactCustomFields)
                                {
                                    string fieldName = contactField.Split('^')[0];
                                    string fieldValue = contactField.Split('^')[1];

                                    if (!string.IsNullOrEmpty(fieldValue))
                                    {
                                        if (fieldValue == "null")
                                        {
                                            fieldValue = "";
                                        }

                                        foreach (var sceCRMfield in sceCRMCustomFields)
                                        {
                                            if (string.IsNullOrEmpty(sceCRMfield.Name))
                                                continue;

                                            if (sceCRMfield.Name.Trim() == fieldName)
                                            {
                                                CustomField contactCustomField = new CustomField();
                                                contactCustomField.Value = fieldValue;
                                                contactCustomField.ID = sceCRMfield.ID;

                                                contactCustomFields.Add(contactCustomField);
                                                contactCustomFieldsUpdated = true;
                                            }
                                        }
                                    }
                                }

                                if (contactCustomFieldsUpdated)
                                {
                                    Thread.Sleep(1000);
                                    client.SaveCustomFields(0, new int[] { acc[0].PrimaryContactID }, null, null, contactCustomFields.ToArray());
                                }

                                MessagePrinter.PrintMessage($"{customer.Email} - processed");
                            }
                        }
                        else
                        {
                            MessagePrinter.PrintMessage($"Account already exist - {customer.Email}", ImportanceLevel.Mid);
                        }
                    }
                }
                catch (Exception e)
                {
                    MessagePrinter.PrintMessage(e.ToString());
                }
            }

            if (customerInfos.Count > 0)
            {
                string filePath = FileHelper.CreateNotValidFile(FileHelper.GetSettingsPath("FailedCustomers.csv"), customerInfos);
                MessagePrinter.PrintMessage($"Faild file - {filePath}", ImportanceLevel.High);
            }

            StartOrPushPropertiesThread();
        }

        protected void ProcessBrandsListPage(ProcessQueueItem pqi)
        {
            if (cancel)
                return;

            pqi.Processed = true;
            MessagePrinter.PrintMessage("Brands list processed");
            StartOrPushPropertiesThread();
        }

        protected override Action<ProcessQueueItem> GetItemProcessor(ProcessQueueItem item)
        {
            Action<ProcessQueueItem> act;
            if (item.ItemType == 1)
                act = ProcessBrandsListPage;
            else act = null;

            return act;
        }
    }
}
