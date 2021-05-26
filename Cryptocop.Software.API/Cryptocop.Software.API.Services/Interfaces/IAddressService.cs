using Cryptocop.Software.API.Models;
using Cryptocop.Software.API.Models.Dtos;
using System.Collections.Generic;

namespace Cryptocop.Software.API.Services.Interfaces
{
    public interface IAddressService
    {
        void AddAddress(string email, AddressInputModel address);
        IEnumerable<AddressDto> GetAllAddresses(string email);
        void DeleteAddress(string email, int addressId);
    }
}