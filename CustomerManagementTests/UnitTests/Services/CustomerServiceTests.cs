using AutoFixture.NUnit3;
using CustomerManagement.Entities;
using CustomerManagement.Repositories;
using CustomerManagement.Services;
using FluentAssertions;
using Moq;

namespace CustomerManagementTests.UnitTests.Services
{
    public class CustomerServiceTests
    {
        [Test]
        [AutoDataNoRecursion]
        public async Task GetCustomerById(Customer customer, Mock<ICustomerRepository> mockCustomerRepo, Mock<IAddressRepository> mockAddressRepo)
        {
            mockCustomerRepo.Setup(x => x.GetCustomerById(customer.Id)).ReturnsAsync(customer);
            var service = new CustomerService(mockCustomerRepo.Object, mockAddressRepo.Object);
            var result = await service.GetCustomerById(customer.Id);
            mockCustomerRepo.Verify(x => x.GetCustomerById(customer.Id), Times.Once);
            result.Should().Be(customer);
        }

        [Test]
        [AutoDataNoRecursion]
        public async Task Delete_should_call_repo_to_delete_customer(Guid customerId, Mock<ICustomerRepository> mockCustomerRepo, Mock<IAddressRepository> mockAddressRepo)
        {
            var service = new CustomerService(mockCustomerRepo.Object, mockAddressRepo.Object);
            await service.DeleteCustomer(customerId);
            mockCustomerRepo.Verify(x => x.DeleteCustomer(customerId), Times.Once);
        }

        [Test]
        [AutoDataNoRecursion]
        public async Task CreateCustomer_Should_Generate_An_Id_And_Create_The_Customer(CreateCustomerDTO createCustomerDTO, Mock<ICustomerRepository> mockCustomerRepo, Mock<IAddressRepository> mockAddressRepo)
        {
            var service = new CustomerService(mockCustomerRepo.Object, mockAddressRepo.Object);
            var customer = await service.CreateCustomer(createCustomerDTO);

            var customerWithoutAddress = customer;
            customerWithoutAddress.PrimaryAddressId = Guid.Empty;

            mockCustomerRepo.Verify(x => x.CreateCustomer(customerWithoutAddress), Times.Once);
        }

        [Test]
        [AutoDataNoRecursion]
        public async Task CreateCustomer_Should_Create_The_Primary_address_For_The_Customer_And_Update_The_Customers_Primary_Address(CreateCustomerDTO createCustomerDTO, Mock<ICustomerRepository> mockCustomerRepo, Mock<IAddressRepository> mockAddressRepo)
        {
            var service = new CustomerService(mockCustomerRepo.Object, mockAddressRepo.Object);
            var customer = await service.CreateCustomer(createCustomerDTO);
            mockAddressRepo.Verify(x => x.CreateAddress(customer.Addresses.First()), Times.Once);
            mockCustomerRepo.Verify(x => x.UpdateCustomer(customer), Times.Once);

        }

        [Test]
        [AutoDataNoRecursion]
        public async Task CreateCustomer_Should_Create_Set_The_Country_Of_The_Address_To_UK_If_Not_Provided(CreateCustomerDTO createCustomerDTO, Mock<ICustomerRepository> mockCustomerRepo, Mock<IAddressRepository> mockAddressRepo)
        {
            var service = new CustomerService(mockCustomerRepo.Object, mockAddressRepo.Object);
            createCustomerDTO.Address.Country = null;
            var customer = await service.CreateCustomer(createCustomerDTO);
            mockAddressRepo.Verify(x => x.CreateAddress(It.Is<Address>(x=> x.Id == customer.PrimaryAddressId && x.Country == "UK")), Times.Once);
        }

        [Test]
        [AutoDataNoRecursion]
        public async Task IsPrimaryAddress_Returns_true_if_the_address_is_a_Primary_address(Customer customer, Address address, CreateCustomerDTO createCustomerDTO, Mock<ICustomerRepository> mockCustomerRepo, Mock<IAddressRepository> mockAddressRepo)
        {
            customer.PrimaryAddressId = address.Id;
            customer.Id = address.CustomerId;

            mockCustomerRepo.Setup(x => x.GetCustomerById(customer.Id)).ReturnsAsync(customer);
            mockAddressRepo.Setup(x => x.GetAddressById(address.Id)).Returns(address);


            var service = new CustomerService(mockCustomerRepo.Object, mockAddressRepo.Object);
            var response = await service.IsPrimaryAddress(address.Id);
            response.Should().BeTrue();
        }
    }
}
