using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ChariAPI.Models
{
    public class Donations
    {
        [Key]
        public Guid DonationID { get; set; }
        public Guid UserID { get; set; }
        public Decimal TransAmount { get; set; }
        public Decimal DonationsAmount { get; set; }
        public DateTime DonationDate { get; set; }
        public Boolean IsOneTimeDonation { get; set; }
        public string DonationEin { get; set; }
        public string DonationName { get; set; }
    }
}
