using System;
using System.Collections.Generic;

namespace FlmAutoRent.Data.Entities
{
    public class Homepage
    {
        public int Id { get; set; }
        public string WelcomeMessage { get; set; }
        public string PresentationMessage { get; set; }

        public int HeaderImageId { get; set; }
        public ContentImage HeaderContentImage { get; set; }

        public int PresentationImageId { get; set; }
        public ContentImage PresentationContentImage { get; set; }

    }
}