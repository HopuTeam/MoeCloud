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
            try
            {
                var mod = EF.Users.FirstOrDefault(x => x.ID == id);
                mod.Password = Common.Security.MD5Encrypt32(password);
                EF.SaveChanges();
                return true;
            }
            catch// (Exception ex)
            {
                return false;
            }
        }

        //获取一个用户的信息，可为ID、Account、Email
        public Model.User GetUser(int id = 0, string account = null, string email = null)
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
            try
            {
                EF.Remove(EF.Users.FirstOrDefault(x => x.ID == id));
                EF.Remove(EF.Files.Where(x => x.UserID == id));
                EF.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        //Add user
        public bool AddUser(Model.User user)
        {
            try
            {
                user.Password = Common.Security.MD5Encrypt32(user.Password);
                EF.Users.Add(user);
                EF.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        // Edit user info
        public bool EditUser(Model.User user)
        {
            try
            {
                var mod = EF.Users.FirstOrDefault(x => x.ID == user.ID);
                if (user.Email != null && mod.Email != user.Email)
                {
                    mod.Email = user.Email;
                    mod.Active = false;
                }
                if (user.Account != null)
                    mod.Account = user.Account;
                EF.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        //update useSize
        public bool UpdateUseSize(int id, long useSize)
        {
            try
            {
                var mod = EF.Users.FirstOrDefault(x => x.ID == id);
                mod.UseSize = useSize;
                EF.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        //change role
        public bool ChangeRole(int id, int roleID)
        {
            try
            {
                var mod = EF.Users.FirstOrDefault(x => x.ID == id);
                mod.RoleID = roleID;
                EF.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 改变账户的邮箱激活状态(用于修改绑定邮箱、注册用户等)
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        public bool ChangeActive(int id)
        {
            try
            {
                var mod = EF.Users.FirstOrDefault(x => x.ID == id);
                if (mod.Active)
                    mod.Active = false;
                else
                    mod.Active = true;
                EF.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        // (封禁/解封)帐户
        public bool SwichStatus(int id)
        {
            try
            {
                var mod = EF.Users.FirstOrDefault(x => x.ID == id);
                if (mod.Status)
                    mod.Status = false;
                else
                    mod.Status = true;
                EF.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        // Edit password
        public bool EditPassword(int id, string password)
        {
            try
            {
                var mod = EF.Users.FirstOrDefault(x => x.ID == id);
                mod.Password = Common.Security.MD5Encrypt32(password);
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