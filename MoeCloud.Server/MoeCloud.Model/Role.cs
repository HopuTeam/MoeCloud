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
        //是否可被分配
        public bool Status { get; set; }
        //public int BucketID { get; set; }
    }
}