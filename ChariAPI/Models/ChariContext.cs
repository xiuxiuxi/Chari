using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChariAPI.Classes;
//using ChariAPI.Classes;
using ChariAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ChariAPI.Models
{
    public class ChariContext : DbContext
    {

        public ChariContext(DbContextOptions<ChariContext> options) : base(options)
        {

        }
        public DbSet<User> User { get; set; }
        public DbSet<PlaidItem> PlaidItem { get; set; }
        //public DbSet<Organization> Organization { get; set; }
        //public DbSet<PlaidDonation> PlaidDonations { get; set; }
        //Add class (4/5/20)
        public DbSet<Donations> Donations { get; set; }
        public DbSet<Account> Account { get; set; }
        public DbSet<DwollaToken> DwollaToken { get; set; }
        public DbSet<Dwolla> Dwolla { get; set; }



    }
}