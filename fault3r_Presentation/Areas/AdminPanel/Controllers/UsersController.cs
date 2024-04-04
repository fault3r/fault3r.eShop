using fault3r_Application.Services.UsersRepository;
using fault3r_Presentation.Areas.AdminPanel.Models.ViewModels.Users;
using Microsoft.AspNetCore.Mvc;

namespace fault3r_Presentation.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    [Route("[area]/[controller]")]
    public class UsersController : Controller
    {
        private readonly IUsersRepository _usersRepository;

        public UsersController(IUsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }

        [Route("Index")]
        [HttpGet]
        public IActionResult Index([FromQuery] string search = "", int page = 1)
        {
            search = search == null ? "" : search;                
            var tUsers = _usersRepository.GetUsers(search, page);
            UsersViewModel users = new();
            foreach (var user in tUsers.Users)
            {
                users.Users.Add(new UserViewModel
                {
                    Email = user.Email,
                    Role = user.Role,
                    Name = user.Name,
                    Picture = user.Picture,
                    Rank = user.Rank,
                    IsActive = user.IsActive,
                });
            }
            users.Search = search;
            users.Pagination = tUsers.Pagination;
            return View(users);
        }                  
    }
}
