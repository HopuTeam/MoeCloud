using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace MoeCloud.Api.Handles
{
    public class JwtHelper
    {
        public JwtHelper(IOptions<JwtConfig> config)
        {
            this.config = config;
        }

        public IOptions<JwtConfig> config;

        /// <summary>
        /// 生成JwtToken
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        public string GetToken<Entity>(Entity model)
        {
            var claims = new List<Claim>();
            var type = model.GetType();
            var props = type.GetProperties();
            foreach (var item in props)
            {
                var val = item.GetValue(model);
                claims.Add(new Claim(item.Name, val is null ? "" : val.ToString()));
            }

            var jwt = new JwtSecurityToken(
                    issuer: config.Value.Issuer,
                    audience: config.Value.Audience,
                    claims: claims,
                    expires: config.Value.Expires,
                    notBefore: config.Value.NotBefor,
                    signingCredentials: config.Value.Credentials
                );
            string token = new JwtSecurityTokenHandler().WriteToken(jwt);
            return token;
        }
        /// <summary>
        /// 注册Jwt服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void AddJwtServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped(typeof(JwtHelper));
            services.Configure<JwtConfig>(configuration.GetSection("JwtConfig"));
            var jwtConfig = configuration.GetSection("JwtConfig").Get<JwtConfig>();
            services.AddAuthentication(option =>
            {
                // 配置验证方案
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(config =>
            {
                // 令牌验证参数
                config.TokenValidationParameters = new TokenValidationParameters()
                {
                    // 是否验证颁发者
                    ValidateIssuer = true,
                    // 是否验证接收者
                    ValidateAudience = true,
                    // 是否验证数字证书
                    ValidateIssuerSigningKey = true,
                    // 是否验证Token有效期
                    ValidateLifetime = true,
                    // 验证的接收者
                    ValidAudience = jwtConfig.Audience,
                    // 验证的颁发者
                    ValidIssuer = jwtConfig.Issuer,
                    // 数字证书
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.Secretkey))
                };
            });
        }
        /// <summary>
        /// 注册中间件
        /// </summary>
        /// <param name="app"></param>
        public static void AddMiddleware(IApplicationBuilder app)
        {
            app.UseAuthentication();
            app.UseAuthorization();
        }
    }

    public class JwtConfig : IOptions<JwtConfig>
    {
        public JwtConfig Value => this;
        /// <summary>
        /// 密钥
        /// </summary>
        public string Secretkey { get; set; }
        /// <summary>
        /// 颁发者
        /// </summary>
        public string Issuer { get; set; }
        /// <summary>
        /// 接收者
        /// </summary>
        public string Audience { get; set; }
        /// <summary>
        /// 过期时间（单位分钟）
        /// </summary>
        public int Expired { get; set; }
        /// <summary>
        /// 不早于此时间
        /// </summary>
        public DateTime NotBefor => DateTime.Now;
        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime Expires => NotBefor.AddMinutes(Expired);
        /// <summary>
        /// 数字签名的加密密钥
        /// </summary>
        private SymmetricSecurityKey Key => new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Secretkey));
        /// <summary>
        /// 签名证书
        /// </summary>
        public SigningCredentials Credentials => new SigningCredentials(Key, SecurityAlgorithms.HmacSha256);
    }
}

public static class ResquestExtend
{
    /// <summary>
    /// 从请求中解析信息
    /// </summary>
    /// <typeparam name="Entity"></typeparam>
    /// <param name="request"></param>
    /// <returns></returns>
    public static Entity GetModel<Entity>(this HttpRequest request) where Entity : new()
    {
        var authorization = request.Headers["Authorization"].ToString();
        var auths = authorization.Split(" ")[1];
        var jwtArr = auths.Split('.');
        var dic = JsonConvert.DeserializeObject<Dictionary<string, string>>(Base64UrlEncoder.Decode(jwtArr[1]));
        var model = new Entity();
        var type = model.GetType();
        var props = type.GetProperties();
        foreach (var item in props)
        {
            var val = dic[item.Name];

            switch (item.PropertyType.Name)
            {
                case "Int32":
                    item.SetValue(model, Convert.ToInt32(val));
                    break;
                case "DateTime":
                    item.SetValue(model, Convert.ToDateTime(val));
                    break;
                case "Boolean":
                    item.SetValue(model, Convert.ToBoolean(val));
                    break;
                case "Decimal":
                    item.SetValue(model, Convert.ToDecimal(val));
                    break;
                case "Double":
                    item.SetValue(model, Convert.ToDouble(val));
                    break;
                case "Float":
                    item.SetValue(model, Convert.ToSingle(val));
                    break;
                default:
                    item.SetValue(model, val);
                    break;
            }
        }
        return model;
    }
}