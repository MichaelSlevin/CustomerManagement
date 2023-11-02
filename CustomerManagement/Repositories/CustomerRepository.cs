using CustomerManagement.Entities;
using Microsoft.EntityFrameworkCore;

namespace CustomerManagement.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        public CustomerManagementDbContext _context { get; set; }
        public CustomerRepository(CustomerManagementDbContext context)
        {
            _context = context;

        }
        public async Task<Customer> GetCustomerById(Guid id)
        {
            return await _context.Customers.Include("Addresses").FirstOrDefaultAsync(x=> x.Id == id);
        }

        public async Task CreateCustomer(Customer customer)
        {
            var createdCustomer = await _context.AddAsync(customer);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCustomer(Guid customerId)
        {
            var customer = _context.Customers.FirstOrDefault(x => x.Id == customerId);
            if(customer is not null)
            {
                _context.Remove(customer);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Customer>> GetCustomers()
        {
            return _context.Customers.Include("Addresses");
        }

        public async Task UpdateCustomer(Customer customer)
        {
            _context.Update(customer);
            await _context.SaveChangesAsync();
        }
    }

}
