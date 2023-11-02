using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CustomerManagement.Entities
{

    [Table("Addresses")]
    public class Address
    {

        public Address()
        {
        }

        public Address(Customer customer, CreateAddressDTO createAddress)
        {
            Id = new Guid();
            Line1 = createAddress.Line1;
            Line2 = createAddress.Line2 ?? "";
            Town = createAddress.Town;
            County = createAddress.County ?? "";
            Postcode = createAddress.Postcode;
            Country = createAddress.Country ?? "UK";
            CustomerId = customer.Id;
        }

        public Guid Id { get; set; }

        [Required]
        [Column(TypeName = "varchar(80)")]
        public string Line1 { get; set; }

        [Column(TypeName = "varchar(80)")]
        public string Line2 { get; set; }
        
        [Column(TypeName = "varchar(50)")]
        [Required]
        public string Town { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string County { get; set; }

        [Column(TypeName = "varchar(10)")]
        [Required]
        public string Postcode { get; set; }

        public string Country { get; set; }

        [ForeignKey("CustomerId")]
        public Guid CustomerId { get; set; }
    }
}
