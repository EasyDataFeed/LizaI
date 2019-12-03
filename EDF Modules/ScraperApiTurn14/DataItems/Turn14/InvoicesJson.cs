using System;
using System.Collections.Generic;

namespace ScraperApiTurn14.DataItems.Turn14
{
    public class InvoicesJson
    {
        public List<dataInvoice> data { get; set; }
        public meta meta { get; set; }
        public links links { get; set; }
    }

    public class dataInvoice
    {
        public string type { get; set; }
        public long id { get; set; }
        public attributesInvoice attributes { get; set; }

    }

    public class attributesInvoice
    {
        public long invoice_number { get; set; }
        public DateTime date { get; set; }
        public double total_price { get; set; }
        public double discount_percent { get; set; }
        public double discount_amount { get; set; }
        public double freight { get; set; }
        public double paid_amount { get; set; }
        public double amount_due { get; set; }
        public string bill_to_address { get; set; }
        public string ship_to_address { get; set; }
        public string sales_rep { get; set; }
        public string customer_name { get; set; }
        public string payment_terms { get; set; }
        public List<trackingInvoice> tracking { get; set; }
        public List<linesInvoice> lines { get; set; }
        public List<relationshipsInvoice> relationships { get; set; }
        public long website_order_number { get; set; }
        public string comments { get; set; }
    }

    public class trackingInvoice
    {
        public string ship_method { get; set; }
        public string tracking_number { get; set; }
    }

    public class linesInvoice
    {
        public long item_id { get; set; }
        public string part_number { get; set; }
        public string part_description { get; set; }
        public int quantity { get; set; }
        public int location_id { get; set; }
        public double unit_price { get; set; }
        public double total_price { get; set; }
    }

    public class relationshipsInvoice
    {
        public order order { get; set; }
    }

    public class order
    {
        public long order_id { get; set; }
        public string links { get; set; }
    }

    public class meta
    {
        public int total_pages { get; set; }
    }

    public class links
    {
        public string self { get; set; }
        public string last { get; set; }
    }

    class SingleInvoiceJson
    {
        public dataSingleInvoice data { get; set; }
    }

    public class dataSingleInvoice
    {
        public string type { get; set; }
        public string id { get; set; }
        public attributesSingleInvoice attributes { get; set; }
    }

    public class attributesSingleInvoice
    {
        public string invoice_number { get; set; }
        public DateTime date { get; set; }
        public double total_price { get; set; }
        public double discount_percent { get; set; }
        public double discount_amount { get; set; }
        public double freight { get; set; }
        public double paid_amount { get; set; }
        public double amount_due { get; set; }
        public string bill_to_address { get; set; }
        public string ship_to_address { get; set; }
        public string sales_rep { get; set; }
        public string customer_name { get; set; }
        public string payment_terms { get; set; }
        public List<trackingSingleInvoice> tracking { get; set; }
        public List<linesSingleInvoice> lines { get; set; }
        public List<relationshipsSingleInvoice> relationships { get; set; }
        public string comments { get; set; }
    }

    public class trackingSingleInvoice
    {
        public string ship_method { get; set; }
        public string tracking_number { get; set; }
    }

    public class linesSingleInvoice
    {
        public string item_id { get; set; }
        public string part_number { get; set; }
        public string part_description { get; set; }
        public int quantity { get; set; }
        public string location_id { get; set; }
        public double unit_price { get; set; }
        public double total_price { get; set; }
    }

    public class relationshipsSingleInvoice
    {
        public orderSingleInvoice order { get; set; }
    }

    public class orderSingleInvoice
    {
        public string order_id { get; set; }
        public string links { get; set; }
    }
}
