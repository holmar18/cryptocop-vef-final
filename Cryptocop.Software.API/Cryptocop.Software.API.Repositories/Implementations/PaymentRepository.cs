using System.Collections.Generic;
using Cryptocop.Software.API.Repositories.Helpers;
using Cryptocop.Software.API.Repositories.Interfaces;
using Cryptocop.Software.API.Models;
using Cryptocop.Software.API.Repositories.Context;
using System.Linq;
using System;

namespace Cryptocop.Software.API.Repositories.Implementations
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly CryptoCopDbContext _dbContext;

        public PaymentRepository(CryptoCopDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        private static PaymentCardDto ToPayDto(PaymentCard addr)
        {
            // Create a mask for the dto Card numbers mask only used in orders according to proj desc
            //string mask =  "xxxx-xxxx-xxxx-" +  addr.CardNumber.Substring(addr.CardNumber.Length-4,4);
            return new PaymentCardDto {
                Id = addr.Id,
                CardholderName = addr.CardholderName,
                CardNumber = addr.CardNumber,
                Month = addr.Month,
                Year = addr.Year
            };
        }
    

        public void AddPaymentCard(string email, PaymentCardInputModel paymentCard)
        {
            // Find the user to get the user id
            var user = _dbContext.User.FirstOrDefault(r => r.Email == email);
            int newId = _dbContext.PaymentCard.Count() + 1;

            var data = new PaymentCard 
            {
                Id = newId,
                UserId = user.Id,
                CardholderName = paymentCard.CardholderName,
                CardNumber = paymentCard.CardNumber,
                Month = paymentCard.Month,
                Year = paymentCard.Year
            };
            // Add it to the database & save
            _dbContext.PaymentCard.Add(data);
            _dbContext.SaveChanges();
        }

        public IEnumerable<PaymentCardDto> GetStoredPaymentCards(string email)
        {
            // find the user with the email
            var user = _dbContext.User.FirstOrDefault(r => r.Email == email);
            // find all the PaymentCard with his ID
            return _dbContext.PaymentCard.Where(m => m.UserId == user.Id).Select(m => ToPayDto(m));
        }
    }
}