using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Labor.Common;
using Labor.Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

namespace Labor.Core
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            JwtHelper.GetConfiguration(Configuration);
        }

        public IConfiguration Configuration { get; }

        // 开启控制台sql log
        public static readonly ILoggerFactory MyLoggerFactory = LoggerFactory.Create(builder => { builder.AddConsole(); });

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddJsonOptions(options =>
            {
                //返回JSON给前端，保证大小写匹配
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
            });

            #region 数据库连接，生命周期默认为scope（在一个http请求中），改为transient（每次注入都重新生成）
            services.AddDbContext<LaborContext>(options =>
            {
                options.UseLoggerFactory(MyLoggerFactory);
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            },
            ServiceLifetime.Transient);

            #endregion

            #region cors跨域配置
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
                    //.WithExposedHeaders("x-custom-header");//自定义请求头
                });
            });
            #endregion

            #region 注册Swagger服务
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "V1",
                    Title = "Labor API Doc-V1",
                    Description = "Labor API接口文档-V1版",
                    Contact = new OpenApiContact { Name = "sheng31", Email = "1445089856@qq.com" },
                    TermsOfService = new Uri("https://github.com/shengshengsheng31"),
                });
                //接口排序规则
                options.OrderActionsBy(x => x.RelativePath);
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "Labor.Core.xml"), true);
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "Labor.Model.xml"));

                #region Swagger中的Jwt配置
                options.OperationFilter<AddResponseHeadersFilter>();
                options.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();
                options.OperationFilter<SecurityRequirementsOperationFilter>();
                options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme()
                {
                    Description = "输入请求头中需要添加Jwt授权Token：Bearer Token",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                });
                #endregion
            });
            #endregion

            #region 注册Jwt服务
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,//验证秘钥
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JwtToken:secret"])),
                    ValidateIssuer = true,//验证发行人
                    ValidIssuer = Configuration["JwtToken:issuer"],
                    ValidateAudience = true,//验证订阅人
                    ValidAudience = Configuration["JwtToken:audience"],
                    RequireExpirationTime = true,//验证过期时间
                    ValidateLifetime = true,//验证生命周期
                    ClockSkew = TimeSpan.Zero//缓冲过期时间，及时配置了过期时间，也要考虑过期时间和缓冲
                };
            });
            #endregion
        }

        //configureContainer访问AutoFac容器生成器，自动注入service和repository
        //替代手动注入
        //services.AddScoped<IUserService, UserService>();
        //services.AddScoped<IUserRepository, UserRepository>();
        public void ConfigureContainer(ContainerBuilder builder)
        {
            //获取程序集并注册,采用每次请求都创建一个新的对象的模式
            var assemblyIServices = Assembly.LoadFrom(Path.Combine(AppContext.BaseDirectory, "Labor.Services.dll"));
            var assemblyIRepository = Assembly.LoadFrom(Path.Combine(AppContext.BaseDirectory, "Labor.Repository.dll"));
            builder.RegisterAssemblyTypes(assemblyIServices, assemblyIRepository).AsImplementedInterfaces().InstancePerDependency();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            #region 启用Swagger,正式环境时Swagger应该在开发环境，现在因为开放api所以不作处理
            app.UseSwagger();
            app.UseSwaggerUI(x =>
            {
                x.SwaggerEndpoint("/swagger/v1/swagger.json", "ApiHelp V1");
                x.RoutePrefix = "";//通过根域名直接访问
            });
            #endregion

            app.UseRouting();

            app.UseCors();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        
    }
}
