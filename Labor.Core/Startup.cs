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

            #region ע��Swagger����
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "V1",
                    Title = "Labor API Doc-V1",
                    Description = "Labor API�ӿ��ĵ�-V1��",
                    Contact = new OpenApiContact { Name = "sheng31", Email = "1445089856@qq.com" },
                    TermsOfService = new Uri("https://github.com/shengshengsheng31"),
                });
                //�ӿ��������
                options.OrderActionsBy(x => x.RelativePath);
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "Labor.Core.xml"), true);
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "Labor.Model.xml"));

                #region Swagger�е�Jwt����
                options.OperationFilter<AddResponseHeadersFilter>();
                options.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();
                options.OperationFilter<SecurityRequirementsOperationFilter>();
                options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme()
                {
                    Description = "��������ͷ����Ҫ���Jwt��ȨToken��Bearer Token",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                });
                #endregion
            });
            #endregion

            #region ע��Jwt����
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,//��֤��Կ
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JwtToken:secret"])),
                    ValidateIssuer = true,//��֤������
                    ValidIssuer = Configuration["JwtToken:issuer"],
                    ValidateAudience = true,//��֤������
                    ValidAudience = Configuration["JwtToken:audience"],
                    RequireExpirationTime = true,//��֤����ʱ��
                    ValidateLifetime = true,//��֤��������
                    ClockSkew = TimeSpan.Zero//�������ʱ�䣬��ʱ�����˹���ʱ�䣬ҲҪ���ǹ���ʱ��ͻ���
                };
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

            #region ����Swagger,��ʽ����ʱSwaggerӦ���ڿ���������������Ϊ����api���Բ�������
            app.UseSwagger();
            app.UseSwaggerUI(x =>
            {
                x.SwaggerEndpoint("/swagger/v1/swagger.json", "ApiHelp V1");
                x.RoutePrefix = "";//ͨ��������ֱ�ӷ���
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
