using MediatR;
using QDryClean.Application.Dtos;
using QDryClean.Domain.Enums;
using System.Text.Json.Serialization;

namespace QDryClean.Application.UseCases.Users.Commands
{
    public class UpdateUserCommand : IRequest<UserDto>
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public UserRole UserRole { get; set; } = UserRole.Receptionist; // Default role is Receptionist
    }
}
