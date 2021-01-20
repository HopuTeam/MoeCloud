using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace MoeCloud.Logic
{
    public class ShareLogic : ILogic.IShare
    {
        private Data.CoreEntities EF { get; }
        public ShareLogic(Data.CoreEntities _ef)
        {
            EF = _ef;
        }

        // create new share => 分享的提取码(Code)请在前端传值并验证
        public Model.Share Create(Model.Share share)
        {
            share.AddTime = DateTime.Now;
            share.Link = Common.Security.MD5Encrypt32(share.FileID.ToString()).Substring(new Random().Next(1, 19), 9).ToLower();
            EF.Add(share);
            if (EF.SaveChanges() > 0)
                return share;
            else
                return null;
        }

        // 查看分享的详情内容
        public List<Model.File> GetDetail(string auth)
        {
            return (from s in EF.Shares
                    join f in EF.Files on s.FileID equals f.ID
                    where s.Link == auth
                    select f).ToList();
        }

        // delete a share
        public bool Delete(int id, int userID)
        {
            try
            {
                if (userID == 0)
                    EF.Remove(EF.Shares.Where(x => x.ID == id));
                else
                    EF.Remove(EF.Shares.Where(x => x.ID == id && x.UserID == userID));

                EF.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        // swich share function => 用户端
        public bool Swich(int id)
        {
            try
            {
                Random ran = new Random();
                var mod = EF.Shares.FirstOrDefault(x => x.ID == id);
                if (mod.Code == null)
                    mod.Code = Common.Security.MD5Encrypt32(ran.Next(1000, 9999).ToString()).Substring(ran.Next(1, 16), 6).ToLower();
                else
                    mod.Code = null;
                EF.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        // get all list => 管理员查全部请传 userID = 0
        public List<Model.Share> GetList(int userID)
        {
            if (userID == 0)
                return EF.Shares.ToList();
            else
                return EF.Shares.Where(x => x.UserID == userID).ToList();
        }
    }
}