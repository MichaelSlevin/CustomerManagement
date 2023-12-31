using CustomerManagement.Entities;
using CustomerManagement.Repositories;
using CustomerManagement.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CustomerManagement.Controllers
{
    [ApiController]
    [Route("customers")]
    public class CustomerController : ControllerBase
    {

        private readonly ILogger<CustomerController> _logger;
        private readonly ICustomerService _customerService;
        private readonly ICustomerRepository _customerRepository;

        public CustomerController(ICustomerService customerService, ICustomerRepository customerRepository, ILogger<CustomerController> logger)
        {
            _customerService = customerService;
            _customerRepository = customerRepository;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var customer = await _customerService.GetCustomerById(id);
            if(customer is null)
            {
                return NotFound();
            }
            return Ok(customer);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCustomers()
        {
            return Ok(await _customerRepository.GetCustomers());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody]CreateCustomerDTO customer)
        {
            try
            {
                var createdCustomer = await _customerService.CreateCustomer(customer);
                return Created($"/customers/{createdCustomer.Id}", createdCustomer);
            } 
            catch (DbUpdateException dbUpdateException)
            {
                return BadRequest("A customer with the email address provided already exists");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _customerService.DeleteCustomer(id);
            return Ok();
        }

        [HttpPatch("{customerId}/primary-address/{addressId}")]
        public async Task<IActionResult> SetPrimaryAddress(Guid customerId, Guid addressId)
        {
            await _customerService.UpdatePrimaryAddressIfBelongsToCustomer(customerId, addressId);
            return Ok();
        }

        [HttpPatch("{customerId:Guid}/active/{active:bool}")]
        public async Task<IActionResult> SetActiveStatus(Guid customerId, bool active)
        {
            await _customerRepository.SetActiveStatus(customerId, active);
            return Ok();
        }


        [HttpGet("active")]
        public async Task<IActionResult> GetActiveCustomers()
        {
            return Ok(await _customerRepository.GetActiveCustomers());
        }
    }
}