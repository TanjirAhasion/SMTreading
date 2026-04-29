using Microsoft.AspNetCore.Mvc;
using SMT.Application.DTO.Items;
using SMT.Application.Interfaces.Items;
using SMT.Domain.Enums;

namespace SMTreading.api.Controllers.Items
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductSerialsController(IProductSerialService service) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await service.GetAllAsync());

        [HttpGet("{id:long}")]
        public async Task<IActionResult> Get(long id)
        {
            var item = await service.GetByIdAsync(id);
            return item is null ? NotFound() : Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateProductSerialDto dto) {
            var id = await service.CreateAsync(dto);
            return Ok(id);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(long id, UpdateProductSerialDto dto)
        {
            var item = await service.UpdateAsync(id, dto);
            return item is null ? NotFound() : Ok(item);
        }
        
        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(long id) => (await service.DeleteAsync(id)) ? Ok() : NotFound();

        [HttpGet("productSerialStatuses")]
        public IActionResult GetStatuses()
        {
            var data = Enum.GetValues(typeof(ProductSerialStatus))
                .Cast<ProductSerialStatus>()
                .Select(x => new {
                    id = (int)x,
                    name = x.ToString()
                });

            return Ok(data);
        }
    }
}
