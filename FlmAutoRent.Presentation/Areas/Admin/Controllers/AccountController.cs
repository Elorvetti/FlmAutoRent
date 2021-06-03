using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using FlmAutoRent.Presentation.Areas.Admin.Models;
using FlmAutoRent.Services;

namespace FlmAutoRent.Presentation.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccountController : Controller
    {
        private readonly IOperatorServices _operatorServices;
        public AccountController(IOperatorServices operatorServices){
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
                    _operatorServices.UpdateProfilingOperatorPassword(model.AccountGuid, model.PasswordCrypt);
                    
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
                try{
                    if(_operatorServices.LoginOperatorSuccess(model.UserID, model.PasswordCrypt)){
                        var userLogged = _operatorServices.LoginOperator(model.UserID, model.PasswordCrypt);
                        var userLoggedGroup = userLogged.ProfilingOperatorGroups.FirstOrDefault().Groups.Name;

                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, model.UserID),
                            new Claim(ClaimTypes.Role, model.ConvertToEncrypt(userLoggedGroup))
                        };
                        var authProperties = new AuthenticationProperties{ };

                        ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                        return RedirectToAction("Index", "System",  new { area = "Admin" });
                    }
                } catch(Exception ex){
                    Console.WriteLine(ex);
                }
            }

            var errorModel = new ErrorViewModel();
            errorModel.Error = "Username o password errati";

            return View("500", errorModel);
            
        }
        
        public async Task<IActionResult> Logout(){
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }
        
    }
}