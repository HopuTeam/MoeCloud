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

        #region 文件夹上传
        //[HttpPost]
        ////文件夹上传
        //public IActionResult UpLoad()
        //{
        //    var files = Request.Form.Files;
        //    long Size = files.Sum(f => f.Length);//计算文件大小
        //    string rootpath = Env.ContentRootPath + @"/Upload/测试/"; ; //获取根目录
        //    try
        //    {
        //        foreach (var file in files)
        //        {
        //            string[] arrpath = file.FileName.Split(@"/");
        //            string dirpath = "";//该文件的所在目录（包括一、二级目录）
        //            string filename = arrpath[arrpath.Length - 1].ToString();//该文件名

        //            for (int i = 0; i < arrpath.Length; i++)
        //            {
        //                if (i == arrpath.Length - 1)
        //                {
        //                    break;
        //                }
        //                dirpath += arrpath[i] + @"/";
        //            }
        //            DicCreate(Path.Combine(rootpath, dirpath));//不存在则创建该目录


        //            string filepath = Path.Combine(rootpath, file.FileName);
        //            using (var addFile = new FileStream(filepath, FileMode.OpenOrCreate))
        //            {
        //                if (file != null)
        //                {
        //                    file.CopyTo(addFile);
        //                }
        //                else
        //                {
        //                    Request.Body.CopyTo(addFile);
        //                }
        //                addFile.Close();
        //            }
        //        }

        //        return Ok(new
        //        {
        //            success = true,
        //            message = "上传成功"
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(new
        //        {
        //            success = false,
        //            message = ex.Message
        //        });
        //    }
        //}

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

        #region 分片上传
        [HttpPost]
        public IActionResult Upload()
        {
            //string fileName = Request.Form["name"];
            int index = Convert.ToInt32(Request.Form["chunk"]);//当前分块序号
            var guid = Request.Form["guid"];//前端传来的GUID号
            var dir = $"{ Env.ContentRootPath }/Upload/测试/{ guid }/";//临时保存分块的目录
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            string filePath = dir + index.ToString();//分块文件名为索引名，更严谨一些可以加上是否存在的判断，防止多线程时并发冲突
            var data = Request.Form.Files["file"];//表单中取得分块文件
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                data.CopyTo(stream);
            }
            return Ok(new { error = 0 });
        }

        [HttpPost]
        public IActionResult Merge()
        {
            var uploadDir = Env.ContentRootPath + @"/Upload/测试/";//Upload 文件夹
            var dir = Path.Combine(uploadDir, Request.Form["guid"]);//临时文件夹
            var fs = new FileStream(Path.Combine(uploadDir, Request.Form["fileName"]), FileMode.Create);
            foreach (var part in Directory.GetFiles(dir).OrderBy(x => x.Length).ThenBy(x => x))//排一下序，保证从0-N Write
            {
                var bytes = System.IO.File.ReadAllBytes(part);
                fs.Write(bytes, 0, bytes.Length);
                bytes = null;
                System.IO.File.Delete(part);//删除分块
            }
            fs.Flush();
            fs.Close();
            Directory.Delete(dir);//删除文件夹
            return Ok(new { error = 0 });//随便返回个值，实际中根据需要返回
        }
        #endregion

    }

}