using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ChariAPI.Models;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Newtonsoft.Json;
using ChariAPI.Classes;

namespace ChariAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ChariContext _context;
        private readonly AppSettings _appSettings;

        public UsersController(ChariContext context, IOptions<AppSettings> options)
        {
            _context = context;
            _appSettings = options.Value;
        }

        /// <summary>
        /// Get information of logged in user
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<User> GetUser()
        {
            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            User user = _context.User.FirstOrDefault(x => x.Token == token);

            
            return Ok(user.WithoutPassword());
        }

        /// <summary>
        /// Create a new user
        /// </summary>
        /// <remarks>
        /// Authentication is not needed &#xD;
        /// Id, Token, and TokenExpiration don't need to be included in request body &#xD;
        /// All other fields are required
        /// </remarks>
        [AllowAnonymous]
        [HttpPost("createuser")]
        public ActionResult<User> CreateUser(User user)
        {
            user.Id = Guid.NewGuid();

            //Create token
            var tokenHandler = new JwtSecurityTokenHandler();
            var secretKey = Encoding.ASCII.GetBytes(_appSettings.SecretKey);
            var expiration = DateTime.UtcNow.AddMinutes(30);
            var tokenDesc = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = expiration,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDesc);

            user.Password = HashPSW.CreateHash(user.Password);
            //Save token info
            user.Token = tokenHandler.WriteToken(token);
            user.TokenExpiration = expiration;

            Account acc = new Account();
            acc.UserId = user.Id;
            acc.AccountId = Guid.NewGuid();

            

            try
            {
                _context.User.Add(user);
                _context.SaveChanges();
                _context.Account.Add(acc);
                _context.SaveChanges();
            }
            catch(Exception e)
            {

            }

            return Ok(user);
        }

        ///<summary>
        ///Login and receive Token
        ///</summary>
        ///<remarks>
        ///Authentication is not needed &#xD;
        ///TokenExpiration is in UTC
        ///</remarks>
        [AllowAnonymous]
        [HttpPost("login")]
        public ActionResult<User> Login(Login login)
        {
            //var newUser = _userService.Authenticate(user.Email, user.Password);

            var user = _context.User.FirstOrDefault(x => x.Email == login.Email);


            if (user == null || !HashPSW.ValidatePassword(login.Password,user.Password))
                return BadRequest();

            //Create token
            var tokenHandler = new JwtSecurityTokenHandler();
            var secretKey = Encoding.ASCII.GetBytes(_appSettings.SecretKey);
            var expiration = DateTime.UtcNow.AddMinutes(30);
            var tokenDesc = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = expiration,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDesc);

            //Save token info
            user.Token = tokenHandler.WriteToken(token);
            user.TokenExpiration = expiration;
            _context.User.Update(user);
            _context.SaveChanges();

            return Ok(user.WithoutPassword());
        }

        /// <summary>
        /// Refresh users token
        /// </summary>
        /// <returns></returns>
        [HttpPost("refreshtoken")]
        public ActionResult<User> RefreshToken()
        {
            //Find user based on their current active token
            string tkn = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var user = _context.User.First(x => x.Token == tkn);

            if (user == null)
                return BadRequest();

            //Create new token
            var tokenHandler = new JwtSecurityTokenHandler();
            var secretKey = Encoding.ASCII.GetBytes(_appSettings.SecretKey);
            var expiration = DateTime.UtcNow.AddMinutes(30);
            var tokenDesc = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = expiration,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDesc);

            //Save new token info
            user.Token = tokenHandler.WriteToken(token);
            user.TokenExpiration = expiration;
            _context.User.Update(user);
            _context.SaveChanges();

            return Ok(user.WithoutPassword());
        }

        

    }

}
