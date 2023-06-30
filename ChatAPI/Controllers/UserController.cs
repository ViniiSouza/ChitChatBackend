using Chat.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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


    }
}
