using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoeCloud.Web
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options =>
            {
                options.EnableEndpointRouting = false;
            });
            //JwT
          
            //设置接收文件长度的最大值。
            services.Configure<Microsoft.AspNetCore.Http.Features.FormOptions>(x =>
            {
                x.ValueLengthLimit = int.MaxValue;
                x.MultipartBodyLengthLimit = int.MaxValue;
                x.MultipartHeadersLengthLimit = int.MaxValue;
            });
            // 数据库连接字符串注入
            services.AddDbContext<Data.CoreEntities>(options =>
            {
                options.UseMySQL(Configuration.GetConnectionString("EFDbConnection"));
            });
            // BLL注入
            services.AddScoped<ILogic.ICommon, Logic.CommonLogic>();
            services.AddScoped<ILogic.IUser, Logic.UserLogic>();
            services.AddScoped<ILogic.IRole, Logic.RoleLogic>();
            services.AddScoped<ILogic.ISite, Logic.SiteLogic>();
            services.AddScoped<ILogic.IReg, Logic.RegLogic>();
            services.AddScoped<ILogic.IFile, Logic.FileLogic>();
            services.AddScoped<ILogic.IShare, Logic.ShareLogic>();
            services.AddScoped<ILogic.IUser, Logic.UserLogic>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();         
            app.UseStaticFiles(new StaticFileOptions
            {
                //设置不限制content-type
                ServeUnknownFileTypes = true
            }) ;
            app.UseMvc(options =>
            {
                options.MapRoute("Default", "{Controller=Home}/{Action=Index}");
            });
        }
    }
}
