using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using MoeCloud.Api.Handles;
using MoeCloud.Model;
using MoeCloud.Logic;
using MoeCloud.ILogic;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Checksum;

namespace MoeCloud.Api.Controllers
{
    [Route("[Controller]/[Action]")]
    [ApiController]
    public class FileUploadController : ControllerBase
    {
        private readonly JwtHelper jwt;
        private readonly IFile file;

        public IWebHostEnvironment Env { get; }

        public FileUploadController(IWebHostEnvironment env, JwtHelper jwt, IFile file)
        {
            Env = env;
            this.jwt = jwt;
            this.file = file;
        }

        #region 文件夹上传
        [HttpPost]
        //文件夹上传
        public IActionResult UpLoadOne()
        {
            var files = Request.Form.Files;
            int a = 0;
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
                    a++;
                }
                if (a==files.Count())//如果a=文件的总数就是成功上传
                {
                    return Ok(new
                    {
                        success = true,
                        message = "上传成功"
                    });
                }
                return Ok(new
                {
                    success = false,
                    message = "上传失败"
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
              
        public ActionResult Merge()
        {
            var uploadDir = Env.ContentRootPath + @"/Upload/测试/";//Upload 文件夹
            var dir = Path.Combine(uploadDir, Request.Form["guid"]);//临时文件夹
            string  fileName = Request.Form["fileName"];
            var fs = new FileStream(Path.Combine(uploadDir, fileName), FileMode.Create);
            foreach (var part in Directory.GetFiles(dir).OrderBy(x => x.Length).ThenBy(x => x))//排一下序，保证从0-N Write
            {
                var bytes = System.IO.File.ReadAllBytes(part);
                fs.Write(bytes, 0, bytes.Length);
                bytes = null;
                System.IO.File.Delete(part);//删除分块
            }
            long size = fs.Length;          
            string[] lij = fs.Name.Split("Upload");
            string path = @"1/" + lij[1];//存的虚路径
            //string pid = file.DirFind();
            fs.Flush();
            fs.Close();
            Directory.Delete(dir);//删除文件夹
            Model.File aa = new Model.File
            {
                Name = fileName,
                Size = size,
                UserID = 1,
                Path = path,
                //ParentID=
            };
           bool c= file.Create(aa);
            if (c)
            {
                return Ok(new { error = "成功"});//随便返回个值，实际中根据需要返回
            }
            return Ok(new { error = "失败" });//随便返回个值，实际中根据需要返回
        }
        #endregion




        [AllowAnonymous]//跳过Jwt验证     
        [HttpPost]
        public ActionResult JwtYz( int id = 1)
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
        [HttpPost]
        public ActionResult Parsing()
        {          
            var data = HttpContext.Request.GetModel<Role>();
            return Ok(new { successful = data });
        }

        #region 压缩文件
        /// <summary>
        /// 压缩
        /// </summary>
        /// <param name="source">源目录,要压缩的文件夹，@"F:\111"</param>
        /// <param name="strZip">压缩包的名字，111.zip</param>
        [HttpPost]
        public void ZipFile(string strFile, string strZip)
        {
            strFile = @"F:\111";
            strZip = "111.zip";
            var uploadDir = Env.ContentRootPath + @"/Upload/测试/";//Upload 文件夹           
            var fs = new FileStream(Path.Combine(uploadDir, strZip), FileMode.Create);//创建压缩文件夹           
            //string a = fs.Name.Substring(fs.Name.LastIndexOf("\\")+1);//拿到文件名字
            if (strFile[strFile.Length - 1] != Path.DirectorySeparatorChar)
                strFile += Path.DirectorySeparatorChar;
            ZipOutputStream s = new ZipOutputStream(fs);
            s.SetLevel(6); //压缩级别
            zip(strFile, s, strFile);//递归
            s.Finish();
            s.Close();//关闭并释放文件流
        }

        private void zip(string strFile, ZipOutputStream s, string staticFile)
        {
            if (strFile[strFile.Length - 1] != Path.DirectorySeparatorChar)
                strFile += Path.DirectorySeparatorChar;
            Crc32 crc = new Crc32();
            string[] filenames = Directory.GetFileSystemEntries(strFile);//打开文件夹，拿到文件夹里面文件的数量
            foreach (string file in filenames)
            {

                if (Directory.Exists(file))
                {
                    zip(file, s, staticFile);
                }

                else // 否则直接压缩文件
                {
                    //var fs = new FileStream(finalFilePath, FileMode.Create)
                    //打开压缩文件
                    FileStream fs =new FileStream(file, FileMode.Create);

                    byte[] buffer = new byte[fs.Length];
                    fs.Read(buffer, 0, buffer.Length);
                    string tempfile = file.Substring(staticFile.LastIndexOf("\\") + 1);
                    ZipEntry entry = new ZipEntry(tempfile);

                    entry.DateTime = DateTime.Now;
                    entry.Size = fs.Length;
                    fs.Close();
                    crc.Reset();
                    crc.Update(buffer);
                    entry.Crc = crc.Value;
                    s.PutNextEntry(entry);

                    s.Write(buffer, 0, buffer.Length);
                }
            }
        }
        #endregion

    }

}

