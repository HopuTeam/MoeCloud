using System;
using System.Collections.Generic;
using System.Text;

namespace MoeCloud.Model
{
    public class Mail
    {
        //SMTP地址
        public string Host { get; set; }
        //STMP账号/发件人邮箱
        public string Account { get; set; }
        //SMTP密码
        public string Password { get; set; }
        //端口
        public int Port { get; set; }
        //发信邮箱用户名，一般与邮箱地址相同
        public string From { get; set; }
        //是否使用加密连接
        public bool SSL { get; set; }
        //用户激活邮件模板
        public string ActiveModel { get; set; }
        //重置密码邮件模板
        public string ResetModel { get; set; }
    }
}