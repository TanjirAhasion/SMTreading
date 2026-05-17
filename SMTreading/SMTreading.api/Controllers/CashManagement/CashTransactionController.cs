using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SMT.Application.DTO.CashManagement;
using SMT.Application.DTO.Items;
using SMT.Application.Helper;
using SMT.Application.Interfaces;
using SMT.Application.Interfaces.CashManagement;
using SMT.Domain.Entities.CashManagement;

namespace SMTreading.api.Controllers.CashManagement
{
    [Route("api/[controller]")]
    [ApiController]
    public class CashTransactionController : ControllerBase
    {
        private readonly ICashTransactionService _service;

        public CashTransactionController(
            ICashTransactionService service)
        {
            _service = service;
        }

        // GET ALL CASH TRANSACTIONS
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _service.GetAllAsync();

            return Ok(data);
        }


        [HttpGet("GetAllBySearchWithPagination")]
        public async Task<ActionResult<PagedResult<CashTransactionDto>>> GetAllBySearchWithPagination(
     [FromQuery] int page = 1, [FromQuery] int pageSize = 10,
     [FromQuery] string? search = null, [FromQuery] int? status = null)
        {
            var result = await _service.GetPagedAsync(page, pageSize, search, status);
            return Ok(result);
        }
    }
}
