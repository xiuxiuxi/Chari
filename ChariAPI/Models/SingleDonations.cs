using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChariAPI.Models
{
    public class SingleDonations
    {
        public List<SingleDonation> donations { get; set; }
    }

    public class SingleDonation
    {
        public string ein { get; set; }
        public string charityName { get; set; }
        public double amount { get; set; }
    }
}
