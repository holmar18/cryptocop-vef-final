using Cryptocop.Software.API.Repositories.Interfaces;
using Cryptocop.Software.API.Models;
using Cryptocop.Software.API.Repositories.Context;
using System.Linq;
using Cryptocop.Software.API.Repositories.Helpers;

namespace Cryptocop.Software.API.Repositories.Implementations
{
    public class TokenRepository : ITokenRepository
    {
        private readonly CryptoCopDbContext _dbContext;

        public TokenRepository(CryptoCopDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public JwtToken CreateNewToken()
        {
            // Create a new token
            var token = new JwtToken();
            // add the token to the token table and save the changes
            _dbContext.JwtToken.Add(token);
            _dbContext.SaveChanges();
            return token;
        }

        public bool IsTokenBlacklisted(int tokenId)
        {
            // Get the token from the data base
            var token = _dbContext.JwtToken.FirstOrDefault(t => t.Id == tokenId);
            // chech the byte Blacklisted
            if (token == null) { return true; }
            // send the right bool back
            return token.Blacklisted;
        }

        public void VoidToken(int tokenId)
        {
            // find the token
            var token = _dbContext.JwtToken.FirstOrDefault(t => t.Id == tokenId);
            // if there is no token do nothing
            if (token == null) { return; }
            // if token is there update Blacklisted to true
            token.Blacklisted = true;
            // save the changes
            _dbContext.SaveChanges();
        }
    }
}