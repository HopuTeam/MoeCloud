using System;
using System.Collections.Generic;
using System.Text;

namespace MoeCloud.Logic
{
    public class UserLogic : ILogic.IUser
    {
        private Data.CoreEntities EF { get; }
        public UserLogic(Data.CoreEntities _ef)
        {
            EF = _ef;
        }


    }
}
