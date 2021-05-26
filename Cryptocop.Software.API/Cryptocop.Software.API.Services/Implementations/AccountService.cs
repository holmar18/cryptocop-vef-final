using Cryptocop.Software.API.Repositories.Interfaces;
using Cryptocop.Software.API.Services.Interfaces;
using Cryptocop.Software.API.Models;
using Cryptocop.Software.API.Models.Dtos;

namespace Cryptocop.Software.API.Services.Implementations
{
    public class AccountService : IAccountService
    {
        private readonly IUserRepository _UserRepository;
        private readonly ITokenRepository _TokenRepository;

        public AccountService(IUserRepository UserRepository, ITokenRepository tokenRepository = null)
        {
            _UserRepository = UserRepository;
            _TokenRepository = tokenRepository;
        }

        public UserDto CreateUser(RegisterInputModel inputModel)
        {
            return _UserRepository.CreateUser(inputModel);
        }

        public UserDto AuthenticateUser(LoginInputModel loginInputModel)
        {
            return _UserRepository.AuthenticateUser(loginInputModel);
        }

        public void Logout(int tokenId)
        {
            _TokenRepository.VoidToken(tokenId);
        }
    }
}