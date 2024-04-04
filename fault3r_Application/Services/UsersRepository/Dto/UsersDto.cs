
using fault3r_Common;
using System.Collections.Generic;

namespace fault3r_Application.Services.UsersRepository.Dto
{
    public class UsersDto
    {
        public List<UserDto> Users { get; set; }

        public PaginationDto Pagination { get; set; }
    }
}
