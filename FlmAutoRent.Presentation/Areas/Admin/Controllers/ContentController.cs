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
    public class ContentController : Controller
    {
        private readonly IDataProtectionProvider _dataProtectionProvider;
        private const string Key = "cxz92k13md8f981hu6y7alkc";
        private readonly ICommonService _commonService;
        private readonly IFileService _fileService;
        private readonly IOperatorServices _operatorServices;
        private readonly ICategoryService _categoryService;
        private readonly IImageService _imagesService;
        private readonly IVideoService _videosService;
        private readonly IAttachmentService _attachmentService;
        private readonly INewsServices _newsService;
        private readonly IHomePageService _homepageService;

        public ContentController(IDataProtectionProvider dataProtectionProvider, IOperatorServices operatorServices, ICommonService commonService, ICategoryService categoryService, IFileService fileService, IImageService imagesService,IVideoService videosService, IAttachmentService attachmentService, INewsServices newsService, IHomePageService homepageService){            
            this._dataProtectionProvider = dataProtectionProvider;
            this._commonService = commonService;
            this._operatorServices = operatorServices;
            this._categoryService = categoryService;
            this._fileService = fileService;
            this._imagesService = imagesService;
            this._videosService = videosService;
            this._attachmentService = attachmentService;
            this._newsService = newsService;
            this._homepageService = homepageService;
        }

        public IActionResult Index(){
            return RedirectToAction("Category");
        }   

        public IActionResult Category(){
             
            var model = new CategoryViewModel();
            
            var categories = _categoryService.GetCategories();
            
            foreach(var category in categories){
                model.Categories.Add(category);
            }

            return View(model);
        }

        public IActionResult CategoryAddOrEdit(int id, int idFather = 0){
            ViewData["Title"] = "Aggiungi Categoria";
            var model = new CategoryAddViewModel();
                        
            model.IdFather = idFather;
            var images = _imagesService.GetContentImages();

            if(id != 0){
                //GET CATEGORY
                var entitiesCategory = _categoryService.GetCategoryById(id);
                model.Id = entitiesCategory.Id;
                model.IdFather = entitiesCategory.IdFather;
                model.Name = entitiesCategory.Name;
                model.Description = entitiesCategory.Description;
                model.DisplayRef = entitiesCategory.Display;
                model.Priority = entitiesCategory.Priority;

                foreach(var image in images){
                    
                    if(entitiesCategory.ContentCategoryImages.Where(x => x.Images.Id == image.Id ).Any()){
                        model.ImageListSelected = image.Id;
                    }

                    model.ImagesList.Add(new FileViewModel{
                        Id = image.Id,
                        FilePath = string.Concat("/", image.FilePath.Replace("\\", "/")),
                        Name = image.FileName,
                    });
                }

            } else {

                foreach(var image in images){
                    model.ImagesList.Add(new FileViewModel{
                        Id = image.Id,
                        FilePath = string.Concat("/", image.FilePath.Replace("\\", "/")),
                        Name = image.FileName,
                    });
                }

            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CategoryAddOrEdit(CategoryAddViewModel model){
            var entitiesCategory = new ContentCategory();
            
            if(ModelState.IsValid){
                //GET OPERATOR ID
                var UserId = _dataProtectionProvider.CreateProtector(Key).Unprotect(User.Identity.Name);
                var description = "";

                if(model.Description != string.Empty ){
                    if(model.Description.Length > 255){
                        description = model.Description.Substring(0, 250);
                    } else {
                        description = model.Description;
                    }
                } 

                entitiesCategory.IdFather = model.IdFather;
                entitiesCategory.Name = model.Name;
                entitiesCategory.Description = model.Description;
                entitiesCategory.Priority = model.Priority;
                entitiesCategory.Display = model.DisplayRef;
                entitiesCategory.MetaTitle = model.Name;
                entitiesCategory.MetaDescription = description;
                entitiesCategory.PermaLink = _commonService.cleanStringPath(model.Name);
                entitiesCategory.OperatorData = DateTime.Now;
                entitiesCategory.IDOperator = _operatorServices.GetOperatorByUserId(UserId).Id;

                //UPDATE OR CREATE CATEGORY
                if(model.Id != 0){
                    //UPDATE GROUP
                    entitiesCategory.Id = model.Id;
                    _categoryService.UpdateContentCategory(entitiesCategory);
                    _categoryService.DeleteContentCategoryImage(model.Id);
                } else {
                    //CREATE GROUP
                    _categoryService.InsertContentCategory(entitiesCategory);
                }

                //INSERT CONTENT CATEGORY IMAGE
                if(model.ImageListSelected != 0){
                    var entitiesCategoryImage = new ContentCategoryImage();
                    entitiesCategoryImage.Categories = entitiesCategory;
                    entitiesCategoryImage.Images = _imagesService.GetContentImageById(model.ImageListSelected);
                    _categoryService.InsertContentCategoryImage(entitiesCategoryImage);
                }
                
            }

            return RedirectToAction("Category");
        }

        public IActionResult CategoryMoveTop(int id){
            var entitiesCategory = _categoryService.GetCategoryById(id);
            if(entitiesCategory.Priority > 1){
                entitiesCategory.Priority = entitiesCategory.Priority - 1;
            }
            
            _categoryService.UpdateContentCategory(entitiesCategory);
            
            return RedirectToAction("Category");
        }

        public IActionResult CategoryMoveBottom(int id){
            var entitiesCategory = _categoryService.GetCategoryById(id);
            entitiesCategory.Priority = entitiesCategory.Priority + 1;

            _categoryService.UpdateContentCategory(entitiesCategory);
            
            return RedirectToAction("Category");
        }
    
        public IActionResult Images(){
            ViewData["Title"] = "Immagini";
            var model = new FilesTableViewModel();

            model.HowManyFieldList = new List<FilesTableViewModel.HowManyFields>{
                new FilesTableViewModel.HowManyFields{ Value = 10, DisplayText="Visualizza 10 risultati per pagina" },
                new FilesTableViewModel.HowManyFields{ Value = 100, DisplayText="Visualizza 100 risultati per pagina" },
                new FilesTableViewModel.HowManyFields{ Value = 1000, DisplayText="Visualizza 1000 risultati per pagina" },
                new FilesTableViewModel.HowManyFields{ Value = 100000, DisplayText="Visualizza tutti i risultati" }
            };

            var contentImages = _imagesService.GetContentImages();
            foreach(var contentImage in contentImages){
                model.FilesListViewModel.Add(new FileViewModel{ 
                    Id = contentImage.Id, 
                    Name = contentImage.Title,
                    FilePath = string.Format("/{0}", contentImage.FilePath.Replace("\\", "/")),
                    Nusing = contentImage.ContentCategoryImages.Where(x => x.Images.Id == contentImage.Id).Count()
                });        
            }
            

            return View(model);
        }
    
        public IActionResult ImagesAddOrEdit(int id){
            ViewData["Title"] = "Aggiungi Immagine";
            var model = new FileAddViewModel();

            if(id != 0){
                var entitiesImage = _imagesService.GetContentImageById(id);
                
                model.Id = entitiesImage.Id;
                model.Title = entitiesImage.Title;
                model.Description = entitiesImage.Description;
                model.FilePath = string.Concat("/", entitiesImage.FilePath.Replace("\\", "/"));
            }

            return View(model);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ImagesAddOrEdit(FileAddViewModel model){
           var entityImage = new ContentImage();
           
            if(ModelState.IsValid){
                var UserId = _dataProtectionProvider.CreateProtector(Key).Unprotect(User.Identity.Name);
                
                if(model.Id == 0){
                    var fileExtension = new[] { "image/gif", "image/jpg", "image/jpeg", "image/tiff", "image/png" };
            
                    if(_fileService.fileExtensionOk(model.Upload.ContentType, fileExtension)){
                        
                        //UPLOAD FILE
                        var fileName = string.Concat(model.Title.Replace(" ", "-"), '_', Guid.NewGuid().ToString().Substring(0, 4), Path.GetExtension(model.Upload.FileName));
                        _fileService.UploadFile("category", fileName, model.Upload);

                        //SAVE DATA ON DB
                        entityImage.Title = model.Title;
                        entityImage.Description = model.Description;
                        entityImage.FileName = fileName.Replace(" ", "-");
                        entityImage.FilePath = Path.Combine("upload", "category", fileName);
                        entityImage.FileExtenstion = Path.GetExtension(model.Upload.FileName);
                        entityImage.FileNameOriginal = model.Upload.FileName;
                        entityImage.OperatorData = DateTime.Now;
                        entityImage.IDOperator = _operatorServices.GetOperatorByUserId(UserId).Id;

                        _imagesService.InsertContentImage(entityImage);

                        return RedirectToAction("Images");
                    }

                } else {
                    entityImage.Id = model.Id;
                    entityImage.Title = model.Title;
                    entityImage.Description = model.Description;   
                    entityImage.OperatorData = DateTime.Now;
                    entityImage.IDOperator = _operatorServices.GetOperatorByUserId(UserId).Id;
                    _imagesService.UpdateContentImage(entityImage);
                }
                
               

               return RedirectToAction("Images");
            }

            return View("500");
        }
    
        public IActionResult ImageDelete(int id){
            //DELETE PHISYCAL IMAGE
            var contentImage = _imagesService.GetContentImageById(id);
            _fileService.DeleteFile(contentImage.FilePath);
            
            //DELETE IMAGE 
            _imagesService.DeleteContentImage(id);

            return RedirectToAction("Images");
        }

        public IActionResult Videos(){
            ViewData["Title"] = "Video";
            var model = new FilesTableViewModel();

            model.HowManyFieldList = new List<FilesTableViewModel.HowManyFields>{
                new FilesTableViewModel.HowManyFields{ Value = 10, DisplayText="Visualizza 10 risultati per pagina" },
                new FilesTableViewModel.HowManyFields{ Value = 100, DisplayText="Visualizza 100 risultati per pagina" },
                new FilesTableViewModel.HowManyFields{ Value = 1000, DisplayText="Visualizza 1000 risultati per pagina" },
                new FilesTableViewModel.HowManyFields{ Value = 100000, DisplayText="Visualizza tutti i risultati" }
            };

            var contentVideos = _videosService.GetContentVideos();
            foreach(var contentVideo in contentVideos){
                model.FilesListViewModel.Add(new FileViewModel{ 
                    Id = contentVideo.Id, 
                    Name = contentVideo.Title,
                    FilePath = string.Format("/{0}", contentVideo.FilePath.Replace("\\", "/"))
                });        
            }
            

            return View(model);
        }
    
        public IActionResult VideosAddOrEdit(int id){
            ViewData["Title"] = "Aggiungi Video";
            var model = new FileAddViewModel();

            if(id != 0){
                var entitiesVideo = _videosService.GetContentVideoById(id);
                
                model.Id = entitiesVideo.Id;
                model.Title = entitiesVideo.Title;
                model.Description = entitiesVideo.Description;
                model.FilePath = entitiesVideo.FilePath;
            }

            return View(model);
        }

 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult VideosAddOrEdit(FileAddViewModel model){
           var entityVideo = new ContentVideo();
           
            if(ModelState.IsValid){
                var UserId = _dataProtectionProvider.CreateProtector(Key).Unprotect(User.Identity.Name);
                
                if(model.Id == 0){
                    var fileExtension = new[] {  "video/mp4", "video/ogg", "video/3gp", "video/wmv", "video/webm", "video/flv"  };
            
                    if(_fileService.fileExtensionOk(model.Upload.ContentType, fileExtension)){
                        
                        //UPLOAD FILE
                        var fileName = string.Concat(model.Title.Replace(" ", "-"), '_', Guid.NewGuid().ToString().Substring(0, 4), Path.GetExtension(model.Upload.FileName));
                        _fileService.UploadFile("video", fileName, model.Upload);

                        //SAVE DATA ON DB
                        entityVideo.Title = model.Title;
                        entityVideo.Description = model.Description;
                        entityVideo.FileName = fileName.Replace(" ", "-");
                        entityVideo.FilePath = Path.Combine("upload", "video", fileName);
                        entityVideo.FileExtenstion = Path.GetExtension(model.Upload.FileName);
                        entityVideo.FileNameOriginal = model.Upload.FileName;
                        entityVideo.OperatorData = DateTime.Now;
                        entityVideo.IDOperator = _operatorServices.GetOperatorByUserId(UserId).Id;

                        _videosService.InsertContentVideo(entityVideo);

                        return RedirectToAction("Images");
                    }

                } else {
                    entityVideo.Id = model.Id;
                    entityVideo.Title = model.Title;
                    entityVideo.Description = model.Description;   
                    entityVideo.OperatorData = DateTime.Now;
                    entityVideo.IDOperator = _operatorServices.GetOperatorByUserId(UserId).Id;
                    _videosService.UpdateContentVideo(entityVideo);
                }
                
               

               return RedirectToAction("Videos");
            }

            return View("500");
        }
    
        public IActionResult VideoDelete(int id){
            //DELETE PHISYCAL IMAGE
            var contentVideo = _videosService.GetContentVideoById(id);
            _fileService.DeleteFile(contentVideo.FilePath);
            
            //DELETE IMAGE 
            _videosService.DeleteContentVideo(id);

            return RedirectToAction("Videos");
        }

        public IActionResult Attachments(){
            ViewData["Title"] = "Allegati";
            var model = new FilesTableViewModel();

            model.HowManyFieldList = new List<FilesTableViewModel.HowManyFields>{
                new FilesTableViewModel.HowManyFields{ Value = 10, DisplayText="Visualizza 10 risultati per pagina" },
                new FilesTableViewModel.HowManyFields{ Value = 100, DisplayText="Visualizza 100 risultati per pagina" },
                new FilesTableViewModel.HowManyFields{ Value = 1000, DisplayText="Visualizza 1000 risultati per pagina" },
                new FilesTableViewModel.HowManyFields{ Value = 100000, DisplayText="Visualizza tutti i risultati" }
            };

            var contentAttachments = _attachmentService.GetContentAttachments();
            foreach(var contentAttachment in contentAttachments){
                model.FilesListViewModel.Add(new FileViewModel{ 
                    Id = contentAttachment.Id, 
                    Name = contentAttachment.Title,
                    FilePath = string.Format("/{0}", contentAttachment.FilePath.Replace("\\", "/")),
                });        
            }
            

            return View(model);
        }
    
        public IActionResult AttachmentsAddOrEdit(int id){
            ViewData["Title"] = "Aggiungi Allegato";
            var model = new FileAddViewModel();

            if(id != 0){
                var entitiesAttachment = _attachmentService.GetContentAttachmentById(id);
                
                model.Id = entitiesAttachment.Id;
                model.Title = entitiesAttachment.Title;
                model.Description = entitiesAttachment.Description;
                model.FilePath = entitiesAttachment.FilePath;
            }

            return View(model);
        }

                
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AttachmentsAddOrEdit(FileAddViewModel model){
           var entityAttachment = new ContentAttachment();
           
            if(ModelState.IsValid){
                var UserId = _dataProtectionProvider.CreateProtector(Key).Unprotect(User.Identity.Name);
                
                if(model.Id == 0){
                    var fileExtension = new[] {  "application/pdf", "application/doc", "application/docx"};
            
                    if(_fileService.fileExtensionOk(model.Upload.ContentType, fileExtension)){
                        
                        //UPLOAD FILE
                        var fileName = string.Concat(model.Title.Replace(" ", "-"), '_', Guid.NewGuid().ToString().Substring(0, 4), Path.GetExtension(model.Upload.FileName));
                        _fileService.UploadFile("attachment", fileName, model.Upload);

                        //SAVE DATA ON DB
                        entityAttachment.Title = model.Title;
                        entityAttachment.Description = model.Description;
                        entityAttachment.FileName = fileName.Replace(" ", "-");
                        entityAttachment.FilePath = Path.Combine("upload", "attachment", fileName);
                        entityAttachment.FileExtenstion = Path.GetExtension(model.Upload.FileName);
                        entityAttachment.FileNameOriginal = model.Upload.FileName;
                        entityAttachment.OperatorData = DateTime.Now;
                        entityAttachment.IDOperator = _operatorServices.GetOperatorByUserId(UserId).Id;

                        _attachmentService.InsertContentAttachment(entityAttachment);

                        return RedirectToAction("Attachments");
                    } else {
                        return RedirectToAction("500");
                    }

                } else {
                    entityAttachment.Id = model.Id;
                    entityAttachment.Title = model.Title;
                    entityAttachment.Description = model.Description;   
                    entityAttachment.OperatorData = DateTime.Now;
                    entityAttachment.IDOperator = _operatorServices.GetOperatorByUserId(UserId).Id;
                    _attachmentService.UpdateContentAttachment(entityAttachment);
                }
                
               

               return RedirectToAction("Attachments");
            }

            return View("500");
        }
    
        public IActionResult AttachmentDelete(int id){
            //DELETE PHISYCAL IMAGE
            var contentAttachment = _attachmentService.GetContentAttachmentById(id);
            _fileService.DeleteFile(contentAttachment.FilePath);
            
            //DELETE IMAGE 
            _attachmentService.DeleteContentAttachment(id);

            return RedirectToAction("Attachments");
        }

        public IActionResult News(){
            ViewData["Title"] = "Notizie";
            var model = new NewsTableViewModel();

            model.HowManyFieldList = new List<NewsTableViewModel.HowManyFields>{
                new NewsTableViewModel.HowManyFields{ Value = 10, DisplayText="Visualizza 10 risultati per pagina" },
                new NewsTableViewModel.HowManyFields{ Value = 100, DisplayText="Visualizza 100 risultati per pagina" },
                new NewsTableViewModel.HowManyFields{ Value = 1000, DisplayText="Visualizza 1000 risultati per pagina" },
                new NewsTableViewModel.HowManyFields{ Value = 100000, DisplayText="Visualizza tutti i risultati" }
            };
            
            var contentNews = _newsService.GetContentNews();
            foreach(var news in contentNews){
                model.NewsListViewModel.Add(new NewsViewModel {
                    Id = news.Id,
                    Category = news.ContentCategoryNews.Any() ? news.ContentCategoryNews.FirstOrDefault().ContentCategories.Name : "",
                    PublicDate = news.OperatorData,
                    Title = news.Title
                });
            }
            
            return View(model);
        }
        
        public IActionResult NewsAddOrEdit(int Id){
            ViewData["Title"] = "Aggiungi Notizie";
            var model = new NewsAddViewModel();
            
            //GET CATEGORY 
            var categories = _categoryService.GetCategories();
            foreach(var category in categories){
                model.Categories.Add(category);
            }

            //SEO
            model.SeoIndex =  _newsService.GetListSeoIndex().Select(x => new SelectListItem{
                Value = x.Id.ToString(),
                Text = x.Description,
            });

            if(Id != 0){
                var contentNews = _newsService.GetContentNewsById(Id);
                
                model.Id = contentNews.Id;
                model.DisplayNews = contentNews.Display;
                model.DisplayOnFooter = contentNews.DisplayOnFooter;
                model.Title = contentNews.Title;
                model.SubTitle = contentNews.SubTitle;
                model.Summary = contentNews.Summary;
                model.News = contentNews.News;
                model.MetaTitle = contentNews.MetaTitle;
                model.MetaDescription = contentNews.MetaDescription;
                model.PermaLink = contentNews.PermaLink;
                model.SeoIndexId = contentNews.SeoIndexRef;

                //LIST CATEGORY
                foreach(var category in categories){
                    if(contentNews.ContentCategoryNews.Where(x => x.ContentCategories.Id == category.Id ).Any()){
                        model.CategorySelected = category.Id;
                    }
                }

                //LIST IMAGE
                var images = _imagesService.GetContentImages();
                foreach(var image in images){
                    model.ImagesList.Add(new NewsFileViewModel{
                        Id = image.Id,
                        Name = image.Title,
                        FilePath = string.Concat("/", image.FilePath.Replace("\\", "/")),
                        Selected = contentNews.ContentNewsImage.Any(x => x.ContentImage.Id == image.Id) ? true : false
                    });
                }

                //LIST VIDEO
                var videos = _videosService.GetContentVideos();
                foreach(var video in videos){
                    model.VideoList.Add(new NewsFileViewModel{
                        Id = video.Id,
                        Name = video.Title,
                        FilePath = string.Concat("/", video.FilePath.Replace("\\", "/")),
                        Selected = contentNews.ContentNewsVideo.Any(x => x.ContentVideo.Id == video.Id) ? true : false
                    });
                }

                //LIST ATTACHMENT
                var attachments = _attachmentService.GetContentAttachments();
                foreach(var attachment in attachments){
                    model.AttachmentList.Add(new NewsFileViewModel{
                        Id = attachment.Id,
                        Name = attachment.Title,
                        FilePath = string.Concat("/", attachment.FilePath.Replace("\\", "/")),
                        Selected = contentNews.ContentNewsAttachment.Any(x => x.ContentAttachment.Id == attachment.Id) ? true : false
                    });
                }

            } else {
                //LIST IMAGE
                var images = _imagesService.GetContentImages();
                foreach(var image in images){
                    model.ImagesList.Add(new NewsFileViewModel{
                        Id = image.Id,
                        Name = image.Title,
                        FilePath = string.Concat("/", image.FilePath.Replace("\\", "/")),
                        Selected = false
                    });
                }

                //LIST VIDEO
                var videos = _videosService.GetContentVideos();
                foreach(var video in videos){
                    model.VideoList.Add(new NewsFileViewModel{
                        Id = video.Id,
                        Name = video.Title,
                        FilePath = string.Concat("/", video.FilePath.Replace("\\", "/")),
                        Selected = false
                    });
                }

                //LIST ATTACHMENT
                var attachments = _attachmentService.GetContentAttachments();
                foreach(var attachment in attachments){
                    model.AttachmentList.Add(new NewsFileViewModel{
                        Id = attachment.Id,
                        Name = attachment.Title,
                        FilePath = string.Concat("/", attachment.FilePath.Replace("\\", "/")),
                        Selected = false
                    });
                }      
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult NewsAddOrEdit(NewsAddViewModel model){
            var entityNews = new ContentNews();
            if(ModelState.IsValid){
                
                var UserId = _dataProtectionProvider.CreateProtector(Key).Unprotect(User.Identity.Name);
                
                //SALVO DATI NOTIZIA
                entityNews.Display = model.DisplayNews;
                entityNews.Guid = Guid.NewGuid();
                entityNews.Title = model.Title;
                entityNews.SubTitle = model.SubTitle;
                entityNews.Summary = model.Summary;
                entityNews.News = model.News;
                entityNews.DisplayOnFooter = model.DisplayOnFooter;
                entityNews.IDOperator = _operatorServices.GetOperatorByUserId(UserId).Id;
                entityNews.OperatorData = DateTime.Now;
                entityNews.SeoIndexRef = model.SeoIndexId;

                //SALVO DATI SEO
                if(model.PermaLink == null || model.PermaLink == string.Empty){
                    entityNews.PermaLink = _commonService.cleanStringPath(model.Title);
                } else {
                    entityNews.PermaLink = _commonService.cleanStringPath(model.PermaLink);
                }

                if(model.MetaDescription == null || model.MetaDescription == string.Empty){
                    entityNews.MetaDescription = model.Summary != null && model.Summary.Length > 500 ? model.Summary.Substring(0, 500) : model.Summary;
                } else {
                    entityNews.MetaDescription = model.MetaDescription;
                }

                if(model.MetaTitle == null || model.MetaTitle == string.Empty){
                    entityNews.MetaTitle = model.Title;
                } else {
                    entityNews.MetaTitle = model.MetaTitle;
                }

                //INSERISCO NEWS
                if(model.Id == 0){
                    entityNews.SeoIndex = _newsService.GetContentNewsSeoIndexById(model.SeoIndexId);
                    entityNews.SeoIndex.ContentNews = entityNews;
                    
                    _newsService.InsertContentNews(entityNews);
                } else {
                    //DELETE NEWS - CATEGORY
                    entityNews.Id = model.Id;
                    var news = _newsService.GetContentNewsById(model.Id);
                    if(news.ContentCategoryNews.Any()){
                        _newsService.DeleteContentCategoryNews(news.ContentCategoryNews.FirstOrDefault().Id);
                    }
                    
                    //DELETE CONTENT - IMAGE
                    if(news.ContentNewsImage.Any()){
                        foreach(var contentnewsimage in news.ContentNewsImage){
                            _newsService.DeleteContentNewsImage(contentnewsimage.Id);
                        }
                    }

                    //DELETE CONTENT - VIDEO
                    if(news.ContentNewsVideo.Any()){
                        foreach(var contentnewsvideo in news.ContentNewsVideo){
                            _newsService.DeleteContentNewsVideo(contentnewsvideo.Id);
                        }
                    }

                    //DELETE CONTENT - ATTACHAMENT
                    if(news.ContentNewsAttachment.Any()){
                        foreach(var contentnewsattachment in news.ContentNewsAttachment){
                            _newsService.DeleteContentNewsAttachment(contentnewsattachment.Id);
                        }
                    }

                    _newsService.UpdateContentNews(model.Id, entityNews);   
                }

                //INSERISCO NEWS - CATEGORY
                if(model.CategorySelected != 0){
                    var contentCategoryNews = new ContentCategoryNews();
                    contentCategoryNews.News = entityNews;
                    contentCategoryNews.ContentCategories = _categoryService.GetCategoryById(model.CategorySelected);
                    _newsService.InsertContentCategoryNews(contentCategoryNews);
                }

                //INSERISCO NEWS - IMAGE
                if(model.ImagesList.Any(x => x.Selected == true)){
                    var images = model.ImagesList.Where(x => x.Selected== true).ToList();
                    foreach(var image in images){
                        var ContentNewsImage = new ContentNewsImage();
                        ContentNewsImage.ContentNews = entityNews;
                        ContentNewsImage.ContentImage = _imagesService.GetContentImageById(image.Id);
                        _newsService.InsertContentNewsImage(ContentNewsImage);
                    }
                }

                //INSERISCO NEWS - VIDEO
                if(model.VideoList.Any(x => x.Selected == true)){
                    var videos = model.VideoList.Where(x => x.Selected == true).ToList();
                    foreach(var video in videos){
                        var contentNewsVideo = new ContentNewsVideo();
                        contentNewsVideo.ContentNews = entityNews;
                        contentNewsVideo.ContentVideo = _videosService.GetContentVideoById(video.Id);
                        _newsService.InsertContentNewsVideo(contentNewsVideo);
                    }
                }

                 //INSERISCO NEWS - ATTACHMENT
                if(model.AttachmentList.Any(x => x.Selected == true)){
                    var attachments = model.AttachmentList.Where(x => x.Selected == true).ToList();;
                    foreach(var attachment in attachments){
                        var contentNewsAttachments = new ContentNewsAttachment();
                        contentNewsAttachments.ContentNews = entityNews;
                        contentNewsAttachments.ContentAttachment = _attachmentService.GetContentAttachmentById(attachment.Id);
                        _newsService.InsertContentNewsAttachment(contentNewsAttachments);
                    }
                }

            }

            return RedirectToAction("News");
        }   
 
        public IActionResult NewsDelete(int Id){
            var news = _newsService.GetContentNewsById(Id);
            
            //DELETE NEWS - CATEGORY
            if(news.ContentCategoryNews.Any()){
                _newsService.DeleteContentCategoryNews(news.ContentCategoryNews.FirstOrDefault().Id);
            }
            
            //DELETE CONTENT - IMAGE
            if(news.ContentNewsImage.Any()){
                foreach(var contentnewsimage in news.ContentNewsImage){
                    _newsService.DeleteContentNewsImage(contentnewsimage.Id);
                }
            }

            //DELETE CONTENT - VIDEO
            if(news.ContentNewsVideo.Any()){
                foreach(var contentnewsvideo in news.ContentNewsVideo){
                    _newsService.DeleteContentNewsVideo(contentnewsvideo.Id);
                }
            }

            //DELETE CONTENT - ATTACHAMENT
            if(news.ContentNewsAttachment.Any()){
                foreach(var contentnewsattachment in news.ContentNewsAttachment){
                    _newsService.DeleteContentNewsAttachment(contentnewsattachment.Id);
                }
            }

            //DELETE NEWS
            _newsService.DeleteContentNews(Id);

            return RedirectToAction("News");
        }

        public IActionResult Homepage(){
            ViewData["Title"] = "Gestione Homepage";
            var model = new HomepageViewModel();
            
            var vehicles = _homepageService.GetVehicles();
            foreach(var vehicle in vehicles){
                model.Vehicles.Add(new VehicleCard{
                    Id = vehicle.Id,
                    srcImg = string.Concat("/", vehicle.VehiclesImages.FirstOrDefault( x => x.Vehicle.Id == vehicle.Id).Image.Path.Replace("\\", "/")),
                    altImg = vehicle.VehiclesImages.FirstOrDefault( x => x.Vehicle.Id == vehicle.Id).Image.Description,
                    vehiclebrand = vehicle.VehiclesMappings.FirstOrDefault( x => x.Vehicles.Id == vehicle.Id).Brands.BrandName,
                    vehicleModel = vehicle.Model,
                    vehicleDescription = vehicle.Description,
                    permaLink = vehicle.PermaLink
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Homepage(HomepageViewModel model){
            var entityHomePage = new Homepage();
            
            if(ModelState.IsValid){
                entityHomePage.WelcomeMessage = model.WelcomeMessage;
                entityHomePage.PresentationMessage = model.Presentation;
                entityHomePage.HeaderImageId = model.HeaderImageListSelected;
                entityHomePage.PresentationImageId = model.PresentationImageListSelected;
                
                _homepageService.InsertUpdateHomepage(entityHomePage);

                return RedirectToAction("Homepage");
            }
            
            return View("500");
        }
    }
}