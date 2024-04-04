using Microsoft.AspNetCore.Http;

namespace fault3r_Presentation.Models.ViewModels.Accounts
{
    public class SignUpViewModel
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public string RePassword { get; set; }

        public string Name { get; set; }        

        public bool Terms { get; set; }
    }
}
