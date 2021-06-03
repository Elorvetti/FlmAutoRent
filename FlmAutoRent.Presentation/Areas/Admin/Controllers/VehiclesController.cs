using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using FlmAutoRent.Presentation.Areas.Admin.Models;
using FlmAutoRent.Data.Entities;
using FlmAutoRent.Services;
using Microsoft.AspNetCore.DataProtection;

namespace FlmAutoRent.Presentation.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class VehiclesController : Controller
    {
        private readonly IVehiclesBrandsServices _vehiclesBrandService;
        private readonly ICarServices _carService;
        private readonly ICategoryService _categoryService;
        private readonly IFileService _fileService;
        private readonly ICommonService _commonService;
        private readonly IOperatorServices _operatorServices;
        private readonly ILogServices _logService;
        private readonly IDataProtectionProvider _dataProtectionProvider;
        private const string Key = "cxz92k13md8f981hu6y7alkc";
        
        public VehiclesController(IVehiclesBrandsServices vehiclesBrandService, ICarServices carService, ICategoryService categoryService, IFileService fileService, ICommonService commonService, IOperatorServices operatorServices, ILogServices logServices, IDataProtectionProvider dataProtectionProvider){
            this._vehiclesBrandService = vehiclesBrandService;
            this._carService = carService;
            this._categoryService = categoryService;
            this._fileService = fileService;
            this._commonService = commonService;
            this._operatorServices = operatorServices;
            this._logService = logServices;
            this._dataProtectionProvider = dataProtectionProvider;
        }
        
        public IActionResult Index(){
            return RedirectToAction("Brands");
        }

        public IActionResult Brands(int pageSize = 10, int pageNumber = 1){
            ViewData["Title"] = "Brand";
            var model = new BrandsTableViewModel();
            
            model.HowManyField = pageSize;

            //Pagination
            model.totalRecords = _vehiclesBrandService.GetVehiclesBrands().Count;
            model.pageNumber = pageNumber;
            model.pageSize = pageSize;
            model.pageTotal =  Math.Ceiling((double)model.totalRecords / pageSize);
            var excludeRecords = (pageSize * pageNumber) - pageSize;  
            model.displayRecord = model.pageSize * pageNumber;
            if(model.displayRecord > model.pageTotal){
                model.displayRecord = model.totalRecords;
            }

            if(model.totalRecords > pageSize){
                model.displayPagination = true;
            }

            var brands = _vehiclesBrandService.GetVehiclesBrands(excludeRecords, pageSize);
            model.BrandsListViewModel = new List<BrandsViewModel>();
            
            foreach(var brand in brands){
                model.BrandsListViewModel.Add(new BrandsViewModel{ 
                    Id = brand.Id, 
                    Name = brand.BrandName,
                    PathLogo = string.Concat("/", brand.BrandImagePath.Replace("\\", "/")),
                    Nusing =_vehiclesBrandService.VehicleBrandUsage(brand.Id)
                });        
            }
         
            return View(model);
        }

        [HttpPost]
        public IActionResult Brands(BrandsTableViewModel model, int pageNumber = 1){
            ViewData["Title"] = "Brand";

            //Pagination
            model.totalRecords = _vehiclesBrandService.GetVehiclesBrandsByName(model.Find).Count;
            model.pageNumber = pageNumber;
            model.pageSize = model.HowManyField;
            model.pageTotal =  Math.Ceiling((double)model.totalRecords / model.HowManyField);
            var excludeRecords = (model.HowManyField * pageNumber) - model.HowManyField;  
            model.displayRecord = model.pageSize * pageNumber;
            if(model.displayRecord > model.pageTotal){
                model.displayRecord = model.totalRecords;
            }

            if(model.totalRecords > model.HowManyField){
                model.displayPagination = true;
            }
            
            var brands = _vehiclesBrandService.GetVehiclesBrandsByName(model.Find, excludeRecords, model.pageSize);
            model.BrandsListViewModel = new List<BrandsViewModel>();            
            foreach(var brand in brands){
                model.BrandsListViewModel.Add(new BrandsViewModel{ 
                    Id = brand.Id, 
                    Name = brand.BrandName,
                    PathLogo = string.Concat("/", brand.BrandImagePath.Replace("\\", "/")),
                    Nusing = brand.VehiclesMappings.Where(x => x.Brands.Id == brand.Id).Count() //brand.VehiclesMappings.Where(x => x.Brands.Id == brand.Id).Count()
                });        
            }
         
            return View(model);
        }

        public IActionResult BrandsAddOrEdit(int id){
            ViewData["Title"] = "Aggiungi Brand";
            var model = new BrandAddViewModel();

            //SEO
            model.SeoIndex =  _vehiclesBrandService.GetListSeoIndex().Select(x => new SelectListItem{
                Value = x.Id.ToString(),
                Text = x.Description,
            });

            if(id != 0){
                var brand = _vehiclesBrandService.GetVehiclesBrandById(id);
                model.Id = id;
                model.Nome = brand.BrandName;
                model.Description = brand.Description;
                model.LogoPath =  string.Concat("/", brand.BrandImagePath.Replace("\\", "/"));
                model.PermaLink =  brand.PermaLink;
                model.MetaDescription =  brand.MetaDescription;
                model.MetaTitle =  brand.MetaTitle;
                model.SeoIndexId = brand.SeoIndexRef;
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult BrandsAddOrEdit(BrandAddViewModel model){
           var entityBrand = new VehiclesBrand();
           
            if(ModelState.IsValid){
                var UserId = _dataProtectionProvider.CreateProtector(Key).Unprotect(User.Identity.Name);
                
                if(model.Id == 0){
                    var fileExtension = new[] { "image/gif", "image/jpg", "image/jpeg", "image/tiff", "image/png" };
            
                    if(_fileService.fileExtensionOk(model.Upload.ContentType, fileExtension)){
                        
                        //UPLOAD FILE
                        var fileName = string.Concat(model.Nome.Replace(" ", "-"), '_', Guid.NewGuid().ToString().Substring(0, 4), Path.GetExtension(model.Upload.FileName));
                        _fileService.UploadFile(Path.Combine("vehicles", "brand"), fileName, model.Upload);

                        //SAVE DATA ON DB
                        entityBrand.BrandName = model.Nome;
                        entityBrand.Description = model.Description;
                        entityBrand.BrandImagePath = Path.Combine("upload", "vehicles", "brand", fileName);
                        entityBrand.OperatorData = DateTime.Now;
                        entityBrand.IDOperator = _operatorServices.GetOperatorByUserId(UserId).Id;

                        //SALVO DATI SEO
                        if(model.PermaLink == null || model.PermaLink == string.Empty){
                            entityBrand.PermaLink = _commonService.cleanStringPath(model.Nome);
                        } else {
                            entityBrand.PermaLink = _commonService.cleanStringPath(model.PermaLink);
                        }

                        if(model.MetaDescription == null || model.MetaDescription == string.Empty){
                            entityBrand.MetaDescription = model.Description != null && model.Description.Length > 500 ? model.Description.Substring(0, 500) : model.Description;
                        } else {
                            entityBrand.MetaDescription = model.MetaDescription;
                        }

                        if(model.MetaTitle == null || model.MetaTitle == string.Empty){
                            entityBrand.MetaTitle = model.Nome;
                        } else {
                            entityBrand.MetaTitle = model.MetaTitle;
                        }

                        entityBrand.SeoIndexRef = model.SeoIndexId;
                        entityBrand.SeoIndex = _carService.GetContentNewsSeoIndexById(model.SeoIndexId);
                        entityBrand.SeoIndex.VehiclesBrand = entityBrand;

                        _vehiclesBrandService.InsertBrand(entityBrand);

                        return RedirectToAction("Brands");
                    }

                } else {
                    entityBrand.Id = model.Id;
                    entityBrand.BrandName = model.Nome;
                    entityBrand.Description = model.Description;   
                    entityBrand.OperatorData = DateTime.Now;
                    entityBrand.IDOperator = _operatorServices.GetOperatorByUserId(UserId).Id;
                    
                    //SALVO DATI SEO
                    if(model.PermaLink == null || model.PermaLink == string.Empty){
                        entityBrand.PermaLink = _commonService.cleanStringPath(model.Nome);
                    } else {
                        entityBrand.PermaLink = _commonService.cleanStringPath(model.PermaLink);
                    }

                    if(model.MetaDescription == null || model.MetaDescription == string.Empty){
                        entityBrand.MetaDescription = model.Description != null && model.Description.Length > 500 ? model.Description.Substring(0, 500) : model.Description;
                    } else {
                        entityBrand.MetaDescription = model.MetaDescription;
                    }

                    if(model.MetaTitle == null || model.MetaTitle == string.Empty){
                        entityBrand.MetaTitle = model.Nome;
                    } else {
                        entityBrand.MetaTitle = model.MetaTitle;
                    }
                    
                    _vehiclesBrandService.UpdateBrand(entityBrand);
                }
            
               return RedirectToAction("Brands");
            }

            return View("500");
        }

        public IActionResult BrandDelete(int id){
            //DELETE PHISYCAL IMAGE
            var brand = _vehiclesBrandService.GetVehiclesBrandById(id);
            _fileService.DeleteFile(brand.BrandImagePath);
            
            _vehiclesBrandService.DeleteBrand(id);

            return RedirectToAction("Brands");
        }

        public IActionResult Cars(int pageSize = 10, int pageNumber = 1){
            ViewData["Title"] = "Veicoli";
            var model = new CarsTableViewModel();

            model.HowManyField = pageSize;

            //Pagination
            model.totalRecords = _carService.GetVehicles().Count;
            model.pageNumber = pageNumber;
            model.pageSize = pageSize;
            model.pageTotal =  Math.Ceiling((double)model.totalRecords / pageSize);
            var excludeRecords = (pageSize * pageNumber) - pageSize;  
            model.displayRecord = model.pageSize * pageNumber;
            if(model.displayRecord > model.pageTotal){
                model.displayRecord = model.totalRecords;
            }

            if(model.totalRecords > pageSize){
                model.displayPagination = true;
            }

            var cars = _carService.GetVehicles(excludeRecords, pageSize);
            model.CarsListViewModel = new List<CarsViewModel>();
            
            foreach(var car in cars){
                model.CarsListViewModel.Add(new CarsViewModel{ 
                    Id = car.Id, 
                    PathLogo = string.Concat("/", car.VehiclesMappings.FirstOrDefault(x => x.Vehicles.Id == car.Id).Brands.BrandImagePath.Replace("\\", "/")),
                    Category = car.ContentCategoryNews.FirstOrDefault(x => x.Vehicle.Id == car.Id).ContentCategories.Name,
                    Model = car.Model,
                    Display = car.Bookable ? "SI" : "NO",
                    TotalRequest = car.PeopleMessages.Where(x => x.Vehicle.Id == car.Id).Count()
                });        
            }
            
            return View(model);
        }

        [HttpPost]
        public IActionResult Cars(CarsTableViewModel model, int pageNumber = 1){
            ViewData["Title"] = "Veicoli";

            //Pagination
            model.totalRecords = _carService.GetVehiclesByName(model.Find).Count;
            model.pageNumber = pageNumber;
            model.pageSize = model.HowManyField;
            model.pageTotal =  Math.Ceiling((double)model.totalRecords / model.HowManyField);
            var excludeRecords = (model.HowManyField * pageNumber) - model.HowManyField;  
            model.displayRecord = model.pageSize * pageNumber;
            if(model.displayRecord > model.pageTotal){
                model.displayRecord = model.totalRecords;
            }

            if(model.totalRecords > model.HowManyField){
                model.displayPagination = true;
            }

            var cars = _carService.GetVehiclesByName(model.Find, excludeRecords, model.pageSize);
            model.CarsListViewModel = new List<CarsViewModel>();
            
            foreach(var car in cars){
                model.CarsListViewModel.Add(new CarsViewModel{ 
                    Id = car.Id, 
                    PathLogo = string.Concat("/", car.VehiclesMappings.FirstOrDefault(x => x.Vehicles.Id == car.Id).Brands.BrandImagePath.Replace("\\", "/")),
                    Category = car.ContentCategoryNews.FirstOrDefault(x => x.Vehicle.Id == car.Id).ContentCategories.Name,
                    Model = car.Model,
                    Display = car.Bookable ? "SI" : "NO",
                    TotalRequest = car.PeopleMessages.Where(x => x.Vehicle.Id == car.Id).Count()
                });        
            }
            
            return View(model);
        }
    
        public IActionResult CarsFirstStep(int id){
            ViewData["Title"] = "Aggiungi Veicolo";
            var model = new CarAddViewModel();
            
            //GET CATEGORY 
            var categories = _carService.GetVehicleCategory();
            foreach(var category in categories){
                model.carFirstStep.Categories.Add(category);
            }

            model.carFirstStep.BrandList =  _vehiclesBrandService.GetVehiclesBrands().Select(x => new SelectListItem{
                Value = x.Id.ToString(),
                Text = x.BrandName,
            });
            
            model.carFirstStep.PowerSupplyList =  _carService.GetVehiclePowerSupply().Select(x => new SelectListItem{
                Value = x.Id.ToString(),
                Text = x.PowerSupply,
            });

            model.disableSecondStep = true;
            model.disableThirdStep = true;
            model.disableFourStep = true;

            if(id != 0){
                var car = _carService.GetVehicleById(id);
                model.Id = id;
                model.carFirstStep.CategorySelected = car.ContentCategoryNews.FirstOrDefault(x => x.Vehicle.Id == id).ContentCategories.Id;
                model.carFirstStep.BrandId = car.VehiclesMappings.FirstOrDefault(x => x.Vehicles.Id == id).Brands.Id;
                model.carFirstStep.Model = car.Model;
                model.carFirstStep.Description = car.Description;
                model.carFirstStep.PowerSupplyId = car.VehiclesMappings.FirstOrDefault(x => x.Vehicles.Id == id).Supplies.Id;
                model.carFirstStep.CV = car.Cv;
                model.carFirstStep.KW = car.Kw;
                model.carFirstStep.Bookable = car.Bookable;
                model.carFirstStep.DisplayHp = car.DisplayHp;
                model.carFirstStep.Priority = car.Priority;

                if(car.FirstStep){
                    model.disableSecondStep = false;
                }

                if(car.SecondStep){
                    model.disableThirdStep = false;
                }

                if(car.ThirdStep){
                    model.disableFourStep = false;
                }
                
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CarsFirstStep(CarAddViewModel model){
            var entityCar = new Vehicle();
            
            if(ModelState.IsValid){
                var UserId = _dataProtectionProvider.CreateProtector(Key).Unprotect(User.Identity.Name);
                var brandName = _vehiclesBrandService.GetVehiclesBrandById(model.carFirstStep.BrandId).BrandName;

                var description = "";
                if(model.carFirstStep.Description != string.Empty ){
                    if(model.carFirstStep.Description.Length > 255){
                        description = model.carFirstStep.Description.Substring(0, 250);
                    } else {
                        description = model.carFirstStep.Description;
                    }
                } 


                entityCar.Model = model.carFirstStep.Model;
                entityCar.Description = model.carFirstStep.Description;
                entityCar.Cv = model.carFirstStep.CV;
                entityCar.Kw = model.carFirstStep.KW;
                entityCar.Bookable = model.carFirstStep.Bookable;
                entityCar.MetaTitle = string.Concat(brandName, " - ", model.carFirstStep.Model);
                entityCar.MetaDescription = description;
                entityCar.PermaLink = _commonService.cleanStringPath(model.carFirstStep.Model);
                entityCar.OperatorData = DateTime.Now;
                entityCar.IDOperator = _operatorServices.GetOperatorByUserId(UserId).Id;
                entityCar.FirstStep = true;
                entityCar.DisplayHp = model.carFirstStep.DisplayHp;
                entityCar.Priority = model.carFirstStep.Priority;

                if(model.Id != 0){
                    entityCar.Id = model.Id;
                    _carService.UpdateFirstStepVehicle(entityCar);

                    //DELETE VEHICLE CATEGORY
                    _carService.DeleteVehicleCategory(entityCar.Id);

                    //DELETE VEHICLE BRAND
                    _carService.DeleteVehicleMapping(entityCar.Id);

                } else {
                    _carService.InsertFirstStepVehicle(entityCar);
                }

                //INSERT VEHICLE CATEGORY
                if(model.carFirstStep.CategorySelected != 0){
                    var VehicleCategory = new ContentCategoryNews();
                    VehicleCategory.Vehicle = entityCar;
                    VehicleCategory.ContentCategories = _categoryService.GetCategoryById(model.carFirstStep.CategorySelected);
                    _carService.InsertVehicleCategory(VehicleCategory);
                }

                //INSERT VEHICLE BRAND
                if(model.carFirstStep.BrandId != 0 && model.carFirstStep.PowerSupplyId != 0){
                    var VehicleMapping = new VehiclesMapping();
                    VehicleMapping.Vehicles = entityCar;
                    VehicleMapping.Brands = _vehiclesBrandService.GetVehiclesBrandById(model.carFirstStep.BrandId);
                    VehicleMapping.Supplies = _carService.GetVehiclePowerSupplyById(model.carFirstStep.PowerSupplyId);
                    _carService.InsertVehicleMapping(VehicleMapping);
                }

                return RedirectToAction("CarsSecondStep", new {id = entityCar.Id});
            }

            return View("500");
            
        }

        public IActionResult CarsSecondStep(int id, int imageId){
            ViewData["Title"] = "Aggiungi Veicolo";
            var model = new CarAddViewModel();
            
            model.Id = id;
            model.disableThirdStep = true;
            model.disableFourStep = true;

            if(_carService.GetVehiclesImages(model.Id).Any()){
                var car = _carService.GetVehicleById(id);

                if(car.SecondStep){
                    model.disableThirdStep = false;
                }

                if(car.ThirdStep){
                    model.disableFourStep = false;
                }
            }

            if(imageId != 0){
                model.carSecondStep.ImageId = imageId;
                var image = _carService.GetVehiclesImagesById(model.carSecondStep.ImageId);
                model.carSecondStep.FilePath = string.Concat("/", image.Path.Replace("\\", "/")) ;
                model.carSecondStep.Title = image.Name;
                model.carSecondStep.Description = image.Description;
                model.carSecondStep.Priority = image.Priority;
            }


            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CarsSecondStep(CarAddViewModel model){
            var entityVehicleImage = new VehiclesImage();
            
            if(ModelState.IsValid){
                if(model.carSecondStep.ImageId != 0){
                    
                    //UPDATE IMAGE
                    entityVehicleImage.Id = model.carSecondStep.ImageId;
                    entityVehicleImage.Name = model.carSecondStep.Title;
                    entityVehicleImage.Description = model.carSecondStep.Description;
                    entityVehicleImage.Priority = model.carSecondStep.Priority;

                    _carService.UpdateVehicleImage(entityVehicleImage);

                    return RedirectToAction("CarsSecondStepMenage", new {id = model.Id});
                } else {
                    var fileExtension = new[] { "image/gif", "image/jpg", "image/jpeg", "image/tiff", "image/png" };
            
                    if(_fileService.fileExtensionOk(model.carSecondStep.Upload.ContentType, fileExtension)){
                            
                        //UPLOAD FILE
                        var fileName = string.Concat(model.carSecondStep.Title.Replace(" ", "-"), '_', Guid.NewGuid().ToString().Substring(0, 4), Path.GetExtension(model.carSecondStep.Upload.FileName));
                        
                        //CREATE VEHICLE FOLDER IF NOT EXIST AND UPLOAD IMAGE
                        _fileService.CreateFolder("vehicles", model.Id.ToString());
                        _fileService.UploadFile(Path.Combine("vehicles", model.Id.ToString()), fileName, model.carSecondStep.Upload);

                        //SAVE IMAGE DATA
                        entityVehicleImage.Name = model.carSecondStep.Title;
                        entityVehicleImage.Description = model.carSecondStep.Description;
                        entityVehicleImage.Path = Path.Combine("upload", "vehicles",  model.Id.ToString(), fileName);
                        entityVehicleImage.Priority = model.carSecondStep.Priority;

                        _carService.InsertVehicleImage(entityVehicleImage);

                        //INSERT VEHICLE - IMAGE MAPPING
                        var entityVehicleImageMapping = new VehiclesImagesMapping();
                        entityVehicleImageMapping.Image = entityVehicleImage;
                        entityVehicleImageMapping.Vehicle = _carService.GetVehicleById(model.Id);
                        _carService.InsertVehicleMapping(entityVehicleImageMapping);
                        
                        //UPDATE VEHICLE STEP
                        var entityVehicle = new Vehicle();
                        entityVehicle.Id = model.Id;
                        entityVehicle.FirstStep = true;
                        entityVehicle.SecondStep = true;
                        entityVehicle.ThirdStep = false;
                        _carService.UpdateVehicleStep(entityVehicle);

                        return RedirectToAction("CarsSecondStep", new {id = model.Id});
                    }

                    return View("500");
                }
    
            }    

            return View("500");
        }

        public IActionResult CarsSecondStepMenage(int id){
            ViewData["Title"] = "Aggiungi Veicolo";
            var model = new CarAddViewModel();
            
            model.Id = id;
            
            var VehicleImages = _carService.GetVehiclesImages(model.Id);
            foreach(var vehicleImage in VehicleImages){
                model.carSecondStepMenage.Add(new CarSecondStep{
                    ImageId = vehicleImage.Image.Id,
                    FilePath = string.Concat("/", vehicleImage.Image.Path.Replace("\\", "/")),
                    Title = vehicleImage.Image.Name,
                    Description = vehicleImage.Image.Description,
                    Priority = vehicleImage.Image.Priority
                });
            }
            

            return View(model);
        }

        public IActionResult VehicleImageDelete(int id){
            //DELETE PHISYCAL IMAGE
            var image = _carService.GetVehiclesImagesById(id);
            var vehicleId = image.VehiclesImages.FirstOrDefault(x => x.Image.Id == id).Vehicle.Id;
            _fileService.DeleteFile(image.Path);
            
            _carService.DeleteVehicleImage(image.Id);
            

            return RedirectToAction("CarsSecondStepMenage", new{ id = vehicleId});
        }

        public IActionResult CarsThirdStep(int id){
            ViewData["Title"] = "Aggiungi Veicolo";
            var model = new CarAddViewModel();
            
            //SEO
            model.thirdStep.SeoIndex =  _vehiclesBrandService.GetListSeoIndex().Select(x => new SelectListItem{
                Value = x.Id.ToString(),
                Text = x.Description,
            });

            model.Id = id;
            var vehicle = _carService.GetVehicleById(model.Id);
            
            model.thirdStep.PermaLink = vehicle.PermaLink;
            model.thirdStep.MetaDescription = vehicle.MetaDescription;
            model.thirdStep.MetaTitle = vehicle.MetaTitle;
            model.thirdStep.SeoIndexId = vehicle.SeoIndexRef;
            
            return View(model);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CarsThirdStep(CarAddViewModel model){
            var entityVehicle = new Vehicle();
            
            if(ModelState.IsValid){
                entityVehicle.Id = model.Id;
                entityVehicle.MetaTitle = model.thirdStep.MetaTitle;
                entityVehicle.MetaDescription = model.thirdStep.MetaDescription;
                entityVehicle.PermaLink = _commonService.cleanStringPath(model.thirdStep.PermaLink);

                entityVehicle.SeoIndexRef = model.thirdStep.SeoIndexId;
                entityVehicle.SeoIndex = _carService.GetContentNewsSeoIndexById(model.thirdStep.SeoIndexId);
                entityVehicle.SeoIndex.Vehicle = _carService.GetVehicleById(model.Id);

                entityVehicle.FirstStep = true;
                entityVehicle.SecondStep = true;
                entityVehicle.ThirdStep = true;
                
                _carService.UpdateVehicleSEO(entityVehicle);

                return RedirectToAction("Cars");
            }

            return View("500");
        }
        
        public IActionResult CarsFourStep(int id){
            ViewData["Title"] = "Aggiungi Veicolo";
            var model = new CarAddViewModel();
            
            model.Id = id;
            var vehicleContact = _carService.GetVehicleContact(model.Id);
            foreach(var contact in vehicleContact){
                model.carFourStep.Add(new FourStep{
                    MessageId = contact.Id,
                    PeopleEmail = contact.People.Email,
                    PeopleName = contact.People.Name,
                    PeopleLastname = contact.People.Lastname,
                    PeopleMessage = contact.Message
                });
            }
            return View(model);
        }

    }
}