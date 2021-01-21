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
        private readonly IFile Ifile;

        public IWebHostEnvironment Env { get; }

        public FileUploadController(IWebHostEnvironment env, JwtHelper jwt, IFile Ifile)
        {
            Env = env;
            this.jwt = jwt;
            this.Ifile = Ifile;
        }
        public class Result
        {
            public bool State { get; set; }
            public object Data { get; set; }
            public string Message { get; set; }
            public int Code { get; set; }
            public static Result Success(string _message = "", object _data = null)
            {
                return new Result()
                {
                    State = true,
                    Message = _message,
                    Data = _data
                };
            }
            public static Result Failed(string _message = "")
            {
                return new Result()
                {
                    State = false,
                    Message = _message
                };
            }
        }
        /// <summary>
        /// 查询文件列表
        /// </summary>
        /// <param name="Userid">用户的ad</param>
        /// <param name="ParentID">文件父级id</param>
        /// <returns></returns>
        [HttpPost]
         public Result GetFiles([FromForm] int Userid,int ParentID)
        {
            List<Model.File> files = Ifile.GetFiles(Userid, ParentID).ToList();
            //var res = Newtonsoft.Json.JsonConvert.SerializeObject(files);
            if (files.Count >0)
            {
                return Result.Success("", files);
            }
            return Result.Failed("查不到数据");
        }
        /// <summary>
        /// 查询单文件
        /// </summary>
        /// <param name="Userid"></param>
        /// <param name="ParentID"></param>
        /// <returns></returns>
        [HttpPost]
        public Result  GetFile([FromForm] int Userid, int ParentID)
        {
            return Result.Success("",new { File = Ifile.GetFile(Userid, ParentID) });
        }


        #region 文件夹上传
        /// <summary>
        /// 文件夹上传
        /// </summary>
        /// <param name="strPath">保存文件的路径</param>
        /// <returns></returns>
        [HttpPost]
        //文件夹上传
        public IActionResult UpLoadOne([FromForm] int ID,int Pid)
        {        
            string strPath = string.Empty;
            var files = Request.Form.Files;
            //var User = HttpContext.Request.GetModel<User>();   //当前登录账户     
            int a = 0;
            strPath = Ifile.GetFile(ID, Pid).Path;//父级目录         
            long Size = files.Sum(f => f.Length);//计算文件大小          
            string rootpath = $"{Env.ContentRootPath}/Upload/UserFiles" + strPath; //获取根目录
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
                        dirpath += arrpath[i] + @"\";
                    }
                    DicCreate(Path.Combine(rootpath, dirpath), dirpath, ID);//不存在则创建该目录,dirpath=>当前创建的文件夹名称
                    string filepath = Path.Combine(rootpath, file.FileName);
                    using (var addFile = new FileStream(filepath, FileMode.OpenOrCreate))
                    {
                        if (file != null)
                        {
                            string[] path = addFile.Name.Split("UserFiles", StringSplitOptions.RemoveEmptyEntries);//去除根目录
                            string[] Thesuperior = path[1].Split($"{filename}", StringSplitOptions.RemoveEmptyEntries);//文件的上级目录
                            int pid = Ifile.DirFind($"{Thesuperior[0]}").ID;//上级目录id
                            Model.File aa = new Model.File
                            {
                                Name = filename,
                                Size = addFile.Length,
                                UserID = 1,
                                Path = path[1],
                               ParentID = pid
                            };
                            bool c = Ifile.Create(aa);
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
                if (a == files.Count())//如果a=文件的总数就是成功上传
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
        private void DicCreate( string path, string thname, int Userid)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);              
                string[] arrpath = path.Split("UserFiles",StringSplitOptions.RemoveEmptyEntries);//去除根目录
                string Virtualpath = arrpath[1];//获得虚路径
                string[] aaa = Virtualpath.Split(new char[] { '\\', '/' }, StringSplitOptions.RemoveEmptyEntries);
                string folder = aaa[aaa.Length - 1];//选取分割数组里面最后一个，就是文件夹名字
                string[] shnagji = Virtualpath.Split($"{folder}",StringSplitOptions.RemoveEmptyEntries);//删除当前文件名得到上级目录           
                int pid = Ifile.DirFind($"{shnagji[0]}").ID;//上级目录id                
                if (Virtualpath[Virtualpath.Length - 1] != Path.DirectorySeparatorChar)//保证文件夹后面是\\ 斜杆
                    Virtualpath += Path.DirectorySeparatorChar;
                Model.File xxx = new Model.File
                {
                    Name = thname,
                    Size = 0,
                    UserID = Userid,
                    Path = Virtualpath,
                    ParentID = pid
                };
                Ifile.Create(xxx);
            }
        }

        #endregion

        #region 分片上传
        [HttpPost]
        public ActionResult Upload([FromBody]  Model.File view)
        {
            //string fileName = Request.Form["name"];
            int index = Convert.ToInt32(Request.Form["chunk"]);//当前分块序号
            var guid = Request.Form["guid"];//前端传来的GUID号
            var dir = Env.ContentRootPath + @"/Upload/UserFiles" + view.Path;//临时保存分块的目录
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            string filePath = dir + index.ToString();//分块文件名为索引名，更严谨一些可以加上是否存在的判断，防止多线程时并发冲突
            var data = Request.Form.Files["file"];//表单中取得分块文件
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                data.CopyTo(stream);
            }           
            return Ok(new { error = "ok" });
        }

        public ActionResult Merge([FromBody] Model.File view)//1是模拟用户id
        {

            var uploadDir = Env.ContentRootPath + @"/Upload/UserFiles" + view.Path;//Upload 文件夹          
            var dir = Path.Combine(uploadDir, Request.Form["guid"]);//临时文件夹
            string fileName = Request.Form["fileName"];
            var fs = new FileStream(Path.Combine(uploadDir), FileMode.Create);
            foreach (var part in Directory.GetFiles(dir).OrderBy(x => x.Length).ThenBy(x => x))//排一下序，保证从0-N Write
            {
                var bytes = System.IO.File.ReadAllBytes(part);
                fs.Write(bytes, 0, bytes.Length);
                bytes = null;
                System.IO.File.Delete(part);//删除分块
            }
            Directory.Delete(dir);//删除文件夹
            fs.Flush();
            fs.Close();
            long size = fs.Length;
            string[] lij = fs.Name.Split("UserFiles");
            string path = lij[1];
            int pid = Ifile.GetFile(view.ID, view.ParentID).ID;
           // var User = HttpContext.Request.GetModel<User>();   //当前登录账户        
            Model.File aa = new Model.File
            {
                Name = fileName,
                Size = size,
                UserID = view.ID,
                Path = path,
                ParentID = pid
            };
            bool c = Ifile.Create(aa);
            if (c)
            {
                return Ok(new { error = "成功" });//随便返回个值，实际中根据需要返回
            }
            return Ok(new { error = "失败" });//随便返回个值，实际中根据需要返回
        }
        #endregion
    
        #region 压缩文件
        /// <summary>
        /// 压缩
        /// </summary>
        /// <param name="strFile">源目录,要压缩的文件夹，@"F:\111"</param>
        /// <param name="strZip">压缩包的名字，111.zip</param>
        [HttpPost]
        public void ZipFile([FromForm] Model.File view)
        {
            string StrFile = Env.ContentRootPath + @"\Upload\UserFiles\" + view.Path;//拼接根目录拿到文件          
            string Theparentdirectory = Ifile.GetFile(view.ID, view.ParentID).Path;  //父级路径        
            string Savethepath = Env.ContentRootPath + @"\Upload\UserFiles\"+ Theparentdirectory;
            string staticFile = view.Name + ".zip"; //压缩文件的名字
                if (!Directory.Exists(Savethepath)) //创建保存的文件夹
                {
                    Directory.CreateDirectory(Savethepath);
                }
                var fs = new FileStream(Path.Combine(Savethepath, staticFile), FileMode.Create);//创建压缩文件夹          
            if (StrFile[StrFile.Length - 1] != Path.DirectorySeparatorChar)//加上 \\ 才能打开文件夹
                StrFile += Path.DirectorySeparatorChar;
            ZipOutputStream s = new ZipOutputStream(fs);
            zip(StrFile, s, staticFile);//递归
            s.SetLevel(6); //压缩级别
            s.Finish();
            s.Close();//关闭并释放文件流
            //int pid = Ifile.DirFind(Theparentdirectory).ID;
            Model.File aa = new Model.File
            {
                Name = staticFile,
                Size = 0,
                UserID = view.ID,
                Path = view.Path+ staticFile,
                ParentID = view.ID
            };
           Ifile.Create(aa); //存进数据库

            //strFile = @"\Upload\UserFiles\1\aaa\ASP.NETCore.docx";

            //string[] aaa = strFile.Split(new char[] { '\\', '/' }, StringSplitOptions.RemoveEmptyEntries);
            //string FileName = aaa[aaa.Length - 1];//拿到文件名
            //string[] Theparentdirectory = strFile.Split($"{FileName}", StringSplitOptions.RemoveEmptyEntries);//父级路径
            //strZip = $"{FileName}.zip";
            //var uploadDir = Env.ContentRootPath+Theparentdirectory[0];//保存的路径
            //if (!Directory.Exists(uploadDir))
            //{
            //    Directory.CreateDirectory(uploadDir);
            //}
            //var fs = new FileStream(Path.Combine(uploadDir, strZip), FileMode.Create);//创建压缩文件夹           
            ////string a = fs.Name.Substring(fs.Name.LastIndexOf("\\")+1);//拿到文件名字
            //if (StrFile[StrFile.Length - 1] != Path.DirectorySeparatorChar)
            //    StrFile += Path.DirectorySeparatorChar;
            //ZipOutputStream s = new ZipOutputStream(fs);
            //s.SetLevel(6); //压缩级别
            // zip(StrFile, s, strZip);//递归
            //s.Finish();
            //s.Close();//关闭并释放文件流
            //          //存进数据库


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
                   
                    FileStream fs = new FileStream(file, FileMode.Open);//打开文件夹
                  
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

        #region 解压缩
        /// <summary>
        /// 
        /// </summary>
        /// <param name="TargetFile">待解压的文件路径</param>
        /// <param name="fileDir">解压到的目录</param>
        /// <returns></returns>
        public string unZipFile([FromBody]  Model.File view )
        {
            string TargetFile = @"";
             string fileDir = "";
            string rootFile = "";

            TargetFile = Env.ContentRootPath+ @"\Upload\UserFiles\"+ view.Path;//文件路径
            fileDir= Ifile.GetFile(view.ID, view.ParentID).Path;  //父级路径
            FileStream fs = new FileStream(TargetFile, FileMode.Open);
            string lj ="@"+ TargetFile;
            ZipInputStream s = new ZipInputStream(fs);
            ZipEntry theEntry;
            string path = fileDir;
            string rootDir = "";
            //TargetFile = Env.ContentRootPath + @"\Upload\UserFiles\"+lj;
            //fileDir = $"{ Env.ContentRootPath }/Upload/UserFiles/{1}/aaa/";
            // FileStream fs = new FileStream(TargetFile, FileMode.Open);
            //读取压缩文件(zip文件),准备解压缩
            // ZipInputStream s = new ZipInputStream(fs);
            // ZipEntry theEntry;
            //string path = fileDir;
            //解压出来的文件保存的路径
            // string rootDir = "";
            //根目录下的第一个子文件夹的名称
            while ((theEntry = s.GetNextEntry()) != null)
            {
                rootDir = Path.GetDirectoryName(theEntry.Name);             
                //得到根目录下的第一级子文件夹的名称
                if (rootDir.IndexOf("\\") >= 0)
                {
                    rootDir = rootDir.Substring(0, rootDir.IndexOf("\\") + 1);
                }
                string dir = Path.GetDirectoryName(theEntry.Name);
                //根目录下的第一级子文件夹的下的文件夹的名称
                string fileName = Path.GetFileName(theEntry.Name);
                //根目录下的文件名称
                if (dir != " ")
                //创建根目录下的子文件夹,不限制级别
                {
                    if (!Directory.Exists(fileDir+ dir))
                    {
                        path = fileDir + dir;//根目录加文件名
                        //在指定的路径创建文件夹
                        Directory.CreateDirectory(path);
                        string[] Paths = path.Split("UserFiles", StringSplitOptions.RemoveEmptyEntries);//切割虚目录
                        string  Virtualpath = Paths[1];//获得虚路径
                        string[] aaa = Virtualpath.Split(new char[] { '/','\\'}, StringSplitOptions.RemoveEmptyEntries);//分割路由便于拿到文件夹名字
                        string folder = aaa[aaa.Length - 1];
                        string[] Purl = Virtualpath.Split($"{folder}", StringSplitOptions.RemoveEmptyEntries);
                        int pid = Ifile.DirFind($"{Purl[0]}").ID;
                        if (Virtualpath[Virtualpath.Length - 1] != Path.DirectorySeparatorChar)//保证文件夹后面是\\ 斜杆
                            Virtualpath += Path.DirectorySeparatorChar;
                        Model.File xxx = new Model.File
                        {
                            Name = folder,
                            Size = 0,
                            UserID = view.ID,
                            Path = Virtualpath,
                            ParentID = pid
                        };
                        Ifile.Create(xxx);
                    }
                }
                else if (dir == " " && fileName != "")
                //根目录下的文件
                {
                    path = fileDir;
                    rootFile = fileName;
                }
                else if (dir != " " && fileName != "")
                //根目录下的第一级子文件夹下的文件
                {
                    if (dir.IndexOf("\\") > 0)
                    //指定文件保存的路径
                    {
                        path = fileDir + "\\" + dir;
                    }
                }

                if (dir == rootDir)
                //判断是不是需要保存在根目录下的文件
                {
                    path = fileDir + "\\" + rootDir;
                }
                //以下为解压缩zip文件的基本步骤
                //基本思路就是遍历压缩文件里的所有文件,创建一个相同的文件。
                if (fileName != String.Empty)
                {
                    FileStream streamWriter = new FileStream(path + "\\" + fileName, FileMode.Create);
                    //fs.Name.Substring(fs.Name.LastIndexOf("\\") + 1);//拿到文件名字                     
                    int size = 2048;
                    byte[] data = new byte[2048];
                    while (true)
                    {
                        size = s.Read(data, 0, data.Length);
                        if (size > 0)
                        {
                            streamWriter.Write(data, 0, size);
                        }
                        else
                        {
                            break;
                        }
                    }
                    string[] Path= streamWriter.Name.Split("UserFiles", StringSplitOptions.RemoveEmptyEntries);//切割虚目录
                    string Virtualpath = Path[1];//获得虚路径
                    string[] Purl = Virtualpath.Split($"{fileName}", StringSplitOptions.RemoveEmptyEntries);//上级目录
                    int pid = Ifile.DirFind($"{Purl[0]}").ID;//父级id
                    Model.File xxx = new Model.File
                    {
                        Name = fileName,
                        Size = streamWriter.Length,
                        UserID = 1,
                        Path = Virtualpath,
                        ParentID = pid
                    };
                    Ifile.Create(xxx);
                    streamWriter.Close();//关闭文件流                 
                }
            }
            s.Close();

            return lj;
        }
        #endregion      

    }

}

