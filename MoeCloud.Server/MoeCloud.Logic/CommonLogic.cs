using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace MoeCloud.Logic
{
    public class CommonLogic : ILogic.ICommon
    {
        private Data.CoreEntities EF { get; }
        public CommonLogic(Data.CoreEntities _ef)
        {
            EF = _ef;
        }

        /// <summary>
        /// 邮件发送
        /// </summary>
        /// <param name="mail">收件人地址</param>
        /// <param name="title">邮件标题</param>
        /// <param name="content">邮件内容(可包含HTML内容)</param>
        /// <returns>是否成功</returns>
        public bool SendMail(string mail, string title, string content)
        {
            var mod = EF.Mails.FirstOrDefault();
            if (Common.MailHelper.SendMail(mod.Host, mod.Account, mod.Password, mod.Port, mod.From, mod.SSL, mail, title, content))
                return true;
            else
                return false;
        }
    }
}