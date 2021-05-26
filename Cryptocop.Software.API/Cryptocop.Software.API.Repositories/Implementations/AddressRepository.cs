using System.Collections.Generic;
using Cryptocop.Software.API.Repositories.Interfaces;
using Cryptocop.Software.API.Models;
using Cryptocop.Software.API.Repositories.Context;
using System.Linq;
using System;

namespace Cryptocop.Software.API.Repositories.Implementations
{
    public class AddressRepository : IAddressRepository
    {
        private readonly CryptoCopDbContext _dbContext;

        private static AddressDto ToAddrDto(Address addr)
        {
            return new AddressDto {
                Id = addr.Id,
                StreetName = addr.StreetName,
                HouseNumber = addr.HouseNumber,
                ZipCode = addr.ZipCode,
                Country = addr.Country,
                City = addr.City
            };
        }

        public AddressRepository(CryptoCopDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public void AddAddress(string email, AddressInputModel address)
        {
            // find the user with the email
            var user = _dbContext.User.FirstOrDefault(r => r.Email == email);
            // get new id 
             var nextId = _dbContext.Address.Count() + 1;

            var data = new Address {
                Id = nextId,
                UserId = user.Id,
                StreetName = address.StreetName,
                HouseNumber = address.HouseNumber,
                ZipCode = address.ZipCode,
                Country = address.Country,
                City = address.City
            };
            // Add it to the database & save
            _dbContext.Address.Add(data);
            _dbContext.SaveChanges();
        }

        public IEnumerable<AddressDto> GetAllAddresses(string email)
        {
            // find the user with the email
            var user = _dbContext.User.FirstOrDefault(r => r.Email == email);
            // find all the addresses with his ID
            return _dbContext.Address.Where(m => m.UserId == user.Id).Select(m => ToAddrDto(m));

        }

        public void DeleteAddress(string email, int addressId)
        {
            // find the user with the email
            var user = _dbContext.User.FirstOrDefault(r => r.Email == email);
            // Find the address with that id
            var address = _dbContext.Address.FirstOrDefault(r => r.Id == addressId && r.UserId == user.Id);
            // remove it & save
            _dbContext.Address.Remove(address);
            _dbContext.SaveChanges();
        }
    }
}