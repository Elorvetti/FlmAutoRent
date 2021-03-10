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
    public interface IAttachmentService
    {
        IList<ContentAttachment> GetContentAttachments();
        ContentAttachment GetContentAttachmentById(int id);
        void InsertContentAttachment(ContentAttachment model);
        void UpdateContentAttachment(ContentAttachment model);
        void DeleteContentAttachment(int id);
    }
    public partial class AttachmentServices : IAttachmentService
    {
        private readonly FlmAutoRentContext _ctx;
        
        public AttachmentServices(FlmAutoRentContext ctx){
            this._ctx = ctx;
        }
        
        public IList<ContentAttachment> GetContentAttachments(){
            return _ctx.ContentAttachments.ToList();
        }

        public ContentAttachment GetContentAttachmentById(int id){
            return _ctx.ContentAttachments.FirstOrDefault(x => x.Id == id);
        }

        public void InsertContentAttachment(ContentAttachment model){
            _ctx.ContentAttachments.Add(model);
            _ctx.SaveChanges();
        }

        public void UpdateContentAttachment(ContentAttachment model){
            var entitiesAttachment = _ctx.ContentAttachments.Find(model.Id);
    
            entitiesAttachment.Title = model.Title;
            entitiesAttachment.Description = model.Description;
            entitiesAttachment.OperatorData = model.OperatorData;
            entitiesAttachment.IDOperator = model.IDOperator;
            
            _ctx.SaveChanges();
        }

        public void DeleteContentAttachment(int id){
            var contentAttachment = _ctx.ContentAttachments.Where(x => x.Id == id);
            _ctx.ContentAttachments.RemoveRange(contentAttachment);

            _ctx.SaveChanges();
        }


        
    }
}