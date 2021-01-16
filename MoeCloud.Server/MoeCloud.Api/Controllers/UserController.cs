using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace MoeCloud.Api.Controllers
{
    [ApiController]
    [Route("[Controller]/[Action]")]
    public class UserController : Controller
    {
        private ILogic.IUser Iuser { get; }
        private Handles.JwtHelper Jwt { get; }
        private ILogic.IRole Irole { get; }
        public UserController(ILogic.IUser iuser, Handles.JwtHelper jwt, ILogic.IRole irole)
        {
            Iuser = iuser;
            Jwt = jwt;
            Irole = irole;
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

        [AllowAnonymous]
        [HttpPost]
        public Result Sign([FromBody] Model.User user)
        {
            var mod = Iuser.Sign(user);
            if (mod == null)
                return Result.Failed("用户名或密码错误");

            var info = new
            {
                mod.ID,
                mod.Account,
                mod.Email
            };
            return Result.Success($"{ mod.Account },欢迎回来", Jwt.GetToken(info));
        }

        [AllowAnonymous]
        [HttpPost]
        public Result Register([FromBody] Model.User user)
        {
            if (Iuser.AddUser(user))
                return Result.Success($"用户{ user.Account }注册成功");
            else
                return Result.Failed("服务器返回异常");
        }

        //刷新用户已使用的存储
        [Authorize]
        [HttpPost]
        public Result UseSize([FromBody] int ID)
        {
            var mod = Iuser.GetUser(ID);
            if (mod == null)
                return Result.Failed("数据异常");

            return Result.Success("ok", new { mod.UseSize, Irole.GetRole(mod.RoleID).MaxSize });
        }

        [Authorize]
        [HttpPost]
        public Result Reset([FromBody] Model.User user)
        {
            if (user.Account == null || user.Email == null || user.Password == null)
                return Result.Failed("数据异常");

            var mod = Iuser.GetUser(0, user.Account, user.Email);
            if (mod.Email != user.Email || mod.Account != user.Account)
                return Result.Failed("数据异常");

            if (Iuser.RestPassword(mod.ID, user.Password))
                return Result.Success("密码重置成功");

            return Result.Failed("重置失败，请重试");
        }
    }
}