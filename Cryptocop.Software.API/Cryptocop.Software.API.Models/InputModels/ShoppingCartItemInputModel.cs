using System.ComponentModel.DataAnnotations;


namespace Cryptocop.Software.API.Models
{
    public class ShoppingCartItemInputModel
    {
        [Required]
        public string ProductIdentifier { get; set; }

        [Required]
        [Range(0.01, float.MaxValue, ErrorMessage = "Value to low or high")]
        public float? Quantity { get; set; }
    }
}