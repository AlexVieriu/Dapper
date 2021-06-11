using System;
using System.Collections.Generic;

namespace DapperTutorial.Models
{
    public class Customer
    {
        public int CustomerID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public CustomerAddress CustomerAddress { get; set; }
        public List<CustomerAddress> CustomerAddressList { get; set; }
    }
}
