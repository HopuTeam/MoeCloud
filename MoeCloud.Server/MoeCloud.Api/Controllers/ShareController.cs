using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoeCloud.Api.Controllers
{
    [ApiController]
    [Route("[Controller]/[Action]")]
    public class ShareController : Controller
    {
        private ILogic.IShare Ishare { get; }
        public ShareController(ILogic.IShare ishare)
        {
            Ishare = ishare;
        }

        #region Result Class
        public class Result
        {
            public bool State { get; set; }
            public object Data { get; set; }
            public string Message { get; set; }
            public int Code { get; set; }
            public static Result Success(string _message = "", object _data = null)
            {
                return new Result()
                {
                    State = true,
                    Message = _message,
                    Data = _data
                };
            }
            public static Result Failed(string _message = "")
            {
                return new Result()
                {
                    State = false,
                    Message = _message
                };
            }
        }
        #endregion

        // 创建一条分享链接
      
        [HttpPost]
        public Result Create([FromForm] Model.Share share)
        {
            if (share.FileID == 0 || share.UserID == 0)
                return Result.Failed("数据异常");

            var shareInfo = Ishare.Create(share);
            if (shareInfo == null)
                return Result.Failed("数据异常");

            return Result.Success("ok", new { shareInfo.Link });
        }

        
        [HttpPost]
        public Result Link([FromForm] string auth)
        {
            //if (auth == null || auth.Length != 9)
            //    return Result.Failed("分享的文件不存在");
            var List = Ishare.GetDetail(auth);
            if (List.Count > 0)
                return Result.Success("ok", new { List });
            else
                return Result.Failed("分享的文件不存在");
        }

       
        [HttpPost]
        public Result Delete([FromForm] int shareID, int userID)
        {
            if (Ishare.Delete(shareID, userID))
                return Result.Success("ok");
            else
                return Result.Failed("数据异常");
        }

        //change share lock status
        
        [HttpPost]
        public Result LockStatus([FromForm] int shareID)
        {
            if (Ishare.Swich(shareID))
                return Result.Success("ok");
            else
                return Result.Failed("数据异常");
        }

        // 管理员获取所有分享的文件信息
      
        [HttpPost]
        public Result  GetList([FromForm] int userID)
        {
            return Result.Success("ok", new { List = Ishare.GetList(userID) });
        }
    }
}