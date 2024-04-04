
using Microsoft.AspNetCore.Http;

namespace fault3r_Application.Services.ForumsRepository.Dtos
{
    public class AddForumDto
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public IFormFile Image { get; set; }

        public bool UseDefaultImage { get; set; }

        public string ParentId { get; set; }
    }
}
