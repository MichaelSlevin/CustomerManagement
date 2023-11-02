using CustomerManagement.Entities;

namespace CustomerManagement.Repositories
{
    public class AddressRepository : IAddressRepository
    {
        public CustomerManagementDbContext _context { get; set; }
        public AddressRepository(CustomerManagementDbContext context)
        {
            _context = context;

        }

        public async Task CreateAddress(Address address)
        {
            var createdAddress = await _context.AddAsync(address);
            await _context.SaveChangesAsync();
        }
    }

}
