using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace MoeCloud.Logic
{
    public class RegLogic : ILogic.IReg
    {
        private Data.CoreEntities EF { get; }
        public RegLogic(Data.CoreEntities _ef)
        {
            EF = _ef;
        }

        //查询
        public Model.Reg GetReg()
        {
            return EF.Regs.FirstOrDefault();
        }

        //修改
        public bool EditReg(Model.Reg reg)
        {
            try
            {
                var mod = EF.Regs.FirstOrDefault();
                mod.AllowReg = reg.AllowReg;
                mod.EmailActive = reg.EmailActive;
                mod.RegCode = reg.RegCode;
                mod.SignCode = reg.SignCode;
                mod.DefaultRoleID = reg.DefaultRoleID;
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