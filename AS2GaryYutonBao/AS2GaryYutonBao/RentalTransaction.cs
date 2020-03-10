using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS2GaryYutonBao
{
    class RentalTransaction
    {
        public string dlNumber;
        public DateTime startDate;
        public string vehicleType;
        public string rentalType;
        public int days;
        public string Optoins;

        public RentalTransaction()
        {

        }
        public RentalTransaction(string dlNumber, DateTime startDate, string vehicleType, string rentalType,
                                int days, string Optoins)
        {
            this.dlNumber = dlNumber;
            this.startDate = startDate;
            this.vehicleType = vehicleType;
            this.rentalType = rentalType;
            this.days = days;
            this.Optoins = Optoins;
        }


    }
}
