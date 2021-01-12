using System;
using System.Collections.Generic;
using System.Text;

namespace MoeCloud.ILogic
{
    public interface IUser
    {
        Model.User Sign(Model.User user);
        List<Model.User> GetUsers();
        bool RestPassword(int id, string password);
        Model.User GetUser(int id, string account, string email);
        bool DelUser(int id);
        bool AddUser(Model.User user);
    }
}