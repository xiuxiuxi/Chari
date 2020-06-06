using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChariAPI
{
    public class AppSettings
    {
        public string ConnectionString { get; set; }
        public string SecretKey { get; set; }
        public string CharityNavOrgSearchBaseLink { get; set; }
        public string CharityNavBaseLink { get; set; }
        public string CharityNavAppID { get; set; }
        public string CharityNavAppKey { get; set; }
        public string PlaidClientId { get; set; }
        public string PlaidPublicKey { get; set; }
        public string PlaidBaseLink { get; set; }
        public string PlaidSecret { get; set; }
        public string DwollaKey { get; set; }
        public string DwollaSecret { get; set; }
        public string DwollaBaseLink { get; set; }
        public string DwollaFundingDestination { get; set; }
    }
}
