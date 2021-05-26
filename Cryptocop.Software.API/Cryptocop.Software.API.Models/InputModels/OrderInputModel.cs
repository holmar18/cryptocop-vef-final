using System.ComponentModel.DataAnnotations;


namespace Cryptocop.Software.API.Models
{
    public class OrderInputModel
    {
        public int AddressId { get; set; }

        public int PaymentCardId { get; set; }
    }
}