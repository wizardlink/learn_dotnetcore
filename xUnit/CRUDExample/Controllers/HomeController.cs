using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace CRUDExample.Controllers
{
    public class HomeController : Controller
    {
        [Route("Error")]
        public ActionResult Index()
        {
            var exceptionPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            if (exceptionPathFeature != null)
                ViewBag.ErrorMessage = exceptionPathFeature.Error.Message;

            return View();
        }
    }
}
