using Microsoft.AspNetCore.Mvc;

namespace fault3r_Presentation.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]    
    public class HomeController : Controller
    {
        public IActionResult Index()
        {

            return View();
        }  
    }
}
