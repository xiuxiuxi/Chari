using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ChariAPI.Models;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authorization;
using ChariAPI.Models.Plaid;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using ChariAPI.Classes;

namespace ChariAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PlaidItemController : ControllerBase
    {
        private readonly ChariContext _context;
        private readonly AppSettings _appSettings;

        public PlaidItemController(ChariContext context, IOptions<AppSettings> options)
        {
            _context = context;
            _appSettings = options.Value;
        }

        [HttpPost("AddBank")]
        public ActionResult AddAccount(string public_token)
        {
            User user;
            string tokenString = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            user = _context.User.FirstOrDefault(x => x.Token == tokenString);

            if (user == null)
                return BadRequest();

            TokenExchange exchange = new TokenExchange();
            exchange.client_id = _appSettings.PlaidClientId;
            exchange.public_token = public_token;
            exchange.secret = _appSettings.PlaidSecret;

            WebRequest request = WebRequest.Create(_appSettings.PlaidBaseLink + "item/public_token/exchange");
            request.Method = "Post";
            request.ContentType = "application/json";
            //Json body
            using(var stream = new StreamWriter(request.GetRequestStream()))
            {
                stream.Write(JsonConvert.SerializeObject(exchange));
                stream.Flush();
            }

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Token token;
            using(var stream = new StreamReader(response.GetResponseStream()))
            {
                token = JsonConvert.DeserializeObject<Token>(stream.ReadToEnd());
            }

            PlaidItem item = new PlaidItem();
            item.PlaidItemAccessToken = token.access_token;
            item.PlaidItemId = Guid.NewGuid();
            item.PlaidItemItemRef = token.item_id;
            item.PlaidItemRequestId = token.request_id;
            item.UserId = user.Id;
            _context.PlaidItem.Add(item);

            //Get accounts from plaid
            AccountRequest accountReq = new AccountRequest();
            accountReq.client_id = _appSettings.PlaidClientId;
            accountReq.access_token = item.PlaidItemAccessToken;
            accountReq.secret = _appSettings.PlaidSecret;

            request = WebRequest.Create(_appSettings.PlaidBaseLink + "accounts/get");
            request.Method = "Post";
            request.ContentType = "application/json";
            //Json body
            using (var stream = new StreamWriter(request.GetRequestStream()))
            {
                stream.Write(JsonConvert.SerializeObject(accountReq));
                stream.Flush();
            }
            try
            {
                response = (HttpWebResponse)request.GetResponse();
            }
            catch(Exception e)
            {

            }
            AccountResponse accountResp;
            using (var stream = new StreamReader(response.GetResponseStream()))
            {
                accountResp = JsonConvert.DeserializeObject<AccountResponse>(stream.ReadToEnd());
            }
            BankAccount BankAcc = accountResp.accounts.Where(x => x.subtype.ToLower().Contains("checking")).FirstOrDefault();

            //Get Dwolla token from plaid
            var plaidDwollaReq = new PlaidDwollaTokenRequest();
            plaidDwollaReq.client_id = _appSettings.PlaidClientId;
            plaidDwollaReq.secret = _appSettings.PlaidSecret;
            plaidDwollaReq.access_token = item.PlaidItemAccessToken;
            plaidDwollaReq.account_id = BankAcc.account_id;

            request = WebRequest.Create(_appSettings.PlaidBaseLink + "processor/dwolla/processor_token/create");
            request.Method = "Post";
            request.ContentType = "application/json";
            //Json body
            using (var stream = new StreamWriter(request.GetRequestStream()))
            {
                stream.Write(JsonConvert.SerializeObject(plaidDwollaReq));
                stream.Flush();
            }

            try
            {
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (Exception e)
            {

            }
            PlaidDwollaTokenResponse plaidDwollaResp;
            using (var stream = new StreamReader(response.GetResponseStream()))
            {
                plaidDwollaResp = JsonConvert.DeserializeObject<PlaidDwollaTokenResponse>(stream.ReadToEnd());
            }

            //Create Dwolla customer and funding source
            DwollaManager dwollaManager = new DwollaManager(_context, _appSettings);
            string customerRef = dwollaManager.CreateCustomer(user.FirstName, user.LastName, user.Email);
            string fundingRef = dwollaManager.CreateFundingSource(customerRef, plaidDwollaResp.processor_token, BankAcc.name);

            Dwolla dwolla = new Dwolla();
            dwolla.DwollaId = Guid.NewGuid();
            dwolla.UserId = user.Id;
            dwolla.CustomerRef = customerRef;
            dwolla.FundingSourceRef = fundingRef;
            _context.Dwolla.Add(dwolla);
            //try
            //{
            _context.SaveChanges();
            //}
            //catch (Exception e)
            //{

            //}
            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("PlaidWebhook")]
        public ActionResult AddDonation(Webhook webhook)
        {
            PlaidItem item = _context.PlaidItem.FirstOrDefault(x => x.PlaidItemItemRef == webhook.item_id);
            if (item == null)
                return BadRequest();
            if (webhook.webhook_type == "HISTORICAL_UPDATE")
                return Ok();

            TransactionRequest trans = new TransactionRequest
            {
                access_token = item.PlaidItemAccessToken,
                secret = _appSettings.PlaidSecret,
                client_id = _appSettings.PlaidClientId,
                start_date = DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd"),
                end_date = DateTime.Now.ToString("yyyy-MM-dd"),
                options = new Models.Plaid.Options
                {
                    count = webhook.new_transactions
                }
            };

            WebRequest request = WebRequest.Create(_appSettings.PlaidBaseLink + "transactions/get");
            request.Method = "Post";
            request.ContentType = "application/json";
            //Json body
            using (var stream = new StreamWriter(request.GetRequestStream()))
            {
                stream.Write(JsonConvert.SerializeObject(trans));
            }
            HttpWebResponse response = null;
            try
            {
                response = (HttpWebResponse)request.GetResponse();

            }
            catch (Exception ex)
            {

            }
            TransactionsResponse tranResp;
            using (var stream = new StreamReader(response.GetResponseStream()))
            {
                string responsetxt = stream.ReadToEnd();
                tranResp = JsonConvert.DeserializeObject<TransactionsResponse>(responsetxt);
                TransactionsManager manager = new TransactionsManager(_context, _appSettings);
                manager.AddTrans(tranResp, item.UserId);
            }
            return Ok();
        }

        [AllowAnonymous]
        [HttpGet("test")]
        public ActionResult test()
        {
            return Ok();
        }
        
    }
}
