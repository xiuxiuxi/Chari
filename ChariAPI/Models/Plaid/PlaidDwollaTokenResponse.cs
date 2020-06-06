using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChariAPI.Models.Plaid
{
    public class PlaidDwollaTokenResponse
    {
        public string processor_token { get; set; }
        public string request_id { get; set; }
    }
}
