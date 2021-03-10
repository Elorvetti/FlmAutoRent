using System;
using System.Collections.Generic;

namespace FlmAutoRent.Data.Entities
{
    public partial class ContentVideo
    {
        public ContentVideo(){
            ContentNewsVideo = new HashSet<ContentNewsVideo>();
        }
        public int Id {get; set; }
        public string Title {get; set; }
        public string Description {get; set; }
        public string FilePath {get; set; }
        public string FileName {get; set; }
        public string FileNameOriginal {get; set; }
        public string FileExtenstion {get; set; }
        public DateTime OperatorData {get; set; }
        public int IDOperator {get; set; }

        public ICollection<ContentNewsVideo> ContentNewsVideo { get; set; }

    }
}