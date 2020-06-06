using System;
namespace ChariAPI.Models
{
    public class Account
    {
        public Guid AccountId { get; set; }
        public Guid UserId { get; set; }
        public int Points { get; set; }
        public string Charity { get; set; }
        public string CharityName { get; set; }
    }
}
