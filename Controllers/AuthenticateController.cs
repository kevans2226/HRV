using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using HRV.Data.Db;
using HRV.Data.View;

namespace HRV.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private IConfiguration _config;

        public AuthenticateController(UserManager<ApplicationUser> userManager, IConfiguration config)
        {
            this._userManager = userManager;
            this._config = config;
        }
        private async Task<bool> CheckSid()
        {
            var user = HttpContext.User;

            var userInDb = await _userManager.FindByNameAsync(user.Identity.Name);

            return (userInDb != null);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            ApplicationUser user = await _userManager.FindByEmailAsync(model.UserName);
            var testCred = await _userManager.CheckPasswordAsync(user, model.Password);
            if (user != null && testCred)
            {
                return CreateToken(user); 
            }
            return Unauthorized();
        }

        [HttpGet("refresh")]
        public async Task<IActionResult> RefreshToken()
        {
            var user = HttpContext.User;
            var userInDb = await _userManager.FindByNameAsync(user.Identity.Name);

            if(userInDb != null)
            {
                return CreateToken(userInDb); 
            }

            return Unauthorized("Token Invalid");
        }


        private IActionResult CreateToken(ApplicationUser user)
        {
            var authClaims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Sid, user.Id)
                };
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Token:Key"]));
            var token = new JwtSecurityToken(
                issuer: _config["Token:Issuer"],
                audience: _config["Token:Audience"],
                expires: DateTime.Now.AddDays(5),
                claims: authClaims,
                signingCredentials: new Microsoft.IdentityModel.Tokens.SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );
            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo,
                userName = $"{user.UserName}"
            });
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("createUser")]
        public async Task<IActionResult> CreateUser(NewUser newUser)
        {
            ApplicationUser user = new ApplicationUser()
            {
                Email =  newUser.EmailAddress,
                UserName = newUser.UserName,
                SecurityStamp = Guid.NewGuid().ToString(),
                FirstName = newUser.FirstName,
                LastName = newUser.LastName
            };

            var record = await _userManager.CreateAsync(user, newUser.Password);

            return Created("/home", new { UserName = newUser.UserName });
        }

        [Authorize]
        [HttpGet("check")]
        public async Task<IActionResult> CheckUser()
        {
            var bol = await CheckSid();

            return Ok(new { TokenValiden = bol });
        }

        [Authorize]
        [HttpPut]
        [Route("passwordChange")]
        public async Task<IActionResult> ChangePassword(PasswordChange password)
        {
            var usr = HttpContext.User;
            var user = await _userManager.FindByNameAsync(usr.Identity.Name);
            var testCred = await _userManager.CheckPasswordAsync(user, password.OldPassword);

            if(testCred)
            {
                await _userManager.ChangePasswordAsync(user, password.OldPassword, password.NewPassword);
                return Accepted(); 
            }
            else
            {
                return StatusCode(400, "Password is incorrect"); 
            }
        }

        [Authorize]
        [HttpPut]
        [Route("updateNames")]
        public async Task<IActionResult> UpdateNames(NewName newName)
        {
            var usr = HttpContext.User;
            var user = await _userManager.FindByNameAsync(usr.Identity.Name);
            user.LastName = newName.LastName;
            user.FirstName = newName.FirstName;
            user.Email = newName.EmailAddress;

            await _userManager.UpdateAsync(user);

            return Accepted(); 
        }
    }
}