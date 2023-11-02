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
using System.Net;
using System.Web.Mvc;

namespace CustomerManagementTests.UnitTests.Controllers
{
    public class AddressControllerTests
    {

        private readonly Mock<ILogger<AddressController>> _mockLogger = new Mock<ILogger<AddressController>>();
        [Test]
        [AutoDataNoRecursion]
        public async Task Get_Returns_404_IfNoAddressFoundWithId(Mock<ICustomerService> mockCustomerService, Mock<ICustomerRepository> mockCustomerRepo)

        {
            var mockAddressRepo = new Mock<IAddressRepository>();
            mockAddressRepo.Setup(x =>
                x.GetAddressById(It.IsAny<Guid>())
                ).Returns((Address)null);
            var controller = new AddressController(mockAddressRepo.Object, mockCustomerRepo.Object, mockCustomerService.Object,  _mockLogger.Object);

            var result = await controller.Get(Guid.NewGuid());
            ((NotFoundObjectResult)result).StatusCode.Should().Be(404);
        }

        [Test]
        [AutoDataNoRecursion]
        public async Task Get_Returns_Correct_Address_If_Found(Address address, Mock<ICustomerService> mockCustomerService, Mock<ICustomerRepository> mockCustomerRepo)
        {
            var mockAddressRepo = new Mock<IAddressRepository>();
            mockAddressRepo.Setup(x =>
                x.GetAddressById(It.IsAny<Guid>())
                ).Returns(address);

            var controller = new AddressController(mockAddressRepo.Object, mockCustomerRepo.Object, mockCustomerService.Object, _mockLogger.Object);

            var result = (ObjectResult)(await controller.Get(Guid.NewGuid()));
            result.StatusCode.Should().Be(200);
            result.Value.Should().Be(address);
        }

        [Test]
        [AutoDataNoRecursion]
        public async Task Create_Successfully_creates_address_and_returns_201_if_Customer_ID_valid(
            CreateAddressDTO createAddressDto,
            Mock<ICustomerService> mockCustomerService,
            Mock<ICustomerRepository> mockCustomerRepo
        )
        {
            var mockAddressRepo = new Mock<IAddressRepository>();

            mockCustomerRepo.Setup(x => x.DoesCustomerExist(It.IsAny<Guid>())).Returns(Task.FromResult(true));


            var customerId = Guid.NewGuid();
            var controller = new AddressController(mockAddressRepo.Object, mockCustomerRepo.Object, mockCustomerService.Object, _mockLogger.Object);

            var result = (CreatedResult)(await controller.Create(customerId, createAddressDto));
            var createdAddress = (Address)result.Value;
            createdAddress.Country.Should().Be(createAddressDto.Country);
            createdAddress.County.Should().Be(createAddressDto.County);
            createdAddress.Line1.Should().Be(createAddressDto.Line1);
            createdAddress.Line2.Should().Be(createAddressDto.Line2);
            createdAddress.Town.Should().Be(createAddressDto.Town);
            createdAddress.Postcode.Should().Be(createAddressDto.Postcode);
            
            result.StatusCode.Should().Be(201);
            result.Location.As<string>().Should().Be($"/addresses/{createdAddress.Id}");
        }

        [Test]
        [AutoDataNoRecursion]
        public async Task Create_Returns_400_if_customer_doesnt_exist(
            CreateAddressDTO createAddressDto,
            Mock<ICustomerService> mockCustomerService,
            Mock<ICustomerRepository> mockCustomerRepo
        )
        {
            var mockAddressRepo = new Mock<IAddressRepository>();

            mockCustomerRepo.Setup(x => x.DoesCustomerExist(It.IsAny<Guid>())).Returns(Task.FromResult(false));


            var customerId = Guid.NewGuid();
            var controller = new AddressController(mockAddressRepo.Object, mockCustomerRepo.Object, mockCustomerService.Object, _mockLogger.Object);

            var result = (BadRequestObjectResult)(await controller.Create(customerId, createAddressDto));
           

            result.StatusCode.Should().Be(400);
            result.Value.Should().Be("Customerid invalid");
        }


        [Test]
        [AutoDataNoRecursion]
        public async Task Delete_returns_400_if_address_is_primary_address(
            CreateAddressDTO createAddressDto,
            Mock<ICustomerService> mockCustomerService,
            Mock<ICustomerRepository> mockCustomerRepo
        )
        {
            var mockAddressRepo = new Mock<IAddressRepository>();
            var addressId = Guid.NewGuid();

            mockCustomerService.Setup(x => x.IsPrimaryAddress(It.IsAny<Guid>())).Returns(Task.FromResult(true));

            var controller = new AddressController(mockAddressRepo.Object, mockCustomerRepo.Object, mockCustomerService.Object, _mockLogger.Object);


            var result = (BadRequestObjectResult)(await controller.Delete(addressId));

            result.StatusCode.Should().Be(400);
            result.Value.Should().Be("You cannot delete a primary address. Please add and/or make primay another address and request to delete this address again");
        }



        [Test]
        [AutoDataNoRecursion]
        public async Task Delete_initiates_delete_process_and_returns_200_if_address_is_not_the_primary_address(
            CreateAddressDTO createAddressDto,
            Mock<ICustomerService> mockCustomerService,
            Mock<ICustomerRepository> mockCustomerRepo
        )
        {
            var mockAddressRepo = new Mock<IAddressRepository>();
            var addressId = Guid.NewGuid();

            mockCustomerService.Setup(x => x.IsPrimaryAddress(It.IsAny<Guid>())).Returns(Task.FromResult(false));

            var controller = new AddressController(mockAddressRepo.Object, mockCustomerRepo.Object, mockCustomerService.Object, _mockLogger.Object);


            var result = (OkResult)(await controller.Delete(addressId));

            result.StatusCode.Should().Be(200);
            mockAddressRepo.Verify(x => x.Delete(addressId), Times.Once);
        }
    }
}