using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AccountsCRMFieldsUpdater.DataItems
{
    public class CustomerInfo
    {
        public CustomerInfo()
        {
            AccountCustomFields = new List<string>();
            ContactCustomFields = new List<string>();
        }

        public string Email { get; set; }
        public string Website { get; set; }
        public string BillingCompany { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone1 { get; set; }
        public string BillingAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Country { get; set; }
        public List<string> AccountCustomFields { get; set; }
        public List<string> ContactCustomFields { get; set; }
    }
}
