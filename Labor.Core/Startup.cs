using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using Labor.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Labor.Core
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
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
