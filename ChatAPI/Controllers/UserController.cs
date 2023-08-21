using Chat.Application.DTOs;
using Chat.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ChatAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly IUserAppService _appService;

        public UserController(IUserAppService appService)
        {
            _appService = appService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var result = _appService.GetAll();
            if (result == null || !result.Any())
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var result = _appService.GetById(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpPost("request")]
        public IActionResult RequestMessage([FromBody] MessagePermissionCreateDTO dto)
        {
            var response = _appService.RequestMessage(GetUserNameFromRequest(), dto);
            if (response != null)
            {
                return Conflict(response);
            }

            return StatusCode(201);
        }

        [HttpGet("contacts")]
        public IActionResult GetContacts()
        {
            var result = _appService.GetContactsByUser(GetUserNameFromRequest());
            return Ok(result);
        }

        [HttpPost("contacts/{id}")]
        public IActionResult AddContact([FromRoute] int id)
        {
            _appService.AddContact(GetUserNameFromRequest(), id);
            return StatusCode(201);
        }

        [HttpDelete("contacts/{id}")]
        public IActionResult RemoveContact([FromRoute] int id)
        {
            _appService.RemoveContact(GetUserNameFromRequest(), id);
            return Ok();
        }

        [HttpGet("search")]
        public IActionResult SearchUser([FromQuery] string username)
        {
            try
            {
                var result = _appService.SearchUser(GetUserNameFromRequest(), username);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("request/all")]
        public IActionResult GetRequestsByUser()
        {
            var result = _appService.GetRequestsByUser(GetUserNameFromRequest());

            return Ok(result);
        }

        [HttpDelete("request")]
        public IActionResult RefuseRequest([FromQuery] int requestId)
        {
            _appService.RefuseRequest(GetUserNameFromRequest(), requestId);
            return Ok();
        }

        private string GetUserNameFromRequest()
        {
            return (HttpContext.User.Identity as ClaimsIdentity).FindFirst(ClaimTypes.Name.ToString()).Value;
        }
    }
}
