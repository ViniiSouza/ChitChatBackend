using Chat.Application.DTOs;
using Chat.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationAppService _appService;
        public AuthenticationController(IAuthenticationAppService appService)
        {
            _appService = appService;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Create([FromBody] CreateUserDTO dto)
        {
            try
            {
                var token = _appService.Register(dto);
                if (token == null)
                {
                    return Conflict("A user with this username already exists!");
                }
                return StatusCode(201, token);
            }
            catch (Exception)
            {
                return StatusCode(500, "Something went wrong. Please try again later!");
            }
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login([FromBody] UserLoginDTO dto)
        {
            var token = _appService.Login(dto);
            if (token == null)
            {
                return NotFound("Wrong username or password!");
            }
            return Ok(token);
        }
    }
}
