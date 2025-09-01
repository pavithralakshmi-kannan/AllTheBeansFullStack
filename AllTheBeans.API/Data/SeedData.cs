using System.Text.Json;
using AllTheBeans.API.Models;
using Microsoft.EntityFrameworkCore;

namespace AllTheBeans.API.Data
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider services, IHostEnvironment env)
        {
            using var context = new ApplicationDbContext(
                services.GetRequiredService<DbContextOptions<ApplicationDbContext>>());

            // Prevent reseeding if data already exists
            if (context.Beans.Any())
                return;

            var jsonPath = Path.Combine(env.ContentRootPath, "Data", "AllTheBeans.json");
            if (!File.Exists(jsonPath))
                return;

            var json = File.ReadAllText(jsonPath);
            var beans = JsonSerializer.Deserialize<List<JsonElement>>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (beans is null || beans.Count == 0)
                return;

            var beanEntities = new List<Bean>();

            foreach (var b in beans)
            {
                // Name
                string name = "";
                if (b.TryGetProperty("name", out var nameProp))
                    name = nameProp.GetString() ?? "";
                else if (b.TryGetProperty("Name", out var namePropPascal))
                    name = namePropPascal.GetString() ?? "";

                // Description
                string? description = null;
                if (b.TryGetProperty("description", out var desc))
                    description = desc.GetString();
                else if (b.TryGetProperty("Description", out var descPascal))
                    description = descPascal.GetString();

                // Country
                string? country = null;
                if (b.TryGetProperty("country", out var countryProp))
                    country = countryProp.GetString();
                else if (b.TryGetProperty("Country", out var countryPascal))
                    country = countryPascal.GetString();

                // Colour
                string? colour = null;
                if (b.TryGetProperty("colour", out var colourProp))
                    colour = colourProp.GetString();
                else if (b.TryGetProperty("Colour", out var colourPascal))
                    colour = colourPascal.GetString();

                // Image
                string? image = null;
                if (b.TryGetProperty("image", out var img))
                    image = img.GetString();
                else if (b.TryGetProperty("Image", out var imgPascal))
                    image = imgPascal.GetString();

                // Cost
                string costStr = "0";
                if (b.TryGetProperty("cost", out var costProp))
                    costStr = costProp.GetString() ?? "0";
                else if (b.TryGetProperty("Cost", out var costPascal))
                    costStr = costPascal.GetString() ?? "0";

                // Strip currency symbols and parse
                costStr = new string(costStr.Where(c => char.IsDigit(c) || c == '.' || c == '-').ToArray());
                decimal.TryParse(costStr, out var cost);

                // Build Bean
                beanEntities.Add(new Bean
                {
                    Name = name,
                    Description = description,
                    Country = country,
                    Colour = colour,
                    Image = image,
                    Cost = cost
                });
            }

            context.Beans.AddRange(beanEntities);
            context.SaveChanges();
        }
    }
}