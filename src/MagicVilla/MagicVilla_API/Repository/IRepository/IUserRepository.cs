using MagicVilla_API.Models;
using MagicVilla_API.Models.Dto;

namespace MagicVilla_API.Repository.IRepository
{
    public interface IUserRepository
    {
        bool isUniqueUser(string username);
        Task<LoginRequestDTO> Login (LoginRequestDTO loginRequestDTO);
        Task<LocalUser> LocalUser (RegistrationRequestDTO registrationRequestDTO);
    }
}
