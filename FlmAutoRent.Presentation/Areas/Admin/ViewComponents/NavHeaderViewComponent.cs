using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.AspNetCore.DataProtection;
using FlmAutoRent.Presentation.Areas.Admin.Models;
using FlmAutoRent.Presentation.Models;
using FlmAutoRent.Services;

namespace FlmAutoRent.Presentation.Areas.Admin.ViewComponents
{
    public class NavHeaderViewComponent : ViewComponent
    {
        private readonly IProfilingGroupServices _profilingGroupServices;

        public NavHeaderViewComponent(IProfilingGroupServices profilingGroupServices){
            this._profilingGroupServices = profilingGroupServices;
        }   

        public IViewComponentResult Invoke(){
            if(ViewBag.HiddenMenu == null || !ViewBag.HiddenMenu){
                if(User.Identity.Name != null){
                    var model = new MenuViewModel();
                    var menus = _profilingGroupServices.GetSystemMenusHeader(User.Identity.Name); 
                    
                    foreach(var menu in menus){
                        model.MenuHeader.Add(new MenuDisplayViewModel{
                            Active = Request.Path.Value.Contains(menu.CodMenu) ? "active" : "",
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

                    return View(model);
                }
            }

            return new ContentViewComponentResult("");
        }

    }
}