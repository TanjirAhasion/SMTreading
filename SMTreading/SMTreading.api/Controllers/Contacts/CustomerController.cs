using Microsoft.AspNetCore.Mvc;
using SMT.Application.DTO.Contacts;
using SMT.Application.Interfaces.Contacts;

namespace SMTreading.api.Controllers.Contacts
{
    [ApiController]
    [Route("api/v1/customers")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _service;

        public CustomerController(ICustomerService service)
        {
            _service = service;
        }

        // -------------------------------
        // GET ALL
        // -------------------------------
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        // -------------------------------
        // GET BY ID
        // -------------------------------
        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetById(long id)
        {
            var result = await _service.GetByIdAsync(id);

            if (result == null)
                return NotFound($"Customer with ID {id} not found");

            return Ok(result);
        }

        // -------------------------------
        // CREATE
        // -------------------------------
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CustomerDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var id = await _service.CreateAsync(dto);

            return CreatedAtAction(nameof(GetById), new { id }, id);
        }

        // -------------------------------
        // UPDATE
        // -------------------------------
        [HttpPut("{id:long}")]
        public async Task<IActionResult> Update(long id, [FromBody] CustomerDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updated = await _service.UpdateAsync(id, dto);

            if (!updated)
                return NotFound($"Customer with ID {id} not found");

            return NoContent();
        }

        // -------------------------------
        // DELETE (Soft Delete)
        // -------------------------------
        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(long id)
        {
            var deleted = await _service.DeleteAsync(id);

            if (!deleted)
                return NotFound($"Customer with ID {id} not found");

            return NoContent();
        }
    }
}
