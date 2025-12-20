

using Microsoft.AspNetCore.Mvc;
using VeterinaryClinic.Application.DTOs.Auth;
using VeterinaryClinic.Application.Interfaces;

namespace VeterinaryClinic.API.Controllers
{
    [ApiController]
    [Route ("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;


        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthResponseDto>> Register(RegisterDto dto)
        {
            _logger.LogInformation("Register request received for email {Email}", dto.Email);

            var result = await _authService.ResgiterAsync(dto);

            return CreatedAtAction(nameof(Register), new {id = result.Id}, result);
            
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> Login(LoginDto dto)
        {
            _logger.LogInformation("Login request received for email {Email}", dto.Email);

            var result = await _authService.LoginAsync(dto);

            return Ok(result);

        }


    }

    
}