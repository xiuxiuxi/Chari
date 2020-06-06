using ChariAPI.Models;
using ChariAPI.Models.DwollaApi;
using ChariAPI.Models.Plaid;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ChariAPI.Classes
{
    public class DwollaManager
    {
        private readonly ChariContext _context;
        private readonly AppSettings _appSettings;
        public DwollaManager(ChariContext context, AppSettings appSettings)
        {
            _context = context;
            _appSettings = appSettings;
        }

        public string CreateFundingSource(string customerRef, string plaidToken, string name)
        {
            var funding = new FundingCreateRequest();
            funding.plaidToken = plaidToken;
            funding.name = name;

            WebRequest request = WebRequest.Create(_appSettings.DwollaBaseLink + "/customers/" + customerRef + "/funding-sources");
            request.Method = "Post";
            request.ContentType = "application/vnd.dwolla.v1.hal+json";
            request.PreAuthenticate = true;
            request.Headers.Add("Authorization", "Bearer " + GetToken());
            request.Headers.Add("Accept", "application/vnd.dwolla.v1.hal+json");

            using (var stream = new StreamWriter(request.GetRequestStream()))
            {
                stream.Write(JsonConvert.SerializeObject(funding));
                stream.Flush();
            }
            HttpWebResponse response = null;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (Exception e)
            {

            }
            string fundingRef = response.Headers["Location"];
            fundingRef = fundingRef.Substring(fundingRef.LastIndexOf("/") + 1);
            return fundingRef;
        }

        public string CreateCustomer(string firstName, string lastName, string email)
        {
            var customerCreate = new CustomerCreateRequest();
            customerCreate.firstName = firstName;
            customerCreate.lastName = lastName;
            customerCreate.email = email;

            WebRequest request = WebRequest.Create(_appSettings.DwollaBaseLink + "/customers");
            request.Method = "Post";
            request.ContentType = "application/vnd.dwolla.v1.hal+json";
            request.PreAuthenticate = true;
            request.Headers.Add("Authorization", "Bearer " + GetToken());
            request.Headers.Add("Accept", "application/vnd.dwolla.v1.hal+json");

            using (var stream = new StreamWriter(request.GetRequestStream()))
            {
                stream.Write(JsonConvert.SerializeObject(customerCreate));
                stream.Flush();
            }

            HttpWebResponse response = null;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (Exception e)
            {

            }
            string customerRef = response.Headers["Location"];
            customerRef = customerRef.Substring(customerRef.LastIndexOf("/") + 1);
            return customerRef;
        }

        public void CreateTransaction(string fundingSource, string fundingDestination, double ammount)
        {
            var transferReq = new TransferRequest();
            transferReq._links = new Links();
            transferReq._links.destination = new Funding();
            transferReq._links.source = new Funding();
            transferReq._links.source.href = _appSettings.DwollaBaseLink + "/funding-sources/" + fundingSource;
            transferReq._links.destination.href = _appSettings.DwollaBaseLink + "/funding-sources/" + fundingDestination;
            transferReq.amount = new Amount();
            transferReq.amount.currency = "USD";
            transferReq.amount.value = ammount;

            WebRequest request = WebRequest.Create(_appSettings.DwollaBaseLink + "/transfers");
            request.Method = "Post";
            request.ContentType = "application/vnd.dwolla.v1.hal+json";
            request.PreAuthenticate = true;
            request.Headers.Add("Authorization", "Bearer " + GetToken());
            request.Headers.Add("Accept", "application/vnd.dwolla.v1.hal+json");

            using (var stream = new StreamWriter(request.GetRequestStream()))
            {
                stream.Write(JsonConvert.SerializeObject(transferReq));
                stream.Flush();
            }

            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            }
            catch (Exception e)
            {

            }
        }

        public string GetToken()
        {
            DwollaToken token = null;
            try
            {
                token = _context.DwollaToken.FirstOrDefault();
            }
            catch
            {

            }
            if (token == null ||  DateTime.Now > token.Expiration.AddMinutes(1))
            {
                if(token == null)
                {
                    token = new DwollaToken();
                }

                //Get Token from Dwolla
                WebRequest request = WebRequest.Create(_appSettings.DwollaBaseLink + "/token");
                request.Method = "Post";
                request.ContentType = "application/x-www-form-urlencoded";
                request.PreAuthenticate = true;
                request.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(_appSettings.DwollaKey + ":" + _appSettings.DwollaSecret)));
                using (var stream = new StreamWriter(request.GetRequestStream()))
                {
                    stream.Write("grant_type=client_credentials");
                    stream.Flush();
                }

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                DwollaTokenResponse tokenRes;
                using (var stream = new StreamReader(response.GetResponseStream()))
                {
                    tokenRes = JsonConvert.DeserializeObject<DwollaTokenResponse>(stream.ReadToEnd());
                }
                token.Expiration = DateTime.Now.AddSeconds(tokenRes.expires_in);
                token.Token = tokenRes.access_token;

                if (token.DwollaTokenId == null)
                {
                    token.DwollaTokenId = Guid.NewGuid();
                    _context.DwollaToken.Add(token);
                }
                else
                    _context.DwollaToken.Update(token);
                try
                {
                    _context.SaveChanges();

                }
                catch(Exception e)
                {

                }
            }

            return token.Token;
        }
    }
}
