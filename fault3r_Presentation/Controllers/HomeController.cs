using fault3r_Application.Interfaces;
using fault3r_Domain.Entities;
using fault3r_Presentation.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace fault3r_Presentation.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDatabaseContext _databaseContext;
        private readonly ILogger<HomeController> _logger;

        public HomeController(IDatabaseContext databaseContext, ILogger<HomeController> logger)
        {
            _databaseContext = databaseContext;
            _logger = logger;
        }

        public IActionResult Index()
        {
       
            return View();
        }     

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
