using CustomerManagement.Entities;

namespace CustomerManagement.Repositories
{
    public interface IAddressRepository
    {
        Address GetAddressById(Guid id);
        Task CreateAddress(Address address);
        Task Delete(Guid id);
    }
}
