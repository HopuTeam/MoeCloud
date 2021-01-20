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
    public class ManagerController : Controller
    {
        private ILogic.ISite Isite { get; }
        private ILogic.IReg Ireg { get; }
        private ILogic.IMail Imail { get; }
        private ILogic.IUser Iuser { get; }
        public ManagerController(ILogic.ISite isite, ILogic.IReg ireg, ILogic.IMail imail, ILogic.IUser iuser)
        {
            Isite = isite;
            Ireg = ireg;
            Imail = imail;
            Iuser = iuser;
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

        // 获取网站基本信息
        [Authorize]
        [HttpPost]
        public Result GetSite([FromBody] int userID)
        {
            if (Iuser.GetUser(userID).RoleID != 1)
                return Result.Failed("权限不足");

            return Result.Success("ok", new { Info = Isite.GetSite() });
        }
        // 修改网站基本信息
        [Authorize]
        [HttpPost]
        public Result EditSite([FromForm] int userID, Model.Site site)
        {
            if (Iuser.GetUser(userID).RoleID != 1)
                return Result.Failed("权限不足");

            if (Isite.EditSite(site))
                return Result.Success("ok");

            return Result.Failed("数据异常");
        }

        // 获取注册设置信息
        [Authorize]
        [HttpPost]
        public Result GetReg([FromBody] int userID)
        {
            if (Iuser.GetUser(userID).RoleID != 1)
                return Result.Failed("权限不足");

            return Result.Success("ok", new { Info = Ireg.GetReg() });
        }
        // 修改注册设置信息
        [Authorize]
        [HttpPost]
        public Result EditReg([FromForm] int userID, Model.Reg reg)
        {
            if (Iuser.GetUser(userID).RoleID != 1)
                return Result.Failed("权限不足");

            if (Ireg.EditReg(reg))
                return Result.Success("ok");

            return Result.Failed("数据异常");
        }

        // 获取邮件功能信息
        [Authorize]
        [HttpPost]
        public Result GetMail([FromBody] int userID)
        {
            if (Iuser.GetUser(userID).RoleID != 1)
                return Result.Failed("权限不足");

            return Result.Success("ok", new { Info = Imail.GetMail() });
        }
        // 修改邮件功能信息
        [Authorize]
        [HttpPost]
        public Result EditMail([FromForm] int userID, Model.Mail mail)
        {
            if (Iuser.GetUser(userID).RoleID != 1)
                return Result.Failed("权限不足");

            if (Imail.EditMail(mail))
                return Result.Success("ok");

            return Result.Failed("数据异常");
        }
    }
}