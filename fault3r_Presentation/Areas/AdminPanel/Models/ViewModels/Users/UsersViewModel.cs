using fault3r_Common;
using System.Collections.Generic;

namespace fault3r_Presentation.Areas.AdminPanel.Models.ViewModels.Users
{
    public class UsersViewModel
    {
        public List<UserViewModel> Users { get; set; } = new();

        public string Search { get; set; }

        public PaginationDto Pagination { get; set; }
    }
}
