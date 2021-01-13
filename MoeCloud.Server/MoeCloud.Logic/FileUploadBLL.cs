using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MoeCloud.Logic
{
  public  class FileUploadBLL: ILogic.IFileUploadBLL
    {
       
        public int Upload(string rootpath, int index,StringValues guid,IFormFile data)
        {           
            var dir = rootpath;//文件上传目录
            dir = Path.Combine(dir, guid);//临时保存分块的目录
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            string filePath = Path.Combine(dir, index.ToString());//分块文件名为索引名，更严谨一些可以加上是否存在的判断，防止多线程时并发冲突          
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                data.CopyTo(stream);
            };
            int a = 0;
            return  a;
        }

        public int Merge(string rootpath, StringValues guid,  StringValues fileName)
        {                     
            var dir = Path.Combine(rootpath, guid);//临时文件夹
            var files = Directory.GetFiles(dir);//获得下面的所有文件
            var finalPath = Path.Combine(rootpath, fileName);//最终的文件名（demo中保存的是它上传时候的文件名，实际操作肯定不能这样）
            var fs = new FileStream(finalPath, FileMode.Create);
            foreach (var part in files.OrderBy(x => x.Length).ThenBy(x => x))//排一下序，保证从0-N Write
            {
                var bytes = System.IO.File.ReadAllBytes(part);
                fs.Write(bytes, 0, bytes.Length);
                bytes = null;
                File.Delete(part);//删除分块
            }
            fs.Flush(); 
            fs.Close();
            Directory.Delete(dir);//删除文件夹
            int a = 0;
            return a;
        }


    }
}
