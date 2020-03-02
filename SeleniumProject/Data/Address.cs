using System;
using System.Collections.Generic;
using System.Text;

namespace SeleniumProject.Data
{
    public class Address
    {
        private string street;
        private string city;
        private string postCode;
        private string country;
        public string Street { get; set; }
        public string City { get; set; }
        public string PostCode { get; set; }
        public string Country { get; set; }
        public Address()
        {

        }

        public Address(string street, string city, string postCode, string country)
        {
            this.Street = street;
            this.City = city;
            this.PostCode = postCode;
            this.Country = country;
        }

    }
}
