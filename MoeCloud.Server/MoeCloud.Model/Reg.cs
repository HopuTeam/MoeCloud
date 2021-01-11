using System;
using System.Collections.Generic;
using System.Text;

namespace MoeCloud.Model
{
    public class Reg
    {
        //是否开启注册功能
        public bool AllowReg { get; set; }
        //是否开启邮件激活功能
        public bool EmailActive { get; set; }
        //是否开启注册验证码
        public bool RegCode { get; set; }
        //是否开启登录验证码
        public bool SignCode { get; set; }
        //默认角色ID，默认2
        public int DefaultRoleID { get; set; }
    }
}