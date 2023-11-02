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

        public Address GetAddressById(Guid id)
        {
            return _context.Addresses.FirstOrDefault(x=> x.Id == id);
        }

        public async Task Delete(Guid id)
        {
            var address =_context.Addresses.FirstOrDefault(x => x.Id == id);
            if(address is not null)
            {
                _context.Remove(address);
                await _context.SaveChangesAsync();
            }
        }
    }

}
