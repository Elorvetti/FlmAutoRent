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
    public interface IVideoService
    {
        IList<ContentVideo> GetContentVideos();
        ContentVideo GetContentVideoById(int id);
        void InsertContentVideo(ContentVideo model);
        void UpdateContentVideo(ContentVideo model);
        void DeleteContentVideo(int id);
    }
    public partial class VideoServices : IVideoService
    {
        private readonly FlmAutoRentContext _ctx;
        
        public VideoServices(FlmAutoRentContext ctx){
            this._ctx = ctx;
        }
        
        public IList<ContentVideo> GetContentVideos(){
            return _ctx.ContentVideos.ToList();
        }

        public ContentVideo GetContentVideoById(int id){
            return _ctx.ContentVideos.FirstOrDefault(x => x.Id == id);
        }

        public void InsertContentVideo(ContentVideo model){
            _ctx.ContentVideos.Add(model);
            _ctx.SaveChanges();
        }

        public void UpdateContentVideo(ContentVideo model){
            var entitiesVideo = _ctx.ContentVideos.Find(model.Id);
    
            entitiesVideo.Title = model.Title;
            entitiesVideo.Description = model.Description;
            entitiesVideo.OperatorData = model.OperatorData;
            entitiesVideo.IDOperator = model.IDOperator;
            
            _ctx.SaveChanges();
        }

        public void DeleteContentVideo(int id){
            var contentVideo = _ctx.ContentVideos.Where(x => x.Id == id);
            _ctx.ContentVideos.RemoveRange(contentVideo);

            _ctx.SaveChanges();
        }


        
    }
}