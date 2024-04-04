using fault3r_Presentation.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace fault3r_Presentation.Controllers
{
    [Route("[controller]")]
    public class ErrorController : Controller
    {
        [Route("{code}")]
        [HttpGet]
        public IActionResult Index(int code)
        {
            StatusCodeViewModel error = new StatusCodeViewModel();
            switch (code)
            {
                case StatusCodes.Status404NotFound:
                    error = new StatusCodeViewModel { Code = StatusCodes.Status404NotFound, Message = "صفحه مورد نظر پیدا نشد!" };
                    break;
                case StatusCodes.Status403Forbidden:
                    error = new StatusCodeViewModel { Code = StatusCodes.Status403Forbidden, Message = "شما مجوز لازم برای دیدن این صفحه را ندارید!" }; 
                    break;
                default:
                    error = new StatusCodeViewModel { Code = 0, Message = "خطا!" };
                    break;
            }           
            return View("Index", error);
        }
    }
}
