using AdeLankaBackEnd.Contracts;
using AdeLankaBackEnd.DataTransferObjects;
using AdeLankaBackEnd.Domain.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdeLankaBackEnd.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")] // api/authmanagement
    [ApiController]
    public class NotesController : ControllerBase
    {
        IUnitOfWork _unitOfWork;
        private ILoggerManager _logger;
        private IMapper _mapper;
        UserManager<IdentityUser> _userManager;
        public NotesController(
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
        public IActionResult GetNotes([FromQuery] PageParameters pageParameters)
        {
            if (User.Identity.IsAuthenticated)
            {
                _logger.LogInfo("User Name: " + User.Identity.Name);
                _logger.LogInfo("User Name From User Manager: " + User.Identity.IsAuthenticated);
            }
            try
            {
                var notes = _unitOfWork.NotesRepository.Get(null,null, pageParameters, x => x.NoteUser, x => x.Comments);
                _logger.LogInfo($"Returned all Notes from database.");
                var notesResult = _mapper.Map<IEnumerable<NoteResponseDto>>(notes);
                return Ok(notesResult);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetNotesPage action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }            
        }

        // POST api/<DocumentController>
        [HttpPost]
        public IActionResult CreateNote([FromBody] NoteCreateDto noteCreateDto)
        {
            if (noteCreateDto == null)
            {
                _logger.LogError("Note object sent from client is null.");
                return BadRequest("Note object is null");
            }
            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid Note object sent from client.");
                return BadRequest("Invalid Note object");
            }
            var currentUser=_userManager.FindByNameAsync(User.Identity.Name);

            _logger.LogInfo("Current User Id: " + currentUser.Result.Id);

            var notesUser = _unitOfWork.NoteUsersRepository.Get(
                user=>user.IdentityId.Equals(currentUser.Result.Id.ToString()))
                .FirstOrDefault();

            if (notesUser != null)
            {
                try
                {
                    _logger.LogInfo("User Name: " + User.Identity.Name);
                    _logger.LogInfo("User Name: " + notesUser.Id);
                    var note = new Note
                    {
                        Title = noteCreateDto.Title,
                        Content = noteCreateDto.Content,
                        NoteUserId = notesUser.Id,
                        DateCreated = DateTime.Now
                    };
                    _unitOfWork.NotesRepository.Insert(note);
                    _unitOfWork.Save();
                    _logger.LogError($"Created new Note");
                    return NoContent();
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Something went wrong inside CreateNote action: {ex.Message}");
                    return StatusCode(500, "Internal server error");
                }
            }
            else
            {
                _logger.LogError("Something wen wrong. Notes User Is Null");
                return StatusCode(500, "Invalid Notes User");
            }
        }
    }
}
