using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MoeCloud.Model
{
    public class Share
    {
        [Key]
        public int ID { get; set; }
        //文件ID
        public int FileID { get; set; }
        //用户ID
        public int UserID { get; set; }
        //分享时间
        public DateTime AddTime { get; set; }
        //下载次数
        public int DownNum { get; set; }
        //分享地址{localhost}:{port}/{s}/{md5type32}
        public string Link { get; set; }
        //提取码(用于判断是否私密分享以及提取文件)
        public string Code { get; set; }
        //CyType，文件停止分享方式，0则不管，1为次数，2为天数
        public int CyType { get; set; }
        //过期(次/天)数，为0时该文件可一直分享
        public int Cycle { get; set; }
    }
}