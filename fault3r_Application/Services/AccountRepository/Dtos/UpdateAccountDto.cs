

using Microsoft.AspNetCore.Http;

namespace fault3r_Application.Services.AccountRepository.Dtos
{
    public class UpdateAccountDto
    {
        public string Name { get; set; }

        public IFormFile Picture { get; set; }

        public string Bio { get; set; }
    }
}
