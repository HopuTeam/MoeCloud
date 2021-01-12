using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace MoeCloud.Logic
{
    public class UserLogic : ILogic.IUser
    {
        private Data.CoreEntities EF { get; }
        public UserLogic(Data.CoreEntities _ef)
        {
            EF = _ef;
        }

        public Model.User GetSign(Model.User user)
        {
            return EF.Users.FirstOrDefault(x => x.Account == user.Account && x.Password == user.Password);
        }
    }
}