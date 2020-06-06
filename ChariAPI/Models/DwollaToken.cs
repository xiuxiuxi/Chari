using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChariAPI.Models
{
    public class DwollaToken
    {
        public Guid DwollaTokenId { get; set; }
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}
