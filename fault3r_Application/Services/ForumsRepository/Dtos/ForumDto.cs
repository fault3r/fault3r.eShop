

using System.Collections.Generic;

namespace fault3r_Application.Services.ForumsRepository.Dtos
{
    public class ForumDto
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Image { get; set; }

        public string ParentId { get; set; }

        public List<ForumDto> SubForums { get; set; }

    }
}
