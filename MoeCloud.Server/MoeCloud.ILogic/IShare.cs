using System;
using System.Collections.Generic;
using System.Text;

namespace MoeCloud.ILogic
{
    public interface IShare
    {
        Model.Share Create(Model.Share share);
        bool Delete(int id);
        bool Swich(int id);
        List<Model.Share> GetList(int userID);
    }
}