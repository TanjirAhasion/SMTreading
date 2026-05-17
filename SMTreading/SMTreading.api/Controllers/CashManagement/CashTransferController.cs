using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SMT.Application.DTO.CashManagement;
using SMT.Application.DTO.Items;
using SMT.Application.Helper;
using SMT.Application.Interfaces.CashManagement;

namespace SMTreading.api.Controllers.CashManagement
{
    [Route("api/[controller]")]
    [ApiController]
    public class CashTransferController : ControllerBase
    {
        private readonly ICashTransferService _service;

        public CashTransferController(ICashTransferService service)
        {
            _service = service;
        }

        // CREATE TRANSFER
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CashTransferDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var id = await _service.CreateAsync(dto);

            return Ok(new
            {
                Success = true,
                Message = "Transfer completed successfully",
                Id = id
            });
        }

        // GET ALL TRANSFERS
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _service.GetAllAsync();

            return Ok(data);
        }

        [HttpGet("GetAllBySearchWithPagination")]
        public async Task<ActionResult<PagedResult<CashTransferDto>>> GetAllBySearchWithPagination(
  [FromQuery] int page = 1, [FromQuery] int pageSize = 10,
  [FromQuery] string? search = null)
        {
            var result = await _service.GetPagedAsync(page, pageSize, search);
            return Ok(result);
        }
    }
}
