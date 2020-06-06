using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChariAPI.Models.Plaid
{
    public class TransactionsResponse
    {
        public int total_transactions { get; set; }
        public List<PlaidTransaction> transactions { get; set; }
    }

    public class PlaidTransaction
    {
        public string account_id { get; set; }
        public string account_owner { get; set; }
        public Decimal amount { get; set; }
        public List<string> category { get; set; }
        public string category_id { get; set; }
        public DateTime date { get; set; }
        public string iso_currency_code { get; set; }
        public Location location { get; set; }
        public string name { get; set; }
        public Payment_Meta payment_meta { get; set; }
        public bool pending { get; set; }
        public string pending_transaction_id { get; set; }
        public string transaction_id { get; set; }
        public string transaction_type { get; set; }
        public string unofficial_currency_code { get; set; }

    }
    public class Location
    {
        public string address { get; set; }
        public string city { get; set; }
        public string lat { get; set; }
        public string lon { get; set; }
        public string state { get; set; }
        public string store_number { get; set; }
        public string zip { get; set; }
    }

    public class Payment_Meta
    {
        public string by_order_of { get; set; }
        public string payee { get; set; }
        public string payer { get; set; }
        public string payment_method { get; set; }
        public string payment_processor { get; set; }
        public string ppd_id { get; set; }
        public string reason { get; set; }
        public string reference_number { get; set; }
    }
}
