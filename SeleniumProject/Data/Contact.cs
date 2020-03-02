using System;
using System.Collections.Generic;
using System.Text;

namespace SeleniumProject.Data
{
    public class Contact
    {
        private string firstName;
        private string lastName;
        private string preferredName;
        private string phoneNumber;
        private string mobileNumber;
        private string email;
        private string fax;
        private Address address;
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PreferredName { get; set; }
        public string PhoneNumber { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public string Fax { get; set; }
        public string Address { get; set; }

        public Contact()
        {

        }
        // constructor with mandatory fields
        public Contact(string firstName, string lastName, string phoneNumber)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.PhoneNumber = phoneNumber;
        }

        // quick solution (TO BE FIXED LATER)
        // convert it to JSON string for comparison
        public override string ToString()
        {
            return "[" + FirstName + "] [" + LastName + "] [" + PhoneNumber + "]";
        }
    }
}
