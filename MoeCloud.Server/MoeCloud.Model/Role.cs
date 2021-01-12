using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MoeCloud.Model
{
    public class Role
    {
        [Key]
        public int ID { get; set; }
        //角色名称
        public string Name { get; set; }
        //存储最大值，B为单位
        public long MaxSize { get; set; }
        //最大下载速度，B为单位
        public long Speed { get; set; }
        //是否允许打包下载
        public bool Bundle { get; set; }
        //是否允许压缩/解压缩
        public bool Release { get; set; }
        //是否可被分配，默认为true，游客为false，不可更改
        public bool Status { get; set; }
    }
}