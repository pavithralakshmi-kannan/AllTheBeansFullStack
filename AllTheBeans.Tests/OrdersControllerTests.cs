using System.Threading.Tasks;
using AllTheBeans.API.Controllers;
using AllTheBeans.API.Data;
using AllTheBeans.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace AllTheBeans.Tests
{
    public class OrdersControllerTests
    {
        private ApplicationDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("OrdersTestDb")
                .Options;

            var context = new ApplicationDbContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            return context;
        }

        [Fact]
        public async Task PostOrder_ReturnsBadRequest_WhenOrderIsInvalid()
        {
            var context = GetDbContext();
            var controller = new OrdersController(context);

            var invalidOrder = new Order { CustomerName = "", Address = "", Quantity = 0 };

            var result = await controller.PostOrder(invalidOrder);

            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task PostOrder_CreatesOrderSuccessfully()
        {
            var context = GetDbContext();
            context.Beans.Add(new Bean { Id = 1, Name = "Test Bean", Cost = 4.5m });
            await context.SaveChangesAsync();

            var controller = new OrdersController(context);

            var order = new Order
            {
                BeanId = 1,
                CustomerName = "AAA",
                Address = "123 Street",
                Quantity = 2
            };

            var result = await controller.PostOrder(order);

            var createdAt = Assert.IsType<CreatedAtActionResult>(result.Result);
            var createdOrder = Assert.IsType<Order>(createdAt.Value);
            Assert.Equal("AAA", createdOrder.CustomerName);
        }
    }
}