using fault3r_Application.Services.ForumsRepository;
using fault3r_Presentation.Areas.AdminPanel.Models.ViewModels.Forums;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace fault3r_Presentation.Controllers
{
    [Route("[controller]")]
    public class ForumsController : Controller
    {
        private readonly IForumsRepository _forumsRepository;

        public ForumsController(IForumsRepository forumsRepository)
        {
            _forumsRepository=forumsRepository;
        }

        [Route("Index")]
        [HttpGet]
        public IActionResult Index()
        {
            var tForums = _forumsRepository.GetForums(null);
            List<ForumViewModel> forums = new();
            foreach(var forum in tForums)
            {
                List<ForumViewModel> subs = new();
                foreach(var sub in forum.SubForums)
                {
                    subs.Add(new ForumViewModel
                    {
                        Id = sub.Id,
                        Title = sub.Title,
                        Description = sub.Description,
                        Image = sub.Image,
                        SubForums = null,
                    });
                }
                forums.Add(new ForumViewModel
                {
                    Id = forum.Id,
                    Title = forum.Title,
                    Description = forum.Description,
                    Image = forum.Image,
                    SubForums = subs,
                }) ;
            }
            return View("Index", forums);
        }
    }
}
