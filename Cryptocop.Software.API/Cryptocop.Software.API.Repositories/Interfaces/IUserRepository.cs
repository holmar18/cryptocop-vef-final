﻿using Cryptocop.Software.API.Models;
using Cryptocop.Software.API.Models.Dtos;

namespace Cryptocop.Software.API.Repositories.Interfaces
{
    public interface IUserRepository
    {
        UserDto CreateUser(RegisterInputModel inputModel);
        UserDto AuthenticateUser(LoginInputModel loginInputModel);
    }
}