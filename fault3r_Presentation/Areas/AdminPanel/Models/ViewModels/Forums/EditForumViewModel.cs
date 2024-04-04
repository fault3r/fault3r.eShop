using Microsoft.AspNetCore.Http;

namespace fault3r_Presentation.Areas.AdminPanel.Models.ViewModels.Forums
{
    public class EditForumViewModel
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public IFormFile Image { get; set; }  

        public string ParentId { get; set; }
    }
}
