using Furion;
using Furion.DatabaseAccessor;
using Furion.UnifyResult;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.WebSockets;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sys.Hub.EntityFramework.Core.DbContexts;
using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Net.WebSockets;

namespace Sys.Hub.Web.Core
{
    /// <summary>
    /// 启动项目
    /// </summary>
    public class Startup : AppStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            //开启跨域
            services.AddCorsAccessor();

            #region 日志配置
            //添加日志
            services.AddFileLogging("Logs/{0:yyyy}-{0:MM}-{0:dd}.log", options =>
            {
                options.FileNameRule = fileName =>
                {
                    return string.Format(fileName, DateTime.UtcNow);
                };
                options.FileSizeLimitBytes = 10485760;  //默认10M 分文件
                options.DateFormat = "yyyy-MM-dd HH:mm:ss.fffffff zzz dddd";
            });

            //添加全局请求、响应日志
            services.AddMvcFilter<LoggingMonitorAttribute>();
            #endregion

            services.AddControllersWithViews();
            services.AddInjectWithUnifyResult<RESTfulResultProvider>();
            services.AddViewEngine();
            services.AddRazorPages();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();

            #region WebSocket

            app.UseWebSockets();
            #endregion

            #region 注册Area路由
            app.UseEndpoints(endpoints =>
            {
                //供区域使用的路由
                endpoints.MapControllerRoute(
                    name: "areas",
                    pattern: "{area:exists}/{controller}/{action}/{id?}"
                    );
                //自定义路由系统endpoints.MapControllerRoute(); 有两个参数，第一个是路由名称，第二个是路由模板
                endpoints.MapControllerRoute(
                    //对MVC的路由，无区域的
                    name: "default",
                    pattern: "{controller=Home}/{action=ToLogin}/{id?}"
                    );

                //对Razor的路由
                endpoints.MapRazorPages();

            });
            #endregion

            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions
            {
                RequestPath = "/Content"
            });

            app.UseCorsAccessor();

            app.UseInject("swagger");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}