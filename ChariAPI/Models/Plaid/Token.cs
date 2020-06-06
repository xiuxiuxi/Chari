using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChariAPI.Models.Plaid
{
    public class Token
    {
        public string access_token { get; set; }
        public string item_id { get; set; }
        public string request_id { get; set; }
    }
}
