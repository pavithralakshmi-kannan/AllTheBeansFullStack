using System.Linq;
using System.Threading.Tasks;
using AllTheBeans.API.Data;
using AllTheBeans.API.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace AllTheBeans.Tests
{
    public class SeedDataTests
    {
        private ApplicationDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("SeedDataTestDb")
                .Options;

            var context = new ApplicationDbContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            return context;
        }

        [Fact]
        public async Task DatabaseStartsEmpty()
        {
            var context = GetDbContext();

            var beans = await context.Beans.ToListAsync();

            Assert.Empty(beans);
        }

        [Fact]
        public async Task CanAddSeedBean()
        {
            var context = GetDbContext();

            context.Beans.Add(new Bean { Name = "Seed Bean", Cost = 1.5m });
            await context.SaveChangesAsync();

            Assert.Equal(1, context.Beans.Count());
        }
    }
}
