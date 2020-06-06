using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChariAPI.Models.Plaid
{
    public class Webhook
    {
        public string webhook_type { get; set; }
        public string webhook_code { get; set; }
        public string item_id { get; set; }
        public string error { get; set; }
        public int new_transactions { get; set; }
    }
}
