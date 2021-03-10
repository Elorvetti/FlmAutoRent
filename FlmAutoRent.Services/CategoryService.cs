using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FlmAutoRent.Data;
using FlmAutoRent.Data.Entities;

namespace FlmAutoRent.Services
{
    public interface ICategoryService
    {
        IList<ContentCategory> GetCategories();
        ContentCategory GetCategoryById(int Id);
        void InsertContentCategory(ContentCategory model);
        void UpdateContentCategory(ContentCategory model);
        void InsertContentCategoryImage(ContentCategoryImage model);
        void DeleteContentCategoryImage(int ContentCategoryId);

    }
    public class CategoryService : ICategoryService
    {
        private readonly FlmAutoRentContext _ctx;
        
        public CategoryService(FlmAutoRentContext ctx){
            this._ctx = ctx;
        }

        public IList<ContentCategory> GetCategories(){
            return _ctx.ContentCategories.OrderBy(x => x.Priority).ToList();
        }
        public ContentCategory GetCategoryById(int Id){
            return _ctx.ContentCategories.Include(x => x.ContentCategoryImages).ThenInclude(x => x.Images).FirstOrDefault(x => x.Id == Id);
        }
        public void InsertContentCategory(ContentCategory model){
            _ctx.ContentCategories.Add(model);
            _ctx.SaveChanges();
        }

        public void UpdateContentCategory(ContentCategory model){
            var entitiesCategory = _ctx.ContentCategories.Find(model.Id);
            
            entitiesCategory.Name = model.Name;
            entitiesCategory.Description = model.Description;
            entitiesCategory.Priority = model.Priority;
            entitiesCategory.Display = model.Display;
            entitiesCategory.MetaTitle = model.Name;
            entitiesCategory.MetaDescription = model.Description;
            entitiesCategory.PermaLink = model.PermaLink;
            entitiesCategory.OperatorData = model.OperatorData;
            entitiesCategory.IDOperator = model.IDOperator;
            
            _ctx.SaveChanges();
        }
    
        public void InsertContentCategoryImage(ContentCategoryImage model){
            _ctx.ContentCategoryImages.Add(model);
            _ctx.SaveChanges();
        }

        public void DeleteContentCategoryImage(int ContentCategoryImageId){
            var contentCategoryImage = _ctx.ContentCategoryImages.Where( x => x.Categories.Id == ContentCategoryImageId);
            _ctx.ContentCategoryImages.RemoveRange(contentCategoryImage);

            _ctx.SaveChanges();
        }
    }
}