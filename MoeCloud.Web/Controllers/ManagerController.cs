using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MoeCloud.Web.Controllers
{
    public class ManagerController : Controller
    {
        private ILogic.IFile Ifile { get; }
        private IWebHostEnvironment Env { get; }
        public ManagerController(ILogic.IFile ifile, IWebHostEnvironment env)
        {
            Ifile = ifile;
            Env = env;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Download(int fileID)
        {
            var path = Env.ContentRootPath + "/" + Ifile.GetFile(0, fileID).Path;
            FileInfo fileInfo = new FileInfo(path);
            var ext = fileInfo.Extension;
            new FileExtensionContentTypeProvider().Mappings.TryGetValue(ext, out var contenttype);
            return File(System.IO.File.ReadAllBytes(path), contenttype ?? "application/octet-stream", fileInfo.Name);
        }
    }
}
