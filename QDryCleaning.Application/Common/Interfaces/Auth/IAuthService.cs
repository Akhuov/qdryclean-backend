using QDryClean.Application.Common.Responses;
using QDryClean.Application.Dtos;

namespace QDryClean.Application.Common.Interfaces.Auth
{
    public interface IAuthService
    {
        Task<ApiResponse<UserAuthDto>> LoginAsync(string login, string password);
    }
}
