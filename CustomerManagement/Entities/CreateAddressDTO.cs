using System.ComponentModel.DataAnnotations;

namespace CustomerManagement.Entities
{
    public class CreateAddressDTO
    {
        [Required]
        [MaxLength(80)]
        public string Line1 { get; set; }

        [MaxLength(80)]
        public string? Line2 { get; set; }

        [MaxLength(50)]
        [Required]
        public string Town { get; set; }

        [MaxLength(50)]
        public string? County { get; set; }

        [MaxLength(10)]
        [Required]
        public string Postcode { get; set; }
        public string? Country { get; set; }
    }
}
