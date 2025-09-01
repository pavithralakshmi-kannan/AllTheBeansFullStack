using Microsoft.AspNetCore.Mvc;
using AllTheBeans.API.Services;
using AllTheBeans.API.Models;

namespace AllTheBeans.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BeansController : ControllerBase
    {
        private readonly IBeanService _svc;
        public BeansController(IBeanService svc) => _svc = svc;

        // GET /api/beans?q=&country=&colour=&minCost=&maxCost=&page=&pageSize=&sort=
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string? q, [FromQuery] string? country,
                                             [FromQuery] string? colour, [FromQuery] decimal? minCost,
                                             [FromQuery] decimal? maxCost, [FromQuery] int page = 1,
                                             [FromQuery] int pageSize = 20, [FromQuery] string? sort = null)
        {
            var (items, total) = await _svc.SearchAsync(q, country, colour, minCost, maxCost, page, pageSize, sort);
            Response.Headers["X-Total-Count"] = total.ToString();
            return Ok(items);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var bean = await _svc.GetByIdAsync(id);
            return bean is null ? NotFound() : Ok(bean);
        }

        [HttpGet("bean-of-the-day")]
        public async Task<IActionResult> GetBeanOfTheDay()
        {
            var botd = await _svc.GetBeanOfTheDayAsync();
            return botd is null ? NotFound() : Ok(botd);
        }

        //CRUD
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Bean input)
        {
            if (input is null) return BadRequest();
            var created = await _svc.CreateAsync(input);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] Bean input)
        {
            if (input is null) return BadRequest();
            var updated = await _svc.UpdateAsync(id, input);
            return updated is null ? NotFound() : Ok(updated);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _svc.DeleteAsync(id);
            return ok ? NoContent() : NotFound();
        }
    }
}