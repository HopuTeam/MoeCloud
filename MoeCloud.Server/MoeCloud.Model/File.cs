using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MoeCloud.Model
{
    public class File
    {
        [Key]
        public int ID { get; set; }
        //文件名
        public string Name { get; set; }
        //文件大小
        public long Size { get; set; }
        //上级目录ID
        public int ParentID { get; set; }
        //用户ID
        public int SignID { get; set; }
        //文件上传时间
        public DateTime AddTime { get; set; }
        //文件状态/是否删除
        public bool Status { get; set; }
        //何时该文件彻底从回收站删除
        public DateTime DelTime { get; set; }
        //实际虚路径
        public string Path { get; set; }
    }
}