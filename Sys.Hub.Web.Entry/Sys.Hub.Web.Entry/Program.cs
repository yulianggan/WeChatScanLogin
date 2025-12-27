using Furion.SpecificationDocument;
using IGeekFan.AspNetCore.Knife4jUI;
using Sys.Hub.Core.Util;

var builder = WebApplication.CreateBuilder(args);
//读取配置文件
builder.Configuration.SetBasePath(LinuxUtil.GetRuntimeDirectory(Directory.GetCurrentDirectory() + "/Configs"))
       .AddJsonFile("SysConfig.json", optional: true, reloadOnChange: true);
builder.Inject();

var app = builder.Build();

// 自定义Swagger 风格
app.UseKnife4UI(options =>
{
    options.RoutePrefix = "newapi";  // 配置 Knife4UI 路由地址，现在是 /newapi
    foreach (var groupInfo in SpecificationDocumentBuilder.GetOpenApiGroups())
    {
        options.SwaggerEndpoint("/" + groupInfo.RouteTemplate, groupInfo.Title);
    }
});
app.UseInject();

app.Run();