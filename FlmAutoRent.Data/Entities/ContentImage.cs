using System;
using System.Collections.Generic;

namespace FlmAutoRent.Data.Entities
{
    public partial class ContentImage
    {
        public ContentImage(){
            ContentCategoryImages = new HashSet<ContentCategoryImage>();
            ContentNewsImage = new HashSet<ContentNewsImage>();
        }

        public int Id {get; set; }
        public string Title {get; set; }
        public string Description {get; set; }
        public string FilePath {get; set; }
        public string FileName {get; set; }
        public string FileNameOriginal {get; set; }
        public string FileExtenstion {get; set; }
        public int FileWidth {get; set; }
        public int FileHeight {get; set; }
        public int FileSize {get; set; }
        public DateTime OperatorData {get; set; }
        public int IDOperator {get; set; }

        public ICollection<ContentCategoryImage> ContentCategoryImages { get; set; } 
        public ICollection<ContentNewsImage> ContentNewsImage { get; set; } 
        public Homepage HomepageHeaderImage { get; set; }
        public Homepage HomepagePresetationImage { get; set; }
    }
}