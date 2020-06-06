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
    public class AccountController : ControllerBase
    {
        private readonly ChariContext _context;
        private readonly AppSettings _appSettings;

        public AccountController(ChariContext context, IOptions<AppSettings> options)
        {
            _context = context;
            _appSettings = options.Value;
        }

        /// <summary>
        /// Updates users followed charity for recurring donations
        /// </summary>
        /// <returns></returns>
        [HttpPut("followcharity")]
        public ActionResult FollowChar(string charity)
        {
            User user;
            string tokenString = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            user = _context.User.First(x => x.Token == tokenString);

            var str = user.Id;
            var u = _context.Account.First(x => x.UserId == str);
            u.Charity = charity;
            _context.Account.Update(u);
            _context.SaveChanges();

            return Ok();

        }

        /// <summary>
        /// Returns users current followed charity
        /// </summary>
        /// <returns></returns>
        [HttpGet("currentcharity")]
        public ActionResult<string> CurrentChar()
        {
            User user;
            string tokenString = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            user = _context.User.First(x => x.Token == tokenString);

            var str = user.Id;
            var u = _context.Account.First(x => x.UserId == str);
            string currchar = u.Charity;
            return Ok(currchar);
        }

        /// <summary>
        /// Returns users current number of points achieved for milestone
        /// </summary>
        /// <returns></returns>
        [HttpGet("currentpoints")]
        public ActionResult<int> CurrentPoints()
        {
            User user;
            string tokenString = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            user = _context.User.First(x => x.Token == tokenString);

            var str = user.Id;
            var u = _context.Account.First(x => x.UserId == str);
            int currpoints = u.Points;
            return Ok(currpoints);
        }

    }
}
