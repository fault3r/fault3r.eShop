
using fault3r_Application.Interfaces;
using fault3r_Application.Services.ForumsRepository.Dtos;
using fault3r_Common;
using fault3r_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace fault3r_Application.Services.ForumsRepository
{
    public class ForumsRepository : IForumsRepository
    {
        private readonly IDatabaseContext _databaseContext;

        public ForumsRepository(IDatabaseContext databaseContext)
        {
            _databaseContext=databaseContext;
        }

        public List<ForumDto> GetForums(string id)
        {   
            var forums = _databaseContext.Forums.AsQueryable()
                .Where(p => p.ParentForumId.ToString() == id)
                .Select(r => new ForumDto
                {
                    Id = r.Id.ToString(),
                    Title = r.Title,
                    Description = r.Description,
                    Image = Convert.ToBase64String(r.Image),
                    ParentId = r.ParentForumId.ToString(),
                    SubForums = _databaseContext.Forums.Where(p => p.ParentForumId.ToString() == r.Id.ToString()).Select(rc => new ForumDto
                    {
                        Id = rc.Id.ToString(),
                        Title = rc.Title,
                        Description = rc.Description,
                        Image = Convert.ToBase64String(rc.Image),
                        ParentId = rc.ParentForumId.ToString(),
                        SubForums = null,
                    }).ToList(),
                })
                .ToList();
            return forums;
        }

        public List<SelectListItemDto> GetForumsList()
        {
            return _databaseContext.Forums.AsQueryable()
                .Where(p => p.ParentForumId == null)
                .Select(r => new SelectListItemDto
                {
                    Value = r.Id.ToString(),
                    Label = r.Title,
                })
                .ToList();
        }

        public ForumDto GetForum(string id)
        {
            return _databaseContext.Forums.Where(p => p.Id.ToString() == id)
                .Select(r => new ForumDto
                {
                    Id = r.Id.ToString(),
                    Title = r.Title,
                    Description = r.Description,
                    Image = Convert.ToBase64String(r.Image),
                    ParentId = r.ParentForumId.ToString(),
                    SubForums = _databaseContext.Forums.Where(p => p.ParentForumId.ToString() == r.Id.ToString()).Select(rc => new ForumDto
                    {
                        Id = rc.Id.ToString(),
                        Title = rc.Title,
                        Description = rc.Description,
                        Image = Convert.ToBase64String(rc.Image),
                        ParentId = rc.ParentForumId.ToString(),
                        SubForums = null,
                    }).ToList(),
                })
                .FirstOrDefault();
        }

        public ForumsRepositoryResult AddForum(AddForumDto forum)
        {
            var tForum = new Forum
            {
                Title = forum.Title,
                Description = forum.Description,
                Image = forum.UseDefaultImage == true ? ResourceMemorizer.DefaultForumPicture.ToArray() : ImageResizer.ResizeImage(forum.Image, 200, 200).ToArray(),
                ParentForumId = forum.ParentId == "null" ? null : Guid.Parse(forum.ParentId),
            };
            _databaseContext.Forums.Add(tForum);
            _databaseContext.SaveChanges();
            return new ForumsRepositoryResult { Success = true, Message = "انجمن با موفقیت افزوده شد." };
        }

        public ForumsRepositoryResult EditForum(EditForumDto forum)
        {
            var tForum = _databaseContext.Forums.Where(p => p.Id.ToString() == forum.Id)
                .FirstOrDefault();
            tForum.Title = forum.Title;
            tForum.Description = forum.Description;
            if (forum.Image != null)
                tForum.Image = ImageResizer.ResizeImage(forum.Image, 200, 200).ToArray();
            if (forum.ParentId == "null")
                tForum.ParentForumId = null;
            else
                tForum.ParentForumId = Guid.Parse(forum.ParentId);
            _databaseContext.Forums.Update(tForum);
            _databaseContext.SaveChanges();
            return new ForumsRepositoryResult { Success = true, Message = "انجمن با موفقیت ویرایش شد." };
        }

        public ForumsRepositoryResult DeleteForum(string id)
        {
            try
            {
                var forum = _databaseContext.Forums.Where(p => p.Id.ToString() == id)
                    .FirstOrDefault();
                _databaseContext.Forums.Remove(forum);
                _databaseContext.SaveChanges();
            }
            catch(Exception e)
            {
                string message = string.Empty;
                if (e.InnerException != null)
                {
                    if (e.InnerException.Message.Contains("statement conflicted"))
                        message = "این انجمن دارای زیرانجمن می باشد!";
                    else
                        message = "خطا!";
                }
                else
                    message = "خطا!";
                return new ForumsRepositoryResult { Success = false, Message = message };
            }
            return new ForumsRepositoryResult { Success = true, Message = "انجمن با موفقیت حذف شد." };
        }

    }


   


}
