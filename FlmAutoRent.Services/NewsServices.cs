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
    public interface INewsServices
    {
        IList<SeoIndex> GetListSeoIndex();
        SeoIndex GetContentNewsSeoIndexById(int Id);
        IList<ContentNews> GetContentNews(int excludeRecord = 0, int pageSize = int.MaxValue);
        IList<ContentNews> GetContentNewsByName(string find, int excludeRecord = 0, int pageSize = int.MaxValue);
        ContentNews GetContentNewsById(int id);
        void InsertContentNews(ContentNews model);
        void UpdateContentNews(int Id, ContentNews model);
        void DeleteContentNews(int Id);
        void InsertContentCategoryNews(ContentCategoryNews model);
        void DeleteContentCategoryNews(int Id);
        void InsertContentNewsImage(ContentNewsImage model);
        void DeleteContentNewsImage(int id);
        void InsertContentNewsVideo(ContentNewsVideo model);
        void DeleteContentNewsVideo(int id);
        void InsertContentNewsAttachment(ContentNewsAttachment model);
        void DeleteContentNewsAttachment(int id);
    }
    public partial class NewsServices : INewsServices
    {
        private readonly FlmAutoRentContext _ctx;
        
        public NewsServices(FlmAutoRentContext ctx){
            this._ctx = ctx;
        }

        public IList<SeoIndex> GetListSeoIndex(){
            return _ctx.SeoIndex.ToList();
        }        

        public SeoIndex GetContentNewsSeoIndexById(int Id){
            return _ctx.SeoIndex.FirstOrDefault(x => x.Id == Id);
        }

        public IList<ContentNews> GetContentNews(int excludeRecord = 0, int pageSize = int.MaxValue){
            return _ctx.ContentNews.Include(x => x.ContentCategoryNews).ThenInclude(x => x.ContentCategories).Skip(excludeRecord).Take(pageSize).ToList();
        }

        public IList<ContentNews> GetContentNewsByName(string find, int excludeRecord = 0, int pageSize = int.MaxValue){
            return _ctx.ContentNews.Include(x => x.ContentCategoryNews).ThenInclude(x => x.ContentCategories).Where(x => EF.Functions.Like(x.Title, string.Concat("%", find, "%")) ).Skip(excludeRecord).Take(pageSize).ToList();
        }

        public ContentNews GetContentNewsById(int id){
            return _ctx.ContentNews.Include(x => x.ContentCategoryNews).ThenInclude(x => x.ContentCategories).Include(x => x.ContentNewsImage).ThenInclude(x => x.ContentImage).Include(x => x.ContentNewsAttachment).ThenInclude(x => x.ContentAttachment).Include(x => x.ContentNewsImage).ThenInclude(x => x.ContentImage).Include(x => x.SeoIndex).FirstOrDefault(x => x.Id == id);
        }

        public void InsertContentNews(ContentNews model){
            _ctx.ContentNews.Add(model);
            _ctx.SaveChanges();
        }

        public void UpdateContentNews(int Id, ContentNews model){
            var entitiesNews = _ctx.ContentNews.Find(Id);

            entitiesNews.Title = model.Title;
            entitiesNews.SubTitle = model.SubTitle;
            entitiesNews.Summary = model.Summary;
            entitiesNews.News = model.News;
            entitiesNews.DisplayOnFooter = model.DisplayOnFooter;
            entitiesNews.Display = model.Display;
            entitiesNews.IDOperator = model.IDOperator;
            entitiesNews.OperatorData = model.OperatorData;
            entitiesNews.SeoIndexRef = model.SeoIndexRef;

            _ctx.SaveChanges();

        }

        public void DeleteContentNews(int Id){
            var contentNews = _ctx.ContentNews.Where(x => x.Id == Id);
            _ctx.ContentNews.RemoveRange(contentNews);
            
            _ctx.SaveChanges();
        }

        public void InsertContentCategoryNews(ContentCategoryNews model){
            _ctx.ContentCategoryNews.Add(model);
            _ctx.SaveChanges();
        }

        public void DeleteContentCategoryNews(int Id){
            var entitiesCategoryNews = _ctx.ContentCategoryNews.Where(x => x.Id == Id);
            _ctx.ContentCategoryNews.RemoveRange(entitiesCategoryNews);

            _ctx.SaveChanges();
        }

        public void InsertContentNewsImage(ContentNewsImage model){
            _ctx.ContentNewsImages.Add(model);
            _ctx.SaveChanges();
        }

        public void DeleteContentNewsImage(int id){
            var entitiesContentNewsImage = _ctx.ContentNewsImages.Where(x => x.Id == id);
            _ctx.ContentNewsImages.RemoveRange(entitiesContentNewsImage);
            _ctx.SaveChanges();
        }

        public void InsertContentNewsVideo(ContentNewsVideo model){
            _ctx.ContentNewsVideo.Add(model);
            _ctx.SaveChanges();
        }

        public void DeleteContentNewsVideo(int id){
            var entitiesContentNewsVideo = _ctx.ContentNewsVideo.Where(x => x.Id == id);
            _ctx.ContentNewsVideo.RemoveRange(entitiesContentNewsVideo);
            _ctx.SaveChanges();
        }
        public void InsertContentNewsAttachment(ContentNewsAttachment model){
            _ctx.ContentNewsAttachments.Add(model);
            _ctx.SaveChanges();
        }

        public void DeleteContentNewsAttachment(int id){
            var entitiesContentNewsAttachment = _ctx.ContentNewsAttachments.Where(x => x.Id == id);
            _ctx.ContentNewsAttachments.RemoveRange(entitiesContentNewsAttachment);
            _ctx.SaveChanges();
        }
    }
}