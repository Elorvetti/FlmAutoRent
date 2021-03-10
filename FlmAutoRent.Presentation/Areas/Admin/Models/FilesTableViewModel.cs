using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using FlmAutoRent.Data.Entities; 

namespace FlmAutoRent.Presentation.Areas.Admin.Models
{
    public class FilesTableViewModel : TableViewModel
    {
        public FilesTableViewModel(){
            FilesListViewModel = new List<FileViewModel>();
        }

        public List<FileViewModel> FilesListViewModel { get; set; } 

    }

    public class FileViewModel {
        public int Id { get; set; }
        public string FilePath { get; set; }
        public string Name { get; set; }
        public int Nusing { get; set; }

    }
}