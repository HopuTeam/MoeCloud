using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MoeCloud.Api.Controllers
{
    [Route("{Controller}/{Action}")]
    [ApiController]
    public class FileUploadController : ControllerBase
    {
        public IWebHostEnvironment Env
        {
            get;
        }
        public FileUploadController(IWebHostEnvironment env)
        {
            Env = env;
        }


        [HttpPost]
        //文件夹上传
        public IActionResult UpLoad()
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


      
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> FileSave()
        {
            var date = Request;
            var files = Request.Form.Files;
            long size = files.Sum(f => f.Length);
            string rootpath = Env.ContentRootPath + @"/Upload/测试/"; ; //获取根目录
            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    string fileExt = formFile.FileName.Substring(formFile.FileName.IndexOf('.')); //文件扩展名，不含“.”
                    long fileSize = formFile.Length; //获得文件大小，以字节为单位
                    //string newFileName = Guid.NewGuid().ToString() + "." + fileExt; //随机生成新的文件名
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





    }

}