using EmployeeMangement.Models;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeMangement.Controllers
{
    public class ErrorController : Controller
    {
        [Route("Error/{StatusCode}")]
        public IActionResult Index(int StatusCode)
        {
            StatusResult model = new StatusResult();
            var statusCodeResult = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
            switch (StatusCode)
            {
                case 404:
                    {
                        model.Message = "Sorry, the ressource you requested could not be found";
                        model.Path = statusCodeResult.OriginalPath;
                        model.QS = statusCodeResult.OriginalQueryString;
                    }
                    break;
                default:
                    break;
            }
            return View("NotFound", model);
        }

        [Route("Error")]
        public ActionResult Error()
        {
            var execeptionStatus = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            ViewBag.Message = execeptionStatus.Error.Message;
            ViewBag.StackTrace = execeptionStatus.Error.StackTrace;
            ViewBag.Path = execeptionStatus.Path;

            return View();

        }
    }
}
