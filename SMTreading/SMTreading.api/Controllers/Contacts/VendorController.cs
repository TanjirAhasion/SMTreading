using Microsoft.AspNetCore.Mvc;
using SMT.Application.DTO.Contacts;
using SMT.Application.Interfaces.Contacts;

namespace SMTreading.api.Controllers.Contacts
{
    [ApiController]
    [Route("api/vendors")]
    public class VendorController : ControllerBase
    {
        private readonly IVendorService _service;

        public VendorController(IVendorService service)
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
                return NotFound($"Vendor with ID {id} not found");

            return Ok(result);
        }

        // -------------------------------
        // CREATE
        // -------------------------------
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] VendorDto dto)
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
        public async Task<IActionResult> Update(long id, [FromBody] VendorDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updated = await _service.UpdateAsync(id, dto);

            if (!updated)
                return NotFound($"Vendor with ID {id} not found");

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
                return NotFound($"Vendor with ID {id} not found");

            return NoContent();
        }
    }
}
