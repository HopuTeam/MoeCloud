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
        //文件大小，单位B
        public long Size { get; set; }
        //上级目录ID，仅文件夹
        public int ParentID { get; set; }
        //用户ID
        public int UserID { get; set; }
        //文件上传至时间
        public DateTime AddTime { get; set; }
        /*
         * 何时该文件彻底从回收站删除
         * 7天后删除示例:DateTime.Now.AddDays(7);
         * 超过可撤回时间后不在用户列表展示
         */
        public DateTime? DelTime { get; set; }
        //实际虚路径
        public string Path { get; set; }
    }
}