using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace AS2GaryYutonBao
{
    class Customer
    {
        public string name;
        public string dlNumber;
        public string discount;

        public Customer()
        {

        }
        public Customer(string name, string dlNumber, string discount)
        {
            this.name = name;
            this.dlNumber = dlNumber;
            this.discount = discount;

        }
    }
}
