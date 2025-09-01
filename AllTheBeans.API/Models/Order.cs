using System.Text.Json.Serialization;
namespace AllTheBeans.API.Models
{
    public class Order
    {
        public int Id { get; set; }

        public int BeanId { get; set; }

		[JsonIgnore]
        public Bean? Bean { get; set; } 

        public int Quantity { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
    }
}