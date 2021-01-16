using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace MoeCloud.Logic
{
    public class MailLogic : ILogic.IMail
    {
        private Data.CoreEntities EF { get; }
        public MailLogic(Data.CoreEntities _ef)
        {
            EF = _ef;
        }

        //查询
        public Model.Mail GetMail()
        {
            return EF.Mails.FirstOrDefault();
        }

        //修改
        public bool EditMail(Model.Mail mail)
        {
            try
            {
                var mod = EF.Mails.FirstOrDefault();
                mod.Host = mail.Host;
                mod.Account = mail.Account;
                mod.Password = mail.Password;
                mod.Port = mail.Port;
                mod.From = mail.From;
                mod.SSL = mail.SSL;
                mod.ActiveModel = mail.ActiveModel;
                mod.ResetModel = mail.ResetModel;
                EF.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}