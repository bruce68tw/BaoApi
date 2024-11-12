using BaoApi.Models;
using BaoApi.Services;
using Base.Enums;
using Base.Interfaces;
using Base.Models;
using Base.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Data.Common;
using System.Data.SqlClient;

namespace BaoApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //1.JSON控制
            services.AddControllers()
                //use pascal for newtonSoft json, need mvc.newtonSoft !!
                .AddNewtonsoftJson(opts => { opts.UseMemberCasing(); })
                //use pascal for MVC json
                .AddJsonOptions(opts => { opts.JsonSerializerOptions.PropertyNamingPolicy = null; });

            //2.Swagger API文件
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "BaoApi", Version = "v1" });
            });

            //3.http context
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            //4.user info for base component
            services.AddSingleton<IBaseUserSvc, MyBaseUserService>();

            //5.ado.net for mssql
            services.AddTransient<DbConnection, SqlConnection>();
            services.AddTransient<DbCommand, SqlCommand>();

            //6.appSettings "FunConfig" section -> _Fun.Config
            var config = new ConfigDto();
            Configuration.GetSection("FunConfig").Bind(config);
            _Fun.Config = config;

            //7.appSettings "XpConfig" section -> _Xp.Config
            var xpConfig = new XpConfigDto();
            Configuration.GetSection("XpConfig").Bind(xpConfig);
            _Xp.Config = xpConfig;

            //jwt驗證
            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opts => {
                    opts.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateLifetime = true,  //是否驗證超時  當設置exp和nbf時有效 
                        ValidateIssuerSigningKey = true,  //是否驗證密鑰
                        IssuerSigningKey = _Xp.GetJwtKey(),     //SecurityKey

                        //ValidateIssuer = false,
                        //ValidateAudience = false,
                        //ValidAudience = "http://localhost:49999",//Audience
                        //ValidIssuer = "http://localhost:49998",//Issuer，這兩項和登入時頒發的一致
                        //緩衝過期時間，總的有效時間等於這個時間加上jwt的過期時間，預設為5分鐘                                                                                                            //注意這是緩衝過期時間，總的有效時間等於這個時間加上jwt的過期時間，如果不配置，默認是5分鐘
                        //ClockSkew = TimeSpan.FromMinutes(60)   //設置過期時間
                    };
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //1.initial & set locale
            _Fun.Init(env.IsDevelopment(), app.ApplicationServices, DbTypeEnum.MSSql);
            //_Locale.SetCultureAsync(_Fun.Config.Locale);

            //2.global exception handler
            app.UseExceptionHandler("/Home/Error");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BaoApi v1"));
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            //3.Authorize
            app.UseAuthentication();    //認証
            app.UseAuthorization();     //授權

            //session
            //app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            /*
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
            */
        }
    }
}
