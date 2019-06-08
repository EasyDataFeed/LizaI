#region using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Turn14Connector.Extensions;
using Turn14Connector.SCEapi;

#endregion

namespace Turn14Connector.DataItems
{
    public class OrderSync
    {

        #region Constants

        private const string ProdCustomTextPattern = @"\.?-text";

        #endregion

        #region Constructors

        public OrderSync(Order order)
        {
            SceOrder = order;

            SceCompany = order.Account.Company;
            OrderId = order.OrderID;
            OrderDate = order.OrderDate;
            TotalShipping = order.TotalShipping.Round();
            TotalDiscount = order.TotalDiscount.Round();
            TotalTax = order.OrderItems.Sum(i => i.Tax).Round();
            if (order.ShippingService != null)
            {
                ShippingProvider = order.ShippingService.CarrierTag;
                ShippingDescription = order.ShippingService.Description;
                ShippingType = order.ShippingService.SceCode.ToString();
            }
            if (order.Shipments != null && order.Shipments.Any())
            {
                var shipDate = order.Shipments.First().ShipDate;
                if (shipDate != default(DateTime))
                {
                    ShipDate = shipDate;
                }
            }

            TrackingNumbers = order.TrackingNumbers;
            var primaryContId = order.Account.PrimaryContactID;
            var primaryCont = order.Account.Contacts.FirstOrDefault(a => a.ID == primaryContId) ??
            order.Account.Contacts.FirstOrDefault();
            SceCustomer = $"{primaryCont?.FirstName} {primaryCont?.MiddleName} {primaryCont?.LastName}".RemoveDoubleSpace();
            TotalAmount = order.Total;

            foreach (var item in order.OrderItems)
            {
                item.PartNo = Regex.Replace(item.PartNo, ProdCustomTextPattern, "");
            }

            OrderItems = new List<SceOrderItem>();

            var groupedSimpleOrBundle =
            order.OrderItems.Where(i => i.BundleProdID == 0).GroupBy(p => new { p.PartNo, p.FiledWarehouse });

            foreach (var simpleOrBundle in groupedSimpleOrBundle)
            {
                var orderItem = new SceOrderItem();
                orderItem.PartNumber = simpleOrBundle.Key.PartNo;
                orderItem.ProductId = simpleOrBundle.First().ProductID;
                orderItem.Quantity = simpleOrBundle.Sum(i => i.Qty);
                orderItem.Price = simpleOrBundle.First().Price;
                orderItem.Category = simpleOrBundle.First().Category;
                orderItem.Description = simpleOrBundle.First().Description;
                orderItem.Tax = simpleOrBundle.First().Tax;

                OrderItems.Add(orderItem);
            }
        }

        #endregion

        #region Properties

        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime? ShipDate { get; set; }
        public double TotalAmount { get; set; }
        public double TotalShipping { get; set; }
        public double TotalDiscount { get; set; }
        public double TotalTax { get; set; }
        public string ShippingProvider { get; set; }
        public string ShippingType { get; set; }
        public string ShippingDescription { get; set; }
        public string TrackingNumbers { get; set; }
        public string SceCompany { get; set; }
        public string SceCustomer { get; set; }
        public List<SceOrderItem> OrderItems { get; set; }
        public Order SceOrder { get; set; }
        public bool Imported { get; set; }

        #endregion

    } 
}
