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
        Model.User GetUser(int id = 0, string account = null, string email = null);
        bool DelUser(int id);
        bool AddUser(Model.User user);
        bool EditUser(Model.User user);
        bool UpdateUseSize(int id, long useSize);
        bool ChangeRole(int id, int roleID);
        bool ChangeActive(int id);
        bool SwichStatus(int id);
        bool EditPassword(int id, string password);
    }
}