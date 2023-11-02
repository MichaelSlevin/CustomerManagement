using CustomerManagement.Entities;
using CustomerManagement.Repositories;
using CustomerManagement.Services;
using Microsoft.AspNetCore.Mvc;

namespace CustomerManagement.Controllers
{
    [ApiController]
    [Route("addresses")]
    public class AddressController : ControllerBase
    {
     
        private readonly ILogger<AddressController> _logger;
        private readonly ICustomerRepository _customerRepo;
        private readonly IAddressRepository _addressRepository;
        private readonly ICustomerService _customerService;

        public AddressController(IAddressRepository addressRepository, ICustomerRepository customerRepository, ICustomerService customerService, ILogger<AddressController> logger)
        {
            _addressRepository = addressRepository;
            _customerRepo = customerRepository;
            _customerService = customerService;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var address = _addressRepository.GetAddressById(id);
            if(address == null)
            {
                return NotFound("Address with the provided Id cannot be found");
            }
            return Ok(address);
        }

        [HttpPost]
        [Route("customer/{customerId}/address")]
        public async Task<IActionResult> Create(Guid customerId, [FromBody] CreateAddressDTO createAddressDto)
        {
            if( await _customerRepo.DoesCustomerExist(customerId))
            {
                var address = new Address(customerId, createAddressDto);
                await _addressRepository.CreateAddress(address);
                return Created($"/addresses/{address.Id}", address);
            };
            return BadRequest("Customerid invalid");

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            if(await _customerService.IsPrimaryAddress(id))
            {
                return BadRequest("You cannot delete a primary address. Please add and/or make primay another address and request to delete this address again");
            }
            await _addressRepository.Delete(id);
            return Ok();
        }
    }
}