using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using FlmAutoRent.Presentation.Areas.Admin.Models;
using FlmAutoRent.Services;

namespace FlmAutoRent.Presentation.Areas.Admin.ViewComponents
{
    public class NavAsideViewComponent : ViewComponent
    {
        private readonly IProfilingGroupServices _profilingGroupServices;

        public NavAsideViewComponent(IProfilingGroupServices profilingGroupServices){
            this._profilingGroupServices = profilingGroupServices;
        }   

        public IViewComponentResult Invoke(){
            if((ViewBag.HiddenMenu == null || !ViewBag.HiddenMenu) && ViewBag.HiddenMenuAside == null){
                if(User.Identity.Name != null){
                    var model = new MenuViewModel();

                    var menus = _profilingGroupServices.GetSystemMenusAside(Request.Path.Value, User.Identity.Name); 
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

                    
                    model.Menu = _profilingGroupServices.GetSystemMenus(User.Identity.Name);

                    return View(model);
                }
            }

            return new ContentViewComponentResult("");
        }

    }
}