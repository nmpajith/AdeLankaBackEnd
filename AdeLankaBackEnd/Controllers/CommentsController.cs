using AdeLankaBackEnd.Contracts;
using AdeLankaBackEnd.DataTransferObjects;
using AdeLankaBackEnd.Domain.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdeLankaBackEnd.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        IUnitOfWork _unitOfWork;
        private ILoggerManager _logger;
        private IMapper _mapper;
        UserManager<IdentityUser> _userManager;
        public CommentsController(
            ILoggerManager logger
            , IUnitOfWork unitOfWork
            , IMapper mapper,
            UserManager<IdentityUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult GetComments([FromQuery] PageParameters pageParameters)
        {
            if (User.Identity.IsAuthenticated)
            {
                _logger.LogInfo("User Name: " + User.Identity.Name);
                _logger.LogInfo("User Name From User Manager: " + User.Identity.IsAuthenticated);
            }
            try
            {
                var comments = _unitOfWork.CommentsRepository.Get(null, null, pageParameters, x => x.NoteUser, x => x.Note);
                _logger.LogInfo($"Returned all Comments from database.");
                var commentsResult = _mapper.Map<IEnumerable<CommentResponseDto>>(comments);
                return Ok(commentsResult);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetNotesPage action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        // POST api/<DocumentController>
        [HttpPost]
        public IActionResult CreateComment([FromBody] CommentCreateDto commentCreateDto)
        {
            if (commentCreateDto == null)
            {
                _logger.LogError("Comment object sent from client is null.");
                return BadRequest("Comment object is null");
            }
            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid Comment object sent from client.");
                return BadRequest("Invalid Comment object");
            }
            var currentUser = _userManager.FindByNameAsync(User.Identity.Name);

            _logger.LogInfo("Current User Id: " + currentUser.Result.Id);

            var notesUser = _unitOfWork.NoteUsersRepository.Get(
                user => 
                user.IdentityId.Equals(currentUser.Result.Id.ToString()))
                .FirstOrDefault();

            var note = _unitOfWork.NotesRepository.Get(
                note =>
                (note.NoteUserId==notesUser.Id)
                &&(DateTime.Compare(note.DateCreated,commentCreateDto.NoteCreatedDate)==0)
                &&(note.Title.Equals(commentCreateDto.NoteTitle)))
                .FirstOrDefault();

            if (notesUser != null && note!=null)
            {
                try
                {
                    var comment = new Comment
                    {
                        NoteId = note.Id,
                        Content = commentCreateDto.Content,
                        NoteUserId = notesUser.Id,
                        DateCommented = DateTime.Now
                    };
                    _unitOfWork.CommentsRepository.Insert(comment);
                    _unitOfWork.Save();
                    _logger.LogError($"Created new Comment");
                    return NoContent();
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Something went wrong inside CreateComment action: {ex.Message}");
                    return StatusCode(500, "Internal server error");
                }
            }
            else
            {
                _logger.LogError("Something went wrong. Notes User or Note Is Null");
                return StatusCode(500, "Invalid Notes User or Note");
            }
        }
    }
}
