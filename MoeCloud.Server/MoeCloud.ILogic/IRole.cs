using System;
using System.Collections.Generic;
using System.Text;

namespace MoeCloud.ILogic
{
    public interface IRole
    {
        bool AddRole(Model.Role role);
        bool DelRole(int id);
        bool EditRole(Model.Role role);
        List<Model.Role> GetList();
        Model.Role GetRole(int id);
    }
}