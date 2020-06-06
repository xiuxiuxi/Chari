using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChariAPI.Models
{
    public class Dwolla
    {
        public Guid DwollaId { get; set; }
        public Guid UserId { get; set; }
        public string CustomerRef { get; set; }
        public string FundingSourceRef { get; set; }
    }
}
