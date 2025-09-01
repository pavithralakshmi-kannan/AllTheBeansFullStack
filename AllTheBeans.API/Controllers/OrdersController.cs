using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AllTheBeans.API.Data;
using AllTheBeans.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AllTheBeans.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public OrdersController(ApplicationDbContext db)
        {
            _db = db;
        }

        // GET: api/orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            var orders = await _db.Orders
                .Include(o => o.Bean) //Load bean details
                .AsNoTracking()
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            return Ok(orders);
        }

        // GET: api/orders/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var order = await _db.Orders
                .Include(o => o.Bean) //Load bean details
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null) return NotFound();
            return Ok(order);
        }

        // POST: api/orders
        [HttpPost]
        public async Task<ActionResult<Order>> PostOrder(Order order)
        {
            if (string.IsNullOrWhiteSpace(order.CustomerName) ||
                string.IsNullOrWhiteSpace(order.Address) ||
                order.Quantity <= 0)
            {
                return BadRequest("Invalid order.");
            }

            var beanExists = await _db.Beans.AnyAsync(b => b.Id == order.BeanId);
            if (!beanExists) return BadRequest("Invalid beanId.");

            // Only set the date
            order.OrderDate = DateTime.UtcNow;

            _db.Orders.Add(order);
            await _db.SaveChangesAsync();

            // Load bean after saving
            await _db.Entry(order).Reference(o => o.Bean).LoadAsync();

            return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order);
        }

        // PUT: api/orders/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> PutOrder(int id, Order update)
        {
            if (id != update.Id) return BadRequest("Mismatched order id.");
            if (update.Quantity <= 0) return BadRequest("Quantity must be > 0.");

            var exists = await _db.Orders.AnyAsync(o => o.Id == id);
            if (!exists) return NotFound();

            var beanExists = await _db.Beans.AnyAsync(b => b.Id == update.BeanId);
            if (!beanExists) return BadRequest("Invalid beanId.");

            _db.Entry(update).Property(o => o.OrderDate).IsModified = false;
            _db.Entry(update).State = EntityState.Modified;

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _db.Orders.AnyAsync(o => o.Id == id)) return NotFound();
                throw;
            }

            return NoContent();
        }

        // DELETE: api/orders/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _db.Orders.FindAsync(id);
            if (order == null) return NotFound();

            _db.Orders.Remove(order);
            await _db.SaveChangesAsync();

            return NoContent();
        }
    }
}