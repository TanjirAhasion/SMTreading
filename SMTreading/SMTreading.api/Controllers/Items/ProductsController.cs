using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SMT.Application.DTO.Items;
using SMT.Application.Interfaces.Items;

namespace SMTreading.api.Controllers.Items
{
    [ApiController]
    //[Authorize(Policy = "TenantOnly")]
    [Route("api/[controller]")]
    public class ProductsController(IProductService service) : ControllerBase
    {
        
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await service.GetAllAsync());
        }

        [HttpGet("{id:long}")]
        public async Task<IActionResult> Get(long id)
        {
            var item = await service.GetByIdAsync(id);
            return item is null ? NotFound() : Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateProductDto dto) => Ok(await service.CreateAsync(dto));

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateProductDto dto)
        {
            var item = await service.UpdateAsync(id, dto);
            return item is null ? NotFound() : Ok(item);
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(long id) => (await service.DeleteAsync(id)) ? Ok() : NotFound();
    }
}
