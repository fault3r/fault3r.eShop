using Microsoft.AspNetCore.Http;

namespace fault3r_Presentation.Models.ViewModels.Account
{
    public class UpdateAccountViewModel
    {
        public string Name { get; set; }

        public IFormFile Picture { get; set; }

        public string Bio { get; set; }
    }
}
