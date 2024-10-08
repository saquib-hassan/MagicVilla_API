﻿using MagicVilla_API.Models;
using MagicVilla_API.Models.Dto;

namespace MagicVilla_API.Repository.IRepository
{
    public interface IUserRepository
    {
        bool isUniqueUser(string username);
        Task<LoginResponseDTO> Login (LoginRequestDTO loginRequestDTO);
        Task<LocalUser> Register (RegistrationRequestDTO registrationRequestDTO);
    }
}
