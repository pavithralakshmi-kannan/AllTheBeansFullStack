using System.Collections.Generic;
using System.Threading.Tasks;
using AllTheBeans.API.Controllers;
using AllTheBeans.API.Data;
using AllTheBeans.API.Models;
using AllTheBeans.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace AllTheBeans.Tests
{
    public class BeansControllerTests
    {
        private ApplicationDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "BeansTestDb")
                .Options;
            return new ApplicationDbContext(options);
        }

        [Fact]
        public async Task Get_ReturnsBeans_WhenBeansExist()
        {
            // Arrange
            var context = GetDbContext();
            context.Beans.Add(new Bean
            {
                Name = "Test Bean",
                Cost = 5.5m,
                Country = "Brazil",
                Colour = "Brown"
            });
            await context.SaveChangesAsync();

            var service = new BeanService(context);
            var controller = new BeansController(service);

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            // Act
            var result = await controller.Get(null, null, null, null, null);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var beans = Assert.IsAssignableFrom<IEnumerable<Bean>>(okResult.Value);
            Assert.NotEmpty(beans);
        }
    }
}