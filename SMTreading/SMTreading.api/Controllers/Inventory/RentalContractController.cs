using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SMT.Application.DTO.Inventory;
using SMT.Application.Helper;
using SMT.Application.Interfaces.Inventory;
using SMT.Application.Interfaces.Inventory.Rental;

namespace SMTreading.api.Controllers.Inventory
{
    [Route("api/[controller]")]
    [ApiController]
    public class RentalContractController : ControllerBase
    {
        private readonly IRentalContractService _service;

        public RentalContractController(IRentalContractService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateRentalContractRequest dto)
        {
            var result = await _service.CreateAsync(dto);
            return Ok(result);
        }

        [HttpGet("GetAllBySearchWithPagination")]
        public async Task<ActionResult<PagedResult<RentalContractDto>>> GetAllBySearchWithPagination(
[FromQuery] SearchRentalContractDto searchSaleDto)
        {
            var result = await _service.GetPagedAsync(searchSaleDto);
            return Ok(result);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> Get(long id)
        {
            var result = await _service.GetRentalContractByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

    }
}
