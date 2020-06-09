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

        // ��������̨sql log
        public static readonly ILoggerFactory MyLoggerFactory = LoggerFactory.Create(builder => { builder.AddConsole(); });

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddJsonOptions(options =>
            {
                //����JSON��ǰ�ˣ���֤��Сдƥ��
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
            });

            #region ���ݿ����ӣ���������Ĭ��Ϊscope����һ��http�����У�����Ϊtransient��ÿ��ע�붼�������ɣ�
            services.AddDbContext<LaborContext>(options =>
            {
                options.UseLoggerFactory(MyLoggerFactory);
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            },
            ServiceLifetime.Transient);

            #endregion

            #region cors��������
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
                    //.WithExposedHeaders("x-custom-header");//�Զ�������ͷ
                });
            });
            #endregion
        }

        //configureContainer����AutoFac�������������Զ�ע��service��repository
        //����ֶ�ע��
        //services.AddScoped<IUserService, UserService>();
        //services.AddScoped<IUserRepository, UserRepository>();
        public void ConfigureContainer(ContainerBuilder builder)
        {
            //��ȡ���򼯲�ע��,����ÿ�����󶼴���һ���µĶ����ģʽ
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
