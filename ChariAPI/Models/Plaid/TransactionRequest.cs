using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChariAPI.Models.Plaid
{
    public class TransactionRequest
    {
        public string client_id { get; set; }
        public string secret { get; set; }
        public string access_token { get; set; }
        public string start_date { get; set; }
        public string end_date { get; set; }
        public Options options { get; set; }
    }

    public class Options
    {
        public int count { get; set; }
        public int offset { get; set; }
    }
}
