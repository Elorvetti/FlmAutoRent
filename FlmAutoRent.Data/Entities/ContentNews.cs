using System;
using System.Collections.Generic;

namespace FlmAutoRent.Data.Entities
{
    public partial class ContentNews
    {
        public ContentNews(){
            ContentCategoryNews = new HashSet<ContentCategoryNews>();
            ContentNewsImage = new HashSet<ContentNewsImage>();
            ContentNewsVideo = new HashSet<ContentNewsVideo>();
            ContentNewsAttachment = new HashSet<ContentNewsAttachment>();
        }

        public int Id { get; set; }
        public Guid Guid { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Summary { get; set; }
        public string News { get; set; }
        public string PermaLink { get; set; }
        public string MetaDescription { get; set; }
        public string MetaTitle { get; set; }
        public DateTime OperatorData { get; set; }
        public int IDOperator { get; set; }
        public bool DisplayOnFooter { get; set; }
        public bool Display { get; set; }
        public ICollection<ContentCategoryNews> ContentCategoryNews { get; set; }
        public ICollection<ContentNewsImage> ContentNewsImage { get; set; }
        public ICollection<ContentNewsVideo> ContentNewsVideo { get; set; }
        public ICollection<ContentNewsAttachment> ContentNewsAttachment { get; set; }
        
        public int SeoIndexRef { get; set;}
        public SeoIndex SeoIndex { get; set; }

    }
}