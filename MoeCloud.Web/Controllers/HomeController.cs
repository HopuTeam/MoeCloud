using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoeCloud.Web.Controllers
{
    public class HomeController : Controller
    {
        private ILogic.IFile Ifile { get; }
        public HomeController(ILogic.IFile Ifile)
        {
            this.Ifile = Ifile;
        }

        // 前端强类型自行解决
        public IActionResult Index()
        {
            Ifile.GetFiles(HttpContext.Session.GetModel<Model.User>("User").ID, 0).ToList();
            return View();
        }

    }
}