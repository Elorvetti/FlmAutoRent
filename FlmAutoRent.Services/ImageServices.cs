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
    public interface IImageService
    {
        IList<ContentImage> GetContentImages();
        ContentImage GetContentImageById(int id);
        void InsertContentImage(ContentImage model);
        void UpdateContentImage(ContentImage model);
        void DeleteContentImage(int id);
    }
    public partial class ImageServices : IImageService
    {
        private readonly FlmAutoRentContext _ctx;
        
        public ImageServices(FlmAutoRentContext ctx){
            this._ctx = ctx;
        }
        
        public IList<ContentImage> GetContentImages(){
            return _ctx.ContentImages.Include(x => x.ContentCategoryImages).ThenInclude(x => x.Categories).ToList();
        }

        public ContentImage GetContentImageById(int id){
            return _ctx.ContentImages.FirstOrDefault(x => x.Id == id);
        }

        public void InsertContentImage(ContentImage model){
            _ctx.ContentImages.Add(model);
            _ctx.SaveChanges();
        }

        public void UpdateContentImage(ContentImage model){
            var entitiesImage = _ctx.ContentImages.Find(model.Id);
    
            entitiesImage.Title = model.Title;
            entitiesImage.Description = model.Description;
            entitiesImage.OperatorData = model.OperatorData;
            entitiesImage.IDOperator = model.IDOperator;
            
            _ctx.SaveChanges();
        }

        public void DeleteContentImage(int id){
            var contentImage = _ctx.ContentImages.Where(x => x.Id == id);
            _ctx.ContentImages.RemoveRange(contentImage);

            _ctx.SaveChanges();
        }


        
    }
}