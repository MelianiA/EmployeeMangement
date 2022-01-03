using EmployeeMangement.Models;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EmployeeMangement.Controllers
{
    public class ErrorController : Controller
    {
        private readonly ILogger<ErrorController> _logger;

        public ErrorController(ILogger<ErrorController> logger)
        {
            this._logger = logger;
        }

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
                        _logger.LogWarning($"404 error occured.path = {statusCodeResult.OriginalPath}" +
                            $"and QueryString = {statusCodeResult.OriginalQueryString}");
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

            _logger.LogError($"The path {execeptionStatus.Path} throw an exeception {execeptionStatus.Error}");

            return View();

        }
    }
}
