using CustomerManagement.Entities;
using CustomerManagement.Repositories;

namespace CustomerManagement.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IAddressRepository _addressRepository;
        public CustomerService(ICustomerRepository customerRepository, IAddressRepository addressRepository) 
        { 
            _customerRepository = customerRepository;
            _addressRepository = addressRepository;
        }

        public async Task<Customer> GetCustomerById(Guid id)
        {
            var customer =  await _customerRepository.GetCustomerById(id);
            return customer;
        }

        public async Task<Customer> CreateCustomer(CreateCustomerDTO createCustomer)
        {
            var id = Guid.NewGuid();
            var customer = new Customer
            {
                Id = id,
                Email = createCustomer.Email,
                Title = createCustomer.Title,
                Forename = createCustomer.Forename,
                Surname = createCustomer.Surname,
                MobileNumber = createCustomer.MobileNumber,

            };
            await _customerRepository.CreateCustomer(customer);

            var address = new Address(id, createCustomer.Address);
            await _addressRepository.CreateAddress(address);
            customer.PrimaryAddressId = address.Id;
            await _customerRepository.UpdateCustomer(customer);
            customer.Addresses = new List<Address> { address };
            return customer;
        }

        public async Task DeleteCustomer(Guid id)
        {
            await _customerRepository.DeleteCustomer(id);
        }

        public async Task<bool> IsPrimaryAddress(Guid addressId)
        {
            var address = _addressRepository.GetAddressById(addressId);
            var customer = await _customerRepository.GetCustomerById(address.CustomerId);
            return customer.PrimaryAddressId == addressId;
        }
    }
}
