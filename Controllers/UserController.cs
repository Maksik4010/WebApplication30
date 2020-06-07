using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.V3.Pages.Account.Internal;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication30.Data.Managers;
using WebApplication30.Models;
using WebApplication30.Models.JwtToken;
using WebApplication30.Models.Parameters;

namespace WebApplication30.Controllers
{
    
    public class UserController : ControllerBase
    {
        // Properties
        public AppUserManager UserManager { get; set; }

        // Constructor
        public UserController(AppUserManager userManager)
        {
            UserManager = userManager;
        }

        // Methods
        [HttpPost]
        [AllowAnonymous]
        [Route("User/Token")]
        public async Task<IActionResult> Login([FromBody]Models.Parameters.LoginModel inputModel)
        {
            var user = UserManager.Users.FirstOrDefault(u => u.UserName == inputModel.Username);

            if (user == null)
            {
                return Unauthorized();
            }

            var isAuthenticated = await UserManager.CheckPasswordAsync(user, inputModel.Password);

            if (!isAuthenticated)
            {
                return Unauthorized();
            }

            var token = new JwtTokenBuilder()
                                .AddSecurityKey(JwtSecurityKey.Create("fiver-secret-key"))
                                .AddSubject("james bond")
                                .AddIssuer("Fiver.Security.Bearer")
                                .AddAudience("Fiver.Security.Bearer")
                                .AddClaim("MembershipId", "111")
                                .AddExpiry(1)
                                .Build();

            if (isAuthenticated)
            {
                var result = await UserManager.SetAuthenticationTokenAsync(user, "Default", "Token", token.Value);
            }

            // return Ok(token);
            return Ok(token.Value);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("User/Create")]
        public async Task<ActionResult> Create([FromBody]CreateUserModel model)
        {
            var user = new AppUser()
            {
                UserName = model.UserName,
                Email = model.Email,
            };

            var result = await UserManager.CreateAsync(user, model.Password);

            if (result.Errors.Any())
            {
                return BadRequest();
            }

            return Ok();
        }
    }
}

