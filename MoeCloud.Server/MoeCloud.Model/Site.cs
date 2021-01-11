using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MoeCloud.Model
{
    public class Site
    {
        [Key]
        public int ID { get; set; }
        //主标题
        public string MainTitle { get; set; }
        //副标题
        public string SecTitle { get; set; }
        //站点描述
        public string Description { get; set; }
        //站点地址{localhost}:{port}
        public string Url { get; set; }
        //全局脚本
        public string Script { get; set; }
    }
}