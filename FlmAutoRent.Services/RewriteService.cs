using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using FlmAutoRent.Data;
using FlmAutoRent.Data.Entities;

namespace FlmAutoRent.Services
{
    public interface IRewriteService {
        ContentCategory GetCategorieByPermalink(string peramlink);
        IList<ContentCategory> GetCategorieByIDFather(int IDFather);
        IList<ContentCategoryNews> GetContentCategoryNews(int IdContentCategory);
        ContentNews GetContentNews(string permaLink);
        IList<ContentCategoryNews> GetContentCategoriesVehicles(string permalink);
        Vehicle GetVehicle(string permaLink);
    }
    
    public class RewriteService : IRewriteService
    {
        private readonly FlmAutoRentContext _ctx;

        public RewriteService(FlmAutoRentContext ctx){
            this._ctx = ctx;
        }

        public ContentCategory GetCategorieByPermalink(string peramlink){
            return _ctx.ContentCategories.Include(x => x.ContentCategoryNews).ThenInclude(x => x.News).Include(x => x.ContentCategoryNews).ThenInclude(x => x.Vehicle).Include(x => x.ContentCategoryImages).ThenInclude(x => x.Images).FirstOrDefault(x => x.PermaLink == peramlink);
        }
       
        public IList<ContentCategory> GetCategorieByIDFather(int IDFather){
            return _ctx.ContentCategories.Include(x => x.ContentCategoryNews).ThenInclude(x => x.News).Include(x => x.ContentCategoryNews).ThenInclude(x => x.Vehicle).Include(x => x.ContentCategoryImages).ThenInclude(x => x.Images).Where(x => x.IdFather == IDFather && x.Display == true).OrderBy(x => x.Priority).ToList();
        }

        public IList<ContentCategoryNews> GetContentCategoryNews(int IdContentCategory){
            return _ctx.ContentCategoryNews.Include(x => x.ContentCategories).Include(x => x.News).ThenInclude(x => x.ContentNewsImage).ThenInclude(x => x.ContentImage).Where(x => x.ContentCategories.Id == IdContentCategory).ToList(); 
        }

        public ContentNews GetContentNews(string permaLink){
            return _ctx.ContentNews.Include(x => x.ContentCategoryNews).ThenInclude(x => x.ContentCategories).Include(x => x.ContentNewsImage).ThenInclude(x => x.ContentImage).FirstOrDefault(x => x.PermaLink == permaLink);
        }

        public Vehicle GetVehicle(string permaLink){
            return _ctx.Vehicles.Include(x => x.VehiclesMappings).ThenInclude(x => x.Brands).Include(x => x.VehiclesImages).ThenInclude(x => x.Image).FirstOrDefault(x => x.PermaLink == permaLink);
        }

        public IList<ContentCategoryNews> GetContentCategoriesVehicles(string permalink){
            return _ctx.ContentCategoryNews.Include(x => x.Vehicle).ThenInclude(x => x.VehiclesImages).ThenInclude(x => x.Image).Include(x => x.Vehicle).ThenInclude(x => x.VehiclesMappings).ThenInclude(x => x.Brands).Include(x => x.ContentCategories).Where(x => x.ContentCategories.PermaLink == permalink && x.Vehicle.Bookable).ToList();
        }
    }
}