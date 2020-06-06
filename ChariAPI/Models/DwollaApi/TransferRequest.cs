using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChariAPI.Models.DwollaApi
{
    public class TransferRequest
    {
        public Links _links { get; set; }
        public Amount amount { get; set; }
    }

    public class Links
    {
        public Funding source { get; set; }
        public Funding destination { get; set; }
    }

    public class Funding
    {
        public string href { get; set; }
    }

    public class Amount
    {
        public string currency { get; set; }
        public double value { get; set; }
    }
}
