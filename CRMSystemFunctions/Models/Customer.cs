using System;
using System.Collections.Generic;
using System.Text;

namespace CRMSystemFunctions.Models
{
    public class Customer
    {
        public int CustomerId { get; set; }
        public string FullName { get; set; }
        public string Address { get; set; }
        public int CreditLimit { get; set; }
        public bool ActiveStatus { get; set; }

        public override string ToString()
        {
            return string.Format(@"{0}, {1}, {2, {3},, {4}",
                this.CustomerId, this.FullName, this.Address, this.CreditLimit, this.ActiveStatus);
        }
    }
}
