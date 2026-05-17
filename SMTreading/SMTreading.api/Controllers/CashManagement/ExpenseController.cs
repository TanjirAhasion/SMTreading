using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SMT.Application.DTO.CashManagement;
using SMT.Application.Interfaces.CashManagement;

namespace SMTreading.api.Controllers.CashManagement
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpenseController : ControllerBase
    {

        private readonly IExpenseService _service;

        public ExpenseController(IExpenseService service)
        {
            _service = service;
        }

        // CREATE EXPENSE
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ExpenseDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var id = await _service.CreateAsync(dto);

            return Ok(new
            {
                Success = true,
                Message = "Expense created successfully",
                Id = id
            });
        }

        // GET ALL EXPENSES
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _service.GetAllAsync();

            return Ok(data);
        }
    }
}
