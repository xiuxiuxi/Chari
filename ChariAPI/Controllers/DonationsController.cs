using ChariAPI.Classes;
using ChariAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChariAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DonationsController: ControllerBase
    {
        private readonly ChariContext _context;
        private readonly AppSettings _appSettings;

        public DonationsController(ChariContext context, IOptions<AppSettings> options)
        {
            _context = context;
            _appSettings = options.Value;
        }

        [HttpGet]
        public ActionResult<List<Donations>> GetDonations()
        {
            string tokenString = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            User user = _context.User.FirstOrDefault(x => x.Token == tokenString);
            Account acc = _context.Account.FirstOrDefault(x => x.UserId == user.Id);
            if (user == null)
                return BadRequest();

            List<Donations> donations = null;
            try
            {
                donations = _context.Donations.Where(x => x.UserID == user.Id).ToList();

            }
            catch (Exception e)
            {

            }

            foreach(var d in donations)
            {
                if(d.DonationName == null || d.DonationEin == null)
                {
                    d.DonationName = acc.Charity;
                }
            }
            return Ok(donations);
        }

        [HttpPost("SingleDonations")]
        public ActionResult SingleDonations(SingleDonations donations)
        {
            DwollaManager dwollaManager = new DwollaManager(_context, _appSettings);
            string tokenString = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            User user = _context.User.FirstOrDefault(x => x.Token == tokenString);
            string fundingSource = _context.Dwolla.FirstOrDefault(x => x.UserId == user.Id).FundingSourceRef;

            Donations donationsRec;
            foreach (var d in donations.donations)
            {
                dwollaManager.CreateTransaction(fundingSource, _appSettings.DwollaFundingDestination, d.amount);
                donationsRec = new Donations();
                donationsRec.DonationID = Guid.NewGuid();
                donationsRec.DonationDate = DateTime.Now;
                donationsRec.DonationEin = d.ein;
                donationsRec.IsOneTimeDonation = true;
                donationsRec.DonationName = d.charityName;
                donationsRec.DonationsAmount = (decimal)d.amount;
                donationsRec.UserID = user.Id;
                _context.Donations.Add(donationsRec);
            }
            return Ok();
        }
    }
}
