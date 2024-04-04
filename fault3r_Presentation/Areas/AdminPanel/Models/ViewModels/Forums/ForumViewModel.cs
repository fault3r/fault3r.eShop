using System.Collections.Generic;

namespace fault3r_Presentation.Areas.AdminPanel.Models.ViewModels.Forums
{
    public class ForumViewModel
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Image { get; set; }       

        public List<ForumViewModel> SubForums { get; set; }
    }
}
