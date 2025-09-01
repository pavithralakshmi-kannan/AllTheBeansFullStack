namespace AllTheBeans.API.Models
{
    public class DailySelection
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }

        // Foreign key
        public int BeanId { get; set; }

        public Bean? Bean { get; set; }
    }
}