using Microsoft.AspNetCore.Mvc;
using SMT.Application.DTO.Items;
using SMT.Application.Interfaces.Items;

namespace SMTreading.api.Controllers.Items
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductImagesController(IProductImageService service) : ControllerBase
    {
        // GET: api/productImages/serial/5
        [HttpGet("serial/{serialId:long}")]
        public async Task<IActionResult> GetBySerialId(long serialId)
        {
            // You'll need to add this method to your IProductImageService
            var items = await service.GetBySerialIdAsync(serialId);
            return Ok(items);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateProductImageDto dto)
        {
            // dto.File will be populated by the [FromForm] attribute
            var result = await service.CreateAsync(dto);
            return Ok(result);
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(long id)
            => (await service.DeleteAsync(id)) ? Ok() : NotFound();

        // Fixed Update Route to include ID in the URL
        [HttpPut("{id:long}")]
        public async Task<IActionResult> Update(long id, [FromForm] UpdateProductImageDto dto)
        {
            var item = await service.UpdateAsync(id, dto);
            return item is null ? NotFound() : Ok(item);
        }
    }
}
