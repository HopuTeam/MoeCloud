using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace MoeCloud.Data
{
    public class InitData
    {
        public static void Send(IServiceProvider service)
        {
            using var context = new CoreEntities(service.GetRequiredService<DbContextOptions<CoreEntities>>());
            if (context.Users.ToList().Count() == 0)
            {
                context.Users.Add(new Model.User()
                {
                    Email = "admin@admin.com",
                    Account = "admin",
                    Password = "E1ADC3949BA59ABBE56E057F2F883E",//MD5(123456),
                    UseSize = 0,
                    RoleID = 1
                });
                context.Roles.Add(new Model.Role()
                {
                    ID = 3,
                    Name = "游客",
                    MaxSize = 0,
                    Status = false
                });
                context.Roles.Add(new Model.Role()
                {
                    ID = 2,
                    Name = "注册用户",
                    MaxSize = 8589934592,//1G，以B为单位
                    Status = true
                });
                context.Roles.Add(new Model.Role()
                {
                    ID = 1,
                    Name = "管理员",
                    MaxSize = 8589934592,//1G
                    Status = true
                });
                context.Sites.Add(new Model.Site()
                {
                    MainTitle = "MoeCloud",
                    SecTitle = "轻松上云",
                    Description = "MoeCloud是一个基于.Net5开发的轻量级企业云盘",
                    //Url = "",
                    //Script = "",
                });
                context.Regs.Add(new Model.Reg()
                {
                    AllowReg = true,
                    EmailActive = true,
                    RegCode = true,
                    SignCode = true,
                    DefaultRoleID = 2,
                });
                context.Mails.Add(new Model.Mail()
                {
                    Host = "smtp.ym.163.com",
                    Account = "work@echocode.club",
                    Password = "9qC3uNQJRy",
                    Port = 994,
                    From = "work@echocode.club",
                    SSL = true,
                    //ActiveModel = "",
                    //ResetModel = "",
                });
                context.SaveChanges();
            }
        }
    }
}