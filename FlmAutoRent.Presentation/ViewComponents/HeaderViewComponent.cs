using System;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.AspNetCore.DataProtection;
using FlmAutoRent.Presentation.Models;
using FlmAutoRent.Services;

namespace FlmAutoRent.Presentation.ViewComponents
{
    public class HeaderViewComponent : ViewComponent
    {

         private readonly IHomePageService _homepageService;
        public HeaderViewComponent(IHomePageService homepageService){
            this._homepageService = homepageService;
        }   

        public IViewComponentResult Invoke(){
            var model = new HomepageViewModel();
            model.ContentCategories = _homepageService.GetContentCategories();

            var homepage = _homepageService.GetHomepage();
            model.ContentImageHeader = string.Concat("/", homepage.HeaderContentImage.FilePath.Replace("\\", "/"));
            model.WelcomeMessage = homepage.WelcomeMessage;

            return View(model);
        }

    }
}