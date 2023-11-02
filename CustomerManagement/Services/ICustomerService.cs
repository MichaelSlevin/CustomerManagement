using CustomerManagement.Entities;

namespace CustomerManagement.Services
{
    public interface ICustomerService
    {
        Task<Customer> GetCustomerById(Guid id);
        Task DeleteCustomer(Guid id);
        Task<Customer> CreateCustomer(CreateCustomerDTO customer);
        Task<bool> IsPrimaryAddress(Guid addressId);
        Task UpdatePrimaryAddressIfBelongsToCustomer(Guid customerId, Guid addressId);
    }
}
