using Microsoft.EntityFrameworkCore;
using QDryClean.Application.Absreactions;
using QDryClean.Application.Common.Exceptions;
using QDryClean.Application.Common.Interfaces.Auth;
using QDryClean.Application.Common.Responses;
using QDryClean.Application.Dtos;

namespace QDryClean.Infrastructure.Services.JWT;

public class AuthService : IAuthService
{
    private readonly IApplicationDbContext _context;
    private readonly ITokenService _tokenService;

    public AuthService(IApplicationDbContext context, ITokenService tokenService)
    {
        _context = context;
        _tokenService = tokenService;
    }

    public async Task<ApiResponse<UserAuthDto>> LoginAsync(string login, string password)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.LogIn == login);
        if (user is null)
        {
            throw new InvalidLoginOrPasswordException("Invalid login.");
        }
        if (user.Password != password)
        {
            throw new InvalidLoginOrPasswordException("Invalid password.");
        }
        var userDto = new UserAuthDto
        {
            Token = _tokenService.GenerateToken(user.Id, user.UserRole),
            Role = user.UserRole.ToString()
        };

        return ApiResponseFactory.Ok(userDto);
    }
}
