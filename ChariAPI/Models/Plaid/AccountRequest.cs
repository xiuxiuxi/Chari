﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChariAPI.Models.Plaid
{
    public class AccountRequest
    {
        public string client_id { get; set; }
        public string secret { get; set; }
        public string access_token { get; set; }
    }
}
