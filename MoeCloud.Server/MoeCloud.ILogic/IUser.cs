using System;
using System.Collections.Generic;
using System.Text;

namespace MoeCloud.ILogic
{
    public interface IUser
    {
        Model.User GetSign(Model.User user);
    }
}