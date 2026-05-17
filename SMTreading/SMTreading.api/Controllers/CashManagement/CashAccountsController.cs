using Microsoft.AspNetCore.Mvc;
using SMT.Application.DTO.CashManagement;
using SMT.Application.DTO.Items;
using SMT.Application.Interfaces.CashManagement;
using SMT.Application.Interfaces.Items;

namespace SMTreading.api.Controllers.CashManagement
{
    [ApiController]
    [Route("api/[controller]")]
    public class CashAccountsController : ControllerBase
    {
        private readonly ICashAccountService _service;

        public CashAccountsController(ICashAccountService service)
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
        public async Task<IActionResult> Create(CashAccountDto dto)
        {
            var id = await _service.CreateAsync(dto);
            return Ok(id);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, CashAccountDto dto)
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
