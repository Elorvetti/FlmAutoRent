using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using FlmAutoRent.Presentation.Areas.Admin.Models;
using FlmAutoRent.Services;

namespace FlmAutoRent.Presentation.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccountController : Controller
    {
        private readonly IDataProtectionProvider _dataProtectionProvider;
        private const string Key = "cxz92k13md8f981hu6y7alkc";
        private readonly IOperatorServices _operatorServices;
        public AccountController(IDataProtectionProvider dataProtectionProvider, IOperatorServices operatorServices){
            this._dataProtectionProvider = dataProtectionProvider;
            this._operatorServices = operatorServices;
        }
        
        [Route("Admin/Account/Password/{guid:guid}")]
        public IActionResult Password(Guid guid)
        {
            ViewBag.HiddenMenu = true;
            if(_operatorServices.ExistOperatorGuid(guid)){
                var model = new AccountPasswordViewModel();
                model.AccountGuid = guid;

                return View(model);
            }

            return View("500");

        }
        
        [Route("Admin/Account/Password/{guid:guid}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Password(AccountPasswordViewModel model){
            if(ModelState.IsValid){
                if(_operatorServices.ExistOperatorGuid(model.AccountGuid)){
                    var passwordCrypt = _dataProtectionProvider.CreateProtector(Key).Protect(model.Password);
                    _operatorServices.UpdateProfilingOperatorPassword(model.AccountGuid, passwordCrypt);
                    
                    return RedirectToAction("Login");
                }
            }
            
            return View("500");
        }
        
        [Route("/Admin")]
        public IActionResult Login(){
            ViewBag.HiddenMenu = true;
            var model = new AccountLoginViewModel();

            return View(model);
        }

        [Route("/Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(AccountLoginViewModel model){
            if(ModelState.IsValid){
                if(_operatorServices.LoginOperatorSuccess(model.UserID, model.Password)){
                    var Operator = _operatorServices.LoginOperator(model.UserID, model.Password);
                   
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, _dataProtectionProvider.CreateProtector(Key).Protect(Operator.UserId)),
                        new Claim(ClaimTypes.Role, _dataProtectionProvider.CreateProtector(Key).Protect(Operator.ProfilingOperatorGroups.FirstOrDefault().Groups.Name))
                    };
                    var authProperties = new AuthenticationProperties{ };

                    ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                    return RedirectToAction("Index", "System",  new { area = "Admin" });
                }

            }

            return View("500");
            
        }
        
        public async Task<IActionResult> Logout(){
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }
        
    }
}