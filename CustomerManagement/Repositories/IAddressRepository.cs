using CustomerManagement.Entities;

namespace CustomerManagement.Repositories
{
    public interface IAddressRepository
    {
        Task CreateAddress(Address address);
    }
}
