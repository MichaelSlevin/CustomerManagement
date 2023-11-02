using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CustomerManagement.Entities
{
    [Table("Customers")]
    [Index(nameof(Email), IsUnique = true)]

    public class Customer
    {
        public Customer()
        {
        }
        public Customer(CreateCustomerDTO createCustomer)
        {
            Id = new Guid();
            Email = createCustomer.Email;
            Title = createCustomer.Title;
            Forename = createCustomer.Forename;
            Surname = createCustomer.Surname;
            MobileNumber = createCustomer.MobileNumber;
        }


        public Guid Id { get; set; }

        public bool IsActive { get; set; }

        [Column(TypeName = "varchar(20)")]
        [Required]
        public string Title { get; set; }


        [Required]
        [Column(TypeName = "varchar(50)")]
        public string Forename { get; set; }

        [Required]
        [Column(TypeName = "varchar(50)")]
        public string Surname { get; set; }

        [Required]
        [Column(TypeName = "varchar(75)")]
        public string Email { get; set; }

        [Required]
        [Column(TypeName = "varchar(15)")]
        public string MobileNumber { get; set; }

        [ForeignKey("primaryAddressId")]
        public Guid PrimaryAddressId { get; set; }
        
        public ICollection<Address>? Addresses { get; set; }
    }
}
