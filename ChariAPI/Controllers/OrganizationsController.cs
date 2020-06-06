using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.IO;
using ChariAPI.Models;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authorization;

namespace ChariAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrganizationController : ControllerBase
    {
        private readonly ChariContext _context;
        private readonly AppSettings _appSettings;

        public OrganizationController(ChariContext context, IOptions<AppSettings> options)
        {
            _context = context;
            _appSettings = options.Value;
        }


        /// <summary>
        /// Retrieve list of organizations based on search criteria
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("getorgs")]
        public ActionResult<string> GetOrgs(OrgSearchCriteria Search)
        {
            string strURL = _appSettings.CharityNavOrgSearchBaseLink; 
            if (Search.pageSize != null)
            {
                strURL = strURL + "&pageSize=" + Search.pageSize;
            }
            if (Search.searchTerm != null)
            {
                strURL = strURL + "&search=" + Search.searchTerm;
            }
            if (Search.state != null)
            {
                strURL = strURL + "&state=" + Search.state;
            }
            if (Search.city != null)
            {
                strURL = strURL + "&city=" + Search.city;
            }
            if (Search.zip != null)
            {
                strURL = strURL + "&zip=" + Search.zip;
            }
            if (Search.minRating != null)
            {
                strURL = strURL + "&minRating=" + Search.minRating;
            }
            if (Search.maxRating != null)
            {
                strURL = strURL + "&maxRating=" + Search.maxRating;
            }
            if (Search.scopeOfWork != null)
            {
                strURL = strURL + "&scopeOfWork=" + Search.scopeOfWork;
            }
            if (Search.sort != null)
            {
                strURL = strURL + "&sort=" + Search.sort;
            }
            WebRequest requestobjGet = WebRequest.Create(strURL);
            requestobjGet.Method = "GET";
            requestobjGet.ContentType = "application/json";
            HttpWebResponse responseobjGet = null;
            responseobjGet = (HttpWebResponse)requestobjGet.GetResponse();
            string strresult = null;
            using (Stream stream = responseobjGet.GetResponseStream())
            {
                StreamReader sr = new StreamReader(stream);
                strresult = sr.ReadToEnd();
                sr.Close();
            }
            return Ok(strresult);
        }

        /// <summary>
        /// Retrieve details of specific organization
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("getorgdets")]
        public ActionResult<string> GetOrgDetails(string ein)
        {
            string strURL = _appSettings.CharityNavBaseLink + "/Organizations/" + ein + "?app_id=" + _appSettings.CharityNavAppID + "&app_key=" + _appSettings.CharityNavAppKey;
            WebRequest requestobjGet = WebRequest.Create(strURL);
            requestobjGet.Method = "GET";
            requestobjGet.ContentType = "application/json";
            HttpWebResponse responseobjGet = null;
            responseobjGet = (HttpWebResponse)requestobjGet.GetResponse();
            string strresult = null;
            using (Stream stream = responseobjGet.GetResponseStream())
            {
                StreamReader sr = new StreamReader(stream);
                strresult = sr.ReadToEnd();
                sr.Close();
            }
            return Ok(strresult);
        }


        /// <summary>
        /// Check if specific organization has active advisory warnings, and provide warning details if so
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("getorgwarnings")]
        public ActionResult<string> OrgAdvisoryWarnings(string ein)
        {
            string strURL = _appSettings.CharityNavBaseLink + "/Organizations/" + ein + "/Advisories?app_id=" + _appSettings.CharityNavAppID + "&app_key=" + _appSettings.CharityNavAppKey + "&status=ACTIVE" ;
            WebRequest requestobjGet = WebRequest.Create(strURL);
            requestobjGet.Method = "GET";
            requestobjGet.ContentType = "application/json";
            HttpWebResponse responseobjGet = null;
            responseobjGet = (HttpWebResponse)requestobjGet.GetResponse();
            string strresult = null;
            using (Stream stream = responseobjGet.GetResponseStream())
            {
                StreamReader sr = new StreamReader(stream);
                strresult = sr.ReadToEnd();
                sr.Close();
            }
            return Ok(strresult);
        }

    }

}