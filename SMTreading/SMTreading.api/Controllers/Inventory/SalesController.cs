using Microsoft.AspNetCore.Mvc;
using SMT.Application.DTO.Inventory;
using SMT.Application.Helper;
using SMT.Application.Interfaces.Inventory;

namespace SMTreading.api.Controllers.Inventory
{
    [ApiController]
    [Route("api/[controller]")]
    public class SalesController : ControllerBase
    {
        private readonly ISaleService _service;

        public SalesController(ISaleService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateSaleRequest dto)
        {
            var result = await _service.CreateSaleAsync(dto);
            return Ok(result);
        }

        [HttpGet("GetAllBySearchWithPagination")]
        public async Task<ActionResult<PagedResult<SalesDto>>> GetAllBySearchWithPagination(
     [FromQuery] SearchSalesDto searchSaleDto)
        {
            var result = await _service.GetPagedAsync(searchSaleDto);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(long id)
        {
            var result = await _service.GetInvoiceByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }
    }
}
