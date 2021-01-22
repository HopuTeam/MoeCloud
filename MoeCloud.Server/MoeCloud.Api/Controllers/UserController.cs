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

        // 用户登录
        [AllowAnonymous]
        [HttpPost]
        public Result Sign([FromForm] Model.User user)
        {
            var mod = Iuser.Sign(user);
            if (mod == null)
                return Result.Failed("用户名或密码错误");

            return Result.Success($"{ mod.Account },欢迎回来", Jwt.GetToken(new { mod.ID }));
        }

        // 用户注册
        [AllowAnonymous]
        [HttpPost]
        public Result Register([FromForm] Model.User user)
        {
            if (user.Account == null || user.Email == null || user.Password == null)
                return Result.Failed("数据异常，请重试");

            user.Active = false;
            user.Status = true;
            user.RoleID = 2;
            user.EntryTime = DateTime.Now;
            //user.UseSize = 0;

            if (Iuser.AddUser(user))
                return Result.Success("注册成功");
            else
                return Result.Failed("服务器返回异常");
        }

        //刷新用户已使用的存储
        [Authorize]
        [HttpPost]
        public Result UseSize([FromForm] int ID)
        {
            var mod = Iuser.GetUser(ID);
            if (mod == null)
                return Result.Failed("数据异常");

            return Result.Success("ok", new { mod.UseSize, Irole.GetRole(mod.RoleID).MaxSize });
        }

        // 密码重置接口
        [Authorize]
        [HttpPost]
        public Result Reset([FromForm] Model.User user)
        {
            // 判断是否传数据
            if (user.Account == null || user.Email == null || user.Password == null)
                return Result.Failed("数据异常");

            // 判断用户传输数据是否相同
            var mod = Iuser.GetUser(0, user.Account, user.Email);
            if (mod.Email != user.Email || mod.Account != user.Account)
                return Result.Failed("用户数据信息异常");

            if (Iuser.RestPassword(mod.ID, user.Password))
                return Result.Success("密码重置成功");

            // 抛出异常，记录日志表LogInfo
            return Result.Failed("重置失败，请重试");
        }

        // 获取个人信息(用户端)
        [Authorize]
        [HttpPost]
        public Result GetInfo([FromForm] int ID)
        {
            var user = Iuser.GetUser(ID);
            if (user == null)
                return Result.Failed("数据获取异常");

            return Result.Success("ok", new { user.Account, user.Active, user.Email, user.EntryTime, Irole.GetRole(user.RoleID).Name });
        }
        // 获取用户信息(管理员端
        [Authorize]
        [HttpPost]
        public Result GetUsersList([FromForm] int ID)
        {
            if (Iuser.GetUser(ID).RoleID == 1)
                return Result.Success("ok", Iuser.GetUsers());
            else
                return Result.Failed("权限不足");
        }

        // 修改个人信息(用户端
        [Authorize]
        [HttpPost]
        public Result EditInfo([FromForm] Model.User user)
        {
            if (Iuser.EditUser(user))
                return Result.Success("ok");

            return Result.Failed("数据异常");
        }
        // 修改用户信息(管理员端
        [Authorize]
        [HttpPost]
        public Result EditUserInfo([FromForm] int ID, Model.User user)
        {
            if (Iuser.GetUser(ID).RoleID == 1)
            {
                if (Iuser.EditUser(user))
                    return Result.Success("ok");

                return Result.Failed("数据异常");
            }
            else
            {
                return Result.Failed("权限不足");
            }
        }

        /// <summary>
        /// 修改密码(仅供用户使用
        /// </summary>
        /// <param name="auth">true为密码验证，false为邮箱验证码验证</param>
        /// <param name="user"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public Result EditPass([FromForm] bool auth, Model.User user)
        {
            if (auth)
            {
                // 通过密码验证来修改密码
                if (Iuser.GetUser(user.ID).Password != Common.Security.MD5Encrypt32(user.Password))
                    return Result.Failed("原密码不正确");
            }
            else
            {
                // 通过邮箱验证码来修改密码
                return Result.Failed("暂未开放");
            }

            if (Iuser.EditPassword(user.ID, user.Password))
                return Result.Failed("密码修改成功");

            return Result.Failed("数据异常");
        }
    }
}