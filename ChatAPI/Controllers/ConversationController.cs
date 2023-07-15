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
            var userName = (HttpContext.User.Identity as ClaimsIdentity).FindFirst(ClaimTypes.Name.ToString()).Value;
            var result = _appService.CreateAllowedPrivate(dto, userName);
            return StatusCode(201, result);
        }

        [HttpPost("accept-request")]
        public IActionResult CreateConversationFromRequest([FromQuery] int requestId)
        {
            var userName = (HttpContext.User.Identity as ClaimsIdentity).FindFirst(ClaimTypes.Name.ToString()).Value;
            var result = _appService.CreatePrivateByRequest(requestId, userName);
            return Ok(result);
        }

        [HttpGet("load-all")]
        public IActionResult GetAllFromUser() // handle filters in the future
        {
            var userName = (HttpContext.User.Identity as ClaimsIdentity).FindFirst(ClaimTypes.Name.ToString()).Value;
            var result = _appService.LoadConversationsByUser(userName);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public IActionResult GetConversation([FromRoute] int id)
        {
            var userName = (HttpContext.User.Identity as ClaimsIdentity).FindFirst(ClaimTypes.Name.ToString()).Value;
            var result = _appService.GetConversation(id, userName);
            return Ok(result);
        }

        [HttpGet("simple/{targetUserName}")]
        public IActionResult FindSimpleConversation([FromRoute] string targetUserName)
        {
            var userName = (HttpContext.User.Identity as ClaimsIdentity).FindFirst(ClaimTypes.Name.ToString()).Value;
            var result = _appService.GetSimplePrivate(userName, targetUserName);
            if (result == null) return NotFound("Chat not found!");

            return Ok(result);
        }

        [HttpPost("message")]
        public IActionResult SendMessage([FromBody] MessageCreateDTO dto)
        {
            var userName = (HttpContext.User.Identity as ClaimsIdentity).FindFirst(ClaimTypes.Name.ToString()).Value;
            var result = _appService.SendMessage(dto, userName);
            return StatusCode(201, result);
        }
    }
}
