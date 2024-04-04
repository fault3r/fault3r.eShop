
using Microsoft.AspNetCore.Http;

namespace fault3r_Application.Services.ForumsRepository.Dtos
{
    public class EditForumDto
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public IFormFile Image { get; set; }

        public string ParentId { get; set; }
    }
}
