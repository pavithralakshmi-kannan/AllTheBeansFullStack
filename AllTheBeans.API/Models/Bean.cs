namespace AllTheBeans.API.Models
{
    public class Bean
    {
        public int Id { get; set; }

        // Required fields
        public string Name { get; set; } = string.Empty;
        public decimal Cost { get; set; }

        // Optional fields
        public string? Colour { get; set; }
        public string? Country { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
    }
}