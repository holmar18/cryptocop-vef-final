namespace Cryptocop.Software.API.Repositories.Helpers
{
    public class PaymentCardHelper
    {
        public string MaskPaymentCard(string paymentCardNumber)
        {
            string mask =  "xxxx-xxxx-xxxx-" +  paymentCardNumber.Substring(paymentCardNumber.Length-4,4);
            return mask;
        }
    }
}