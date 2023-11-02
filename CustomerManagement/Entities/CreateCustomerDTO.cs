using System.ComponentModel.DataAnnotations;

namespace CustomerManagement.Entities
{
    public class CreateCustomerDTO
    {
        [MaxLength(20)]
        [Required]
        public string Title { get; set; }

        [Required]
        [MaxLength(50)]
        public string Forename { get; set; }

        [Required]
        [MaxLength(50)]
        public string Surname { get; set; }

        [Required]
        [MaxLength(75)]
        public string Email { get; set; }

        [Required]
        [MaxLength(15)]
        public string MobileNumber { get; set; }

        public CreateAddressDTO Address { get; set; }
    }
}
