using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using MoeCloud.ILogic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MoeCloud.Web.Controllers
{
    public class FileController : Controller
    {     
        private readonly IFile Ifile;
        public IWebHostEnvironment Env { get; }

        public FileController(IWebHostEnvironment env, IFile Ifile)
        {
            Env = env;          
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
        public Result GetFiles([FromForm] int Userid, int ParentID)
        {
            List<Model.File> files = Ifile.GetFiles(Userid, ParentID).ToList();
            //var res = Newtonsoft.Json.JsonConvert.SerializeObject(files);
            if (files.Count > 0)
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
        public Result GetFile([FromForm] int Userid, int ParentID)
        {
            return Result.Success("", new { File = Ifile.GetFile(Userid, ParentID) });
        }

        #region 文件夹上传
        /// <summary>
        /// 文件夹上传
        /// </summary>
        /// <param name="strPath">保存文件的路径</param>
        /// <returns></returns>
        [HttpPost]
        //文件夹上传
        public IActionResult UpLoadOne([FromForm] int ID, int Pid)
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
                    return Json(new
                    {
                        success = true,
                        message = "上传成功"
                    });
                }
                return Json(new
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
        private void DicCreate(string path, string thname, int Userid)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                string[] arrpath = path.Split("UserFiles", StringSplitOptions.RemoveEmptyEntries);//去除根目录
                string Virtualpath = arrpath[1];//获得虚路径
                string[] aaa = Virtualpath.Split(new char[] { '\\', '/' }, StringSplitOptions.RemoveEmptyEntries);
                string folder = aaa[aaa.Length - 1];//选取分割数组里面最后一个，就是文件夹名字
                string[] shnagji = Virtualpath.Split($"{folder}", StringSplitOptions.RemoveEmptyEntries);//删除当前文件名得到上级目录           
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
        public ActionResult Upload([FromBody] Model.File view)
        {
            //string fileName = Request.Form["name"];
            int index = Convert.ToInt32(Request.Form["chunk"]);//当前分块序号
            var guid = Request.Form["guid"];//前端传来的GUID号
            //var dir = $"{ Env.ContentRootPath }/Upload/UserFiles/{1}/aaa/{ guid }/";//临时保存分块的目录
            var dir = Env.ContentRootPath + @"/Upload/UserFiles"+ view.Path;//临时保存分块的目录
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

        public ActionResult Merge([FromForm] string strPath)//1是模拟用户id
        {

            var uploadDir = Env.ContentRootPath + @"/Upload/UserFiles" + strPath;//Upload 文件夹          
            var dir = Path.Combine(uploadDir, Request.Form["guid"]);//临时文件夹
            string fileName = Request.Form["fileName"];
            var fs = new FileStream(Path.Combine(uploadDir, fileName), FileMode.Create);
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
            int pid = Ifile.DirFind($"{strPath}/").ID;
             
            Model.File aa = new Model.File
            {
                Name = fileName,
                Size = size,
                UserID = User.ID,
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





















    }
}
