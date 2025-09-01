using System.Linq;
using System.Threading.Tasks;
using AllTheBeans.API.Data;
using AllTheBeans.API.Models;
using AllTheBeans.API.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace AllTheBeans.Tests
{
    public class BeanServiceTests
    {
        private ApplicationDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("BeanServiceTestDb")
                .Options;

            var context = new ApplicationDbContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            return context;
        }

        [Fact]
        public async Task CreateAsync_AddsBean()
        {
            var context = GetDbContext();
            var service = new BeanService(context);

            var bean = new Bean { Name = "New Bean", Cost = 3.5m };

            var created = await service.CreateAsync(bean);

            Assert.NotNull(created);
            Assert.Equal("New Bean", created.Name);
        }

        [Fact]
        public async Task SearchAsync_FindsBeanByName()
        {
            var context = GetDbContext();
            context.Beans.Add(new Bean { Name = "Match Bean", Cost = 2.5m });
            await context.SaveChangesAsync();

            var service = new BeanService(context);

            var (items, total) = await service.SearchAsync("Match", null, null, null, null);

            Assert.True(total > 0);
            Assert.Contains(items, b => b.Name.Contains("Match"));
        }
    }
}