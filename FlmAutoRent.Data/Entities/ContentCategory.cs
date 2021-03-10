using System;
using System.Collections.Generic;

namespace FlmAutoRent.Data.Entities
{
    public partial class ContentCategory
    {
        public ContentCategory(){
            ContentCategoryNews = new HashSet<ContentCategoryNews>();
            ContentCategoryImages = new HashSet<ContentCategoryImage>();
        }
        public int Id { get; set; }
        public int IdFather { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Priority { get; set; }
        public string MetaTitle { get; set; }
        public string MetaDescription { get; set; }
        public string PermaLink { get; set; }
        public bool Display { get; set; }
        public DateTime OperatorData { get; set; }
        public int IDOperator { get; set; }
        public ICollection<ContentCategoryNews> ContentCategoryNews { get; set; }
        public ICollection<ContentCategoryImage> ContentCategoryImages { get; set; }
    }
}