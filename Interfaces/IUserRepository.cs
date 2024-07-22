using MedwiseBackend.Dto;
using MedwiseBackend.Models;

namespace MedwiseBackend.Interfaces
{
    public interface IUserRepository
    {
        public string GenerateToken(UserDto userdto);
        public  Task<User> AuthenticateAsync(UserDto userdto);
        public Task<User> RegisterUser(UserDto userdto);

    }
}
