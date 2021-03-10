using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.AspNetCore.DataProtection;
using FlmAutoRent.Presentation.Models;
using FlmAutoRent.Services;

namespace FlmAutoRent.Presentation.ViewComponents
{
    public class FooterViewComponent : ViewComponent
    {

         private readonly IHomePageService _homepageService;
        public FooterViewComponent(IHomePageService homepageService){
            this._homepageService = homepageService;
        }   

        public IViewComponentResult Invoke(){
            var model = new HomepageViewModel();
            var newsFooter = _homepageService.GetContentNewsOnFooter();

            foreach(var news in newsFooter){
                model.NewsOnFooter.Add(new GetContentNewsOnFooter{
                    PageName = news.Title,
                    Permalink = string.Concat("/", news.ContentCategoryNews.FirstOrDefault(x => x.News.Id == news.Id).ContentCategories.PermaLink, "/", news.PermaLink)
                });
            }

            var homepage = _homepageService.GetHomepage();
            model.ContentImageHeader = string.Concat("/", homepage.HeaderContentImage.FilePath.Replace("\\", "/"));
            model.WelcomeMessage = homepage.WelcomeMessage;

            return View(model);
        }

    }
}