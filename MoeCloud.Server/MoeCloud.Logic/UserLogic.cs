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

        //登录
        public Model.User Sign(Model.User user)
        {
            return EF.Users.FirstOrDefault(x => x.Account == user.Account && x.Password == Common.Security.MD5Encrypt32(user.Password));
        }

        //获取所有用户信息
        public List<Model.User> GetUsers()
        {
            return EF.Users.ToList();
        }

        //重置密码，通过ID
        public bool RestPassword(int id, string password)
        {
            var mod = EF.Users.FirstOrDefault(x => x.ID == id);
            mod.Password = Common.Security.MD5Encrypt32(password);
            if (EF.SaveChanges() > 0)
                return true;
            else
                return false;
        }

        //获取一个用户的信息，可为ID、Account、Email
        public Model.User GetUser(int id, string account, string email)
        {
            Model.User user = null;
            if (id != 0)
                user = EF.Users.FirstOrDefault(x => x.ID == id);
            else if (account != null)
                user = EF.Users.FirstOrDefault(x => x.Account == account);
            else if (email != null)
                user = EF.Users.FirstOrDefault(x => x.Email == email);
            return user;
        }

        //Delete a user
        public bool DelUser(int id)
        {
            EF.Remove(EF.Users.FirstOrDefault(x => x.ID == id));
            EF.Remove(EF.Files.Where(x => x.UserID == id));
            if (EF.SaveChanges() > 0)
                return true;
            else
                return false;
        }

        //Add user
        public bool AddUser(Model.User user)
        {
            user.Password = Common.Security.MD5Encrypt32(user.Password);
            EF.Users.Add(user);
            if (EF.SaveChanges() > 0)
                return true;
            else
                return false;
        }

        //Edit a user
        //public bool EditUser(Model.User user)
        //{
        //    var mod = EF.Users.FirstOrDefault(x => x.ID == user.ID);
        //    mod.
        //}
    }
}