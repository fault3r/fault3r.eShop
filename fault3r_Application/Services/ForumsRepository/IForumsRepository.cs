
using fault3r_Application.Services.ForumsRepository.Dtos;
using fault3r_Common;
using System.Collections.Generic;

namespace fault3r_Application.Services.ForumsRepository
{
    public interface IForumsRepository
    {
        List<ForumDto> GetForums(string id);

        List<SelectListItemDto> GetForumsList();

        ForumDto GetForum(string id);

        ForumsRepositoryResult AddForum(AddForumDto forum);

        ForumsRepositoryResult EditForum(EditForumDto forum);

        ForumsRepositoryResult DeleteForum(string id);
    }
}
