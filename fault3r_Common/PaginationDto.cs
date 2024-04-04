

using System.Collections.Generic;

namespace fault3r_Common
{
    public class PaginationDto
    {
        public static int PageSize = 5;

        public List<int> Pages { get; set; } = new();

        public int PageCount { get; set; }

        public int CurrentPage { get; set; }

        public bool HasPrev { get; set; } = false;

        public bool HasMorePrev { get; set; } = false;

        public bool HasNext { get; set; } = false;

        public bool HasMoreNext { get; set; } = false;
    }
}
