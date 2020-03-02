using System;
using System.Collections.Generic;
using System.Text;

namespace SeleniumProject.Data
{
    public class Customer
    {
        private string name;
        private Contact customerContact;
        private Contact billingContact;
        private bool isCustomerContactAndBillingContactSame = false;
        private string gst;

        public string Name { get; set; }
        public Contact CustomerContact { get; set; }
        public Contact BillingContact { get; set; }
        public bool IsCustomerContactAndBillingContactSame { get; set; }
        public string Gst { get; set; }

        public Customer()
        {

        }

        public Customer(string name, Contact customerContact, Contact billingContact, bool isCustomerContactAndBillingContactSame)
        {
            this.Name = name;
            this.CustomerContact = customerContact;
            this.BillingContact = billingContact;
            this.IsCustomerContactAndBillingContactSame = isCustomerContactAndBillingContactSame;
        }

        public Customer(string name, Contact customerContact, Contact billingContact)
        {
            this.Name = name;
            this.CustomerContact = customerContact;
            this.BillingContact = billingContact;
            this.IsCustomerContactAndBillingContactSame = (customerContact.ToString() == billingContact.ToString());
        }
    }
}
