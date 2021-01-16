using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace MoeCloud.Logic
{
    public class RoleLogic : ILogic.IRole
    {
        private Data.CoreEntities EF { get; }
        public RoleLogic(Data.CoreEntities _ef)
        {
            EF = _ef;
        }

        public bool AddRole(Model.Role role)
        {
            try
            {
                role.Status = true;
                EF.Add(role);
                EF.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool DelRole(int id)
        {
            try
            {
                if (id >= 0 && id <= 3)
                    return false;
                EF.Remove(EF.Roles.FirstOrDefault(x => x.ID == id));
                EF.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool EditRole(Model.Role role)
        {
            try
            {
                var mod = EF.Roles.FirstOrDefault(x => x.ID == role.ID);
                mod.Name = role.Name;
                mod.MaxSize = role.MaxSize;
                mod.Speed = role.Speed;
                mod.Bundle = role.Bundle;
                mod.Release = role.Release;
                EF.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<Model.Role> GetList()
        {
            return EF.Roles.ToList();
        }

        public Model.Role GetRole(int id)
        {
            return EF.Roles.FirstOrDefault(x => x.ID == id);
        }
    }
}