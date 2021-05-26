using System.ComponentModel.DataAnnotations;
using System;
using Cryptocop.Software.API.Repositories.Interfaces;
using Cryptocop.Software.API.Models;
using Cryptocop.Software.API.Models.Dtos;
using Cryptocop.Software.API.Repositories.Context;
using Cryptocop.Software.API.Repositories.Helpers;
using System.Linq;

namespace Cryptocop.Software.API.Repositories.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly CryptoCopDbContext _dbContext;
        private readonly ITokenRepository _TokenRepository;


        public UserRepository(CryptoCopDbContext dbContext, ITokenRepository tokenRepository = null)
        {
            _dbContext = dbContext;
            _TokenRepository = tokenRepository;
        }


        public UserDto CreateUser(RegisterInputModel user)
        {
            // check if user with the email exist
            var userExist = _dbContext.User.FirstOrDefault(r => r.Email == user.Email);
            // if he does exist return 403 Forbidden
            if (userExist != null) { return null; }
            // Generate the next id
            var nextId = _dbContext.User.Count() + 1;
            // Create new token
            JwtToken token = _TokenRepository.CreateNewToken();

            // Create new userDto to return
            UserDto userDt = new UserDto
            {
                Id = nextId,
                FullName = user.FullName,
                Email = user.Email,
                TokenId = token.Id
            };
            // Create new user to store in db
            User userNew = new User
            {
                Id = nextId,
                FullName = user.FullName,
                Email = user.Email,
                Password = HashingHelper.HashPassword(user.Password)
            };

            // Add the newly registered user to the Database of users & save
            _dbContext.User.Add(userNew);
            _dbContext.SaveChanges();
            return userDt;

        }

        public UserDto AuthenticateUser(LoginInputModel loginInputModel)
        {
            // find the user
            var user = _dbContext.User.FirstOrDefault(r => 
                r.Email == loginInputModel.Email &&
                r.Password == HashingHelper.HashPassword(loginInputModel.Password));
            if (user == null) { return null; }

            // create a new token and add the token to the token table and save the changes
            JwtToken token = _TokenRepository.CreateNewToken();

            // Create new userDto and assign it the token id
            return new UserDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                TokenId = token.Id
            };
        }
    }
}