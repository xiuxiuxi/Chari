using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChariAPI.Models.DwollaApi
{
    public class CustomerCreateRequest
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
    }
}
