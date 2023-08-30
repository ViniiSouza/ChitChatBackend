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
    public class ConversationController : ControllerBase
    {
        private readonly IConversationAppService _appService;

        public ConversationController(IUserAppService userAppService, IConversationAppService appService)
        {
            _appService = appService;
        }

        [HttpPost("create")]
        public IActionResult CreateConversation([FromBody] ConversationCreateDTO dto)
        {
            var result = _appService.CreateAllowedPrivate(dto, GetUserNameFromRequest());
            return StatusCode(201, result);
        }

        [HttpPost("accept-request")]
        public IActionResult CreateConversationFromRequest([FromQuery] int requestId)
        {
            var result = _appService.CreatePrivateByRequest(requestId, GetUserNameFromRequest());
            return Ok(result);
        }

        [HttpGet("load-all")]
        public IActionResult GetAllFromUser() // handle filters in the future
        {
            var result = _appService.LoadConversationsByUser(GetUserNameFromRequest());
            return Ok(result);
        }

        [HttpGet("{id}")]
        public IActionResult GetConversation([FromRoute] int id)
        {
            var result = _appService.GetConversation(id, GetUserNameFromRequest());
            return Ok(result);
        }

        [HttpGet("{id}/messages")]
        public IActionResult GetMessagesBefore([FromRoute] int id, [FromQuery] int messageId)
        {
            var result = _appService.GetBeforeMessage(id, messageId);
            return Ok(result);
        }

        [HttpGet("simple/{targetUserName}")]
        public IActionResult FindSimpleConversation([FromRoute] string targetUserName)
        {
            var result = _appService.GetSimplePrivate(GetUserNameFromRequest(), targetUserName);
            if (result == null) return NotFound("Chat not found!");

            return Ok(result);
        }

        [HttpPost("message")]
        public IActionResult SendMessage([FromBody] MessageCreateDTO dto)
        {
            var result = _appService.SendMessage(dto, GetUserNameFromRequest());
            return StatusCode(201, result);
        }

        private string GetUserNameFromRequest()
        {
            return (HttpContext.User.Identity as ClaimsIdentity).FindFirst(ClaimTypes.Name.ToString()).Value;
        }
    }
}
