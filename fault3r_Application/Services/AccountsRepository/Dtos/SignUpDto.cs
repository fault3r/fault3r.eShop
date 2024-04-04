

using Microsoft.AspNetCore.Http;

namespace fault3r_Application.Services.AccountsRepository.Dtos
{
    public class SignUpDto
    {
        public string Email { get; set; } 

        public string Password { get; set; }

        public string Name { get; set; }          
    }
}
