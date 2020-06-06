using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChariAPI.Models.Plaid
{
    public class TokenExchange
    {
        public string client_id { get; set; }
        public string secret { get; set; }
        public string public_token { get; set; }
    }
}
