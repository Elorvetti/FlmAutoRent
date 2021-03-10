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
    public interface IHomePageService 
    {
        List<ContentCategory> GetContentCategories();
        List<Vehicle> GetVehicles(); 
        Homepage GetHomepage();
        void InsertUpdateHomepage(Homepage model);
        List<ContentNews> GetContentNewsOnFooter();
        ContentCategory GetContentCategory(int Id);
    }
    
    public class HomePageService : IHomePageService
    {
        private readonly FlmAutoRentContext _ctx;

        public HomePageService(FlmAutoRentContext ctx){
            this._ctx = ctx;
        }

        public List<ContentCategory> GetContentCategories(){
            return _ctx.ContentCategories.Where(x => x.IdFather == 0 && x.Display).OrderBy(x => x.Priority).ToList();
        }

        public List<Vehicle> GetVehicles(){
            return _ctx.Vehicles.Include(x => x.VehiclesMappings).ThenInclude(x => x.Brands).Include(x => x.VehiclesImages).ThenInclude(x => x.Image).Include(x => x.ContentCategoryNews).ThenInclude(x => x.ContentCategories).Where(x => x.DisplayHp && x.Bookable).ToList(); 
        }

        public Homepage GetHomepage(){
            return _ctx.Homepage.Include(x => x.HeaderContentImage).Include(x => x.PresentationContentImage).FirstOrDefault();
        }

        public List<ContentNews> GetContentNewsOnFooter(){
            return _ctx.ContentNews.Include(x => x.ContentCategoryNews).ThenInclude(x => x.ContentCategories).Where(x => x.DisplayOnFooter && x.Display).ToList();
        }
        
        public ContentCategory GetContentCategory(int Id){
            return _ctx.ContentCategories.FirstOrDefault(x => x.Id == Id);
        }

        public void InsertUpdateHomepage(Homepage model){
            if(_ctx.Homepage.Any()){
                var entityHomepage = _ctx.Homepage.FirstOrDefault();
                entityHomepage.HeaderImageId = model.HeaderImageId;
                entityHomepage.WelcomeMessage = model.WelcomeMessage;
                entityHomepage.PresentationImageId = model.PresentationImageId;
                entityHomepage.PresentationMessage = model.PresentationMessage;
            } else {
                _ctx.Homepage.Add(model);
            }
            
            _ctx.SaveChanges();
        }
        
    }
}