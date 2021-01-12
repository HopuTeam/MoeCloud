using System;
using System.Collections.Generic;
using System.Text;

namespace MoeCloud.ILogic
{
    public interface ICommon
    {
        bool SendMail(string mail, string title, string content);
    }
}