using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FlmAutoRent.Presentation.Models;
using FlmAutoRent.Services;

namespace FlmAutoRent.Presentation.Controllers
{
    public class RewriteController : Controller
    {
        private readonly ILogger<RewriteController> _logger;
        private readonly IHomePageService _homepageService;
        private readonly IRewriteService _rewriteService;
        private readonly IImageService _imagesService;

        public RewriteController(ILogger<RewriteController> logger, IHomePageService homepageService, IImageService imagesService, IRewriteService rewriteService)
        {
            this._logger = logger;
            this._homepageService = homepageService;
            this._imagesService = imagesService;
            this._rewriteService = rewriteService;
        }

        [Route("/{category}")]
        public IActionResult Category(string category)
        {      
            var model = new CategoryViewModel();      
            
            var cat = _rewriteService.GetCategorieByPermalink(category);
            ViewData["Title"] = cat.Name;
            ViewData["MetaTitle"] = cat.MetaTitle;
            ViewData["MetaDescription"] = cat.MetaDescription;

            model.Id = cat.Id;
            model.Title = cat.Name;

            if(!cat.ContentCategoryNews.Any()){
                
                model.Description = cat.Description;

                var catChilds = _rewriteService.GetCategorieByIDFather(model.Id);
                foreach(var catChild in catChilds){
                    model.Cards.Add(new CardViewModel{
                        pathImg = string.Concat("/", catChild.ContentCategoryImages.FirstOrDefault(x => x.Categories.Id == catChild.Id).Images.FilePath.Replace("\\", "/")),
                        Title = catChild.Name,
                        Description = catChild.Description,
                        Permalink = string.Concat("/", cat.PermaLink, "/", catChild.PermaLink)
                    });
                }
            } else {
                var contentCategoryNewsList = _rewriteService.GetContentCategoryNews(model.Id);

                if(contentCategoryNewsList.Any()){
                    if(contentCategoryNewsList.Count() > 1){
                        foreach(var contentCategoryNews in contentCategoryNewsList){
                            model.Cards.Add(new CardViewModel{
                                pathImg = string.Concat("/", contentCategoryNews.News.ContentNewsImage.FirstOrDefault(x => x.ContentNews.Id == contentCategoryNews.News.Id).ContentImage.FilePath.Replace("\\", "/")),
                                Title = contentCategoryNews.News.Title,
                                Description = contentCategoryNews.News.Summary,
                                Permalink = string.Concat("/", cat.PermaLink, "/", contentCategoryNews.News.PermaLink)
                            });
                        }
                    }  else {
                        return RedirectToAction("Post", new{category = cat.PermaLink,  post = contentCategoryNewsList.FirstOrDefault().News.PermaLink});
                    }
                } else {
                    model.Description = "Non sono presente veicoli in vendita";
                }
           
            }
            

            return View("~/Views/Category/Index.cshtml", model);
        }

        [Route("/{category}/{post}")]
        public IActionResult Post(string category, string post)
        {            
            var model = new PostViewModel();

            var news = _rewriteService.GetContentNews(post);
            if(news != null){
                model.Id = news.Id;
                model.Title = news.Title;
                model.Description = news.Summary;
                model.DisplayText = news.News;

                ViewData["Title"] = news.Title;
                ViewData["MetaTitle"] = news.MetaTitle;
                ViewData["MetaDescription"] = news.MetaDescription;

                return View("~/Views/Post/Index.cshtml", model);
            } 

            var vehicleCategory = _rewriteService.GetContentCategoriesVehicles(post);
            foreach(var vehicle in vehicleCategory){
                model.Id = vehicle.ContentCategories.Id;
                model.Title = vehicle.ContentCategories.Name;
                model.Description = vehicle.ContentCategories.Description;
                
                ViewData["Title"] = vehicle.ContentCategories.Name;
                ViewData["MetaTitle"] = vehicle.ContentCategories.MetaTitle;
                ViewData["MetaDescription"] = vehicle.ContentCategories.MetaDescription;

                var brandPermaLink = vehicle.Vehicle.VehiclesMappings.FirstOrDefault(x => x.Vehicles.Id == vehicle.Vehicle.Id).Brands.PermaLink;
                var vehiclePermaLink = vehicle.Vehicle.PermaLink;

                model.Cards.Add(new VehicleCard{
                    srcImg = string.Concat("/", vehicle.Vehicle.VehiclesImages.FirstOrDefault(x => x.Vehicle.Id == vehicle.Vehicle.Id).Image.Path.Replace("\\", "/")),
                    vehiclebrand = vehicle.Vehicle.VehiclesMappings.FirstOrDefault(x => x.Vehicles.Id == vehicle.Vehicle.Id).Brands.BrandName,
                    vehicleModel = vehicle.Vehicle.Model,
                    vehicleDescription = vehicle.Vehicle.Description,
                    permaLink = string.Concat("/", category, "/", post, "/", brandPermaLink, "/", vehiclePermaLink)
                });
        }

            return View("~/Views/Post/Card.cshtml", model);
        }

        [Route("/{category}/{post}/{brand}")]
        public IActionResult VehicleBrand(string category, string post, string brand)
        {            
            var model = new HomepageViewModel();
            

            return RedirectToAction("Index", "Home");
        }
        
        [Route("/{category}/{post}/{brand}/{vehicle}")]
        public IActionResult Vehicle(string category, string post, string brand, string vehicle)
        {            
            var model = new VehicleViewModel();

            var vehicleData = _rewriteService.GetVehicle(vehicle);
            if(vehicleData != null){
                model.Id = vehicleData.Id;
                model.BrandImage = string.Concat("/", vehicleData.VehiclesMappings.FirstOrDefault(x => x.Vehicles.Id == vehicleData.Id).Brands.BrandImagePath.Replace("\\", "/"));
                model.Brand = vehicleData.VehiclesMappings.FirstOrDefault(x => x.Vehicles.Id == vehicleData.Id).Brands.BrandName;
                model.ModelName = vehicleData.Model;
                model.Description = vehicleData.Description;

                ViewData["Title"] = vehicleData.Model;
                ViewData["MetaTitle"] = vehicleData.MetaTitle;
                ViewData["MetaDescription"] = vehicleData.MetaDescription;

                foreach(var VehiclesImage in vehicleData.VehiclesImages){
                    model.vehiclesImages.Add(new VehiclesImage{
                        Id = VehiclesImage.Image.Id,
                        Path = string.Concat("/", VehiclesImage.Image.Path.Replace("\\", "/")),
                        Alt = VehiclesImage.Image.Description
                    });
                }
            }

            

            return View("~/Views/Vehicle/Index.cshtml", model);
        }
        
    }
}
