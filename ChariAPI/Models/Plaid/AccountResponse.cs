using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChariAPI.Models.Plaid
{
    public class AccountResponse
    {
        public List<BankAccount> accounts { get; set; }
    }

    public class BankAccount
    {
        public string account_id { get; set; }
        public string mask { get; set; }
        public string name { get; set; }
        public string official_name { get; set; }
        public string subtype { get; set; }
        public string type { get; set; }
    }
}
