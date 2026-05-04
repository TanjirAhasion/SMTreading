using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SMT.Application.DTO.Items;
using SMT.Application.Helper;
using SMT.Application.Interfaces.Items;
using SMT.Domain.Enums;

namespace SMTreading.api.Controllers.Items
{
    [ApiController]
    //[Authorize(Policy = "TenantOnly")]
    [Route("api/[controller]")]
    public class BrandsController : ControllerBase
    {
        private readonly IBrandService _service;

        public BrandsController(IBrandService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var data = await _service.GetByIdAsync(id);
            if (data == null) return NotFound();

            return Ok(data);
        }

        [HttpPost]
        public async Task<IActionResult> Create(BrandDto dto)
        {
            var id = await _service.CreateAsync(dto);
            return Ok(id);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, BrandDto dto)
        {
            var ok = await _service.UpdateAsync(id, dto);

            if (!ok) return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _service.DeleteAsync(id);

            if (!ok) return NotFound();

            return NoContent();
        }
    }
}
