using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;

namespace MoeCloud.Web.Controllers
{
    public class SignController : Controller
    {
        private ILogic.IUser Iuser { get; }
        private ILogic.ICommon Icommon { get; }
        public SignController(ILogic.IUser iuser, ILogic.ICommon icommon)
        {
            Iuser = iuser;
            Icommon = icommon;
        }

        #region Result Class
        public class Result
        {
            public bool State { get; set; }
            public object Data { get; set; }
            public string Message { get; set; }
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

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public Result Sign(Model.User user)
        {
            var mod = Iuser.Sign(user);
            if (mod == null)
                return Result.Failed("用户名或密码错误");

            HttpContext.Session.SetModel("User", mod);
            var info = new
            {
                mod.ID,
                mod.Account,
                mod.Email
            };
            return Result.Success($"{mod.Account},欢迎回来", info);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public Result Register(Model.User user)
        {
            if (user.Account == null || user.Email == null || user.Password == null)
                return Result.Failed("数据异常，请重试");

            if (Iuser.GetUser(0, user.Account, null) != null)
                return Result.Failed("用户名已被使用");
            else if (Iuser.GetUser(0, null, user.Email) != null)
                return Result.Failed("邮箱已被绑定");

            user.Active = false;
            user.Status = true;
            user.RoleID = 2;
            user.EntryTime = DateTime.Now;

            if (Iuser.AddUser(user))
                return Result.Success("注册成功");

            return Result.Failed("数据异常，请重试");
        }

        [HttpGet]
        public IActionResult Forget()
        {
            return View();
        }
        [HttpPost]
        public Result Forget(string Code, Model.User user)
        {
            if (Code != HttpContext.Session.GetString("code"))
                return Result.Failed("验证码错误");

            var acc = Iuser.GetUser(0, user.Account, null);
            var eml = Iuser.GetUser(0, null, user.Email);
            if (acc == null || eml == null || acc.ID != eml.ID)
                return Result.Failed("邮箱与账号信息不匹配");

            if (Iuser.EditPassword(acc.ID, user.Password))
                return Result.Success("密码重置成功");
            else
                return Result.Failed("数据异常");
        }

        [HttpPost]
        public Result SendMail(string Email, Model.User user)
        {
            Random random = new Random();
            HttpContext.Session.SetString("code", Common.Security.MD5Encrypt32(random.Next(0, 9999).ToString()).Substring(random.Next(1, 16), 6).ToUpper());

            var acc = Iuser.GetUser(0, user.Account, null);
            var eml = Iuser.GetUser(0, null, user.Email);
            if (acc == null || eml == null || acc.ID != eml.ID)
                return Result.Failed("邮箱与账号信息不匹配");

            if (Icommon.SendMail(Email, "找回密码操作", $"尊敬的用户 { user.Account }：<br />您正在进行<span style='color:red;'>找回密码</span>操作！<br />本次操作的验证码是：<span style='color:red;'>{ HttpContext.Session.GetString("code") }</span>。<br />请注意谨防验证码泄露，保护账号安全！"))
                return Result.Success("邮件发送成功");
            else
                return Result.Failed("邮件发送失败");
        }
    }
}