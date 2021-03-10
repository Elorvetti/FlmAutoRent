using System;
using System.IO;
using System.Web;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace FlmAutoRent.Services
{
    public interface IFileService
    {
        bool fileExtensionOk(string fileExtension, string[] extensioneSupported);
        void UploadFile(string saveTo, string fileName, IFormFile fileUpload);
        void CreateFolder(string path, string folderName);
        void DeleteFile(string path);
    }
    public partial class FileService : IFileService
    {
        private readonly IWebHostEnvironment _env;

        public FileService(IWebHostEnvironment env){
            this._env = env;
        }

        public bool fileExtensionOk(string fileExtension, string[] extensioneSupported){
            return extensioneSupported.Contains(fileExtension);
        }

        public void UploadFile(string saveTo, string fileName, IFormFile fileUpload){
            var p = Path.Combine(_env.ContentRootPath, "wwwroot", "upload", saveTo);
            var fp = Path.Combine(p, fileName);
            fileUpload.CopyTo(new FileStream(fp, FileMode.Create));
        }

        public void CreateFolder(string path, string folderName){
            var f = Path.Combine(_env.ContentRootPath, "wwwroot", "upload", path, folderName);
            if(!Directory.Exists(f)){
                Directory.CreateDirectory(f);
            }
        }

        public void DeleteFile(string path)
        {
            var f = Path.Combine(_env.ContentRootPath, "wwwroot", path);
            if(File.Exists(f)){
                File.Delete(f);
            }
        }
        
    }
}