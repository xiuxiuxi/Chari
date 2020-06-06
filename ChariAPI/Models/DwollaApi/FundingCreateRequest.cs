using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChariAPI.Models.DwollaApi
{
    public class FundingCreateRequest
    {
        public string plaidToken { get; set; }
        public string name { get; set; }
    }
}
