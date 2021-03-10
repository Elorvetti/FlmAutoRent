using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using FlmAutoRent.Data.Entities; 

namespace FlmAutoRent.Presentation.Models
{
    public class FileViewModel {
        public int Id { get; set; }
        public string FilePath { get; set; }
        public string Name { get; set; }
        public int Nusing { get; set; }

    }
}