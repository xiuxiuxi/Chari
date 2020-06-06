using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace ChariAPI.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
#nullable enable
        public string? Token { get; set; }
#nullable disable
        public DateTime? TokenExpiration { get; set; }
        public ICollection<PlaidItem> PlaidItems { get; set; }

        public User WithoutPassword()
        {
            Password = null;
            return this;
        }
    }
}
