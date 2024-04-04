using fault3r_Application.Services.ForumsRepository;
using fault3r_Application.Services.ForumsRepository.Dtos;
using fault3r_Common;
using fault3r_Presentation.Areas.AdminPanel.Models.ViewModels.Forums;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace fault3r_Presentation.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    [Route("[area]/[controller]")]
    public class ForumsController : Controller
    {
        private readonly IForumsRepository _forumsRepository;
        private readonly IValidator<AddForumViewModel> _addForumValidator;
        private readonly IValidator<EditForumViewModel> _editForumValidator;
        private readonly IValidator<DeleteForumViewModel> _deleteForumValidator;

        public ForumsController(IForumsRepository forumsRepository,
            IValidator<AddForumViewModel> addForumValidator, IValidator<EditForumViewModel> editForumValidator, IValidator<DeleteForumViewModel> deleteForumValidator)
        {
            _forumsRepository=forumsRepository; ;
            _addForumValidator = addForumValidator;
            _editForumValidator=editForumValidator;
            _deleteForumValidator=deleteForumValidator;
        }

        [Route("Index")]
        [HttpGet]
        public IActionResult Index([FromQuery]string id)
        {
            List<ForumViewModel> forums = new();
            var tForums = _forumsRepository.GetForums(id);
            foreach(var forum in tForums)
            {
                List<ForumViewModel> subs = new();
                foreach(var sub in forum.SubForums)
                {
                    subs.Add(new ForumViewModel
                    {
                        Id = sub.Id,
                        Title = sub.Title,
                        Description = sub.Description,
                        Image = sub.Image,
                        SubForums = null,
                    });
                }
                forums.Add(new ForumViewModel
                {
                    Id = forum.Id,
                    Title = forum.Title,
                    Description = forum.Description,
                    Image = forum.Image,
                    SubForums = subs,
                });
            }
            return View("Index",forums);
        }

        [Route("Add")]
        [HttpGet]
        public IActionResult Add([FromQuery]string id)
        {
            AddForumViewModel selected = new();
            selected.ParentId = id;
            ViewBag.ParentsList = new SelectList(_forumsRepository.GetForumsList(), "Value", "Label");
            return View("Add", selected);
        }

        [Route("Add")]
        [HttpPost]
        public IActionResult Add([FromForm]AddForumViewModel forum)
        {
            var vResult = _addForumValidator.Validate(forum);
            if (vResult.IsValid)
            {
                var result = _forumsRepository.AddForum(new AddForumDto
                {
                    Title = forum.Title,
                    Description = forum.Description,
                    Image = forum.Image,
                    UseDefaultImage = forum.UseDefaultImage,
                    ParentId = forum.ParentId,
                });
                ModelState.AddModelError("Add", result.Message);
            }
            else
                ModelState.AddFluentResult(vResult);
            ViewBag.ParentsList = new SelectList(_forumsRepository.GetForumsList(), "Value", "Label");
            return View("Add", forum);
        }

        [Route("Edit")]
        [HttpGet]
        public IActionResult Edit([FromQuery]string id)
        {
            var tForum = _forumsRepository.GetForum(id);
            EditForumViewModel forum = new EditForumViewModel
            {
                Id = tForum.Id,
                Title = tForum.Title,
                Description = tForum.Description,
                Image = null,
                ParentId = tForum.ParentId,
            };
            if(forum.ParentId!=null)
                ViewBag.ParentsList = new SelectList(_forumsRepository.GetForumsList(), "Value", "Label");            
            return View("Edit",forum);
        }

        [Route("Edit")]
        [HttpPost]
        public IActionResult Edit([FromForm]EditForumViewModel forum)
        {
            var vResult = _editForumValidator.Validate(forum);
            if (vResult.IsValid)
            {
                var result = _forumsRepository.EditForum(new EditForumDto
                {
                    Id = forum.Id,
                    Title = forum.Title,
                    Description = forum.Description,
                    Image = forum.Image,
                    ParentId = forum.ParentId,
                });
                ModelState.AddModelError("Edit", result.Message);
            }
            else
                ModelState.AddFluentResult(vResult);
            if (forum.ParentId != "null")
                ViewBag.ParentsList = new SelectList(_forumsRepository.GetForumsList(), "Value", "Label");
            return View("Edit",forum);
        }

        [Route("Delete")]
        [HttpGet]
        public IActionResult Delete([FromQuery]string id)
        {
            var tForum = _forumsRepository.GetForum(id);
            DeleteForumViewModel forum = new DeleteForumViewModel
            {
                Id = tForum.Id,           
                Confirm = false,
            };            
            return View("Delete", forum);
        }

        [Route("Delete")]
        [HttpPost]
        public IActionResult Delete([FromForm] DeleteForumViewModel forum)
        {
            var vResult = _deleteForumValidator.Validate(forum);
            if (vResult.IsValid)
            {
                var result = _forumsRepository.DeleteForum(forum.Id);
                ModelState.AddModelError("Delete", result.Message);
            }
            else
                ModelState.AddFluentResult(vResult);
            return View("Delete", forum);
        }
    }
}
