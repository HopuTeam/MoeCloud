using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MoeCloud.Model
{
    public class User
    {
        [Key]
        public int ID { get; set; }
        //用户邮件地址
        public string Email { get; set; }
        //账号
        public string Account { get; set; }
        //密码
        public string Password { get; set; }
        //已使用的存储空间，默认0
        public long UseSize { get; set; }
        //注册时间
        public DateTime EntryTime { get; set; }
        //帐号状态(是否通过邮箱验证)
        public bool Active { get; set; }
        //账号状态(是否封禁)
        public bool Status { get; set; }
        //角色ID，默认2(注册用户)
        public int RoleID { get; set; }
    }
}