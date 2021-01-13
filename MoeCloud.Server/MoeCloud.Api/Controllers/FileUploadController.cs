using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using MoeCloud.Api.Handler;
using MoeCloud.Model;
using MoeCloud.Logic;
using MoeCloud.ILogic;

namespace MoeCloud.Api.Controllers
{
    [Route("{Controller}/{Action}")]
    [ApiController]
    public class FileUploadController : ControllerBase
    {
        private readonly JwtHelper jwt;
        private readonly IFileUploadBLL fileUpload;

        public IWebHostEnvironment Env { get; }     
        public FileUploadController(IWebHostEnvironment env, Handler.JwtHelper jwt, ILogic.IFileUploadBLL fileUpload)
        {
            Env = env;
            this.jwt = jwt;
            this.fileUpload = fileUpload;
        }

        #region 文件夹上传
        [HttpPost]
        //文件夹上传
        public IActionResult UpLoadOne()
        {
            var files = Request.Form.Files;
            long Size = files.Sum(f => f.Length);//计算文件大小
            string rootpath = Env.ContentRootPath + @"/Upload/测试/"; ; //获取根目录
            try
            {
                foreach (var file in files)
                {
                    string[] arrpath = file.FileName.Split(@"/");
                    string dirpath = "";//该文件的所在目录（包括一、二级目录）
                    string filename = arrpath[arrpath.Length - 1].ToString();//该文件名

                    for (int i = 0; i < arrpath.Length; i++)
                    {
                        if (i == arrpath.Length - 1)
                        {
                            break;
                        }
                        dirpath += arrpath[i] + @"/";
                    }
                    DicCreate(Path.Combine(rootpath, dirpath));//不存在则创建该目录


                    string filepath = Path.Combine(rootpath, file.FileName);
                    using (var addFile = new FileStream(filepath, FileMode.OpenOrCreate))
                    {
                        if (file != null)
                        {
                            file.CopyTo(addFile);
                        }
                        else
                        {
                            Request.Body.CopyTo(addFile);
                        }
                        addFile.Close();
                    }
                }

                return Ok(new
                {
                    success = true,
                    message = "上传成功"
                });
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        /// <summary>
        /// 文件目录如果不存在，就创建一个新的目录
        /// </summary>
       /// <param name="path"></param>
        private void DicCreate(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

#endregion

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> FileSave()
        {
            var date = Request;
            var files = Request.Form.Files;//获取前端传进的文件
            long size = files.Sum(f => f.Length);//计算大小
            string rootpath = Env.ContentRootPath + @"/Upload/测试/"; ; //获取根目录
            foreach (var formFile in files)
            {
                if (formFile.Length > 0) 
                {
                    string fileExt = formFile.FileName.Substring(formFile.FileName.IndexOf('.')); //文件扩展名，不含“.”
                    long fileSize = formFile.Length; //获得文件大小，以字节为单位
                    string newFileName = Guid.NewGuid().ToString() + "." + fileExt; //随机生成新的文件名
                    string DirPath = Path.Combine(rootpath, Request.Form["guid"]);
                    if (!Directory.Exists(DirPath))
                    {
                        Directory.CreateDirectory(DirPath);
                    }
                    var filePath = DirPath + "/" + Request.Form["chunk"] + fileExt;
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);

                    }
                }
            }
            return Ok(new { count = files.Count, size });
        }

        /// <summary>
        /// 合并请求
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> FileMerge()
        {
            bool ok = false;
            string rootpath01 = Env.ContentRootPath + @"/Upload/测试/临时文件夹";  //临时文件夹
            string errmsg = "";
            try
            {
                var temporary = Path.Combine(rootpath01, Request.Form["guid"]);//临时文件夹
                string fileName = Request.Form["fileName"];//文件名
                string fileExt = Path.GetExtension(fileName);//获取文件后缀
                var files = Directory.GetFiles(temporary);//获得下面的所有文件

                var finalFilePath = Path.Combine(rootpath01 + fileName);//最终的文件名
                //var fs = new FileStream(finalFilePath, FileMode.Create);
                using (var fs = new FileStream(finalFilePath, FileMode.Create))
                {
                    foreach (var part in files.OrderBy(x => x.Length).ThenBy(x => x))
                    {
                        var bytes = System.IO.File.ReadAllBytes(part);
                        await fs.WriteAsync(bytes, 0, bytes.Length);
                        bytes = null;
                        System.IO.File.Delete(part);//删除分块
                    }
                    Directory.Delete(temporary);//删除文件夹
                    ok = true;
                }
            }
            catch (Exception ex)
            {
                ok = false;
                errmsg = ex.Message;
               //log4net.Error(errmsg);
            }
            if (ok)
            {
                return Ok(new { success = true, msg = "" });
            }
            else
            {
                return Ok(new { success = false, msg = errmsg }); ;
            }
        }

        #region 分片上传
        [HttpPost]      
        public ActionResult Upload()
        {
            string rootpath = Env.ContentRootPath + @"/Upload/测试/"; ; //获取根目录       
            int index = Convert.ToInt32(Request.Form["chunk"]);//当前分块序号
            var guid = Request.Form["guid"];//前端传来的GUID号      
            var data = Request.Form.Files["file"];//表单中取得分块文件   
            
            int a= fileUpload.Upload(rootpath, index, guid, data);
            return Ok(new { count = a });
        }
              
        public ActionResult Merge()
        {
            string rootpath = Env.ContentRootPath + @"/Upload/测试/"; ; //获取根目录
            var guid = Request.Form["guid"];//GUID      
            var fileName = Request.Form["fileName"];//文件名     

            fileUpload.Merge(rootpath, guid, fileName);
            return Ok(new { error = 0 });//随便返回个值，实际中根据需要返回
        }
        #endregion
        [AllowAnonymous]//跳过Jwt验证     
        public ActionResult JwtYz([FromBody] int id = 1)
        {
            Role role = new Role()
            {
                ID = id,
                Name = "张三"
            };
            var toKen = jwt.GetToken(role);

            return Content(toKen);
        }
        [Authorize]//需要Jwt验证
        public ActionResult Parsing()
        {

            var data = HttpContext.Request.GetModel<Role>();
            return Ok(new { successful = true });
        }
    }

}