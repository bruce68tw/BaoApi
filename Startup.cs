using BaoApi.Services;
using Base.Enums;
using Base.Models;
using Base.Services;
using BaseWeb.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
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
            services.AddControllers()
                //use pascal for newtonSoft json, need mvc.newtonSoft !!
                .AddNewtonsoftJson(opts => { opts.UseMemberCasing(); })
                //use pascal for MVC json
                .AddJsonOptions(opts => { opts.JsonSerializerOptions.PropertyNamingPolicy = null; });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "BaoApi", Version = "v1" });
            });

            //3.http context
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            //4.user info for base component
            services.AddSingleton<IBaseUserService, MyBaseUserService>();

            //5.ado.net for mssql
            services.AddTransient<DbConnection, SqlConnection>();
            services.AddTransient<DbCommand, SqlCommand>();

            //6.appSettings "FunConfig" section -> _Fun.Config
            var config = new ConfigDto();
            Configuration.GetSection("FunConfig").Bind(config);
            _Fun.Config = config;

            //7.session (memory cache)
            services.AddDistributedMemoryCache();
            //services.AddStackExchangeRedisCache(opts => { opts.Configuration = "127.0.0.1:6379"; });
            services.AddSession(opts =>
            {
                opts.Cookie.HttpOnly = true;
                opts.Cookie.IsEssential = true;
                opts.IdleTimeout = TimeSpan.FromMinutes(60);
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //initial & set locale
            _Fun.Init(env.IsDevelopment(), app.ApplicationServices, DbTypeEnum.MSSql, AuthTypeEnum.Data);
            _Locale.SetCulture(_Fun.Config.Locale);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BaoApi v1"));
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();

            //session
            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
