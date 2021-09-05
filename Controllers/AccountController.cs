using CarInfoFromDatabase.Data;
using CarInfoFromDatabase.Services;
using CarInfoFromDatabase.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CarInfoFromDatabase.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly SignInManager<StoreUsers> _signInManager;
        private readonly IConfiguration _config;
        private readonly UserManager<StoreUsers> _userManager;



        public AccountController(ILogger<AccountController> logger
            , SignInManager<StoreUsers> signInManager,
            UserManager<StoreUsers> userManager,
            IConfiguration config)
        {
            this._logger = logger;
            this._userManager = userManager;
            this._config = config;
            this._signInManager = signInManager;
        }



        public IActionResult Login()
        {
            if(this.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index","Home");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
          if(ModelState.IsValid)
          {
                var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, false);

                if(result.Succeeded)
                {
                    if(Request.Query.Keys.Contains("ReturnUrl"))
                    {
                        return Redirect(Request.Query["ReturnUrl"].First());
                    }
                    else
                    {
                        return RedirectToAction("Index","Home");
                    }
                }

          }
            ModelState.AddModelError("","Failed to login");
            return View();
        }

        public async Task<IActionResult>Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index","Home");
        }


        [HttpPost]
        public async Task<IActionResult> CreateToken([FromBody] LoginViewModel model)
        {

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.UserName);

                if (user != null)
                {
                    var result = await _signInManager.CheckPasswordSignInAsync(user,model.Password,false);
                    if(result.Succeeded)
                    {
                        //create token

                        var claims = new[]
                        {
                            new Claim(JwtRegisteredClaimNames.Sub,user.Email),
                            new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                            new Claim(JwtRegisteredClaimNames.UniqueName,user.UserName)
                        };

                        var key = new SymmetricSecurityKey(Encoding.UTF32.GetBytes(_config["Token:Key"]));

                        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                        var token = new JwtSecurityToken(
                            _config["Token:Issuer"],
                            _config["Token:Audience"],
                            claims,
                            signingCredentials:creds,
                            expires:DateTime.UtcNow.AddMinutes(20)) ;

                        return Created("",new
                        {
                           token=new JwtSecurityTokenHandler().WriteToken(token),
                           expiration=token.ValidTo
                        });
                    }
                }

            }

            return BadRequest();

        }
    }
}
