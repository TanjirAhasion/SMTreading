using Microsoft.AspNetCore.Mvc;
using SMT.Application.DTO.Inventory;
using SMT.Application.DTO.Items;
using SMT.Application.Helper;
using SMT.Application.Interfaces.Inventory;

namespace SMTreading.api.Controllers.Inventory
{
    [ApiController]
    [Route("api/[controller]")]
    public class PurchaseController : ControllerBase
    {
        private readonly IPurchaseService _service;

        public PurchaseController(IPurchaseService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreatePurchaseRequest dto)
        {
            var result = await _service.CreatePurchaseAsync(dto);
            return Ok(result);
        }

        //[HttpGet]
        //public async Task<IActionResult> GetAll()
        //{
        //    return Ok(await _service.GetAllAsync());
        //}


        [HttpGet("GetAllBySearchWithPagination")]
        public async Task<ActionResult<PagedResult<PurchaseDto>>> GetAllBySearchWithPagination(
        [FromQuery] SearchPurchaseDto searchPurchaseDto)
        {
            var result = await _service.GetPagedAsync(searchPurchaseDto);
            return Ok(result);
        }
        //[HttpGet("{id}")]
        //public async Task<IActionResult> Get(long id)
        //{
        //    var result = await _service.GetByIdAsync(id);
        //    if (result == null) return NotFound();
        //    return Ok(result);
        //}
    }
}
