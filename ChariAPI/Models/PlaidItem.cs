using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ChariAPI.Models
{
    public class PlaidItem
    {
        public Guid PlaidItemId { get; set; }
        public string PlaidItemAccessToken { get; set; }
        public string PlaidItemItemRef { get; set; }
        public string PlaidItemRequestId { get; set; }
        [ForeignKey("User")]
        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}
