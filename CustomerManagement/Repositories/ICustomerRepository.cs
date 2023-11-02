using CustomerManagement.Entities;

namespace CustomerManagement.Repositories
{
    public interface ICustomerRepository
    {
        Task<IEnumerable<Customer>> GetCustomers();
        Task<Customer> GetCustomerById(Guid id);
        Task CreateCustomer(Customer customer);
        Task DeleteCustomer(Guid customerId);
        Task UpdateCustomer(Customer customer);
        Task<bool> DoesCustomerExist(Guid customerId);
        Task SetActiveStatus(Guid customerId, bool active);
        Task<IEnumerable<Customer>> GetActiveCustomers();
    }
}
