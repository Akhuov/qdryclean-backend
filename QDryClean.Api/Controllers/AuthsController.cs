using Microsoft.AspNetCore.Mvc;
using QDryClean.Application.Absreactions;
using QDryClean.Application.Common.Interfaces.Auth;
using QDryClean.Application.Dtos;

namespace QDryClean.Api.Controllers
{
    [Route("api/v1/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IApplicationDbContext _context;
        private readonly ITokenService _tokenService;
        private readonly IAuthService _authService;
        public AuthController(IApplicationDbContext context, ITokenService tokenService, IAuthService authService)
        {
            _context = context;
            _tokenService = tokenService;
            _authService = authService;
        }
        [HttpPost]
        public async Task<IActionResult> LoginAsync(AuthDto dto)
            => Ok(await _authService.LoginAsync(dto.LogIn, dto.Password));
    }
}
