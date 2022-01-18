using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Caffe.Models;
using Caffe.Models.ApiModels;
using Caffe.Models.MongoModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace Caffe.Controllers
{
    public class AuthControllers : Controller
    {
        private readonly UserManager<MongoUser> _userManager;
        private readonly SignInManager<MongoUser> _signInManager;
        private readonly Mail _mail;
        private readonly Dictionary<string, UserDto> _userById;

        public AuthControllers(UserManager<MongoUser> userManager, SignInManager<MongoUser> signInManager,
            Dictionary<string, UserDto> userById, Mail mail)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userById = userById;
            _mail = mail;
        }

        [HttpGet]
        [Route("api/Account/AcceptRegister/{id}")]
        public async Task<IActionResult> AcceptRegister(string id)
        {
            var user = _userById[id];
            var appUser = new MongoUser()
            {
                UserName = user.Email,
                Email = user.Email
            };
            var res = await _userManager.CreateAsync(appUser, user.Password);
            if (res.Succeeded)
            {
                await _signInManager.PasswordSignInAsync(appUser, user.Password, false, false);
                return Redirect("/user");
            }

            return BadRequest(res.Errors.First().Description);
        }

        [HttpPost]
        [Route("api/Account/Register")]
        public async Task<IActionResult> Register([FromBody] UserDto user)
        {
            if (string.IsNullOrEmpty(user.Email)
                || string.IsNullOrEmpty(user.Password))
                return BadRequest(new[] { "Необходимо заполнить все поля" });
            var appUser = new MongoUser()
            {
                Email = user.Email
            };

            if (await _userManager.FindByEmailAsync(user.Email) != null)
                return BadRequest(new[] { "Пользователь с такой почтой уже существует" });

            var enumerable = _userManager.PasswordValidators.Select(async x =>
                await x.ValidateAsync(_userManager, appUser, user.Password)).ToArray();
            await Task.WhenAll(enumerable);
            var identityResults = enumerable.Select(x => x.Result).ToList();
            if (identityResults.Any(x => !x.Succeeded))
            {
                return BadRequest(identityResults.SelectMany(x => x.Errors.Select(x => x.Description)));
            }

            SendVerificationMessage(user);
            return Ok(new[] { $@"На почту {user.Email} отправленно письмо для подтверждения" });
        }

        private void SendVerificationMessage(UserDto user)
        {
            var id = Math.Abs(DateTime.Now.GetHashCode() + user.Email.GetHashCode()).ToString();
            _userById[id] = user;
            Request.Headers.TryGetValue("Origin", out var url);
            _mail.SendCode($"{url}/api/Account/AcceptRegister/{id}", user.Email);
        }

        [HttpPost]
        [Route("api/Account/Login")]
        public async Task<IActionResult> Login([FromBody] UserDto user)
        {
            var appUser = await _userManager.FindByEmailAsync(user.Email);
            if (appUser != null)
            {
                var result = await _signInManager.PasswordSignInAsync(appUser, user.Password, false, false);
                if (result.Succeeded)
                {
                    return Ok("Успех");
                }
            }

            return BadRequest("Неверный логин или пароль");
        }

        [HttpGet]
        [Authorize]
        [Route("api/Account/Logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok();
        }


        [HttpPost]
        [Route("account/external-login")]
        public async Task<IActionResult> ExternalLogin(string provider, string returnUrl)
        {
            var redirectUrl =
                $"/account/external-auth-callback?returnUrl={returnUrl}";
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            properties.AllowRefresh = true;
            return Challenge(properties, provider);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("account/external-auth-callback")]
        public async Task<IActionResult> ExternalLoginCallback3([FromQuery] string returnUrl)
        {
            ExternalLoginInfo info = await _signInManager.GetExternalLoginInfoAsync();

            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                await _userManager.CreateAsync(new MongoUser()
                {
                    UserName = email,
                    Email = email
                });
                user = await _userManager.FindByEmailAsync(email);
            }

            await _signInManager.SignInAsync(user, false);


            return Redirect(returnUrl);
        }
    }
}