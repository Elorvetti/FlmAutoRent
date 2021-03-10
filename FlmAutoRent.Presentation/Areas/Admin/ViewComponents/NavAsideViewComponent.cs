using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.AspNetCore.DataProtection;
using FlmAutoRent.Presentation.Areas.Admin.Models;
using FlmAutoRent.Services;

namespace FlmAutoRent.Presentation.Areas.Admin.ViewComponents
{
    public class NavAsideViewComponent : ViewComponent
    {
        private readonly IDataProtectionProvider _dataProtectionProvider;
        private const string Key = "cxz92k13md8f981hu6y7alkc";
        private readonly IProfilingGroupServices _profilingGroupServices;

        public NavAsideViewComponent(IDataProtectionProvider dataProtectionProvider, IProfilingGroupServices profilingGroupServices){
            this._dataProtectionProvider = dataProtectionProvider;
            this._profilingGroupServices = profilingGroupServices;
        }   

        public IViewComponentResult Invoke(){
            if((ViewBag.HiddenMenu == null || !ViewBag.HiddenMenu) && ViewBag.HiddenMenuAside == null){
                if(User.Identity.Name != null){
                    var UserId = _dataProtectionProvider.CreateProtector(Key).Unprotect(User.Identity.Name);
                    var model = new MenuViewModel();

                    var menus = _profilingGroupServices.GetSystemMenusAside(Request.Path.Value, UserId); 
                        foreach( var menu in menus){
                        model.MenuAside.Add(new MenuDisplayViewModel{
                            Active = Request.Path.Value.Contains(menu.Link) || Request.Path.Value.Contains(string.Concat(menu.Link, "AddOrEdit")) ? "active" : "",
                            Id = menu.Id,
                            CodMenu = menu.CodMenu,
                            DisplayHeader = menu.DisplayHeader,
                            Link = menu.Link,
                            MenuFatherId = menu.MenuFatherId,
                            Name = menu.Name,
                            Priority = menu.Priority,
                            ProfilingGroupSystemMenus = menu.ProfilingGroupSystemMenus,
                            Visible = menu.Visible
                        });
                    }

                    
                    model.Menu = _profilingGroupServices.GetSystemMenus(UserId);

                    return View(model);
                }
            }

            return new ContentViewComponentResult("");
        }

    }
}