using AutoFixture.NUnit3;
using CustomerManagement.Controllers;
using CustomerManagement.Entities;
using CustomerManagement.Repositories;
using CustomerManagement.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System.Web.Mvc;

namespace CustomerManagementTests.UnitTests.Controllers
{
    public class CustomerControllerTests
    {

        private readonly Mock<ILogger<CustomerController>> _mockLogger = new Mock<ILogger<CustomerController>>();
        [Test]
        [AutoDataNoRecursion]
        public async Task Get_Returns_404_IfNoCustomerFoundWithId(Mock<ICustomerService> mockCustomerService, Mock<ICustomerRepository> mockCustomerRepo)
        {
            mockCustomerService.Setup(x =>
                x.GetCustomerById(It.IsAny<Guid>())
                ).Returns(Task.FromResult<Customer>(null));
            var controller = new CustomerController(mockCustomerService.Object, mockCustomerRepo.Object, _mockLogger.Object);

            var result = await controller.Get(Guid.NewGuid());
            ((StatusCodeResult)result).StatusCode.Should().Be(404);
           
        }

        [Test]
        [AutoDataNoRecursion]
        public async Task Get_Returns_Correct_Customer_If_Found(Customer customer, Mock<ICustomerService> mockCustomerService, Mock<ICustomerRepository> mockCustomerRepo)
        {
            mockCustomerService.Setup(x =>
                x.GetCustomerById(It.IsAny<Guid>())
                ).ReturnsAsync(customer);
            var controller = new CustomerController(mockCustomerService.Object, mockCustomerRepo.Object, _mockLogger.Object);

            var result = (ObjectResult)(await controller.Get(Guid.NewGuid()));
            result.StatusCode.Should().Be(200);
            (result.Value.As<Customer>()).Should().Be(customer);

        }

        [Test]
        [AutoDataNoRecursion]
        public async Task GetAllCustomers_Returns_All_customers(
            Customer customer1, 
            Customer customer2, 
            Mock<ICustomerService> mockCustomerService, 
            Mock<ICustomerRepository> mockCustomerRepo
        ) {
            IEnumerable<Customer> customers = new List<Customer> { customer1, customer2 };
            mockCustomerRepo.Setup(x =>
                x.GetCustomers()
                ).ReturnsAsync(customers);
            var controller = new CustomerController(mockCustomerService.Object, mockCustomerRepo.Object, _mockLogger.Object);

            var result = (ObjectResult)(await controller.GetAllCustomers());
            result.StatusCode.Should().Be(200);
            result.Value.As<IEnumerable<Customer>>().Should().BeEquivalentTo(customers);

        }

        [Test]
        [AutoDataNoRecursion]
        public async Task Create_Successfully_creates_a_customer_and_returns_201_and_id_of_created_Customer(
            Customer customer,
            CreateCustomerDTO createCustomerDto,
            Mock<ICustomerService> mockCustomerService,
            Mock<ICustomerRepository> mockCustomerRepo
        ) {
            mockCustomerService.Setup(x =>
                x.CreateCustomer(createCustomerDto)
                ).ReturnsAsync(customer);
            var controller = new CustomerController(mockCustomerService.Object, mockCustomerRepo.Object, _mockLogger.Object);

            var result = (CreatedResult)(await controller.Create(createCustomerDto));
            result.StatusCode.Should().Be(201);
            result.Value.As<Customer>().Should().Be(customer);
            result.Location.As<string>().Should().Be($"/customers/{customer.Id}");
        }

        [Test]
        [AutoDataNoRecursion]
        public async Task Create_returns_a_400_and_Error_message_if_duplicate_email_is_used(
            Customer customer,
            CreateCustomerDTO createCustomerDto,
            Mock<ICustomerService> mockCustomerService,
            Mock<ICustomerRepository> mockCustomerRepo
        )
        {
            mockCustomerService.Setup(x =>
                x.CreateCustomer(createCustomerDto)
                ).Throws<DbUpdateException>();
            var controller = new CustomerController(mockCustomerService.Object, mockCustomerRepo.Object, _mockLogger.Object);

            var result = (BadRequestObjectResult)(await controller.Create(createCustomerDto));
            result.StatusCode.Should().Be(400);
            result.Value.Should().Be("A customer with the email address provided already exists");
        }

        [Test]
        [AutoDataNoRecursion]
        public async Task Delete_initiates_delete_process_and_returns_200(
             Guid customerId, 
             Mock<ICustomerService> mockCustomerService,
             Mock<ICustomerRepository> mockCustomerRepo
        )
        {
            var controller = new CustomerController(mockCustomerService.Object, mockCustomerRepo.Object, _mockLogger.Object);

            var result = (OkResult)(await controller.Delete(customerId));

            result.StatusCode.Should().Be(200);
            mockCustomerService.Verify(x=> x.DeleteCustomer(customerId), Times.Once());
        }

        [Test]
        [AutoDataNoRecursion]
        public async Task SetPrimaryAddress_initiates_Update_and_returns_200(
             Guid customerId,
             Guid addressId, 
             Mock<ICustomerService> mockCustomerService,
             Mock<ICustomerRepository> mockCustomerRepo)
        {
            
            var controller = new CustomerController(mockCustomerService.Object, mockCustomerRepo.Object, _mockLogger.Object);
            var result = (OkResult) await controller.SetPrimaryAddress(customerId, addressId);
            mockCustomerService.Verify(x => x.UpdatePrimaryAddressIfBelongsToCustomer(customerId, addressId), Times.Once);
        }
    }
}