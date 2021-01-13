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

namespace MoeCloud.Api
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
            services.AddControllers();
            // ���ý����ļ����ȵ����ֵ
            services.Configure<Microsoft.AspNetCore.Http.Features.FormOptions>(x =>
            {
                x.ValueLengthLimit = int.MaxValue;
                x.MultipartBodyLengthLimit = int.MaxValue;
                x.MultipartHeadersLengthLimit = int.MaxValue;
            });
            // ע��������
            services.AddCors(options =>
            {
                options.AddPolicy("Cors", Tion =>
                {
                    Tion.AllowAnyHeader();
                    Tion.AllowAnyOrigin();
                    Tion.AllowAnyMethod();
                });
            });
            // ���ݿ������ַ���
            services.AddDbContext<Data.CoreEntities>(options =>
            {
                options.UseMySQL(Configuration.GetConnectionString("EFDbConnection"));
            });
            // ע��ȫ�ֹ�����
            services.AddControllers(options =>
            {
                //options.Filters.Add<ErrorFilter>();
            });
            // ҵ���߼���ע��
            services.AddScoped<ILogic.ICommon, Logic.CommonLogic>();
            services.AddScoped<ILogic.IUser, Logic.UserLogic>();
            services.AddScoped<ILogic.IRole, Logic.RoleLogic>();
            services.AddScoped<ILogic.ISite, Logic.SiteLogic>();
            services.AddScoped<ILogic.IReg, Logic.RegLogic>();
            services.AddScoped<ILogic.IMail, Logic.MailLogic>();
            services.AddScoped<ILogic.IShare, Logic.ShareLogic>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();
            app.UseCors("Cors");
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}