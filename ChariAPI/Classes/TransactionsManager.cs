using ChariAPI.Models;
using ChariAPI.Models.Plaid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChariAPI.Classes
{
    public class TransactionsManager
    {
        private readonly ChariContext _context;
        private readonly AppSettings _appSettings;
        public TransactionsManager(ChariContext chariContext, AppSettings appSettings)
        {
            _context = chariContext;
            _appSettings = appSettings;
        }
        public void AddTrans(TransactionsResponse transactions, Guid userId)
        {
            Account acc = _context.Account.FirstOrDefault(x => x.UserId == userId);
            foreach (PlaidTransaction trans in transactions.transactions)
            {
                Donations donations = new Donations();
                donations.DonationID = Guid.NewGuid();
                donations.UserID = userId;

                donations.TransAmount = trans.amount;
                donations.DonationsAmount = Math.Ceiling(donations.TransAmount) - donations.TransAmount;
                if (donations.DonationsAmount <= 0 || donations.TransAmount < 0)
                    continue;
                donations.DonationDate = trans.date;
                donations.DonationEin = acc.Charity;
                donations.DonationName = acc.CharityName;
                DwollaManager dwollaManager = new DwollaManager(_context, _appSettings);

                Dwolla dwolla = _context.Dwolla.FirstOrDefault(x => x.UserId == userId);

                //Charge account
                dwollaManager.CreateTransaction(dwolla.FundingSourceRef, _appSettings.DwollaFundingDestination, (double)donations.DonationsAmount);

                _context.Donations.Add(donations);
                _context.SaveChanges();
            }          
        }
    }
}
