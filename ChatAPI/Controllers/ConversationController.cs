﻿using Chat.Application.DTOs;
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
        private readonly IUserAppService _userAppService;
        private readonly IConversationAppService _appService;

        public ConversationController(IUserAppService userAppService, IConversationAppService appService)
        {
            _userAppService = userAppService;
            _appService = appService;
        }

        [HttpPost("request")]
        public IActionResult RequestMessage([FromQuery] string requester, [FromQuery] string receiver, [FromQuery] string message)
        {
            var response = _userAppService.RequestMessage(requester, receiver, message);
            if (response != null)
            {
                return Conflict(response);
            }

            return StatusCode(201);
        }

        [HttpPost("create")]
        public IActionResult CreateConversation([FromBody] ConversationCreateDTO dto)
        {
            var userName = (HttpContext.User.Identity as ClaimsIdentity).FindFirst(ClaimTypes.Name.ToString()).Value;
            var result = _appService.CreateAllowedPrivate(dto, userName);
            return Ok(result);
        }

        [HttpPost("accept-request")]
        public IActionResult CreateConversation([FromQuery] int requestId)
        {
            var userName = (HttpContext.User.Identity as ClaimsIdentity).FindFirst(ClaimTypes.Name.ToString()).Value;
            var result = _appService.CreatePrivateByRequest(requestId, userName);
            return Ok(result);
        }
    }
}