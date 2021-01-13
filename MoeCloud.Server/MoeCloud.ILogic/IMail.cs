using System;
using System.Collections.Generic;
using System.Text;

namespace MoeCloud.ILogic
{
    public interface IMail
    {
        Model.Mail GetMail();
        bool EditMail(Model.Mail mail);
    }
}
