using System;
using System.Collections.Generic;
using System.Text;

namespace MoeCloud.ILogic
{
    public interface IReg
    {
        Model.Reg GetReg();
        bool EditReg(Model.Reg reg);
    }
}
