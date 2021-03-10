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
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHomePageService _homepageService;
        private readonly IImageService _imagesService;

        public HomeController(ILogger<HomeController> logger, IHomePageService homepageService, IImageService imagesService)
        {
            _logger = logger;
            this._homepageService = homepageService;
            this._imagesService = imagesService;

        }

        public IActionResult Index()
        {            
            ViewData["Title"] = "Home";
            
            var model = new HomepageViewModel();
            
            var vehicles = _homepageService.GetVehicles();
            foreach(var vehicle in vehicles){
                var categoryIDToFind = vehicle.ContentCategoryNews.FirstOrDefault(x => x.Vehicle.Id == vehicle.Id).ContentCategories.IdFather;
                var category = _homepageService.GetContentCategory(categoryIDToFind);
                var catgoryChildPermalink = vehicle.ContentCategoryNews.FirstOrDefault(x => x.Vehicle.Id == vehicle.Id).ContentCategories.PermaLink;
                var brandPermalink = vehicle.VehiclesMappings.FirstOrDefault(x => x.Vehicles.Id == vehicle.Id).Brands.PermaLink;

                model.Vehicles.Add(new VehicleCard{
                    Id = vehicle.Id,
                    srcImg = string.Concat("/", vehicle.VehiclesImages.FirstOrDefault( x => x.Vehicle.Id == vehicle.Id).Image.Path.Replace("\\", "/")),
                    altImg = vehicle.VehiclesImages.FirstOrDefault( x => x.Vehicle.Id == vehicle.Id).Image.Description,
                    vehiclebrand = vehicle.VehiclesMappings.FirstOrDefault( x => x.Vehicles.Id == vehicle.Id).Brands.BrandName,
                    vehicleModel = vehicle.Model,
                    vehicleDescription = vehicle.Description,
                    permaLink =  string.Concat("/", category.PermaLink , "/", catgoryChildPermalink ,"/", brandPermalink, "/", vehicle.PermaLink)
                });
            }

            var images = _imagesService.GetContentImages();
            foreach(var image in images){

                model.HeaderImagesList.Add(new FileViewModel{
                    Id = image.Id,
                    FilePath = string.Concat("/", image.FilePath.Replace("\\", "/")),
                    Name = image.FileName,
                });

                model.PresentationImagesList.Add(new FileViewModel{
                    Id = image.Id,
                    FilePath = string.Concat("/", image.FilePath.Replace("\\", "/")),
                    Name = image.FileName,
                });

            }
            
            model.ContentCategories = _homepageService.GetContentCategories();
            
            var homepage = _homepageService.GetHomepage();
            model.HeaderImageListSelected = homepage.HeaderImageId;
            model.ContentImageHeader = string.Concat("/", homepage.HeaderContentImage.FilePath.Replace("\\", "/"));
            model.WelcomeMessage = homepage.WelcomeMessage;
            model.PresentationImageListSelected = homepage.PresentationImageId;
            model.ContentImagePresentation = string.Concat("/", homepage.PresentationContentImage.FilePath.Replace("\\", "/"));
            model.Presentation = homepage.PresentationMessage;


            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
