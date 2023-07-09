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

        #region Message request
        [HttpPost("request")]
        public IActionResult RequestMessage([FromQuery] string requester, [FromQuery] string receiver, [FromQuery] string message)
        {
            var response = _appService.RequestMessage(requester, receiver, message);
            if (response != null)
            {
                return Conflict(response);
            }

            return StatusCode(201);
        }

        #endregion

        [HttpGet("contacts")]
        public IActionResult GetContacts()
        {
            var userName = (HttpContext.User.Identity as ClaimsIdentity).FindFirst(ClaimTypes.Name.ToString()).Value;
            var result = _appService.GetContactsByUser(userName);
            return Ok(result);
        }

        [HttpDelete("contacts/{id}")]
        public IActionResult RemoveContact([FromRoute] int id)
        {
            var userName = (HttpContext.User.Identity as ClaimsIdentity).FindFirst(ClaimTypes.Name.ToString()).Value;
            _appService.RemoveContact(userName, id);
            return Ok();
        }

        [HttpGet("search")]
        public IActionResult SearchUser([FromQuery] string username)
        {
            var requester = (HttpContext.User.Identity as ClaimsIdentity).FindFirst(ClaimTypes.Name.ToString()).Value;
            try
            {
                var result = _appService.SearchUser(requester, username);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
