using Microsoft.AspNetCore.Mvc;

namespace Sys.Hub.Web.Entry.Areas.ScanDemo.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Area("PangTaiYi")] //指定区域，括号里是区域名
    [Route("PangTaiYi/[controller]/[action]")] //指定路由
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
