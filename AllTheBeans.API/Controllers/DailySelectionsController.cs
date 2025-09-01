using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AllTheBeans.API.Data;
using AllTheBeans.API.Models;

namespace AllTheBeans.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DailySelectionsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DailySelectionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/dailyselections
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DailySelection>>> GetDailySelections()
        {
            return await _context.DailySelections
                                 .Include(d => d.Bean)
                                 .ToListAsync();
        }

        // GET: api/dailyselections/today
        [HttpGet("today")]
        public async Task<ActionResult<DailySelection>> GetTodaySelection()
        {
            var today = DateTime.UtcNow.Date;

            var selection = await _context.DailySelections
                                          .Include(d => d.Bean)
                                          .FirstOrDefaultAsync(d => d.Date == today);

            if (selection == null)
                return NotFound();

            return selection;
        }
    }
}