using System.Collections.Generic;
using Cryptocop.Software.API.Services.Interfaces;
using Cryptocop.Software.API.Models;
using Cryptocop.Software.API.Repositories.Interfaces;

namespace Cryptocop.Software.API.Services.Implementations
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;

        public PaymentService(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        public void AddPaymentCard(string email, PaymentCardInputModel paymentCard)
        {
            _paymentRepository.AddPaymentCard(email, paymentCard);
        }

        public IEnumerable<PaymentCardDto> GetStoredPaymentCards(string email)
        {
            return _paymentRepository.GetStoredPaymentCards(email);
        }
    }
}